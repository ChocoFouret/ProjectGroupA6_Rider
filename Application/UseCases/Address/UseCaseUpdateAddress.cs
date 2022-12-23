using Application.UseCases.Accounts.Dtos;
using Domain;

namespace Service.UseCases.Address;

public class UseCaseUpdateAddress
{
    private readonly IAddressRepository _addressRepository;

    public UseCaseUpdateAddress(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    // Call the method into EfAccountRepesitory
    public Boolean Execute(DtoInputUpdateAddress input)
    {
        var address = _addressRepository.FetchById(input.address.IdAddress);

        address.City = input.address.City;
        address.Street = input.address.Street;

        address.Number = input.address.Number;
        address.PostCode = input.address.PostCode;

        return _addressRepository.Update(address);
    }
}