namespace Edulytics.Tests.Acceptance;

public sealed class PublicHomeLearningPhilosophyContractTests
{
    [Fact]
    public void HomeLearningPhilosophy_IsCanonicalLocalizedAndArabicRtl()
    {
        var root = FindRoot();
        var home = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Home/Index.cshtml"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-home-philosophy-v34.js"));

        Assert.Contains("ed-home-philosophy", home, StringComparison.Ordinal);
        Assert.DoesNotContain("Why Edulytics?", home, StringComparison.OrdinalIgnoreCase);

        Assert.Contains("public-home-philosophy-v34.js", layout, StringComparison.Ordinal);
        Assert.Contains("The Edulytics Learning Philosophy", runtime, StringComparison.Ordinal);
        Assert.Contains("Filozofia uczenia się Edulytics", runtime, StringComparison.Ordinal);
        Assert.Contains("فلسفة التعلّم في Edulytics", runtime, StringComparison.Ordinal);
        Assert.Contains("المنهج في صميم التعلّم", runtime, StringComparison.Ordinal);
        Assert.Contains("المعلّم يبقى صاحب القرار", runtime, StringComparison.Ordinal);
        Assert.Contains("كل نتيجة تقود إلى الخطوة التالية", runtime, StringComparison.Ordinal);
        Assert.Contains("section.setAttribute('dir', 'rtl')", runtime, StringComparison.Ordinal);
        Assert.Contains("section.setAttribute('lang', 'ar')", runtime, StringComparison.Ordinal);
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
