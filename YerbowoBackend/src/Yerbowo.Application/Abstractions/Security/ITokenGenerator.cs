namespace Yerbowo.Application.Abstractions.Security;

public interface ITokenGenerator
{
    TokenDto CreateToken(Guid userId, string userName, string role);
}