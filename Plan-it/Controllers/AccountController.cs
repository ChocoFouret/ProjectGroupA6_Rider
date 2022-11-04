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

    /// <summary>
    /// It returns a list of DtoOutputAccount objects, which are the result of the Execute() function of the
    /// _useCaseFetchAllAccounts object
    /// </summary>
    /// <returns>
    /// A list of DtoOutputAccount objects.
    /// </returns>
    [HttpGet]
    [Route("/fetch/")]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }

    /// <summary>
    /// It returns a DtoOutputAccount object if the id is found, otherwise it returns a 404 Not Found error
    /// </summary>
    /// <param name="id">int - This is the route parameter. It's a required parameter.</param>
    /// <returns>
    /// ActionResult<DtoOutputAccount>
    /// </returns>
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

    /// <summary>
    /// It takes an email address as a parameter, and returns a DTO object containing the account details
    /// </summary>
    /// <param name="email">The email of the account to fetch.</param>
    /// <returns>
    /// The action result is returning a DtoOutputAccount object.
    /// </returns>
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

    /// <summary>
    /// The function is called when a POST request is made to the /create route. It takes a DtoInputCreateAccount object as
    /// a parameter, and returns a DtoOutputAccount object
    /// </summary>
    /// <param name="DtoInputCreateAccount">The input data transfer object (DTO) that contains the data that the user will
    /// send to the API.</param>
    /// <returns>
    /// The action result of the create method is being returned.
    /// </returns>
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

    /// <summary>
    /// The function takes in a DTO (Data Transfer Object) and returns a boolean
    /// </summary>
    /// <param name="id">The id of the account to be deleted</param>
    /// <returns>
    /// The return type is ActionResult<Boolean>
    /// </returns>
    [HttpDelete]
    [Route("/delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteAccount.Execute(id);
    }

    /// <summary>
    /// The function takes a DTO as input, calls the use case, and returns the result of the use case
    /// </summary>
    /// <param name="DtoInputUpdateAccount">This is the input parameter for the use case.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    [HttpPut]
    [Route("/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAccount dto)
    {
        return _useCaseUpdateAccount.Execute(dto);
    }
    
    /// <summary>
    /// It takes a DTO as input, checks if the user exists, and if so, it generates a JWT token and returns it as a cookie
    /// </summary>
    /// <param name="DtoInputLoginAccount">This is the input data transfer object that will be used to pass the data to the
    /// use case.</param>
    /// <returns>
    /// A cookie with the name "session" and the value of the generated token.
    /// </returns>
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
    
    /// <summary>
    /// It returns a JSON object with three boolean values, one for each role
    /// </summary>
    /// <returns>
    /// A JSON object with the following properties:
    /// - logged: true if the user is logged in, false otherwise
    /// - director: true if the user is a director or an administrator, false otherwise
    /// - admin: true if the user is an administrator, false otherwise
    /// </returns>
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