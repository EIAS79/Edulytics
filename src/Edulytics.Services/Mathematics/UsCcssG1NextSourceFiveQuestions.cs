using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original 2021 IM/Kendall Hunt G1 grade positions 36–40: Unit 2 L21–22,
/// Unit 3 L01–03. Checks source-specific mathematical actions, not generic
/// outcome-only fallback. Physical partner centers, fluency timing and
/// unrestricted survey/story discussion remain teacher-observed.
/// </summary>
public static class UsCcssG1NextSourceFiveQuestions
{
    public const string Puzzle="usccss.g1.u02.l21.number_puzzles.distinct_cards";
    public const string Missing="usccss.g1.u02.l21.related_missing_values";
    public const string Survey="usccss.g1.u02.l22.survey.three_equations";
    public const string Questions="usccss.g1.u02.l22.survey.compare_question";
    public const string Sums="usccss.g1.u03.l01.sums.fluency_selfcheck";
    public const string Favorite="usccss.g1.u03.l01.sums.favorite_and_reason";
    public const string CountOn="usccss.g1.u03.l02.count_on.start_steps_total";
    public const string Commute="usccss.g1.u03.l02.commutativity.two_equations";
    public const string Equal="usccss.g1.u03.l03.expressions.compare_both_sides";
    public const string Sort="usccss.g1.u03.l03.expressions.sort_equal_sums";

