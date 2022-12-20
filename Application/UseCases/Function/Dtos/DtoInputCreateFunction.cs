using System.ComponentModel.DataAnnotations;
using Domain;

public class DtoInputCreateFunction
{
    [Required] public string title { get; set; }
}