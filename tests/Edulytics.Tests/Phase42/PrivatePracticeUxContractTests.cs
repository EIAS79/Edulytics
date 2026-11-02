namespace Edulytics.Tests.Phase42;

public sealed class PrivatePracticeUxContractTests
{
    private static readonly string Root = FindRoot();

    [Fact]
    public void RichLessonCommonMistakes_DoNotRepeatCommonMistakeLabelPerCard()
    {
        var view = Read(
            "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml");

        Assert.DoesNotContain(
            "var mistakeLabel",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "<h3>@mistakeLabel</h3>",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "L[\"CommonMistakes\"]",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GenerateMyPractice_DoesNotRouteLessonScopeIntoAdaptivePractice()
    {
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs");

        var generateStart = controller.IndexOf(
            "public async Task<IActionResult> Generate(",
            StringComparison.Ordinal);
        var nextAction = controller.IndexOf(
            "[HttpPost(\"lesson/start\")",
            generateStart,
            StringComparison.Ordinal);

        Assert.True(generateStart >= 0 && nextAction > generateStart);

        var generateAction = controller[generateStart..nextAction];

        Assert.Contains(
            "privatePractice.GenerateAsync",
            generateAction,
            StringComparison.Ordinal);
        Assert.Contains(
            "mode = PersonalTestMode",
            generateAction,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "StartLessonPractice(",
            generateAction,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "adaptivePractice.StartLessonAsync",
            generateAction,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PrivatePracticeScopeLimits_AreFiveTenFifteenAndClientClampsInput()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentPractice/Index.cshtml");

        Assert.Contains(
            "StudentPrivatePracticeScope.Lesson\" data-question-limit=\"5\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "StudentPrivatePracticeScope.Unit\" data-question-limit=\"10\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "StudentPrivatePracticeScope.WholeCurriculum\" data-question-limit=\"15\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "count.max = String(limit)",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "if (current > limit) count.value = String(limit)",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LessonDifficultyOptions_AreDisabledFromActualLessonCapabilities()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentPractice/Index.cshtml");

        Assert.Contains(
            "data-supported-difficulties",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "option.disabled = isLesson",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "lesson?.addEventListener('change', apply)",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PersonalTest_SubmitsAllAnswersBeforeShowingResults()
    {
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs");
        var view = Read(
            "src/Edulytics.Web/Views/StudentPractice/Attempt.cshtml");

        Assert.Contains(
            "SubmitPersonalTest",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "practice.AnswerAsync",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "practice.SubmitAsync",
            controller,
            StringComparison.Ordinal);

        Assert.Contains(
            "asp-action=\"SubmitPersonalTest\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "name=\"answers[@questionIndex]\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "name=\"attemptItemIds[@questionIndex]\"",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LessonPage_StillKeepsAdaptivePracticeEntryPoint()
    {
        var lessonView = Read(
            "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml");
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs");

        Assert.Contains(
            "asp-action=\"StartLessonPractice\"",
            lessonView,
            StringComparison.Ordinal);
        Assert.Contains(
            "adaptivePractice.StartLessonAsync",
            controller,
            StringComparison.Ordinal);
    }

    private static string Read(string relative) =>
        File.ReadAllText(Path.Combine(Root, relative));

    private static string FindRoot()
    {
        for (
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            directory is not null;
            directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
