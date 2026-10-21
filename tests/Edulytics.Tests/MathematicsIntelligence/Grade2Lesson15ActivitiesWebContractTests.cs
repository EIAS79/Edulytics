using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Web/CSRF contract written for Stage D, no tests executed.</summary>
public sealed class Grade2Lesson15ActivitiesWebContractTests
{
    [Fact]
    public void LearnerTapeCardSortMustBeProtectedByCurrentLessonAuthorizationAndCsrf()
    {
        var type=typeof(Grade2Lesson15ActivitiesController);
        Assert.Equal("StudentPortal",
            type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l15",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
