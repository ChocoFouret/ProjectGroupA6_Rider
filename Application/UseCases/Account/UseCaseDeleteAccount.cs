using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseDeleteAccount : IUseCaseWriter<Boolean, String>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseDeleteAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Boolean Execute(string email)
    {
        var account = _accountRepository.FetchByEmail(email);
        return _accountRepository.Delete(account);
    }
}