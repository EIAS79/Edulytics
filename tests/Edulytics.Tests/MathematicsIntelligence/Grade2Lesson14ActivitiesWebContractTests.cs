using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C auth and CSRF contract, execution deferred to Stage D.</summary>
public sealed class Grade2Lesson14ActivitiesWebContractTests
{
    [Fact]
    public void SourceTapeDiagramCompanionRequiresEnrolledStudentAndAntiforgery()
    {
        var type=typeof(Grade2Lesson14ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l14",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
