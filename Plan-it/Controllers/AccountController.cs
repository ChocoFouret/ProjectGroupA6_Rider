using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Domain;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UseCaseLoginAccount _useCaseLoginAccount;
    private readonly UseCaseCreateAccount _useCaseCreateAccount;
    private readonly UseCaseUpdateAccount _useCaseUpdateAccount;
    private readonly UseCaseDeleteAccount _useCaseDeleteAccount;
    private readonly UseCaseFetchAllAccounts _useCaseFetchAllAccounts;
    private readonly UseCaseFetchAccountById _useCaseFetchAccountById;
    private readonly UseCaseFetchAccountByEmail _useCaseFetchAccountByEmail;
    private readonly UseCaseGetAccount _useCaseGetAccount;

    private readonly ISessionService _sessionService;
    private readonly IConfiguration _config;
    
    public AccountController(
        UseCaseLoginAccount useCaseLoginAccount,
        UseCaseCreateAccount useCaseCreateAccount,
        UseCaseUpdateAccount useCaseUpdateAccount,
        UseCaseDeleteAccount useCaseDeleteAccount,
        UseCaseFetchAllAccounts useCaseFetchAllAccounts,
        UseCaseFetchAccountById useCaseFetchAccountById,
        UseCaseGetAccount useCaseGetAccount,
        UseCaseFetchAccountByEmail useCaseFetchAccountByEmail,
        ISessionService sessionService,
        IConfiguration configuration
    )
    {
        _useCaseLoginAccount = useCaseLoginAccount;
        _useCaseCreateAccount = useCaseCreateAccount;
        _useCaseUpdateAccount = useCaseUpdateAccount;
        _useCaseDeleteAccount = useCaseDeleteAccount;
        _useCaseFetchAllAccounts = useCaseFetchAllAccounts;
        _useCaseFetchAccountById = useCaseFetchAccountById;
        _useCaseGetAccount = useCaseGetAccount;
        _useCaseFetchAccountByEmail = useCaseFetchAccountByEmail;

        _sessionService = sessionService;
        _config = configuration;
    }

    [HttpGet]
    [Route("/account/find/")]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }

    [HttpGet]
    [Route("/account/find/{id:int}")]
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

    [HttpGet]
    [Route("/account/find/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputAccount> FetchByEmail(string email)
    {
        try
        {
            return _useCaseFetchAccountByEmail.Execute(email);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize(Policy = "all")]
    [HttpPost]
    [Route("/account/create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAccount> Create(DtoInputCreateAccount dto)
    {
        // Use for add new account easily
        dto.account.Function = "Employee";
        var output = _useCaseCreateAccount.Execute(dto);

        if (output == null) return Conflict(new Account());

        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.Id },
            output
        );
    }

    [HttpDelete]
    [Route("/account/delete/{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(string email)
    {
        return _useCaseDeleteAccount.Execute(email);
    }

    [HttpPut]
    [Route("/account/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAccount dto)
    {
        // Use for add new account easily
        if (dto.account.Function.Equals("string")) dto.account.Function = "Employee";
        return _useCaseUpdateAccount.Execute(dto);
    }
    
    [HttpPost]
    [Route("/account/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult Login(DtoInputLoginAccount dto)
    {
        if (_useCaseLoginAccount.Execute(dto))
        {
            var generatedToken =
                _sessionService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), _useCaseGetAccount.Execute(dto.Email));
            
            var cookie = new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("session", generatedToken, cookie);

            return Ok(new {});
        }

        return Unauthorized();
    }
    
    [Authorize]
    [HttpGet]
    [Route("/account/is/employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult IsLogged()
    {
        return Ok(new {ok = true});
    }
    
    [Authorize(Policy = "all")]
    [HttpGet]
    [Route("/account/is/director")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult IsDirector()
    {
        return Ok(new {ok = true});
    }
    
    [Authorize(Policy = "administrator")]
    [HttpGet]
    [Route("/account/is/admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult IsAdmin()
    {
        return Ok(new {ok = true});
    }
}