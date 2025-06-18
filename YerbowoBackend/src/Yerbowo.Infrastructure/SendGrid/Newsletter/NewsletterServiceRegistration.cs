namespace Yerbowo.Infrastructure.SendGrid.Newsletter;

public static class NewsletterServiceRegistration
{
    public static void AddNewsletterServices(this IServiceCollection services)
    {
        services.AddScoped<IRegistrationConfirmationEmailSender, RegistrationConfirmationEmailSender>();
        services.AddScoped<INewsletterInvitationEmailSender, NewsletterInvitationEmailSender>();
        services.AddScoped<INewsletterEmailSender, NewsletterEmailSender>();
    }
}