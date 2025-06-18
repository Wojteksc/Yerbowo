namespace Yerbowo.Application.Functions.Newsletters.Command.UnsubscribeNewsletter;

public class UnsubscribeNewsletterHandler(
    INewsletterRepository newsletterRepository,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<UnsubscribeNewsletterCommand>
{
    public async Task Handle(UnsubscribeNewsletterCommand request, CancellationToken cancellationToken)
    {
        var newsletter = await newsletterRepository.GetAsync(request.Email) 
            ?? throw new NewsletterNotFoundException(localizer);
        
        if (newsletter.VerificationToken != request.Token)
        {
            throw new InvalidTokenException(localizer);
        }

        newsletter.Unsubscribe();
        await newsletterRepository.UpdateAsync(newsletter);
    }
}