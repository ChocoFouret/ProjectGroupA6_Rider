using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseDeleteEvents : IUseCaseWriter<Boolean, int>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseDeleteEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }
    
    public Boolean Execute(int IdEventsEmployee)
    {
        var events = _eventsRepository.FetchById(IdEventsEmployee);
        return _eventsRepository.Delete(events);
    }
}