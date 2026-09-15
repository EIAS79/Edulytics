namespace Edulytics.Tests.Acceptance;

public sealed class PublicQualityAssuranceSectionContractTests
{
    [Fact]
    public void PublicQualitySection_UsesApprovedEddyAssetAndOutcomeFocusedQualitySignals()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-trust-v1.js"));
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-trust-v1.css"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));
        var eddy = Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/images/public/eddy-certificate.png");

        Assert.True(File.Exists(eddy));
        Assert.Contains("/images/public/eddy-certificate.png", script, StringComparison.Ordinal);
        Assert.Contains("1,200+", script, StringComparison.Ordinal);
        Assert.Contains("Automated security checks", script, StringComparison.Ordinal);
        Assert.Contains("Data integrity validation", script, StringComparison.Ordinal);
        Assert.Contains("Infrastructure security", script, StringComparison.Ordinal);
        Assert.Contains("Backup & recovery verified", script, StringComparison.Ordinal);
        Assert.Contains("Production readiness", script, StringComparison.Ordinal);
        Assert.Contains("الجودة والتحقق التقني", script, StringComparison.Ordinal);
        Assert.Contains("JAKOŚĆ I WERYFIKACJA TECHNICZNA", script, StringComparison.Ordinal);
        Assert.Contains("document.querySelector(\".ed-home-footer\")", script, StringComparison.Ordinal);
        Assert.Contains("beforebegin", script, StringComparison.Ordinal);

        foreach (var implementationName in new[] { "CodeQL", "PostgreSQL", "Trivy" })
        {
            Assert.DoesNotContain(
                implementationName,
                script,
                StringComparison.OrdinalIgnoreCase);
        }

        Assert.Contains(".ed-quality-section", css, StringComparison.Ordinal);
        Assert.Contains(".ed-quality-card-grid", css, StringComparison.Ordinal);
        Assert.Contains("[dir=\"rtl\"]", css, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 760px)", css, StringComparison.Ordinal);

        Assert.Contains("public-trust-v1.css", layout, StringComparison.Ordinal);
        Assert.Contains("public-trust-v1.js", layout, StringComparison.Ordinal);
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
