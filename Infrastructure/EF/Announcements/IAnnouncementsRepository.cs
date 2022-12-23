namespace Domain;

public interface IAnnouncementsRepository
{
    IEnumerable<Announcements> FetchAll(int id);
    Announcements FetchById(int idAnnouncements);
    IEnumerable<Announcements> FetchByIdFunction(int idCompanies, int idFunction);
    Announcements Create(Announcements announcements);
    bool Update(Announcements announcements);
    bool Delete(Announcements announcements);
}