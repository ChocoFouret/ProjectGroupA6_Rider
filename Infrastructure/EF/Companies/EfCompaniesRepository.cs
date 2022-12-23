using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Companies;
using Domain;

public class EfCompaniesRepository : ICompaniesRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    public EfCompaniesRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }
    
    
    public IEnumerable<Companies> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
            return context.Companies.ToList<Domain.Companies>();
    }

    public Companies FetchById(int id)
    {
        using var context = _planitContextProvider.NewContext();
            var companies = context.Companies.FirstOrDefault(companies => companies.IdCompanies == id);

            if (companies == null)
                throw new KeyNotFoundException($"Companies with {id} has not been found");
            return companies;
    }

    public IEnumerable<Companies> FetchByName(string name)
    {
        using var context = _planitContextProvider.NewContext();
        
        var companies = context.Companies.Where(companies => companies.CompaniesName == name).ToList();

        return companies;
    }

    public Companies Create(Companies companie)
    {
        using var context = _planitContextProvider.NewContext();
            try
            {
                context.Companies.Add(companie);
                context.SaveChanges();
                return companie;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return null;
            }
    }

    public bool Update(Companies companies)
    {
        using var context = _planitContextProvider.NewContext();
            try
            {
                context.Companies.Update(companies);
                return context.SaveChanges() == 1;
            }
            catch (DbUpdateConcurrencyException e)
            {
                return false;
            }
    }
    
    public bool Delete(Companies companies)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Companies.Remove(companies);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public Companies FetchByEmail(string email)
    {
        using var context = _planitContextProvider.NewContext();
        
        var companies = context.Companies.FirstOrDefault(companies=>companies.DirectorEmail==email);
        return companies;
    }

    public bool Join(string inputName, string inputPassword)
    {
        using var context = _planitContextProvider.NewContext();
        var companie = context.Companies.FirstOrDefault(companie => companie.CompaniesName == inputName);
        if (companie == null)
            throw new KeyNotFoundException($"companie with {inputName} has not been found");
        
        return EncryptPassword.ValidatePassword(inputPassword, companie.Password);
    }
}