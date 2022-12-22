namespace Infrastructure.Ef.DbEntities;

public class DbAnnouncements
{
    public int IdAnnouncements { get; set; }
    public int IdCompanies { get; set; }
    public int IdFunctions { get; set; }
    public string Content { get; set; }
}