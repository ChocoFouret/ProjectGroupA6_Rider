using System.ComponentModel.DataAnnotations;
using Domain;

public class DtoOutputAnnouncements
{
    public int IdAnnouncements { get; set; }
    public int IdCompanies { get; set; }
    public int IdFunctions { get; set; }
    public string Content { get; set; }
}