using Domain;

namespace JWT.Models;

public interface ISessionService
{
    string BuildToken(string key, string issuer, Account account, string function);

    // string BuildTokenFunction(string key, string issuer, string role);
    
    string BuildTokenPublic(string key, string issuer, Account account, int id, string function);

    bool IsTokenValid(string key, string issuer, string token);
}