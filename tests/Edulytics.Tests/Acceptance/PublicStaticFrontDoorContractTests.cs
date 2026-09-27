namespace Edulytics.Tests.Acceptance;

public sealed class PublicStaticFrontDoorContractTests
{
    [Fact]
    public void FrontDoor_IsStaticMultilingualAndWakesTheBackendInTheBackground()
    {
        var root = FindRoot();
        var html = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/index.html"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/frontdoor.js"));
        var build = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/build.sh"));

        Assert.Contains("data-lang="pl"", html, StringComparison.Ordinal);
        Assert.Contains("data-lang="en"", html, StringComparison.Ordinal);
        Assert.Contains("data-lang="ar"", html, StringComparison.Ordinal);

        Assert.Contains(
            "https://edulytics-4346.onrender.com",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "/health/ready?frontdoor=",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "ensureBackendReady",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-backend-path",
            html,
            StringComparison.Ordinal);

        Assert.Contains(
            "حل متكامل لتعليم الرياضيات",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "ابدأ باستخدام Edulytics",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "Get started with Edulytics",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "Zacznij korzystać z Edulytics",
            runtime,
            StringComparison.Ordinal);

        Assert.Contains(
            "css/public-home.css",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "cp -R "$SRC/images/." "$OUT/images/"",
            build,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "Render",
            html,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ReadyEndpoint_AllowsCrossOriginFrontDoorPolling()
    {
        var root = FindRoot();
        var program = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Program.cs"));

        Assert.Contains(
            ""/health/ready"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "AccessControlAllowOrigin = "*"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "CacheControl = "no-store"",
            program,
            StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
