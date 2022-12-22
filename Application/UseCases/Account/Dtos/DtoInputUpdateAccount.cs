using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdateAccount
{
    // To add information to be updated, also add in UseCaseUpdateAccount.cs
    [Required] public int IdAccount { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Phone { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string FirstName { get; set; }
    public bool? IsAdmin { get; set; }
    public string? PictureURL {get; set;}
}