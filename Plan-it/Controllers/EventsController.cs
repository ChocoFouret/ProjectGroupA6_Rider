using Application.UseCases.Accounts;
using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebSocketDemo.Hubs;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
// [Authorize(Policy = "all")]
public class EventsController : ControllerBase
{
    private readonly UseCaseCreateEvents _useCaseCreateEvents;
    private readonly UseCaseDeleteEvents _useCaseDeleteEvents;
    private readonly UseCaseFetchAllEvents _useCaseFetchAllEvents;
    private readonly UseCaseFetchEventsById _useCaseFetchEventById;
    private readonly UseCaseFetchFromToEvents _useCaseFetchFromToEvents;
    private readonly UseCaseFetchFromToAccountEvents _useCaseFetchFromToAccountEvents;
    private readonly UseCaseUpdateEvents _useCaseUpdateEvents;
    private readonly IHubContext<EventsHub> _eventsHub;
    private readonly UseCaseFetchEventsByEmployee _useCaseFetchEventsByEmployee;

    public EventsController(
        UseCaseCreateEvents useCaseCreateEvents,
        UseCaseDeleteEvents useCaseDeleteEvents,
        UseCaseFetchAllEvents useCaseFetchAllEvents,
        UseCaseFetchEventsById useCaseFetchEventById,
        UseCaseFetchFromToEvents useCaseFetchFromToEvents,
        UseCaseFetchFromToAccountEvents useCaseFetchFromToAccountEvents,
        UseCaseUpdateEvents useCaseUpdateEvents,
        IHubContext<EventsHub> eventsHub,
        UseCaseFetchEventsByEmployee useCaseFetchEventsByEmployee
    )
    {
        _useCaseCreateEvents = useCaseCreateEvents;
        _useCaseDeleteEvents = useCaseDeleteEvents;
        _useCaseFetchAllEvents = useCaseFetchAllEvents;
        _useCaseFetchEventById = useCaseFetchEventById;
        _useCaseFetchFromToEvents = useCaseFetchFromToEvents;
        _useCaseFetchFromToAccountEvents = useCaseFetchFromToAccountEvents;
        _useCaseUpdateEvents = useCaseUpdateEvents;
        _useCaseFetchEventsByEmployee = useCaseFetchEventsByEmployee;
        _eventsHub = eventsHub;
    }

    [HttpGet]
    [Route("fetch/all")]
    public IEnumerable<DtoOutputEvents> FetchAll()
    {
        return _useCaseFetchAllEvents.Execute();
    }
    
    [HttpGet]
    [Route("fetch/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputEvents> FetchById(string id)
    {
        try
        {
            return _useCaseFetchEventById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    [Route("fetch/{idCompanies}/{from}/{to}")]
    public IEnumerable<DtoOutputEvents> FetchFromTo(int idCompanies, DateTime from, DateTime to)
    {
        DtoInputDateEvents date = new DtoInputDateEvents
        {
            IdCompanies = idCompanies,
            From = from,
            To = to
        };

        return _useCaseFetchFromToEvents.Execute(date);
    }
    
    [HttpGet]
    [Route("fetch/{idCompanies}/{idAccount}/{from}/{to}")]
    public IEnumerable<DtoOutputEvents> FetchFromTo(int idCompanies, DateTime from, DateTime to, int idAccount)
    {
        DtoInputDateEvents date = new DtoInputDateEvents
        {
            IdAccount = idAccount,
            IdCompanies = idCompanies,
            From = from,
            To = to
        };

        return _useCaseFetchFromToAccountEvents.Execute(date);
    }
    
    [HttpPost]
    [Route("create/{idCompanies}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoInputCreateEvents> Create(DtoInputCreateEvents dto, string idCompanies)
    {
        var output = _useCaseCreateEvents.Execute(dto);
        _eventsHub.Clients.Group(idCompanies).SendAsync(WebSocketActions.MESSAGE_CREATED, dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdEventsEmployee },
            output
        );
    }
    
    [HttpPut]
    [Route("update/{idCompanies}")]
    // [Authorize(Policy = "all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateEvents dto, string idCompanies)
    {
        /*
        if (!User.IsInRole("Directeur"))
        {
            Console.WriteLine("PAS DIRECTEUR");
            if (dto.StartDate > dto.EndDate || dto.IsValid)
            {
                return false;
            }
        }
        */
        
        _eventsHub.Clients.Group(idCompanies).SendAsync(WebSocketActions.MESSAGE_UPDATED, dto);
        return _useCaseUpdateEvents.Execute(dto);
    }
    
    [HttpDelete]
    [Route("delete/{IdEventsEmployee}/{idCompanies}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(string IdEventsEmployee, string idCompanies)
    {
        _eventsHub.Clients.Group(idCompanies).SendAsync(WebSocketActions.MESSAGE_DELETED, IdEventsEmployee);
        return _useCaseDeleteEvents.Execute(IdEventsEmployee);
    }
    
    [HttpGet]
    [Route("fetch/employee/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputEvents> FetchByEmployee(int id)
    {
        return _useCaseFetchEventsByEmployee.Execute(id);
    }

}