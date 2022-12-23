namespace Domain;

public interface IAddressRepository
{
    IEnumerable<Address> FetchAll();
    Address FetchById(int idAddress);
    IEnumerable<Address> FetchByPostCode(string postCode);
    Address Create(Address address);
    bool Update(Address address);
}