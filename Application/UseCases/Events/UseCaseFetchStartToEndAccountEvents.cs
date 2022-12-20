using System.Data;
using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class UseCaseFetchStartToEndAccountEvents : IUseCaseParameterizedQuery<IEnumerable<DtoOutputEvents>, DtoInputDateEvents>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseFetchStartToEndAccountEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }
    
    public IEnumerable<DtoOutputEvents> Execute(DtoInputDateEvents date)
    {
        var dbEvents = _eventsRepository.FetchFromToAccount(date.IdCompanies, date.From, date.To, date.IdAccount);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEvents>>(dbEvents);
    }
}