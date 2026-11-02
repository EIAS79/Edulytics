using System.Reflection;
using Edulytics.Core.Constants;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Edulytics.Tests.Phase05;

public sealed class Phase05UiContractTests
{
    [Fact]
    public void SchoolRolePicker_IsActorScopedAndNeverExposesSuperAdmin()
    {
        var root = FindRepositoryRoot();

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/SchoolUsersController.cs"));

        Assert.Contains(
            "User.IsInRole(RoleNames.SchoolAdmin)",
            controller);
        Assert.Contains(
            "User.IsInRole(RoleNames.SubjectSupervisor)",
            controller);
        Assert.Contains(
            "new(RoleNames.SubjectSupervisor, \"RoleSubjectSupervisor\")",
            controller);
        Assert.Contains(
            "new(RoleNames.Teacher, \"RoleTeacher\")",
            controller);
        Assert.Contains(
            "new(RoleNames.Student, \"RoleStudent\")",
            controller);
        Assert.DoesNotContain(
            "new(RoleNames.SuperAdmin",
            controller);
    }

    [Fact]
    public void LegacySupervisorSubjectAssignmentEndpoint_FailsClosed()
    {
        var root = FindRepositoryRoot();

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/"
                + "SubjectSupervisorAssignmentsController.cs"));

        Assert.Contains(
            "[Route(\"school/subject-supervisors\")]",
            controller);
        Assert.DoesNotContain(
            "ISubjectSupervisorAssignmentService",
            controller);
        Assert.Contains(
            "public IActionResult Index() => NotFound();",
            controller);
        Assert.Contains(
            "public IActionResult Assign() => NotFound();",
            controller);
        Assert.Contains(
            "public IActionResult Remove(Guid assignmentId) => NotFound();",
            controller);
    }

    [Fact]
    public void PasswordSetupPost_UsesIdentityTokenAndRateLimit()
    {
        var method =
            typeof(AccountController)
                .GetMethods()
                .Single(
                    x =>
                        x.Name == "SetPassword" &&
                        x.GetCustomAttributes<
                            HttpPostAttribute>()
                            .Any());

        Assert.True(
            method.GetCustomAttributes<
                    IgnoreAntiforgeryTokenAttribute>()
                .Any());

        Assert.True(
            method.GetCustomAttributes<
                    EnableRateLimitingAttribute>()
                .Any());

        var root = FindRepositoryRoot();

        var view =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/Views/"
                    + "Account/SetPassword.cshtml"));

        Assert.Contains(
            "asp-for=\"UserId\"",
            view);

        Assert.Contains(
            "asp-for=\"Token\"",
            view);

        Assert.DoesNotContain(
            "@Html.AntiForgeryToken()",
            view);
    }

    [Fact]
    public void Phase05Views_UseLocalizedResources()
    {
        var root = FindRepositoryRoot();

        var files = new[]
        {
            "src/Edulytics.Web/Views/SchoolUsers/Index.cshtml",
            "src/Edulytics.Web/Views/SchoolUsers/Create.cshtml",
            "src/Edulytics.Web/Views/SchoolUsers/Details.cshtml",
            "src/Edulytics.Web/Views/SchoolHome/Dashboard.cshtml",
            "src/Edulytics.Web/Views/Account/SetPassword.cshtml",
            "src/Edulytics.Web/Views/Account/PasswordSet.cshtml"
        };

        foreach (var relative in files)
        {
            var text =
                File.ReadAllText(
                    Path.Combine(root, relative));

            Assert.Contains(
                "PlatformResource",
                text);

            Assert.Contains(
                "T[\"",
                text);
        }
    }

    [Fact]
    public void UserDetails_DoesNotExposeRawPasswordSetupLink()
    {
        var root = FindRepositoryRoot();

        var view =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/Views/"
                    + "SchoolUsers/Details.cshtml"));

        Assert.DoesNotContain(
            "SchoolUserSetupLink",
            view);

        Assert.DoesNotContain(
            "CopyLinkInstruction",
            view);

        Assert.DoesNotContain(
            "PasswordSetupLink",
            view);

        Assert.Contains(
            "ResendInvitation",
            view);
    }

    [Fact]
    public void InvitationLanguageAndRateLimitContractsExist()
    {
        var root = FindRepositoryRoot();

        var controller =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/Controllers/"
                    + "SchoolUsersController.cs"));

        var program =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/Program.cs"));

        var resilienceRegistration =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/Extensions/"
                    + "BackendResilienceRegistrationExtensions.cs"));

        var rateLimitRegistration =
            program
            + Environment.NewLine
            + resilienceRegistration;

        Assert.Contains(
            "CurrentUICulture",
            controller);

        Assert.DoesNotContain(
            "result.SchoolCulture",
            controller);

        Assert.Contains(
            "SchoolUserCreate",
            rateLimitRegistration);

        Assert.Contains(
            "InvitationResend",
            rateLimitRegistration);

        Assert.Contains(
            "PasswordSetup",
            rateLimitRegistration);

        Assert.Contains(
            "UseRateLimiter",
            program);
    }

    [Fact]
    public void ResponsiveCssContractExists()
    {
        var root = FindRepositoryRoot();

        var css =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains(
            "@media (max-width: 767px)",
            css);

        Assert.Contains(
            "@media (max-width: 420px)",
            css);

        Assert.Contains(
            ".user-table",
            css);

        Assert.Contains(
            ".user-admin-grid",
            css);

        Assert.Contains(
            ".user-form-card",
            css);
    }

    private static string FindRepositoryRoot()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

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
