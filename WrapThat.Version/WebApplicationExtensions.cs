using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WrapThat.Version
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MapVersionApiEndpoints(this WebApplication app)
        {
            app.MapGet("/api/info", context => HandleRequest(context, c => c.Info()))
                .AllowAnonymous();
            app.MapGet("/api/info/version", context => HandleRequest(context, c => c.Version()))
                .AllowAnonymous();
            app.MapGet("/api/info/productversion", context => HandleRequest(context, c => c.ProductVersion()))
                .AllowAnonymous();
            app.MapGet("/api/info/shields/version", context => HandleRequest(context, c => c.InfoShields()))
                .AllowAnonymous();
            app.MapGet("/api/info/shields/productversion", context => HandleRequest(context, c => c.ProductVersionShields()))
                .AllowAnonymous();
            app.MapGet("/api/info/status", context => HandleRequest(context, c => c.Status()))
                .AllowAnonymous();
            return app;
        }

        private static async Task HandleRequest<T>(HttpContext context, Func<InfoController, ActionResult<T>> action)
        {
            var controller = new InfoController();
            var actionResult = action(controller);
            await WriteResultAsync(context, actionResult);
        }

        private static async Task WriteResultAsync<T>(HttpContext context, ActionResult<T> actionResult)
        {
            if (actionResult.Result is ObjectResult result)
            {
                context.Response.StatusCode = (int)result.StatusCode!;
                await context.Response.WriteAsJsonAsync(result.Value);
            }
            else
            {
                if (actionResult.Result is StatusCodeResult statusCodeResult)
                {
                    context.Response.StatusCode = statusCodeResult.StatusCode;
                }
                else if (actionResult.Value != null)
                {
                    await context.Response.WriteAsJsonAsync(actionResult.Value);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                }
            }
        }
    }
}