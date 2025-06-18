namespace Yerbowo.Application.Functions.Emails.Command.SendDiscountCouponEmail;

public class SendDiscountCouponEmailHandler(
    INewsletterEmailSender emailSender,
    IAppSettings appSettings,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<SendDiscountCouponEmailCommand>
{
    public async Task Handle(SendDiscountCouponEmailCommand request, CancellationToken cancellationToken)
    {
        //TO DO: create coupon

        //TO DO: send email with coupon

        object dynamicTemplateData = new
        {
            UnsubscribeLink = $"{appSettings.BaseUrl}/unsubscribe?email={request.Email}&token={request.VerificationToken}"
        };
        var responseEmail = await emailSender.SendEmailAsync(new EmailAddress(request.Email), dynamicTemplateData);

        if (!responseEmail.IsSuccessStatusCode)
        {
            throw new EmailSendingFailedException(localizer);
        }
    }
}