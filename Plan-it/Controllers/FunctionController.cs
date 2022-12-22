using Application.UseCases.Functions;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FunctionController : ControllerBase
{
    private readonly UseCaseCreateFunction _useCaseCreateFunction;
    private readonly UseCaseFetchFunctionById _useCaseFetchFunctionById;
    
    public FunctionController(UseCaseCreateFunction useCaseCreateFunction,
        UseCaseFetchFunctionById useCaseFetchFunctionById)
    {
        _useCaseCreateFunction = useCaseCreateFunction;
        _useCaseFetchFunctionById = useCaseFetchFunctionById;
    }

    [HttpGet]
    [Route("fetchByTitle/{title}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputFunction> FetchById(int id)
    {
        try
        {
            return _useCaseFetchFunctionById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<DtoOutputFunction> Create(DtoInputCreateFunction dto)
    {
        // Use for add new function easily
        var output = _useCaseCreateFunction.Execute(dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.Id },
            output
        );
    }
}