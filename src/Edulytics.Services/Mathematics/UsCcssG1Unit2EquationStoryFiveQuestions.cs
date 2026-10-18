using System.Globalization;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM K–5 2021 Grade 1 Unit 2 lessons 17–20 (grade ordinals 32–35).
/// Lesson 16 reuses the existing genuine Capture Squares subtraction and mental
/// subtraction Practice instead of duplicating a center generator.
/// Open-ended partner work and fully free-form story authorship require a
/// teacher: automated checks for lesson 20 cover ONLY a structured story scaffold.
/// </summary>
public static class UsCcssG1Unit2EquationStoryFiveQuestions
{
    public const string Compare="usccss.g1.u02.story.compare_structure_and_equation";
    public const string Box="usccss.g1.u02.story.match_equation_with_unknown_box";
    public const string Two="usccss.g1.u02.story.two_matching_equations";
    public const string Create="usccss.g1.u02.story.compose_scaffold_from_equation";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Compare or Box or Two or Create;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G1 U02 L17–L20");
        var a=random.Next(2,7);
        var b=random.Next(2,Math.Min(11-a,7));
        var sum=a+b;
        var mode=family==Compare||family==Box||family==Create?random.Next(3):0;
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["a"]=a,["b"]=b,["sum"]=sum,["mode"]=mode};
        var prompt=family switch
        {
            Compare=>mode switch
            {
                0=>$"Story A: {a} children play outside. {b} more join. How many children are playing NOW? "+
                    "What type of unknown is this? Write TYPE=TOTAL and one completed equation. "+
                    "Format: TYPE=TOTAL; EQUATION=2+3=5 (use YOUR numbers).",
                1=>$"Story B: {sum} children play outside. {a} came first and the rest joined later. "+
                    "How many joined later? Write TYPE=CHANGE and one complete matching equation. "+
                    "Format: TYPE=CHANGE; EQUATION=5-2=3 (use YOUR numbers).",
                _=>$"Story C: {sum} children were playing. {b} left. How many children remain? "+
                    "Write TYPE=RESULT and one complete matching equation. "+
                    "Format: TYPE=RESULT; EQUATION=5-3=2 (use YOUR numbers)."
            },
            Box=>mode switch
            {
                0=>$"A story says: {a} counters and some more counters make {sum}. "+
                    $"Complete {a} + □ = {sum}. Replace the □ with the correct number. "+
                    "Write the FULL equation including =.",
                1=>$"A story says: some counters and {b} counters make {sum}. "+
                    $"Complete □ + {b} = {sum}. Replace the □ with the correct number. "+
                    "Write the FULL equation including =.",
                _=>$"A story says: {sum} counters, and {a} are hidden. "+
                    $"Complete {sum} - □ = {b}. Replace □ and write the FULL equation."
            },
            Two=>$"A Lotería board has {sum} spaces. {a} are covered by called cards; "+
                $"{b} are uncovered. Write TWO DIFFERENT, complete and true equations "+
                "that match the SAME story (addition, subtraction, changing the addend order, "+
                "or placing the total to the LEFT of '=' are valid). "+
                "Separate equations with a semicolon, not just two answers.",
            Create=>mode switch
            {
                0=>$"An equation is {a} + {b} = {sum}. COMPOSE a TWO-SENTENCE joining story "+
                     "that fits it, by filling this student-authored story scaffold: "+
                     $"STORY=I had {a} shells. I got {b} more shells and now I have {sum} shells. "+
                     "Rewrite with your own classroom objects if you want; keep the numbers, "+
                     "joining action and complete sentences.",
                1=>$"An equation is {sum} - {b} = {a}. COMPOSE a TWO-SENTENCE taking-away story "+
                     "that fits it. Use this scaffold, or another matching object: "+
                     $"STORY=I had {sum} shells. I gave away {b} shells and now I have {a} shells. "+
                     "Change the objects if desired. The teacher can check more creative stories.",
                _=>$"An equation is {a} + □ = {sum}. COMPOSE a TWO-SENTENCE joining story "+
                     "with a missing part. Use this scaffold, or another matching object: "+
                     $"STORY=I had {a} shells. I got {b} more shells and now I have {sum} shells. "+
                     "Write your own nouns if desired. A teacher checks fully original stories."
            },
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
            "Match the same quantities, unknown position and story structure. "+
            "For free-form explanations or stories, also ask the teacher to review the meaning.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("sum",out int s)||
           !p.TryGetValue("mode",out int mode))return false;
        return a is >=2 and <=6 && b is >=2 and <=6 && s==a+b&&s<=10 &&
            mode is >=0 and <=2 && (family!=Two||mode==0);
    }
    private static string N(int n)=>n.ToString(CultureInfo.InvariantCulture);
    private static string Eq(int a,int b,int s)=>$"{a}+{b}={s}";
    private static string Sub(int s,int b,int a)=>$"{s}-{b}={a}";
    private static string Compact(string s)=>string.Concat(s.Where(c=>!char.IsWhiteSpace(c)));

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 story parameters");
        int a=p["a"],b=p["b"],s=p["sum"],m=p["mode"];
        return family switch
        {
            Compare=>m switch
            {
                0=>$"TYPE=TOTAL;EQUATION={Eq(a,b,s)}",
                1=>$"TYPE=CHANGE;EQUATION={Sub(s,a,b)}",
                _=>$"TYPE=RESULT;EQUATION={Sub(s,b,a)}"
            },
            Box=>m switch
            {
                0=>Eq(a,b,s),1=>Eq(b,a,s),_=>Sub(s,a,b)
            },
            Two=>$"{Eq(a,b,s)};{Sub(s,a,b)}",
            Create=>m==1
                ?$"STORY=I had {s} shells. I gave away {b} shells and now I have {a} shells."
                :$"STORY=I had {a} shells. I got {b} more shells and now I have {s} shells.",
            _=>throw new InvalidOperationException()
        };
    }

    private static IReadOnlySet<string> AllowedEquations(int a,int b,int s)
    {
        var set=new HashSet<string>(StringComparer.Ordinal);
        foreach(var (left,right) in new[]{
            ($"{a}+{b}",N(s)),($"{b}+{a}",N(s)),
            ($"{s}-{a}",N(b)),($"{s}-{b}",N(a))})
        {
            set.Add(left+"="+right);
            set.Add(right+"="+left);
        }
        return set;
    }
    private static bool EquationCorrect(string raw,int a,int b,int s)=>
        AllowedEquations(a,b,s).Contains(Compact(raw));

    private static bool GradeStoryScaffold(int a,int b,int s,int m,string response)
    {
        // Intentionally bounded: written stories outside the scaffold need
        // teacher review; they are not falsely AI-certified.
        var text=Regex.Replace(response.Trim(),@"\s+"," ",RegexOptions.CultureInvariant);
        var mt=Regex.Match(text,
            @"^STORY\s*=\s*I had (\d+) ([A-Za-z]+)\.\s*I (got|received|found|gave away|lost|used) (\d+) (?:more )?([A-Za-z]+) and now I have (\d+) ([A-Za-z]+)\.$",
            RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        if(!mt.Success)return false;
        var nums=new[]{mt.Groups[1].Value,mt.Groups[4].Value,mt.Groups[6].Value};
        if(!nums.All(x=>int.TryParse(x,NumberStyles.None,CultureInfo.InvariantCulture,out _)))return false;
        var before=int.Parse(nums[0],CultureInfo.InvariantCulture);
        var change=int.Parse(nums[1],CultureInfo.InvariantCulture);
        var after=int.Parse(nums[2],CultureInfo.InvariantCulture);
        var noun=mt.Groups[2].Value;
        if(!string.Equals(noun,mt.Groups[5].Value,StringComparison.OrdinalIgnoreCase)||
           !string.Equals(noun,mt.Groups[7].Value,StringComparison.OrdinalIgnoreCase))return false;
        var action=mt.Groups[3].Value.ToLowerInvariant();
        return m==1
            ? before==s&&change==b&&after==a&&
                action is "gave away" or "lost" or "used"
            : before==a&&change==b&&after==s&&
                action is "got" or "received" or "found";
    }
    public static bool GradePersistedAnswer(string? family,string expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(response))return false;
        if(family==Create)
        {
            var canonical=Regex.Match(expected,
                @"^STORY=I had (\d+) shells\. I (?:got|gave away) (\d+) (?:more )?shells and now I have (\d+) shells\.$",
                RegexOptions.CultureInvariant);
            if(!canonical.Success)return false;
            int a=int.Parse(canonical.Groups[1].Value,CultureInfo.InvariantCulture);
            int b=int.Parse(canonical.Groups[2].Value,CultureInfo.InvariantCulture);
            int s=int.Parse(canonical.Groups[3].Value,CultureInfo.InvariantCulture);
            return expected.Contains("gave away",StringComparison.Ordinal)
                ? GradeStoryScaffold(s,b,a,1,response)
                : GradeStoryScaffold(a,b,s,0,response);
        }
        if(family==Compare)
        {
            var match=Regex.Match(Compact(expected),
                @"^TYPE=(TOTAL|CHANGE|RESULT);EQUATION=(\d+)([+-])(\d+)=(\d+)$");
            var submission=Regex.Match(Compact(response),
                @"^TYPE=(TOTAL|CHANGE|RESULT);EQUATION=(.+)$",RegexOptions.IgnoreCase);
            if(!match.Success||!submission.Success||
               !string.Equals(match.Groups[1].Value,submission.Groups[1].Value,
                   StringComparison.OrdinalIgnoreCase))return false;
            int l=int.Parse(match.Groups[2].Value,CultureInfo.InvariantCulture);
            int r=int.Parse(match.Groups[4].Value,CultureInfo.InvariantCulture);
            int w=int.Parse(match.Groups[5].Value,CultureInfo.InvariantCulture);
            // Accept only the two original story quantities/answer, not an
            // unrelated arithmetic fact with the same result.
            return match.Groups[1].Value switch
            {
                "TOTAL"=>EquationCorrect(submission.Groups[2].Value,l,r,w),
                "CHANGE"=>EquationCorrect(submission.Groups[2].Value,w,r,l),
                _=>EquationCorrect(submission.Groups[2].Value,w,r,l)
            };
        }
        if(family==Box)
        {
            var e=Compact(expected);
            var m=Regex.Match(e,@"^(\d+)([+-])(\d+)=(\d+)$");
            if(!m.Success)return false;
            int l=int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture);
            int r=int.Parse(m.Groups[3].Value,CultureInfo.InvariantCulture);
            int w=int.Parse(m.Groups[4].Value,CultureInfo.InvariantCulture);
            return Compact(response)==e||Compact(response)==w+"="+l+m.Groups[2].Value+r;
        }
        if(family==Two)
        {
            var match=Regex.Match(Compact(expected),@"^(\d+)\+(\d+)=(\d+);");
            if(!match.Success)return false;
            int a=int.Parse(match.Groups[1].Value,CultureInfo.InvariantCulture);
            int b=int.Parse(match.Groups[2].Value,CultureInfo.InvariantCulture);
            int s=int.Parse(match.Groups[3].Value,CultureInfo.InvariantCulture);
            var lines=response.Split(';',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries)
                .Select(Compact).ToArray();
            return lines.Length==2&&lines[0]!=lines[1]&&
                lines.All(line=>EquationCorrect(line,a,b,s));
        }
        return false;
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string response)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family,p),response);
}
