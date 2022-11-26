using System.Security.Cryptography;

namespace Domain;

public interface IEventTypesRepository
{
    IEnumerable<EventTypes> FetchAll();
    EventTypes FetchByTypes(string type);
    EventTypes Create(EventTypes eventTypes);
    bool Update(EventTypes eventTypes);
    bool Delete(EventTypes eventTypes);
}