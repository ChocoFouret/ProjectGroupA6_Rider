using Application;
using Application.UseCases.Announcements.Dtos;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchAnnouncementsByIdFunction : IUseCaseParameterizedQuery<IEnumerable<DtoOutputAnnouncements>, DtoInputEmployeeAnnouncements>
{
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseFetchAnnouncementsByIdFunction(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }

    public IEnumerable<DtoOutputAnnouncements> Execute(DtoInputEmployeeAnnouncements dto)
    {
        var dbAnnouncements = _announcementsRepository.FetchByIdFunction(dto.IdCompanies, dto.IdFunctions);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAnnouncements>>(dbAnnouncements);
    }
}