using Application;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Companies.Dtos;
using Domain;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseCreateCompanies
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseCreateCompanies(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputCompanies Execute(DtoInputCreateCompanies input)
    {
        var companies = _companiesRepository.FetchByName(input.companie.CompaniesName);
        if (companies != null) return null;

        var dbCompanies = _companiesRepository.Create(input.companie);
        return Mapper.GetInstance().Map<DtoOutputCompanies>(dbCompanies);
    }
}