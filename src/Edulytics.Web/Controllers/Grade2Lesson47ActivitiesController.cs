using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l13")]
public sealed class Grade2Lesson47ActivitiesController(IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
 private const string Code=UsCcssG2Unit3ThirteenthLessonCenters.LessonCode;
 [HttpGet("")][ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
 public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct){
  var (title,err)=await Access(curriculumAdoptionId,lessonId,ct);if(err is not null)return err;
  Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
  return View(new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson47Answers(),[]));
 }
 [HttpPost("submit"),ValidateAntiForgeryToken][ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
 public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson47Answers answers,CancellationToken ct){
  var (title,err)=await Access(curriculumAdoptionId,lessonId,ct);if(err is not null)return err;
  if(answers is null)return BadRequest("Missing answers");
  if(typeof(Grade2Lesson47Answers).GetProperties().Where(p=>p.PropertyType==typeof(string)).Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))return BadRequest("Input too long");
  var rows=new List<Grade2Lesson47Feedback>();void Add(string name,UsCcssG2Unit3ThirteenthLessonCenters.Review r)=>rows.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
  Add("Source four Number Talk sums",UsCcssG2Unit3ThirteenthLessonCenters.Talk(answers.Talk1,answers.Talk2,answers.Talk3,answers.Talk4));
  Add("First-party numerical deviation; SAME unit",UsCcssG2Unit3ThirteenthLessonCenters.Measure(answers.Target,answers.Actual,answers.Unit,answers.Score));
  Add("First-party Stage4 compose/decompose equations",UsCcssG2Unit3ThirteenthLessonCenters.Puzzles(answers.PuzzleAdd,answers.PuzzleSub));
  Add("Independent Edulytics exit",UsCcssG2Unit3ThirteenthLessonCenters.Exit(answers.ExitMeasure,answers.ExitAdd));
  rows.Add(new("Actual Target Measurements eight rounds and Number Puzzles cards",null,true,"Teacher checks genuine drawn lines, rulers at zero, units, score assignment to STOP partner, eight-round winner and original digit cards. Digital values cannot attest these."));
  Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
  return View("Index",new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
 }
 private async Task<(string? Title,IActionResult? Error)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct){
  if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var user))return(null,Forbid());
  var w=await privatePractice.GetWorkspaceAsync(user,adoptionId,ct);
  if(w.SelectedCurriculumAdoptionId!=adoptionId)return(null,NotFound());
  var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
  if(lesson is null||lesson.LessonCode!=Code)return(null,NotFound());
  var content=await lessonContent.GetPublishedForStudentAsync(user,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
  if(content.Value is null)return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
  if(content.Value.LessonCode!=Code)return(null,NotFound());
  return(content.Value.Title,null);
 }
}
