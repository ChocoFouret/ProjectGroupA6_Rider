
using Domain;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

public class EfAnnouncementsRepository : IAnnouncementsRepository
{
    private readonly PlanitContextProvider _planitContextProvider;
    
    public EfAnnouncementsRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    public IEnumerable<Announcements> FetchAll(int id)
    {
        using var context = _planitContextProvider.NewContext();
        return context.Announcements.ToList().Where(announcements => announcements.IdCompanies == id);
    }

    public Announcements FetchById(int idAnnouncements)
    {
        using var context = _planitContextProvider.NewContext();
        var announcements = context.Announcements.FirstOrDefault(announcements => announcements.IdAnnouncements == idAnnouncements);

        if (announcements == null)
            throw new KeyNotFoundException($"Announcements with {idAnnouncements} has not been found");

        return announcements;
    }
    
    public IEnumerable<Announcements> FetchByIdFunction(int idCompanies, int idFunctions)
    {
        using var context = _planitContextProvider.NewContext();
        var announcements = context.Announcements.ToList().Where(announcements => announcements.IdFunctions == idFunctions && announcements.IdCompanies == idCompanies);
        return announcements;
    }

    public Announcements Create(Announcements announcements)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Announcements.Add(announcements);
            context.SaveChanges();
            return announcements;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    public bool Update(Announcements announcements)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Announcements.Update(announcements);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Delete(Announcements announcements)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Announcements.Remove(announcements);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
}