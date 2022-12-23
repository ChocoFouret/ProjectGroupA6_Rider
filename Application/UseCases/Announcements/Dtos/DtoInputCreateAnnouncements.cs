using System.ComponentModel.DataAnnotations;
using Domain;

public class DtoInputCreateAnnouncements
{
    [Required] public Announcements Announcements { get; set; }
}