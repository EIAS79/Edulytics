using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Lesson49ActivitiesWebContractTests
{
    [Fact]
    public void RealPencilMeasuringHasProtectedStudentPageAndCsrf()
    {
        var t=typeof(Grade2Lesson49ActivitiesController);
        Assert.Equal("StudentPortal",t.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u03-l15",t.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(t.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
