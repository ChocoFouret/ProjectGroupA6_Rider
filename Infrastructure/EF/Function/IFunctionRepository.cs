namespace Domain;

public interface IFunctionRepository
{
    IEnumerable<Function> FetchAll();
    Function Create(string title);
    Function FetchByTitle(string Function);
}