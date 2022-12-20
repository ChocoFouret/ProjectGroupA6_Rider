using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Addresss;
using Domain;
using JWT.Models;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AddressController : ControllerBase
{
    private readonly UseCaseFetchAllAddress _useCaseFetchAllAddress;
    private readonly UseCaseFetchAddressById _useCaseFetchAddressById;
    private readonly UseCaseFetchAddressByPostCode _useCaseFetchAddressByPostCode;

    public AddressController(
        UseCaseFetchAllAddress useCaseFetchAllAddress,
        UseCaseFetchAddressById useCaseFetchAddressById,
        UseCaseFetchAddressByPostCode useCaseFetchAddressByPostCode
    )
    {
        _useCaseFetchAllAddress = useCaseFetchAllAddress;
        _useCaseFetchAddressById = useCaseFetchAddressById;
        _useCaseFetchAddressByPostCode = useCaseFetchAddressByPostCode;
    }
    
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputAddress> FetchAll()
    {
        return _useCaseFetchAllAddress.Execute();
    }
    
    [HttpGet]
    [Route("fetch/id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputAddress> FetchById(int id)
    {
        try
        {
            return _useCaseFetchAddressById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    [Route("fetch/postCode/{postCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Address> FetchByPostCode(string postCode)
    {
        try
        {
            return _useCaseFetchAddressByPostCode.Execute(postCode);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    /*
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAddress> Create(DtoInputCreateAddress dto)
    {
        var output = _useCaseCreateAddress.Execute(dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdAddress },
            output
        );
    }
    */
    
}