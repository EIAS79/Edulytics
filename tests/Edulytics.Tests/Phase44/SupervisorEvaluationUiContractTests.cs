using System.Reflection;
using Edulytics.Core.Constants;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.Phase44;

public sealed class SupervisorEvaluationUiContractTests
{
    [Fact]
    public void SubjectOverview_IsRestrictedToSupervisorOrSchoolAdmin()
    {
        var method = typeof(AnalyticsController)
            .GetMethod(nameof(AnalyticsController.SubjectOverview));

        Assert.NotNull(method);

        var authorize = method!
            .GetCustomAttributes<AuthorizeAttribute>()
            .Single();

        Assert.Contains(
            RoleNames.SubjectSupervisor,
            authorize.Roles ?? string.Empty);
        Assert.Contains(
            RoleNames.SchoolAdmin,
            authorize.Roles ?? string.Empty);

        Assert.Contains(
            method.GetCustomAttributes<HttpGetAttribute>(),
            x => x.Template == "subject-overview");
    }

    [Fact]
    public void SupervisorOverview_ShowsClassComparisonAndSharedSkillGaps()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/SubjectOverview.cshtml"));
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/AnalyticsService.cs"));

        Assert.Contains("supervisor-class-grid", view);
        Assert.Contains("PriorityGaps", view);
        Assert.Contains("AffectedStudentCount", view);
        Assert.Contains("AffectedClassCount", view);
        Assert.Contains("CurrentMasteryPercentage", view);

        Assert.Contains("GetSupervisorSubjectOverviewAsync", service);
        Assert.Contains("scope.SupervisedSubjectIds.Contains(subjectId)", service);
        Assert.Contains("EvaluationSkillStatus.Critical", service);
        Assert.Contains("EvaluationSkillStatus.NeedsFocus", service);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
