using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Exact teacher-given tape lengths, not attested physical student measurements.</summary>
public static class UsCcssG2Unit3FifthLessonMeters
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L05";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Choral count20–30 then120–130, explain aligned counts100 apart without forcing conceptual hundreds mastery",
        "Actually lay source floor-tape models, not real reptiles: A58 B72 C44 D180cm",
        "Observe three- or four-student teams using centimeter cubes,10cm tools and rulers, recording A–D measurements",
        "Introduce real meterstick: 100cm equals1m and find zero/50cm marks",
        "Observe D180cm measured AGAIN as1m80cm and compare tools; approximate2m is not exact2m",
        "Check source E adult alligator336cm, F adult cobra190cm, G ribbon snake282cm with meterstick and cm remainder",
        "Observe authentic tape tool alignments, MLR2 vocabulary, real partner discussion and no gaps or overlap"
    ];
    private static bool N(string? s,int low,int high,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=low&&n<=high;
    public static Review SourceTapeABCD(string? a,string? b,string? c,string? d)
    {
        bool ok=N(a,0,500,out var x)&&x==58&&N(b,0,500,out var y)&&y==72&&
            N(c,0,500,out var z)&&z==44&&N(d,0,500,out var w)&&w==180;
        return new(ok,true,ok?"Exact teacher-prepared tape A58,B72,C44,D180 cm; teacher still checks real measured strips."
            :"Tape model lengths in original preparation are A58,B72,C44,D180 centimeters.");
    }
    public static Review SourceTapeEFG(string? e,string? f,string? g)
    {
        bool ok=N(e,0,500,out var x)&&x==336&&N(f,0,500,out var y)&&y==190&&
            N(g,0,500,out var z)&&z==282;
        return new(ok,true,ok?"Exact source meterstick floor tape E336,F190,G282cm. Teacher observes physical measuring."
            :"3m36cm=336,1m90cm=190,2m82cm=282.");
    }
    public static Review DInMeters(string? m,string? cm)
    {
        var ok=N(m,0,5,out var a)&&a==1&&N(cm,0,99,out var b)&&b==80;
        return new(ok,true,ok?"D=180cm=1m80cm (approximately2m, not exactly2m). Teacher checks actual two tools."
            :"100cm=1m so180cm=1m80cm.");
    }
    public static Review Independent(string? a,string? b,string? c)
    {
        bool ok=N(a,0,500,out var x)&&x==247&&N(b,0,99,out var y)&&y==65&&
            N(c,0,500,out var z)&&z==412;
        return new(ok,false,ok?"Independent maths2m47cm=247cm,365cm=3m65cm (remainder65),4m12cm=412cm."
            :"Use100cm per meter for first-party2m47cm,365cm remaining after3m,4m12cm.");
    }
}
