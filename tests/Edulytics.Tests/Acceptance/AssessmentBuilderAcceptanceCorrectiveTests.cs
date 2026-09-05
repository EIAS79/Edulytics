using Edulytics.Web.Printing;

namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentBuilderAcceptanceCorrectiveTests
{
    [Fact]
    public void AssessmentAndQuestionMarks_AreIntegerOnlyInBackendAndUi()
    {
        var assessmentSupport = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentService.Support.cs");
        var builderService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");
        var assessmentIndex = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");
        var assessmentEdit = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Edit.cshtml");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("decimal.Truncate(value) == value", assessmentSupport, StringComparison.Ordinal);
        Assert.Contains("decimal.Truncate(value) == value", builderService, StringComparison.Ordinal);
        Assert.Contains("min=\"1\" max=\"10000\"", assessmentIndex, StringComparison.Ordinal);
        Assert.Contains("min=\"1\" max=\"10000\" step=\"1\"", assessmentEdit, StringComparison.Ordinal);
        Assert.Contains("name=\"maxScore\" type=\"number\" min=\"1\" max=\"10000\" step=\"1\"", builderView, StringComparison.Ordinal);
        Assert.Contains("name=\"maxScorePerQuestion\" type=\"number\" min=\"1\" step=\"1\"", builderView, StringComparison.Ordinal);
        Assert.DoesNotContain("step=\"0.01\"", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void ManualSolution_IsOptionalWhileStillStored()
    {
        var builderService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("solution.Length <= 4000", builderService, StringComparison.Ordinal);
        Assert.DoesNotContain("solution.Length is >= 1", builderService, StringComparison.Ordinal);
        Assert.Contains("Solution = solution", builderService, StringComparison.Ordinal);
        Assert.Contains("<textarea name=\"solution\" maxlength=\"4000\" rows=\"3\"></textarea>", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void FailedManualQuestionSave_PreservesEnteredValuesUntilSuccess()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("TempData[\"ManualQuestionSaved\"] = true", controller, StringComparison.Ordinal);
        Assert.Contains("data-manual-saved", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.setItem(key", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.getItem(key", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.removeItem(key)", builderView, StringComparison.Ordinal);
        Assert.Contains("outcomeIds", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateAssessment_ShowsClassNameWithoutInternalClassCode()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");

        var classSelectorStart = view.IndexOf("<select id=\"classGroupId\"", StringComparison.Ordinal);
        Assert.True(classSelectorStart >= 0, "Class selector was not found.");
        var classSelectorEnd = view.IndexOf("</select>", classSelectorStart, StringComparison.Ordinal);
        Assert.True(classSelectorEnd > classSelectorStart, "Class selector closing tag was not found.");

        var classSelector = view[classSelectorStart..(classSelectorEnd + "</select>".Length)];
        Assert.Contains("<option value=\"@item.Id\">@item.Name</option>", classSelector, StringComparison.Ordinal);
        Assert.DoesNotContain("@item.Code", classSelector, StringComparison.Ordinal);
    }

    [Fact]
    public void OutcomeBuilder_GuardsMissingSetup_LabelsAiCapability_AndHidesEnumErrors()
    {
        var builderService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");
        var resolver = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "NativeMathematicsOutcomeProfileResolver.cs");
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");
        var english = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.resx");
        var polish = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.pl.resx");

        Assert.Contains("if (ids.Count == 0) return false;", builderService, StringComparison.Ordinal);
        Assert.Contains("profiles.Any(x => x is null)", builderService, StringComparison.Ordinal);
        Assert.Contains("public static bool Supports(string? code, string? description)", resolver, StringComparison.Ordinal);

        Assert.Contains("LearningOutcomesSetupRequired", view, StringComparison.Ordinal);
        Assert.Contains("disabled=\"@(!hasEligibleOutcomes)\"", view, StringComparison.Ordinal);
        Assert.Contains("NativeMathematicsOutcomeProfileResolver.Supports", view, StringComparison.Ordinal);
        Assert.Contains("disabled=\"@(!aiSupported)\"", view, StringComparison.Ordinal);
        Assert.Contains("\"AiSupported\" : \"ManualOnly\"", view, StringComparison.Ordinal);

        Assert.Contains("ErrorOutcomeDoesNotMatchAssessment", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("result.Error?.ToString()", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("BuilderOperationFailed\", result.Error", controller, StringComparison.Ordinal);

        Assert.Contains("Subject Supervisor", english, StringComparison.Ordinal);
        Assert.Contains("AI supported", english, StringComparison.Ordinal);
        Assert.Contains("opiekuna przedmiotu", polish, StringComparison.Ordinal);
        Assert.Contains("Obsługiwane przez AI", polish, StringComparison.Ordinal);
    }

    [Fact]
    public void OfflinePdf_StudentContractCannotCarryAnswersOrSolutions()
    {
        var studentProperties = typeof(StudentAssessmentPaperQuestion)
            .GetProperties()
            .Select(property => property.Name)
            .ToArray();
        var teacherProperties = typeof(TeacherAssessmentAnswerKeyQuestion)
            .GetProperties()
            .Select(property => property.Name)
            .ToArray();

        Assert.Contains(nameof(StudentAssessmentPaperQuestion.Prompt), studentProperties);
        Assert.DoesNotContain(nameof(TeacherAssessmentAnswerKeyQuestion.CorrectAnswer), studentProperties);
        Assert.DoesNotContain(nameof(TeacherAssessmentAnswerKeyQuestion.Solution), studentProperties);
        Assert.Contains(nameof(TeacherAssessmentAnswerKeyQuestion.CorrectAnswer), teacherProperties);
        Assert.Contains(nameof(TeacherAssessmentAnswerKeyQuestion.Solution), teacherProperties);
    }

    [Fact]
    public void OfflinePdf_UsesTeacherWorkspaceAuthorization_AndLocalizedOfflineButtons()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");
        var renderer = ReadRepositoryFile(
            "src", "Edulytics.Web", "Printing", "AssessmentPdfRenderer.cs");
        var project = ReadRepositoryFile(
            "src", "Edulytics.Web", "Edulytics.Web.csproj");
        var docker = ReadRepositoryFile("Dockerfile");
        var english = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.resx");
        var polish = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.pl.resx");

        Assert.Contains("[HttpGet(\"student-paper.pdf\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"answer-key.pdf\")]", controller, StringComparison.Ordinal);
        Assert.Equal(
            3,
            controller.Split(
                "service.GetWorkspaceAsync(actorId, assessmentId, cancellationToken)",
                StringSplitOptions.None).Length - 1);
        Assert.Contains("AssessmentPrintDocumentFactory.CreateStudentPaper", controller, StringComparison.Ordinal);
        Assert.Contains("AssessmentPrintDocumentFactory.CreateTeacherAnswerKey", controller, StringComparison.Ordinal);
        Assert.Contains("assessment.DeliveryMode == AssessmentDeliveryMode.Offline", view, StringComparison.Ordinal);
        Assert.Contains("DownloadStudentPaper", view, StringComparison.Ordinal);
        Assert.Contains("DownloadTeacherAnswerKey", view, StringComparison.Ordinal);
        Assert.Contains("RenderStudentPaper(\n        StudentAssessmentPaper paper", renderer, StringComparison.Ordinal);
        Assert.DoesNotContain("RenderStudentPaper(AssessmentBuilderWorkspace", renderer, StringComparison.Ordinal);
        Assert.Contains("PDFsharp-MigraDoc\" Version=\"6.2.4\"", project, StringComparison.Ordinal);
        Assert.Contains("fonts-dejavu-core", docker, StringComparison.Ordinal);
        Assert.Contains("never contains correct answers", english, StringComparison.Ordinal);
        Assert.Contains("nigdy nie zawiera poprawnych odpowiedzi", polish, StringComparison.Ordinal);
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
