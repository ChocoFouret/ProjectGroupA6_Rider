namespace Infrastructure;

public interface IConnectionStringProvider
{
    string? GetConnectionString(string key);
}