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
        Assert.DoesNotContain(
            "Technical details",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "ReasonText(item.DecisionReasonCode)",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AdaptiveFeedbackStaysWithEddyUntilLearnerSelectsNext()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml");
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs");

        Assert.Contains(
            "ViewData[\"AdaptiveReview\"] as AdaptivePracticeReviewView",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-practice-review",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-practice-next",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "data-practice-feedback",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "reviewSequence = sequence",
            controller,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AdaptiveReviewKeepsSubmittedAnswerAndReplacesCheckWithNext()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml");
        var contracts = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeServiceContracts.cs");
        var service = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeV2Service.cs");

        Assert.Contains(
            "string SubmittedAnswer",
            contracts,
            StringComparison.Ordinal);
        Assert.Contains(
            "turn.SubmittedAnswer ??",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "value=\"@reviewAnswer\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "readonly",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-practice-next",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AdaptiveWrongRetrySpeaksHintBeforeRepeatingQuestion()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml");
        var javascript = Read(
            "src/Edulytics.Web/wwwroot/js/student-lesson-practice.js");

        Assert.Contains(
            "data-practice-retry-feedback",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "const retryFeedback = document.querySelector('[data-practice-retry-feedback=\"true\"]')",
            javascript,
            StringComparison.Ordinal);

        var branchStart = javascript.IndexOf(
            "if (retryFeedback)",
            StringComparison.Ordinal);
        var branchEnd = javascript.IndexOf(
            "return parts;",
            branchStart,
            StringComparison.Ordinal);
        var retryBranch = javascript[
            branchStart..branchEnd];

        Assert.True(branchStart >= 0);
        Assert.True(branchEnd > branchStart);
        Assert.True(
            retryBranch.IndexOf(
                "parts.push(hint.textContent.trim())",
                StringComparison.Ordinal) <
            retryBranch.IndexOf(
                "parts.push(question.textContent.trim())",
                StringComparison.Ordinal));
    }

    [Fact]
    public void StudentQuestionLogUsesLearnerFacingAdaptiveLanguage()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptiveIntelligence/QuestionLog.cshtml");
        var contracts = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptiveIntelligenceV2Contracts.cs");

        Assert.Contains(
            "Correct first attempt",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Correct after retry",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Closed incorrect",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "You answered recent questions correctly, so Edulytics increased the challenge.",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "CurriculumLabel",
            contracts,
            StringComparison.Ordinal);
        Assert.Contains(
            "LessonTitle",
            contracts,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "bounded increase in mathematical complexity",
            view,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "Technical details",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void CompletedAdaptivePracticeIsAlwaysSilent()
    {
        var view = Read(
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml");
        var javascript = Read(
            "src/Edulytics.Web/wwwroot/js/student-lesson-practice.js");

        Assert.Contains(
            "data-practice-terminal",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "const terminal = document.querySelector(\"[data-practice-terminal]\")",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "if (voice && !terminal)",
            javascript,
            StringComparison.Ordinal);
        Assert.Contains(
            "nextButton.addEventListener(\"click\", stopVoice)",
            javascript,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AdaptiveComplexityIsNormalizedToGeneratedCapability()
    {
        var service = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeV2Service.cs");
        var generator = Read(
            "src/Edulytics.Services/AdaptivePractice/AdaptiveVerifiedItemGenerator.cs");

        Assert.Contains(
            "NormalizeDecisionToTruthfulCapability",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "ClampComplexityToCognitiveBand",
            generator,
            StringComparison.Ordinal);
        Assert.Contains(
            "PracticeCognitiveDifficulty.Standard =>",
            generator,
            StringComparison.Ordinal);
        Assert.Contains(
            "Math.Min(requestedComplexity, 55)",
            generator,
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
    public void LessonPracticeAvailabilityStopsAfterUsableAdoption()
    {
        var controller = Read(
            "src/Edulytics.Web/Controllers/StudentPortalController.cs");

        Assert.Contains(
            "HasResolvedPracticeAvailability(",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "exactPracticeAdoptionId.HasValue ||",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "pilotAdoptionId.HasValue;",
            controller,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "gameAdoptionId.HasValue &&\n                exactPracticeAdoptionId.HasValue &&\n                pilotAdoptionId.HasValue",
            controller,
            StringComparison.Ordinal);
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
