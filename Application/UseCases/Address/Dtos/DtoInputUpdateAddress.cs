using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdateAddress
{
    [Required] public Address address { get; set; }
}