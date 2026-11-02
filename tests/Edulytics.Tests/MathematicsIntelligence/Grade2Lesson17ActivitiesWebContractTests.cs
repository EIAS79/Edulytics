using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Lesson17ActivitiesWebContractTests
{
    [Fact]
    public void CountTo120CentersRequireStudentPolicyAndAntiForgery()
    {
        var t=typeof(Grade2Lesson17ActivitiesController);
        Assert.Equal("StudentPortal",t.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l17",
            t.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(t.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
