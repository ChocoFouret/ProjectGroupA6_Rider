using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseDeleteAccount : IUseCaseWriter<Boolean, int>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseDeleteAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    
    // Call the method into EfAccountRepesitory
    public Boolean Execute(int id)
    {
        var account = _accountRepository.FetchById(id);
        return _accountRepository.Delete(account);
    }
}