using Infrastructure.EF.Has;

namespace Application.UseCases.Has;

public class UseCaseDeleteHas
{
    private readonly IHasRepository _hasRepository;

    public UseCaseDeleteHas(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public Boolean Execute(int id)
    {
        var has = _hasRepository.FetchById(id);
        return _hasRepository.Delete(has);
    }
}