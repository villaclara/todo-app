using Microsoft.AspNetCore.Identity.UI.Services;

namespace TodoListApp.WebApp.Services.EmailSender;

public class StubEmailSender : IEmailSender
{
    private readonly ILogger<StubEmailSender> logger;

    public StubEmailSender(ILogger<StubEmailSender> logger)
    {
        this.logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // TODO - Maybe check and add real Email Sending.
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
