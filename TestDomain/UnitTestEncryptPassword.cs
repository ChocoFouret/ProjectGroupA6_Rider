using Domain;
using NUnit.Framework;

namespace TestEncryptPassword;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var passCrypt = EncryptPassword.HashPassword("12345");
        Assert.False(passCrypt=="12345");
        var passCrypt2 = EncryptPassword.HashPassword("12345");
        Assert.False(passCrypt==passCrypt2);
        
        Assert.False(EncryptPassword.ValidatePassword("123",passCrypt));
        Assert.True(EncryptPassword.ValidatePassword("12345",passCrypt));
    }
}