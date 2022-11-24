using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EventsController : ControllerBase
{
    private readonly UseCaseCreateEvents _useCaseCreateEvents;
    private readonly UseCaseFetchAllEvents _useCaseFetchAllEvents;
    private readonly UseCaseFetchEventsById _useCaseFetchEventById;
    private readonly UseCaseFetchFromToEvents _useCaseFetchFromToEvents;
    private readonly UseCaseUpdateEvents _useCaseUpdateEvents;
    
    private readonly IConfiguration _config;

    public EventsController(
        UseCaseCreateEvents useCaseCreateEvents,
        UseCaseFetchAllEvents useCaseFetchAllEvents,
        UseCaseFetchEventsById useCaseFetchEventById,
        UseCaseFetchFromToEvents useCaseFetchFromToEvents,
        UseCaseUpdateEvents useCaseUpdateEvents,
        IConfiguration configuration
    )
    {
        _useCaseCreateEvents = useCaseCreateEvents;
        _useCaseFetchAllEvents = useCaseFetchAllEvents;
        _useCaseFetchEventById = useCaseFetchEventById;
        _useCaseFetchFromToEvents = useCaseFetchFromToEvents;
        _useCaseUpdateEvents = useCaseUpdateEvents;
        _config = configuration;
    }


    [HttpGet]
    [Route("fetch/all")]
    public IEnumerable<DtoOutputEvents> FetchAll()
    {
        return _useCaseFetchAllEvents.Execute();
    }
    
    [HttpGet]
    [Route("fetch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputEvents> FetchById(int id)
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
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoInputCreateEvents> Create(DtoInputCreateEvents dto)
    {
        return dto;
        var output = _useCaseCreateEvents.Execute(dto);
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
        return _useCaseUpdateEvents.Execute(dto);
    }
    
}