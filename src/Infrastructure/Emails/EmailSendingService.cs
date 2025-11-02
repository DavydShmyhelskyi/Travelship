using Application.Common.Interfaces;

namespace Infrastructure.Emails;

/// <summary>
/// Email sending service, only for lecture purposes
/// </summary>
public class EmailSendingService : IEmailSendingService
{
    public Task SendEmailAsync(string emailTo, string emailBody)
    {
        return Task.CompletedTask;
    }
}