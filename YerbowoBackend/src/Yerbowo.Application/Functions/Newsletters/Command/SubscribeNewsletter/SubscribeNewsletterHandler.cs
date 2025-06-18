namespace Yerbowo.Application.Functions.Newsletters.Command.SubscribeNewsletter;

public class SubscribeNewsletterHandler(
    INewsletterRepository newsletterRepository,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<SubscribeNewsletterCommand>
{
    public async Task Handle(SubscribeNewsletterCommand request, CancellationToken cancellationToken)
    {
        var newsletter = await newsletterRepository.GetAsync(request.Email) 
            ?? throw new NewsletterNotFoundException(localizer);
        
        if (newsletter.VerificationToken != request.Token)
        {
            throw new InvalidTokenException(localizer);
        }

        if (newsletter.IsSubscribed())
        {
            throw new EmailWasAlreadyConfirmedException(localizer);
        }

        newsletter.Subscribe();
        await newsletterRepository.UpdateAsync(newsletter);
    }
}