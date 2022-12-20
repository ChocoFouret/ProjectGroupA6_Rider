using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAddressById : IUseCaseParameterizedQuery<DtoOutputAddress, int>
{
    private readonly IAddressRepository _addressRepository;

    public UseCaseFetchAddressById(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public DtoOutputAddress Execute(int id)
    {
        var dbAddress = _addressRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputAddress>(dbAddress);
    }
}