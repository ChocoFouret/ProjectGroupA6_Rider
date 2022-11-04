using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAccountByEmail : IUseCaseParameterizedQuery<DtoOutputAccount, string>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchAccountByEmail(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputAccount Execute(string email)
    {
        var dbAccount = _accountRepository.FetchByEmail(email);
        return Mapper.GetInstance().Map<DtoOutputAccount>(dbAccount);
    }
}