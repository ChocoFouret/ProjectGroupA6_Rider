using System.ComponentModel.DataAnnotations.Schema;
using Domain;

namespace Infrastructure.Ef.DbEntities;

public class DbEvents
{
    public string IdEventsEmployee { get; set; }
    public int IdCompanies { get; set; }
    public int IdAccount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsValid { get; set; }
    public string? Comments { get; set; }
    
    [ForeignKey("Types")]
    public string Types { get; set; }
    public EventTypes EventTypes { get; set; }
}