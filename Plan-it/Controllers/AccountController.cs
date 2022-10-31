using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IEnumerable<Account> GetAll()
    {
        return _accountRepository.GetAll();
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Account> GetById(int id)
    {
        try
        {
            return Ok(_accountRepository.GetById(id));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [Route("/account/create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Account> Create(Account account)
    {
        // Use for add new user easily
        if (account.idFunction == 0) account.idFunction = 1;
        return StatusCode(201, _accountRepository.Create(account));
    }
    
    [HttpPost]
    [Route("/account/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Account> Login(int idAccount, string password)
    {
        if (_accountRepository.Login(idAccount, password)) return Ok();
        return Unauthorized();
    }

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
}