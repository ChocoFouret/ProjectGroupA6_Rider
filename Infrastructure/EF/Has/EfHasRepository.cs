using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Has;
using Domain;

public class EfHasRepository: IHasRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    public EfHasRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }
    public IEnumerable<Has> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Has.ToList<Has>();
    }

    public IEnumerable<Domain.Has> FetchByCompanies(int id)
    {
        using var context = _planitContextProvider.NewContext();
        var companies = context.Has.Where(has=>has.IdCompanies==id).ToList();

        if (companies == null)
            throw new KeyNotFoundException($"Companies with {id} has not been found");
        
        for(var x = 0; x < companies.Count; x++)
        {
            companies[x].Account = context.Accounts.FirstOrDefault(account => account.IdAccount == companies[x].IdAccount);
            companies[x].Function = context.Functions.FirstOrDefault(function => function.IdFunctions == companies[x].IdFunctions);
        }
        
        return companies;
    }

    public IEnumerable<Domain.Has> FetchByFunctions(int id)
    {
        using var context = _planitContextProvider.NewContext();
        var functions = context.Has.Where(has=>has.IdFunctions==id).ToList();

        if (functions == null)
            throw new KeyNotFoundException($"Functions with {id} has not been found");
        return functions;
    }

    public IEnumerable<Domain.Has> FetchByAccount(int id)
    {
        using var context = _planitContextProvider.NewContext();
        var accounts = context.Has.Where(has=>has.IdAccount==id).ToList();

        if (accounts == null)
            throw new KeyNotFoundException($"Accounts with {id} has not been found");
            
        for(var x = 0; x < accounts.Count; x++)
        {
            accounts[x].Account = context.Accounts.FirstOrDefault(a => a.IdAccount == accounts[x].IdAccount);
            accounts[x].Function = context.Functions.FirstOrDefault(a => a.IdFunctions == accounts[x].IdFunctions);
        }
            
        return accounts;
    }

    public Has Create(Has has)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Has.Add(has);
            context.SaveChanges();
            return has;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    public Has FetchById(int id)
    {
        using var context = _planitContextProvider.NewContext();
        var has = context.Has.FirstOrDefault(has => has.IdHas == id);

        if (has == null)
            throw new KeyNotFoundException($"Has with {id} has not been found");
        return has;
    }

    public bool Delete(Has has)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Has.Remove(has);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
}