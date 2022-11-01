using Domain;

namespace JWT.Models;

public interface ISessionService
{
    string BuildToken(string key, string issuer, Account account);
    bool IsTokenValid(string key, string issuer, string token);
}