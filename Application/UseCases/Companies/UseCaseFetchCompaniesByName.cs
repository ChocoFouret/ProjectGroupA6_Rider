using Application;
using Application.UseCases.Companies.Dtos;
using Application.UseCases.Utils;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseFetchCompaniesByName: IUseCaseParameterizedQuery<IEnumerable<DtoOutputCompanies>, string>
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseFetchCompaniesByName(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public IEnumerable<DtoOutputCompanies> Execute(string name)
    {
        var dbCompanies = _companiesRepository.FetchByName(name);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputCompanies>>(dbCompanies);
    }
}