using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Events.Dtos;

public class DtoInputDateEvents
{
    public int? IdAccount { get; set; }
   [Required] public int IdCompanies { get; set; }
   [Required] public DateTime From { get; set; }
    [Required] public DateTime To { get; set; }
}