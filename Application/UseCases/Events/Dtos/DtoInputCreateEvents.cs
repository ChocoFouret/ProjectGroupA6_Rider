using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Events.Dtos;

public class DtoInputCreateEvents
{
    [Required] public Domain.Events events { get; set; }
}