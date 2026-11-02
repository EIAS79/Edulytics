using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04/final-l{lessonNumber:int}")]
public sealed class Grade2Unit4FinalSixActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private static bool Allowed(int n)=>n>=10&&n<=15;
    private static string Code(int n)=>$"PED:US-CCSS-MATH:G2:U04:L{n:D2}";
    private static readonly IReadOnlyDictionary<int,string[]> Questions=new Dictionary<int,string[]>{
        [10]=["Original base-ten warm-up28+10+5","Clare's source blocks46−35","Original independent58−24","Diego source33+45"],
        [11]=["Original Number Talk32−6","Original Number Talk43−8","Original Number Talk51−5","Original Number Talk52−7"],
        [12]=["Original riddle15 RIGHT17 endpoint","Original ? LEFT20 ends33: start","Original42 to80: jump length","Original76 LEFT27 endpoint"],
        [13]=["Original diagram5+4 total","Original Clare24→42 missing addend","Original Andre37+39 total","How many forms STORY/EQUATION/LINE/TAPE are in each source card-sort group?"],
        [14]=["Original True/False28+2+20 sum","Original True/False16+3+30 sum","Original True/False36+4+20 sum","Number of ORIGINAL target landings needed to win"],
        [15]=["Original Kiran age7 sister+5","Original sister12 mom+27","Original mom39 brother−24","Original brother15 grandma+53"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> FirstParty=new Dictionary<int,string[]>{
        [10]=["FIRST-PARTY58−24","FIRST-PARTY23+24","FIRST-PARTY40−26","FIRST-PARTY50−32"],
        [11]=["FIRST-PARTY53−29","FIRST-PARTY82−35","FIRST-PARTY66−28","FICTIONAL100−47"],
        [12]=["FICTIONAL21+12","FICTIONAL?−14=52: start","FICTIONAL70−?=32: missing amount","FICTIONAL?+23=81: start"],
        [13]=["FICTIONAL cube train31 to52: cubes added","FICTIONAL34+28 cubes total","FICTIONAL tape whole70 one part46","FICTIONAL15+34 cubes total"],
        [14]=["FICTIONAL marker30 + move20","FICTIONAL marker45+5 target50 exact hit (1/0)","FICTIONAL marker48+5 target50 exact hit (1/0)","FICTIONAL one exact target landed: more needed to win two"],
        [15]=["FICTIONAL sibling age9+6","FICTIONAL sister15+21 older mom","FICTIONAL mom36 brother13 younger","FICTIONAL younger age23 grandma17 older"]
    };
    private static readonly IReadOnlyDictionary<int,string> Authentic=new Dictionary<int,string>{
        [10]="SOURCE actual warm-up arrows0→28→38→43, original Clare and Diego block drawings, tens/ones number-line methods58−24 and33+45, partner comparison; all true physical graphics teacher-only.",
        [11]="SOURCE original Diego/Tyler53−29 diagrams showing two strategies (tens-first/ones-first), Number Talk subtract to a ten; authentic student number-line drawings required.",
        [12]="SOURCE original five or more unknown-position arrow riddles, each written with ? and represented on a true0..100 number line, real partner observations.",
        [13]="SOURCE original publisher A–P story/equation/tape/number-line card sort and genuine train examples; inaccessible card artwork never invented.",
        [14]="OPTIONAL SOURCE actual Jump the Line stage1 gameboard: real partner selects3 targets, physically spins3 spinners, chooses one move, first exact landings on TWO targets wins; actual printed spinner results unknown to software.",
        [15]="OPTIONAL SOURCE Kiran's chained family age riddles plus FOUR actual user-chosen people with age clues; publisher candle photos are picture-only."
    };
    private static Grade2Unit4FinalSixPage Make(int n,Guid adoption,Guid lesson,string title,bool done,
        Grade2Unit4FinalSixAnswers answers,IReadOnlyList<Grade2Unit4FinalSixFeedback> f)=>
        new(adoption,lesson,title,n,done,Questions[n],FirstParty[n],Authentic[n],
            "Original source artwork, partner notes, actual physical drawings and optional personal facts require TEACHER review.",answers,f);
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(Make(lessonNumber,curriculumAdoptionId,lessonId,title!,false,new(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Unit4FinalSixAnswers answers,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Unit4FinalSixAnswers).GetProperties().Where(x=>x.PropertyType==typeof(string))
           .Select(x=>x.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var s=UsCcssG2Unit4FinalSixSourcePractice.Source(lessonNumber,
            answers.SourceA,answers.SourceB,answers.SourceC,answers.SourceD);
        var own=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(lessonNumber,
            answers.IndependentA,answers.IndependentB,answers.IndependentC,answers.IndependentD);
        var rows=new List<Grade2Unit4FinalSixFeedback>{
            new("Source-aligned numeric questions",s.Correct,true,s.Feedback),
            new("Separately authored numerical practice",own.Correct,false,own.Feedback),
            new("Genuine original source work",null,true,
                "Physical original imagery, diagrams, teacher-directed centers and student's authentic discussion MUST be observed by a teacher; numeric checks do not demonstrate academic completion.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",Make(lessonNumber,curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Error)> Access(int n,Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Allowed(n)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return(null,NotFound());
        var match=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(match is null||match.LessonCode!=Code(n))return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(actor,lessonId,
            System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code(n))return(null,NotFound());
        return(content.Value.Title,null);
    }
}
