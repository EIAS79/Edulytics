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
        var footerCss = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-footer-layout-v2.css"));

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

        Assert.Contains("public-footer-layout-v2.css", layout, StringComparison.Ordinal);
        Assert.Contains("margin: 30px auto 0 !important;", footerCss, StringComparison.Ordinal);
        Assert.Contains("justify-content: space-between !important;", footerCss, StringComparison.Ordinal);
        Assert.Contains("margin-inline-start: auto !important;", footerCss, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 760px)", footerCss, StringComparison.Ordinal);
        Assert.Contains("[dir=\"rtl\"]", footerCss, StringComparison.Ordinal);

        Assert.Equal(
            1,
            CountOccurrences(footer, "href=\"/legal/content-sources\""));

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

    private static int CountOccurrences(string value, string token)
    {
        var count = 0;
        var index = 0;

        while ((index = value.IndexOf(token, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += token.Length;
        }

        return count;
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
