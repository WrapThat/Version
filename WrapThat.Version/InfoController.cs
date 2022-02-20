using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class InfoController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [Route("version")]
    public ActionResult<string> Version()
    {
        var version = Assembly.GetEntryAssembly()!.GetName().Version;
        return Ok(version?.ToSemver());
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<string> Info()
    {
        var version = Assembly.GetEntryAssembly()!.GetName().Version;
        var shields = new ShieldsIo("Version", version!.ToSemver());
        return Ok(shields);
    }

    [HttpGet]
    [Route("status")]
    public ActionResult<string> Status()
    {
        var version = Assembly.GetEntryAssembly()!.GetName().Version;
        return Ok(version?.ToSemver());
    }

}

public static class VersionExtensions
{
    public static string ToSemver(this System.Version version) => $"{version.Major}.{version.Minor}.{version.Build}";
}



/// <summary>
/// To be used with Badges in Shields.io, ref https://shields.io/endpoint
/// </summary>
public class ShieldsIo
{
    public int schemaVersion => 1;
    public string label { get; set; }
    public string message { get; set; }

    public string color { get; set; } = "lightgrey";

    public ShieldsIo(string label, string message)
    {
        this.label = label;
        this.message = message;
    }
}
