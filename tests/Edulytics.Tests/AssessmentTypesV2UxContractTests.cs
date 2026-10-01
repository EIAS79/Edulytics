namespace Edulytics.Tests;

public sealed class AssessmentTypesV2UxContractTests
{
    [Fact]
    public void Builder_IsTypeAware_AndUsesTypeSpecificPublishActions()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");
        var resources = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.resx");

        Assert.Contains("AssessmentBuilderHomework", view, StringComparison.Ordinal);
        Assert.Contains("AssessmentBuilderWorksheet", view, StringComparison.Ordinal);
        Assert.Contains("PublishHomework", view, StringComparison.Ordinal);
        Assert.Contains("PublishWorksheet", view, StringComparison.Ordinal);
        Assert.Contains("PublishHomework", resources, StringComparison.Ordinal);
        Assert.Contains("PublishWorksheet", resources, StringComparison.Ordinal);
    }

    [Fact]
    public void Builder_Readiness_RejectsHomeworkWithoutAFutureDueTime()
    {
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");

        Assert.Contains("homeworkScheduleValid", service, StringComparison.Ordinal);
        Assert.Contains("DueAtUtc.Value > DateTime.UtcNow", service, StringComparison.Ordinal);
        Assert.Contains("BuilderHomeworkDueRequired", service, StringComparison.Ordinal);
        Assert.Contains("BuilderHomeworkDuePast", service, StringComparison.Ordinal);
    }

    [Fact]
    public void AnswerKey_IsAvailableForOnlineAndOfflineTasks()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        var methodStart = controller.IndexOf(
            "public async Task<IActionResult> AnswerKeyPdf",
            StringComparison.Ordinal);
        var methodEnd = controller.IndexOf(
            "[HttpPost",
            methodStart,
            StringComparison.Ordinal);

        Assert.True(methodStart >= 0);
        Assert.True(methodEnd > methodStart);

        var method = controller[methodStart..methodEnd];
        Assert.DoesNotContain(
            "DeliveryMode != AssessmentDeliveryMode.Offline",
            method,
            StringComparison.Ordinal);

        Assert.Contains("asp-action=" + '"' + "AnswerKeyPdf" + '"', view, StringComparison.Ordinal);
        Assert.Contains("DownloadTeacherAnswerKey", view, StringComparison.Ordinal);
    }

    [Fact]
    public void UnscoredBuilder_DoesNotPresentQuestionMarks()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("if (isScored)", view, StringComparison.Ordinal);
        Assert.Contains("name=" + '"' + "maxScore" + '"' + " value=" + '"' + "0" + '"', view, StringComparison.Ordinal);
        Assert.Contains("Unscored", view, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentDashboard_UsesTasksToDoAndKeepsOfficialResultStatesExamOnly()
    {
        var viewModel = ReadRepositoryFile(
            "src", "Edulytics.Web", "ViewModels", "StudentPortal", "StudentPortalViewModels.cs");
        var dashboard = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Dashboard.cshtml");

        Assert.Contains("!x.IsDeadlinePassed", viewModel, StringComparison.Ordinal);
        Assert.Contains("x.AssessmentType == Edulytics.Core.Enums.AssessmentType.Exam", viewModel, StringComparison.Ordinal);
        Assert.Contains("S[" + '"' + "TasksToDo" + '"' + "]", dashboard, StringComparison.Ordinal);
        Assert.Contains("TaskTypeExamTest", dashboard, StringComparison.Ordinal);
        Assert.Contains("TaskTypeHomework", dashboard, StringComparison.Ordinal);
        Assert.Contains("TaskTypeWorksheet", dashboard, StringComparison.Ordinal);
        Assert.Contains("OpenHomework", dashboard, StringComparison.Ordinal);
        Assert.Contains("OpenWorksheet", dashboard, StringComparison.Ordinal);
    }

    [Fact]
    public void UserFacingAssessmentTimes_UseSchoolLocal12HourFormatWithoutTimezoneIds()
    {
        var details = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Details.cshtml");
        var index = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");
        var student = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Assessments.cshtml");
        var edit = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Edit.cshtml");

        foreach (var source in new[] { details, index, student })
        {
            Assert.Contains("h:mm tt", source, StringComparison.Ordinal);
            Assert.Contains("CultureInfo.InvariantCulture", source, StringComparison.Ordinal);
        }

        Assert.DoesNotContain("(@Model.Details.SchoolTimeZoneId)", details, StringComparison.Ordinal);
        Assert.DoesNotContain("(@Model.Workspace.SchoolTimeZoneId)", index, StringComparison.Ordinal);
        Assert.DoesNotContain("(@Model.Workspace.SchoolTimeZoneId)", student, StringComparison.Ordinal);
        Assert.DoesNotContain("School time zone: @Model.Details.SchoolTimeZoneId", edit, StringComparison.Ordinal);
        Assert.DoesNotContain("Times use the school time zone:", index, StringComparison.Ordinal);
    }

    [Fact]
    public void AssessmentDetails_UsesTypeSpecificActionsAndCompactContext()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentsController.cs");
        var details = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Details.cshtml");

        Assert.Contains("SuccessHomeworkCreated", controller, StringComparison.Ordinal);
        Assert.Contains("SuccessWorksheetCreated", controller, StringComparison.Ordinal);
        Assert.Contains("EditHomework", details, StringComparison.Ordinal);
        Assert.Contains("EditWorksheet", details, StringComparison.Ordinal);
        Assert.Contains("AssessmentBuilderHomeworkLink", details, StringComparison.Ordinal);
        Assert.Contains("AssessmentBuilderWorksheetLink", details, StringComparison.Ordinal);
        Assert.Contains("DeleteHomework", details, StringComparison.Ordinal);
        Assert.Contains("DeleteWorksheet", details, StringComparison.Ordinal);
        Assert.Contains("assessment-meta-grid", details, StringComparison.Ordinal);
        Assert.DoesNotContain("@Model.SubjectName · @Model.ClassName · @Model.TermName", details, StringComparison.Ordinal);
    }

    [Fact]
    public void Builder_AiGenerationIsLockedUntilDeliverySettingsAreSaved()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("TempData[\"DeliverySettingsSaved\"] = \"true\"", controller, StringComparison.Ordinal);
        Assert.Contains("TempData.Peek(\"DeliverySettingsSaved\")", view, StringComparison.Ordinal);
        Assert.Contains("ed-ai-fieldset", view, StringComparison.Ordinal);
        Assert.Contains("Save delivery settings first.", view, StringComparison.Ordinal);
        Assert.Contains("sessionStorage", view, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentTaskSubmission_LocksHomeworkAndWorksheetAndBlocksHistoryReopen()
    {
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "StudentAssessmentDeliveryService.cs");
        var take = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "TakeAssessment.cshtml");
        var submitted = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "AssessmentSubmitted.cshtml");
        var list = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Assessments.cshtml");

        Assert.Contains("AssessmentType.Homework or AssessmentType.Worksheet", service, StringComparison.Ordinal);
        Assert.Contains("AssessmentAttemptStatus.Submitted or AssessmentAttemptStatus.Completed", service, StringComparison.Ordinal);
        Assert.Contains("pageshow", take, StringComparison.Ordinal);
        Assert.Contains("edulytics:assessment-submitted:", submitted, StringComparison.Ordinal);
        Assert.Contains("Cannot edit it after submission.", submitted, StringComparison.Ordinal);
        Assert.Contains("Cannot edit it after completion.", submitted, StringComparison.Ordinal);
        Assert.DoesNotContain("Open again", list, StringComparison.Ordinal);
    }

    [Fact]
    public void DeliveryLabels_AreShort()
    {
        var resources = ReadRepositoryFile(
            "src", "Edulytics.Web", "Resources", "AssessmentBuilderResource.resx");

        Assert.Contains("<data name=\"DeliveryOnline\" xml:space=\"preserve\"><value>Online</value></data>", resources, StringComparison.Ordinal);
        Assert.Contains("<data name=\"DeliveryOffline\" xml:space=\"preserve\"><value>Offline</value></data>", resources, StringComparison.Ordinal);
        Assert.DoesNotContain("student answers and submits in Edulytics", resources, StringComparison.Ordinal);
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
