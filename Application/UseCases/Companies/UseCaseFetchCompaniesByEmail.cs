using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseFetchCompaniesByEmail 
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseFetchCompaniesByEmail(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public Domain.Companies Execute(string email)
    {
        return _companiesRepository.FetchByEmail(email);
    }
}