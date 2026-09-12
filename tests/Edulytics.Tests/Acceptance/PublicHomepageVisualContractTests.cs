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
    public void HomepageMascot_UsesApprovedTransparentAssetWithoutWhiteCard()
    {
        var root = FindRoot();
        var cleanupScript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-cartoon-cleanup.js"));
        var visualCss = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-visual-contract-v32.css"));
        var mascotPath = Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/images/brand/edulytics-mascot-final.png");

        Assert.True(File.Exists(mascotPath));
        Assert.Contains(
            "/images/brand/edulytics-mascot-final.png?v=33",
            cleanupScript,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "/images/public/edulytics-math-mascot.png",
            cleanupScript,
            StringComparison.Ordinal);
        Assert.Contains(
            "ed-home-v16-mascot-canvas ed-home-v17-mascot-image",
            cleanupScript,
            StringComparison.Ordinal);
        Assert.Contains(
            "background: transparent !important;",
            visualCss,
            StringComparison.Ordinal);
        Assert.Contains(
            "box-shadow: none !important;",
            visualCss,
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
