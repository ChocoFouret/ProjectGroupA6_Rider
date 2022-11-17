﻿namespace Application.UseCases.Accounts.Dtos;

// Data
// Transfer
// Object
public class DtoOutputAccount
{
    // Returns the information you wish to display ONLY
    public int IdAccount { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int IdAddress { get; set; }
    public string PictureURL { get; set; }
}