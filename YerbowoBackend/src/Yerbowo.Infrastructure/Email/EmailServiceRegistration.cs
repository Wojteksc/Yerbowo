namespace Yerbowo.Infrastructure.Email;

[ExcludeFromCodeCoverage]
public static class EmailServiceRegistration
{
    public static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));

        var provider = configuration["EmailSettings:Provider"];

        if (provider == "SendGrid")
        {
            services.AddScoped<IEmailClient, SendGridEmailClient>();
        }
        else if (provider == "Mailgun")
        {
            services.AddScoped<IEmailClient, MailgunEmailClient>();
        }
        else
        {
            throw new InvalidOperationException("Unknown email provider configured");
        }

        services.AddScoped<IEmailService<RegistrationConfirmationTemplate>, EmailService<RegistrationConfirmationTemplate>>();
        services.AddScoped<IEmailService<NewsletterInvitationTemplate>, EmailService<NewsletterInvitationTemplate>>();
        services.AddScoped<IEmailService<NewsletterCouponTemplate>, EmailService<NewsletterCouponTemplate>>();
    }
}