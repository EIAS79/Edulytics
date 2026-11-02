using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Source-aligned Grade2 IM2021 Lessons 2.4.7 to 2.4.9; teacher judges actual arrows and images.</summary>
public static class UsCcssG2Unit4ArrowLessons
{
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    private static readonly IReadOnlyDictionary<int,int[]> SourceAnswers=new Dictionary<int,int[]>{
        [7]=[5,8,10,10],
        [8]=[22,12,53,24],
        [9]=[18,3,46,3]
    };
    private static readonly IReadOnlyDictionary<int,int[]> FirstPartyAnswers=new Dictionary<int,int[]>{
        [7]=[9,9,27,12],
        [8]=[7,14,5,5],
        [9]=[5,34,7,23]
    };
    public static readonly IReadOnlyDictionary<int,IReadOnlyList<string>> TeacherRubrics=
        new Dictionary<int,IReadOnlyList<string>>{
            [7]=[
                "Real warm-up source arrows8 to12 right4 and12 to8 left4",
                "Source Activity1 real arrows10→15,13→5,12→2,4→14 have lengths5,8,10,10",
                "The true source4→14 is described4+10=14, not10+4=14 even though sums match",
                "Source Activity2 ACTUAL publisher A-H image cards, match eight printed equation strips",
                "One original source equation is unmatched and student draws it on real blank number line",
                "Teacher checks direction, start/end, real cut-and-glue evidence and peer reasons"
            ],
            [8]=[
                "Original 99→49,98→48,95→45 in jumps of ten; ones digits unchanged",
                "Original Source Activity1 true arrow15→22,18→12,46→53,33→24; drawn on source axes",
                "Original Activity2 true diagrams3→15,15→12,15→3,12→15",
                "Begin each equation at pictured source START not arbitrary equivalent operand",
                "True equal one-unit positions, number line arrows and MLR8 partner dialogue are teacher observed",
                "Restricted publisher Cool-down remains separate from first-party exact questions"
            ],
            [9]=[
                "Original warm-up:20−2=18,20−17=3,49−3=46,67−64=3",
                "Source Activity1 real diagrams38−4=34,75−68=7,57−24=33",
                "Taking away: start larger and jump LEFT removed quantity to result",
                "Unknown addend: start smaller and jump RIGHT the difference length to larger",
                "Elena original string65in cuts33in leaves32in; choose TWO authentic source correct diagrams",
                "Reject publisher diagram65→98(+33) as Elena subtraction representation",
                "Han original string87in cuts85in leaves2in; real partner compares two methods",
                "Teacher inspects actual drawing endpoints, source figures, MLR6 Three Reads and real discussion"
            ]
        };
    private static bool N(string? s,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=0&&value<=100;
    private static Review Match(int lesson,string? a,string? b,string? c,string? d,
        IReadOnlyDictionary<int,int[]> bank,bool needsTeacher)
    {
        if(!bank.TryGetValue(lesson,out var expected))return new(false,needsTeacher,"Unknown source lesson");
        var values=new[]{a,b,c,d};bool ok=values.Select((s,i)=>N(s,out var n)&&n==expected[i]).All(x=>x);
        return new(ok,needsTeacher,ok
            ?needsTeacher?"Original-source numerical results hold; educator still checks true image arrows, physical work and peer explanation."
                         :"Independently authored numerical values correct, NOT publisher class evidence."
            :needsTeacher?"Check the source values AND actual diagram and jump direction."
                         :"Recompute the first-party numerical jump/difference prompts.");
    }
    public static Review Source(int lesson,string? a,string? b,string? c,string? d)=>
        Match(lesson,a,b,c,d,SourceAnswers,true);
    public static Review Independent(int lesson,string? a,string? b,string? c,string? d)=>
        Match(lesson,a,b,c,d,FirstPartyAnswers,false);
}
