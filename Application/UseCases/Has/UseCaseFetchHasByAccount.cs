using Application.UseCases.Utils;
using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Application.UseCases.Has.Dtos;

public class UseCaseFetchHasByAccount: IUseCaseParameterizedQuery<IEnumerable<DtoOutputHas>, int>
{
    private readonly IHasRepository _hasRepository;

    public UseCaseFetchHasByAccount(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public IEnumerable<DtoOutputHas> Execute(int id)
    {
        var dbHas = _hasRepository.FetchByAccount(id);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputHas>>(dbHas);
    }
}