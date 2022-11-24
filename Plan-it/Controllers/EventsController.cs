using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EventsController
{
    private readonly UseCaseCreateEvents _useCaseCreateEvents;
    private readonly UseCaseFetchAllEvents _useCaseFetchAllEvents;
    private readonly UseCaseFetchFromToEvents _useCaseFetchFromToEvents;
    private readonly UseCaseUpdateEvents _useCaseUpdateEvents;
    
    private readonly IConfiguration _config;

    public EventsController(
        UseCaseCreateEvents useCaseCreateEvents,
        UseCaseFetchAllEvents useCaseFetchAllEvents,
        UseCaseFetchFromToEvents useCaseFetchFromToEvents,
        UseCaseUpdateEvents useCaseUpdateEvents,
        IConfiguration configuration
    )
    {
        _useCaseCreateEvents = useCaseCreateEvents;
        _useCaseFetchAllEvents = useCaseFetchAllEvents;
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
        // Use for add new account easily
        //dto.account.Function = "Employee";
        var output = _useCaseCreateEvents.Execute(dto);
        return null;
        /*
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdAccount },
            output
        );
        */
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