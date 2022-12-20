using System.ComponentModel.DataAnnotations;
namespace Application.UseCases.Companies.Dtos;

public class DtoInputCreateCompanies
{
    [Required] public Domain.Companies companie { get; set; }
}