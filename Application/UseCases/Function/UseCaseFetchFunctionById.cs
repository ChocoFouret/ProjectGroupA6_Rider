
using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchFunctionByTitle : IUseCaseParameterizedQuery<DtoOutputFunction, string>
{
    private readonly IFunctionRepository _functionRepository;

    public UseCaseFetchFunctionByTitle(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public DtoOutputFunction Execute(string title)
    {
        var dbFunction = _functionRepository.FetchByTitle(title);
        return Mapper.GetInstance().Map<DtoOutputFunction>(dbFunction);
    }
}