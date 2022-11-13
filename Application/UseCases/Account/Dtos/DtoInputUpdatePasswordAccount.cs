using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoInputUpdatePasswordAccount
{
    // public Account account { get; set; }
    
    // To add information to be updated, also add in UseCaseUpdateAccount.cs
    public int Id { get; set; }
}