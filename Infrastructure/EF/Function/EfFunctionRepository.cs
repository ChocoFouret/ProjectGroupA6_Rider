using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfFunctionRepository : IFunctionRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    public EfFunctionRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<Function> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Functions.ToList<Function>();
    }

    public Function Create(string title)
    {
        using var context = _planitContextProvider.NewContext();

        Function function = new Function();
        function.Title = title;
        
        context.Functions.Add(function);
        
        context.SaveChanges();
        return function;
    }
    
    public Function FetchByTitle(string title)
    {
        using var context = _planitContextProvider.NewContext();
        var function = context.Functions.FirstOrDefault(function => function.Title == title);

        if (function == null)
            throw new KeyNotFoundException($"Function with {title} has not been found");

        return function;
    }
    
    
    /*
    public Function Read(Function function)
    {
        return null;
    }

    public bool Update(Function function)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Functions.Update(function);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Delete(string Function)
    {
        using var context = _planitContextProvider.NewContext();

        try
        {
            context.Functions.Remove(new Function { Id = idFunction });
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
    */
}