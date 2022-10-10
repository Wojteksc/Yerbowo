using Yerbowo.Application.Functions.Auth.Command;

namespace Yerbowo.Application.Services.Jwt;

public interface IJwtHandler
{
    TokenDto CreateToken(int userId, string userName, string role);
}
