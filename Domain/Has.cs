using Microsoft.EntityFrameworkCore;

namespace Domain;

[Keyless]
public class Has
{
    public int IdCompanies { get; set; }
    public int IdAccount { get; set; }
    public int IdFunctions { get; set; }
    public int IdHas { get; set; }
    public Account? Account { get; set; }
    public Function? Function { get; set; }
}