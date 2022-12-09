using System.Data;
using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class UseCaseFetchFromToEvents : IUseCaseParameterizedQuery<IEnumerable<DtoOutputEvents>, DtoInputDateEvents>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseFetchFromToEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    // Call the method into EfAccountRepesitory
    public IEnumerable<DtoOutputEvents> Execute(DtoInputDateEvents date)
    {
        var dbEvents = _eventsRepository.FetchFromTo(date.IdCompanies, date.From, date.To);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEvents>>(dbEvents);
    }
}