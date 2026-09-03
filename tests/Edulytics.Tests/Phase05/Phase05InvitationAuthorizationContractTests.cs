using Edulytics.Core.Constants;

namespace Edulytics.Tests.Phase05;

public sealed class Phase05InvitationAuthorizationContractTests
{
    [Fact]
    public void PasswordSetupInvitation_IncludesSubjectSupervisorAuthorization()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Notifications/NotificationService.cs"));

        Assert.Contains(
            "var recipientRole =",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "RoleNames.SchoolAdmin",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "RoleNames.SubjectSupervisor",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "RoleNames.Teacher",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "RoleNames.Student",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "actor.SchoolId ==",
            source,
            StringComparison.Ordinal);
        Assert.Contains(
            "recipient.SchoolId",
            source,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "actorRole ==\n                    RoleNames.SchoolAdmin;",
            source,
            StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(RoleNames.SchoolAdmin, RoleNames.SubjectSupervisor, true)]
    [InlineData(RoleNames.SchoolAdmin, RoleNames.Teacher, true)]
    [InlineData(RoleNames.SchoolAdmin, RoleNames.Student, true)]
    [InlineData(RoleNames.SubjectSupervisor, RoleNames.Teacher, true)]
    [InlineData(RoleNames.SubjectSupervisor, RoleNames.Student, true)]
    [InlineData(RoleNames.SubjectSupervisor, RoleNames.SubjectSupervisor, false)]
    [InlineData(RoleNames.Teacher, RoleNames.Student, false)]
    public void NotificationInvitationRoleMatrix_IsExplicit(
        string actorRole,
        string recipientRole,
        bool expected)
    {
        var allowed =
            actorRole == RoleNames.SchoolAdmin ||
            actorRole == RoleNames.SubjectSupervisor &&
            (
                recipientRole == RoleNames.Teacher ||
                recipientRole == RoleNames.Student
            );

        Assert.Equal(expected, allowed);
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
