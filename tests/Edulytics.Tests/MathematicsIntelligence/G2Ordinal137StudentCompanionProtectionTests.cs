using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal137StudentCompanionProtectionTests {
 [Fact] public void PupilSourcePageIsPrivateAndSubmissionCsrfProtected(){
  var controller=typeof(Grade2Unit9ActivitiesController);
  Assert.Equal("StudentPortal",controller.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
  Assert.Equal("student/practice/grade2-u09/l{lessonNumber:int}",controller.GetCustomAttribute<RouteAttribute>()!.Template);
  Assert.NotNull(controller.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
  Assert.NotNull(controller.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
  Assert.NotNull(controller.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
 }
}