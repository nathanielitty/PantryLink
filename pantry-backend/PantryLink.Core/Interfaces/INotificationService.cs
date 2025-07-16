namespace PantryLink.Core.Interfaces;

public interface INotificationService
{
    Task SendAsync(string recipient, string subject, string message);
    Task SendAsync(string recipient, string message); // For SMS
}

public interface IEmailNotificationService : INotificationService
{
    Task SendPasswordResetAsync(string email, string resetToken);
    Task SendWelcomeEmailAsync(string email, string firstName);
}

public interface ISmsNotificationService : INotificationService
{
    Task SendVerificationCodeAsync(string phoneNumber, string code);
}
