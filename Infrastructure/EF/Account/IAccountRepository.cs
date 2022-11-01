namespace Domain;

public interface IAccountRepository
{
    IEnumerable<Account> FetchAll();
    Account FetchById(int idAccount);
    Account? FetchByEmail(string email);
    Account? GetAccount(string email);
    Account Create(Account account);
    bool Update(Account account);
    bool Delete(Account account);
    bool Login(string email, string password);
}