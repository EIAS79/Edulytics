namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdaptiveIntelligenceV2Phase10To13Tests
{
    [Fact]
    public void Phase10_MyNextSteps_UsesExistingEvidenceDrivenEvaluation()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/StudentSelfEvaluationService.cs"));
        var adaptive = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveIntelligenceV2Service.cs"));

        Assert.Contains("BuildNextSteps(", service, StringComparison.Ordinal);
        Assert.Contains("WeakPrerequisiteSkillKeys", service, StringComparison.Ordinal);
        Assert.Contains("EnableDirectNextSteps", adaptive, StringComparison.Ordinal);
        Assert.Contains("studentSelfEvaluation.GetAsync", adaptive, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase11_QuestionLog_IsBackedByPersistedAdaptiveEvidence()
    {
        var root = FindRoot();
        var adaptive = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveIntelligenceV2Service.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentAdaptiveIntelligence/QuestionLog.cshtml"));

        Assert.Contains("GetSessionsForStudentAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("GetTurnsForSessionsAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("GetDecisionSnapshotsForSessionsAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("EnableQuestionLog", adaptive, StringComparison.Ordinal);
        Assert.Contains("Why this question", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Misconception focus", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Technical details", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase12_LiveClassroom_UsesAuthorizedAnalyticsAndRecentAdaptiveSessions()
    {
        var root = FindRoot();
        var adaptive = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveIntelligenceV2Service.cs"));
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AdaptiveIntelligenceV2Controller.cs"));

        Assert.Contains("GetStudentsEvaluationAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("GetRecentSessionsAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("GetMisconceptionStatesForStudentsAsync", adaptive, StringComparison.Ordinal);
        Assert.Contains("EnableLiveClassroom", adaptive, StringComparison.Ordinal);
        Assert.Contains("[Authorize(Policy = \"AnalyticsRead\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"live\")]", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase13_DiagnosticV2_UsesExistingAdaptiveDiagnosticEngine()
    {
        var root = FindRoot();
        var adaptive = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveIntelligenceV2Service.cs"));
        var registration = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Extensions/AdaptivePracticeV2RegistrationExtensions.cs"));

        Assert.Contains("EnableDiagnosticV2", adaptive, StringComparison.Ordinal);
        Assert.Contains("diagnosticEngine.DecideNext", adaptive, StringComparison.Ordinal);
        Assert.Contains("AssessmentPurpose.Diagnostic", adaptive, StringComparison.Ordinal);
        Assert.Contains("AddSingleton<AdaptiveDiagnosticAssessmentEngine>()", registration, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase10To13_EndpointsAreNoStoreAndStudentScopedWhereRequired()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AdaptiveIntelligenceV2Controller.cs"));

        Assert.Contains("[Authorize(Policy = \"StudentPortal\")]", controller, StringComparison.Ordinal);
        Assert.Contains("ResponseCache(NoStore = true", controller, StringComparison.Ordinal);
        Assert.Contains("student/adaptive-intelligence", controller, StringComparison.Ordinal);
        Assert.Contains("school/analytics/adaptive", controller, StringComparison.Ordinal);
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
