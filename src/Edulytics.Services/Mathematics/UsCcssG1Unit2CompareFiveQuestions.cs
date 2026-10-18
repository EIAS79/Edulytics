using System.Globalization;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-aligned first-party practice: IM K-5 first-edition 2021 G1 U2 L11-L15.
/// All generated quantities are within ten. Physical cube-building, class data
/// collection, center choice and partner discussion require a real teacher.
/// </summary>
public static class UsCcssG1Unit2CompareFiveQuestions
{
    public const string Towers="usccss.g1.u02.compare.equal_cubes_write_equation";
    public const string Supplies="usccss.g1.u02.compare.school_supplies_difference";
    public const string Data="usccss.g1.u02.compare.art_supply_data_difference";
    public const string Related="usccss.g1.u02.compare.write_related_equations";
    public const string Mixed="usccss.g1.u02.compare.mixed_story_equation";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family)=>
        family is Towers or Supplies or Data or Related or Mixed;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G1 U02 L11-L15 family");
        int small=random.Next(2,8);
        int big=random.Next(small+1,11);
        int other=random.Next(1,8);
        int mode=family==Mixed||family==Data?random.Next(3):0;
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["small"]=small,["big"]=big,["other"]=other,["mode"]=mode
        };
        string question=family switch
        {
            Towers=>$"A blue cube tower has {big} cubes and a yellow tower has {small} cubes. "+
                "How many cubes must be ADDED to the shorter tower OR REMOVED from the taller tower to make the towers the same height? "+
                "Write one complete addition or subtraction equation showing the DIFFERENCE. "+
                "You may also show your cube-matching strategy to your teacher.",
            Supplies=>$"There are {big} notebooks for a class, but only {small} pencils. "+
                "How many more pencils are needed for each notebook to have a pencil? "+
                "Write one full addition or subtraction equation that matches the comparison.",
            Data=>$"An imaginary classroom chart shows favorite art supplies: "+
                $"crayons {big}, markers {small}, paint {other}. "+
                (mode==0
                    ?"How many MORE votes did crayons receive than markers? "+
                     "Match their two category counts and write one full addition or subtraction equation. "
                    :$"Statement: There were {big-small+(mode==2?1:0)} MORE crayon votes than marker votes. "+
                     "Decide whether the statement matches the data. Write TRUE or FALSE. ")+
                "These are sample data, NOT a real class survey.",
            Related=>$"A tower has {big} cubes and another has {small} cubes. "+
                "Find the DIFFERENCE. Write BOTH a completed addition equation "+
                "and its matching subtraction equation about the SAME towers. "+
                "Separate your two equations with a semicolon. Do not just enter a number.",
            Mixed=>mode switch
            {
                0=>$"Story A (Put Together, Total Unknown): {small} red blocks and {big-small} blue blocks "+
                    "are on a table. How many blocks in all? Write a full equation.",
                1=>$"Story B (One Part Unknown): There are {big} pattern blocks in all. "+
                    $"{small} are triangles and the others are squares. "+
                    "How many squares are there? Write a complete matching equation.",
                _=>$"Story C (Compare, Difference Unknown): "+
                    $"One child has {big} beads and another has {small}. "+
                    "How many MORE beads does the first child have? Write a complete matching equation."
            },
            _=>throw new InvalidOperationException()
        };
        string solution="Match the smaller collection with an equal number from the larger "+
            "collection; unmatched objects give the difference. "+
            "The related equations describe the same whole and parts.";
        return new(family,question,solution,AssessmentItemType.ShortAnswer,p);
    }

    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("small",out var small)||
           !p.TryGetValue("big",out var big)||
           !p.TryGetValue("other",out var other)||
           !p.TryGetValue("mode",out var mode))return false;
        return small is >=2 and <=7 &&
            big>small&&big<=10&&other is >=1 and <=7 &&
            mode is >=0 and <=2&&(family==Mixed||family==Data||mode==0);
    }

    private static string Add(int a,int b,int t)=>$"{a}+{b}={t}";
    private static string Sub(int t,int a,int b)=>$"{t}-{a}={b}";
    private static string Compact(string s)=>string.Concat(s.Where(c=>!char.IsWhiteSpace(c)));
    private static string[] Parts(string s)=>s.Split(';',
        StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries)
        .Select(Compact).ToArray();

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade 1 comparison parameters");
        int small=p["small"],big=p["big"],difference=big-small;
        if(family==Data&&p["mode"]!=0)
            return p["mode"]==1?"TRUE":"FALSE";
        return family==Related
            ?Add(small,difference,big)+";"+Sub(big,small,difference)
            :Add(small,difference,big);
    }

    public static bool GradePersistedAnswer(string? family,string expected,string? response)
    {
        if(!Supports(family)||response is null)return false;
        if(family==Data && expected is ("TRUE" or "FALSE"))
            return string.Equals(response.Trim(),expected,StringComparison.OrdinalIgnoreCase);
        var canonical=Parts(expected);
        var parts=Parts(response);
        if(canonical.Length is <1 or >2||parts.Length<1||parts.Length>2)return false;
        var m=Regex.Match(canonical[0],@"^(\d+)\+(\d+)=(\d+)$");
        if(!m.Success)return false;
        var smaller=int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture);
        var diff=int.Parse(m.Groups[2].Value,CultureInfo.InvariantCulture);
        var whole=int.Parse(m.Groups[3].Value,CultureInfo.InvariantCulture);
        if(smaller<2||diff<1||whole>10||smaller+diff!=whole)return false;
        var addition=Add(smaller,diff,whole);
        var swapped=Add(diff,smaller,whole);
        var subtraction=Sub(whole,smaller,diff);
        var allowed=new HashSet<string>(StringComparer.Ordinal)
            {addition,swapped,subtraction};
        if(family==Related)
            return parts.Length==2 &&
                parts.Distinct(StringComparer.Ordinal).Count()==2 &&
                (parts[0]==addition||parts[0]==swapped||parts[1]==addition||parts[1]==swapped) &&
                parts.Contains(subtraction,StringComparer.Ordinal) &&
                parts.All(allowed.Contains);
        return parts.Length==1&&allowed.Contains(parts[0]);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string response)=>
        Valid(family,parameters)&&
        GradePersistedAnswer(family,Solve(family,parameters),response);
}
