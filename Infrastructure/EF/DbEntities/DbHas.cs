using System.ComponentModel.DataAnnotations.Schema;
using Domain;

namespace Infrastructure.Ef.DbEntities;

public class DbHas
{
    public int IdCompanies { get; set; }
    public int IdAccount { get; set; }
    public int IdFunctions { get; set; }
    public int IdHas { get; set; }
}