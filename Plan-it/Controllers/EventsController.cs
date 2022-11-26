using Application.UseCases.Accounts;
using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebSocketDemo.Hubs;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EventsController : ControllerBase
{
    private readonly UseCaseCreateEvents _useCaseCreateEvents;
    private readonly UseCaseDeleteEvents _useCaseDeleteEvents;
    private readonly UseCaseFetchAllEvents _useCaseFetchAllEvents;
    private readonly UseCaseFetchEventsById _useCaseFetchEventById;
    private readonly UseCaseFetchFromToEvents _useCaseFetchFromToEvents;
    private readonly UseCaseFetchFromToAccountEvents _useCaseFetchFromToAccountEvents;
    private readonly UseCaseUpdateEvents _useCaseUpdateEvents;
    private readonly IHubContext<EventHub> eventHub;

    public EventsController(
        UseCaseCreateEvents useCaseCreateEvents,
        UseCaseDeleteEvents useCaseDeleteEvents,
        UseCaseFetchAllEvents useCaseFetchAllEvents,
        UseCaseFetchEventsById useCaseFetchEventById,
        UseCaseFetchFromToEvents useCaseFetchFromToEvents,
        UseCaseFetchFromToAccountEvents useCaseFetchFromToAccountEvents,
        UseCaseUpdateEvents useCaseUpdateEvents,
        IHubContext<EventHub> eventHub
    )
    {
        _useCaseCreateEvents = useCaseCreateEvents;
        _useCaseDeleteEvents = useCaseDeleteEvents;
        _useCaseFetchAllEvents = useCaseFetchAllEvents;
        _useCaseFetchEventById = useCaseFetchEventById;
        _useCaseFetchFromToEvents = useCaseFetchFromToEvents;
        _useCaseFetchFromToAccountEvents = useCaseFetchFromToAccountEvents;
        _useCaseUpdateEvents = useCaseUpdateEvents;
        this.eventHub = eventHub;
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
    [Route("fetch/{idSchedule}/{from}/{to}")]
    public IEnumerable<DtoOutputEvents> FetchFromTo(int idSchedule, DateTime from, DateTime to)
    {
        DtoInputDateEvents date = new DtoInputDateEvents
        {
            IdSchedule = idSchedule,
            From = from,
            To = to
        };

        return _useCaseFetchFromToEvents.Execute(date);
    }
    
    [HttpGet]
    [Route("fetch/{idSchedule}/{idAccount}/{from}/{to}")]
    public IEnumerable<DtoOutputEvents> FetchFromTo(int idSchedule, DateTime from, DateTime to, int idAccount)
    {
        DtoInputDateEvents date = new DtoInputDateEvents
        {
            IdAccount = idAccount,
            IdSchedule = idSchedule,
            From = from,
            To = to
        };

        return _useCaseFetchFromToAccountEvents.Execute(date);
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoInputCreateEvents> Create(DtoInputCreateEvents dto)
    {
        var output = _useCaseCreateEvents.Execute(dto);
        eventHub.Clients.All.SendAsync(WebSocketActions.MESSAGE_CREATED, dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdEventsEmployee },
            output
        );
    }
    
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateEvents dto)
    {
        eventHub.Clients.All.SendAsync(WebSocketActions.MESSAGE_UPDATED, dto);
        return _useCaseUpdateEvents.Execute(dto);
    }
    
    [HttpDelete]
    [Route("delete/{IdEventsEmployee}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(string IdEventsEmployee)
    {
        eventHub.Clients.All.SendAsync(WebSocketActions.MESSAGE_DELETED, IdEventsEmployee);
        return _useCaseDeleteEvents.Execute(IdEventsEmployee);
    }
}