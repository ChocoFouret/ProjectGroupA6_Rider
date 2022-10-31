namespace Domain;

public interface IFunctionRepository
{
    IEnumerable<Function> FetchAll();
    Function Create(string title);
    Function FetchById(int idFunction);
}