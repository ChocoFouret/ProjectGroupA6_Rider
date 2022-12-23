using Application.UseCases.Announcements;
using Application.UseCases.Announcements.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Plan_it.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly UseCaseCreateAnnouncements _useCaseCreateAnnouncements;
    private readonly UseCaseDeleteAnnouncements _useCaseDeleteAnnouncements;
    private readonly UseCaseUpdateAnnouncements _useCaseUpdateAnnouncements;
    private readonly UseCaseFetchAllByCompanyAnnouncements _useCaseFetchAllByCompanyAnnouncements;
    private readonly UseCaseFetchAnnouncementsById _useCaseFetchAnnouncementsById;
    private readonly UseCaseFetchAnnouncementsByIdFunction _useCaseFetchAnnouncementsByIdFunction;

    public AnnouncementsController(
        UseCaseCreateAnnouncements useCaseCreateAnnouncements,
        UseCaseDeleteAnnouncements useCaseDeleteAnnouncements,
        UseCaseUpdateAnnouncements useCaseUpdateAnnouncements,
        UseCaseFetchAllByCompanyAnnouncements useCaseFetchAllByCompanyAnnouncements,
        UseCaseFetchAnnouncementsById useCaseFetchAnnouncementsById,
        UseCaseFetchAnnouncementsByIdFunction useCaseFetchAnnouncementsByIdFunction
    )
    {
        _useCaseCreateAnnouncements = useCaseCreateAnnouncements;
        _useCaseDeleteAnnouncements = useCaseDeleteAnnouncements;
        _useCaseUpdateAnnouncements = useCaseUpdateAnnouncements;
        _useCaseFetchAllByCompanyAnnouncements = useCaseFetchAllByCompanyAnnouncements;
        _useCaseFetchAnnouncementsById = useCaseFetchAnnouncementsById;
        _useCaseFetchAnnouncementsByIdFunction = useCaseFetchAnnouncementsByIdFunction;
    }

    [HttpGet]
    [Route("fetchByCompany/{id:int}")]
    public IEnumerable<DtoOutputAnnouncements> FetchAll(int id)
    {
        return _useCaseFetchAllByCompanyAnnouncements.Execute(id);
    }

    [HttpPost]
    [Route("fetchByFunction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IEnumerable<DtoOutputAnnouncements> FetchByIdFunction(DtoInputEmployeeAnnouncements dto)
    {
        return _useCaseFetchAnnouncementsByIdFunction.Execute(dto);
    }

    [HttpGet]
    [Route("fetch/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoOutputAnnouncements> FetchById(int id)
    {
        try
        {
            return _useCaseFetchAnnouncementsById.Execute(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPut]
    [Route("update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Update(DtoInputUpdateAnnouncements dto)
    {
        return _useCaseUpdateAnnouncements.Execute(dto);
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<DtoOutputAnnouncements> Create(DtoInputCreateAnnouncements dto)
    {
        var output = _useCaseCreateAnnouncements.Execute(dto);
        return CreatedAtAction(
            nameof(FetchById),
            new { id = output.IdAnnouncements },
            output
        );
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Boolean> Delete(int id)
    {
        return _useCaseDeleteAnnouncements.Execute(id);
    }
}