namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHeroStage_IsTransparentAndServedFromFreshCssBundle()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-mascot-transparency-v35.css"));
        var bundle = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/PublicAssetBundleController.cs"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));

        Assert.Contains(".ed-home-v12-slide:first-child .ed-home-v12-visual", css, StringComparison.Ordinal);
        Assert.Contains("background:transparent!important", css, StringComparison.Ordinal);
        Assert.Contains("border:0!important", css, StringComparison.Ordinal);
        Assert.Contains("box-shadow:none!important", css, StringComparison.Ordinal);

        Assert.Contains("public-home-mascot-transparency-v35.css", bundle, StringComparison.Ordinal);
        Assert.Contains("/css/public-site-v35.css", bundle, StringComparison.Ordinal);
        Assert.Contains("public-css-v35", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v35.css", layout, StringComparison.Ordinal);
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
