using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class UseCaseFetchAllEventType : IUseCaseQuery<IEnumerable<DtoOutputEventTypes>>
{
    private readonly IEventTypesRepository _eventTypesRepository;

    public UseCaseFetchAllEventType(IEventTypesRepository eventTypesRepository)
    {
        _eventTypesRepository = eventTypesRepository;
    }

    // Call the method into EfAccountRepesitory
    public IEnumerable<DtoOutputEventTypes> Execute()
    {
        var dbEvents = _eventTypesRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEventTypes>>(dbEvents);
    }
}