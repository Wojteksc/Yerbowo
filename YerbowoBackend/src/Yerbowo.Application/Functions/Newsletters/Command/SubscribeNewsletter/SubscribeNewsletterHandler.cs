namespace Yerbowo.Application.Functions.Newsletters.Command.SubscribeNewsletter;

public class SubscribeNewsletterHandler : IRequestHandler<SubscribeNewsletterCommand>
{
    private readonly IMediator _mediator;
    private readonly INewsletterRepository _newsletterRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SubscribeNewsletterHandler(
        IMediator mediator,
        INewsletterRepository newsletterRepository, 
        IStringLocalizer<SharedResource> localizer)
    {
        _mediator = mediator;
        _newsletterRepository = newsletterRepository;
        _localizer = localizer;
    }

    public async Task Handle(SubscribeNewsletterCommand request, CancellationToken cancellationToken)
    {
        var newsletter = await _newsletterRepository.GetAsync(request.Email);
        if (newsletter == null || newsletter.VerificationToken != request.Token)
        {
            throw new ArgumentException(_localizer["ResponseBadRequest"]);
        }

        if (newsletter.IsSubscribed())
        {
            throw new Exception(_localizer["ExceptionEmailWasConfirmed"]);
        }

        newsletter.Subscribe();
        await _newsletterRepository.UpdateAsync(newsletter);
    }
}