using System.Data;
using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class UseCaseFetchFromToAccountEvents : IUseCaseParameterizedQuery<IEnumerable<DtoOutputEvents>, DtoInputDateEvents>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseFetchFromToAccountEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    // Call the method into EfAccountRepesitory
    public IEnumerable<DtoOutputEvents> Execute(DtoInputDateEvents date)
    {
        var dbEvents = _eventsRepository.FetchFromToAccount(date.IdSchedule, date.From, date.To, date.IdAccount);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEvents>>(dbEvents);
    }
}