namespace Infrastructure.EF;

public class PlanitContextProvider
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public PlanitContextProvider(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    public PlanitContext NewContext()
    {
        return new PlanitContext(_connectionStringProvider);
    }
}