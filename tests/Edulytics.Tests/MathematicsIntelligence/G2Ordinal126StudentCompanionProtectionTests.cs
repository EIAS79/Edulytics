using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal126StudentCompanionProtectionTests {
 [Fact] public void OriginalActivityIsPrivateAndPostHasAntiForgery(){
  var t=typeof(Grade2Unit8ActivitiesController);
  Assert.Equal("StudentPortal",t.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
  Assert.Equal("student/practice/grade2-u08/l{lessonNumber:int}",t.GetCustomAttribute<RouteAttribute>()!.Template);
  Assert.NotNull(t.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
  Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
  Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
 }
}