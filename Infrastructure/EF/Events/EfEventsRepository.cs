using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Events;

public class EfEventsRepository : IEventsRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    public EfEventsRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<Domain.Events> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Events.ToList();
    }

    public Domain.Events FetchById(string idEventsEmployee)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.FirstOrDefault(events => events.IdEventsEmployee == idEventsEmployee);

        if (events == null)
            throw new KeyNotFoundException($"Events with {idEventsEmployee} has not been found");

        var eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events.Types);
        events.EventTypes = eventType;

        return events;
    }

    public IEnumerable<Domain.Events> FetchFromTo(int idCompanies, DateTime from, DateTime to)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events =>
            events.IdCompanies == idCompanies && events.StartDate >= from && events.EndDate <= to).ToList();
        
        if (events == null)
            throw new KeyNotFoundException(
                $"Events for Company ID {idCompanies} between {from} and {to} were not found");

        EventTypes eventType;
        int x;
        for (x = 0; x <= events.Count - 1; x++)
        {
            eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events[x].Types)!;
            events[x].EventTypes = eventType;
        }
        return events;
    }
    
    public IEnumerable<Domain.Events> FetchFromToAccount(int idCompanies, DateTime from, DateTime to, int? idAccount)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events =>
            events.IdCompanies == idCompanies && events.StartDate >= from && events.EndDate <= to && events.IdAccount == idAccount).ToList();

        if (events == null)
            throw new KeyNotFoundException(
                $"Events for Company ID {idCompanies} between {from} and {to} were not found");

        EventTypes eventType;
        int x;
        for (x = 0; x <= events.Count - 1; x++)
        {
            eventType = context.EventTypes.FirstOrDefault(eventsType => eventsType.Types == events[x].Types)!;
            events[x].EventTypes = eventType;
        }
        events.Sort((a, b) => DateTime.Compare(a.StartDate, b.StartDate));
        return events;
    }

    public Domain.Events Create(Domain.Events events)
    {
        using var context = _planitContextProvider.NewContext();
        
        if(context.Companies.FirstOrDefault(company => company.IdCompanies == events.IdCompanies) == null)
            throw new KeyNotFoundException($"Company with ID {events.IdCompanies} has not been found");
        
        if(context.Accounts.FirstOrDefault(account => account.IdAccount == events.IdAccount) == null)
            throw new KeyNotFoundException($"Account with ID {events.IdAccount} has not been found");
        
        if(context.EventTypes.FirstOrDefault(eventType => eventType.Types == events.Types) == null)
            throw new KeyNotFoundException($"Event Type {events.Types} has not been found");
        
        if (events.StartDate > events.EndDate)
            throw new ArgumentException("Start date cannot be after end date");

        try
        {
            context.Events.Add(events);
            context.SaveChanges();
            return events;
        }
        catch (DbUpdateConcurrencyException)
        {
            return null!;
        }
    }

    public bool Update(Domain.Events events)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Events.Update(events);
            //Console.WriteLine(events.Types);
            //Console.WriteLine(context.Events.FirstOrDefault(e => e.IdEventsEmployee == events.IdEventsEmployee)?.Types);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public bool Delete(Domain.Events events)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Events.Remove(events);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public IEnumerable<Domain.Events> FetchEventsByEmployee(Account account)
    {
        using var context = _planitContextProvider.NewContext();
        var events = context.Events.Where(events => events.IdAccount == account.IdAccount).ToList();
        events.Sort((a, b) => DateTime.Compare(a.StartDate, b.StartDate));
        if (events == null)
            throw new KeyNotFoundException(
                $" event for account not found");
        return events;
    }
}