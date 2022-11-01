using Application;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;

public class UseCaseFetchAllFunctions : IUseCaseQuery<IEnumerable<DtoOutputFunction>>
{
    private readonly IFunctionRepository _functionRepository;

    public UseCaseFetchAllFunctions(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public IEnumerable<DtoOutputFunction> Execute()
    {
        var dbFunctions = _functionRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputFunction>>(dbFunctions);
    }
}