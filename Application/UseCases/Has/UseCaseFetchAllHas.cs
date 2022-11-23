using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Application.UseCases.Has;

public class UseCaseFetchAllHas
{
    private readonly IHasRepository _hasRepository;

    public UseCaseFetchAllHas(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    public IEnumerable<DtoOutputHas> Execute()
    {
        var dbHas = _hasRepository.FetchAll();
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputHas>>(dbHas);
    }
}