using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Lesson52ActivitiesWebContractTests
{
    [Fact]
    public void RealWorldYardstickCompanionIsStudentOnlyAndCsrfProtected()
    {
        var controller = typeof(Grade2Lesson52ActivitiesController);
        Assert.Equal("StudentPortal",
            controller.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u03-l18",
            controller.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(controller.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(controller.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
