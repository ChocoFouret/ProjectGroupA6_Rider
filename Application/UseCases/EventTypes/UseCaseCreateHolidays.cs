using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseCreateEventTypes : IUseCaseWriter<DtoOutputEventTypes, DtoInputCreateEventTypes>
{
    private readonly IEventTypesRepository _eventTypesRepository;

    public UseCaseCreateEventTypes(IEventTypesRepository eventTypesRepository)
    {
        _eventTypesRepository = eventTypesRepository;
    }

    public DtoOutputEventTypes Execute(DtoInputCreateEventTypes input)
    {
        EventTypes newEventTypes = new EventTypes();
        
        var eventTypes = _eventTypesRepository.Create(newEventTypes);

        return Mapper.GetInstance().Map<DtoOutputEventTypes>(eventTypes);
    }
}