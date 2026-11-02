using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal119StudentCompanionProtectionTests
{
 [Fact] public void OriginalStudentCompanionUsesAuthAndAntiforgery()
 {
  var t=typeof(Grade2Unit7ActivitiesController);
  Assert.Equal("StudentPortal",t.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
  Assert.Equal("student/practice/grade2-u07/l{lessonNumber:int}",t.GetCustomAttribute<RouteAttribute>()!.Template);
  Assert.NotNull(t.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
  Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
  Assert.NotNull(t.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
 }
}