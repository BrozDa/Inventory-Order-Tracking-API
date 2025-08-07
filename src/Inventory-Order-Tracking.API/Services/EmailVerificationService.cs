using FluentEmail.Core;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    /// <summary>
    /// Provides operations related to email verification..
    /// </summary>
    public class EmailVerificationService(
        IFluentEmail emailService,
        IEmailVerificationTokenRepository repository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<EmailVerificationService> logger,
        LinkGenerator linkGenerator
        ) : IEmailVerificationService
    {
        /// <inheritdoc/>
        public async Task<ServiceResult<object>> SendVerificationEmailAsync(User user)
        {
            try
            {
                var token = await GenerateAndStoreToken(user.Id);
                if (token is null)  //logging is in GenerateAndStoreToken
                    return ServiceResult<object>.InternalServerError("Could not store email verification token.");

                var link = GenerateVerificationLink(token);
                if (link is null) //logging is in GenerateVerificationLink
                    return ServiceResult<object>.InternalServerError("Failed to generate verification link");

                var sendResult = await emailService
                    .To(user.Email)
                    .Subject("Verification - Inventory Management dashboard")
                    .Body($"Click on link to verify your email <a href='{link}'>Verification Link</a>", isHtml: true)
                    .SendAsync();

                if (!sendResult.Successful)
                {
                    logger.LogError($"[EmailVerificationService][SendVerificationEmailAsync] Failed to send email: {string.Join(";", sendResult.ErrorMessages)}");
                    return ServiceResult<object>.InternalServerError("Failed to sent verification email");
                }

                return ServiceResult<object>.Ok();
            }
            catch (ApplicationException)
            {
                //thrown by GenerateAndStoreToken - already logged
                return ServiceResult<object>.InternalServerError("Failed to generate verification token");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EmailVerificationService][SendVerificationEmailAsync] Unhandled error occurred");
                return ServiceResult<object>.InternalServerError("Unhandled error occurred");
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<object>> VerifyEmailAsync(Guid tokenId)
        {
            try
            {
                var storedToken = await repository.GetByIdAsync(tokenId);

                if (storedToken is null)
                {
                    logger.LogWarning($"[EmailVerificationService][VerifyEmailAsync] Invalid token verification attempt");
                    return ServiceResult<object>.Unauthorized("Invalid Token received");
                }

                if (storedToken.ExpiresOn < DateTime.UtcNow)
                {
                    logger.LogWarning("[EmailVerificationService][VerifyEmailAsync] Expired token verification attempt by {UserId}", storedToken.User.Id);
                    return ServiceResult<object>.Unauthorized("Verification link expired");
                }

                if (storedToken.User.IsVerified)
                {
                    logger.LogWarning("[EmailVerificationService][VerifyEmailAsync] Already verified user attempt by {UserId}", storedToken.User.Id);
                    return ServiceResult<object>.Unauthorized("User already verified");
                }

                storedToken.User.IsVerified = true;
                await repository.RemoveAsync(storedToken);
                await repository.SaveChangesAsync();

                return ServiceResult<object>.Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EmailVerificationService][VerifyEmailAsync] Unhandled error occurred");
                return ServiceResult<object>.InternalServerError("Unhandled error occurred");
            }
        }

        /// <summary>
        /// Generates email verification link for newly registered user
        /// </summary>
        /// <param name="token">An <see cref="EmailVerificationToken"/> associated with new user</param>
        /// <returns>An string representation of a verification URI for the user in case of success, false otherwise</returns>
        private string? GenerateVerificationLink(EmailVerificationToken token)
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                logger.LogError("HttpContext null when trying to generate email verification link");
                return $"https://localhost:7296/api/auth/user/verify/{token.Id}";
            }

            var uri = linkGenerator.GetUriByAction(
                httpContext: httpContext,
                action: "Verify",
                controller: "Auth",
                values: new { tokenId = token.Id }
                );

            if (uri is null)
                logger.LogError("Failed to generate verification link");

            return uri;
        }
        /// <summary>
        /// Generates and stores verification token for newly registered user
        /// </summary>
        /// <param name="userId">An Id in form of <see cref="Guid"/> of newly registered user</param>
        /// <returns>An instance of <see cref="EmailVerificationToken"/> on success, null otherwise</returns>
        private async Task<EmailVerificationToken?> GenerateAndStoreToken(Guid userId)
        {
            try
            {
                var token = await repository.AddTokenAsync(new EmailVerificationToken()
                {
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddDays(1)
                });

                return token;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error when storing verification token for user {UserId}", userId);
                return null;
            }
        }
    }
}