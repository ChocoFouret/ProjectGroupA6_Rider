using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EventTypesController : ControllerBase
{
    private readonly UseCaseCreateEventTypes _useCaseCreateEventTypes;
    private readonly UseCaseFetchAllEventType _useCaseFetchAllEventType;
    private readonly UseCaseFetchEventTypesByType _useCaseFetchEventTypesByType;
    private readonly UseCaseUpdateEventTypes _useCaseUpdateEventTypes;
    
    public EventTypesController(
        UseCaseCreateEventTypes useCaseCreateEventTypes,
        UseCaseFetchAllEventType useCaseFetchAllEventType,
        UseCaseFetchEventTypesByType useCaseFetchEventTypesByType,
        UseCaseUpdateEventTypes useCaseUpdateEventTypes
    )
    {
        _useCaseCreateEventTypes = useCaseCreateEventTypes;
        _useCaseFetchAllEventType = useCaseFetchAllEventType;
        _useCaseFetchEventTypesByType = useCaseFetchEventTypesByType;
        _useCaseUpdateEventTypes = useCaseUpdateEventTypes;
    }
    
    [HttpGet]
    [Route("fetch/all")]
    public IEnumerable<DtoOutputEventTypes> FetchAll()
    {
        return _useCaseFetchAllEventType.Execute();
    }
    
    [HttpGet]
    [Route("fetch/{type}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputEventTypes> FetchByType(string type)
    {
        try
        {
            return _useCaseFetchEventTypesByType.Execute(type);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoInputCreateEventTypes> Create(DtoInputCreateEventTypes dto)
    {
        var output = _useCaseCreateEventTypes.Execute(dto);
        return CreatedAtAction(
            nameof(FetchByType),
            new { type = output.Types },
            output
        );
    }

    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateEventTypes dto)
    {
        return _useCaseUpdateEventTypes.Execute(dto);
    }
}