using System.ComponentModel.DataAnnotations;
using Application.UseCases.Accounts;
using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Has;
using Application.UseCases.Has.Dtos;
using Domain;
using JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Service.UseCases.Has;
using Service.UseCases.Has.Dtos;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HasController : Controller
{
    private readonly UseCaseFetchAllHas _useCaseFetchAllHas;
    private readonly UseCaseFetchHasByCompanies _useCaseFetchHasByCompanies;
    private readonly UseCaseFetchHasByAccount _useCaseFetchHasByAccount;
    private readonly UseCaseFetchHasByFunctions _useCaseFetchHasByFunctions;
    private readonly UseCaseCreateHas _useCaseCreateHas;
    private readonly UseCaseFetchHasById _useCaseFetchHasById;
    private readonly UseCaseDeleteHas _useCaseDeleteHas;
    private readonly UseCaseFetchAccountByEmail _useCaseFetchAccountByEmail;
    private readonly UseCaseFetchFunctionById _useCaseFetchFunctionById;

    private readonly AccountController _accountController;
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _config;
    

    public HasController(UseCaseFetchAllHas useCaseFetchAllHas,
        UseCaseFetchHasByCompanies useCaseFetchHasByCompanies,
        UseCaseFetchHasByAccount useCaseFetchHasByAccount,
        UseCaseFetchHasByFunctions useCaseFetchHasByFunctions,
        UseCaseCreateHas useCaseCreateHas,
        UseCaseFetchHasById useCaseFetchHasById,
        UseCaseDeleteHas useCaseDeleteHas,
        AccountController accountController,
        UseCaseFetchAccountByEmail useCaseFetchAccountByEmail,
        UseCaseFetchFunctionById useCaseFetchFunctionById,
        ISessionService sessionService,
        IConfiguration configuration)
    {
        _useCaseFetchAllHas = useCaseFetchAllHas;
        _useCaseFetchHasByCompanies = useCaseFetchHasByCompanies;
        _useCaseFetchHasByAccount = useCaseFetchHasByAccount;
        _useCaseFetchHasByFunctions = useCaseFetchHasByFunctions;
        _useCaseCreateHas = useCaseCreateHas;
        _useCaseFetchHasById = useCaseFetchHasById;
        _useCaseDeleteHas = useCaseDeleteHas;
        _accountController = accountController;
        _useCaseFetchAccountByEmail = useCaseFetchAccountByEmail;
        _useCaseFetchFunctionById = useCaseFetchFunctionById;
        _sessionService = sessionService;
        _config = configuration;
    }
    //fetches are used to see if there is already a
    //has with one of the data we send
    [HttpGet]
    [Route("fetch/")]
    public IEnumerable<DtoOutputHas> FetchAll()
    {
        return _useCaseFetchAllHas.Execute();
    }
    
    [HttpGet]
    [Route("fetchById/{idHas:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputHas> FetchById(int idHas)
    {
        try
        {
            return _useCaseFetchHasById.Execute(idHas);
        }
        catch (KeyNotFoundException e)
        {
            return null;
        }
    }
    
    [HttpGet]
    [Route("fetchCompanies/{idCompanies:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdCompanies(int idCompanies)
    {
        return _useCaseFetchHasByCompanies.Execute(idCompanies);
    }
    
    [HttpGet]
    [Route("fetchAccount/{idAccount:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdAccount(int idAccount)
    {
        return _useCaseFetchHasByAccount.Execute(idAccount);
    }
    
    [HttpGet]
    [Route("fetchFunctions/{idFunction:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputHas> FetchByIdFunction(int idFunction)
    {
        return _useCaseFetchHasByFunctions.Execute(idFunction);
    }
    
    //Since has is a table that unites function, account, company,
    //if you create a company with a manager, you need a has
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoInputCreateHas> Create(DtoInputCreateHas dto)
    {
        var output = _useCaseCreateHas.Execute(dto);

        if (output == null) return Conflict(new Has());

        var account = _accountController.FetchProfilById(dto.has.IdAccount);
        DtoInputLoginAccount accountLogin = new DtoInputLoginAccount
        {
            Email = account.Value.Email,
            Password = ""
        };
        disconnect();
        connect(accountLogin);
        return CreatedAtAction(
            nameof(FetchById),
            new { idHas = output.IdHas },
            output
        );
        
    }
    
    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteHas.Execute(id);
    }
    
    //Connect and disconnect are the same methods as in account controler
    //but if you don't put them there it can't find the diff
    [HttpPost]
    [Route("connect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public void connect(DtoInputLoginAccount dto)
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
            
        var generatedToken =
            _sessionService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], account, functionName);
        var cookie = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Append("session", generatedToken, cookie); 
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
    
    [HttpPost]
    [Route("disconnect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult disconnect()
    {
        Response.Cookies.Delete("public");
        Response.Cookies.Delete("session");
        return Ok(new {});
    }
}