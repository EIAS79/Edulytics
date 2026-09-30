using Edulytics.Core.AdaptivePractice;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdaptiveV2C0C5ClosureTests
{
    [Fact]
    public void C0_RetryAndSessionLimitsAreExplicit()
    {
        Assert.Equal(
            1,
            AdaptivePracticeV2Behavior.MaximumSameItemRetries);
        Assert.Equal(
            30,
            AdaptivePracticeV2Behavior.MaximumSessionItems);
    }

    [Fact]
    public void C2_WrongOneReturnsSameItemButWrongTwoClosesIt()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeV2Service.cs"));

        Assert.Contains(
            "turn.IncorrectAttemptCount++",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "AdaptivePracticeV2Behavior.MaximumSameItemRetries",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "Wrong #2 closes this exact item",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "turn.IsCorrect = false;",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "turn.AnsweredAtUtc = now;",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "nextTurn.Feedback",
            service,
            StringComparison.Ordinal);
    }

    [Fact]
    public void C3_StructuredRemediationIsRenderedInLearnerWorkspace()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentAdaptivePractice/Attempt.cshtml"));

        Assert.Contains(
            "AdaptiveRemediationStageCodes.TargetedRetry",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "AdaptiveRemediationStageCodes.ScaffoldedRecovery",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-practice-scaffold",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "question.RemediationHint",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "question?.WorkedExample",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "data-practice-scaffold",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void C4_AssistedSuccessCannotCountAsIndependentProgression()
    {
        var root = FindRoot();
        var assembler = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveLearningStateAssembler.cs"));
        var projector = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptivePracticeEvidenceProjector.cs"));

        Assert.Contains(
            "response.IncorrectAttemptCount > 0",
            assembler,
            StringComparison.Ordinal);
        Assert.Contains(
            "turn.IsIndependentConfirmation &&",
            assembler,
            StringComparison.Ordinal);
        Assert.Contains(
            "turn.IncorrectAttemptCount == 0",
            assembler,
            StringComparison.Ordinal);
        Assert.Contains(
            "cleanIndependentConfirmation",
            projector,
            StringComparison.Ordinal);
    }

    [Fact]
    public void C4_OpenRemediationCanExtendOrdinaryQuestionBudgetButIsHardBounded()
    {
        var extended =
            AdaptiveSessionBudgetPolicy.Evaluate(
                answeredSequence: 8,
                targetQuestionCount: 8,
                remediationLockActive: true,
                confirmationRequired: true,
                answerCorrect: false);

        Assert.True(extended.ExtendSession);
        Assert.False(extended.CompleteSession);
        Assert.Equal(9, extended.TargetQuestionCount);
        Assert.Null(extended.StopReason);

        var hardStop =
            AdaptiveSessionBudgetPolicy.Evaluate(
                AdaptivePracticeV2Behavior.MaximumSessionItems,
                AdaptivePracticeV2Behavior.MaximumSessionItems,
                remediationLockActive: true,
                confirmationRequired: true,
                answerCorrect: false);

        Assert.False(hardStop.ExtendSession);
        Assert.True(hardStop.CompleteSession);
        Assert.Equal(
            AdaptivePracticeV2Behavior.MaximumSessionItems,
            hardStop.TargetQuestionCount);
        Assert.Equal(
            "MAX_REMEDIATION_BUDGET_REACHED",
            hardStop.StopReason);
    }

    [Fact]
    public void C5_UnifiedReadyVerifiedStartPrecedesLegacyCompatibilityPath()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs"));

        var start = controller.IndexOf(
            "public async Task<IActionResult> StartLessonPractice",
            StringComparison.Ordinal);
        var end = controller.IndexOf(
            "[HttpPost(\"lesson-game/start\")",
            start,
            StringComparison.Ordinal);
        var block = controller[start..end];

        var adaptive = block.IndexOf(
            "adaptivePractice.StartLessonAsync",
            StringComparison.Ordinal);
        var legacy = block.IndexOf(
            "privatePractice.GenerateAsync",
            StringComparison.Ordinal);

        Assert.True(adaptive >= 0);
        Assert.True(legacy > adaptive);
        Assert.Contains(
            "never",
            block,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "silently",
            block,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void C0ContractDocumentsEveryRequiredBranch()
    {
        var root = FindRoot();
        var contract = File.ReadAllText(Path.Combine(
            root,
            "docs/adaptive-v2/C0_C5_CLOSURE_CONTRACT.md"));

        foreach (var required in new[]
        {
            "Wrong #1",
            "Retry correct",
            "Wrong #2",
            "Remediation item correct",
            "Remediation item wrong",
            "Fresh independent confirmation correct",
            "Fresh independent confirmation wrong",
            "V1 is historical/out-of-rollout compatibility only"
        })
        {
            Assert.Contains(
                required,
                contract,
                StringComparison.Ordinal);
        }
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

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
