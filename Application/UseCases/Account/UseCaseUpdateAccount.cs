using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseUpdateAccount : IUseCaseWriter<Boolean, DtoInputUpdateAccount>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseUpdateAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Boolean Execute(DtoInputUpdateAccount input)
    {
        var account = _accountRepository.FetchByEmail(input.account.Email);
        if (account == null) return false;

        input.account.Id = account.Id;

        var tmp = input.account.PasswordHash;
        input.account.PasswordHash = EncryptPassword.HashPassword(tmp);
        
        return _accountRepository.Update(input.account);
    }
}