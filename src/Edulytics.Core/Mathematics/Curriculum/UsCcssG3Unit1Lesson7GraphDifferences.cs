namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Exact Grade 3 U01 L07 publicly readable 2021 HTML table/alt data.
/// Source bug bar graph has ticks by ten; source teacher instructions accept
/// reasonable estimates, so NO approximate bug response may be auto-passed.
/// </summary>
public static class UsCcssG3Unit1Lesson7GraphDifferences
{
    public const string LessonCode="PED:US-CCSS-MATH:G3:U01:L07";
    public sealed record Bar(string Category,int Count);
    public static IReadOnlyList<Bar> Bugs {get;}=Array.AsReadOnly(new[]{
        new Bar("Ants",62),new Bar("Ladybugs",11),
        new Bar("Beetles",19),new Bar("Spiders",7)
    });
    public static IReadOnlyList<Bar> IndependentCoolDown {get;}=Array.AsReadOnly(new[]{
        new Bar("Soccer",32),new Bar("Basketball",17),
        new Bar("Swimming",21),new Bar("Tennis",10)
    });

    public static bool TryGradeExact(string? id,int answer,out bool correct)
    {
        correct=false;
        int? expected=id switch{
            "L07-SEASONS-TOTAL"=>99,
            "L07-SEASONS-SPRING-OR-FALL"=>35,
            "L07-SEASONS-SUMMER-MINUS-WINTER"=>16,
            "L07-SEASONS-FALL-MINUS-SPRING"=>9,
            "L07-EDU-CD-SOCCER-MINUS-TENNIS"=>22,
            "L07-EDU-CD-SWIMMING-PLUS-BASKETBALL"=>38,
            "L07-EDU-CD-TOTAL"=>80,
            _=>null
        };
        if(expected is null)return false;
        correct=answer==expected;return true;
    }
    public static bool TryCheckApproximateBugAnswer(string? id,int answer,out bool plausible)
    {
        plausible=false;
        int? sourceNumber=id switch{
            "L07-BUGS-TOTAL"=>99,
            "L07-BUGS-ANTS-MINUS-SPIDERS"=>55,
            "L07-BUGS-ANTS-MINUS-LADYBUGS"=>51,
            _=>null
        };
        if(sourceNumber is null)return false;
        // Provisional diagnostic only: the source accepts reasonable visual
        // estimates, assessed in context by a teacher. Never make a pass grade.
        plausible=Math.Abs(answer-sourceNumber.Value)<=5;
        return true;
    }
    public static bool RequiresTeacherReview(string kind)=>kind is
        "scaled-chart-estimate" or "partner-authored-questions"
            or "justify-add-or-subtract" or "warmup-dot-grouping"
            or "teacher-validated-graph-from-prior-lesson"
            or "original-cooldown-explanation";
}
