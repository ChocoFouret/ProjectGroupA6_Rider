namespace Domain;

public interface IFunctionRepository
{
    IEnumerable<Function> GetAll();
    Function GetById(int idAccount);
    Function Create(Function account);
    Function Read(Function account);
    bool Update(Function account);
    bool Delete(int idAccount);

}