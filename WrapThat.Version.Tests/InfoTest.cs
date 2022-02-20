using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version.Tests;

internal class InfoTest
{
    [Test]
    public void ShieldsIoTest()
    {
        var sut = new InfoController();
        var result = sut.Info();
        var value = result.Result.AssertObjectResult<ShieldsIo>();
        Assert.Multiple(() =>
        {
            Assert.That(value.label, Is.EqualTo("Version"));
            Assert.That(value.message.Length, Is.GreaterThan(0));
        });
        TestContext.WriteLine($"Version is {value.message}");
    }

    [Test]
    public void VersionTest()
    {
        var sut = new InfoController();
        var result = sut.Version();
        var value = result.Result.AssertObjectResult<string>();
        Assert.Multiple(() =>
        {
            Assert.That(value.Count(c => c == '.'), Is.EqualTo(2));
            Assert.That(value.All(c => c == '.' || char.IsDigit(c)));
        });
        
    }

    [Test]
    public void VersionExtensionTest()
    {
        var v = new System.Version(1,2,3,4);
        var result = v.ToSemver();
        Assert.That(result, Is.EqualTo("1.2.3"));
    }
}

public static class ConvertObjectResult
{

    public static T AssertObjectResult<T>(this ActionResult? result)
    {
        var realResult = result as OkObjectResult;

        Assert.That(realResult, Is.Not.Null, "Returnerer ikke Ok result");
        Assert.That(realResult!.Value, Is.Not.Null, "Returnerer null value");

        return (T)realResult.Value!;

    }
}