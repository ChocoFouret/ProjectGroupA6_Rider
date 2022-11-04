using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAccountById : IUseCaseParameterizedQuery<DtoOutputAccount, int>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchAccountById(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputAccount Execute(int id)
    {
        var dbAccount = _accountRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputAccount>(dbAccount);
    }
}