using Edulytics.Web.GameRouting;

namespace Edulytics.Tests.Acceptance;

public sealed class LessonGroundedPracticeContractTests
{
    private const string CambridgeStage6LessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:RATIO:BUILD";

    [Fact]
    public void SupportingRatioLesson_UsesFullLessonSemanticsInsteadOfGenericOperations()
    {
        var context = string.Join(" ", new[]
        {
            "Solve problems involving ratio relationships: Build the Idea.",
            "Addition and subtraction describe part-whole, change and difference relationships.",
            "Represent the situation with a concept model, number line or equation.",
            "Reason about unknowns, ratio and linked calculations.",
            "Check an additive result with the inverse operation or by estimating.",
            "Do not choose an operation from a keyword alone; verify the relationship."
        });

        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Additive Structures and Relationships",
            "Solve problems involving ratio relationships: Build the Idea",
            context,
            requireLessonGrounding: true);

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouteResolver.LessonGroundedRendererKey, route.RendererKey);
        Assert.Equal("REASONING_MODELING", route.Workspace);
        Assert.Equal("ADDITIVE_RATIO_RELATIONSHIPS", route.Mechanic);
        Assert.Equal("lesson-content-grounding", route.ClassificationSource);
    }

    [Fact]
    public void SupportingLessonWithoutVerifiedSemanticMatch_IsBlockedInsteadOfFallingBack()
    {
        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Additive Structures and Relationships",
            "Unclassified supporting lesson",
            "A short lesson body without a sufficiently specific mathematical relationship.",
            requireLessonGrounding: true);

        Assert.False(route.IsPlayable);
        Assert.Null(route.RendererKey);
        Assert.Equal("NEEDS_REVIEW", route.Workspace);
        Assert.Equal("lesson-grounding-required", route.ClassificationSource);
    }

    [Fact]
    public void NonSupportingLegacyRouting_RemainsBackwardCompatible()
    {
        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Additive Structures and Relationships",
            "Addition practice");

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouter.UniversalRendererKey, route.RendererKey);
        Assert.Equal("OPERATIONS", route.Workspace);
    }

    [Fact]
    public void StudentGame_LoadsPublishedLessonBodyBeforeRoutingSupportingPractice()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPracticeController.cs"));

        Assert.Contains("ILessonContentService lessonContent", controller, StringComparison.Ordinal);
        Assert.Contains("GetPublishedForStudentAsync", controller, StringComparison.Ordinal);
        Assert.Contains("detail.IsSupporting", controller, StringComparison.Ordinal);
        Assert.Contains("BuildLessonPracticeContext", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.Explanation", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.KeyConceptsAndRules", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.WorkedExamples", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.StepByStepSolutions", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.CommonMistakes", controller, StringComparison.Ordinal);
        Assert.Contains("lesson.QuickSummary", controller, StringComparison.Ordinal);
        Assert.Contains("requireLessonGrounding: true", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentLessonPage_UsesVerifiedCapabilityAsAvailabilityAuthority_AndGameAsOptionalPresentation()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPortalController.cs"));

        Assert.Contains("StudentLessonDetail lessonDetail", controller, StringComparison.Ordinal);
        Assert.Contains("lessonDetail.IsSupporting", controller, StringComparison.Ordinal);
        Assert.Contains("BuildLessonPracticeContext(lessonDetail)", controller, StringComparison.Ordinal);
        Assert.Contains("requireLessonGrounding: true", controller, StringComparison.Ordinal);
        Assert.Contains("LessonPracticeCapabilityResolver.TryResolve", controller, StringComparison.Ordinal);
        Assert.Contains("exactPracticeAdoptionId = workspace.SelectedCurriculumAdoptionId", controller, StringComparison.Ordinal);
        Assert.Contains("route.IsPlayable &&", controller, StringComparison.Ordinal);
        Assert.Contains("route.RendererKey is not null", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void GroundedRuntime_ContainsExactlyEightLessonRelationshipRounds_AndNoGenericOperationFallback()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v1.js"));

        Assert.Contains("data-lesson-grounded-game", view, StringComparison.Ordinal);
        Assert.Contains("lesson-grounded-practice-v1.js", view, StringComparison.Ordinal);
        Assert.Contains("count: 8", runtime, StringComparison.Ordinal);

        Assert.Contains("partWholeQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("changeQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("differenceQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("equationRepresentationQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("numberLineRepresentationQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("unknownQuantityQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("ratioLinkedQuantityQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("ratioErrorAnalysisQuestion", runtime, StringComparison.Ordinal);

        Assert.DoesNotContain("Use the operation shown.", runtime, StringComparison.Ordinal);
        Assert.DoesNotContain("JOIN_COMBINE", runtime, StringComparison.Ordinal);
        Assert.Contains("does not yet have a verified lesson-grounded question model", runtime, StringComparison.Ordinal);
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
