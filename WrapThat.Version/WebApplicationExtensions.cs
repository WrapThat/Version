using Microsoft.AspNetCore.Builder;

namespace WrapThat.Version;

public static class WebApplicationExtensions
{
    public static WebApplication MapVersionApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api/info", () => new InfoController().Info())
            .AllowAnonymous();
        app.MapGet("/api/info/version", () => new InfoController().Version())
            .AllowAnonymous();
        app.MapGet("/api/info/productversion", () => new InfoController().ProductVersion())
            .AllowAnonymous();
        app.MapGet("/api/info/shields/version", () => new InfoController().InfoShields())
            .AllowAnonymous();
        app.MapGet("/api/info/shields/productversion", () => new InfoController().ProductVersionShields())
            .AllowAnonymous();
        app.MapGet("/api/info/status", () => new InfoController().Status())
            .AllowAnonymous();
        return app;
    }
}