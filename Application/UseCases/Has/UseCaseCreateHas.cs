using Application.UseCases.Utils;
using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Application.UseCases.Has;

public class UseCaseCreateHas : IUseCaseWriter<DtoOutputHas, DtoInputCreateHas>
{
   private readonly IHasRepository _hasRepository;

    public UseCaseCreateHas(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public DtoOutputHas Execute(DtoInputCreateHas input)
    {
        var dbHas = _hasRepository.Create(input.has);
        return Mapper.GetInstance().Map<DtoOutputHas>(dbHas);
    }
}