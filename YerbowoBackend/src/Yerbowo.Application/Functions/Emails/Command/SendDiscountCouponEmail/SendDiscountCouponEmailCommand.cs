namespace Yerbowo.Application.Functions.Emails.Command.SendDiscountCouponEmail;

public class SendDiscountCouponEmailCommand : IRequest
{
    public string Email { get; }
    public string VerificationToken { get; }

    public SendDiscountCouponEmailCommand(string email, string verificationToken)
    {
        Email = email;
        VerificationToken = verificationToken;
    }
}