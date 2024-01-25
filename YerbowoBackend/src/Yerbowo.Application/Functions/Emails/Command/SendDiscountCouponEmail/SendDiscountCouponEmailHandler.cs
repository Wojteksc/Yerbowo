using Yerbowo.Application.Abstractions.Emails.Newsletters;

namespace Yerbowo.Application.Functions.Emails.Command.SendDiscountCouponEmail;

public class SendDiscountCouponEmailHandler : IRequestHandler<SendDiscountCouponEmailCommand>
{
    private readonly INewsletterEmailSender _emailSender;
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SendDiscountCouponEmailHandler(
        INewsletterEmailSender emailSender,
        IOptions<AppSettings> appSettings,
        IStringLocalizer<SharedResource> localizer)
    {
        _appSettings = appSettings.Value;
        _emailSender = emailSender;
        _localizer = localizer;
    }

    public async Task Handle(SendDiscountCouponEmailCommand request, CancellationToken cancellationToken)
    {
        //TO DO: create coupon

        //TO DO: send email with coupon

        object dynamicTemplateData = new
        {
            UnsubscribeLink = $"{_appSettings.BaseUrl}/unsubscribe?email={request.Email}&token={request.VerificationToken}"
        };
        var responseEmail = await _emailSender.SendEmailAsync(new EmailAddress(request.Email), dynamicTemplateData);

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new Exception(_localizer["ExceptionFailedAttemptToSendEmail"]);
        }
    }
}