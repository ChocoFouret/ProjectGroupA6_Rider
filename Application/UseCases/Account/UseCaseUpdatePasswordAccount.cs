using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Accounts;

public class UseCaseUpdatePasswordAccount : IUseCaseWriter<String, DtoInputUpdatePasswordAccount>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseUpdatePasswordAccount(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public String Execute(DtoInputUpdatePasswordAccount dto)
    {
        var account = _accountRepository.FetchById(dto.Id);

        var newPassword = EncryptPassword.GeneratePassword(10, 0);
        account.Password = EncryptPassword.HashPassword(newPassword);

        if (_accountRepository.Update(account))
        {
            return newPassword;
        }
        else
        {
            return "Password not updated";
        }
    }
}