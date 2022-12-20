using Application.UseCases.Companies;
using Application.UseCases.Companies.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Companies;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CompaniesController : Controller
{
    private readonly UseCaseFetchAllCompanies _useCaseFetchAllCompanies;

    private readonly UseCaseFetchCompaniesById _useCaseFetchCompaniesById;

    private readonly UseCaseFetchCompaniesByName _useCaseFetchCompaniesByName;

    private readonly UseCaseCreateCompanies _useCaseCreateCompanies;

    private readonly UseCaseUpdateCompanies _useCaseUpdateCompanies;

    private readonly UseCaseDeleteCompanies _useCaseDeleteCompanies;

    private readonly UseCaseFetchCompaniesByEmail _useCaseFetchCompaniesByEmail;
    // GET

    public CompaniesController(
        UseCaseFetchAllCompanies useCaseFetchAllCompanies,
        UseCaseFetchCompaniesById useCaseFetchCompaniesById,
        UseCaseFetchCompaniesByName useCaseFetchCompaniesByName,
        UseCaseCreateCompanies useCaseCreateCompanies,
        UseCaseUpdateCompanies useCaseUpdateCompanies,
        UseCaseDeleteCompanies useCaseDeleteCompanies,
        UseCaseFetchCompaniesByEmail useCaseFetchCompaniesByEmail)
    {
        _useCaseFetchAllCompanies = useCaseFetchAllCompanies;
        _useCaseFetchCompaniesById = useCaseFetchCompaniesById;
        _useCaseFetchCompaniesByName = useCaseFetchCompaniesByName;
        _useCaseCreateCompanies = useCaseCreateCompanies;
        _useCaseUpdateCompanies = useCaseUpdateCompanies;
        _useCaseDeleteCompanies = useCaseDeleteCompanies;
        _useCaseFetchCompaniesByEmail = useCaseFetchCompaniesByEmail;

    }
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputCompanies> FetchAll()
    {
        return _useCaseFetchAllCompanies.Execute();
    }
    
    [HttpGet]
    [Route("fetchById/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputCompanies> FetchById(int id)
    {
        try
        {
            return _useCaseFetchCompaniesById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    [Route("fetchByEmail/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Companies FetchByEmail(string email)
    {
        try
        {
            return _useCaseFetchCompaniesByEmail.Execute(email);
        }
        catch (KeyNotFoundException e)
        {
            return null;
        }
    }
    
    [HttpGet]
    [Route("fetchByName/{name}")]
    public IEnumerable<DtoOutputCompanies> FetchByName(string name)
    {
        return _useCaseFetchCompaniesByName.Execute(name);
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputCompanies> Create(DtoInputCreateCompanies dto)
    {
        var output = _useCaseCreateCompanies.Execute(dto);

        if (output == null) return Conflict(new Companies());

        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdCompanies },
            output
        );
    }
    
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateCompanies dto)
    {
        return _useCaseUpdateCompanies.Execute(dto);
    }
    
    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteCompanies.Execute(id);
    }
}