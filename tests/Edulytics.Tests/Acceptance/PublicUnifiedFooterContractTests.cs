namespace Edulytics.Tests.Acceptance;

public sealed class PublicUnifiedFooterContractTests
{
    [Fact]
    public void PublicFooterNormalizer_ProvidesFourLegalLinksAcrossLegacyAndSharedFooters()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-footer-normalize-v1.js"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));
        var footer = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicSiteFooter.cshtml"));

        foreach (var route in new[]
        {
            "/legal/privacy",
            "/legal/terms",
            "/legal/content-sources",
            "/legal/data-processing-agreement"
        })
        {
            Assert.Contains(route, script, StringComparison.Ordinal);
            Assert.Contains($"href=\"{route}\"", footer, StringComparison.Ordinal);
        }

        Assert.Contains("ed-home-footer-legal", script, StringComparison.Ordinal);
        Assert.Contains("legacyLegalText.replaceWith(legalNav)", script, StringComparison.Ordinal);
        Assert.Contains("/help", script, StringComparison.Ordinal);

        var normalizer = layout.IndexOf(
            "public-footer-normalize-v1.js",
            StringComparison.Ordinal);
        var globalUi = layout.IndexOf(
            "public-site-global-ui-v30.js",
            StringComparison.Ordinal);
        var trust = layout.IndexOf(
            "public-trust-v1.js",
            StringComparison.Ordinal);

        Assert.True(normalizer >= 0);
        Assert.True(globalUi > normalizer);
        Assert.True(trust > globalUi);
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
