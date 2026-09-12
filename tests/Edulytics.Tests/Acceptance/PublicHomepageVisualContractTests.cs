namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomepageVisualContractTests
{
    [Fact]
    public void VisualRestoreGuard_RunsAfterPublicLocalizationAndKeepsMarkers()
    {
        var root = FindRoot();
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-visual-contract-v32.js"));

        var languageCookie = layout.IndexOf(
            "public-language-cookie-v31.js",
            StringComparison.Ordinal);
        var visualGuard = layout.IndexOf(
            "public-home-visual-contract-v32.js",
            StringComparison.Ordinal);

        Assert.True(languageCookie >= 0);
        Assert.True(visualGuard > languageCookie);
        Assert.Contains(
            ".ed-home-v14-menu-icon, .ed-home-v14-menu-dot",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "link.prepend(marker);",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void HomepageMascot_UsesExistingTransparentPublicAssetDirectly()
    {
        var root = FindRoot();
        var cleanupScript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-cartoon-cleanup.js"));
        var heroCss = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-commercial-v12.css"));
        var transparencyCss = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-mascot-transparency-v35.css"));
        var mascotPath = Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/images/public/edulytics-math-mascot.png");

        Assert.True(File.Exists(mascotPath));
        Assert.Contains(
            "/images/public/edulytics-math-mascot.png?v=41",
            cleanupScript,
            StringComparison.Ordinal);
        Assert.Contains(
            "ed-home-v40-mascot-image",
            cleanupScript,
            StringComparison.Ordinal);
        Assert.DoesNotContain("document.createElement('canvas')", cleanupScript, StringComparison.Ordinal);
        Assert.DoesNotContain("getImageData", cleanupScript, StringComparison.Ordinal);
        Assert.DoesNotContain("putImageData", cleanupScript, StringComparison.Ordinal);

        Assert.Contains(".ed-home-v12-visual{", heroCss, StringComparison.Ordinal);
        Assert.Contains("overflow:visible;", heroCss, StringComparison.Ordinal);
        Assert.Contains(".ed-home-v12-slide:nth-child(2) .ed-home-v12-visual{", heroCss, StringComparison.Ordinal);
        Assert.Contains("background:#fff;", heroCss, StringComparison.Ordinal);
        Assert.DoesNotContain("background-color:transparent!important", transparencyCss, StringComparison.Ordinal);
        Assert.DoesNotContain("box-shadow:none!important", transparencyCss, StringComparison.Ordinal);
        Assert.DoesNotContain("border-radius:0!important", transparencyCss, StringComparison.Ordinal);
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
