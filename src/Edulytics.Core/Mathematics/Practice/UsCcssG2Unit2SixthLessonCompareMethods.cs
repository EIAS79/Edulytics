using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Source-true comparison mathematics plus original digital Target
/// Numbers Stage4 two-turn arithmetic; no claim of actual game results.</summary>
public static class UsCcssG2Unit2SixthLessonCompareMethods
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L06";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Student explains which 64 equality is false and why moving tens changes the remaining ones",
        "Original student's base-ten drawing shows both 82−9 diagrams and relation to Tyler's equations",
        "Observe paired Target Numbers Stage4 with physical blocks and NUMBER CARDS 1–9 only",
        "Each of up to six real turns removes chosen card's ONES or TENS, never both",
        "Check signed equations, updated totals, remaining nonnegative and authentic nearest-to-zero winner",
        "Observe MLR2 language in comparing decomposing diagrams and lesson synthesis50−7",
        "A numeric digital check cannot verify actual game, physical blocks, written diagram or partner participation"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review Warmup(string? first,string? second,string? third)
    {
        var good=first=="True"&&second=="True"&&third=="False";
        return new(good,true,good?"True; True; False. Since30+24=54, it cannot equal64. Teacher reviews the explanation."
            :"64=60+4 and64=50+14 are true;64=30+24 is false.");
    }
    public static Review Diagram(string? tens,string? ones,string? after)
    {
        var good=N(tens,0,9,out var t)&&t==7&&N(ones,0,19,out var o)&&o==12
            &&N(after,0,100,out var a)&&a==73;
        return new(good,true,good?"82=7 tens+12 ones and after taking9 singles the result is73. Original drawings and MLR2 remain teacher-reviewed."
            :"Compare Diego, Elena and Tyler using82=70+12,12−9=3,70+3=73.");
    }
    public static Review Target(string? first,string? card1,string? mode1,string? after1,
        string? card2,string? mode2,string? after2)
    {
        var valid=N(first,99,99,out var start)&&N(card1,1,9,out var c1)&&
            N(after1,0,99,out var next)&&N(card2,1,9,out var c2)&&
            N(after2,0,99,out var finish)&&
            (mode1 is "Ones" or "Tens")&&(mode2 is "Ones" or "Tens");
        var correct=valid&&start-(mode1=="Ones"?c1:10*c1)==next&&
            next-(mode2=="Ones"?c2:10*c2)==finish;
        return new(correct,true,correct?"Two FIRST-PARTY example turns have legal nonnegative equations. Teacher verifies actual up-to-six-round partner game and winner."
            :"Start99. Card must be1–9; subtract exactly that number of ones OR tens, update the next starting number, never go below zero.");
    }
    public static Review Cooldown(string? a,string? b,string? c)
    {
        var good=N(a,0,100,out var x)&&x==63&&N(b,0,100,out var y)&&y==53&&
            N(c,0,100,out var z)&&z==84;
        return new(good,false,good?"Independent Edulytics exit correct:71−8=63,60−7=53,92−8=84."
            :"Try71=60+11,60=50+10 and92=80+12 before subtracting. This is not publisher Mai's Method.");
    }
}
