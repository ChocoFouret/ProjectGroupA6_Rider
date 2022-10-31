using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UseCaseCreateAccount _useCaseCreateAccount;
    private readonly UseCaseFetchAllAccounts _useCaseFetchAllAccounts;
    private readonly UseCaseFetchAccountById _useCaseFetchAccountById;

    public AccountController(UseCaseCreateAccount useCaseCreateAccount, UseCaseFetchAllAccounts useCaseFetchAllAccounts,
        UseCaseFetchAccountById useCaseFetchAccountById)
    {
        _useCaseCreateAccount = useCaseCreateAccount;
        _useCaseFetchAllAccounts = useCaseFetchAllAccounts;
        _useCaseFetchAccountById = useCaseFetchAccountById;
    }

    [HttpGet]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputAccount> FetchById(int id)
    {
        try
        {
            return _useCaseFetchAccountById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [Route("/account/create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<DtoOutputAccount> Create(DtoInputCreateAccount dto)
    {
        // Use for add new account easily
        if (dto.account.idFunction == 0) dto.account.idFunction = 1;
        var output = _useCaseCreateAccount.Execute(dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.Id },
            output
        );
    }
    
    /*
    [HttpPost]
    [Route("/account/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Account> Login(int idAccount, string password)
    {
        if (_accountRepository.Login(idAccount, password)) return Ok();
        return Unauthorized();
    }
    */
    
    /*
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id)
    {
        return _accountRepository.Delete(id) ? NoContent() : NotFound();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Update(Account account)
    {
        return _accountRepository.Update(account) ? NoContent() : NotFound();
    }
    */
}