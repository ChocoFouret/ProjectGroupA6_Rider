using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdateAccount
{
    // public Account account { get; set; }
    
    // To add information to be updated, also add in UseCaseUpdateAccount.cs
    public int Id { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Street {get; set;}
    public string Number {get; set;}
    public int PostCode {get; set;}
    public string City {get; set;}
    public bool IsChief {get; set;}
    public string PictureURL {get; set;}
    public string Function { get; set; }
    
}