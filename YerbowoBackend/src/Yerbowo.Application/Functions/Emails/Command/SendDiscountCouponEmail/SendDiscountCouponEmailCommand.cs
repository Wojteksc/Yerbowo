namespace Yerbowo.Application.Functions.Emails.Command.SendDiscountCouponEmail;

public record SendDiscountCouponEmailCommand(string Email, string VerificationToken) : ICommand { }