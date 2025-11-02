namespace Application.Common.Interfaces;

public interface IEmailSendingService
{
    Task SendEmailAsync(string emailTo, string emailBody);
}