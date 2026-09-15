namespace Edulytics.Tests.Acceptance;

public sealed class PublicLegalPagesContractTests
{
    [Fact]
    public void PublicLegalRoutes_AreAnonymousAndUseDedicatedViews()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/LegalController.cs"));

        Assert.Contains("[AllowAnonymous]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/legal/privacy\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/legal/terms\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"/legal/data-processing-agreement\")]", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void PublicLegalPages_UsePublicShellAndSupportArabicRtl()
    {
        var root = FindRoot();
        foreach (var relativePath in new[]
        {
            "src/Edulytics.Web/Views/Legal/Privacy.cshtml",
            "src/Edulytics.Web/Views/Legal/Terms.cshtml",
            "src/Edulytics.Web/Views/Legal/DataProcessingAgreement.cshtml",
            "src/Edulytics.Web/Views/Home/ContentSources.cshtml"
        })
        {
            var view = File.ReadAllText(Path.Combine(root, relativePath));
            Assert.Contains("_PublicLayout", view, StringComparison.Ordinal);
            Assert.Contains("_PublicSiteHeader", view, StringComparison.Ordinal);
            Assert.Contains("_PublicSiteFooter", view, StringComparison.Ordinal);
            Assert.Contains("ed-legal-ar", view, StringComparison.Ordinal);
            Assert.DoesNotContain("Last updated", view, StringComparison.OrdinalIgnoreCase);
        }

        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-legal-v1.css"));
        Assert.Contains(".ed-site-ar .ed-legal-ar", css, StringComparison.Ordinal);
        Assert.Contains("direction: rtl", css, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PublicLegalCopy_UsesApprovedCompanyIdentityWithoutPublicProviderBrandNames()
    {
        var root = FindRoot();
        var legalFiles = new[]
        {
            "src/Edulytics.Web/Views/Legal/Privacy.cshtml",
            "src/Edulytics.Web/Views/Legal/Terms.cshtml",
            "src/Edulytics.Web/Views/Legal/DataProcessingAgreement.cshtml"
        };

        var combined = string.Join("\n", legalFiles.Select(path => File.ReadAllText(Path.Combine(root, path))));

        Assert.Contains("OUR-CS Sp. z o.o.", combined, StringComparison.Ordinal);
        Assert.Contains("Aleje Jerozolimskie 81, lok. 7.10", combined, StringComparison.Ordinal);
        Assert.Contains("02-001 Warszawa, Poland", combined, StringComparison.Ordinal);
        Assert.Contains("info@our-cs.com", combined, StringComparison.Ordinal);
        Assert.Contains("bank transfer", combined, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("schools and educational organisations", combined, StringComparison.OrdinalIgnoreCase);

        foreach (var disallowedPublicBrand in new[]
        {
            "Google Analytics",
            "OpenAI",
            "Meta Pixel",
            "Hotjar",
            "Render",
            "Neon",
            "Wise",
            "Stripe"
        })
        {
            Assert.DoesNotContain(disallowedPublicBrand, combined, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Footer_LinksToFourLegalResourcesInsteadOfPlainText()
    {
        var root = FindRoot();
        var footer = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicSiteFooter.cshtml"));

        Assert.Contains("href=\"/legal/privacy\"", footer, StringComparison.Ordinal);
        Assert.Contains("href=\"/legal/terms\"", footer, StringComparison.Ordinal);
        Assert.Contains("href=\"/legal/content-sources\"", footer, StringComparison.Ordinal);
        Assert.Contains("href=\"/legal/data-processing-agreement\"", footer, StringComparison.Ordinal);
        Assert.Contains("ed-home-footer-legal", footer, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "<span>@L(\"Privacy · Terms · Content licences\"",
            footer,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublicPrivacyAndTerms_KeepApprovedOperatingModel()
    {
        var root = FindRoot();
        var privacy = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Legal/Privacy.cshtml"));
        var terms = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Legal/Terms.cshtml"));

        Assert.Contains("created or authorised by the relevant school", privacy, StringComparison.Ordinal);
        Assert.Contains("does not currently use advertising, behavioural profiling or marketing tracking technologies", privacy, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not intended to make final high-impact educational decisions", privacy, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not currently sold directly to individual students or parents", terms, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("bank transfer against invoice", terms, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("should not be used as the sole basis for significant decisions concerning a student", terms, StringComparison.OrdinalIgnoreCase);
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
