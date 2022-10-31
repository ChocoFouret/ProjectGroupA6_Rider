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

    public IEnumerable<Account> GetAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Accounts.ToList<Account>();
    }

    public Account GetById(int idAccount)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Id == idAccount);

        if (account == null)
            throw new KeyNotFoundException($"Account with {idAccount} has not been found");

        return account;
    }

    public Account Create(Account account)
    {
        using var context = _planitContextProvider.NewContext();

        account.PasswordHash = EncryptPassword.HashPassword(account.PasswordHash);

        context.Accounts.Add(account);
        context.SaveChanges();
        return account;
    }

    public Account Read(Account account)
    {
        return null;
    }

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

    public bool Delete(int idAccount)
    {
        using var context = _planitContextProvider.NewContext();

        try
        {
            context.Accounts.Remove(new Account { Id = idAccount });
            return context.SaveChanges() == 1;
        }
        catch (DbUpdateConcurrencyException e)
        {
            return false;
        }
    }
    
    public bool Login(int idAccount, string password)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Id == idAccount);

        if (account == null)
            throw new KeyNotFoundException($"Account with {idAccount} has not been found");
        
        return EncryptPassword.ValidatePassword(password, account.PasswordHash);
    }
}