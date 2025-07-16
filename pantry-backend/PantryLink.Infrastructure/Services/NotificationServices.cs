using SendGrid;
using SendGrid.Helpers.Mail;
using PantryLink.Core.Interfaces;

namespace PantryLink.Infrastructure.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailNotificationService(ISendGridClient sendGridClient, string fromEmail = "noreply@pantrylink.com", string fromName = "PantryLink")
    {
        _sendGridClient = sendGridClient;
        _fromEmail = fromEmail;
        _fromName = fromName;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(recipient);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendAsync(string recipient, string message)
    {
        await SendAsync(recipient, "PantryLink Notification", message);
    }

    public async Task SendPasswordResetAsync(string email, string resetToken)
    {
        var subject = "Password Reset - PantryLink";
        var message = $@"
            <h2>Password Reset Request</h2>
            <p>You have requested to reset your password. Please use the following token:</p>
            <p><strong>{resetToken}</strong></p>
            <p>If you did not request this reset, please ignore this email.</p>
            <p>Best regards,<br>PantryLink Team</p>
        ";

        await SendAsync(email, subject, message);
    }

    public async Task SendWelcomeEmailAsync(string email, string firstName)
    {
        var subject = "Welcome to PantryLink!";
        var message = $@"
            <h2>Welcome to PantryLink, {firstName}!</h2>
            <p>Thank you for joining our community food pantry network.</p>
            <p>You can now:</p>
            <ul>
                <li>Search for food pantries in your area</li>
                <li>Set dietary preferences for personalized recommendations</li>
                <li>Get real-time inventory updates</li>
            </ul>
            <p>Best regards,<br>PantryLink Team</p>
        ";

        await SendAsync(email, subject, message);
    }
}

public class SmsNotificationService : ISmsNotificationService
{
    // This would integrate with Twilio or another SMS service
    // For now, we'll implement a stub
    public async Task SendAsync(string recipient, string subject, string message)
    {
        await SendAsync(recipient, message);
    }

    public async Task SendAsync(string recipient, string message)
    {
        // TODO: Implement actual SMS sending logic with Twilio
        await Task.Delay(100); // Simulate async operation
        Console.WriteLine($"SMS to {recipient}: {message}");
    }

    public async Task SendVerificationCodeAsync(string phoneNumber, string code)
    {
        var message = $"Your PantryLink verification code is: {code}";
        await SendAsync(phoneNumber, message);
    }
}
