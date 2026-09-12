namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHero_RemovesStageAndLargestBakedLightBackgroundFromFreshBundle()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-mascot-transparency-v35.css"));
        var cleanup = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-mascot-background-v36.js"));
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

        Assert.Contains("removeLargestLightComponent", cleanup, StringComparison.Ordinal);
        Assert.Contains("largestComponentSize", cleanup, StringComparison.Ordinal);
        Assert.Contains("minimumBackgroundSize", cleanup, StringComparison.Ordinal);
        Assert.Contains("pixels[index * 4 + 3] = 0", cleanup, StringComparison.Ordinal);
        Assert.Contains("dataset.edMascotBackgroundCleaned", cleanup, StringComparison.Ordinal);
        Assert.Contains("mixBlendMode = 'multiply'", cleanup, StringComparison.Ordinal);

        Assert.Contains("public-home-mascot-transparency-v35.css", bundle, StringComparison.Ordinal);
        Assert.Contains("public-home-mascot-background-v36.js", bundle, StringComparison.Ordinal);
        Assert.Contains("/js/public-site-v37.js", bundle, StringComparison.Ordinal);
        Assert.Contains("public-js-v37", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v35.css", layout, StringComparison.Ordinal);
        Assert.Contains("~/js/public-site-v37.js", layout, StringComparison.Ordinal);
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
