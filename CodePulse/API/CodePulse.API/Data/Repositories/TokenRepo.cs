using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Data.Repositories;

public interface ITokenRepo
{
    string GenerateToken(IdentityUser user, List<string> roles);
}

public class TokenRepo(IConfiguration config) : ITokenRepo
{
    private readonly IConfiguration _configuration = config;

    public string GenerateToken(IdentityUser user, List<string> roles)
    {
        // Create claims

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email)
        };
        if (claims == null) throw new ArgumentNullException(nameof(claims));
        claims.AddRange(roles.Select(role => new Claim("role", role)));


        // JWT security token parameters
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

        var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creadentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}