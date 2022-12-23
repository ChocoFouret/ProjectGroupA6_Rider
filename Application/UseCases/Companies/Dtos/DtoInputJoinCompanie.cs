using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Companies.Dtos;

public class DtoInputJoinCompanie
{
    [Required] public string name { get; set; }
    [Required] public string password { get; set; }
}