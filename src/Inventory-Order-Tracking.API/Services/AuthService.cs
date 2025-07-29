using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory_Order_Tracking.API.Services
{
    public class AuthService(IUserRepository repository, IConfiguration configuration, ILogger<IAuthService> logger)
        : IAuthService
    {
        public async Task<AuthServiceResult<string>> RegisterAsync(UserRegistrationDto request)
        {
            try
            {
                if (await repository.UsernameExistsAsync(request.Username))
                {
                    logger.LogWarning("[Registration][UsernameExistsAsync] Duplicate username {Username}", request.Username);
                    return AuthServiceResult<string>.BadRequest("User Already Exists");
                }

                var (hash, salt) = PasswordHasher.GenerateHashAndSalt(request.Password);

                await repository.AddAsync(new User
                {
                    Username = request.Username,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Email = request.Email,
                    Role = UserRoles.Admin,
                    IsVerified = false,
                });

                return AuthServiceResult<string>.Ok("Registration successful. Please verify your email to activate your account.");
            }
            catch (ArgumentNullException ArgNullEx)
            {
                // should not ever happen - validation is prior calling this 
                logger.LogWarning(ArgNullEx, "[Registration][ArgumentNullException] Empty password for {Username}", request.Username);
                return AuthServiceResult<string>.BadRequest("Password cannot be empty");
            }
            catch (DbUpdateException dbEx)
            {
                logger.LogError(dbEx, 
                    "[Registration][DbUpdateException] Database error during processing request for {Username}", request.Username);
                return AuthServiceResult<string>.InternalServerError("A database error occured during processing the request");
            }
            catch (Exception ex) 
            {
                logger.LogError(ex,
                    "[Registration][UnhandledException] Unexpected error during processing request for {Username}", request.Username);
                return AuthServiceResult<string>.InternalServerError("An Unexpected error occured during processing the request");
            }
            
        }
        public async Task<AuthServiceResult<string>> LoginAsync(UserLoginDto request)
        {
            try
            {
                var user = await repository.GetByUsernameAsync(request.Username);

                if (user is null)
                {
                    logger.LogWarning("[LoginAsync][AuthenticationFailed] Invalid username or password: {Username}", request.Username);
                    return AuthServiceResult<string>.BadRequest("Invalid username or password");
                }
                if(!PasswordHasher.VerifyPassword(user.PasswordHash, request.Password, user.PasswordSalt))
                { 
                    logger.LogWarning("[LoginAsync][AuthenticationFailed] Invalid username or password: {Username}", request.Username);
                    return AuthServiceResult<string>.BadRequest("Invalid username or password");
                }
                    

                if (!user.IsVerified)
                {
                    logger.LogWarning("[LoginAsync][Unverified] Unverified user login: {Username}", request.Username);
                    return AuthServiceResult<string>.BadRequest("User not verified");
                }

                string token = CreateToken(user);

                return AuthServiceResult<string>.Ok(token);
            }
            catch (ArgumentNullException ArgNullEx)
            {
                // should not ever happen - validation is prior calling this 
                logger.LogWarning(ArgNullEx, "[Registration][ArgumentNullException] Empty password for {Username}", request.Username);
                return AuthServiceResult<string>.BadRequest("Password cannot be empty");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[LoginAsync][Exception] Unexpected error during processing request for {Username}", request.Username);
                return AuthServiceResult<string>.InternalServerError("An Unexpected error occured during processing the request");
            }

        }

        
        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
