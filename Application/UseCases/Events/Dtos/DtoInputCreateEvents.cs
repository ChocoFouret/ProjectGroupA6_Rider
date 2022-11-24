using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Events.Dtos;

public class DtoInputCreateEvents
{
    public int IdEventsEmployee { get; set; }
    public int IdSchedule { get; set; }
    public int IdAccount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? IdWork { get; set; }
    public int? IdAbsents { get; set; }
    public int? IdHolidays { get; set; }
}