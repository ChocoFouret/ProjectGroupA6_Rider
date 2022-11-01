using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseGetAccount : IUseCaseParameterizedQuery<Account, string>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseGetAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Account? Execute(string email)
    {
        return _accountRepository.GetAccount(email);
    }
}