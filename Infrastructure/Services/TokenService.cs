using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"] ?? string.Empty));
    }

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        };
        
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _config["Token:Issuer"]
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}