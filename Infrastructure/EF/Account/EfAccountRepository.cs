using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfAccountRepository : IAccountRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    public EfAccountRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }
    
    
    public IEnumerable<Account> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Accounts.ToList();
    }

    
    public Account FetchById(int idAccount)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.IdAccount == idAccount);

        if (account == null)
            throw new KeyNotFoundException($"Account with {idAccount} has not been found");

        return account;
    }
    
    public Account? FetchByEmail(string email)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Email == email);

        return account;
    }

    //Use to create and account in the register page
    public Account Create(Account account)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Accounts.Add(account);
            context.SaveChanges();
            return account;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return null;
        }
    }

    //Use to update the profil in the profil page
    public bool Update(Account account)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Accounts.Update(account);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Delete(Account account)
    {
        using var context = _planitContextProvider.NewContext();
        try
        {
            context.Accounts.Remove(account);
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }

    public bool Login(string email, string password)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Email == email);
        if (account == null)
            throw new KeyNotFoundException($"Account with {email} has not been found");
        
        return EncryptPassword.ValidatePassword(password, account.Password);
    }
}