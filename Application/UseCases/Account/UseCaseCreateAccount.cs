using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseCreateAccount : IUseCaseWriter<DtoOutputAccount, DtoInputCreateAccount>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseCreateAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public DtoOutputAccount Execute(DtoInputCreateAccount input)
    {
        var dbAccount = _accountRepository.Create(input.account);
        return Mapper.GetInstance().Map<DtoOutputAccount>(dbAccount);
    }
}