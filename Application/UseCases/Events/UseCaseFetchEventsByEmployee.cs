using Application;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchEventsByEmployee : IUseCaseParameterizedQuery< IEnumerable<DtoOutputEvents>, int>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchEventsByEmployee(IEventsRepository eventsRepository, IAccountRepository accountRepository)
    {
        _eventsRepository = eventsRepository;
        _accountRepository = accountRepository;
    }

    public  IEnumerable<DtoOutputEvents> Execute(int id)
    {
        var account = _accountRepository.FetchById(id);
        var dbEvents = _eventsRepository.FetchEventsByEmployee(account);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputEvents>>(dbEvents);
    }
}