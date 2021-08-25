using MediatR;

namespace Yerbowo.Application.Auth.SocialLogin
{
	public class SocialLoginCommand : IRequest<ResponseToken>
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhotoUrl { get; set; }
		public string Provider { get; set; }
	}
}
