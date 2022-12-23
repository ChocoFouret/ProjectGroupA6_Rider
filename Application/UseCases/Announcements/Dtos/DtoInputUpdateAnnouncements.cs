using System.ComponentModel.DataAnnotations;
using Domain;

public class DtoInputUpdateAnnouncements
{
    [Required] public Announcements Announcements { get; set; }
}