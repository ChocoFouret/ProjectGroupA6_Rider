using Application.UseCases.Utils;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseFetchCompaniesByEmail : IUseCaseParameterizedQuery<Domain.Companies, string>
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