using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Accounts;

public class UseCaseDeleteEvents : IUseCaseWriter<Boolean, string>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseDeleteEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }
    
    public Boolean Execute(string IdEventsEmployee)
    {
        var events = _eventsRepository.FetchById(IdEventsEmployee);
        return _eventsRepository.Delete(events);
    }
}