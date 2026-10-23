using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Numerical market companion; actual pupils' roles, cube rolls, sales and report need teacher validation.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l16")]
public sealed class Grade2Lesson34ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2SixteenthLessonMarketInventory.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson34Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson34Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson34Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson34Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var results=new List<Grade2Lesson34Feedback>();
        void Add(string label,UsCcssG2Unit2SixteenthLessonMarketInventory.Review r)=>
            results.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Three-product original inventory constraints",UsCcssG2Unit2SixteenthLessonMarketInventory.CheckOriginalInventory(
            answers.StockA,answers.StockB,answers.StockC));
        Add("Independent first-party inventory sales, leftovers and restock",UsCcssG2Unit2SixteenthLessonMarketInventory.FirstPartySales(
            answers.ExampleStockA,answers.ExampleStockB,answers.ExampleStockC,
            answers.ExampleSaleA,answers.ExampleSaleB,answers.ExampleSaleC,
            answers.ExampleLeftA,answers.ExampleLeftB,answers.ExampleLeftC,
            answers.ExampleTotalSold,answers.ExampleTotalLeft,answers.ExampleRestock));
        Add("Independent Edulytics market exit",UsCcssG2Unit2SixteenthLessonMarketInventory.CheckIndependentExit(
            answers.ExitLeft,answers.ExitRestock));
        results.Add(new("Real buyer/seller transactions and classroom report",null,true,
            "Teacher checks physical number-cube rolls, realistic purchase quantities, actual remaining inventory, source image, MLR8 partner presentation and actual end-of-day report. Digit entry cannot certify play."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson34Page(curriculumAdoptionId,lessonId,title!,true,answers,results));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
