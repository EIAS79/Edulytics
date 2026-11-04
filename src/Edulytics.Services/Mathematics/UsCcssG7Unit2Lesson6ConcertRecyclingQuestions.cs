using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// OUR/IM Grade7 Unit2 Lesson6. Exact source-scoped revenue, inverse count,
/// recycled mass/cans/value ratios. A completed contextual explanation,
/// integral ticket inventory and model drawing require teacher review.
/// Uses the existing ExactSkillContractQuestionEngine (not a new engine).
/// </summary>
internal static class UsCcssG7Unit2Lesson6ConcertRecyclingQuestions
{
    public const string Family="usccss.g7.u02.l06.concert_recycling_equations";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported L06 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["mode"]=rng.Next(0,8),["feature"]=rng.Next(0,4)
        };
        int mode=p["mode"],f=p["feature"];
        string q=mode switch
        {
            0 => "An original ticket model expects 5,000 tickets to yield $311,000. "+
                 "What is the exact price per ticket in dollars?",
            1 => "Tickets cost the same each. 5,000 tickets yield $311,000. "+
                 "What dollar revenue comes from "+new[]{"7000","10000","50000","120000"}[f]+
                 " tickets? Write revenue R=kt in your explanation.",
            2 => "Each ticket costs $311,000/5,000. "+
                 "What is the theoretical real-valued ticket count for dollar revenue "+
                 new[]{"404300","5000000","311000","622"}[f]+
                 "? Enter the exact rational value. Actual tickets must be whole; "+
                 "discuss feasibility and rounding upward with a teacher.",
            3 => "Ten aluminum cans weigh 0.16 kilograms. "+
                 "How many cans have mass "+new[]{"0.16","2.4","3.2","0.48"}[f]+
                 " kilograms at the same unit mass?",
            4 => "Ten recycled cans earn $0.14. What dollar value corresponds to "+
                 new[]{"10","50","150","200"}[f]+" cans?",
            5 => "Ten recycled cans weigh 0.16 kg and earn $0.14. "+
                 "At the same rate, how many dollars correspond to "+
                 new[]{"0.16","2.4","3.2","0.48"}[f]+" kg of cans?",
            6 => "A car travels at constant 65 miles/hour. "+
                 (f==0?"How many miles in 1.5 hours?":
                  f==1?"How many hours for 26 miles?":
                  f==2?"How many miles in 3 hours?":
                       "How many minutes for 65 miles?"),
            _ => "Each water bottle holds 17 fluid ounces. "+
                 (f==0?"How many ounces are in 51 bottles?":
                  f==1?"How many bottles hold 51 fluid ounces?":
                  f==2?"How many ounces are in 3 bottles?":
                       "How many bottles hold 102 fluid ounces?")
        };
        return new(Family,q+" Respond with an exact nonnegative number or fraction.",
            "Set y=kx for the stated quantities. Solve y by multiplication "+
            "or x by division. Contextual whole-item feasibility, units and "+
            "a written equation explanation must be checked by an educator.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid L06 parameters.");
        var (n,d)=Expected(p);var g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family, p))
            return false;
        var (n, d) = Expected(p);
        return G7ExactRationalAnswer.Matches(answer, n, d, 100000000);
    }

    private static (long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int mode=p["mode"],f=p["feature"];
        long[] ticketCounts=[7000,10000,50000,120000];
        long[] targets=[404300,5000000,311000,622];
        long[] massHundredths=[16,240,320,48];
        long[] recycleCans=[10,50,150,200];
        return mode switch
        {
            0 => (311,5),
            1 => (311*ticketCounts[f],5),
            2 => (5*targets[f],311),
            3 => (massHundredths[f]*10,16),
            4 => (recycleCans[f]*14,1000),
            5 => (massHundredths[f]*14,1600),
            6 => f switch
            {
                0=>(195,2),1=>(2,5),2=>(195,1),_=>(60,1)
            },
            _ => f switch
            {
                0=>(867,1),1=>(3,1),2=>(51,1),_=>(6,1)
            }
        };
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long t=a%b;a=b;b=t;}return a;}
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is not null&&p.Count==2&&
        p.TryGetValue("mode",out int m)&&m is >=0 and <=7&&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3;
}
