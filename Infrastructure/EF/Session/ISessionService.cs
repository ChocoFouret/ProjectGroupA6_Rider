using Domain;

namespace JWT.Models;

public interface ISessionService
{
    string BuildToken(string key, string issuer, Account account);

    string BuildTokenFunction(string key, string issuer, string role);
    bool IsTokenValid(string key, string issuer, string token);
}