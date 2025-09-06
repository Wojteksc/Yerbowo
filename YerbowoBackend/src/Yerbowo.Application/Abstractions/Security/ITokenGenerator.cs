namespace Yerbowo.Application.Abstractions.Security;

public interface ITokenGenerator
{
    TokenDto CreateToken(int userId, string userName, string role);
}