namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHero_UsesTransparentPngDirectlyWithoutPixelProcessing()
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

        Assert.Contains("ed-home-v40-mascot-stage", css, StringComparison.Ordinal);
        Assert.Contains("background:none!important", css, StringComparison.Ordinal);
        Assert.Contains("background-color:transparent!important", css, StringComparison.Ordinal);
        Assert.Contains("border:0!important", css, StringComparison.Ordinal);
        Assert.Contains("border-radius:0!important", css, StringComparison.Ordinal);
        Assert.Contains("box-shadow:none!important", css, StringComparison.Ordinal);
        Assert.Contains(".ed-home-v40-mascot-image", css, StringComparison.Ordinal);

        Assert.Contains("document.createElement('img')", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("ed-home-v40-mascot-image", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("/images/public/edulytics-math-mascot.png?v=40", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("document.createElement('canvas')", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("getImageData", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("putImageData", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("Uint8Array", mascotLoader, StringComparison.Ordinal);

        Assert.Contains("[HttpGet(\"/css/public-site-v40.css\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-css-v40", bundle, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/js/public-site-v40.js\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-js-v40", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v40.css", layout, StringComparison.Ordinal);
        Assert.Contains("~/js/public-site-v40.js", layout, StringComparison.Ordinal);
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
