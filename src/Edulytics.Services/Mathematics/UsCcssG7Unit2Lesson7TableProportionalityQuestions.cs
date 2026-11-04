using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 Unit 2 L07: decide whether observed table values COULD be
/// proportional; inconsistent quotients prove they CANNOT be. Constant
/// quotients alone never certify an unobserved whole relationship.
/// Uses shared lesson-scoped exact generator, not a new engine.
/// </summary>
internal static class UsCcssG7Unit2Lesson7TableProportionalityQuestions
{
    public const string Family="usccss.g7.u02.l07.table_proportional_or_not";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Problem Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G7 L07 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=rng.Next(0,8),["feature"]=rng.Next(0,4)};
        int k=p["kind"],i=p["feature"];
        string q=k switch
        {
            0 => "An original lemonade mix has 5 lemons, 2 cups water and 2 tbsp honey. "+
                 "A proposed batch has "+
                 new[]{"10 lemons,4 cups water,4 tbsp honey",
                       "2.5 lemons,1 cup water,1 tbsp honey",
                       "10 lemons,2 cups water,2 tbsp honey",
                       "3 lemons,2 cups water,2 tbsp honey"}[i]+
                 ". Is this a proportional scaling of ALL the ingredients? Enter 1 yes or 0 no.",
            1 => "A park charges a $6 fixed vehicle fee plus $2 for each person. "+
                 "What is the total dollar charge for "+
                 new[]{"2","4","10","50"}[i]+" people?",
            2 => "Decide whether the price-vs-number-of-people relationship COULD be proportional "+
                 "for ALL nonnegative counts when the total is "+
                 (i%2==0?"$6 plus $2 per person":"$2 per person, with no fixed fee")+
                 ". Enter 1 if it could be proportional, or 0 if it cannot.",
            3 => "A runner's completed-lap/time table (laps,time in minutes) is "+
                 new[]{"(2,4),(4,9),(6,15),(8,23)",
                       "(2,5),(4,10),(6,15),(8,20)",
                       "(2,6),(4,12),(6,18),(8,24)",
                       "(2,5),(4,10),(6,15),(8,21)"}[i]+
                 ". Could these measured pairs follow ONE constant pace? "+
                 "Enter 1 for consistent with proportionality or 0 for inconsistent.",
            4 => "An observed input-output table is "+
                 new[]{"smoothie ounces/price: (8,6),(12,9),(16,12)",
                       "smoothie ounces/price: (8,6),(12,8),(16,10)",
                       "listener distance/sound level: (5,85),(10,79),(20,73)",
                       "drink ounces/price: (16,1.49),(20,1.59),(30,1.89)"}[i]+
                 ". Are all shown pairs consistent with ONE constant output/input quotient? "+
                 "Enter 1 for yes (could be proportional), 0 for no (cannot).",
            5 => "A taxi charges $1 for the first tenth of a mile and $0.10 for each "+
                 "additional tenth. What is the total charge in dollars for "+
                 new[]{"0.9","2","3.1","10"}[i]+" miles? "+
                 "A nonzero first-distance charge changes the average rate.",
            6 => "One animal's observed distance/time table (meters,minutes) is "+
                 new[]{"(108,2),(405,7.5),(540,10),(1768.5,32.75)",
                       "(800,1),(900,5),(1107.5,20),(1524,32.5)",
                       "(120,2),(240,4),(360,6),(480,8)",
                       "(120,2),(240,4),(360,6),(481,8)"}[i]+
                 ". Are these pairs consistent with a single constant speed? "+
                 "Enter 1 consistent or 0 inconsistent.",
            _ => "A table of output b versus input a has pairs "+
                 new[]{"(2,14),(5,35),(9,63)",
                       "(3,360),(5,600),(8,960)",
                       "(75,3),(200,8),(1525,61)",
                       "(4,10),(6,15),(22,55)"}[i]+
                 ". Find the exact constant b/a as a number or fraction."
        };
        return new(Family,q+" Give only the exact response; your teacher assesses "+
            "qualitative taste comparisons and the reasons for your classification.",
            "A constant quotient across observed nonzero inputs is necessary for "+
            "proportionality. One unequal ratio rules it out. Matching finite "+
            "table ratios only establishes consistency, not a universal law.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G7 L07 parameters");
        var(n,d)=Expected(p);var g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family, p))
            return false;
        var (n, d) = Expected(p);
        return G7ExactRationalAnswer.Matches(answer, n, d, 1000000);
    }

    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],i=p["feature"];
        return k switch
        {
            0 => (i is 0 or 1 ? 1 : 0,1),
            1 => (new long[]{10,14,26,106}[i],1),
            2 => (i%2==0?0:1,1),
            3 => (i is 1 or 2?1:0,1),
            4 => (i==0?1:0,1),
            5 => (new long[]{18,29,40,109}[i],10),
            6 => (i is 0 or 2?1:0,1),
            _ => i switch
            {
                0=>(7,1),1=>(120,1),2=>(1,25),_=>(5,2)
            }
        };
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long t=a%b;a=b;b=t;}return a;}
    private static bool Valid(string? f,IReadOnlyDictionary<string,int>? p)=>
        Supports(f)&&p is not null&&p.Count==2&&
        p.TryGetValue("kind",out int k)&&k is >=0 and <=7&&
        p.TryGetValue("feature",out int i)&&i is >=0 and <=3;
}
