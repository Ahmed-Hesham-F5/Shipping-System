using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ShippingSystem.Interfaces;
using ShippingSystem.Results;
using ShippingSystem.Settings;

namespace ShippingSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<OperationResult> SendAsync(string toEmail, string subject, Func<string> messageBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = messageBody()
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Pass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch
            {
                return OperationResult.Fail(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }

            return OperationResult.Ok();
        }

        public Func<string> EmailConfirmationBody(string confirmEmailUrl, string userEmail, string token)
        {
            var confirmationLink = $"{confirmEmailUrl}?email={userEmail}&token={token}";

            string htmlBody = $@"
                <p>Dear Shipper,</p>

                <p>Thank you for registering with <strong>StakeExpress</strong>!</p>

                <p>To complete your registration, please confirm your email by clicking the link below:</p>

                <p><a href='{confirmationLink}'>Confirm My Email</a></p>

                <p>If the link does not work, you can copy and paste the following URL into your browser:</p>

                <p>{confirmationLink}</p>


                <p>This link is valid for the next 6 hours. Please make sure to confirm your email before it expires.</p>

                <p>Thank you,<br/>
                The StakeExpress Team</p>
                ";

            return () => htmlBody;
        }

        public Func<string> RequestResetPasswordBody(string resetPasswordUrl, string userEmail, string token)
        {
            var resetPasswordLink = $"{resetPasswordUrl}/reset-password?email={userEmail}&token={token}";

            var htmlBody = $@"
                <p>Dear User,</p>
                <p>We received a request to reset your password. Please click the link below to set a new password:</p>
                <p><a href='{resetPasswordLink}'>Reset My Password</a></p>
                <p>If the link does not work, you can copy and paste the following URL into your browser:</p>
                <p>{resetPasswordLink}</p>
                <p>This link is valid for the next 6 hours. Please make sure to reset your password before it expires.</p>
                <p>Thank you,<br/>
                The StakeExpress Team</p>
                ";

            return () => htmlBody;
        }
    }
}
