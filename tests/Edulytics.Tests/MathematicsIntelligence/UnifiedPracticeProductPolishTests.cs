namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UnifiedPracticeProductPolishTests
{
    [Fact]
    public void PracticeUi_RestoresEstablishedExamWorkspaceInsideStudentPortal()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml"));

        Assert.Contains("Layout = \"_StudentLayout\"", view, StringComparison.Ordinal);
        Assert.Contains("gw-game gw-universal", view, StringComparison.Ordinal);
        Assert.Contains("EDULYTICS PRACTICE", view, StringComparison.Ordinal);
        Assert.Contains("gw-mission", view, StringComparison.Ordinal);
        Assert.Contains("gw-eddy", view, StringComparison.Ordinal);
        Assert.DoesNotContain("up-hero", view, StringComparison.Ordinal);
        Assert.DoesNotContain("up-card", view, StringComparison.Ordinal);
    }

    [Fact]
    public void PracticeUi_RestoresQuestionHintAndFeedbackVoiceHooks()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml"));

        Assert.Contains("data-practice-sound", view, StringComparison.Ordinal);
        Assert.Contains("data-practice-question", view, StringComparison.Ordinal);
        Assert.Contains("data-practice-hint", view, StringComparison.Ordinal);
        Assert.DoesNotContain("data-practice-feedback", view, StringComparison.Ordinal);
        Assert.Contains("data-practice-review", view, StringComparison.Ordinal);
        Assert.Contains("data-practice-next", view, StringComparison.Ordinal);
        Assert.Contains("practice-voice-runtime.js", view, StringComparison.Ordinal);
        Assert.Contains("student-lesson-practice.js", view, StringComparison.Ordinal);
    }

    [Fact]
    public void PracticeVoice_ReviewReadsOnlyEddyFeedbackBeforeNextQuestion()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/student-lesson-practice.js"));

        Assert.Contains(
            "const review = document.querySelector(\"[data-practice-review]\")",
            script,
            StringComparison.Ordinal);

        var reviewBranch = script.IndexOf("if (review)", StringComparison.Ordinal);
        var reviewReturn = script.IndexOf(
            "return parts;",
            reviewBranch,
            StringComparison.Ordinal);
        var questionSpeech = script.IndexOf(
            "parts.push(question.textContent.trim())",
            reviewReturn,
            StringComparison.Ordinal);

        Assert.True(reviewBranch >= 0);
        Assert.True(reviewReturn > reviewBranch);
        Assert.Contains(
            "feedback?.textContent.trim()",
            script[reviewBranch..reviewReturn],
            StringComparison.Ordinal);
        Assert.Contains(
            "hint?.textContent.trim()",
            script[reviewBranch..reviewReturn],
            StringComparison.Ordinal);
        Assert.True(
            questionSpeech > reviewReturn,
            "The answered question must not be spoken before Eddy feedback on review pages.");
    }

    [Fact]
    public void WrongAnswer_IsCommittedAsGuidedRetryWithoutAdvancingSequence()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeV2Service.cs"));

        var wrong = service.IndexOf("if (!correct)", StringComparison.Ordinal);
        var retryReturn = service.IndexOf(
            "return AdaptivePracticeAnswerResult.Success(",
            wrong,
            StringComparison.Ordinal);
        var nextSequence = service.IndexOf(
            "var nextSequence = sequence + 1;",
            wrong,
            StringComparison.Ordinal);

        Assert.True(wrong >= 0);
        Assert.True(retryReturn > wrong);
        Assert.True(nextSequence > retryReturn);
        Assert.Contains("turn.IncorrectAttemptCount++", service, StringComparison.Ordinal);
        Assert.Contains("turn.LastIncorrectAnswer = trimmed", service, StringComparison.Ordinal);
        Assert.Contains(
            "BuildSessionViewAsync",
            service[wrong..nextSequence],
            StringComparison.Ordinal);
    }

    [Fact]
    public void CorrectedRetry_StillRequiresFreshIndependentConfirmation()
    {
        var root = FindRoot();
        var assembler = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveLearningStateAssembler.cs"));
        var engine = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveNextItemDecisionEngine.cs"));

        Assert.Contains("turn.IncorrectAttemptCount > 0", assembler, StringComparison.Ordinal);
        Assert.Contains("IsCorrect: false", assembler, StringComparison.Ordinal);
        Assert.Contains(
            "MisconceptionConfirmationRequired",
            engine,
            StringComparison.Ordinal);
        Assert.Contains(
            "isIndependentConfirmation: true",
            engine,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LessonHelp_DoesNotRenderGoogleOrYoutubeSearchButtons()
    {
        var root = FindRoot();
        var partial = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml"));

        Assert.DoesNotContain("Search the web", partial, StringComparison.Ordinal);
        Assert.DoesNotContain("Search YouTube", partial, StringComparison.Ordinal);
        Assert.DoesNotContain("suggestion.Url", partial, StringComparison.Ordinal);
        Assert.Contains("youtube-nocookie.com/embed/", partial, StringComparison.Ordinal);
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

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
