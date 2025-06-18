namespace Yerbowo.Application.Abstractions.Security;

public interface IAuthenticator
{
    TokenDto CreateToken(int userId, string userName, string role);
}