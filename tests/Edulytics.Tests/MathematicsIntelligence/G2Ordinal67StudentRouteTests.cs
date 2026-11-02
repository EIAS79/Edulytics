using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal67StudentRouteTests
{
 [Fact] public void SelectedLessonStudentOnlyAndCsrfPost()
 {
  var c=typeof(Grade2Unit4FinalSixActivitiesController);
  Assert.Equal("StudentPortal",c.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
  Assert.Equal("student/practice/grade2-u04/final-l{lessonNumber:int}",c.GetCustomAttribute<RouteAttribute>()!.Template);
  Assert.NotNull(c.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
  Assert.NotNull(c.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
  Assert.NotNull(c.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
 }
}
