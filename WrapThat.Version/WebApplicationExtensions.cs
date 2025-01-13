using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version;

public static class WebApplicationExtensions
{
    public static WebApplication MapVersionApiEndpoints(this WebApplication app)
    {
        var controller = new InfoController();

        app.MapGet("/api/info", [AllowAnonymous] () => HandleRequest(c=>c.Info()));

        app.MapGet("/api/info/version", [AllowAnonymous] () => HandleRequest(c => c.Version()));

        app.MapGet("/api/info/productversion", [AllowAnonymous] () => HandleRequest(c => c.ProductVersion()));

        app.MapGet("/api/info/shields/version", [AllowAnonymous] () => HandleRequest(c => c.InfoShields()));

        app.MapGet("/api/info/shields/productversion", [AllowAnonymous] () => HandleRequest(c => c.ProductVersionShields()));

        app.MapGet("/api/info/status", [AllowAnonymous] () => HandleRequest(c => c.Status()));

        return app;
    }

    private static ActionResult<string> HandleRequest(Func<InfoController, ActionResult<string>> action)
    {
        var controller = new InfoController();
        var actionResult = action(controller);
        return actionResult as ActionResult<string>;
    }
}