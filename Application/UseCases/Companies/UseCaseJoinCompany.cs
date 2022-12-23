using Application.UseCases.Companies.Dtos;
using Application.UseCases.Utils;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseJoinCompany:IUseCaseWriter<Boolean, DtoInputJoinCompanie>
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseJoinCompany(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public Boolean Execute(DtoInputJoinCompanie input)
    {
        return _companiesRepository.Join(input.name, input.password);
    }
}