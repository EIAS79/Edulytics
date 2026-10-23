using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Student scope and CSRF checks written for Stage D, not run now.</summary>
public sealed class Grade2Lesson10ActivitiesWebContractTests
{
    [Fact]
    public void StudentDrawnGraphsAreAccessibleOnlyThroughAuthorizedLessonAndCsrf()
    {
        var type=typeof(Grade2Lesson10ActivitiesController);
        Assert.Equal("StudentPortal",
            type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l10",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!
            .GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void SourceOriginalGraphConstructionRequiresTeacherReview()
    {
        var review=Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
                3,"Our lunch choices","★","1",["4","6","3","2"]);
        Assert.True(review.Correct);
        Assert.True(review.TeacherReviewNeeded);
    }
}
