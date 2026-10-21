using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G2 U01 L01 Stage C HTTP contract checks, authored without test execution.
/// An API/HTTP change is not academic or Stage D verification on its own.
/// </summary>
public sealed class Grade2Lesson1ActivitiesWebContractTests
{
    [Fact]
    public void OriginalCoolDownIsStudentAuthorizedAndRestrictedToExactLesson()
    {
        var controller = typeof(Grade2Lesson1ActivitiesController);
        var auth = controller.GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(auth);
        Assert.Equal("StudentPortal", auth!.Policy);

        var prefix = controller.GetCustomAttribute<RouteAttribute>();
        Assert.Equal("student/practice/grade2-u01-l01", prefix!.Template);
        var source = controller.GetMethod("Index");
        Assert.NotNull(source);
        Assert.NotNull(source!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!
            .GetCustomAttribute<HttpPostAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L01",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1FirstLessonEdulyticsCoolDown.LessonCode);
    }

    [Fact]
    public void ClosedFormRubricCannotClaimAutomaticTeacherApproval()
    {
        var content = Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1FirstLessonEdulyticsCoolDown;
        var rubric = content.ExplanationRubric;
        Assert.Equal(4, rubric.Count);
        Assert.Contains(rubric, x => x.Contains("representation"));
        var open = content.AssessInteger("CD-4", "10");
        Assert.False(open.ScorableByMachine);
        Assert.True(open.TeacherReviewRequired);
    }
}
