namespace Yerbowo.Application.Functions.Newsletters.Command.InviteNewsletter;

public class InviteNewsletterHandler : IRequestHandler<InviteNewsletterCommand, string>
{
    private readonly INewsletterRepository _newsletterRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IWebEncoder _webEncoder;

    public InviteNewsletterHandler(INewsletterRepository newsletterRepository,
        IStringLocalizer<SharedResource> localizer,
        IWebEncoder webEncoder)
    {
        _newsletterRepository = newsletterRepository;
        _localizer = localizer;
        _webEncoder = webEncoder;
    }

    public async Task<string> Handle(InviteNewsletterCommand request, CancellationToken cancellationToken)
    {
        string token = _webEncoder.Base64UrlEncodeGuid();

        var newsletter = await _newsletterRepository.GetAsync(request.Email);

        if (newsletter?.IsSubscribed() ?? false)
        {
            throw new Exception(_localizer["Newsletter.Error.AlreadySubscribed"]);
        }

        newsletter ??= Newsletter.Create(request.Email, token);
        newsletter.Invite();

        await _newsletterRepository.UpdateAsync(newsletter);
        //TO DO: powinien byc async _unitOfWork.SaveChangesAsync();

        //await _mediator.Publish(new NewsletterSubscribedEvent(newsletter));

        return _localizer["Newsletter.Response.SentEmail"];
    }
}