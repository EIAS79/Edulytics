using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C source-aligned real classroom response UI tests, authored
/// without test execution before the separate Stage D gate.
/// </summary>
public sealed class Grade2Lesson7ActivitiesWebContractTests
{
    [Fact]
    public void AuthenticSurveyRequiresStudentPolicyAndCsrf()
    {
        var type=typeof(Grade2Lesson7ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l07",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
    }

    [Fact]
    public void OriginalDataLessonNeverNeedsFabricatedClassResponses()
    {
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L07",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1SeventhLessonClassData.LessonCode);
        Assert.False(Edulytics.Core.Mathematics.Practice
            .UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses(null).Accepted);
    }
}
