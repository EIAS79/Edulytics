namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHero_RemovesOnlyEdgeConnectedLightBackgroundAndUsesFreshBundle()
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

        Assert.Contains("renderMascotWithoutWhiteBackground", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("isEdgeBackground", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("visited[index]", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("pixels[index * 4 + 3] = 0", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("dataset.edMascotEdgeBackgroundRemoved", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("/images/public/edulytics-math-mascot.png?v=39", mascotLoader, StringComparison.Ordinal);

        Assert.Contains("[HttpGet(\"/css/public-site-v39.css\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-css-v39", bundle, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/js/public-site-v39.js\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-js-v39", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v39.css", layout, StringComparison.Ordinal);
        Assert.Contains("~/js/public-site-v39.js", layout, StringComparison.Ordinal);
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
