using Edulytics.Core.Constants;

namespace Edulytics.Tests.Phase05;

public sealed class Phase05DirectStudentCreationContractTests
{
    [Fact]
    public void DirectStudentCreation_IsAtomicAndFullyProvisioned()
    {
        var root = FindRepositoryRoot();

        var filter = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Filters/DirectStudentCreationFilter.cs"));

        Assert.Contains(
            "_transactions.BeginAsync",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "RoleNames.Teacher",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "ConvertToStudentAsync",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "StudentRoleProvisioningRequest",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "GeneratePasswordSetupAsync",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "transaction.CommitAsync",
            filter,
            StringComparison.Ordinal);

        Assert.Contains(
            "transaction.RollbackAsync",
            filter,
            StringComparison.Ordinal);

        var createViewModel = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/ViewModels/SchoolUsers/SchoolUserViewModels.cs"));

        Assert.Contains("StudentNumber", createViewModel);
        Assert.Contains("FirstName", createViewModel);
        Assert.Contains("LastName", createViewModel);
        Assert.Contains("ClassGroupId", createViewModel);

        var createView = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/SchoolUsers/Create.cshtml"));

        Assert.Contains("direct-student-setup", createView);
        Assert.Contains("StudentNumber", createView);
        Assert.Contains("ClassGroupId", createView);
        Assert.Contains("Student-Classes", createView);
    }

    [Fact]
    public void DirectStudentCreation_IsRegisteredAsARequestGate()
    {
        var root = FindRepositoryRoot();

        var registration = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Extensions/SchoolUserManagementRegistrationExtensions.cs"));

        Assert.Contains(
            "DirectStudentCreationFilter",
            registration,
            StringComparison.Ordinal);

        Assert.Contains(
            "Filters.AddService",
            registration,
            StringComparison.Ordinal);

        var optionsController = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/StudentCreationOptionsController.cs"));

        Assert.Contains(
            "RoleNames.SubjectSupervisor",
            optionsController,
            StringComparison.Ordinal);

        Assert.Contains(
            "IStudentCreationClassCatalog",
            optionsController,
            StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory =
            new DirectoryInfo(AppContext.BaseDirectory);

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
