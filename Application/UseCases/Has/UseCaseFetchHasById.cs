using Application;
using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Service.UseCases.Has;

public class UseCaseFetchHasById
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