using Application.UseCases.Companies.Dtos;
using Domain;

namespace Application.UseCases.Accounts.Dtos;

public class DtoOutputProfilAccount
{
    public string FirstName { get; set;}
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PictureURL { get; set; }
    public string Phone { get; set; }
    public string Function { get; set; }
    public int IdAddress { get; set; }
    public DtoOutputAddress Address { get; set; }
    public DtoOutputCompanies Companies { get; set; }
}