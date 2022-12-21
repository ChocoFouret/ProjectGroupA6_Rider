namespace Infrastructure.Ef.DbEntities;

public class DbAddress
{
    public int IdAddress { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
}