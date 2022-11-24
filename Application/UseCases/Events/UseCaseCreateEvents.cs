using Application;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseCreateEvents : IUseCaseWriter<DtoOutputEvents, DtoInputCreateEvents>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseCreateEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    public DtoOutputEvents Execute(DtoInputCreateEvents input)
    {
        Events newEvent = new Events();
        
        newEvent.IdEventsEmployee = input.IdEventsEmployee;
        newEvent.IdSchedule = input.IdSchedule;
        newEvent.IdAccount = input.IdAccount;
        newEvent.StartDate = input.StartDate;
        newEvent.EndDate = input.EndDate;

        var events = _eventsRepository.Create(newEvent);

        events.IdHolidays = null;
        events.IdAbsents = null;
        events.IdWork = null;

        return Mapper.GetInstance().Map<DtoOutputEvents>(events);
    }
}