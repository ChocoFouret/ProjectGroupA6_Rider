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

    // Call the method into EfAccountRepesitory
    public DtoOutputEvents Execute(DtoInputCreateEvents input)
    {
        var events = _eventsRepository.Create(input.events);

        events.IdHolidays = null;
        events.IdAbsents = null;
        events.IdWork = null;

        return Mapper.GetInstance().Map<DtoOutputEvents>(events);
    }
}