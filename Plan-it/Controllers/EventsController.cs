using Application.UseCases.Accounts;
using Application.UseCases.Events.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic.CompilerServices;
using WebSocketDemo.Hubs;

namespace Plan_it.Controllers;

[ApiController]
[Authorize(Policy = "all")]
[Route("api/v1/[controller]")]
public class EventsController : ControllerBase
{
    private readonly UseCaseCreateEvents _useCaseCreateEvents;
    private readonly UseCaseDeleteEvents _useCaseDeleteEvents;
    private readonly UseCaseFetchAllEvents _useCaseFetchAllEvents;
    private readonly UseCaseFetchEventsById _useCaseFetchEventById;
    private readonly UseCaseFetchFromToEvents _useCaseFetchFromToEvents;
    private readonly UseCaseFetchStartToEndAccountEvents _useCaseFetchStartToEndAccountEvents;
    private readonly UseCaseUpdateEvents _useCaseUpdateEvents;
    private readonly IHubContext<EventsHub> _eventsHub;
    private readonly UseCaseFetchEventsByEmployee _useCaseFetchEventsByEmployee;

    public EventsController(
        UseCaseCreateEvents useCaseCreateEvents,
        UseCaseDeleteEvents useCaseDeleteEvents,
        UseCaseFetchAllEvents useCaseFetchAllEvents,
        UseCaseFetchEventsById useCaseFetchEventById,
        UseCaseFetchFromToEvents useCaseFetchFromToEvents,
        UseCaseFetchStartToEndAccountEvents useCaseFetchStartToEndAccountEvents,
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
        _useCaseFetchStartToEndAccountEvents = useCaseFetchStartToEndAccountEvents;
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

    //Allows you to display the details of an event
    //when you click on it in the schedule
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

    //Allows events to be displayed by company
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

    //Allows events to be displayed by company and by account
    [HttpGet]
    [Route("fetch/{idCompanies:int}/{idAccount:int}/{from:datetime}/{to:datetime}")]
    public IEnumerable<DtoOutputEvents> FetchFromTo(int idCompanies, DateTime from, DateTime to, int idAccount)
    {
        DtoInputDateEvents date = new DtoInputDateEvents
        {
            IdAccount = idAccount,
            IdCompanies = idCompanies,
            From = from,
            To = to
        };

        return _useCaseFetchStartToEndAccountEvents.Execute(date);
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<bool> Update(DtoInputUpdateEvents dto, string idCompanies)
    {
        if (!User.IsInRole("Directeur"))
        {
            if (dto.StartDate > dto.EndDate || dto.IsValid)
            {
                return false;
            }
        }

        int[] minutes = { 0, 15, 30, 45, 60 };
        var startMinute = 0;
        var endMinute = 0;
        for (int i = 0; i <= minutes.Length - 2; i++)
        {
            if (minutes[i] <= dto.StartDate.Minute && (minutes[i] + 7) >= dto.StartDate.Minute)
            {
                startMinute = minutes[i];
            }
            else if (minutes[i + 1] - 7 <= dto.StartDate.Minute && minutes[i + 1] >= dto.StartDate.Minute)
            {
                startMinute = minutes[i + 1];
            }

            if (minutes[i] <= dto.EndDate.Minute && (minutes[i] + 7) >= dto.EndDate.Minute)
            {
                endMinute = minutes[i];
            }
            else if (minutes[i + 1] - 7 <= dto.EndDate.Minute && minutes[i + 1] >= dto.EndDate.Minute)
            {
                endMinute = minutes[i + 1];
            }
        }

        if (startMinute == 60)
        {
            dto.StartDate = dto.StartDate.AddMinutes(-dto.StartDate.Minute);
            dto.StartDate = dto.StartDate.AddHours(1);
        }
        else
        {
            dto.StartDate = dto.StartDate.AddMinutes(-dto.StartDate.Minute);
            dto.StartDate = dto.StartDate.AddMinutes(startMinute);
        }

        if (endMinute == 60)
        {
            dto.EndDate = dto.EndDate.AddMinutes(-dto.EndDate.Minute);
            dto.EndDate = dto.EndDate.AddHours(1);
        }
        else
        {
            dto.EndDate = dto.EndDate.AddMinutes(-dto.EndDate.Minute);
            dto.EndDate = dto.EndDate.AddMinutes(endMinute);
        }

        _eventsHub.Clients.Group(idCompanies).SendAsync(WebSocketActions.MESSAGE_UPDATED, dto);
        return _useCaseUpdateEvents.Execute(dto);
    }

    
    [HttpDelete]
    [Route("delete/{idEventsEmployee}/{idCompanies}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<bool> Delete(string idEventsEmployee, string idCompanies)
    {
        _eventsHub.Clients.Group(idCompanies).SendAsync(WebSocketActions.MESSAGE_DELETED, idEventsEmployee);
        return _useCaseDeleteEvents.Execute(idEventsEmployee);
    }

    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fetchByEmployee/{id:int}")]
    [Authorize(Policy = "all")]
    public IEnumerable<DtoOutputEvents> FetchByEmployee(int id)
    {
        return _useCaseFetchEventsByEmployee.Execute(id);
    }
}