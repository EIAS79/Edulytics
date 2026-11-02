using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored HTTP safety checks; do not execute until D.</summary>
public sealed class Grade2Lesson3ActivitiesWebContractTests
{
    [Fact]
    public void LessonThreeTwentyCubesRequireAuthorizedStudentAndCsrf()
    {
        var type=typeof(Grade2Lesson3ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l03",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L03",
            Edulytics.Core.Mathematics.Practice.UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.LessonCode);
    }
}
