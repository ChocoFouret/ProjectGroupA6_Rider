using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class DtoInputUpdateEvents
{
   [Required] public string IdEventsEmployee { get; set; }
   [Required] public int IdAccount { get; set; }
   [Required] public DateTime StartDate { get; set; }
   [Required] public DateTime EndDate { get; set; }
   [Required] public bool IsValid { get; set; }
    public string? Comments { get; set; } 
    [Required] public string Types { get; set; }
    
    public EventTypes? EventTypes { get; set; }
}