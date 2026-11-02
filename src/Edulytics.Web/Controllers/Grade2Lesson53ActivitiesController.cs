using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04-l01")]
public sealed class Grade2Lesson53ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit4FirstLessonNumberLine.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson53Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson53Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson53Answers answers,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson53Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var feedback=new List<Grade2Lesson53Feedback>();
        void Add(string section,UsCcssG2Unit4FirstLessonNumberLine.Review review)=>
            feedback.Add(new(section,review.Correct,review.TeacherReviewNeeded,review.Feedback));
        Add("Original first 0..10 line missing whole-number labels",
            UsCcssG2Unit4FirstLessonNumberLine.FirstSourceLine(
                answers.MissingThree,answers.MissingFour,answers.MissingSix,answers.MissingSeven));
        Add("Original source point choices greater than6 and less than9",
            UsCcssG2Unit4FirstLessonNumberLine.SourcePoints(
                answers.FirstPointSix,answers.GreaterThanSix,answers.SecondPointNine,answers.LessThanNine));
        Add("Independent digital 0..20 numeric practice",
            UsCcssG2Unit4FirstLessonNumberLine.IndependentlyAuthored(
                answers.NewGapThreeThirteen,answers.NewFromZeroThirteen,answers.NewTotalTickMarks));
        feedback.Add(new("Real 0..20 paper line, accurate equal intervals and real pair discussion",
            null,true,"Teacher must inspect student's own zero-anchored REAL line0..20, equally spaced marks, physical dot placement at3 and13, chosen unit and partner comparison. Digital calculation alone cannot establish physical precision."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson53Page(curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return(null,NotFound());
        var selected=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(selected is null||selected.LessonCode!=Code)return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return(null,NotFound());
        return(content.Value.Title,null);
    }
}
