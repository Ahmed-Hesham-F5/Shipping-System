using ShippingSystem.Results;

namespace ShippingSystem.Interfaces
{
    public interface IEmailService
    {
        Task<OperationResult> SendAsync(string toEmail, string subject, Func<string> messageBody);
        Func<string> EmailConfirmationBody(string confirmEmailUrl, string userEmail, string token);
        Func<string> RequestResetPasswordBody(string resetPasswordUrl, string userEmail, string token);
    }
}
