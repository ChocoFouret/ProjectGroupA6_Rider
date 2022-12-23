using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Companies;
using Application.UseCases.Has.Dtos;
using Domain;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Has.Dtos;

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
    private readonly UseCaseFetchAccountByEmail _useCaseFetchAccountByEmail;
    private readonly UseCaseFetchHasByAccount _useCaseFetchHasByAccount;
    private readonly UseCaseFetchFunctionById _useCaseFetchFunctionById;
    private readonly UseCaseFetchProfilById _useCaseFetchProfilById;
    private readonly UseCaseFetchAddressById _useCaseFetchAddressById;
    private readonly UseCaseFetchCompaniesById _useCaseFetchCompaniesById;
    
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _config;
    
    public AccountController(
        UseCaseLoginAccount useCaseLoginAccount,
        UseCaseCreateAccount useCaseCreateAccount,
        UseCaseUpdateAccount useCaseUpdateAccount,
        UseCaseDeleteAccount useCaseDeleteAccount,
        UseCaseFetchAllAccounts useCaseFetchAllAccounts,
        UseCaseFetchAccountByEmail useCaseFetchAccountByEmail,
        ISessionService sessionService,
        IConfiguration configuration, 
        UseCaseFetchHasByAccount useCaseFetchHasByAccount,
        UseCaseFetchFunctionById useCaseFetchFunctionById,
        UseCaseFetchProfilById useCaseFetchProfilById,
        UseCaseFetchAddressById useCaseFetchAddressById,
        UseCaseFetchCompaniesById useCaseFetchCompaniesById
    )
    {
        _useCaseLoginAccount = useCaseLoginAccount;
        _useCaseCreateAccount = useCaseCreateAccount;
        _useCaseUpdateAccount = useCaseUpdateAccount;
        _useCaseDeleteAccount = useCaseDeleteAccount;
        _useCaseFetchAllAccounts = useCaseFetchAllAccounts;
        _useCaseFetchAccountByEmail = useCaseFetchAccountByEmail;
        _useCaseFetchHasByAccount = useCaseFetchHasByAccount;
        _useCaseFetchFunctionById = useCaseFetchFunctionById;
        _useCaseFetchProfilById = useCaseFetchProfilById;
        _useCaseFetchAddressById = useCaseFetchAddressById;
        _useCaseFetchCompaniesById = useCaseFetchCompaniesById;
        
        _sessionService = sessionService;
        _config = configuration;
    }

    //Use to retrieve all accounts to display this list in the administrator's site management page
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputAccount> FetchAll()
    {
        return _useCaseFetchAllAccounts.Execute();
    }
    
   //Use to show the profil page (webstorm and android studio)
    [HttpGet]
    [Route("fetch/profil/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputProfilAccount> FetchProfilById(int id)
    {
        try
        {
            DtoOutputProfilAccount dtoOutputProfilAccount = _useCaseFetchProfilById.Execute(id);
            dtoOutputProfilAccount.Address = _useCaseFetchAddressById.Execute(dtoOutputProfilAccount.IdAddress);
            var has = _useCaseFetchHasByAccount.Execute(id).FirstOrDefault();
            if (has != null)
            {
                dtoOutputProfilAccount.Companies = _useCaseFetchCompaniesById.Execute(has.IdCompanies); 
                dtoOutputProfilAccount.Function = has.Function.Title;
            }
            
            return dtoOutputProfilAccount;
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    [Route("fetch/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Account> FetchByEmail(string email)
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
    
    //Used to create an account, it requires a confirmPassword attribute
    //to avoid the user to enter 2 different passwords from webstorm
    [HttpPost]
    [Route("create/{confirmPassword}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAccount> Create(DtoInputCreateAccount dto, string confirmPassword)
    {
        //Security when the user intercept dto
        dto.account.IsAdmin = false;
        var passwordBeforeCrypt = dto.account.Password;
        
        if (!dto.account.matchPassword(confirmPassword)) return Conflict(new Account());
        if (!dto.account.goodMail()) return Conflict(new Account());
        
        var output = _useCaseCreateAccount.Execute(dto);

        //if (output == null ) return Conflict(new Account());
        
        DtoInputLoginAccount dtoLogin = new DtoInputLoginAccount()
        {
            Email = dto.account.Email,
            Password = passwordBeforeCrypt
        };
        if (_useCaseLoginAccount.Execute(dtoLogin))
        {
            /*Account account = _useCaseFetchAccountByEmail.Execute(dtoLogin.Email);
            IEnumerable<DtoOutputHas> has = _useCaseFetchHasByAccount.Execute(account.IdAccount);
            bool isHas = has.ToList().Count != 0;

            int idCompanie = -1;
            string functionName = "";
            if (isHas)
            {
                idCompanie = has.ToList().FirstOrDefault().IdCompanies;

                DtoOutputFunction function = _useCaseFetchFunctionById.Execute(has.FirstOrDefault().IdFunctions);
                if (function != null)
                {
                    functionName = function.Title;
                }

            }

            var generatedToken =
                _sessionService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), account,
                    functionName);

            var cookie = new CookieOptions()
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("session", generatedToken, cookie);

            var generatedTokenPublic =
                _sessionService.BuildTokenPublic(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(),
                    account, idCompanie, functionName);
            var cookiePublic = new CookieOptions()
            {
                Secure = true,
                HttpOnly = false,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("public", generatedTokenPublic, cookiePublic);*/
            Connect(dtoLogin);
        }
        return CreatedAtAction(
            nameof(FetchProfilById),
            new { id = output.IdAccount },
            output
        );
    }

    //Use to delete an account via the admin page
    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteAccount.Execute(id);
    }

    //Use to update an account via the profil page
    [HttpPut]
    [Route("update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAccount dto)
    {
        return _useCaseUpdateAccount.Execute(dto);
    }
    
    /*[HttpPut]
    [Route("update/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePassword(DtoInputUpdatePasswordAccount dto)
    {
        return Ok(new {Password = _useCaseUpdatePasswordAccount.Execute(dto)});
    }*/

    //Use to check if the user (email and password entered in angular)
    //match and that he can connect by creating the appropriate cookies 
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(DtoInputLoginAccount dto)
    {
        if (_useCaseLoginAccount.Execute(dto))
        {
            Connect(dto);
            return Ok(new {});
        }
        return Unauthorized();
    }
    
    [HttpPost]
    [Route("disconnect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Disconnect()
    {
        
        Response.Cookies.Delete("public");
        Response.Cookies.Delete("session");
        return Ok(new {});
    }

    
    [HttpPost]
    [Route("connect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public void Connect(DtoInputLoginAccount dto)
    {
        var account = _useCaseFetchAccountByEmail.Execute(dto.Email);
        var has = _useCaseFetchHasByAccount.Execute(account.IdAccount);
        var isHas = has.ToList().Count != 0;

        var idCompanie = -1;
        var functionName = "";
        if (isHas)
        {
            idCompanie = has.ToList().FirstOrDefault().IdCompanies;
                
            DtoOutputFunction function = _useCaseFetchFunctionById.Execute(has.FirstOrDefault().IdFunctions);
            if (function != null)
            {
                functionName = function.Title;   
            }
                
        }
            
        //cookie used in rider for controller [authorize] to communicate
        //with the application (unreadable in angular)
        var generatedToken =
            _sessionService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], account, functionName);
        var cookie = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Append("session", generatedToken, cookie); 
        
        //Allows in angular after decoding the cookie to know which role has the account
        //and in which company it is located
        var generatedTokenPublic =
            _sessionService.BuildTokenPublic(_config["Jwt:Key"], _config["Jwt:Issuer"], account, idCompanie, functionName);
        var cookiePublic = new CookieOptions
        {
            Secure = true,
            HttpOnly = false,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Append("public", generatedTokenPublic, cookiePublic);   
    }
    
    //Same thing than connect() but for android studio
    [HttpPost]
    [Route("login/phone")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<DtoOutputAccountPhone> LoginByPhone(DtoInputLoginAccount dto)
    {
        if (_useCaseLoginAccount.Execute(dto))
        {
            var account = _useCaseFetchAccountByEmail.Execute(dto.Email);
            IEnumerable<DtoOutputHas> has = _useCaseFetchHasByAccount.Execute(account.IdAccount);
            bool isHas = has.ToList().Count != 0;

            int idCompanie = -1;
            string functionName = "";
            if (isHas)
            {
                idCompanie = has.ToList().FirstOrDefault().IdCompanies;
                
                DtoOutputFunction function = _useCaseFetchFunctionById.Execute(has.FirstOrDefault().IdFunctions);
                if (function != null)
                {
                    functionName = function.Title;   
                }
            }
            var generatedToken =
                _sessionService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], account, functionName);
            var generatedTokenPublic =
                _sessionService.BuildTokenPublic(_config["Jwt:Key"], _config["Jwt:Issuer"],
                    account, idCompanie, functionName);
            var dtoOutputAccount = new DtoOutputAccountPhone
            {
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                IdAccount = account.IdAccount,
                Token = generatedTokenPublic,
                TokenPrivate = generatedToken
            };
            
            return dtoOutputAccount;
        }
        return null;
    }
}