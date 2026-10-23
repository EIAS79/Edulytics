using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// First-party G2 U01 L16 Grade 2 2021 source library and Three Reads
/// comparisons. All numerical checks require the actual position of ?.
/// Learner may draw a tape or use another mathematical representation;
/// teacher separately scores the open explanatory work.
/// </summary>
public static class UsCcssG2Unit1SixteenthLessonComparePractice
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L16";
    public const string Version="edulytics-original-g2-u01-l16-place-value-compare-v1";

    public sealed record CompareStory(string Id,string Text,string BiggerPerson,
        string SmallerPerson,int Larger,int Smaller,int Difference,
        string UnknownPosition)
    {
        public int Answer=>UnknownPosition switch{
            "Larger"=>Larger,"Smaller"=>Smaller,"Difference"=>Difference,
            _=>throw new InvalidOperationException("Invalid compare unknown")
        };
    }
    public static readonly IReadOnlyList<CompareStory> OriginalSourceStories=
    [
        new("LibraryBooks","Priya returned 29 books; Andre returned 8 MORE. How many did Andre return?",
            "Andre","Priya",37,29,8,"Larger"),
        new("LibraryPages","Andre read 45 pages; Priya read 20 FEWER. How many did Priya read?",
            "Andre","Priya",45,25,20,"Smaller"),
        new("ThreeReads1","Noah read 27 pages; Jada read 10 FEWER. How many did Jada read?",
            "Noah","Jada",27,17,10,"Smaller"),
        new("ThreeReads2","Noah read 25 minutes; Jada spent 30 MORE. How many minutes did Jada spend reading?",
            "Jada","Noah",55,25,30,"Larger"),
        new("ThreeReads3","Jada read 47 pages; Noah read 20. How many FEWER pages did Noah read?",
            "Jada","Noah",47,20,27,"Difference"),
        new("ThreeReads4","Jada stacked 28 books; Noah stacked 14 MORE. How many books did Noah stack?",
            "Noah","Jada",42,28,14,"Larger")
    ];
    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Explanation);

    public static Review CheckWarmup(string? first,string? second,string? third)
    {
        var correct=first=="1"&&second=="1"&&third=="0";
        return new(true,correct,true,correct
            ?"18+5=18+2+3 true, 28+5=30+3 true, 38+4=40+3 false (42 versus 43)."
            :"Check the value on both sides. Composing tens must preserve the same number.");
    }

    private static readonly Regex Eq=new(
        @"^\s*(\d{1,3})\s*([+−-])\s*(\d{1,3})\s*=\s*(\d{1,3})\s*$",
        RegexOptions.Compiled|RegexOptions.CultureInvariant);
    public static Review CheckOriginalProblem(
        string? id,string? unknown,string? biggerPerson,
        string? numericalAnswer,string? equation)
    {
        var source=OriginalSourceStories.SingleOrDefault(x=>x.Id==id);
        if(source is null)
            return new(false,false,true,"Select a valid source comparison story.");
        var correct=unknown==source.UnknownPosition &&
            biggerPerson==source.BiggerPerson &&
            int.TryParse(numericalAnswer,NumberStyles.None,
                CultureInfo.InvariantCulture,out var value) &&
            value==source.Answer &&
            IsRelatedEquation(source,equation);
        return new(true,correct,true,correct
            ?"Correct unknown position and source quantity, and a valid related addition/subtraction equation. Teacher evaluates the chosen representation and Three Reads reasoning."
            :"Label who has MORE, which tape part is ?, and write an equation with the same larger/smaller/difference relationship.");
    }
    public static bool IsRelatedEquation(CompareStory source,string? input)
    {
        var m=Eq.Match(input??"");
        if(!m.Success)return false;
        var a=int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture);
        var b=int.Parse(m.Groups[3].Value,CultureInfo.InvariantCulture);
        var c=int.Parse(m.Groups[4].Value,CultureInfo.InvariantCulture);
        var op=m.Groups[2].Value;
        if(op=="+")
            return a+b==c && c==source.Larger &&
                (a==source.Smaller&&b==source.Difference ||
                 a==source.Difference&&b==source.Smaller);
        return a-b==c && a==source.Larger &&
            (b==source.Smaller&&c==source.Difference ||
             b==source.Difference&&c==source.Smaller);
    }

    public static Review CheckNewEdulyticsCoolDown(
        string? larger,string? smaller,string? difference)
    {
        var raw=new[]{larger,smaller,difference};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter each missing whole-number quantity.");
        var answers=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var good=answers.SequenceEqual(new[]{54,22,22});
        return new(true,good,false,good
            ?"Dina 34+20=54, Mira 52−30=22 and Tami63−Max41=22."
            :"Identify whether the missing part is the larger amount, smaller amount or difference, then use place value.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Learner identifies the larger person, smaller person and unknown position for BOTH original library stories and FOUR original Three Reads source stories.",
        "Learner uses make-ten strategies for 29+8 and 28+14 when useful, and place-value subtraction of tens for 45−20, 27−10 and 47−20.",
        "Learner may choose a correctly labeled tape diagram, another original pictorial representation, a number sentence or a verbal strategy. Do not force one representation if mathematics is sound.",
        "Three Reads procedure invites understanding the story, naming quantities, then answering the full question; teacher verifies real learner discussion and partner explanations.",
        "Student ties each equation to actual source quantities and verifies direction. Machine checks exact numerical answers but does not certify explanation, drawing, personal discussion or teacher approval."
    ];
}
