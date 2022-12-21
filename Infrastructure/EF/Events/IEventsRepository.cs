namespace Domain;

public interface IEventsRepository
{
    IEnumerable<Events> FetchAll();
    Events FetchById(string idEvents);
    IEnumerable<Events> FetchFromTo(int idCompany, DateTime from, DateTime to);
    IEnumerable<Events> FetchFromToAccount(int idCompany, DateTime from, DateTime to, int? idAccount);
    Events Create(Events events);
    bool Update(Events events);
    bool Delete(Events events);
    IEnumerable<Events> FetchEventsByEmployee(Account account);
}