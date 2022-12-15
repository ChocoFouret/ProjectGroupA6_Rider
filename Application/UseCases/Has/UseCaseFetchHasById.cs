using Application;
using Application.UseCases.Utils;
using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Service.UseCases.Has;

public class UseCaseFetchHasById : IUseCaseParameterizedQuery<DtoOutputHas, int>
{
    private readonly IHasRepository _hasRepository;

    public UseCaseFetchHasById(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public DtoOutputHas Execute(int id)
    {
        var dbHas = _hasRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputHas>(dbHas);
    }
}