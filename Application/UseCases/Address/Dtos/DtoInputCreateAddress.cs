using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputCreateAddress
{
    [Required] public Address address { get; set; }
}