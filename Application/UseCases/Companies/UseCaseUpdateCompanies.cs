using Application.UseCases.Companies.Dtos;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseUpdateCompanies
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseUpdateCompanies(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }
    
    public Boolean Execute(DtoInputUpdateCompanies input)
    {
        var companies = _companiesRepository.FetchById(input.IdCompanies);

        companies.CompaniesName = input.CompaniesName;
        companies.DirectorEmail = input.DirectorEmail;

        return _companiesRepository.Update(companies);
    }
}