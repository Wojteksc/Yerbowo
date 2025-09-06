namespace Yerbowo.Application.Functions.Emails.Command.SendDiscountCouponEmail;

public class SendDiscountCouponEmailHandler(
    IEmailService<NewsletterCouponTemplate> emailService,
    IAppSettings appSettings) : ICommandHandler<SendDiscountCouponEmailCommand>
{
    public async Task Handle(SendDiscountCouponEmailCommand request, CancellationToken cancellationToken)
    {
        //TO DO: create coupon

        //TO DO: send email with coupon

        object dynamicTemplateData = new
        {
            UnsubscribeLink = $"{appSettings.BaseUrl}/newsletter/unsubscribe?email={request.Email}&token={request.VerificationToken}"
        };
        
        await emailService.SendAsync(request.Email, dynamicTemplateData);
    }
}