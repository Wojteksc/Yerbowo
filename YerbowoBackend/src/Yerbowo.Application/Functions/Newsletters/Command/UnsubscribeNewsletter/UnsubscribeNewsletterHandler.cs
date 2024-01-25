namespace Yerbowo.Application.Functions.Newsletters.Command.UnsubscribeNewsletter;

public class UnsubscribeNewsletterHandler : IRequestHandler<UnsubscribeNewsletterCommand>
{
    private readonly INewsletterRepository _newsletterRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UnsubscribeNewsletterHandler(
        INewsletterRepository newsletterRepository, 
        IStringLocalizer<SharedResource> localizer)
    {
        _newsletterRepository = newsletterRepository;
        _localizer = localizer;
    }

    public async Task Handle(UnsubscribeNewsletterCommand request, CancellationToken cancellationToken)
    {
        var newsletter = await _newsletterRepository.GetAsync(request.Email);
        if (newsletter == null || newsletter.VerificationToken != request.Token)
        {
            throw new ArgumentException(_localizer["ResponseBadRequest"]);
        }

        newsletter.Unsubscribe();
        await _newsletterRepository.UpdateAsync(newsletter);
    }
}