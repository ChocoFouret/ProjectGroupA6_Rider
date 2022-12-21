using Application;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Companies.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF.Companies;

namespace Service.UseCases.Companies;

public class UseCaseCreateCompanies: IUseCaseWriter<DtoOutputCompanies, DtoInputCreateCompanies>
{
    private readonly ICompaniesRepository _companiesRepository;

    public UseCaseCreateCompanies(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputCompanies Execute(DtoInputCreateCompanies input)
    {
        var tmp = input.companie.Password;
        input.companie.Password = EncryptPassword.HashPassword(tmp);
        
        var dbCompanies = _companiesRepository.Create(input.companie);
        return Mapper.GetInstance().Map<DtoOutputCompanies>(dbCompanies);
    }
}