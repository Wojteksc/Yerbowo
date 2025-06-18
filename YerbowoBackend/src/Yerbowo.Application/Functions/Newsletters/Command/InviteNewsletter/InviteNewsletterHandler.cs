namespace Yerbowo.Application.Functions.Newsletters.Command.InviteNewsletter;

public class InviteNewsletterHandler(
    INewsletterRepository newsletterRepository,
    IStringLocalizer<SharedResource> localizer,
    IWebEncoder webEncoder) : ICommandHandler<InviteNewsletterCommand, string>
{
    public async Task<string> Handle(InviteNewsletterCommand request, CancellationToken cancellationToken)
    {
        string token = webEncoder.Base64UrlEncodeGuid();

        var newsletter = await newsletterRepository.GetAsync(request.Email);

        if (newsletter?.IsSubscribed() ?? false)
        {
            throw new EmailIsAlreadySubscribedToNewsletterException(localizer);
        }

        newsletter ??= Newsletter.Create(request.Email, token);
        newsletter.Invite();

        await newsletterRepository.UpdateAsync(newsletter);

        return localizer[Localizations.NewsletterSentEmailResponse].Value;
    }
}