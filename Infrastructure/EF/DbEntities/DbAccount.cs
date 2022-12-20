namespace Infrastructure.Ef.DbEntities;

public class DbAccount
{
    public int IdAccount { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

    public int IdAddress { get; set; }
    
    public bool IsAdmin {get; set;}
    public string PictureURL {get; set;}
    public string Phone { get; set; }
}