using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Announcements;

public class UseCaseDeleteAnnouncements : IUseCaseWriter<Boolean, int>
{
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseDeleteAnnouncements(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }
    
    public Boolean Execute(int id)
    {
        var account = _announcementsRepository.FetchById(id);
        return _announcementsRepository.Delete(account);
    }
}