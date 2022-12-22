using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchAllByCompanyAnnouncements : IUseCaseParameterizedQuery<IEnumerable<DtoOutputAnnouncements>, int>
{
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseFetchAllByCompanyAnnouncements(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }

    public IEnumerable<DtoOutputAnnouncements> Execute(int id)
    {
        var dbAnnouncements = _announcementsRepository.FetchAll(id);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputAnnouncements>>(dbAnnouncements);
    }
}