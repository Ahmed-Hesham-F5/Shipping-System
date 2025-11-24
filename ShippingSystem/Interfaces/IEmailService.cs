using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IEmailService
    {
        Task<OperationResult> SendAsync(string toEmail, string subject, Func<string> messageBody);
        Func<string> EmailConfirmationBody(string confirmEmailUrl, string email, string token);
        Func<string> ChangeEmailConfirmationBody(string confirmNewEmailUrl, string newEmail, string oldEmail, string token);
        Func<string> RequestResetPasswordBody(string resetPasswordUrl, string email, string token);
    }
}
