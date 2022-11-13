namespace Application.UseCases.Accounts.Dtos;

// Data
// Transfer
// Object
public class DtoOutputAccount
{
    // Returns the information you wish to display ONLY
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public int PostCode { get; set; }
    public string City { get; set; }
    public string PictureURL { get; set; }
    public string Function { get; set; }
}