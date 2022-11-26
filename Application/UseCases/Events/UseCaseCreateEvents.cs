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
        var events = _eventsRepository.Create(input.Events);
        return Mapper.GetInstance().Map<DtoOutputEvents>(events);
    }
}