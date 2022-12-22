using Application;
using Application.UseCases.Companies.Dtos;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseFetchCompaniesByNameAndPassword
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseFetchCompaniesByNameAndPassword(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public IEnumerable<DtoOutputCompanies> Execute(string name, string password)
    {
        var dbCompanies = _companiesRepository.FetchByName(name);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputCompanies>>(dbCompanies);
    }
}