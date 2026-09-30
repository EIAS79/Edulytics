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
