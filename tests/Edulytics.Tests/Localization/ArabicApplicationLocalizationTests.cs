using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Edulytics.Tests.Localization;

public sealed class ArabicApplicationLocalizationTests
{
    [Fact]
    public void EveryNeutralResource_HasCompleteArabicParity()
    {
        var root = FindRoot();
        var resources = Path.Combine(root, "src", "Edulytics.Web", "Resources");
        var neutral = Directory.GetFiles(resources, "*.resx")
            .Where(x =>
                !x.EndsWith(".pl.resx", StringComparison.OrdinalIgnoreCase) &&
                !x.EndsWith(".ar.resx", StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(22, neutral.Length);

        foreach (var sourcePath in neutral)
        {
            var arabicPath =
                sourcePath[..^".resx".Length] + ".ar.resx";

            Assert.True(
                File.Exists(arabicPath),
                $"Missing Arabic resource: {Path.GetFileName(arabicPath)}");

            var source = Read(sourcePath);
            var arabic = Read(arabicPath);

            Assert.Equal(
                source.Keys.OrderBy(x => x, StringComparer.Ordinal),
                arabic.Keys.OrderBy(x => x, StringComparer.Ordinal));

            foreach (var key in source.Keys)
            {
                Assert.False(
                    string.IsNullOrWhiteSpace(arabic[key]),
                    $"Blank Arabic resource: {Path.GetFileName(arabicPath)}::{key}");

                Assert.Equal(
                    Placeholders(source[key]),
                    Placeholders(arabic[key]));
            }
        }
    }

    [Fact]
    public void Login_AcceptsArabicAsCanonicalCulture()
    {
        var root = FindRoot();
        var program = File.ReadAllText(
            Path.Combine(root, "src", "Edulytics.Web", "Program.cs"));
        var controller = File.ReadAllText(
            Path.Combine(root, "src", "Edulytics.Web", "Controllers", "AccountController.cs"));
        var login = File.ReadAllText(
            Path.Combine(root, "src", "Edulytics.Web", "Views", "Account", "Login.cshtml"));

        Assert.Contains(
            "culture is not (\"en\" or \"pl\" or \"ar\")",
            program,
            StringComparison.Ordinal);
        Assert.Contains(
            "culture is not (\"en\" or \"pl\" or \"ar\")",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "var isArabic = string.Equals(loginCulture, \"ar\"",
            login,
            StringComparison.Ordinal);
        Assert.Contains(
            "اختر نوع حسابك",
            login,
            StringComparison.Ordinal);
        Assert.Contains(
            "مدير المدرسة",
            login,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AuthenticatedLayouts_EnableRtlAndLanguageSwitching()
    {
        var root = FindRoot();

        foreach (var relative in new[]
        {
            "src/Edulytics.Web/Views/Shared/_AppLayout.cshtml",
            "src/Edulytics.Web/Views/Shared/_StudentLayout.cshtml",
            "src/Edulytics.Web/Views/Shared/_StudentAccessLayout.cshtml"
        })
        {
            var content = File.ReadAllText(Path.Combine(root, relative));
            Assert.Contains("dir=", content, StringComparison.Ordinal);
            Assert.Contains("arabic-rtl.css", content, StringComparison.Ordinal);
            Assert.Contains("_AppLanguageSwitcher", content, StringComparison.Ordinal);
        }

        var switcher = File.ReadAllText(
            Path.Combine(root, "src", "Edulytics.Web", "Views", "Shared", "_AppLanguageSwitcher.cshtml"));
        Assert.Contains("(Code: \"ar\", Label: \"العربية\")", switcher, StringComparison.Ordinal);
    }

    [Fact]
    public void UiCulture_DoesNotSelectOrTranslateLessonBody()
    {
        var root = FindRoot();
        var service = File.ReadAllText(
            Path.Combine(root, "src", "Edulytics.Services", "LessonContent", "LessonContentService.cs"));

        Assert.Contains(
            "SelectAcademicContent(\n                    content.Translations,\n                    context.FrameworkCode)",
            service.Replace("\r\n", "\n", StringComparison.Ordinal),
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "CurrentUICulture",
            service,
            StringComparison.Ordinal);
    }

    private static Dictionary<string, string> Read(string path) =>
        XDocument.Load(path)
            .Root!
            .Elements("data")
            .ToDictionary(
                x => x.Attribute("name")!.Value,
                x => x.Element("value")?.Value ?? string.Empty,
                StringComparer.Ordinal);

    private static string[] Placeholders(string value) =>
        Regex.Matches(value, @"\{\d+(?:[^}]*)?\}")
            .Select(x => x.Value)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

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