using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdateAccount
{
    // public Account account { get; set; }
    
    // To add information to be updated, also add in UseCaseUpdateAccount.cs
    public int IdAccount { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    
    public int IdAddress { get; set; }
    public bool IsAdmin {get; set;}
    public string PictureURL {get; set;}

}