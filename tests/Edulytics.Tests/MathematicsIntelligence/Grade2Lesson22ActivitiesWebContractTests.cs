using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Lesson22ActivitiesWebContractTests
{
    [Fact]
    public void CaptureAndFiveCenterCompanionRequiresStudentAndAntiforgery()
    {
        var type=typeof(Grade2Lesson22ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u02-l04",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
