using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class EfAccountRepository : IAccountRepository
{
    private readonly PlanitContextProvider _planitContextProvider;

    // For add method : Application -> UsesCases -> Account

    public EfAccountRepository(PlanitContextProvider planitContextProvider)
    {
        _planitContextProvider = planitContextProvider;
    }

    /// <summary>
    /// > This function fetches all accounts from the database and returns them as a list of Account objects
    /// </summary>
    /// <returns>
    /// A list of all accounts in the database.
    /// </returns>
    public IEnumerable<Account> FetchAll()
    {
        using var context = _planitContextProvider.NewContext();
        return context.Accounts.ToList<Account>();
    }

    /// <summary>
    /// > This function fetches an account by its id
    /// </summary>
    /// <param name="idAccount">The id of the account to be fetched.</param>
    /// <returns>
    /// An account object
    /// </returns>
    public Account FetchById(int idAccount)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Id == idAccount);

        if (account == null)
            throw new KeyNotFoundException($"Account with {idAccount} has not been found");

        return account;
    }

    /// <summary>
    /// > This function fetches an account from the database by email
    /// </summary>
    /// <param name="email">The email of the account to fetch</param>
    /// <returns>
    /// An account object
    /// </returns>
    public Account? FetchByEmail(string email)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Email == email);

        return account;
    }

    /// <summary>
    /// > This function returns an account object if the email exists in the database
    /// </summary>
    /// <param name="email">The email of the account to be retrieved.</param>
    /// <returns>
    /// An account object
    /// </returns>
    public Account? GetAccount(string email)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Email == email);

        return account;
    }

    /// <summary>
    /// This function creates a new account in the database
    /// </summary>
    /// <param name="Account">The account object that is being created.</param>
    /// <returns>
    /// The account that was created.
    /// </returns>
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

    /// <summary>
    /// This function updates an account in the database
    /// </summary>
    /// <param name="Account">The account object that you want to update.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
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

    /// <summary>
    /// > Delete an account from the database
    /// </summary>
    /// <param name="Account">The account to be deleted.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
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

    /// <summary>
    /// It takes in an email and password, checks if the email exists in the database, and if it does, it checks if the
    /// password matches the password in the database
    /// </summary>
    /// <param name="email">The email of the account you want to login with</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>
    /// A boolean value
    /// </returns>
    public bool Login(string email, string password)
    {
        using var context = _planitContextProvider.NewContext();
        var account = context.Accounts.FirstOrDefault(account => account.Email == email);

        if (account == null)
            throw new KeyNotFoundException($"Account with {email} has not been found");

        return EncryptPassword.ValidatePassword(password, account.Password);
    }
}