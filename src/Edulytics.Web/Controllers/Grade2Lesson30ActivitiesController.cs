using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l12")]
public sealed class Grade2Lesson30ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2TwelfthLessonMancalaDiagrams.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson30Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson30Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson30Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson30Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson30Feedback>();
        void Add(string label,UsCcssG2Unit2TwelfthLessonMancalaDiagrams.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Optional source-story numerical interpretation; original Activity1 only matches diagrams",
            UsCcssG2Unit2TwelfthLessonMancalaDiagrams.SupplementalClareHan(
                answers.OptionalSourceSmaller,answers.OptionalSourceTotal,
                answers.OptionalSourceBoard,answers.OptionalSourceSide));
        Add("Original first-party Edulytics story arithmetic",
            UsCcssG2Unit2TwelfthLessonMancalaDiagrams.FirstParty(
                answers.FirstPartySmaller,answers.FirstPartyTotal,
                answers.FirstPartyBoard,answers.FirstPartySide));
        rows.Add(new("Authentic Mancala teacher image, MLR6, source paper card sort and real diagrams",
            null,true,"Teacher checks original tape-diagram letter matches, Three Reads, actual A–D card choices and authentic Cards L and N. Numeric checks do not imply source Activity1 numerical solving was assigned."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson30Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
