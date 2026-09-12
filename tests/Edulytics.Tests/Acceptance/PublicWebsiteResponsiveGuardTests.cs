namespace Edulytics.Tests.Acceptance;

public sealed class PublicWebsiteResponsiveGuardTests
{
    [Fact]
    public void PublicLayout_LoadsResponsiveGuardAfterBundledMarketingCss()
    {
        var root = FindRoot();
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));

        var bundle = layout.IndexOf("public-site-v35.css", StringComparison.Ordinal);
        var guard = layout.IndexOf("public-home-responsive-v38.css", StringComparison.Ordinal);

        Assert.True(bundle >= 0);
        Assert.True(guard > bundle);
        Assert.Contains("asp-append-version=\"true\"", layout[guard..], StringComparison.Ordinal);
    }

    [Fact]
    public void ResponsiveGuard_ConstrainsPublicShellAndScaledExperienceArtwork()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-responsive-v38.css"));

        Assert.Contains("body.ed-public-body", css, StringComparison.Ordinal);
        Assert.Contains("overflow-x: clip !important", css, StringComparison.Ordinal);
        Assert.Contains("display: block !important", css, StringComparison.Ordinal);
        Assert.Contains(".ed-exp-v20-visual", css, StringComparison.Ordinal);
        Assert.Contains(".ed-exp-v20-scene", css, StringComparison.Ordinal);
        Assert.Contains("overflow: clip !important", css, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 390px)", css, StringComparison.Ordinal);
        Assert.Contains("transform: scale(.50) !important", css, StringComparison.Ordinal);
    }

    [Fact]
    public void ResponsiveGuard_RemovesLegacyWhiteMascotCard()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-responsive-v38.css"));

        Assert.Contains(
            ".ed-home-v12-slide:first-child .ed-home-v12-visual",
            css,
            StringComparison.Ordinal);
        Assert.Contains("background: transparent !important", css, StringComparison.Ordinal);
        Assert.Contains("border: 0 !important", css, StringComparison.Ordinal);
        Assert.Contains("box-shadow: none !important", css, StringComparison.Ordinal);
        Assert.Contains("mix-blend-mode: normal !important", css, StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
