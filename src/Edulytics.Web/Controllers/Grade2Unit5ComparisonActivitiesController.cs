using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u05/compare-l{lessonNumber:int}")]
public sealed class Grade2Unit5ComparisonActivitiesController(
 IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
 private static bool Allowed(int n)=>n>=7&&n<=14;
 private static string Code(int n)=>$"PED:US-CCSS-MATH:G2:U05:L{n:D2}";
 private static readonly IReadOnlyDictionary<int,string[]> Source=new Dictionary<int,string[]>{
 [7]=["Source100>99 (enter1 true/0 false)","Source100<99+1","Source98+3>100","Source50+50+50>100"],
 [8]=["Original source point3","Original source point30","Original source point300","Original source point700"],
 [9]=["Original first comparison larger numeral371","Original smaller numeral317","Original original point483","Original other point443"],
 [10]=["Original Number Talk36+40","Original46+30","Original59+40","Original69+30"],
 [11]=["Original86>80+4 (1/0)","Original400+40+6<846 (1/0)","Original330<300+3 (1/0)","Original500+50>505 (1/0)"],
 [12]=["Original80−50","Original87−50","Original76−40","Original66−30"],
 [13]=["Original picture6hundreds4ones","Original picture4hundreds6ones","Original picture6tens4ones","Original source largest picture value"],
 [14]=["Original demonstration cup Jada count","Original demonstration cup Noah count","Original demonstration cup Clare count","Original demonstration cup shared true total"]
 };
 private static readonly IReadOnlyDictionary<int,string[]> Independents=new Dictionary<int,string[]>{
 [7]=["FICTIONAL hundred count","FICTIONAL100+1","FICTIONAL100−1","FICTIONAL completed exact capture1 yes"],
 [8]=["FICTIONAL number-line mark234","FICTIONAL mark450","FICTIONAL mark678","FICTIONAL mark901"],
 [9]=["FICTIONAL first point483","FICTIONAL other point443","Numeric interval gap between the two","Enter1 if first point is farther right"],
 [10]=["FICTIONAL2hundreds? value142","FICTIONAL number274","FICTIONAL number330","FICTIONAL number300"],
 [11]=["FICTIONAL number413","FICTIONAL number415","FICTIONAL number732","FICTIONAL number721"],
 [12]=["FICTIONAL number638","FICTIONAL number681","FICTIONAL number672","FICTIONAL number644"],
 [13]=["FICTIONAL place-value701","FICTIONAL place-value502","FICTIONAL place-value641","FICTIONAL place-value400"],
 [14]=["FICTIONAL bean total320","FICTIONAL bean total452","FICTIONAL bean total671","FICTIONAL bean total805"]
 };
 private static Grade2Unit5ComparisonPage Page(int n,Guid a,Guid l,string title,bool posted,Grade2Unit5ComparisonAnswers answers,IReadOnlyList<Grade2Unit5ComparisonFeedback> f)=>
  new(a,l,title,n,posted,Source[n],Independents[n],UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[n],answers,f);
 [HttpGet("")]
 [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
 public async Task<IActionResult> Index(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
 {
  var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
  if(error is not null)return error;
  Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
  return View(Page(lessonNumber,curriculumAdoptionId,lessonId,title!,false,new(),[]));
 }
 [HttpPost("submit"),ValidateAntiForgeryToken]
 [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
 public async Task<IActionResult> Submit(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Unit5ComparisonAnswers a,CancellationToken ct)
 {
  var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
  if(error is not null)return error;
  if(a is null)return BadRequest("No answer body");
  if(typeof(Grade2Unit5ComparisonAnswers).GetProperties().Where(p=>p.PropertyType==typeof(string))
      .Select(p=>p.GetValue(a) as string).Any(v=>v?.Length>2000))return BadRequest("Answer too long");
  var source=UsCcssG2Unit5ComparisonOriginal.Original(lessonNumber,a.SourceA,a.SourceB,a.SourceC,a.SourceD);
  var own=UsCcssG2Unit5ComparisonOriginal.FirstParty(lessonNumber,a.IndependentA,a.IndependentB,a.IndependentC,a.IndependentD);
  var rows=new List<Grade2Unit5ComparisonFeedback>{
   new("Original source numerical checks",source.Correct,true,source.Explanation),
   new("Edulytics first-party numerical checks",own.Correct,false,own.Explanation),
   new("Original figures, actual plotted points, center games, beans or partner explanations",null,true,
       "Teacher must examine real original publisher visual evidence, physical objects and pupil reasoning. Independent calculation does not certify authentic work.")
  };
  Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
  return View("Index",Page(lessonNumber,curriculumAdoptionId,lessonId,title!,true,a,rows));
 }
 private async Task<(string? Title,IActionResult? Error)> Access(int n,Guid adoption,Guid lessonId,CancellationToken ct)
 {
  if(!Allowed(n)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return(null,Forbid());
  var w=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
  if(w.SelectedCurriculumAdoptionId!=adoption)return(null,NotFound());
  var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
  if(lesson is null||lesson.LessonCode!=Code(n))return(null,NotFound());
  var record=await lessonContent.GetPublishedForStudentAsync(actor,lessonId,
      System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
  if(record.Value is null)return(null,record.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
  if(record.Value.LessonCode!=Code(n))return(null,NotFound());
  return(record.Value.Title,null);
 }
}
