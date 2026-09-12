namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeMascotTransparencyContractTests
{
    [Fact]
    public void MascotHero_FixesWhiteCardAtSourceAndUsesTransparentPngDirectly()
    {
        var root = FindRoot();
        var heroCss = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-commercial-v12.css"));
        var mascotCss = File.ReadAllText(Path.Combine(
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

        Assert.Contains(".ed-home-v12-visual{", heroCss, StringComparison.Ordinal);
        Assert.Contains("overflow:visible;", heroCss, StringComparison.Ordinal);
        Assert.Contains(".ed-home-v12-slide:nth-child(2) .ed-home-v12-visual{", heroCss, StringComparison.Ordinal);
        Assert.Contains("background:#fff;", heroCss, StringComparison.Ordinal);
        Assert.Contains("border:14px solid", heroCss, StringComparison.Ordinal);
        Assert.DoesNotContain("background:none!important", mascotCss, StringComparison.Ordinal);
        Assert.DoesNotContain("border:0!important", mascotCss, StringComparison.Ordinal);
        Assert.DoesNotContain("box-shadow:none!important", mascotCss, StringComparison.Ordinal);
        Assert.Contains(".ed-home-v40-mascot-image", mascotCss, StringComparison.Ordinal);

        Assert.Contains("document.createElement('img')", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("ed-home-v40-mascot-image", mascotLoader, StringComparison.Ordinal);
        Assert.Contains("/images/public/edulytics-math-mascot.png?v=41", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("document.createElement('canvas')", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("getImageData", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("putImageData", mascotLoader, StringComparison.Ordinal);
        Assert.DoesNotContain("Uint8Array", mascotLoader, StringComparison.Ordinal);

        Assert.Contains("[HttpGet(\"/css/public-site-v41.css\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-css-v41", bundle, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/js/public-site-v41.js\")]", bundle, StringComparison.Ordinal);
        Assert.Contains("public-js-v41", bundle, StringComparison.Ordinal);
        Assert.Contains("~/css/public-site-v41.css", layout, StringComparison.Ordinal);
        Assert.Contains("~/js/public-site-v41.js", layout, StringComparison.Ordinal);
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
