namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveProductionClosureRegressionTests
{
    [Fact]
    public void CommonMistakesUseThreeBalancedDesktopColumns()
    {
        var css = Read(
            "src/Edulytics.Web/wwwroot/css/site.css");

        Assert.Contains(
            ".rich-mistake-grid",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "grid-template-columns: repeat(3, minmax(0, 1fr));",
            css,
            StringComparison.Ordinal);
    }

    [Fact]
    public void QuestionLogDistinguishesIncorrectAttemptsFromAcceptedAnswers()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptiveIntelligence/QuestionLog.cshtml");

        Assert.Contains(
            "Incorrect attempts:",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Final submitted answer:",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Closed after the allowed retry was used.",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "Guided retries:",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FreshRecoveryScaffoldIsBuiltFromFreshItem()
    {
        var service = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeV2Service.cs");
        var guidance = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptiveRemediationGuidanceEngine.cs");

        Assert.Contains(
            "guidanceEngine.BuildRecoveryWorkedExample",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "nextItem",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "BuildRoundingWorkedExample",
            guidance,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AdaptiveAnswerSubmissionIsProtectedOnClientAndServer()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml");
        var javascript = Read(
            "src/Edulytics.Web/wwwroot/js/student-lesson-practice.js");
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs");
        var repository = Read(
            "src/Edulytics.Data/Repositories/AdaptivePracticeRepository.cs");

        Assert.Contains(
            "data-adaptive-answer-form",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "if (submitted)",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "event.preventDefault()",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "AdaptivePracticeWriteConflictException",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "TurnAlreadyAnswered or",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "DbUpdateConcurrencyException",
            repository,
            StringComparison.Ordinal);
        Assert.Contains(
            "PostgresErrorCodes.UniqueViolation",
            repository,
            StringComparison.Ordinal);
        Assert.Contains(
            "DuplicateSubmissionConstraints.Contains",
            repository,
            StringComparison.Ordinal);
        Assert.Contains(
            "IsSubmissionOwnedConflict(exception.Entries)",
            repository,
            StringComparison.Ordinal);
        Assert.Contains(
            "HasSharedLearnerStateConflict(exception.Entries)",
            repository,
            StringComparison.Ordinal);
        Assert.Contains(
            "AdaptivePracticeSharedStateWriteConflictException",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "Stamp(answeredTurn)",
            repository,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LearnerLessonReaderUsesAvailableApplicationCanvas()
    {
        var css = Read(
            "src/Edulytics.Web/wwwroot/css/site.css");

        Assert.Contains(
            "learner-production-layout-20260930",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "width: min(100%, 1460px);",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "grid-template-columns: minmax(0, 1fr) minmax(16rem, 18rem);",
            css,
            StringComparison.Ordinal);
        Assert.Contains(
            "max-width: 86ch;",
            css,
            StringComparison.Ordinal);

        var studentLesson = Read(
            "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml");

        Assert.Contains(
            "lesson-reader-layout--full",
            studentLesson,
            StringComparison.Ordinal);
        Assert.Contains(
            ".lesson-reader-layout--full",
            css,
            StringComparison.Ordinal);
    }

    [Fact]
    public void QuestionLogSeparatesHistoricalPracticeSessions()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptiveIntelligence/QuestionLog.cshtml");

        Assert.Contains(
            ".GroupBy(x => x.SessionId)",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Practice session",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Technical details",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "ReasonText(item.DecisionReasonCode)",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PracticeVoiceIsCancelledBeforeSubmitAndNavigation()
    {
        var javascript = Read(
            "src/Edulytics.Web/wwwroot/js/student-lesson-practice.js");

        Assert.Contains(
            "function stopVoice()",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.addEventListener(\"pagehide\", stopVoice)",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "window.addEventListener(\"beforeunload\", stopVoice)",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "submitted = true;\n            stopVoice();",
            javascript,
            StringComparison.Ordinal);
    }

    [Fact]
    public void StudentWorkspaceNavigationUsesShortValidatedCache()
    {
        var service = Read(
            "src/Edulytics.Services/StudentPortal/StudentPortalService.cs");
        var registration = Read(
            "src/Edulytics.Web/Extensions/StudentPortalRegistrationExtensions.cs");

        Assert.Contains(
            "WorkspaceCacheLifetime",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "TimeSpan.FromSeconds(20)",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "_cache.TryGetValue",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "services.AddMemoryCache()",
            registration,
            StringComparison.Ordinal);

        var schoolValidation = service.IndexOf(
            "school.Status != SchoolStatus.Active",
            StringComparison.Ordinal);
        var cacheRead = service.IndexOf(
            "_cache.TryGetValue",
            StringComparison.Ordinal);

        Assert.True(
            schoolValidation >= 0 &&
            cacheRead > schoolValidation,
            "Access and school state must be validated before serving cached workspace data.");
    }

    private static string Read(string relativePath) =>
        File.ReadAllText(
            Path.Combine(
                FindRoot(),
                relativePath.Replace(
                    '/',
                    Path.DirectorySeparatorChar)));

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(
                Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
