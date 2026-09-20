using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class PracticeUiRemediationAcceptanceTests
{
    [Fact]
    public void UnifiedPracticeShell_IsLoadedByBothLessonPracticeEntryViews()
    {
        var game = Read("src/Edulytics.Web/Views/StudentPractice/Game.cshtml");
        var attempt = Read("src/Edulytics.Web/Views/StudentPractice/LessonAttempt.cshtml");
        var css = Read("src/Edulytics.Web/wwwroot/css/practice-unified-ui-v2.css");

        Assert.Contains("practice-unified-ui-v2.css", game, StringComparison.Ordinal);
        Assert.Contains("practice-unified-ui-v2.css", attempt, StringComparison.Ordinal);
        Assert.Contains("--practice-shell-max: 1240px", css, StringComparison.Ordinal);
        Assert.Contains(".gw-head h1 + p", css, StringComparison.Ordinal);
        Assert.Contains(".gw-runtime-board", css, StringComparison.Ordinal);
        Assert.Contains("width: min(920px, 100%) !important", css, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivePracticeRuntimes_UseApprovedCurrentCharacter()
    {
        var runtimePaths = new[]
        {
            "src/Edulytics.Web/wwwroot/js/angle-explorer-preview.js",
            "src/Edulytics.Web/wwwroot/js/count-touch-preview.js",
            "src/Edulytics.Web/wwwroot/js/curriculum-specialized-practice-v3.js",
            "src/Edulytics.Web/wwwroot/js/curriculum-workspace-runtime-v2.js",
            "src/Edulytics.Web/wwwroot/js/fraction-forge-preview.js",
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-stage22.js",
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v1.js",
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v2.js",
            "src/Edulytics.Web/wwwroot/js/solid-geometry-runtime.js"
        };

        foreach (var path in runtimePaths)
        {
            var runtime = Read(path);
            Assert.Contains(
                "/images/public/edulaytiks-character.png?v=43",
                runtime,
                StringComparison.Ordinal);
            Assert.DoesNotContain(
                "/images/game/v9/eddy-guide.webp",
                runtime,
                StringComparison.Ordinal);
            Assert.DoesNotContain(
                "/images/game/v9/eddy-hint.webp",
                runtime,
                StringComparison.Ordinal);
        }

        var attempt = Read("src/Edulytics.Web/Views/StudentPractice/LessonAttempt.cshtml");
        Assert.Contains(
            "~/images/public/edulaytiks-character.png?v=43",
            attempt,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "~/images/game/v9/eddy-guide.webp",
            attempt,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LearnerFacingPractice_DoesNotExposeRuntimeImplementationLabels()
    {
        var stage22 = Read("src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-stage22.js");
        var v2 = Read("src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v2.js");
        var v1 = Read("src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v1.js");
        var attempt = Read("src/Edulytics.Web/Views/StudentPractice/LessonAttempt.cshtml");

        Assert.DoesNotContain("SERVER-VERIFIED PRACTICE", stage22, StringComparison.Ordinal);
        Assert.DoesNotContain("EXACT-SKILL PRACTICE", v2, StringComparison.Ordinal);
        Assert.DoesNotContain("LESSON-GROUNDED PRACTICE", v1, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "round.skillId + ' · ' + round.questionFamily",
            stage22,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "InteractionKind.ToString()",
            attempt,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LessonPresentationParser_PreservesMathematicalComparisonOperators()
    {
        const string examples =
            "Example B: Compare 3/5 and 5/8. " +
            "A common denominator is 40: 3/5 = 24/40 and 5/8 = 25/40, so 3/5 < 5/8. " +
            "Example C: A student says 3/8 > 1/2 because 8 > 2. " +
            "This is false: 1/2 = 4/8, and 4/8 > 3/8.";

        var parsedExamples = LessonPresentationParser.Parse(
            examples,
            sectionKind: "examples");

        var renderedExamples = string.Join(
            " ",
            parsedExamples
                .Where(x => !x.IsVisual)
                .Select(x => x.Text));

        Assert.Contains("3/5 < 5/8", renderedExamples, StringComparison.Ordinal);
        Assert.Contains("3/8 > 1/2", renderedExamples, StringComparison.Ordinal);
        Assert.Contains("4/8 > 3/8", renderedExamples, StringComparison.Ordinal);

        var parsedSummary = LessonPresentationParser.Parse(
            "Compare the values and justify <, > or =.",
            sectionKind: "summary");

        var renderedSummary = string.Join(
            " ",
            parsedSummary
                .Where(x => !x.IsVisual)
                .Select(x => x.Text));

        Assert.Contains("<, > or =", renderedSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void LessonPresentationParser_RemovesRealMarkupWithoutRemovingEncodedMath()
    {
        const string source =
            "<p>Compare 3/5 &lt; 5/8.</p>" +
            "<script>alert('no')</script>" +
            "<strong>Keep this text.</strong>";

        var plain = LessonPresentationParser.ToPlainText(source);

        Assert.Contains("3/5 < 5/8", plain, StringComparison.Ordinal);
        Assert.Contains("Keep this text.", plain, StringComparison.Ordinal);
        Assert.DoesNotContain("<p>", plain, StringComparison.Ordinal);
        Assert.DoesNotContain("<strong>", plain, StringComparison.Ordinal);
        Assert.DoesNotContain("alert", plain, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnifiedPracticeRemediation_DoesNotIntroduceCalculatorUi()
    {
        var game = Read("src/Edulytics.Web/Views/StudentPractice/Game.cshtml");
        var attempt = Read("src/Edulytics.Web/Views/StudentPractice/LessonAttempt.cshtml");
        var css = Read("src/Edulytics.Web/wwwroot/css/practice-unified-ui-v2.css");

        Assert.DoesNotContain("calculator", game, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("calculator", attempt, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("calculator", css, StringComparison.OrdinalIgnoreCase);
    }

    private static string Read(string relativePath)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine(root, relativePath));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
