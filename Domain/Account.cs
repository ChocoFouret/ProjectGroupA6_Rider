﻿namespace Domain;

public class Account
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Street {get; set;}
    public string Number {get; set;}
    public int PostCode {get; set;}
    public string City {get; set;}
    public bool IsChief {get; set;}
    public string PictureURL {get; set;}
    public int idFunction { get; set; } = 1;
}