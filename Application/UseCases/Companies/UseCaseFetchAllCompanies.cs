using Application.UseCases.Utils;
using Infrastructure.EF.Companies;

namespace Application.UseCases.Companies.Dtos;

public class UseCaseFetchAllCompanies: IUseCaseQuery<IEnumerable<DtoOutputCompanies>>
{
    private readonly ICompaniesRepository _companieRepository;

    public UseCaseFetchAllCompanies(ICompaniesRepository companieRepository)
    {
        _companieRepository = companieRepository;
    }
    
    public IEnumerable<DtoOutputCompanies> Execute()
    {
        var dbCompanies = _companieRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputCompanies>>(dbCompanies);
    }
}