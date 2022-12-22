using System.ComponentModel.DataAnnotations;

public class DtoInputUpdateEventTypes
{
    [Required] public string Types { get; set; }
   [Required] public string BarColor { get; set; }
}