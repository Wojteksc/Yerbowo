namespace Yerbowo.Infrastructure.Authentication;

public class Authenticator : IAuthenticator
{
    private readonly JwtOptions _jwtSettings;

    public Authenticator(IOptions<JwtOptions> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public TokenDto CreateToken(int userId, string userName, string role)
    {
        var now = DateTime.UtcNow;
        var claims = new Claim[]
        {
           new (JwtRegisteredClaimNames.Sub, userId.ToString()),
           new (JwtRegisteredClaimNames.UniqueName, userName.ToString()),
           new (ClaimTypes.Role, role),
           new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new (JwtRegisteredClaimNames.Iat, ConvertToTimeStamp(now).ToString()),
        };

        var expires = now.Add(_jwtSettings.Expiry);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            SecurityAlgorithms.HmacSha512Signature);

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: signingCredentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenDto(token);
    }

    private static long ConvertToTimeStamp(DateTime dateTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var time = dateTime.Subtract(new TimeSpan(epoch.Ticks));

        return time.Ticks / 10000;
    }
}