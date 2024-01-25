namespace Yerbowo.Application.Abstractions;

public interface IJwtProvider
{
    TokenDto CreateToken(int userId, string userName, string role);
}