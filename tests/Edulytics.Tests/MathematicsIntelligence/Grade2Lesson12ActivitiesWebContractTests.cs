using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Optional Grade2 source lesson HTTP security contract, Stage C only.</summary>
public sealed class Grade2Lesson12ActivitiesWebContractTests
{
    [Fact]
    public void PhysicalSortCompanionRequiresStudentLessonAndCsrf()
    {
        var type=typeof(Grade2Lesson12ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l12",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
    [Fact]
    public void NumericallyValidCollectionStillNeedsTeacherObservation()
    {
        var review=Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRealCollection("A","B","C","8","12","5");
        Assert.True(review.Review.Correct);
        Assert.True(review.Review.TeacherReviewNeeded);
    }
}
