using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Original IM/Kendall Hunt 2021 G1 U03 L09–L13, V1 G1-010 only.
/// Auditable source-grounded mathematics and fail-closed numeric feedback.
/// Classroom partner explanation and free-form drawings require teachers.</summary>
public static class UsCcssG1TeenFiveQuestions
{
    public const string TenFrame="usccss.g1.u03.l09.ten.add_frame";
    public const string TenMissing="usccss.g1.u03.l09.ten.missing_equation";
    public const string TeenStories="usccss.g1.u03.l10.teen.story_unknown";
    public const string TeenRelations="usccss.g1.u03.l10.teen.related_equations";
    public const string AddRockStory="usccss.g1.u03.l11.teen.add_story";
    public const string TeenAddMissing="usccss.g1.u03.l11.teen.add_missing";
    public const string SubtractCollection="usccss.g1.u03.l12.teen.take_from";
    public const string TeenEquations="usccss.g1.u03.l12.teen.equations_missing";
    public const string ResultStory="usccss.g1.u03.l13.teen.result_unknown";
    public const string ChangeStory="usccss.g1.u03.l13.teen.change_unknown";
    public static readonly IReadOnlyList<string> Families=[
        TenFrame,TenMissing,TeenStories,TeenRelations,AddRockStory,
        TeenAddMissing,SubtractCollection,TeenEquations,ResultStory,ChangeStory
    ];
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>
        family is not null&&Families.Contains(family,StringComparer.Ordinal);
    public static string Normalize(string? answer)=>
        string.Concat((answer??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||!p.TryGetValue("a",out var a)||
           !p.TryGetValue("b",out var b)||!p.TryGetValue("c",out var c)||
           !p.TryGetValue("mode",out var mode)||a<0||a>20||b<0||b>20||
           c<0||c>3||mode<0||mode>3)return false;
        return family switch
        {
            TenFrame or TenMissing=>a is >=11 and <=20,
            TeenRelations=>a is >=11 and <=19,
            TeenStories=>a is >=11 and <=19 && b is >=1 and <=10 && b<a,
            AddRockStory or TeenAddMissing=>a is >=11 and <=19 && b is >=1 and <=9 && a+b<=20,
            SubtractCollection=>a is >=11 and <=19 && b is >=1 and <=9 && a-b>=10,
            TeenEquations=>a is >=11 and <=19 && b is >=1 and <=9 && a-b>=10,
            ResultStory=>a is >=11 and <=20 && b is >=1 and <=9 && b<a,
            ChangeStory=>a is >=11 and <=20 && b is >=1 and <=19 && b<a,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1-010 family");
        int a=random.Next(11,20),b=1,c=random.Next(4),mode=random.Next(4);
        string prompt;
        switch(family)
        {
            case TenFrame:
                a=random.Next(11,21);
                prompt=mode%2==0
                   ?$"Build the number-card {a} using TWO ten-frames. Fill the first ten-frame and decide what belongs in the second. How many complete TENS and leftover ONES? Write TEN=number;ONES=number;EQUATION=number+number=number. Explain your model."
                   :$"The double ten-frame should represent {a} counters. Ten already fill the first frame. Find the missing number of counters on the second frame, the number of FULL tens, and the loose ones. Write TEN=number;ONES=number;EQUATION=number+number=number.";
                break;
            case TenMissing:
                a=random.Next(11,21);
                prompt=mode switch
                {
                    0=>$"Find the missing value: {a}=10+□. Write MISSING=number;CHECK=10+number=number.",
                    1=>$"Find the missing value: 10+{a-10}=□. Write MISSING=number;CHECK=10+number=number.",
                    2=>$"Find the missing value: {a}=□+{a-10}. Write MISSING=number;CHECK=10+number=number.",
                    _=>$"Find the missing value: □+{a-10}={a}. Write MISSING=number;CHECK=10+number=number."
                };
                break;
            case TeenStories:
                a=random.Next(11,20);b=random.Next(1,11);
                prompt=mode%2==0
                  ?$"Kiran has {b} baseball caps. After his birthday, he has {a} caps. How many MORE caps did he get? Write UNKNOWN=number;ADDITION=number+number=number;SUBTRACTION=number-number=number. Explain how both equations match the story."
                  :$"Priya receives {b} comic books. She now has {a} comic books. How many did she have AT THE START? Write UNKNOWN=number;ADDITION=number+number=number;SUBTRACTION=number-number=number. Explain how both equations match the story.";
                break;
            case TeenRelations:
                a=random.Next(11,20);
                prompt=mode switch
                {
                    0=>$"Use one ten and some ones: {a}-□=10. Write MISSING=number;RELATED=number+number=number.",
                    1=>$"Use one ten and some ones: {a}-10=□. Write MISSING=number;RELATED=number+number=number.",
                    2=>$"Find the missing number in □+{a-10}={a}. Write MISSING=number;RELATED=number+number=number.",
                    _=>$"Find the missing number in {a}=10+□. Write MISSING=number;RELATED=number+number=number."
                };
                break;
            case AddRockStory:
                a=random.Next(11,20);b=random.Next(1,21-a);
                prompt=mode%2==0
                 ?$"Kiran has {a} rocks and finds {b} more on a hike. How many rocks does he have? Combine the ones; if ten ones are made, show a SECOND group of ten. Write TOTAL=number;EQUATION=number+number=number."
                 :$"A child collects {a} stones and then {b} additional stones. Find the new total using a full ten-frame and ones. Write TOTAL=number;EQUATION=number+number=number.";
                break;
            case TeenAddMissing:
                a=random.Next(11,20);b=random.Next(1,21-a);
                prompt=mode switch
                {
                    0=>$"Find the missing total in {a}+{b}=□. Write MISSING=number;EQUATION=number+number=number.",
                    1=>$"Find the missing total in □={b}+{a}. Write MISSING=number;EQUATION=number+number=number.",
                    2=>$"Use the teen-number structure: {a}+□={a+b}. Write MISSING=number;EQUATION=number+number=number.",
                    _=>$"Use a ten and some ones: {a+b}=□+{a}. Write MISSING=number;EQUATION=number+number=number."
                };
                break;
            case SubtractCollection:
                a=random.Next(11,20);b=random.Next(1,a-9);
                prompt=mode%2==0
                 ?$"Noah has {a} game pieces and takes {b} out. How many remain? Show subtraction without breaking the bundle of ten. Write REMAIN=number;EQUATION=number-number=number."
                 :$"A box contains {a} counters. A child removes {b} counters. How many counters remain? Keep the ten intact. Write REMAIN=number;EQUATION=number-number=number.";
                break;
            case TeenEquations:
                // IM 2021 L12 Activity 12.2: solve teen-number addition
                // AND subtraction equations for the unknown in every
                // position, keeping one full ten without regrouping.
                a=random.Next(11,20);b=random.Next(1,a-9);
                prompt=mode switch
                {
                    0=>$"Addition and Subtraction Equations with Teen Numbers: solve {a}-{b}=□. Write MISSING=number;CHECK=number+number=number.",
                    1=>$"Addition and Subtraction Equations with Teen Numbers: solve {a}-□={a-b}. Write MISSING=number;CHECK=number+number=number.",
                    2=>$"Addition and Subtraction Equations with Teen Numbers: solve {a-b}+{b}=□. Write MISSING=number;CHECK=number+number=number.",
                    _=>$"Addition and Subtraction Equations with Teen Numbers: solve □+{b}={a}. Write MISSING=number;CHECK=number+number=number."
                };
                break;
            case ResultStory:
                a=random.Next(11,21);b=random.Next(1,10);
                prompt=mode%2==0
                 ?$"There were {a} students standing. {b} students sat down. How many are still standing? This is Take From, RESULT Unknown. Write REMAIN=number;EQUATION=number-number=number."
                 :$"There were {a} children in a room; {b} left. How many are still in the room? Write REMAIN=number;EQUATION=number-number=number.";
                break;
            case ChangeStory:
                a=random.Next(11,21);b=random.Next(1,a);
                prompt=mode%2==0
                 ?$"There were {a} students standing. Some sat down and now {b} are standing. How many SAT DOWN? This is Take From, CHANGE Unknown. Write LEFT=number;EQUATION=number-number=number."
                 :$"There were {a} children in the room. Some went home and {b} are still here. How many went home? Write LEFT=number;EQUATION=number-number=number. Explain how the story differs from result-unknown.";
                break;
            default:throw new InvalidOperationException();
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1-010 problem parameters");
        return new Problem(family,prompt,"Show source-number relationships with a full ten-frame and matching equation; partner explanations are teacher-observed.",AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1-010 source task");
        int a=p["a"],b=p["b"],mode=p["mode"];
        return family switch
        {
            TenFrame=>a==20?$"TEN=2;ONES=0;EQUATION=10+10=20":$"TEN=1;ONES={a-10};EQUATION=10+{a-10}={a}",
            TenMissing=>$"MISSING={(mode is 0 ? a-10 : mode is 1 ? a : 10)};CHECK=10+{a-10}={a}",
            TeenStories=>mode%2==0
                ?$"UNKNOWN={a-b};ADDITION={b}+{a-b}={a};SUBTRACTION={a}-{b}={a-b}"
                :$"UNKNOWN={a-b};ADDITION={a-b}+{b}={a};SUBTRACTION={a}-{b}={a-b}",
            TeenRelations=>$"MISSING={(mode is 0 or 1 or 3 ? a-10:10)};RELATED=10+{a-10}={a}",
            AddRockStory=>$"TOTAL={a+b};EQUATION={a}+{b}={a+b}",
            TeenAddMissing=>$"MISSING={(mode is 0 or 1?a+b:b)};EQUATION={a}+{b}={a+b}",
            SubtractCollection=>$"REMAIN={a-b};EQUATION={a}-{b}={a-b}",
            TeenEquations=>$"MISSING={(mode==1?b:mode==2?a:a-b)};CHECK={a-b}+{b}={a}",
            ResultStory=>$"REMAIN={a-b};EQUATION={a}-{b}={a-b}",
            ChangeStory=>$"LEFT={a-b};EQUATION={a}-{a-b}={b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static bool EquivalentAddition(string x,string y)
    {
        var e=System.Text.RegularExpressions.Regex.Match(x,@"^(\d+)\+(\d+)=(\d+)$");
        var a=System.Text.RegularExpressions.Regex.Match(y,@"^(\d+)\+(\d+)=(\d+)$");
        if(!e.Success||!a.Success)return false;
        int[] n=[int.Parse(e.Groups[1].Value),int.Parse(e.Groups[2].Value),int.Parse(e.Groups[3].Value)];
        int[] m=[int.Parse(a.Groups[1].Value),int.Parse(a.Groups[2].Value),int.Parse(a.Groups[3].Value)];
        return n[0]+n[1]==n[2]&&m[0]+m[1]==m[2]&&n[2]==m[2]&&
               ((n[0]==m[0]&&n[1]==m[1])||(n[0]==m[1]&&n[1]==m[0]));
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||
           string.IsNullOrWhiteSpace(response))return false;
        var left=Normalize(expected).Split(';');
        var right=Normalize(response).Split(';');
        if(left.Length!=right.Length)return false;
        var e=new Dictionary<string,string>(StringComparer.Ordinal);
        var a=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (items,map) in new[]{(left,e),(right,a)})
            foreach(var part in items)
            {
                int eq=part.IndexOf('=');
                if(eq<=0||eq==part.Length-1||!map.TryAdd(part[..eq],part[(eq+1)..]))
                    return false;
            }
        if(e.Count!=a.Count||e.Keys.Except(a.Keys,StringComparer.Ordinal).Any())return false;
        foreach(var (key,value) in e)
        {
            if(value==a[key])continue;
            // Commutativity is valid ONLY when the two exact source addends
            // are unchanged, not because some unrelated equation is true.
            if(key is "EQUATION" or "ADDITION" or "RELATED" or "CHECK" &&
               EquivalentAddition(value,a[key]))continue;
            return false;
        }
        return true;
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),answer);
}
