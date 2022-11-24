using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

public class UseCaseUpdateEvents : IUseCaseWriter<Boolean, DtoInputUpdateEvents>
{
    private readonly IEventsRepository _eventsRepository;

    public UseCaseUpdateEvents(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    // Call the method into EfAccountRepesitory
    public Boolean Execute(DtoInputUpdateEvents input)
    {
        var events = _eventsRepository.FetchById(input.IdEventsEmployee);

        events.StartDate = input.StartDate;
        events.EndDate = input.EndDate;
        events.IdAccount = input.IdAccount;

        return _eventsRepository.Update(events);
    }
}