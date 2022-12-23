using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchProfilById : IUseCaseParameterizedQuery<DtoOutputProfilAccount, int>
{
    private readonly IAccountRepository _accountRepository;

    public UseCaseFetchProfilById(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputProfilAccount Execute(int id)
    {
        var dbAccount = _accountRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputProfilAccount>(dbAccount);
    }
}