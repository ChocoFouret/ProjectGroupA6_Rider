using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Application.UseCases.Has;

public class UseCaseFetchHasByCompanies
{
    private readonly IHasRepository _hasRepository;

    public UseCaseFetchHasByCompanies(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public IEnumerable<DtoOutputHas> Execute(int id)
    {
        var dbHas = _hasRepository.FetchByCompanies(id);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputHas>>(dbHas);
    }
}