    private static readonly string[] Families=
        [Puzzle,Missing,Survey,Questions,Sums,Favorite,CountOn,Commute,Equal,Sort];
    private static readonly string[][] SurveyTopics=
    [
        ["apples","pears","plums"],
        ["cats","dogs","fish"],
        ["red","blue","green"]
    ];
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is not null && Families.Contains(f,StringComparer.Ordinal);
    private static string Compact(string s)=>string.Concat(s.Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family)) throw new InvalidOperationException("Unknown G1-008 source family");
        int a=random.Next(2,6), b=random.Next(1,Math.Min(11-a,6));
        int c=random.Next(1,Math.Min(11-a-b,5));
        int mode=random.Next(3);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode
        };
        string prompt;
        switch(family)
        {
            case Puzzle:
            {
                var first=random.Next(1,5);
                var second=random.Next(1,5);
                if(first==second) second=second==4?3:second+1;
                a=random.Next(2,6); b=first; c=second;
                mode=random.Next(2);
                p["a"]=a;p["b"]=b;p["c"]=c;p["mode"]=mode;
                prompt=mode==0
                    ?$"Number Puzzles: use digit cards {b} and {c} ONCE each. Fill the two blanks: {a} + □ = {a+b}; {a+c} - □ = {a}. Write FIRST=number;SECOND=number. Do not reuse one card."
                    :$"Number Puzzles: use digit cards {b} and {c} ONCE each. Fill the two blanks: □ + {a} = {a+b}; {a+c} - □ = {a}. Write FIRST=number;SECOND=number. Do not reuse one card.";
                break;
            }
            case Missing:
                prompt=$"Number Talk: {a} + □ = {a+b} and {a+b} - □ = {a}. The same part is missing. Write FIRST=number;SECOND=number, and explain the related addition/subtraction fact.";
                break;
            case Survey:
            {
                a=random.Next(1,4);b=random.Next(1,4);c=random.Next(1,4);
                p["a"]=a;p["b"]=b;p["c"]=c;
                var labels=SurveyTopics[mode];
                prompt=$"SAMPLE classroom survey (not a real pupil survey): {labels[0]}={a}, {labels[1]}={b}, {labels[2]}={c}. " +
                    "WRITE THREE DIFFERENT complete equations about these category counts and totals. " +
                    "You may combine one or two categories, find the entire total, or use related subtraction. " +
                    "Separate the three equations with semicolons. Unrelated facts do not count.";
                break;
            }
            case Questions:
            {
                a=random.Next(3,6);b=random.Next(1,a);
                p["a"]=a;p["b"]=b;
                var labels=SurveyTopics[mode];
                var question=mode==0
                    ?$"How many more {labels[0]} than {labels[1]}?"
                    :$"How many fewer {labels[1]} than {labels[0]}?";
                prompt=$"SAMPLE survey: {a} children chose {labels[0]}; {b} chose {labels[1]}. " +
                    $"WRITE a comparison question: {question} " +
                    $"Use QUESTION={question};EQUATION=number-number=number. " +
                    "Explain your choice of quantities to a teacher.";
                break;
            }
            case Sums:
                prompt=$"Sums I've Got: say {a} + {b} mentally and give its sum. Then reflect HONESTLY: " +
                    "could you say it quickly (GOT_IT) or do you need more practice (NOT_YET)? " +
                    "Write SUM=number;SELF=GOT_IT or SUM=number;SELF=NOT_YET. " +
                    "Your reflection is not graded as right or wrong; only the sum is.";
                break;
            case Favorite:
                var other=b>1?b-1:b+1;
                prompt=$"My Favorite Sum: choose the sum you prefer: {a}+{b} OR {a}+{other}. " +
                    "Write the complete equation for YOUR choice. Show a method you could use, " +
                    "with REASON=COUNT_ON for counting on from an addend. " +
                    "Format: EQUATION=number+number=number;REASON=COUNT_ON. " +
                    "Teacher or partner checks original creative explanations.";
                break;
            case CountOn:
                prompt=mode==0
                    ?$"Shake and Spill: {a} red counters and {b} yellow counters. Count ON from {a}, moving {b} steps. Write START=number;STEPS=number;TOTAL=number."
                    :$"Shake and Spill: {a} red counters and {b} yellow counters. Count ON from {b}, moving {a} steps. Write START=number;STEPS=number;TOTAL=number.";
                break;
            case Commute:
                if(a==b){b=b==1?2:b-1;p["b"]=b;}
                prompt=$"Two pupils count the same {a} red and {b} yellow counters, but start from different addends. " +
                    $"Write BOTH matching complete equations, one beginning with {a} and one beginning with {b}. " +
                    "Separate them with a semicolon. Explain to a partner why the total stays the same.";
                break;
            case Equal:
            {
                var otherA=random.Next(1,5);
                var otherB=mode==0?a+b-otherA:Math.Max(1,a+b-otherA+(mode==1?1:-1));
                if(otherB is <1 or >9 || otherA+otherB>10)
                {
                    otherA=a;
                    otherB=mode==0?b:mode==1&&a+b<10?b+1:Math.Max(1,b-1);
                }
                p["c"]=otherA;p["mode"]=mode;
                p["otherB"]=otherB;
                prompt=$"Are the expressions equal? {a}+{b} = {otherA}+{otherB}. " +
                    "Find BOTH values. Write LEFT=number;RIGHT=number;EQUAL=YES or EQUAL=NO.";
                break;
            }
            case Sort:
            {
                // Three expression cards; two and only two have the same value.
                a=random.Next(2,5);b=random.Next(2,5);
                var sum=a+b;
                var pairA=random.Next(1,sum);
                var pairB=sum-pairA;
                var extraA=1;var extraB=sum;
                p["a"]=a;p["b"]=b;p["c"]=pairA;p["pairB"]=pairB;
                p["otherB"]=extraB;
                var cards=mode switch
                {
                    0=>$"A: {a}+{b}, B: {pairA}+{pairB}, C: {extraA}+{extraB}",
                    1=>$"A: {a}+{b}, B: {extraA}+{extraB}, C: {pairA}+{pairB}",
                    _=>$"A: {extraA}+{extraB}, B: {a}+{b}, C: {pairA}+{pairB}"
                };
                prompt=$"Sort expressions by the value of their sum. Cards: {cards}. " +
                    "Exactly two cards have EQUAL sums. Write MATCH=A,B or MATCH=A,C or MATCH=B,C.";
                break;
            }
            default: throw new InvalidOperationException();
        }
        return new Problem(family,prompt,
            "Use only the quantities in the mathematical task; calculate and check the related fact. " +
            "Unrestricted spoken or creative explanations need a teacher.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||
           !p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||
           !p.TryGetValue("mode",out int mode)||
           mode is <0 or >2 || a is <1 or >6 || b is <1 or >6 ||
           c is <1 or >9 || a+b>10)return false;
        bool extras=p.ContainsKey("pairB")||p.ContainsKey("otherB");
        if(family==Equal)
            return p.Count==5&&p.TryGetValue("otherB",out int ob)&&ob is >=1 and <=9&&c+ob<=10;
        if(family==Sort)
            return p.Count==6&&p.TryGetValue("pairB",out int pb)&&
                p.TryGetValue("otherB",out int xb)&&pb>0&&c+pb==a+b&&xb==a+b&&
                1+xb<=10;
        if(extras || p.Count!=4)return false;
        if(family==Puzzle)return b!=c&&a+c<=10&&mode<=1;
        if(family==Survey)return a<=3&&b<=3&&c<=3;
        if(family==Questions)return a>b;
        if(family==Missing)return a+b<=10;
        if(family==CountOn)return mode<=2;
        return true;
    }
    private static string Eq(int a,int b)=>$"{a}+{b}={a+b}";
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1-008 task");
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
        return family switch
        {
            Puzzle=>$"FIRST={b};SECOND={c}",
            Missing=>$"FIRST={b};SECOND={b}",
            Survey=>$"{Eq(a,b)};{Eq(a+b,c)};{a+b+c}-{a}={b+c}",
            Questions=>mode==0
                ?$"QUESTION=How many more {SurveyTopics[mode][0]} than {SurveyTopics[mode][1]}?;EQUATION={a}-{b}={a-b}"
                :$"QUESTION=How many fewer {SurveyTopics[mode][1]} than {SurveyTopics[mode][0]}?;EQUATION={a}-{b}={a-b}",
            Sums=>$"SUM={a+b};SELF=GOT_IT",
            Favorite=>$"EQUATION={Eq(a,b)};REASON=COUNT_ON",
            CountOn=>mode==0?$"START={a};STEPS={b};TOTAL={a+b}":
                $"START={b};STEPS={a};TOTAL={a+b}",
            Commute=>$"{Eq(a,b)};{Eq(b,a)}",
            Equal=>$"LEFT={a+b};RIGHT={c+p["otherB"]};EQUAL={(a+b==c+p["otherB"]?"YES":"NO")}",
            Sort=>$"MATCH={(mode==0?"A,B":mode==1?"A,C":"B,C")}",
            _=>throw new InvalidOperationException()
        };
    }
    private static bool ValidSurveyEquations(int a,int b,int c,string submitted)
    {
        var rows=submitted.Split(';',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries)
            .Select(Compact).ToArray();
        if(rows.Length!=3||rows.Distinct(StringComparer.Ordinal).Count()!=3)return false;
        var parts=new[]{a,b,c};
        var allowed=new HashSet<string>(StringComparer.Ordinal);
        var total=a+b+c;
        for(int i=0;i<3;i++)
        {
            for(int j=i+1;j<3;j++)
            {
                allowed.Add(Compact(Eq(parts[i],parts[j])));
                allowed.Add(Compact(Eq(parts[j],parts[i])));
                var k=3-i-j; // 0+1 -> 2; 0+2 -> 1; 1+2 -> 0
                allowed.Add(Compact($"{parts[i]+parts[j]}+{parts[k]}={total}"));
                allowed.Add(Compact($"{total}-{parts[k]}={parts[i]+parts[j]}"));
            }
        }
        return rows.All(allowed.Contains);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string response)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(response))return false;
        var expected=Solve(family,p);
        if(family==Favorite)return GradePersistedAnswer(family,expected,response);
        if(family==Survey)return ValidSurveyEquations(p["a"],p["b"],p["c"],response);
        if(family==Sums)
        {
            var good=Compact($"SUM={p["a"]+p["b"]};SELF=");
            var normalized=Compact(response);
            return normalized==good+"GOT_IT"||normalized==good+"NOT_YET";
        }
        return Compact(expected)==Compact(response);
    }
    public static bool GradePersistedAnswer(string? family,string expected,string? answer)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==Favorite)
        {
            var match=System.Text.RegularExpressions.Regex.Match(Compact(expected),
                @"^EQUATION=(\d+)\+(\d+)=(\d+);REASON=COUNT_ON$");
            if(!match.Success)return false;
            int a=int.Parse(match.Groups[1].Value,CultureInfo.InvariantCulture);
            int b=int.Parse(match.Groups[2].Value,CultureInfo.InvariantCulture);
            int total=int.Parse(match.Groups[3].Value,CultureInfo.InvariantCulture);
            if(a+b!=total || a is <2 or >5 || b is <1 or >5)return false;
            int other=b>1?b-1:b+1;
            var normalized=Compact(answer);
            return normalized==Compact(expected) ||
                normalized==Compact($"EQUATION={Eq(a,other)};REASON=COUNT_ON");
        }
        if(family==Sums)
        {
            var normalized=Compact(expected);
            var prefix=normalized.EndsWith(";SELF=GOT_IT",StringComparison.Ordinal)
                ?normalized[..^6]:"";
            return prefix.Length>0&&(Compact(answer)==normalized||Compact(answer)==prefix+"NOT_YET");
        }
        if(family==Survey)
        {
            // Persisted task stores the generated reference solution: parse its
            // first two equations to recover the original ordered survey counts.
            var parts=Compact(expected).Split(';');
            if(parts.Length!=3)return false;
            var m=System.Text.RegularExpressions.Regex.Match(parts[0],
                @"^(\d+)\+(\d+)=(\d+)$");
            var n=System.Text.RegularExpressions.Regex.Match(parts[1],
                @"^(\d+)\+(\d+)=(\d+)$");
            if(!m.Success||!n.Success)return false;
            int a=int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture),
                b=int.Parse(m.Groups[2].Value,CultureInfo.InvariantCulture),
                ab=int.Parse(m.Groups[3].Value,CultureInfo.InvariantCulture),
                c=int.Parse(n.Groups[2].Value,CultureInfo.InvariantCulture);
            return ab==a+b&&ValidSurveyEquations(a,b,c,answer);
        }
        return Compact(expected)==Compact(answer);
    }
}
