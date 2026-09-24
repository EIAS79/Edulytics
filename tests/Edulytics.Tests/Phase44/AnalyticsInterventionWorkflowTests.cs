using System.Reflection;
using Edulytics.Core.Constants;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.Phase44;

public sealed class AnalyticsInterventionWorkflowTests
{
    [Fact]
    public void InterventionAction_IsTeacherOnlyAndAntiForgeryProtected()
    {
        var method = typeof(AnalyticsController)
            .GetMethod(nameof(AnalyticsController.CreateInterventionCheck));

        Assert.NotNull(method);

        var authorize = method!
            .GetCustomAttributes<AuthorizeAttribute>()
            .Single();

        Assert.Equal(RoleNames.Teacher, authorize.Roles);

        Assert.Contains(
            method.GetCustomAttributes<HttpPostAttribute>(),
            x =>
                x.Template ==
                "student/{studentProfileId:guid}/intervention-check");

        Assert.Single(
            method.GetCustomAttributes<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void InterventionService_CreatesTargetedOnlineDraftAndRequiresReview()
    {
        var root = FindRoot();
        var source = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/AnalyticsInterventionService.cs"));

        Assert.Contains(
            "GetStudentEvaluationAsync(",
            source);
        Assert.Contains(
            "AssessmentTargetType.Student",
            source);
        Assert.Contains(
            "AssessmentDeliveryMode.Online",
            source);
        Assert.Contains(
            "GenerateQuestionsAsync(",
            source);
        Assert.Contains(
            "request.LearningOutcomeId",
            source);
        Assert.Contains(
            "/school/assessments/{assessmentId:D}/builder",
            source);

        Assert.DoesNotContain(
            ".PublishAsync(",
            source);
        Assert.Contains(
            "TryDeleteDraftAsync(",
            source);
    }

    [Fact]
    public void Student360_OffersInterventionOnlyToTeachers()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Student.cshtml"));

        Assert.Contains(
            "User.IsInRole(RoleNames.Teacher)",
            view);
        Assert.Contains(
            "asp-action=\"CreateInterventionCheck\"",
            view);
        Assert.Contains(
            "learningOutcomeId",
            view);
        Assert.Contains(
            "skillKey",
            view);
        Assert.Contains(
            "questionCount",
            view);
    }

    [Fact]
    public void PersonalizedCheck_IsVisibleInStaffAndStudentHistory()
    {
        var root = FindRoot();

        var staff = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Student.cshtml"));
        var student = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/Progress.cshtml"));

        Assert.Contains("IsPersonalizedCheck", staff);
        Assert.Contains("IsPersonalizedCheck", student);
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
