using Yerbowo.Application.Auth;

namespace Yerbowo.Application.Services.Jwt;

public interface IJwtHandler
{
    TokenDto CreateToken(int userId, string userName, string role);
}
