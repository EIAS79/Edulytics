namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHero_UsesTransparentAssetAndTransparentStageWithoutPixelProcessing()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-mascot-transparency-v35.css"));
        var mascotLoader = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-cartoon-cleanup.js"));
        var bundle = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/PublicAssetBundleController.cs"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));

        Assert.Contains(".ed-home-v12-slide:first-child .ed-home-v12-visual", css, StringComparison.Ordinal);
        Assert.Contains("background:transparent!important", css, StringComparison.Ordinal);
        Assert.Contains("border:0!important", css, StringComparison.Ordinal);
        Assert.Contains("border-radius:0!important", css, StringComparison.Ordinal);
        Assert.Contains("box-shadow:none!important", css, StringComparison.Ordinal);
        Assert.Contains(".ed-home-v12-slide:first-child .ed-home-v16-mascot-canvas", css, StringComparison.Ordinal);
        Assert.Contains("mix-blend-mode:normal!important", css, StringComparison.Ordinal);

        Assert.Contains("/images/public/edulytics-math-mascot.png?v=38", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("public-home-mascot-background-v36.js", bundle, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/css/public-site-v38.css\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-css-v38", bundle, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/js/public-site-v38.js\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-js-v38", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v38.css", layout, StringComparison.Ordinal);
        Assert.Contains("~/js/public-site-v38.js", layout, StringComparison.Ordinal);
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
