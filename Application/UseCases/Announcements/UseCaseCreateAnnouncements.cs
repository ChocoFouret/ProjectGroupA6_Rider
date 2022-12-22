using Application.UseCases.Utils;
using Domain;

namespace Application.UseCases.Announcements;

public class UseCaseCreateAnnouncements : IUseCaseWriter<DtoOutputAnnouncements, DtoInputCreateAnnouncements>
{
    
    private readonly IAnnouncementsRepository _announcementsRepository;

    public UseCaseCreateAnnouncements(IAnnouncementsRepository announcementRepository)
    {
        _announcementsRepository = announcementRepository;
    }

    public DtoOutputAnnouncements Execute(DtoInputCreateAnnouncements input)
    {
        var dbAnnouncements = _announcementsRepository.Create(input.Announcements);
        return Mapper.GetInstance().Map<DtoOutputAnnouncements>(dbAnnouncements);
    }
}