namespace Yerbowo.Application.Functions.Auth.Command.SocialLogin;

public record SocialLoginCommand : ICommand<ResponseToken>
{
	public string Email { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string PhotoUrl { get; init; }
	public string Provider { get; init; }
}