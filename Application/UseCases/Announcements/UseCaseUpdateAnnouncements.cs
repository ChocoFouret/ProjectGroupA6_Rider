using Application.UseCases.Utils;
using Domain;

public class UseCaseUpdateAnnouncements : IUseCaseWriter<Boolean, DtoInputUpdateAnnouncements>
{
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseUpdateAnnouncements(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }
    
    public Boolean Execute(DtoInputUpdateAnnouncements input)
    {
        var announcements = _announcementsRepository.FetchById(input.Announcements.IdAnnouncements);
        announcements = input.Announcements;
        return _announcementsRepository.Update(announcements);
    }
}