using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FunctionController : ControllerBase
{
    private readonly IFunctionRepository _functionRepository;

    public FunctionController(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    [HttpGet]
    public IEnumerable<Function> GetAll()
    {
        return _functionRepository.GetAll();
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Function> GetById(int id)
    {
        try
        {
            return Ok(_functionRepository.GetById(id));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Function> Create(Function function)
    {
        return StatusCode(201, _functionRepository.Create(function));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id)
    {
        return _functionRepository.Delete(id) ? NoContent() : NotFound();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Update(Function function)
    {
        return _functionRepository.Update(function) ? NoContent() : NotFound();
    }
}