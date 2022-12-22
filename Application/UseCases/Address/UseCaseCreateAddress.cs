using Application;
using Application.UseCases.Accounts.Dtos;
using Domain;

namespace Service.UseCases.Address;

public class UseCaseCreateAddress
{
    private readonly IAddressRepository _addressRepository;

    public UseCaseCreateAddress(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    // Call the method into EfAccountRepesitory
    public DtoOutputAddress Execute(Domain.Address input)
    {
    
        var dbAddress = _addressRepository.Create(input);
        return Mapper.GetInstance().Map<DtoOutputAddress>(dbAddress);
    }
}