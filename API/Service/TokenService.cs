using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Azure.Core;

namespace API.Service
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration Configuration;
		private readonly SymmetricSecurityKey Key;

		public TokenService(IConfiguration configuration)
		{
			Configuration = configuration;
			Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string(Configuration["JWT:SigningKey"])));
		}

		public string CreateToken(User user)
		{
			if (user.Email == null)
			{
				throw new ArgumentNullException(nameof(user.Email));
			}

			if (user.UserName == null)
			{
				throw new ArgumentNullException(nameof(user.UserName));
			}

			List<Claim> claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
			};

			SigningCredentials cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = cred,
				Issuer = Configuration["JWT:Issuer"],
				Audience = Configuration["JWT:Audience"]
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}