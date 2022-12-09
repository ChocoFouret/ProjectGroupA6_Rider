
using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchFunctionById : IUseCaseParameterizedQuery<DtoOutputFunction, int>
{
    private readonly IFunctionRepository _functionRepository;

    public UseCaseFetchFunctionById(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public DtoOutputFunction Execute(int id)
    {
        var dbFunction = _functionRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputFunction>(dbFunction);
    }
}