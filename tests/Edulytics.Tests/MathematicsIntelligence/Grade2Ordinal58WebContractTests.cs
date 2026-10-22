using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal58WebContractTests
{
    [Fact]
    public void SourceActivitiesUsePrivateStudentAdoptionAndCsrfProtectedPost()
    {
        var type=typeof(Grade2Unit4NextThreeActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u04/l{lessonNumber:int}",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }
}
