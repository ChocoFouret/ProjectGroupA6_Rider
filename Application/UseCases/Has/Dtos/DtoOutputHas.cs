using Application.UseCases.Accounts.Dtos;

namespace Service.UseCases.Has.Dtos;

public class DtoOutputHas
{
    public int IdCompanies { get; set; }
    public int IdAccount { get; set; }
    public int IdFunctions { get; set; }
    public int IdHas { get; set; }
    public DtoOutputAccountForCompanies Account { get; set; }
    public DtoOutputFunction Function { get; set; }
}