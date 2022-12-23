using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Companies.Dtos;

public class DtoInputUpdateCompanies
{
    [Required] public int IdCompanies { get; set; }
    [Required] public string CompaniesName { get; set; }
    [Required] public string DirectorEmail { get; set; }
}