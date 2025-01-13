using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version;

public static class WebApplicationExtensions
{
    public static WebApplication MapVersionApiEndpoints(this WebApplication app)
    {
        var infoGroup = app.MapGroup("/api/info").WithTags("Version");
        infoGroup.MapGet("/", [AllowAnonymous] () => HandleRequest(c=>c.Info()));
        infoGroup.MapGet("/version", [AllowAnonymous] () => HandleRequest(c => c.Version()));
        infoGroup.MapGet("/productversion", [AllowAnonymous] () => HandleRequest(c => c.ProductVersion()));
        infoGroup.MapGet("/shields/version", [AllowAnonymous] () => HandleRequest(c => c.InfoShields()));
        infoGroup.MapGet("/shields/productversion", [AllowAnonymous] () => HandleRequest(c => c.ProductVersionShields()));
        infoGroup.MapGet("/status", [AllowAnonymous] () => HandleRequest(c => c.Status()));
        return app;
    }

    private static ActionResult<string> HandleRequest(Func<InfoController, ActionResult<string>> action)
    {
        var controller = new InfoController();
        var actionResult = action(controller);
        return actionResult as ActionResult<string>;
    }
}