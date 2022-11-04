using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseLoginAccount : IUseCaseWriter<Boolean, DtoInputLoginAccount>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseLoginAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public Boolean Execute(DtoInputLoginAccount input)
    {
        return _accountRepository.Login(input.Email, input.Password);
    }
}