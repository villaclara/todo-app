using Microsoft.AspNetCore.Identity.UI.Services;

namespace TodoListApp.WebApp.Services.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        this.logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Stub to check if it working as intended.
        // Should be replaced by actual SMPT stuff for prod.
        this.logger.LogInformation("---------------------------------------------------");
        this.logger.LogInformation("[StubEmailSender] To: {Email}", email);
        this.logger.LogInformation("Subject: {Subject}", subject);

        this.logger.LogInformation("Body: {Body}", htmlMessage);

        this.logger.LogInformation("---------------------------------------------------");

        return Task.CompletedTask;
    }
}
