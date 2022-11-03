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
    [Route("/fetch/")]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }

    [HttpGet]
    [Route("/fetch/{id:int}")]
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
    [Route("/fetch/{email}")]
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
    [Route("/create")]
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
    [Route("/delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteAccount.Execute(id);
    }

    [HttpPut]
    [Route("/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAccount dto)
    {
        return _useCaseUpdateAccount.Execute(dto);
    }
    
    [HttpPost]
    [Route("/login")]
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
    [Route("/is/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult IsStatus()
    {
        bool isLogged = User.IsInRole("Employee") | User.IsInRole("Director") | User.IsInRole("Administrator");
        bool isDirector = User.IsInRole("Director") | User.IsInRole("Administrator");
        bool isAdmin = User.IsInRole("Administrator");
        return Ok(new {logged = isLogged, director = isDirector, admin = isAdmin});
    }
}