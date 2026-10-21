using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authoring assertions only; do not run until Stage D.</summary>
public sealed class Grade2Lesson2ActivitiesWebContractTests
{
    [Fact]
    public void SecondLessonCompanionIsLearnerAuthorizedAndCsrfProtected()
    {
        var type=typeof(Grade2Lesson2ActivitiesController);
        var auth=type.GetCustomAttribute<AuthorizeAttribute>();
        Assert.Equal("StudentPortal",auth!.Policy);
        Assert.Equal("student/practice/grade2-u01-l02",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void GenuineTeacherAssessmentIsNotReplacedByNumericOnlyScore()
    {
        Assert.Equal(4,Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1SecondLessonEdulyticsCoolDown.TeacherRubric.Count);
        var review=Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1SecondLessonEdulyticsCoolDown
            .VerifyRelatedEquations("4 + 6","10 − 4");
        Assert.True(review.Correct);
        Assert.True(review.TeacherReviewNeeded);
    }
}
