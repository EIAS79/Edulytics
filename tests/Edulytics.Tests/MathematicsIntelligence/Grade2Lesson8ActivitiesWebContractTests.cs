using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific Stage C checks, written only for later Stage D.</summary>
public sealed class Grade2Lesson8ActivitiesWebContractTests
{
    [Fact]
    public void OriginalPictureGraphCompanionIsLearnerAuthorizedAndCsrfProtected()
    {
        var type=typeof(Grade2Lesson8ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l08",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void MissingMotivationsAreNotInferredFromPictureGraphCounts()
    {
        var review=Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckWhichQuestionsGraphCanAnswer("1","1","0","1","1");
        Assert.True(review.Correct);
        Assert.False(review.TeacherReviewNeeded);
    }
}
