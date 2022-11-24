using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfEventsRepository : IEventsRepository
{
    private readonly PlanitContextProvider _planitContextProvider;
    
    public EfEventsRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<Events> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Events.ToList<Events>();
    }

    public Events FetchById(int idEventsEmployee)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.FirstOrDefault(events => events.IdEventsEmployee == idEventsEmployee);

        if (events == null)
            throw new KeyNotFoundException($"Events with {idEventsEmployee} has not been found");

        return events;
    }
    
    public IEnumerable<Events> FetchFromTo(int IdSchedule, DateTime from, DateTime to)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events =>  events.IdSchedule == IdSchedule && events.StartDate >= from && events.EndDate <= to).ToList();

        if (events == null)
            throw new KeyNotFoundException($"Events for Company ID {IdSchedule} between {from} and {to} were not found");

        return events;
    }
    
    public Events Create(Events events)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Events.Add(events);
            context.SaveChanges();
            return events;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    public bool Update(Events events)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Events.Update(events);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Delete(Events events)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Events.Remove(events);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

}