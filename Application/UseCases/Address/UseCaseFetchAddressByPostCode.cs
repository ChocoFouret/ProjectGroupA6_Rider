using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Addresss;

public class UseCaseFetchAddressByPostCode : IUseCaseParameterizedQuery<Address, string>
{
    private readonly IAddressRepository _addressRepository;

    public UseCaseFetchAddressByPostCode(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public Address Execute(string postCode)
    {
        var dbAddress = _addressRepository.FetchByPostCode(postCode);
        return Mapper.GetInstance().Map<Address>(dbAddress);
    }
}