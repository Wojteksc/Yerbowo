namespace Yerbowo.Application.Functions.Newsletters.Command.InviteNewsletter;

public class InviteNewsletterHandler(
    INewsletterRepository newsletterRepository,
    IStringLocalizer<SharedResource> localizer,
    IWebEncoder webEncoder,
    IIdGenerator idGenerator) : ICommandHandler<InviteNewsletterCommand, string>
{
    public async Task<string> Handle(InviteNewsletterCommand request, CancellationToken cancellationToken)
    {
        string token = webEncoder.Base64UrlEncodeGuid();

        var newsletter = await newsletterRepository.GetAsync(request.Email);

        if (newsletter?.IsSubscribed() ?? false)
        {
            throw new EmailIsAlreadySubscribedToNewsletterException(localizer);
        }

        if (newsletter is null)
        {
            var newNewsletter = Newsletter.Create(idGenerator.Generate(), request.Email, token);
            newNewsletter.Invite();
            await newsletterRepository.AddAsync(newNewsletter);
        }
        else
        {
            newsletter.SetToken(token);
            newsletter.Invite();
            await newsletterRepository.UpdateAsync(newsletter);
        }

        return localizer[Localizations.NewsletterSentEmailResponse].Value;
    }
}