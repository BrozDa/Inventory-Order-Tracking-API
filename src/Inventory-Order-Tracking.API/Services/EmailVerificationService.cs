using FluentEmail.Core;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    public class EmailVerificationService(
        IFluentEmail emailService,
        IEmailVerificationTokenRepository repository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<EmailVerificationService> logger,
        LinkGenerator linkGenerator
        ) : IEmailVerificationService
    {
        public async Task<ServiceResult<object>> SendVerificationEmail(User user)
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
                    logger.LogError($"[VerificationEmailSending] Failed to send email: {string.Join(";", sendResult.ErrorMessages)}");
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
                logger.LogError(ex, "[UnhandledError] Unhandled error occurred");
                return ServiceResult<object>.InternalServerError("Unhandled error occurred");
            }
        }

        public async Task<ServiceResult<object>> VerifyEmail(Guid tokenId)
        {
            try
            {
                var storedToken = await repository.GetById(tokenId);

                if (storedToken is null)
                {
                    logger.LogWarning($"[EmailVerification] Invalid token verification attempt");
                    return ServiceResult<object>.Unauthorized("Invalid Token received");
                }

                if (storedToken.ExpiresOn < DateTime.UtcNow)
                {
                    logger.LogWarning("[EmailVerification] Expired token verification attempt by {UserId}", storedToken.User.Id);
                    return ServiceResult<object>.Unauthorized("Verification link expired");
                }

                if (storedToken.User.IsVerified)
                {
                    logger.LogWarning("[EmailVerification] Already verified user attempt by {UserId}", storedToken.User.Id);
                    return ServiceResult<object>.Unauthorized("User already verified");
                }

                storedToken.User.IsVerified = true;
                await repository.RemoveAsync(storedToken);
                await repository.SaveChangesAsync();

                return ServiceResult<object>.Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[UnhandledError] Unhandled error occurred");
                return ServiceResult<object>.InternalServerError("Unhandled error occurred");
            }
        }

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

        private async Task<EmailVerificationToken?> GenerateAndStoreToken(Guid userId)
        {
            try
            {
                var token = await repository.AddToken(new EmailVerificationToken()
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