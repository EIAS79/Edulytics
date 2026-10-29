using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-scope guard for an opt-in Grade 7 partial. This does NOT certify
/// authenticated page rendering, image delivery, accessibility or grading.
/// </summary>
public sealed class USCCSSG7SharedRichVisualHookTests
{
    [Fact]
    public void RichV2Explanation_InvokesGrade7FiguresOnlyBehindUnitOneGuard()
    {
        var root = FindRoot();
        var sharedPath = Path.Combine(root,
            "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml");
        var partialPath = Path.Combine(root,
            "src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml");

        var shared = File.ReadAllText(sharedPath);
        var gradePartial = File.ReadAllText(partialPath);

        Assert.Contains("Model.LessonCode.StartsWith(\"PED:US-CCSS-MATH:G7:U01:L\"",
            shared, StringComparison.Ordinal);
        Assert.Contains("<partial name=\"_USCCSSG7ScaleConstructionFigures\" model=\"Model\" />",
            shared, StringComparison.Ordinal);
        Assert.Contains("_RichLessonVisual", shared, StringComparison.Ordinal);
        Assert.Contains("_ => null", gradePartial, StringComparison.Ordinal);

        // Exact Grade 7 ownership: one image, accessible description and
        // actionable drawing prompt per Unit 1 lesson. Duplicate switch arms
        // would otherwise break Razor compilation or hide student prompts.
        var assetSection = gradePartial.Split(
            "var assetPath = Model.LessonCode switch", StringSplitOptions.None)[1]
            .Split("var altText = Model.LessonCode switch", StringSplitOptions.None)[0];
        var altSection = gradePartial.Split(
            "var altText = Model.LessonCode switch", StringSplitOptions.None)[1]
            .Split("var constructionPrompt = Model.LessonCode switch", StringSplitOptions.None)[0];
        var promptSection = gradePartial.Split(
            "var constructionPrompt = Model.LessonCode switch", StringSplitOptions.None)[1]
            .Split("@if (assetPath is not null)", StringSplitOptions.None)[0];

        for (var lesson = 1; lesson <= 13; lesson++)
        {
            var code = $"PED:US-CCSS-MATH:G7:U01:L{lesson:00}";
            Assert.Contains(code, gradePartial, StringComparison.Ordinal);
            Assert.Equal(1, assetSection.Split(code, StringSplitOptions.None).Length - 1);
            Assert.Equal(1, altSection.Split(code, StringSplitOptions.None).Length - 1);
            Assert.Equal(1, promptSection.Split(code, StringSplitOptions.None).Length - 1);

            // Real, local first-party asset must back each whitelisted URL.
            // This is a source/asset contract, not an authenticated HTTP test.
            var match = Regex.Match(assetSection,
                Regex.Escape("\"" + code + "\"") +
                @"\s*=>\s*""(?<url>/curriculum/us-ccss-g7/geometry/[a-z0-9-]+\.svg)""");
            Assert.True(match.Success, $"Missing safe SVG mapping for {code}");
            var url = match.Groups["url"].Value;
            var relativePath = url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var diskPath = Path.Combine(root, "src/Edulytics.Web/wwwroot", relativePath);
            Assert.True(File.Exists(diskPath), $"Missing first-party SVG file {diskPath}");
            var svgText = File.ReadAllText(diskPath);
            var svg = XDocument.Parse(svgText);
            XNamespace ns = "http://www.w3.org/2000/svg";
            Assert.Equal(ns + "svg", svg.Root?.Name);
            Assert.Contains(svg.Descendants(ns + "title"), x => !string.IsNullOrWhiteSpace(x.Value));
            Assert.Contains(svg.Descendants(ns + "desc"), x => x.Value.Length >= 60);
            Assert.Equal("img", (string?)svg.Root?.Attribute("role"));
            Assert.DoesNotContain("<script", svgText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("<foreignObject", svgText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("javascript:", svgText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("onload=", svgText, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string FindRoot()
    {
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Edulytics.sln")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
