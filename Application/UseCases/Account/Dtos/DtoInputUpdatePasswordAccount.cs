using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdatePasswordAccount
{
   [Required] public int Id { get; set; }
}