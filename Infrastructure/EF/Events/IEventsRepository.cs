using System.Security.Cryptography;

namespace Domain;

public interface IEventsRepository
{
    IEnumerable<Events> FetchAll();
    Events FetchById(int idEvents);
    IEnumerable<Events> FetchFromTo(int idCompany, DateTime from, DateTime to);
    Events Create(Events events);
    bool Update(Events events);
    bool Delete(Events events);
}