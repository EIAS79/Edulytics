using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Lesson54ActivitiesWebContractTests
{
    [Fact]
    public void SourceImageTeacherBoundaryAndYarnStudentRouteAreProtected()
    {
        var controller=typeof(Grade2Lesson54ActivitiesController);
        Assert.Equal("StudentPortal",controller.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u04-l02",
            controller.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(controller.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
