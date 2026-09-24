using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.Phase44;

public sealed class StudentSelfEvaluationPrivacyTests
{
    [Fact]
    public void ProgressRoute_DoesNotAcceptAnotherStudentIdentifier()
    {
        var method = typeof(StudentPortalController)
            .GetMethod(nameof(StudentPortalController.Progress));

        Assert.NotNull(method);

        var parameterNames = method!
            .GetParameters()
            .Select(x => x.Name)
            .ToArray();

        Assert.DoesNotContain(
            "studentProfileId",
            parameterNames,
            StringComparer.OrdinalIgnoreCase);

        Assert.Contains(
            method.GetCustomAttributes(
                    typeof(HttpGetAttribute),
                    inherit: false)
                .Cast<HttpGetAttribute>(),
            x => x.Template == "progress");
    }

    [Fact]
    public void SelfEvaluation_LoadsPrivatePracticeOnlyByAuthenticatedActor()
    {
        var root = FindRoot();

        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/StudentSelfEvaluationService.cs"));

        Assert.Contains(
            "FindStudentByUserIdAsync(",
            service);
        Assert.Contains(
            "actorUserId",
            service);
        Assert.Contains(
            "ListPrivateEvidenceAsync(",
            service);
        Assert.DoesNotContain(
            "Guid studentProfileId",
            service);
    }

    [Fact]
    public void StaffProjection_StillExcludesPrivatePractice()
    {
        var root = FindRoot();

        var repository = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Data/Repositories/AnalyticsRepository.cs"));

        Assert.Contains(
            "&& !x.IsPrivate",
            repository);
    }

    [Fact]
    public void StudentProgressView_ShowsSeparatedOfficialAndPrivatePractice()
    {
        var root = FindRoot();

        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/Progress.cshtml"));
        var dashboard = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/Dashboard.cshtml"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_StudentLayout.cshtml"));

        Assert.Contains(
            "OfficialEvaluation",
            view);
        Assert.Contains(
            "PrivatePractice",
            view);
        Assert.Contains(
            "PracticeToAssessmentGapPercentagePoints",
            view);
        Assert.Contains(
            "ComparableSkillGrowthPercentagePoints",
            view);
        Assert.Contains(
            "Model.Evaluation",
            dashboard);
        Assert.Contains(
            "asp-action="Progress"",
            layout);
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
