using System;
using System.IO;

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
        for (var lesson = 1; lesson <= 13; lesson++)
        {
            var code = $"PED:US-CCSS-MATH:G7:U01:L{lesson:00}";
            Assert.Contains(code, gradePartial, StringComparison.Ordinal);
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
