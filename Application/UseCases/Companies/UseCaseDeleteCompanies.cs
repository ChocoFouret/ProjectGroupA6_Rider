using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseDeleteCompanies
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseDeleteCompanies(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public Boolean Execute(int id)
    {
        var companies = _companiesRepository.FetchById(id);
        return _companiesRepository.Delete(companies);
    }
}