using System.Reflection;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal121StudentCompanionProtectionTests{
 [Fact] public void OriginalHumanSourceCannotBeAnonymousAndUsesCsrf(){
  var type=typeof(Grade2Unit8ActivitiesController);
  Assert.Equal("StudentPortal",type.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
  Assert.Equal("student/practice/grade2-u08/l{lessonNumber:int}",type.GetCustomAttribute<RouteAttribute>()!.Template);
  Assert.NotNull(type.GetMethod("Index")!.GetCustomAttribute<HttpGetAttribute>());
  Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<HttpPostAttribute>());
  Assert.NotNull(type.GetMethod("Submit")!.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>());
 }
}