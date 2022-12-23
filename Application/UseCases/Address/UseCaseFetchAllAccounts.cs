using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

namespace Application.UseCases.Accounts;

public class UseCaseFetchAllAddress : IUseCaseQuery<IEnumerable<DtoOutputAddress>>
{
    private readonly IAddressRepository _addressRepository;

    public UseCaseFetchAllAddress(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public IEnumerable<DtoOutputAddress> Execute()
    {
        var dbAddress = _addressRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAddress>>(dbAddress);
    }
}