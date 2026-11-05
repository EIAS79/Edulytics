using System.Net;
using Edulytics.Tests.Phase03;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D HTTP integration smoke: real ASP.NET static middleware serves the
/// local Grade 7 first-party SVG assets. This does not claim learner E2E.
/// </summary>
public sealed class UsCcssG7StageDHttpAssetIntegrationTests
    : IClassFixture<EdulyticsWebFactory>
{
    private readonly EdulyticsWebFactory _factory;

    public UsCcssG7StageDHttpAssetIntegrationTests(EdulyticsWebFactory factory)
        => _factory = factory;

    [Fact]
    public async Task AllGrade7SvgAssetsAreServedByRealHttpPipeline()
    {
        var root = FindRepositoryRoot();
        var assets = Path.Combine(root, "src/Edulytics.Web/wwwroot",
            "curriculum/us-ccss-g7/geometry");
        var files = Directory.GetFiles(assets, "*.svg", SearchOption.TopDirectoryOnly);
        Assert.Equal(41, files.Length);

        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });

        foreach (var path in files.Order(StringComparer.Ordinal))
        {
            var url = "/curriculum/us-ccss-g7/geometry/" +
                Uri.EscapeDataString(Path.GetFileName(path));
            using var response = await client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("image/svg+xml", response.Content.Headers.ContentType?.MediaType);
            var svg = await response.Content.ReadAsStringAsync();
            Assert.Contains("<svg", svg, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("<script", svg, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string FindRepositoryRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "global.json")) &&
                Directory.Exists(Path.Combine(dir.FullName, "src/Edulytics.Web/wwwroot")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("Repository root unavailable to G7 HTTP asset test.");
    }
}