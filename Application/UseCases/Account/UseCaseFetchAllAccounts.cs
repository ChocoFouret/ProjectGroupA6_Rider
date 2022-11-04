using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAllAccounts : IUseCaseQuery<IEnumerable<DtoOutputAccount>>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchAllAccounts(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public IEnumerable<DtoOutputAccount> Execute()
    {
        var dbAccounts = _accountRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAccount>>(dbAccounts);
    }
}