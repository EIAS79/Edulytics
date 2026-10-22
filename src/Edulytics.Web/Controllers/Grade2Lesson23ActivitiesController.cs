using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Numeric companion to IM2021 G2 U02 L05; no official marks and no
/// automatic attestation of real cubes, student drawings or pair discussion.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l05")]
public sealed class Grade2Lesson23ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2FifthLessonDecomposeTen.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson23Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson23Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson23Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson23Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var results=new List<Grade2Lesson23Feedback>();
        void Add(string name,UsCcssG2Unit2FifthLessonDecomposeTen.Review r)=>
            results.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original four mental subtractions",UsCcssG2Unit2FifthLessonDecomposeTen.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("82−9, trade a ten",UsCcssG2Unit2FifthLessonDecomposeTen.Activity1(
            answers.EightyTwoAnswer,answers.EightyTwoTens,answers.EightyTwoOnes));
        Add("Diego's 55−20−8 decomposition",UsCcssG2Unit2FifthLessonDecomposeTen.Activity2(
            answers.DiegoStart,answers.DiegoAfterTens,answers.DiegoTradeTens,
            answers.DiegoTradeOnes,answers.DiegoEnd));
        Add("Independent Edulytics Cool-down",UsCcssG2Unit2FifthLessonDecomposeTen.CoolDown(
            answers.CoolA,answers.CoolB,answers.CoolC));
        results.Add(new("Original physical models, drawings and MLR8 discussion",null,true,
            "Teacher must examine real manipulatives, original diagrams and mathematical explanation; digital inputs prove none of these."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson23Page(curriculumAdoptionId,lessonId,title!,true,answers,results));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
