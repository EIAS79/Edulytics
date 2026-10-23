using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Grade 3 L05 exact 2021 comparison data plus class dependent graph helper.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade3-u01-l05")]
public sealed class Grade3Lesson5ActivitiesController(
    IStudentPrivatePracticeService practice,ILessonContentService lessonContent) : Controller
{
    private const string Code=UsCcssG3Unit1Lesson5ScaledBarGraph.LessonCode;

    [HttpGet("")]
    [ResponseCache(Location=ResponseCacheLocation.None,NoStore=true)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var failure=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View(new Grade3Lesson5PracticePage(curriculumAdoptionId,lessonId,false,
            new Grade3Lesson5Answers(),null,2,[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(Location=ResponseCacheLocation.None,NoStore=true)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade3Lesson5Answers answers,CancellationToken ct)
    {
        var failure=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(answers is null || typeof(Grade3Lesson5Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Invalid or oversized response.");

        var scale=answers.SelectedScale=="5"?5:2;
        var data=Parse(answers);
        var valid=UsCcssG3Unit1Lesson5ScaledBarGraph.TryValidatePriorClassCounts(
            data,out var validation);
        var items=new List<Grade3Lesson5Feedback> {
            new("Reused class counts",null,true,valid
                ? "Counts are mathematically well formed; teacher checks that these are exactly the original L04 class responses."
                : validation)
        };
        items.Add(Source("L05-SOURCE-BIKE","Original bike bar",answers.OriginalBikeCount));
        items.Add(Source("L05-SOURCE-BUS-VAN","Original bus or van bar",answers.OriginalBusVanCount));
        items.Add(Source("L05-SOURCE-TOTAL","Original total students",answers.OriginalTotal));
        items.Add(Source("L05-SOURCE-BIKE-MINUS-CAR","Original bike minus car",answers.OriginalBikeCarDifference));
        items.Add(Cool("L05-EDU-CD-ART","Independent Cool-down Art Club",answers.CoolDownArt));
        items.Add(Cool("L05-EDU-CD-SCIENCE-MINUS-MUSIC","Independent Cool-down Science minus Music",answers.CoolDownDifference));
        items.Add(Cool("L05-EDU-CD-TOTAL","Independent Cool-down grand total",answers.CoolDownTotal));
        items.Add(new("Scale selection and accurate personal bar graph",null,true,
            "Teacher reviews bar lengths and whether 2 or 5 is an effective tick step for the original class survey, including nonmultiples of the step."));
        items.Add(new("Compare two original bar graphs and discuss",null,true,
            "Teacher must check that the graphs show the SAME four counts although one axis uses ones and the other twos."));
        items.Add(new("Independent Cool-down drawing and community reflection",null,true,
            "Teacher assesses correct first-party axis labels, graph and explanation. No automatic approval."));

        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View("Index",new Grade3Lesson5PracticePage(curriculumAdoptionId,lessonId,true,
            answers,valid?data:null,scale,items));
    }

    private static List<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>? Parse(
        Grade3Lesson5Answers a)
    {
        var vals=new[]{a.Car,a.Train,a.Boat,a.Balloon,a.Plane,a.Helicopter};
        var categories=UsCcssG3Unit1Lesson4ScaledTravelSurvey.Categories;
        var result=new List<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>();
        for(var i=0;i<categories.Count;i++)
        {
            if(!int.TryParse(vals[i],NumberStyles.Integer,CultureInfo.InvariantCulture,out var n))
                return null;
            result.Add(new(categories[i],n));
        }
        return result;
    }

    private static Grade3Lesson5Feedback Source(string id,string title,string? value)
    {
        if(!int.TryParse(value,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n))
            return new(title,false,false,"Enter a whole number.");
        var eligible=UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeSource(id,n,out var correct);
        return new(title,eligible?correct:null,!eligible,
            eligible?(correct?"Correct for the source graph.":"Read both bar graphs and use the actual category values.")
                :"No numeric rubric registered.");
    }

    private static Grade3Lesson5Feedback Cool(string id,string title,string? value)
    {
        if(!int.TryParse(value,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n))
            return new(title,false,false,"Enter a whole number.");
        var eligible=UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeCoolDown(id,n,out var correct);
        return new(title,eligible?correct:null,!eligible,
            eligible?(correct?"Correct for original fictional Edulytics club data."
                :"Count the correct category and use the scale.")
                :"Not auto-gradeable.");
    }

    private async Task<IActionResult?> CheckAccess(Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return Forbid();
        var workspace=await practice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return NotFound();
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null || lesson.LessonCode!=Code)return NotFound();
        var live=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(live.Value is null)
            return live.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound();
        return live.Value.LessonCode==Code?null:NotFound();
    }
}
