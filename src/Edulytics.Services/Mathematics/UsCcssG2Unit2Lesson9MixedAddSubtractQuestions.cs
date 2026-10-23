using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party variants distinguish four source-requested arithmetic categories.</summary>
internal static class UsCcssG2Unit2Lesson9MixedAddSubtractQuestions
{
    public const string AddCompose="usccss.g2.u02.l09.add_compose_ten";
    public const string AddSimple="usccss.g2.u02.l09.add_no_compose";
    public const string SubBorrow="usccss.g2.u02.l09.subtract_decompose_ten";
    public const string SubSimple="usccss.g2.u02.l09.subtract_no_decompose";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is AddCompose or AddSimple or SubBorrow or SubSimple;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L09 exact arithmetic");
        int aTens,bTens,aOnes,bOnes;
        if(family is AddCompose or AddSimple)
        {
            aTens=r.Next(1,7);bTens=r.Next(1,10-aTens-(family==AddCompose?1:0));
            aOnes=family==AddCompose?r.Next(1,10):r.Next(0,10);
            bOnes=family==AddCompose?r.Next(10-aOnes,10):r.Next(0,10-aOnes);
        }
        else
        {
            aTens=r.Next(3,10);bTens=r.Next(1,aTens);
            aOnes=family==SubBorrow?r.Next(0,9):r.Next(0,10);
            bOnes=family==SubBorrow?r.Next(aOnes+1,10):r.Next(0,aOnes+1);
        }
        int a=10*aTens+aOnes,b=10*bTens+bOnes;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"a",a},{"b",b},{"aTens",aTens},{"bTens",bTens},{"aOnes",aOnes},{"bOnes",bOnes}};
        bool add=family is AddCompose or AddSimple;
        string prompt=$"Original Edulytics "+(add?"addition":"subtraction")+" maths variant: {a}"+(add?"+":"−")+b+
            "=? Use tens-and-ones "+(family==AddCompose?"and COMPOSE a ten.":family==AddSimple?"without making a new ten.":family==SubBorrow?"and DECOMPOSE one ten.":"without exchanging a ten.");
        int value=add?a+b:a-b;
        return new Problem(family,prompt,$"{a}"+(add?"+":"−")+b+$"={value}. Explain in tens and ones.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U02 L09 exact problem");
        bool add=family is AddCompose or AddSimple;
        return (add?p["a"]+p["b"]:p["a"]-p["b"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=6||
           !p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
           !p.TryGetValue("aTens",out var at)||at is <1 or >9||
           !p.TryGetValue("bTens",out var bt)||bt is <1 or >9||
           !p.TryGetValue("aOnes",out var ao)||ao is <0 or >9||
           !p.TryGetValue("bOnes",out var bo)||bo is <0 or >9||
           a!=10*at+ao||b!=10*bt+bo)return false;
        int ones=ao+bo;
        return family switch {
            AddCompose=>ones>=10&&a+b<=100,
            AddSimple=>ones<10&&a+b<=100,
            SubBorrow=>at>bt&&ao<bo&&a>b,
            SubSimple=>at>bt&&ao>=bo&&a>b,
            _=>false
        };
    }
}
