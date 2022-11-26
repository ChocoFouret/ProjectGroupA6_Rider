using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseUpdateEvents : IUseCaseWriter<Boolean, DtoInputUpdateEvents>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IEventTypesRepository _eventTypesRepository;

    public UseCaseUpdateEvents(IEventsRepository eventsRepository, IEventTypesRepository eventTypesRepository)
    {
        _eventsRepository = eventsRepository;
        _eventTypesRepository = eventTypesRepository;
    }
    
    public Boolean Execute(DtoInputUpdateEvents input)
    {
        var events = _eventsRepository.FetchById(input.IdEventsEmployee);
        events.StartDate = input.StartDate;
        events.EndDate = input.EndDate;
        events.IdAccount = input.IdAccount;
        events.EventTypes = null;
        events.Types = input.Types;
        events.IsValid = input.IsValid;
        events.Comments = input.Comments;
        return _eventsRepository.Update(events);
    }
}