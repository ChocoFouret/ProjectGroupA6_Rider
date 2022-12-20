using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputCreateAccount
{
    [Required] public Account account { get; set; }
}