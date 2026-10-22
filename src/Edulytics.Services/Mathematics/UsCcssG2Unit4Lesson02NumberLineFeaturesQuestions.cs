using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Only independent exact numerical tasks; image-specific publisher corrections remain educator-reviewed.</summary>
internal static class UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions
{
    public const string Fives="usccss.g2.u04.l02.count_by_five";
    public const string Card="usccss.g2.u04.l02.yarn_card_position";
    public const string Gap="usccss.g2.u04.l02.equal_interval_gap";
    public const string Missing="usccss.g2.u04.l02.repair_missing_tick";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Fives or Card or Gap or Missing;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G2 U04 L02");
        int end=family switch{Fives=>100,Missing=>10,_=>30};
        int step=family==Fives?5:1;
        int first,second,target;
        if(family==Fives)
        {
            first=random.Next(0,20)*5;
            second=first+5;
            target=second;
        }
        else if(family==Missing)
        {
            target=random.Next(1,10);
            first=target-1;second=target+1;
        }
        else
        {
            first=random.Next(0,end);
            second=random.Next(first+1,end+1);
            target=random.Next(1,end);
        }
        var parameters=new Dictionary<string,int>(StringComparer.Ordinal){
            ["start"]=0,["end"]=end,["step"]=step,
            ["first"]=first,["second"]=second,["target"]=target
        };
        string prompt=family switch{
            Fives=>$"Independently authored Choral Count by5 from zero: after {first}, which number is next? Stay within0..100.",
            Card=>$"First-party abstract equally spaced 0..30 number line: a number card marked {target} is placed correctly. How many ONE-unit spaces from0 to its location?",
            Gap=>$"First-party 0..30 number line has abstract cards at {first} and {second}. What is the count of one-unit intervals BETWEEN the two cards?",
            _=>$"First-party 0..10 equal-unit line has an unlabeled tick after {first} and before {second}. What number belongs on that middle tick? NOT an original Jada/Andre/Elena image."
        };
        int answer=family switch{Fives=>second,Card=>target,Gap=>second-first,_=>target};
        return new(family,prompt,$"Exact independently authored answer {answer}; teacher-only original pictures and class string are not verified.",AssessmentItemType.Numeric,parameters);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid number-line feature parameters");
        int answer=family switch{Fives=>p["second"],Card=>p["target"],Gap=>p["second"]-p["first"],Missing=>p["target"],
            _=>throw new InvalidOperationException("Unsupported feature")};
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=6||
           !p.TryGetValue("start",out var start)||start!=0||
           !p.TryGetValue("end",out var end)||end!=(family switch{Fives=>100,Missing=>10,_=>30})||
           !p.TryGetValue("step",out var step)||step!=(family==Fives?5:1)||
           !p.TryGetValue("first",out var first)||first<0||first>=end||
           !p.TryGetValue("second",out var second)||second<=first||second>end||
           !p.TryGetValue("target",out var target)||target<1||target>end)
            return false;
        if(family==Fives)return first%5==0&&second==first+5&&target==second;
        if(family==Missing)return first==target-1&&second==target+1&&target<10;
        return target<end;
    }
}
