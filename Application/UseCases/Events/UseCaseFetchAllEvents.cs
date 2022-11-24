using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class UseCaseFetchAllEvents : IUseCaseQuery<IEnumerable<DtoOutputEvents>>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseFetchAllEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    // Call the method into EfAccountRepesitory
    public IEnumerable<DtoOutputEvents> Execute()
    {
        var dbEvents = _eventsRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEvents>>(dbEvents);
    }
}