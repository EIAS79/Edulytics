using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Four numerical source checks. Original MLR6/MLR8 and secret partner choices need a teacher.</summary>
public static class UsCcssG2Unit3SixthLessonReptileStories
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L06";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe MLR6 Three Reads for Lin's lizard62 cm SHORTER than Jada by19 cm: Jada bigger unknown81 cm",
        "Observe real tape-diagram choice and reasoning for Mai71 cm LONGER than Diego by17 cm: Diego smaller54 cm",
        "Partner A genuine reptile lengths: day gecko28, komodo98, baby cobra46, iguana65 cm",
        "Partner B genuine reptile lengths: ribbon snake83, gila monster55, baby alligator71, ringneck38 cm",
        "Observe two real partners SECRETLY choose opposite lists, create a correct difference clue and deduce animals",
        "Evaluate MLR8 compare-language and real student diagrams with labeled centimeter quantities",
        "Digital arithmetic cannot prove source pair guessing participation, authentic original diagrams or protected cooldown"
    ];
    private static bool Num(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review NumberTalk(string? a,string? b,string? c,string? d)
    {
        var ok=Num(a,0,100,out var x)&&x==10&&Num(b,0,100,out var y)&&y==20&&
            Num(c,0,100,out var z)&&z==30&&Num(d,0,100,out var w)&&w==40;
        return new(ok,true,ok?"Source mental10,20,30,40 correct; teacher evaluates the ten-more reasoning.":"Think 5+5,15+5,15+15,15+25.");
    }
    public static Review OppositeKeywordStories(string? jada,string? diego)
    {
        var ok=Num(jada,0,100,out var j)&&j==81&&Num(diego,0,100,out var d)&&d==54;
        return new(ok,true,ok?"Jada is LONGER81 cm; Diego is SHORTER54 cm. Teacher checks actual tape diagrams."
            :"Lin62 is19 SHORTER -> Jada62+19=81; Mai71 is17 LONGER -> Diego71−17=54.");
    }
    public static Review SourceReptileLengths(string? gecko,string? komodo,string? cobra,string? iguana,
        string? ribbon,string? gila,string? alligator,string? ringneck)
    {
        var s=new[]{gecko,komodo,cobra,iguana,ribbon,gila,alligator,ringneck};
        var expected=new[]{28,98,46,65,83,55,71,38};
        var ok=s.Length==expected.Length&&s.Select((v,i)=>Num(v,0,100,out var n)&&n==expected[i]).All(x=>x);
        return new(ok,true,ok?"Source A/B list numbers match; partner secret choices and story construction still teacher-reviewed."
            :"Check the source A lengths 28/98/46/65 and source B lengths83/55/71/38 in centimeters.");
    }
    public static Review Independent(string? longer,string? shorter,string? gap)
    {
        var ok=Num(longer,0,100,out var l)&&l==64&&Num(shorter,0,100,out var s)&&s==65&&
            Num(gap,0,100,out var g)&&g==34;
        return new(ok,false,ok?"Independent Edulytics:48+16=64,92−27=65,73−39=34 cm."
            :"Recheck first-party longer48+16, shorter92−27, difference73−39.");
    }
}
