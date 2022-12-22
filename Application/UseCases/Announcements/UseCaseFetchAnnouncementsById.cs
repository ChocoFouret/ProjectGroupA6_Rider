using Application;
using Application.UseCases.Utils;
using Domain;

public class UseCaseFetchAnnouncementsById : IUseCaseParameterizedQuery<DtoOutputAnnouncements, int>
{
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseFetchAnnouncementsById(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }

    public DtoOutputAnnouncements Execute(int id)
    {
        var dbAnnouncements = _announcementsRepository.FetchById(id);
        return Mapper.GetInstance().Map<DtoOutputAnnouncements>(dbAnnouncements);
    }
}