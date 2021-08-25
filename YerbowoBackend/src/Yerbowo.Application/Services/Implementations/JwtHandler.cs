using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Yerbowo.Application.Auth;
using Yerbowo.Application.Extensions;
using Yerbowo.Application.Settings;

namespace Yerbowo.Application.Services.Implementations
{
	public class JwtHandler : IJwtHandler
	{
		private readonly JwtSettings _jwtSettings;

		public JwtHandler(IOptions<JwtSettings> jwtSettings)
		{
			_jwtSettings = jwtSettings.Value;
		}

		public TokenDto CreateToken(int userId, string userName, string role)
		{
			var now = DateTime.UtcNow;
			var claims = new Claim[]
			{
			   new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
			   new Claim(JwtRegisteredClaimNames.UniqueName, userName.ToString()),
			   new Claim(ClaimTypes.Role, role),
			   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			   new Claim(JwtRegisteredClaimNames.Iat, now.ToTimeStamp().ToString())
			};

			var expires = now.AddMinutes(_jwtSettings.ExpiryMinutes);
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
				SecurityAlgorithms.HmacSha512Signature);

			var jwt = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				claims: claims,
				notBefore: now,
				expires: expires,
				signingCredentials: signingCredentials
			);

			string token = new JwtSecurityTokenHandler().WriteToken(jwt);

			return new TokenDto()
			{
				Token = token,
				Expires = expires.ToTimeStamp(),
				Role = role
			};
		}
	}
}
