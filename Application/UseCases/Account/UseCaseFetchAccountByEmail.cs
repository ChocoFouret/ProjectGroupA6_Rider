using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAccountByEmail : IUseCaseParameterizedQuery<Account, string>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchAccountByEmail(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public Account Execute(string email)
    {
        var dbAccount = _accountRepository.FetchByEmail(email);
        return Mapper.GetInstance().Map<Account>(dbAccount);
    }
}