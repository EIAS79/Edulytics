using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// New Grade 1 ordinals 51–55: original 2021 source-goal mathematics.
/// Digital Practice does not claim to replace classroom centers and dialog.
/// </summary>
public static class UsCcssG1FullGradeStartQuestions
{
    public const string Center = "usccss.g1.u03.l14.center.add_subtract";
    public const string ThreeStory = "usccss.g1.u03.l15.story.three_addends_ten";
    public const string Expression = "usccss.g1.u03.l16.three_addends.equivalent";
    public const string MakeTen = "usccss.g1.u03.l17.make_ten.decompose";
    public const string Pattern = "usccss.g1.u03.l18.addition.pattern";
    public static readonly IReadOnlyList<string> Families = [Center,ThreeStory,Expression,MakeTen,Pattern];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is not null && Families.Contains(family,StringComparer.Ordinal);
    private static string Norm(string? s) =>
        string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if (!Supports(family)||p.Count!=4||
            !p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
            !p.TryGetValue("c",out var c)||!p.TryGetValue("mode",out var mode)||
            mode<0||mode>3) return false;
        return family switch
        {
            Center => a>=11&&a<=20&&b>=1&&b<=9&&b<a&&
                (mode==0?a+b<=20:true)&&
                (mode<=1?c==0:(c>=1&&c<=9&&c<a&&c!=b)),
            ThreeStory or Expression => a>=1&&a<=9&&b==10-a&&c>=1&&c<=10&&
                (family!=ThreeStory||mode!=2||(a==7&&b==3&&c==8)),
            MakeTen or Pattern => a>=6&&a<=9&&b>10-a&&b<=10&&a+b<=20&&c==0&&
                (family!=MakeTen||mode!=2||a==9),
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed Grade 1 full-grade family");
        int mode=random.Next(4),a=0,b=0,c=0;
        if(family==Center)
        {
            // In the original Grade 1 lesson 14, BOTH addends must total at most 20.
            // Preserve the existing subtraction / distinct-card modes unchanged.
            a=mode==0?random.Next(11,20):random.Next(11,21);
            b=mode==0?random.Next(1,Math.Min(9,20-a)+1):random.Next(1,10);
            if(mode>=2)
                do{c=random.Next(1,10);}while(c==b);
        }
        else if(family==ThreeStory||family==Expression){
            if(family==ThreeStory&&mode==2){a=7;b=3;c=8;} // Original 2021 three bird groups: 7 blue, 8 brown, 3 babies.
            else {a=random.Next(1,10);b=10-a;c=random.Next(1,11);}
        }
        else {a=family==MakeTen&&mode==2?9:random.Next(6,10);b=random.Next(11-a,11);}
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Generated out-of-scope mathematics");
        string prompt=family switch
        {
            Center=>mode<=1?(mode==0?
                $"Number Puzzles: add {a} + {b}. Write ANSWER=number.":
                $"Number Puzzles: subtract {a} - {b}. Write ANSWER=number."):
                $"Number Puzzles: use TWO DIFFERENT digit cards, each at most ONCE. Fill □ + {a-b} = {a} and {a} - □ = {a-c}. Write CARDS=first_digit,second_digit. Do not repeat a card.",
            ThreeStory=>mode==2?
                "ORIGINAL 2021 STUDENT FACING: 7 blue birds fly in the sky, 8 brown birds sit in a tree, and 3 baby birds sit in a nest. How many altogether? Show all THREE groups and write TOTAL=number;EQUATION=number+number+number=number. Discuss your drawing or counters with a teacher.":
                mode%2==0?
                $"Birds arrive in three groups of {a}, {c}, and {b}. Two groups make ten. How many birds are there in all? Write TOTAL=number;EQUATION=number+number+number=number.":
                $"Add three collections of {a}, {b}, and {c} stones. Pair two groups to make ten. Write TOTAL=number;EQUATION=number+number+number=number.",
            Expression=>mode switch
            {
                0=>$"Decide if {a} + {c} + {b} = 10 + {c} is true. Write TRUE=YES/NO;TOTAL=number.",
                1=>$"Decide if {a} + {c} + {b} = 10 + {c-1} is true. Write TRUE=YES/NO;TOTAL=number.",
                2=>$"MATCH the three-addend expression {a} + {c} + {b} to the equivalent expression. A: 10 + {c}. B: 10 + {c-1}. Write MATCH=10+number;TOTAL=number.",
                _=>$"Original student-written task: WRITE a 10+n expression equal to {a} + {c} + {b}; pair {a} and {b} to make ten. Write WRITE=10+number;TOTAL=number."
            },
            MakeTen=>mode==2?
                $"Original 2021 Nine Plus Game: put out NINE counters then pick a {b} card and add that many. Show TWO matching equations: 9+{b}=... and 10+{b-1}=... . Write EQUATION=expression=value;TEN_EQUATION=expression=value;TOTAL=number. Teacher leads physical counters/card activity.":
                $"Complete a ten-frame: {a}+{b} = 10+□. Move counters from the second group. Write MOVE=number;REST=number;TOTAL=number.",
            Pattern=>mode switch
            {
                0=>$"Are {a}+{b} and 10+{a+b-10} equal? Write EQUAL=YES/NO;TOTAL=number.",
                1=>$"Are {a}+{b} and 10+{a+b-9} equal? Write EQUAL=YES/NO;TOTAL=number.",
                2=>$"ORIGINAL EXPRESSION MATCH: Take card {a}+{b}. Match it under the EQUIVALENT ten-plus expression. Card A: 10+{a+b-10}. Card B: 10+{a+b-9}. Write MATCH=10+number;TOTAL=number.",
                _=>$"ORIGINAL ADDITION PATTERN: Start with {a}+{b}, then INCREASE the first addend by 1 and DECREASE the second by 1 so the total stays unchanged. Write the new two-addend PAIR=first+second;TOTAL=number."
            },
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
            "Explain the source-goal number relationship; classroom materials and discussion remain teacher-led.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid full-grade source question");
        var a=p["a"];var b=p["b"];var c=p["c"];var mode=p["mode"];
        return family switch
        {
            Center=>mode<=1?$"ANSWER={(mode==0?a+b:a-b)}":$"CARDS={b},{c}",
            ThreeStory=>$"TOTAL={a+b+c};EQUATION={a}+{b}+{c}={a+b+c}",
            Expression=>mode<=1?$"TRUE={(mode==0?"YES":"NO")};TOTAL={a+b+c}":
                mode==2?$"MATCH=10+{c};TOTAL={a+b+c}":
                $"WRITE=10+{c};TOTAL={a+b+c}",
            MakeTen=>mode==2?
                $"EQUATION=9+{b}={9+b};TEN_EQUATION=10+{b-1}={9+b};TOTAL={9+b}":
                $"MOVE={10-a};REST={a+b-10};TOTAL={a+b}",
            Pattern=>mode<=1?$"EQUAL={(mode==0?"YES":"NO")};TOTAL={a+b}":
                mode==2?$"MATCH=10+{a+b-10};TOTAL={a+b}":
                $"PAIR={a+1}+{b-1};TOTAL={a+b}",
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))
            return false;
        var e=Norm(expected).Split(';');var v=Norm(response).Split(';');
        if(e.Length!=v.Length)return false;
        var em=new Dictionary<string,string>(StringComparer.Ordinal);
        var vm=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (items,map) in new[]{(e,em),(v,vm)})
            foreach(var part in items)
            {
                var index=part.IndexOf('=');
                if(index<=0||index==part.Length-1||
                    !map.TryAdd(part[..index],part[(index+1)..]))return false;
            }
        if(em.Count!=vm.Count)return false;
        return em.All(x=>vm.TryGetValue(x.Key,out var actual)&&actual==x.Value);
    }
    // The 2021 three-addend story explicitly permits commutative/associative
    // solutions. A student's equation may use any ORDER, but it must use
    // exactly the three story quantities, not merely an arbitrary equal sum.
    private static bool VerifyStoryEquation(IReadOnlyDictionary<string,int> p,string? response)
    {
        var parts=Norm(response).Split(';');
        if(parts.Length!=2)return false;
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var part in parts)
        {
            var separator=part.IndexOf('=');
            if(separator<=0||separator==part.Length-1||
                !fields.TryAdd(part[..separator],part[(separator+1)..]))
                return false;
        }
        if(fields.Count!=2||!fields.TryGetValue("TOTAL",out var totalText)||
           !fields.TryGetValue("EQUATION",out var equation)||
           !int.TryParse(totalText,out var total))return false;
        var match=System.Text.RegularExpressions.Regex.Match(
            equation,@"^(\d+)\+(\d+)\+(\d+)=(\d+)$");
        if(!match.Success)return false;
        var values=new int[4];
        for(var i=0;i<4;i++)
            if(!int.TryParse(match.Groups[i+1].Value,out values[i]))return false;
        var expected=new[]{p["a"],p["b"],p["c"]};
        var submitted=new[]{values[0],values[1],values[2]};
        Array.Sort(expected);
        Array.Sort(submitted);
        return expected.SequenceEqual(submitted) &&
            total==p["a"]+p["b"]+p["c"] &&
            values[3]==total;
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p) &&
        (family==ThreeStory?VerifyStoryEquation(p,response):
            GradePersistedAnswer(family,Solve(family!,p),response));
}
