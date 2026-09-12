namespace Edulytics.Tests.Acceptance;

public sealed class CurriculumWorkspaceRuntimeRegressionTests
{
    [Fact]
    public void UniversalPractice_LoadsRuntimeCorrectionsAfterV2()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));

        var runtimeV2 = view.IndexOf(
            "curriculum-workspace-runtime-v2.js",
            StringComparison.Ordinal);
        var runtimeFixes = view.IndexOf(
            "curriculum-workspace-runtime-v3-fixes.js",
            StringComparison.Ordinal);

        Assert.True(runtimeV2 >= 0);
        Assert.True(runtimeFixes > runtimeV2);
    }

    [Fact]
    public void RuntimeCorrections_RenderAllClockNumbersAndKeepHintsLessonSpecific()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/curriculum-workspace-runtime-v3-fixes.js"));

        Assert.Contains(
            "for (var number = 1; number <= 12; number += 1)",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "text === genericHint && latestLessonGuidance",
            script,
            StringComparison.Ordinal);
        Assert.Contains(
            "var lessonHint = hintPrefix + latestLessonGuidance",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void RuntimeCorrections_ProtectCompleteCorrectFeedbackFromNextQuestionCancellation()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/curriculum-workspace-runtime-v3-fixes.js"));

        Assert.Contains("feedbackProtected = true", script, StringComparison.Ordinal);
        Assert.Contains(
            "if (!feedbackProtected) nativeCancel();",
            script,
            StringComparison.Ordinal);
        Assert.Contains("utterance.onend = release", script, StringComparison.Ordinal);
        Assert.Contains(
            "text.indexOf(correctPrefix) === 0",
            script,
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
