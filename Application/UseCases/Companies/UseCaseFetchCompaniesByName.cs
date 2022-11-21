using Application;
using Application.UseCases.Companies.Dtos;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseFetchCompaniesByName
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseFetchCompaniesByName(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public DtoOutputCompanies Execute(string name)
    {
        var dbCompanies = _companiesRepository.FetchByName(name);
        return Mapper.GetInstance().Map<DtoOutputCompanies>(dbCompanies);
    }
}