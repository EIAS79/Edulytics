namespace Edulytics.Tests.Acceptance;

public sealed class StudentLearningDeepLinkAcceptanceTests
{
    [Fact]
    public void StudentLearningProjection_RetainsExactCurriculumAndClassIdentity()
    {
        var contracts = ReadRepositoryFile(
            "src", "Edulytics.Services", "StudentPortal", "StudentPortalContracts.cs");
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "StudentPortal", "StudentPortalService.cs");

        Assert.Contains("CurriculumAdoptionId", contracts, StringComparison.Ordinal);
        Assert.Contains("ClassGroupId", contracts, StringComparison.Ordinal);
        Assert.Contains("AcademicYearId", contracts, StringComparison.Ordinal);
        Assert.Contains("GradeLevelId", contracts, StringComparison.Ordinal);
        Assert.Contains("CurriculumAdoptionId = adoption.Id", service, StringComparison.Ordinal);
        Assert.Contains("ClassGroupId = classGroup.Id", service, StringComparison.Ordinal);
    }

    [Fact]
    public void PrivatePractice_LinkTargetsExactMyLearningContextAndMappedNodes()
    {
        var practiceContracts = ReadRepositoryFile(
            "src", "Edulytics.Services", "Practice", "StudentPrivatePracticeContracts.cs");
        var practiceService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Practice", "StudentPrivatePracticeService.cs");
        var practiceView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPractice", "Index.cshtml");
        var linkScript = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "js", "student-practice-learning-link.js");
        var portalController = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "StudentPortalController.cs");
        var portalViewModel = ReadRepositoryFile(
            "src", "Edulytics.Web", "ViewModels", "StudentPortal", "StudentPortalViewModels.cs");
        var learningView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var studentLayout = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Shared", "_StudentLayout.cshtml");

        Assert.Contains("asp-controller=\"StudentPortal\"", practiceView, StringComparison.Ordinal);
        Assert.Contains("asp-action=\"Learning\"", practiceView, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", practiceView, StringComparison.Ordinal);
        Assert.Contains("asp-route-classGroupId", practiceView, StringComparison.Ordinal);
        Assert.Contains("id=\"open-learning-context\"", practiceView, StringComparison.Ordinal);
        Assert.Contains("data-learning-focus-node-ids", practiceView, StringComparison.Ordinal);
        Assert.Contains("student-practice-learning-link.js", practiceView, StringComparison.Ordinal);
        Assert.Contains("ShowPracticeOptions", practiceView, StringComparison.Ordinal);

        Assert.Contains("OfficialOutcomeNodeIds", practiceContracts, StringComparison.Ordinal);
        Assert.Contains("context.LessonOutcomes", practiceService, StringComparison.Ordinal);
        Assert.Contains("x.OutcomeNodeId", practiceService, StringComparison.Ordinal);
        Assert.Contains("group.SelectMany(x => x.OfficialOutcomeNodeIds)", practiceService, StringComparison.Ordinal);

        Assert.Contains("focusNodeIds", linkScript, StringComparison.Ordinal);
        Assert.Contains("scope.value === 'Lesson'", linkScript, StringComparison.Ordinal);
        Assert.Contains("scope.value === 'Unit'", linkScript, StringComparison.Ordinal);
        Assert.Contains("selected-learning-node", linkScript, StringComparison.Ordinal);
        Assert.DoesNotContain("unitKey", linkScript, StringComparison.Ordinal);

        Assert.Contains("Guid? curriculumAdoptionId", portalController, StringComparison.Ordinal);
        Assert.Contains("Guid? classGroupId", portalController, StringComparison.Ordinal);
        Assert.Contains("Guid[]? focusNodeIds", portalController, StringComparison.Ordinal);
        Assert.Contains("x.CurriculumAdoptionId == curriculumAdoptionId.Value", portalController, StringComparison.Ordinal);
        Assert.Contains("x.ClassGroupId == classGroupId.Value", portalController, StringComparison.Ordinal);
        Assert.Contains("selectedContext.Nodes.Any(node => node.Id == id)", portalController, StringComparison.Ordinal);
        Assert.Contains("SelectedLearningNodeIds = selectedNodeIds", portalController, StringComparison.Ordinal);
        Assert.Contains("SelectedLearningNodeIds", portalViewModel, StringComparison.Ordinal);

        Assert.Contains("selected-curriculum-context", learningView, StringComparison.Ordinal);
        Assert.Contains("selected-learning-node", learningView, StringComparison.Ordinal);
        Assert.Contains("is-selected-context", learningView, StringComparison.Ordinal);
        Assert.Contains("is-practice-focus-exact", learningView, StringComparison.Ordinal);
        Assert.Contains("currentNode.ParentId", learningView, StringComparison.Ordinal);
        Assert.Contains("acceptance-corrective.css", studentLayout, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentLearningDeepLink_LocalizationKeysRemainInParity()
    {
        var english = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "StudentResource.resx");
        var polish = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "StudentResource.pl.resx");

        Assert.Contains("name=\"ShowPracticeOptions\"", english, StringComparison.Ordinal);
        Assert.Contains("name=\"ShowPracticeOptions\"", polish, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
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
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
