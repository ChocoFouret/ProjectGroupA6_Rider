using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputLoginAccount
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}