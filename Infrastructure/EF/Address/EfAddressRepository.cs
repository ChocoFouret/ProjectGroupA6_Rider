using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfAddressRepository : IAddressRepository
{
    private readonly PlanitContextProvider _planitContextProvider;
    
    public EfAddressRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<Address> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Address.ToList<Address>();
    }

    public Address FetchById(int idAddress)
    {
        using var context = _planitContextProvider.NewContext();
        var address = context.Address.FirstOrDefault(address => address.IdAddress == idAddress);

        if (address == null)
            throw new KeyNotFoundException($"Address with {idAddress} has not been found");

        return address;
    }

    public IEnumerable<Address> FetchByPostCode(string postCode)
    {
        using var context = _planitContextProvider.NewContext();
        var address = context.Address.Where(address => address.PostCode.Equals(postCode));
        return address;
    }

    public Address Create(Address address)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Address.Add(address);
            context.SaveChanges();
            return address;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    public bool Update(Address address)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Address.Update(address);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
}