using Application.UseCases.Companies.Dtos;
using Application.UseCases.Utils;
using Infrastructure.EF.Companies;


namespace Application.UseCases.Companies;

public class UseCaseFetchCompaniesById : IUseCaseParameterizedQuery<DtoOutputCompanies, int>
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseFetchCompaniesById(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public DtoOutputCompanies Execute(int id)
    {
        var dbCompanies = _companiesRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputCompanies>(dbCompanies);
    }
}