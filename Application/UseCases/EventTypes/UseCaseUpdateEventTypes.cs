using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseUpdateEventTypes : IUseCaseWriter<Boolean, DtoInputUpdateEventTypes>
{
    private readonly IEventTypesRepository _eventTypesRepository;

    public UseCaseUpdateEventTypes(IEventTypesRepository eventTypesRepository)
    {
        _eventTypesRepository = eventTypesRepository;
    }

    public Boolean Execute(DtoInputUpdateEventTypes input)
    {
        var eventTypes = _eventTypesRepository.FetchByTypes(input.Types);

        eventTypes.BarColor = input.BarColor;
        
        return _eventTypesRepository.Update(eventTypes);
    }
}