using Application;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchEventsById : IUseCaseParameterizedQuery<DtoOutputEvents, int>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseFetchEventsById(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    public DtoOutputEvents Execute(int id)
    {
        var dbEvents = _eventsRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputEvents>(dbEvents);
    }
}