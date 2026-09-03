namespace Edulytics.Tests.Phase39;

public sealed class Phase39AcademicUxContractTests
{
    [Fact]
    public void ClassLabels_AreCurriculumAwareAndSharedAcrossAcademicUx()
    {
        var root = FindRepositoryRoot();
        var query = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Curriculum/ExplicitCurriculumLevelUiQuery.cs"));
        var studentCatalog = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/StudentSetup/StudentCreationClassCatalog.cs"));
        var studentOptions = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentCreationOptionsController.cs"));

        Assert.Contains("public string DisplayLabel", query, StringComparison.Ordinal);
        Assert.Contains("AcademicProgramName", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumLevelLabel", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumPathway", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumAdoptionId", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("GetAdoptedCurriculumContextsAsync", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("CurriculumDisplayLabel", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("label = x.DisplayLabel", studentOptions, StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssignments_SupportMultipleClassesWithoutExposingSubjectChoice()
    {
        var root = FindRepositoryRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/Phase39AcademicRelationshipsController.cs"));
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));

        Assert.Contains("Guid[]? classGroupIds", controller, StringComparison.Ordinal);
        Assert.Contains("selectedClassIds", controller, StringComparison.Ordinal);
        Assert.Contains("activeClassIds", controller, StringComparison.Ordinal);
        Assert.Contains("AssignTeacherAsync", controller, StringComparison.Ordinal);
        Assert.Contains("teacherClass.multiple = true", javascript, StringComparison.Ordinal);
        Assert.Contains("teacherClass.name = \"classGroupIds\"", javascript, StringComparison.Ordinal);
        Assert.Contains("subjectCell.hidden = true", javascript, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentCreation_UsesUserManagementAsTheNormalEntryPoint()
    {
        var root = FindRepositoryRoot();
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));
        var filter = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Filters/DirectStudentCreationFilter.cs"));

        Assert.Contains("createstudentprofile", javascript, StringComparison.Ordinal);
        Assert.Contains("profileForm.hidden = true", javascript, StringComparison.Ordinal);
        Assert.Contains("/School/Users/Create", javascript, StringComparison.Ordinal);
        Assert.Contains("Change student class enrollment", javascript, StringComparison.Ordinal);
        Assert.Contains("_transactions.BeginAsync", filter, StringComparison.Ordinal);
        Assert.Contains("ConvertToStudentAsync", filter, StringComparison.Ordinal);
        Assert.Contains("RollbackAsync", filter, StringComparison.Ordinal);
        Assert.Contains("CommitAsync", filter, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics repository root not found.");
    }
}
