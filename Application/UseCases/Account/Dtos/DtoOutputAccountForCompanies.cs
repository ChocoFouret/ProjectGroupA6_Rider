namespace Application.UseCases.Accounts.Dtos;

// Data
// Transfer
// Object
public class DtoOutputAccountForCompanies
{
    // So this dto is only used for the creation of a calendar (with the data that are useful for it)
    public int IdAccount { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int IdAddress { get; set; }
    public string Phone { get; set; }
}