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

    public Events FetchById(string idEventsEmployee)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.FirstOrDefault(events => events.IdEventsEmployee == idEventsEmployee);

        if (events == null)
            throw new KeyNotFoundException($"Events with {idEventsEmployee} has not been found");

        var eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events.Types);
        events.EventTypes = eventType;

        return events;
    }

    public IEnumerable<Events> FetchFromTo(int IdSchedule, DateTime from, DateTime to)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events =>
            events.IdSchedule == IdSchedule && events.StartDate >= from && events.EndDate <= to).ToList();

        if (events == null)
            throw new KeyNotFoundException(
                $"Events for Company ID {IdSchedule} between {from} and {to} were not found");

        EventTypes eventType;
        int x;
        for (x = 0; x <= events.Count - 1; x++)
        {
            eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events[x].Types);
            events[x].EventTypes = eventType;
        }

        return events;
    }
    
    public IEnumerable<Events> FetchFromToAccount(int IdSchedule, DateTime from, DateTime to, int? idAccount)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events =>
            events.IdSchedule == IdSchedule && events.StartDate >= from && events.EndDate <= to && events.IdAccount == idAccount).ToList();

        if (events == null)
            throw new KeyNotFoundException(
                $"Events for Company ID {IdSchedule} between {from} and {to} were not found");

        EventTypes eventType;
        int x;
        for (x = 0; x <= events.Count - 1; x++)
        {
            eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events[x].Types);
            events[x].EventTypes = eventType;
        }

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
            //Console.WriteLine(events.Types);
            //Console.WriteLine(context.Events.FirstOrDefault(e => e.IdEventsEmployee == events.IdEventsEmployee)?.Types);
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