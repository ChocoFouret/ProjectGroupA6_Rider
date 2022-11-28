using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfEventTypesRepository : IEventTypesRepository
{
    private readonly PlanitContextProvider _planitContextProvider;
    
    public EfEventTypesRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<EventTypes> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.EventTypes.ToList<EventTypes>();
    }

    public EventTypes FetchByTypes(string type)
    {
        using var context = _planitContextProvider.NewContext();
        var eventTypes = context.EventTypes.FirstOrDefault(eventTypes => eventTypes.Types == type);

        if (eventTypes == null)
            throw new KeyNotFoundException($"EventTypes with {type} has not been found");
        
        return eventTypes;
    }

    public EventTypes Create(EventTypes eventTypes)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.EventTypes.Add(eventTypes);
            context.SaveChanges();
            return eventTypes;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    public bool Update(EventTypes eventTypes)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.EventTypes.Update(eventTypes);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Delete(EventTypes eventTypes)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.EventTypes.Remove(eventTypes);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

}