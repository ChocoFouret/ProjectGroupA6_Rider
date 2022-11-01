using Application.UseCases.Functions;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FunctionController : ControllerBase
{
    private readonly UseCaseCreateFunction _useCaseCreateFunction;
    private readonly UseCaseFetchAllFunctions _useCaseFetchAllFunctions;
    private readonly UseCaseFetchFunctionByTitle _useCaseFetchFunctionByTitle;
    
    public FunctionController(UseCaseCreateFunction useCaseCreateFunction, UseCaseFetchAllFunctions useCaseFetchAllFunctions,
        UseCaseFetchFunctionByTitle useCaseFetchFunctionByTitle)
    {
        _useCaseCreateFunction = useCaseCreateFunction;
        _useCaseFetchAllFunctions = useCaseFetchAllFunctions;
        _useCaseFetchFunctionByTitle = useCaseFetchFunctionByTitle;
    }

    [HttpGet]
    [Route("{title}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputFunction> FetchById(string title)
    {
        try
        {
            return _useCaseFetchFunctionByTitle.Execute(title);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    [Route("/function/create")]
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