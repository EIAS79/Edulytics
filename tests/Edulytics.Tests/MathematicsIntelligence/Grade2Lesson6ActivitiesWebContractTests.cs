using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C only. Security tests written for later D execution.</summary>
public sealed class Grade2Lesson6ActivitiesWebContractTests
{
    [Fact]
    public void OriginalOptionalCenterNeedsStudentAccessAndCsrf()
    {
        var type=typeof(Grade2Lesson6ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l06",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L06",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.LessonCode);
    }
}
