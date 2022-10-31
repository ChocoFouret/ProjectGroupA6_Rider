using Application.UseCases.Utils;
using Domain;
using Infrastructure.EF;

namespace Application.UseCases.Functions;

public class UseCaseCreateFunction : IUseCaseWriter<DtoOutputFunction, DtoInputCreateFunction>
{
    private readonly IFunctionRepository _functionRepository;

    public UseCaseCreateFunction(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public DtoOutputFunction Execute(DtoInputCreateFunction input)
    {
        var dbFunction = _functionRepository.Create(input.title);
        return Mapper.GetInstance().Map<DtoOutputFunction>(dbFunction);
    }
}