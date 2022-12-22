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
        
    }
}