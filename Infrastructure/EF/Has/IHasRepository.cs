namespace Infrastructure.EF.Has;
using Domain;

public interface IHasRepository
{
    IEnumerable<Has> FetchAll();
    IEnumerable<Has> FetchByCompanies(int id);
    IEnumerable<Has> FetchByFunctions(int id);
    IEnumerable<Has> FetchByAccount(int id);
    Has Create(Has has);
    Has FetchById(int id);
    bool Delete(Has has);
}