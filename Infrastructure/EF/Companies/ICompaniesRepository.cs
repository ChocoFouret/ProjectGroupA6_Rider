namespace Infrastructure.EF.Companies;
using Domain;

public interface ICompaniesRepository
{
    IEnumerable<Companies> FetchAll();
    Companies FetchById(int id);
    Companies FetchByName(string name);
    Companies Create(Companies companie);
    bool Update(Companies companies);
    bool Delete(Companies companies);
}