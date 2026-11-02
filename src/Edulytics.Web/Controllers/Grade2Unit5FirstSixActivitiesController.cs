using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u05/l{lessonNumber:int}")]
public sealed class Grade2Unit5FirstSixActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private static string Code(int number)=>$"PED:US-CCSS-MATH:G2:U05:L{number:D2}";
    private static bool Allowed(int n)=>n>=1&&n<=6;
    private static readonly IReadOnlyDictionary<int,string[]> PublishedQuestions=new Dictionary<int,string[]>{
        [1]=["Andre original8 tens16 ones: initial total","Andre later8 tens20 ones: new total","Ten tens: total value in ones","One hundred plus eleven ones"],
        [2]=["Original number90: how many ten rods?","Original number110: how many ten rods?","Original number150: how many ten rods?","Original number300: how many ten rods?"],
        [3]=["Original42+42","Original2hundreds28tens15ones total","Original Mai1hundred7tens18ones total","Original Diego source final total"],
        [4]=["Original riddle6hundreds7tens2ones","Original2hundreds5tens3ones","Original5hundreds0tens9ones","Original1hundred8tens0ones"],
        [5]=["Original3hundreds5tens7ones numeral","Original1hundred9tens2ones","Original2hundreds3tens8ones","Original3hundreds1ten6ones"],
        [6]=["Original words 'two hundred forty-eight' → numeral","Original 'five hundred sixty-two' → numeral","Original source base-ten4hundreds3tens3ones → numeral","Original expanded form900+50+1 → numeral"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> IndependentQuestions=new Dictionary<int,string[]>{
        [1]=["First-party10tens: total number of tens","First-party one hundred in unit ones: total","First-party8tens16ones","First-party100+11"],
        [2]=["First-party3 hundreds: how many ten-rods?","First-party70 tens: hundreds","First-party3 hundreds: value in ones","First-party30ten rods minus1 rod: rods remaining"],
        [3]=["First-party58−24","First-party23+24","First-party40−26","First-party50−32"],
        [4]=["First-party5hundreds0tens7ones: number","First-party6hundreds0tens9ones","First-party4hundreds8tens2ones","First-party 3 tens component in 3hundreds3tens"],
        [5]=["First-party4hundreds6tens5ones","First-party5hundreds: place component in ones","First-party digits6,5,4: greatest number","First-party digits6,5,4: smallest number"],
        [6]=["Independent two hundred forty-eight → numeral","Independent five hundred sixty-two → numeral","Independent four hundred thirty-three → numeral","Independent nine hundred fifty-one → numeral"]
    };
    private static Grade2Unit5FirstSixPage Page(int n,Guid adoption,Guid lesson,string title,bool done,
       Grade2Unit5FirstSixAnswers a,IReadOnlyList<Grade2Unit5Feedback> feedback)=>
       new(adoption,lesson,title,n,done,PublishedQuestions[n],IndependentQuestions[n],
          UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[n],a,feedback);
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
    public async Task<IActionResult> Submit(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Unit5FirstSixAnswers a,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(a is null)return BadRequest("Missing answers");
        if(typeof(Grade2Unit5FirstSixAnswers).GetProperties().Where(x=>x.PropertyType==typeof(string))
            .Select(x=>x.GetValue(a) as string).Any(x=>x?.Length>2000))
            return BadRequest("Invalid answer length");
        var src=UsCcssG2Unit5FirstSixOriginal.Source(lessonNumber,a.SourceA,a.SourceB,a.SourceC,a.SourceD);
        var first=UsCcssG2Unit5FirstSixOriginal.FirstParty(lessonNumber,a.IndependentA,a.IndependentB,a.IndependentC,a.IndependentD);
        var grades=new List<Grade2Unit5Feedback>{
            new("Publisher source numeric quantities",src.Correct,src.TeacherReviewNeeded,src.Feedback),
            new("Independent Edulytics numerical checks",first.Correct,first.TeacherReviewNeeded,first.Feedback),
            new("Real source block construction, word composition, images and student discussion",null,true,
                "Authentic diagrams/blocks, linguistic expressions, open-ended reasons and original image-only questions require educator inspection; correct numerical answers cannot certify physical work or words.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",Page(lessonNumber,curriculumAdoptionId,lessonId,title!,true,a,grades));
    }
    private async Task<(string? Title,IActionResult? Error)> Access(int n,Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Allowed(n)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return(null,Forbid());
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
