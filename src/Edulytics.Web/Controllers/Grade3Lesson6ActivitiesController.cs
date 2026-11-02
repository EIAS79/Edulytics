using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;

/// <summary>Scale selection is reasoning, not just arithmetic; teacher judges graph work.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade3-u01-l06")]
public sealed class Grade3Lesson6ActivitiesController(
    IStudentPrivatePracticeService practice,ILessonContentService lessons) : Controller
{
    private const string Code=UsCcssG3Unit1Lesson6ChooseScale.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View(new Grade3Lesson6PracticePage(curriculumAdoptionId,lessonId,
            false,new Grade3Lesson6Answers(),null,5,5,[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade3Lesson6Answers answers,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null || typeof(Grade3Lesson6Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Invalid submitted answers.");
        var patternScale=int.TryParse(answers.PatternScale,out var p) && p is 2 or 5 or 10?p:5;
        var seasonScale=int.TryParse(answers.SeasonScale,out var s) && s is 2 or 5 or 10?s:5;
        var pattern=Parse(answers);
        var valid=UsCcssG3Unit1Lesson6ChooseScale.TryValidatePatternCounts(pattern,out var reason);
        var enteredSeasons=ParseSeasonBars(answers);
        var validSeasons=enteredSeasons is { Count:4 } &&
            enteredSeasons.All(row=>row.Count>=0 && row.Count<=100);
        var questions=new[]{
            ("L06-SOURCE-PATTERN-TRIANGLE","Pattern-block source: triangles",answers.Triangle),
            ("L06-SOURCE-PATTERN-SQUARE","Pattern-block source: squares",answers.Square),
            ("L06-SOURCE-PATTERN-TRAPEZOID","Pattern-block source: trapezoids",answers.Trapezoid),
            ("L06-SOURCE-PATTERN-HEXAGON","Pattern-block source: hexagons",answers.Hexagon),
            ("L06-SOURCE-SEASON-BAR-WINTER","Student bar: Winter 24",answers.SeasonWinterBar),
            ("L06-SOURCE-SEASON-BAR-SPRING","Student bar: Spring 13",answers.SeasonSpringBar),
            ("L06-SOURCE-SEASON-BAR-SUMMER","Student bar: Summer 40",answers.SeasonSummerBar),
            ("L06-SOURCE-SEASON-BAR-FALL","Student bar: Fall 22",answers.SeasonFallBar),
            ("L06-SOURCE-PET-CAT","Pet lifespan: Cat",answers.PetCat),
            ("L06-SOURCE-PET-PYTHON","Pet lifespan: Python",answers.PetPython),
            ("L06-SOURCE-PET-TOTAL","Pet lifespan: Sum of all values",answers.PetTotal),
            ("L06-SOURCE-SEASONS-SPRING","Favorite season: Spring",answers.Spring),
            ("L06-SOURCE-SEASONS-TOTAL","Favorite season: Total",answers.SeasonTotal),
            ("L06-SOURCE-SEASONS-SUMMER-MINUS-SPRING","Favorite season: Summer minus Spring",answers.SeasonDifference),
            ("L06-EDU-CD-MUSIC","Edulytics Cool-down: Music",answers.CoolMusic),
            ("L06-EDU-CD-SCIENCE","Edulytics Cool-down: Science",answers.CoolScience),
            ("L06-EDU-CD-DIFFERENCE","Edulytics Cool-down: Science minus Music",answers.CoolDifference)
        };
        var feedback=questions.Select(x=>{
            if(!int.TryParse(x.Item3,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)||
                !UsCcssG3Unit1Lesson6ChooseScale.TryGradeNumeric(x.Item1,n,out var correct))
                return new Grade3Lesson6Feedback(x.Item2,false,false,"Enter a whole number.");
            return new Grade3Lesson6Feedback(x.Item2,correct,false,
                correct?"The number matches the relevant supplied graph/table."
                :"Read the category and units again; a graph's scale does not change the raw count.");
        }).ToList();
        feedback.Add(new("Original 2021 pattern-block image count",null,true,valid
            ? "The counts are compared with the geometric audit of the actual source SVG; the teacher reviews visual counting strategy and your graph."
            : reason));
        feedback.Add(new("Drawn seasonal graph and four bar heights",null,true,validSeasons
            ? "Four bar values are recorded; numeric checks appear above. Teacher reviews placement, labels, title, zero baseline and scale choice."
            : "Enter valid nonnegative bar heights (0 to 100) for all four seasons."));
        feedback.Add(new("Source 2021 Cool-down: two separate reflection responses",null,true,
            "Teacher reviews why you chose your scale and what you learned for future scaled bar graphs. Do not replace these with fictional club arithmetic."));
        feedback.Add(new("Source lesson synthesis and Math Community",null,true,
            "Teacher reviews explanation before the Cool-down and the classroom-norm reflection after it."));
        feedback.Add(new("Justify a scale of 2, 5 or 10",null,true,
            "Teacher judges whether the selected step is readable, represents every count exactly, and is explained mathematically."));
        feedback.Add(new("Created graph and Edulytics Cool-down reflection",null,true,
            "Teacher must review bar lengths, title, categories, exact intermediate values, mathematical explanations and collaboration."));
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View("Index",new Grade3Lesson6PracticePage(curriculumAdoptionId,lessonId,
            true,answers,valid?pattern:null,patternScale,seasonScale,feedback,
            validSeasons?enteredSeasons:null));
    }
    private static List<UsCcssG3Unit1Lesson6ChooseScale.CategoryCount>? Parse(Grade3Lesson6Answers a)
    {
        var vals=new[]{a.Triangle,a.Square,a.Trapezoid,a.Hexagon};
        var names=UsCcssG3Unit1Lesson6ChooseScale.PatternCategories;
        var rows=new List<UsCcssG3Unit1Lesson6ChooseScale.CategoryCount>();
        for(var i=0;i<names.Count;i++){
            if(!int.TryParse(vals[i],NumberStyles.Integer,CultureInfo.InvariantCulture,out var value))
                return null;
            rows.Add(new(names[i],value));
        }
        return rows;
    }
    private static List<UsCcssG3Unit1Lesson6ChooseScale.CategoryCount>? ParseSeasonBars(Grade3Lesson6Answers a)
    {
        var inputs=new[]{a.SeasonWinterBar,a.SeasonSpringBar,a.SeasonSummerBar,a.SeasonFallBar};
        var source=UsCcssG3Unit1Lesson6ChooseScale.SourceSeasons;
        var rows=new List<UsCcssG3Unit1Lesson6ChooseScale.CategoryCount>();
        for(var i=0;i<source.Count;i++)
        {
            if(!int.TryParse(inputs[i],NumberStyles.Integer,CultureInfo.InvariantCulture,out var count))
                return null;
            rows.Add(new(source[i].Category,count));
        }
        return rows;
    }
    private async Task<IActionResult?> CheckAccess(Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return Forbid();
        var w=await practice.GetWorkspaceAsync(actor,adoption,ct);
        if(w.SelectedCurriculumAdoptionId!=adoption)return NotFound();
        var item=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(item is null || item.LessonCode!=Code)return NotFound();
        var published=await lessons.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return published.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound();
        return published.Value.LessonCode==Code?null:NotFound();
    }
}
