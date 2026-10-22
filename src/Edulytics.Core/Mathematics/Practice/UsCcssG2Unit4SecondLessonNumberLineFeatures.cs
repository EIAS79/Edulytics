using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Exact digital mathematics is not certification of actual student string, cards or source pictures.</summary>
public static class UsCcssG2Unit4SecondLessonNumberLineFeatures
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U04:L02";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Real original Choral Count by5 from0 to100 including patterned last digits0 and5",
        "Real physical classroom taut yarn line0..30 with anchored zero and random-order number cards",
        "Students revise actual card placements as more values arrive; check equal unit spaces, not just increasing order",
        "Cards not handed out do not remove equal-distance unit intervals",
        "Group of3 inspects PUBLISHER original Jada drawing 0..10 for missing/inconsistent tick placement",
        "Group of3 inspects ACTUAL publisher Andre and Elena figures; image details never invented from plain-text archive",
        "Provide one authentic improvement for each Jada/Andre/Elena figure with evidence about unit length and numeric labels",
        "Original unlabelled 11-tick0..10 line: label1..10 beyond the anchored zero",
        "MLR8 group discussion, actual revisions and classroom participation require educator human judgment",
        "Edulytics independent numeric answers never pretend to be actual source diagram-specific claims or official Cool-down"
    ];
    private static bool N(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review ChoralCount(string? first,string? afterFive,string? halfway,string? last)
    {
        bool ok=N(first,0,100,out var a)&&a==5&&
            N(afterFive,0,100,out var b)&&b==10&&
            N(halfway,0,100,out var c)&&c==50&&
            N(last,0,100,out var d)&&d==100;
        return new(ok,true,ok?"Five-unit landmarks correct; original oral choral count and noticed pattern require teacher review."
            :"Count from0 by adding5: first5, next10, halfway50 and final100.");
    }
    public static Review SourceBlankLine(string? afterZero,string? halfway,string? last,string? tickCount)
    {
        bool ok=N(afterZero,0,10,out var a)&&a==1&&
            N(halfway,0,10,out var b)&&b==5&&
            N(last,0,10,out var c)&&c==10&&
            N(tickCount,0,21,out var d)&&d==11;
        return new(ok,true,ok?"Original 0..10 line 11 ticks and 10 unit intervals; full physical line still teacher-reviewed."
            :"Number0 anchors the first of11 ticks. The next is1, middle5 and last10.");
    }
    public static Review FirstPartyNumbers(string? cardEighteen,string? distanceTwelveToEighteen,string? countFiveLabels)
    {
        bool ok=N(cardEighteen,0,30,out var a)&&a==18&&
            N(distanceTwelveToEighteen,0,30,out var b)&&b==6&&
            N(countFiveLabels,0,30,out var c)&&c==21;
        return new(ok,false,ok?"Fictional card position18, interval gap18-12=6, 0..100 by5 has21 numbers including0."
            :"Independent numeric answers:18 intervals from0,6 intervals from12 to18, and21 multiples of5 from0 to100 inclusive.");
    }
}
