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
