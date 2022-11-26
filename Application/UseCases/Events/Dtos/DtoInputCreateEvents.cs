using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Events.Dtos;

public class DtoInputCreateEvents
{
    [Required] public Domain.Events Events { get; set; }
    /*
    public int IdEventsEmployee { get; set; }
    public int IdSchedule { get; set; }
    public int IdAccount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsValid { get; set; }
    public string? Comments { get; set; }
    public string Types { get; set; }
    */
}