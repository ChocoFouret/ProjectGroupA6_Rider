namespace Domain;

public interface IAccountRepository
{
    IEnumerable<Account> GetAll();
    Account GetById(int idAccount);
    Account Create(Account account);
    Account Read(Account account);
    bool Update(Account account);
    bool Delete(int idAccount);
    bool Login(int idAccount, string password);
}