using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-specific Grade2 Lesson9 HTTP contract, written for Stage D only.
/// </summary>
public sealed class Grade2Lesson9ActivitiesWebContractTests
{
    [Fact]
    public void NumberedBarGraphLessonUsesStudentPolicyAndCsrf()
    {
        var type=typeof(Grade2Lesson9ActivitiesController);
        Assert.Equal("StudentPortal",
            type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l09",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L09",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1NinthLessonEdulyticsCoolDown.LessonCode);
    }
}
