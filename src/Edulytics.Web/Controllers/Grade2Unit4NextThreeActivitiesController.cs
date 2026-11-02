using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>One access-controlled source-specific companion per actual Grade2 U04 lessons4,5,6.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04/l{lessonNumber:int}")]
public sealed class Grade2Unit4NextThreeActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private static bool Supported(int n)=>n is 4 or 5 or 6;
    private static string Code(int n)=>$"PED:US-CCSS-MATH:G2:U04:L{n:D2}";
    private static readonly IReadOnlyDictionary<int,string[]> SourceQuestions=new Dictionary<int,string[]>{
        [4]=["35 − 5","35 − 10","35 − 15","35 − 25"],
        [5]=["Original warm-up 0..40: a too-low estimate","A plausible about-right estimate","A too-high estimate","Revised estimate after teacher adds midpoint20"],
        [6]=["65 − 25","65 − 27","55 − 17","46 − 18"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> IndependentQuestions=new Dictionary<int,string[]>{
        [4]=["FICTIONAL three dice2+4+5 sum","FICTIONAL partner three dice1+6+3 sum","FICTIONAL tens digit5 and ones digit2","FICTIONAL two-digit values52 minus35"],
        [5]=["0..40 exactly halfway tick","Whole-number candidates STRICTLY between50 and55","Whole-number candidates STRICTLY between20 and30","FICTIONAL point32: units from benchmark30"],
        [6]=["FIRST-PARTY different arithmetic65−27","FICTIONAL 0..20 track: marker15 + spin5 exact end? 1 yes, 0 no","FICTIONAL game with3 cubes: how many more to win5?","FICTIONAL marker0 + spin10: location"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> SourceDescriptions=new Dictionary<int,string[]>{
        [4]=[
            "Activity1: actual partners roll THREE physical number cubes each, add actual roll, place 2 genuine counters on the SAME previously built0..20 number line and compare <,> or =, explaining length from zero.",
            "Activity2: on a real0..100 number line labeled in multiples of5, each partner rolls TWO number cubes to make a two-digit number; mark true points, compare near and far pairs using <,> or =.",
            "Teacher observes actual rolls, points, paper-unit spacing, partner MLR8 discourse. Fictional numerical checks below do NOT certify any original roll."],
        [5]=[
            "Warm-up original0..40 source dot: estimate too low/about right/too high and revise when midpoint20 appears; the original exact dot is intentionally NOT revealed and is NOT numerically graded.",
            "Activity1: examine FOUR original source plot images; image1 dot between50/55, image2 between20/30, images3 and4 rely on actual teacher image; Activity2: REAL group3 arranges TEN genuine publisher number cards on chart0..40 labelled0,10,20,30,40 and revises points.",
            "Teacher checks ORIGINAL image evidence and open-ended confidence, actual card values, physical chart, MLR7 reasoning. Edulytics numbers below are separate invented problems."],
        [6]=[
            "OPTIONAL Stage1 Number Line Scoot: REAL spinner, paperclip and original Twos/Fives/Tens gameboard; move physical cubes along line or decompose moves across several tracks.",
            "Capture ONLY when an actual move lands EXACTLY at the original printed end tick; replace captured cube at0, first partner with FIVE captured cubes wins. Choice centers: Capture Squares3/4, Scoot Stage1, Number Puzzles4.",
            "Teacher checks genuine ORIGINAL gameboard endpoints, valid spins and decompositions, real match and optional center choices. Digital model uses an INVENTED track0..20, never alleged original."]
    };
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
        [FromForm] Grade2Unit4NextThreeAnswers answers,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Unit4NextThreeAnswers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var source=UsCcssG2Unit4NextThreeActivities.Source(lessonNumber,
            answers.SourceA,answers.SourceB,answers.SourceC,answers.SourceD);
        var independent=UsCcssG2Unit4NextThreeActivities.Independent(lessonNumber,
            answers.IndependentA,answers.IndependentB,answers.IndependentC,answers.IndependentD);
        var rows=new List<Grade2Unit4NextThreeFeedback>{
            new("Original source warm-up / estimation",source.Correct,source.TeacherReviewNeeded,source.Feedback),
            new("Independent numerical companion",independent.Correct,independent.TeacherReviewNeeded,independent.Feedback),
            new("REAL publisher source activity and class participation",null,true,
                SourceDescriptions[lessonNumber][2])
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",Page(lessonNumber,curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private static Grade2Unit4NextThreePage Page(int n,Guid adoption,Guid lesson,string title,bool submitted,
        Grade2Unit4NextThreeAnswers answers,IReadOnlyList<Grade2Unit4NextThreeFeedback> result)=>
        new(adoption,lesson,title,n,submitted,SourceQuestions[n],IndependentQuestions[n],
            SourceDescriptions[n][0],SourceDescriptions[n][1],SourceDescriptions[n][2],answers,result);
    private async Task<(string? Title,IActionResult? Failure)> Access(int n,Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Supported(n)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return(null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code(n))return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(actor,lessonId,
            System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code(n))return(null,NotFound());
        return(content.Value.Title,null);
    }
}
