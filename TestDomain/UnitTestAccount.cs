using Domain;
using NUnit.Framework;

namespace TestDomainAccount;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Account account=new Account()
        {
            IdAccount = 0,
            Email = "",
            Password = "",
            LastName = "",
            FirstName = "",
            IdAddress = 0,
            IsAdmin = true,
            PictureURL = "",
            Phone = ""
        };
        
        Assert.False(account.goodMail());
        account.Email = "XXX";
        Assert.False(account.goodMail());
        account.Email = "XXX@X";
        Assert.False(account.goodMail());
        account.Email = "XXX.X";
        Assert.False(account.goodMail());
        account.Email = "XXX.X@X";
        Assert.False(account.goodMail());
        account.Email = "XXX@X.";
        Assert.False(account.goodMail());
        account.Email = "XX@X@X";
        Assert.False(account.goodMail());
        account.Email = "XX.@.X";
        Assert.False(account.goodMail());
        account.Email = "XX@X.X";
        Assert.True(account.goodMail());

        account.Password = "12345";
        Assert.False(account.matchPassword("1234"));
        Assert.True(account.matchPassword("12345"));
    }
}