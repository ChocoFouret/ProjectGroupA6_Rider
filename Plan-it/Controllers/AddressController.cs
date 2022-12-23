using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Address;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AddressController : ControllerBase
{
    //private readonly UseCaseFetchAllAddress _useCaseFetchAllAddress;
    private readonly UseCaseFetchAddressById _useCaseFetchAddressById;
    //private readonly UseCaseFetchAddressByPostCode _useCaseFetchAddressByPostCode;
    private readonly UseCaseCreateAddress _useCaseCreateAddress;
    private readonly UseCaseUpdateAddress _useCaseUpdateAddress;

    public AddressController(
       // UseCaseFetchAllAddress useCaseFetchAllAddress,
        UseCaseFetchAddressById useCaseFetchAddressById,
        //UseCaseFetchAddressByPostCode useCaseFetchAddressByPostCode,
        UseCaseCreateAddress useCaseCreateAddress,
        UseCaseUpdateAddress useCaseUpdateAddress
    )
    {
       // _useCaseFetchAllAddress = useCaseFetchAllAddress;
        _useCaseFetchAddressById = useCaseFetchAddressById;
        //_useCaseFetchAddressByPostCode = useCaseFetchAddressByPostCode;
        _useCaseCreateAddress = useCaseCreateAddress;
        _useCaseUpdateAddress = useCaseUpdateAddress;
    }
    /*
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputAddress> FetchAll()
    {
        return _useCaseFetchAllAddress.Execute();
    }*/
    
    //Use to find in the user's profile his address
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
    
    /*[HttpGet]
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
    }*/
    
    //Use to create an address with the profil
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAddress> Create(DtoInputCreateAddress dto)
    {
        var output = _useCaseCreateAddress.Execute(dto.address);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdAddress },
            output
        );
    }
    
    //We can change the address in the profil page
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAddress dto)
    {
        return _useCaseUpdateAddress.Execute(dto);
    }
    
}