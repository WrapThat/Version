using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace WrapThat.Version.Tests;

[TestFixture]
public class WebApplicationExtensionsTest : IDisposable
{
    private readonly WebApplication _app;
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;


    public WebApplicationExtensionsTest()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(0);
        });

        _app = builder.Build();
        _app.MapVersionApiEndpoints();
        _app.StartAsync().GetAwaiter().GetResult();

        var port = new Uri(_app.Urls.First(url => url.StartsWith("http://"))).Port;
        _baseUrl = $"http://localhost:{port}";

        _httpClient = new HttpClient();
    }

    [TestCase("/api/info")]
    [TestCase("/api/info/version")]
    [TestCase("/api/info/productversion")]
    [TestCase("/api/info/shields/version")]
    [TestCase("/api/info/shields/productversion")]
    [TestCase("/api/info/status")]
    public async Task Endpoint_ReturnsNonEmptySuccessResponse(string endpoint)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.Not.Null & Is.Not.Empty, $"Endpoint {endpoint} returned an empty or null response.");
    }

    [Test]
    public async Task Endpoints_ShouldHaveAllowAnonymous()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(0); // 0 betyr at en ledig port vil bli valgt automatisk
        });
        var app = builder.Build();
        app.MapVersionApiEndpoints();
        await app.StartAsync();

        var dataSource = app.Services.GetRequiredService<EndpointDataSource>();
        var endpoints = dataSource.Endpoints;
        var allHaveAllowAnonymous = endpoints.All(endpoint => endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null);

        Assert.That(allHaveAllowAnonymous, Is.True, "Ikke alle endepunkter er merket med AllowAnonymous");
    }

    public void Dispose()
    {
        _app.StopAsync().GetAwaiter().GetResult();
        _httpClient.Dispose();
    }
}