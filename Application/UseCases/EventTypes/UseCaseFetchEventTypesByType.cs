using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchEventTypesByType : IUseCaseParameterizedQuery<DtoOutputEventTypes, string>
{
    private readonly IEventTypesRepository _eventTypesRepository;

    public UseCaseFetchEventTypesByType(IEventTypesRepository eventTypesRepository)
    {
        _eventTypesRepository = eventTypesRepository;
    }

    public DtoOutputEventTypes Execute(string type)
    {
        var dbEventTypes = _eventTypesRepository.FetchByTypes(type);
        return Mapper.GetInstance().Map<DtoOutputEventTypes>(dbEventTypes);
    }
}