using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04/arrow-l{lessonNumber:int}")]
public sealed class Grade2Unit4ArrowActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private static bool Allowed(int n)=>n is 7 or 8 or 9;
    private static string Code(int n)=>$"PED:US-CCSS-MATH:G2:U04:L{n:D2}";
    private static readonly IReadOnlyDictionary<int,string[]> Sources=new Dictionary<int,string[]>{
        [7]=["Source arrow10→15: number of spaces RIGHT","Source arrow13→5: number of spaces LEFT","Source arrow12→2: number of spaces LEFT","Source arrow4→14: number of spaces RIGHT"],
        [8]=["Source equation15+7: end number","Source equation18−6: end number","Source equation46+7: end number","Source equation33−9: end number"],
        [9]=["Original warm-up20−2","Original warm-up20−17","Original warm-up49−3","Original warm-up67−64"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> Independent=new Dictionary<int,string[]>{
        [7]=["Invented arrow18→9: number of spaces LEFT","Invented arrow7→16: number of spaces RIGHT","Invented arrow32 left5: endpoint","Invented arrow8 right4: endpoint"],
        [8]=["Invented19→26: number of spaces RIGHT","Invented45→31: number of spaces LEFT","Invented99→49 by10: number of jumps","Invented98→48 by10: number of jumps"],
        [9]=["Invented91−86: difference","Invented72−38: difference","Invented unknown addition55→62: jump length","Invented40−17: difference"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> Descriptions=new Dictionary<int,string[]>{
        [7]=[
            "ORIGINAL warm-up arrow8→12 and reversed12→8: same LENGTH4, opposite operation. Source Activity1 authentic arrows10→15,13→5,12→2,4→14; a same-result equation10+4=14 is NOT pictured4→14.",
            "Source Activity2 ACTUAL number-line image cards A–H with eight printed equation strips: match/start/end/direction, paste source strips and draw ONE unmatched equation. Teacher checks real printed artwork and peer work, not digital proxy.",
            "Real original source arrows, ACTUAL paper cut/glue A–H, correct start and end, arrow length and partner explanation require teacher. No copyrighted restricted Cool-down copied."],
        [8]=[
            "SOURCE warm-up count BACK by10 from99 to49, from98 to48, from95 to45; ones digit unchanged. Source Activity1 draw REAL arrows for equations15+7=22,18−6=12,46+7=53,33−9=24.",
            "Source Activity2 real original arrows3→15,15→12,15→3,12→15: write matching equations and compare the source drawings with a partner in MLR8.",
            "Teacher inspects all original physical points, source number-line ticks, actual arrow start/length/direction and partner discourse. The numeric fields do NOT certify drawing correctness."],
        [9]=[
            "Source warm-up Add To: 20−2,20−17,49−3,67−64. Original Activity1 show difference38−4=34,75−68=7,57−24=33 both as a LEFT take-away or a RIGHT unknown-addend jump.",
            "Source Activity2 MLR6 Elena original 65in string cuts33in leaves32in (two genuine correct source diagrams; reject65→98); Han 87in cuts85in leaves2in, show real distinct strategies with partner.",
            "Teacher judges actual original source figure choices, string story unit, right-count-on vs left-take-away, the drawn endpoints and peer explanation; digital numbers not student source drawings."]
    };
    private static Grade2Unit4ArrowPage Page(int n,Guid adoption,Guid lesson,string title,bool posted,
        Grade2Unit4ArrowAnswers answers,IReadOnlyList<Grade2Unit4ArrowFeedback> feedback)=>
        new(adoption,lesson,title,n,posted,Sources[n],Independent[n],
            Descriptions[n][0],Descriptions[n][1],Descriptions[n][2],answers,feedback);
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
        [FromForm] Grade2Unit4ArrowAnswers answers,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Unit4ArrowAnswers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var source=UsCcssG2Unit4ArrowLessons.Source(lessonNumber,
            answers.SourceA,answers.SourceB,answers.SourceC,answers.SourceD);
        var own=UsCcssG2Unit4ArrowLessons.Independent(lessonNumber,
            answers.IndependentA,answers.IndependentB,answers.IndependentC,answers.IndependentD);
        var f=new List<Grade2Unit4ArrowFeedback>{
            new("Real source arithmetic",source.Correct,source.TeacherReviewNeeded,source.Feedback),
            new("Independent Edulytics digital questions",own.Correct,own.TeacherReviewNeeded,own.Feedback),
            new("Actual original diagram, student point and discussion",null,true,Descriptions[lessonNumber][2])
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",Page(lessonNumber,curriculumAdoptionId,lessonId,title!,true,answers,f));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(int lessonNumber,Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Allowed(lessonNumber)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return(null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code(lessonNumber))return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(actor,lessonId,
            System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code(lessonNumber))return(null,NotFound());
        return(content.Value.Title,null);
    }
}
