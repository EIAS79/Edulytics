using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>First-party virtual ruler numerical validation. Real objects, calibration and partner talk teacher-only.</summary>
public static class UsCcssG2Unit3FourthLessonEstimateCm
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L04";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Compare original A–D image rulers: A, B and D show 4 cm; C's unequal intervals do not",
        "Check students make three genuine estimates before measuring actual classroom items",
        "Pair receives most objects 5–30 cm and at least one 50–90 cm to practise iteration",
        "Observe correctly aligned ZERO tick versus physical ruler edge as appropriate for the actual tool",
        "Check number intervals, accurately iterated 30-cm ruler placements and recorded cm labels",
        "Compare real measurements with original too-low/about-right/too-high estimates and MLR2 partner vocabulary",
        "Digital virtual rulers and independent measurements do not attest physical object length or actual participation"
    ];
    private static bool N(string? v,int min,int max,out int x)=>
        int.TryParse(v,NumberStyles.None,CultureInfo.InvariantCulture,out x)&&x>=min&&x<=max;
    public static Review RulerImage(string? goodLabels,string? oneToFive)
    {
        var good=string.Equals(goodLabels?.Trim(),"ABD",StringComparison.OrdinalIgnoreCase)&&
            N(oneToFive,0,30,out var length)&&length==4;
        return new(good,true,good?"Source A/B/D describe 4 cm and 1→5 spans four centimeters; teacher observes original A–D images."
            :"A,B,D have four equal 1-cm intervals; C does not. Five minus one is four.");
    }
    public static Review FirstPartyTicks(string? start,string? end,string? length)
    {
        var good=N(start,0,29,out var s)&&N(end,1,30,out var e)&&e>s&&
            N(length,1,30,out var l)&&l==e-s;
        return new(good,false,good?"Calibrated first-party ruler segment equals endpoint minus start mark, in cm."
            :"Start reading the TRUE zero/tick; the measured distance is end tick minus start tick, not the endpoint label.");
    }
    public static Review IteratedRuler(string? fullSpanCount,string? extraCm,string? totalCm)
    {
        var good=N(fullSpanCount,1,2,out var count)&&N(extraCm,0,29,out var extra)&&
            N(totalCm,30,90,out var total)&&total==30*count+extra&&total is >=50 and <=90;
        return new(good,true,good?"First-party length is30 cm per full ruler placement plus remainder; real iteration teacher checked."
            :"Use full 30-cm placements with no gap or overlap, then add the remainder, for a length50–90 cm.");
    }
    public static Review EstimateOrder(string? low,string? about,string? high)
    {
        var good=N(low,0,100,out var a)&&N(about,5,90,out var b)&&N(high,0,100,out var c)&&a<b&&b<c;
        return new(good,true,good?"First-party numerical sequence too low < about right < too high; real object's adequacy not verified."
            :"Record three ordered estimates and compare to the actual object length only after measuring it.");
    }
}
