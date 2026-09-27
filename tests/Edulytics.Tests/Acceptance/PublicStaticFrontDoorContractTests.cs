namespace Edulytics.Tests.Acceptance;

public sealed class PublicStaticFrontDoorContractTests
{
    [Fact]
    public void FrontDoor_BuildsExactServerRenderedSnapshotsForAllLanguages()
    {
        var root = FindRoot();
        var build = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/build.sh"));

        Assert.Contains(
            "ORIGIN=\"https://staging.edulytiks.com\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot \"pl\" \"$OUT/pl/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot \"en\" \"$OUT/en/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "snapshot \"ar\" \"$OUT/ar/index.html\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "Edulytics.Culture=c=${language}|uic=${language}",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "$ORIGIN/css/public-site-v44.css",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "$ORIGIN/js/public-site-v45.js",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "cp -R \"$SRC/.\" \"$OUT/\"",
            build,
            StringComparison.Ordinal);
        Assert.Contains(
            "<script src=\"/frontdoor.js\" defer></script>",
            build,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FrontDoor_RuntimeOnlyAddsWakeupAndBackendRouting()
    {
        var root = FindRoot();
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "public-frontdoor/frontdoor.js"));

        Assert.Contains(
            "https://staging.edulytiks.com",
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
            "installLanguageRouting",
            runtime,
            StringComparison.Ordinal);
        Assert.Contains(
            "installBackendRouting",
            runtime,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "completeTitle:",
            runtime,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "frontdoor-hero-card",
            runtime,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ReadyEndpoint_AllowsCrossOriginFrontDoorPolling()
    {
        var root = FindRoot();
        var program = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Program.cs"));

        Assert.Contains(
            "\"/health/ready\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "AccessControlAllowOrigin = \"*\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "CacheControl = \"no-store\"",
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
