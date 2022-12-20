using Application.UseCases.Has;
using Application.UseCases.Has.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Has;
using Service.UseCases.Has.Dtos;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HasController : Controller
{
    private readonly UseCaseFetchAllHas _useCaseFetchAllHas;
    private readonly UseCaseFetchHasByCompanies _useCaseFetchHasByCompanies;
    private readonly UseCaseFetchHasByAccount _useCaseFetchHasByAccount;
    private readonly UseCaseFetchHasByFunctions _useCaseFetchHasByFunctions;
    private readonly UseCaseCreateHas _useCaseCreateHas;
    private readonly UseCaseFetchHasById _useCaseFetchHasById;
    private readonly UseCaseDeleteHas _useCaseDeleteHas;

    public HasController(UseCaseFetchAllHas useCaseFetchAllHas,
        UseCaseFetchHasByCompanies useCaseFetchHasByCompanies,
        UseCaseFetchHasByAccount useCaseFetchHasByAccount,
        UseCaseFetchHasByFunctions useCaseFetchHasByFunctions,
        UseCaseCreateHas useCaseCreateHas,
        UseCaseFetchHasById useCaseFetchHasById,
        UseCaseDeleteHas useCaseDeleteHas)
    {
        _useCaseFetchAllHas = useCaseFetchAllHas;
        _useCaseFetchHasByCompanies = useCaseFetchHasByCompanies;
        _useCaseFetchHasByAccount = useCaseFetchHasByAccount;
        _useCaseFetchHasByFunctions = useCaseFetchHasByFunctions;
        _useCaseCreateHas = useCaseCreateHas;
        _useCaseFetchHasById = useCaseFetchHasById;
        _useCaseDeleteHas = useCaseDeleteHas;
    }
    
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputHas> FetchAll()
    {
        return _useCaseFetchAllHas.Execute();
    }
    
    [HttpGet]
    [Route("fetchById/{idHas:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputHas> FetchById(int idHas)
    {
        try
        {
            return _useCaseFetchHasById.Execute(idHas);
        }
        catch (KeyNotFoundException e)
        {
            return null;
        }
    }
    
    [HttpGet]
    [Route("fetchCompanies/{idCompanies:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdCompanies(int idCompanies)
    {
        return _useCaseFetchHasByCompanies.Execute(idCompanies);
    }
    
    [HttpGet]
    [Route("fetchAccount/{idAccount:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdAccount(int idAccount)
    {
        return _useCaseFetchHasByAccount.Execute(idAccount);
    }
    
    [HttpGet]
    [Route("fetchFunctions/{idFunction:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdFunction(int idFunction)
    {
        return _useCaseFetchHasByFunctions.Execute(idFunction);
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public String Create(DtoInputCreateHas dto)
    {
        var output = _useCaseCreateHas.Execute(dto);

        if (output == null) return "Not created";

        return "Created";
    }
    
    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteHas.Execute(id);
    }
    
}