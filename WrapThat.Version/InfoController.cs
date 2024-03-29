﻿using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
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
    [Route("productversion")]
    public ActionResult<string> ProductVersion()
    {
        var assembly = Assembly.GetEntryAssembly();
        var fileversioninfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var results = fileversioninfo.ProductVersion.Split('+');
        return Ok(results[0]);
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
    [AllowAnonymous]
    [Route("shields/version")]
    public ActionResult<string> InfoShields() => Info();
    

    [HttpGet]
    [AllowAnonymous]
    [Route("shields/productversion")]
    public ActionResult<string> ProductVersionShields()
    {
        var assembly= Assembly.GetEntryAssembly();
        var fileversioninfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var pv = fileversioninfo.ProductVersion;
        var pvSplit = pv.Split('+');
        var shields = new ShieldsIo("Version", pvSplit[0]);
        return Ok(shields);
    }



    [HttpGet]
    [AllowAnonymous]
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
