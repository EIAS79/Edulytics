using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-goal-scoped Grade 1 new lessons U03 L19–L23 (ordinals 56–60).
/// Automated numeric portions only; classroom partner strategies, cards,
/// How Close? and Five in a Row require real teacher facilitation.
/// </summary>
public static class UsCcssG1FullGradeNextQuestions
{
    public const string FlexibleAdd="usccss.g1.u03.l19.flexible.make_ten";
    public const string ZooThree="usccss.g1.u03.l20.zoo.three_addends";
    public const string CenterTwoThree="usccss.g1.u03.l21.center.two_three_addends";
    public const string TeenSubtract="usccss.g1.u03.l22.teen.subtract_cross_ten";
    public const string TenSubtract="usccss.g1.u03.l23.ten.subtract_in_steps";
    public static readonly IReadOnlyList<string> Families=[FlexibleAdd,ZooThree,CenterTwoThree,TeenSubtract,TenSubtract];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Families.Contains(family,StringComparer.Ordinal);
    private static string Normalize(string? value)=>
        string.Concat((value??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
           !p.TryGetValue("c",out var c)||!p.TryGetValue("mode",out var mode)||
           mode<0||mode>3)return false;
        return family switch
        {
            FlexibleAdd=>a>=6&&a<=9&&b>=11-a&&b<=10&&c==0,
            ZooThree=>a>=1&&a<=8&&b>=1&&b<=8&&c>=1&&c<=8&&a+b+c<=20&&
                (mode!=2||(a==8&&b==7&&c==5)),
            CenterTwoThree=>a>=1&&a<=8&&b>=1&&b<=8&&
                (mode%2==0?c==0:c>=1&&c<=8)&&a+b+c<=20,
            TeenSubtract or TenSubtract=>a>=11&&a<=18&&b>=a-9&&b<=9&&c==0,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1 L56–60 Practice family");
        int mode=random.Next(4),a=0,b=0,c=0;
        if(family==FlexibleAdd)
        {
            a=random.Next(6,10);b=random.Next(11-a,11);
        }
        else if(family==ZooThree)
        {
            if(mode==2){a=8;b=7;c=5;} // Actual 2021 student reptile story.
            else
            {
                a=random.Next(1,9);b=random.Next(1,9);
                c=random.Next(1,Math.Min(8,20-a-b)+1);
            }
        }
        else if(family==CenterTwoThree)
        {
            a=random.Next(1,9);b=random.Next(1,9);
            c=mode%2==0?0:random.Next(1,Math.Min(8,20-a-b)+1);
        }
        else
        {
            a=random.Next(11,19);b=random.Next(a-9,10);
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 L56–60 parameters");
        string prompt=family switch
        {
            FlexibleAdd=>$"Compare addition methods for {a}+{b} within 20. Make a ten by moving counters from the second addend. Write MOVE=number;REST=number;TOTAL=number.",
            ZooThree=>mode==2?
                $"ORIGINAL 2021 STUDENT STORY: Jada sees 8 snakes, 7 iguanas and 5 frogs at the reptile exhibit. How many reptiles? Count all THREE groups and write the matching equation. Write TOTAL=number;EQUATION=number+number+number=number.":
                mode%2==0?
                $"At a zoo, there are {a} zebras, {b} penguins and {c} monkeys. All three groups count. Write TOTAL=number;EQUATION=number+number+number=number.":
                $"At a zoo there are {a} lions, {b} birds and {c} seals. Use ANY correct addition method for all three groups. Write TOTAL=number;EQUATION=number+number+number=number.",
            CenterTwoThree=>mode%2==0
                ?$"How Close? Add the TWO number cards {a} and {b} within 20. Write TOTAL=number;EQUATION=number+number=number."
                :$"Five in a Row: add ALL THREE card numbers {a}, {b} and {c}. Write TOTAL=number;EQUATION=number+number+number=number.",
            TeenSubtract=>$"Take From: start with {a} counters and remove {b}. To cross ten, remove the ones first, then part of a ten. Write REACH10=number;REST=number;DIFFERENCE=number.",
            TenSubtract=>$"Use ten as a landmark for {a}-{b}. First subtract enough to get to ten, then subtract the rest. Write TO10=number;REST=number;DIFFERENCE=number.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
            "Represent the quantities with counters or ten-frames and check equivalent computations. Teacher-led center participation is not digitally certified.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Unreviewed or impossible math instance");
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
        return family switch
        {
            FlexibleAdd=>$"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
            ZooThree=>$"TOTAL={a+b+c};EQUATION={a}+{b}+{c}={a+b+c}",
            CenterTwoThree=>mode%2==0
                ?$"TOTAL={a+b};EQUATION={a}+{b}={a+b}"
                :$"TOTAL={a+b+c};EQUATION={a}+{b}+{c}={a+b+c}",
            TeenSubtract=>$"REACH10={a-10};REST={b-(a-10)};DIFFERENCE={a-b}",
            TenSubtract=>$"TO10={a-10};REST={b-(a-10)};DIFFERENCE={a-b}",
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))
            return false;
        var e=Normalize(expected).Split(';');var r=Normalize(response).Split(';');
        if(e.Length!=r.Length)return false;
        var left=new Dictionary<string,string>(StringComparer.Ordinal);
        var right=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (parts,map) in new[]{(e,left),(r,right)})
            foreach(var part in parts)
            {
                int pos=part.IndexOf('=');
                if(pos<=0||pos==part.Length-1||!map.TryAdd(part[..pos],part[(pos+1)..]))
                    return false;
            }
        if(left.Count!=right.Count)return false;
        return left.All(item=>right.TryGetValue(item.Key,out var value)&&value==item.Value);
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),response);
}
