using System.ComponentModel.DataAnnotations;

namespace Service.UseCases.Has.Dtos;

public class DtoInputCreateHas
{
    [Required] public  Domain.Has has { get; set; }
}