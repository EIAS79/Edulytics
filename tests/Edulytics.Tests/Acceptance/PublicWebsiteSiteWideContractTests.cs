using System.Text.RegularExpressions;

namespace Edulytics.Tests.Acceptance;

public sealed class PublicWebsiteSiteWideContractTests
{
    [Fact]
    public void EveryRegisteredMarketingPage_HasEnglishPolishAndArabicContent()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/HomeController.cs"));

        var keys = Regex.Matches(
                controller,
                "\\[\\\"(?<key>(?:product|teachers|parents|schools|students|company)/[^\\\"]+)\\\"\\]")
            .Select(match => match.Groups["key"].Value)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(keys);

        foreach (var language in new[] { "en", "pl", "ar" })
        {
            var catalog = File.ReadAllText(Path.Combine(
                root,
                $"src/Edulytics.Web/wwwroot/js/public-content-pages-v27-{language}.js"));

            foreach (var key in keys)
            {
                Assert.Contains(
                    $"\"{key}\"",
                    catalog,
                    StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void PublicLayout_LoadsBundledArabicRtlAndGlobalUiGuardsLast()
    {
        var root = FindRoot();
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));

        var publicCss = layout.IndexOf(
            "public-site-v35.css",
            StringComparison.Ordinal);
        var publicRuntime = layout.IndexOf(
            "public-site-v36.js",
            StringComparison.Ordinal);
        var contentRuntime = layout.IndexOf(
            "public-content-v33.js",
            StringComparison.Ordinal);
        var globalUi = layout.IndexOf(
            "public-site-global-ui-v30.js",
            StringComparison.Ordinal);
        var languageCookie = layout.IndexOf(
            "public-language-cookie-v31.js",
            StringComparison.Ordinal);

        Assert.True(publicCss >= 0);
        Assert.True(publicRuntime >= 0);
        Assert.True(contentRuntime >= 0);
        Assert.True(globalUi > publicRuntime);
        Assert.True(globalUi > contentRuntime);
        Assert.True(languageCookie > globalUi);
        Assert.Contains(
            "Edulytics.PublicLanguage",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "edulytics.public.siteLanguage",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "pageDirection = isArabic ? \"rtl\" : \"ltr\"",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "document.documentElement.dir = 'rtl'",
            layout,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublicHomeVisualContract_RestoresMascotCtaAndMegaMenuMarkers()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-home-visual-contract-v32.css"));
        var bundleController = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/PublicAssetBundleController.cs"));

        Assert.Contains(
            "public-home-visual-contract-v32.css",
            bundleController,
            StringComparison.Ordinal);
        Assert.Contains(
            "[HttpGet(\"/css/public-site-v33.css\")]",
            bundleController,
            StringComparison.Ordinal);
        Assert.Contains(
            "[HttpGet(\"/css/public-site-v35.css\")]",
            bundleController,
            StringComparison.Ordinal);
        Assert.Contains(
            "[HttpGet(\"/js/public-site-v33.js\")]",
            bundleController,
            StringComparison.Ordinal);
        Assert.Contains(
            "[HttpGet(\"/js/public-site-v36.js\")]",
            bundleController,
            StringComparison.Ordinal);
        Assert.Contains(
            ".ed-home .ed-home-v12-slide:first-child .ed-home-v12-visual",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "background: transparent !important;",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            ".ed-home .ed-home-header .ed-home-cta",
            css,
            StringComparison.Ordinal);
        Assert.Contains("#2f66e8", css, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("#ff7a1a", css, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            ".ed-home .ed-home-v14-menu-icon",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            ".ed-home .ed-home-v14-menu-dot",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "visibility: visible !important;",
            css,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GlobalPublicUi_ProtectsEveryPublicShellFromMailClientAndStuckArabicState()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-site-global-ui-v30.js"));

        Assert.Contains(
            "a[href^=\"mailto:\"]",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/contact/request-demo'",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/contact/support'",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/contact/sales-enquiry'",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "'/contact/message'",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.localStorage.removeItem(storageKey)",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-public-arabic-switch",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublicContactAndLegalPages_UseThePublicShell()
    {
        var root = FindRoot();

        foreach (var relativePath in new[]
        {
            "src/Edulytics.Web/Views/Contact/Index.cshtml",
            "src/Edulytics.Web/Views/Contact/Help.cshtml",
            "src/Edulytics.Web/Views/Contact/Inquiry.cshtml",
            "src/Edulytics.Web/Views/Home/Index.cshtml",
            "src/Edulytics.Web/Views/Home/LearningBuiltForUnderstanding.cshtml",
            "src/Edulytics.Web/Views/Home/PublicContentPage.cshtml",
            "src/Edulytics.Web/Views/Home/ContentSources.cshtml"
        })
        {
            var view = File.ReadAllText(Path.Combine(root, relativePath));
            Assert.Contains(
                "_PublicLayout",
                view,
                StringComparison.Ordinal);
        }

        var sources = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Home/ContentSources.cshtml"));

        Assert.Contains("_PublicSiteHeader", sources, StringComparison.Ordinal);
        Assert.Contains("_PublicSiteFooter", sources, StringComparison.Ordinal);
        Assert.Contains("data-public-ar=\"contentSourcesTitle\"", sources, StringComparison.Ordinal);
    }

    [Fact]
    public void LegacyDemoRoute_IsRetiredIntoProtectedContactFlow()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/OnboardingController.cs"));

        Assert.Contains(
            "Redirect(\"/contact/request-demo\")",
            controller,
            StringComparison.Ordinal);
        Assert.Contains("[HttpPost(\"\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[ValidateAntiForgeryToken]", controller, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "SubmitDemoRequestAsync",
            controller,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LoginStillPresentsExactlyTheFourSchoolAccountRoles()
    {
        var root = FindRoot();
        var login = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Account/Login.cshtml"));

        foreach (var role in new[]
        {
            "RoleNames.SchoolAdmin",
            "RoleNames.SubjectSupervisor",
            "RoleNames.Teacher",
            "RoleNames.Student"
        })
        {
            Assert.Contains(role, login, StringComparison.Ordinal);
        }

        Assert.DoesNotContain("Parents</strong>", login, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Education leaders</strong>", login, StringComparison.OrdinalIgnoreCase);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
