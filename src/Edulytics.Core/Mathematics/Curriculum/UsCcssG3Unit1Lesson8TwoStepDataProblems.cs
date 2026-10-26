namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Local IM 2021 G3 U01 L08 two-step chart quantities; publisher images are
/// not embedded. Open Three Reads reasoning remains teacher-assessed.
/// Cool-down figures are entirely first-party and unrelated to the publisher
/// gated Reading Time task.
/// </summary>
public static class UsCcssG3Unit1Lesson8TwoStepDataProblems
{
    public const string LessonCode="PED:US-CCSS-MATH:G3:U01:L08";
    public sealed record Bar(string Category,int Value);
    public static IReadOnlyList<Bar> Feelings {get;}=Array.AsReadOnly(new[]{
        new Bar("Excited",55),new Bar("Nervous",23),new Bar("Curious",25)
    });
    public static IReadOnlyList<Bar> Trees {get;}=Array.AsReadOnly(new[]{
        new Bar("Oak",29),new Bar("Pine",82),new Bar("Maple",15),new Bar("Fir",58)
    });
    // Original 2021 first edition Reading Time graph, values visually read
    // between 5-unit grid marks. Published teaching exemplars differ on the
    // unmarked morning bar (11 or about 12): numbers are REASONABLE ESTIMATES.
    // No approximate source Cool-down answer may be automatically passed.
    public static IReadOnlyList<Bar> OriginalSourceReadingTime {get;}=Array.AsReadOnly(new[]{
        new Bar("Morning",12),new Bar("Lunchtime",25),
        new Bar("Afternoon",23),new Bar("Evenings",44)
    });
    public static bool TryCheckSourceReadingTimeEstimate(string? id,int answer,out bool plausible)
    {
        plausible=false;
        int? reference=id switch{
            "L08-SOURCE-CD-MORNING-VS-AFTERNOON"=>12,
            "L08-SOURCE-CD-EVENING-VS-MORNING-LUNCH"=>7,
            _=>null
        };
        if(reference is null)return false;
        plausible=Math.Abs(answer-reference.Value)<=2;
        return true; // Teacher review ALWAYS; this is a diagnostic, not a mark.
    }
    public static IReadOnlyList<Bar> OriginalEdulyticsCoolDown {get;}=Array.AsReadOnly(new[]{
        new Bar("Adventure Books",60),new Bar("Science Books",23),
        new Bar("Comics",18)
    });
    public static bool TryGradeNumeric(string? id,int response,out bool correct)
    {
        correct=false;
        int? expected=id switch{
            "L08-FEELINGS-TWO-STEP"=>7,
            "L08-TREES-PINE-FIR"=>24,
            "L08-TREES-PINE-VS-OAK-MAPLE"=>38,
            "L08-TREES-PINE-OAK"=>53,
            "L08-TREES-FIR-VS-OAK-MAPLE"=>14,
            "L08-EDU-CD-ADVENTURE-SCIENCE"=>37,
            "L08-EDU-CD-ADVENTURE-VS-SCIENCE-COMICS"=>19,
            "L08-EDU-CD-TOTAL"=>101,
            _=>null
        };
        if(expected is null)return false;
        correct=response==expected;return true;
    }
    public static bool RequiresHumanReview(string activity)=>activity is
        "three-reads" or "write-two-methods" or "word-problem-context"
        or "compare-expressions" or "source-scale-discussion"
        or "cool-down-own-equations" or "teacher-student-work"
        or "original-source-reading-time-estimate" or "source-student-section-summary";
}
