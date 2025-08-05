using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Inventory_Order_Tracking.API.Services
{
    public class AuthService(
        IUserRepository repository, 
        IEmailVerificationService emailService,
        IAuditLogService auditService,
        ILogger<AuthService> logger, 
        JwtSettings jwtSettings)
        : IAuthService
    {
        //NOTE: jwtSettings is validated on startup in program.cs - right after build
        public async Task<ServiceResult<string>> RegisterAsync(UserRegistrationDto request)
        {
            try
            {
                if (await repository.UsernameExistsAsync(request.Username))
                {
                    logger.LogWarning("[Registration][UsernameExistsAsync] Duplicate username {Username}", request.Username);
                    return ServiceResult<string>.BadRequest("User Already Exists");
                }

                var (hash, salt) = PasswordHasher.GenerateHashAndSalt(request.Password);

                var user = await repository.AddAsync(new User
                {
                    Username = request.Username,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Email = request.Email,
                    Role = UserRoles.Admin,
                    IsVerified = false,
                });

                await emailService.SendVerificationEmailAsync(user);

                await auditService.AddNewLogAsync(new AuditLogAddDto { UserId = user.Id, Action = "Registered new user" });

                return ServiceResult<string>.Ok("Registration successful. Please verify your email to activate your account.");
            }
            catch (ArgumentNullException)
            {
                return ServiceResult<string>.BadRequest("Password cannot be empty");
            }
            catch (DbUpdateException dbEx)
            {
                logger.LogError(dbEx,
                    "[Registration][DbUpdateException] Database error during processing request for {Username}", request.Username);
                return ServiceResult<string>.InternalServerError("A database error occured during processing the request");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[Registration][UnhandledException] Unexpected error during processing request for {Username}", request.Username);
                return ServiceResult<string>.InternalServerError("An Unexpected error occured during processing the request");
            }
        }

        public async Task<ServiceResult<TokenResponseDto>> LoginAsync(UserLoginDto request)
        {
            try
            {
                var user = await repository.GetByUsernameAsync(request.Username);

                if (user is null)
                {
                    logger.LogWarning("[LoginAsync][AuthenticationFailed] Invalid username or password: {Username}", request.Username);
                    return ServiceResult<TokenResponseDto>.BadRequest("Invalid username or password");
                }
                if (!PasswordHasher.VerifyPassword(user.PasswordHash, request.Password, user.PasswordSalt))
                {
                    logger.LogWarning("[LoginAsync][AuthenticationFailed] Invalid username or password: {Username}", request.Username);
                    return ServiceResult<TokenResponseDto>.BadRequest("Invalid username or password");
                }
                if (!user.IsVerified)
                {
                    logger.LogWarning("[LoginAsync][Unverified] Unverified user login: {Username}", request.Username);
                    return ServiceResult<TokenResponseDto>.BadRequest("User not verified");
                }

                TokenResponseDto tokenResponse = await GenerateTokenResponse(user);

                return ServiceResult<TokenResponseDto>.Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[LoginAsync][Exception] Unexpected error during processing request for {Username}", request.Username);
                return ServiceResult<TokenResponseDto>.InternalServerError("An Unexpected error occured during processing the request");
            }
        }

        public async Task<ServiceResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.ExpiredRefreshToken);

            if (user is null)
            {
                return ServiceResult<TokenResponseDto>.Unauthorized();
            }

            var tokenResponse = await GenerateTokenResponse(user);

            return ServiceResult<TokenResponseDto>.Ok(tokenResponse);
        }

        private string CreateToken(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Role))
                throw new ArgumentException("User must have valid username and role.");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Token));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtSettings.TokenExpirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken()
        {
            var refreshToken = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(refreshToken);

            return Convert.ToBase64String(refreshToken);
        }

        private async Task<string> GenerateAndStoreRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;

            var refreshTokenExpirationDays = jwtSettings.TokenExpirationDays;

            user.RefreshTokenExpirationTime = DateTime
                .UtcNow
                .AddDays(jwtSettings.RefreshTokenExpirationDays);

            await repository.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<TokenResponseDto> GenerateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndStoreRefreshToken(user)
            };
        }

        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null
                || user.RefreshToken != refreshToken
                || user.RefreshTokenExpirationTime < DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }
    }
}