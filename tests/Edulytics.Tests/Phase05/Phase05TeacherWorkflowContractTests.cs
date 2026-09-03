using Edulytics.Core.Constants;

namespace Edulytics.Tests.Phase05;

public sealed class Phase05TeacherWorkflowContractTests
{
    [Fact]
    public void SubjectSupervisorTeacherCreate_DoesNotRequireStudentOnlyFields()
    {
        var root = FindRepositoryRoot();
        var viewModel = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/ViewModels/SchoolUsers/SchoolUserViewModels.cs"));

        Assert.Contains(
            "Microsoft.AspNetCore.Mvc.ModelBinding.Validation",
            viewModel,
            StringComparison.Ordinal);
        Assert.Contains(
            "[ValidateNever]\n    public string StudentNumber",
            viewModel,
            StringComparison.Ordinal);
        Assert.Contains(
            "[ValidateNever]\n    public string FirstName",
            viewModel,
            StringComparison.Ordinal);
        Assert.Contains(
            "[ValidateNever]\n    public string LastName",
            viewModel,
            StringComparison.Ordinal);

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/SchoolUsersController.cs"));

        Assert.Contains(
            "new CreateSchoolUserRequest(\n                model.Email,\n                model.Role)",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "new UserInvitationDeliveryRequest(",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"initial\"",
            controller,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAndStudent_DoNotExposeOrdinaryCrossRolePicker()
    {
        var root = FindRepositoryRoot();
        var details = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/SchoolUsers/Details.cshtml"));

        Assert.Contains(
            "var isOperationalAccount =",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "user.Role == RoleNames.Teacher",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "user.Role == RoleNames.Student",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "!isOperationalAccount",
            details,
            StringComparison.Ordinal);
        Assert.Contains(
            "var canEditStudentSetup =",
            details,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "StudentSetupUnavailable",
            details,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssignment_RemainsClassBasedWithMathematicsInferred()
    {
        var root = FindRepositoryRoot();
        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/AcademicStructureController.cs"));
        var view = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));

        Assert.Contains(
            "AssignTeacherToActiveMathematicsAsync",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-for=\"TeacherUserId\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-for=\"ClassGroupId\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "Mathematics is inferred",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AssessmentResult_RecordsTheEvaluatingUserId()
    {
        var root = FindRepositoryRoot();
        var resultEntity = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Core/Entities/AssessmentResult.cs"));
        var commands = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Assessments/AssessmentService.Commands.cs"));

        Assert.Contains(
            "public Guid EnteredByUserId",
            resultEntity,
            StringComparison.Ordinal);
        Assert.Contains(
            "EnteredByUserId = request.Actor.Id",
            commands,
            StringComparison.Ordinal);
        Assert.Contains(
            "[\"enteredByUserId\"] = result.EnteredByUserId",
            commands,
            StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
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
            "Edulytics repository root not found.");
    }
}
