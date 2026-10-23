using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Grade2 original 2.4.4, 2.4.5, 2.4.6 distinct school activities with human-only physical source evidence.</summary>
public static class UsCcssG2Unit4NextThreeActivities
{
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    private static readonly IReadOnlyDictionary<int,string[]> ExpectedSource=new Dictionary<int,string[]>{
        [4]=["30","25","20","10"],
        [6]=["40","38","38","28"]
    };
    private static readonly IReadOnlyDictionary<int,string[]> ExpectedIndependent=new Dictionary<int,string[]>{
        [4]=["11","10","52","17"],
        [5]=["20","4","9","2"],
        [6]=["38","1","2","10"]
    };
    public static readonly IReadOnlyDictionary<int,IReadOnlyList<string>> TeacherRubrics=
        new Dictionary<int,IReadOnlyList<string>>{
            [4]=[
                "Original warmup 35 minus5,10,15,25: 30,25,20,10",
                "True roll of THREE number cubes per partner on shared actual 0..20 paper number line",
                "Each physical counter represents a sum, compare with < > or = (including equality)",
                "TRUE two cube digits produce two-digit number for each player on 0..100 number line",
                "Only multiples of5 printed; actual one-unit point location still measured",
                "Teacher checks original numbers, physical counters, chosen partner line and MLR8 discussion"
            ],
            [5]=[
                "Original teacher displays 0..40 plotted image; students estimate too low/about right/too high",
                "Reveal midpoint20 but do not reveal a UNIQUE original plotted-point value",
                "Use real source four figures: one between50/55; one between20/30; others require original images",
                "Which estimates are most/least certain? Justify with visible 5,10,20,50 benchmarks",
                "Real three-student group arranges TEN authentic publisher source cards on a0..40 chart labelled10,20,30",
                "Revise cards physically as new cards are added, justify every original marked point and do MLR7 gallery walk",
                "Source card digits and exact original picture points not invented"
            ],
            [6]=[
                "OPTIONAL original subtraction warmup65−25,65−27,55−17,46−18",
                "REAL source Number Line Scoot Stage1 Twos/Fives/Tens spinner and board with groups of TWO",
                "Movement can be decomposed across genuine board tracks per authentic game rules",
                "Capture a cube only on an EXACT original track ending, not when passed",
                "Put new cube at0 after capture; first to capture FIVE cubes wins",
                "Real choices Capture Squares Stage3/4, Number Line Scoot Stage1, Number Puzzles Stage4",
                "Teacher verifies spinner, printed board end locations and partner game outcome"
            ]
        };
    private static bool Natural(string? s,int max,out int number)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out number)&&number>=0&&number<=max;
    public static Review Source(int lesson,string? a,string? b,string? c,string? d)
    {
        var values=new[]{a,b,c,d};
        if(lesson==5)
        {
            bool plausible=Natural(a,40,out var low)&&Natural(b,40,out var about)&&
                Natural(c,40,out var high)&&Natural(d,40,out var revised)&&
                low<about&&about<high&&revised>0&&revised<40;
            return new(null,true,plausible
                ?"Estimates are ordered, but SOURCE point is intentionally open-ended: teacher checks original image, reasons and revision."
                :"Original 0..40 point is open-ended, not uniquely gradable. Offer plausible too-low/about-right/too-high and midpoint20-informed revision.");
        }
        if(!ExpectedSource.TryGetValue(lesson,out var answers))
            return new(false,true,"Lesson not in this Grade2 source sequence");
        bool valid=values.Select((value,i)=>Natural(value,100,out var n)&&n==int.Parse(answers[i],CultureInfo.InvariantCulture)).All(x=>x);
        return new(valid,true,valid
            ?"Original Number Talk numeric answers correct; teacher must still check mental strategies and physical source participation."
            :lesson==4?"Source answers are30,25,20,10 for35−5,35−10,35−15,35−25."
                     :"Optional source subtraction answers are40,38,38,28.");
    }
    public static Review Independent(int lesson,string? a,string? b,string? c,string? d)
    {
        if(!ExpectedIndependent.TryGetValue(lesson,out var answers))
            return new(false,false,"Unknown separate first-party activity");
        var inputs=new[]{a,b,c,d};
        bool valid=inputs.Select((v,i)=>Natural(v,100,out var n)&&n==int.Parse(answers[i],CultureInfo.InvariantCulture)).All(x=>x);
        return new(valid,false,valid?"Independent Edulytics numeric exercises correct: not evidence of actual dice, publisher points or game."
            :"Recompute the four clearly FIRST-PARTY arithmetic examples shown on the page.");
    }
}
