using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoOutputAddress
{
    [Required] public int IdAddress { get; set; }
    [Required] public string Street { get; set; }
    [Required] public string Number { get; set; }
    [Required] public string PostCode { get; set; }
    [Required] public string City { get; set; }
}