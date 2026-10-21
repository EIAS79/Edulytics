using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored HTTP safety assertions, not executed.</summary>
public sealed class Grade2Lesson5ActivitiesWebContractTests
{
    [Fact]
    public void SevenCardStudentPracticeIsAuthorizedAndAntiforgeryProtected()
    {
        var type=typeof(Grade2Lesson5ActivitiesController);
        Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
        Assert.Equal("student/practice/grade2-u01-l05",
            type.GetCustomAttribute<RouteAttribute>()!.Template);
        Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(type.GetMethod("Submit")!
            .GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L05",
            Edulytics.Core.Mathematics.Practice
                .UsCcssG2Unit1FifthLessonEdulyticsCoolDown.LessonCode);
    }
}
