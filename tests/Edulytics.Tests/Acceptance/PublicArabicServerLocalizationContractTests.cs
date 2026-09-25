namespace Edulytics.Tests.Acceptance;

public sealed class PublicArabicServerLocalizationContractTests
{
    [Fact]
    public void Arabic_IsACanonicalServerCulture()
    {
        var root = FindRoot();
        var cultureCookie = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Localization/CultureCookie.cs"));
        var program = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Program.cs"));

        Assert.Contains(
            "new[] { \"en\", \"pl\", \"ar\" }",
            cultureCookie,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"Edulytics.PublicLanguage\"",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "CultureCookie.CreateValue(\"ar\")",
            program,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Homepage_RendersPreviouslyLeakingSectionsInArabicOnTheServer()
    {
        var root = FindRoot();
        var home = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Home/Index.cshtml"));

        foreach (var expected in new[]
        {
            "حل متكامل لتعليم الرياضيات",
            "ابدأ باستخدام Edulytics",
            "اختر دورك وانتقل إلى تسجيل الدخول.",
            "المعلم",
            "مدير المدرسة",
            "الطالب",
            "الموظفون / الإدارة",
            "اجمع المنهج والدروس والتقييم وتدريب الطالب في منصة واحدة.",
            "اطلب عرضًا تجريبيًا",
            "تواصل معنا"
        })
        {
            Assert.Contains(expected, home, StringComparison.Ordinal);
        }

        Assert.Contains(
            "string L(string en, string pl, string ar)",
            home,
            StringComparison.Ordinal);
        Assert.Contains(
            "<partial name=\"_PublicSiteHeader\" />",
            home,
            StringComparison.Ordinal);
        Assert.Contains(
            "<partial name=\"_PublicSiteFooter\" />",
            home,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SharedPublicChrome_HasCanonicalArabicLanguageFormsAndCopy()
    {
        var root = FindRoot();
        var header = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicSiteHeader.cshtml"));
        var footer = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicSiteFooter.cshtml"));
        var switcher = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLanguageSwitcher.cshtml"));

        Assert.Contains(
            "name=\"culture\" value=\"ar\"",
            header,
            StringComparison.Ordinal);
        Assert.Contains(
            "تسجيل الدخول",
            header,
            StringComparison.Ordinal);
        Assert.Contains(
            "المدارس والقيادات التعليمية",
            header,
            StringComparison.Ordinal);
        Assert.Contains(
            "الرياضيات. التعلّم. التقدم.",
            footer,
            StringComparison.Ordinal);
        Assert.Contains(
            "مصادر المحتوى والتراخيص",
            footer,
            StringComparison.Ordinal);
        Assert.Contains(
            "value=\"ar\"",
            switcher,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublicLayout_UsesServerCultureForDirectionBrandAndSeo()
    {
        var root = FindRoot();
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));

        Assert.Contains(
            "pageLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "pageDirection = isArabic ? \"rtl\" : \"ltr\"",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "Edulytics منصة رياضيات متوافقة مع المناهج",
            layout,
            StringComparison.Ordinal);
        Assert.Contains(
            "public-site-v45.js",
            layout,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "window.localStorage.getItem('edulytics.public.siteLanguage')",
            layout,
            StringComparison.Ordinal);
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
