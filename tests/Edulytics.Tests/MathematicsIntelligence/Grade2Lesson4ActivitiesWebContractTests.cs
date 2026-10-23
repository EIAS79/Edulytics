using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific Stage C HTTP contract tests, not run before D.</summary>
public sealed class Grade2Lesson4ActivitiesWebContractTests
{
    [Fact]
    public void HowCloseCompanionIsProtectedAndExactlyLessonScoped()
    {
        var type=typeof(Grade2Lesson4ActivitiesController);
        Assert.Equal("StudentPortal",
            type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l04",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L04",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1FourthLessonEdulyticsCoolDown.LessonCode);
    }
}
