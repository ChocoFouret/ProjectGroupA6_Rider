using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using JWT.Models;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.EF.Session;

public class SessionService : ISessionService
{
    private const double EXPIRY_DURATION_MINUTES = 1440;

    public string BuildToken(string key, string issuer, Account account, string function)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.LastName),
            new Claim(ClaimTypes.Role, function),
            new Claim(ClaimTypes.NameIdentifier,
                Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

//    public string BuildTokenFunction(string key, string issuer, string role)
    public string BuildTokenPublic(string key, string issuer, Account account, int id, string function)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, account.IdAccount.ToString()),
            new Claim(ClaimTypes.Authentication, account.IsAdmin.ToString()),
            new Claim(ClaimTypes.UserData, id.ToString()),
            new Claim(ClaimTypes.Role, function),
            new Claim(ClaimTypes.NameIdentifier,
                Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
}