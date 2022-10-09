namespace Yerbowo.Application.Functions.Auth.Command;

public class TokenDto
{
    public string Token { get; set; }
    public string Role { get; set; }
    public long Expires { get; set; }
}