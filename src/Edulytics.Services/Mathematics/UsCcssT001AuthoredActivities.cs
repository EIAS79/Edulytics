using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Narrow Grade 1 T001 student-authored Practice tasks.
/// L02 requires writing the expression, L07 requires independently assigning
/// EVERY object to a labeled category, and L09 requires vote entry plus a
/// constructed representation. The L09 verifier proves internal consistency,
/// NOT that voters were independent humans (teacher review is still required).
/// These generated exercises do not award official mastery or academic gates.
/// </summary>
internal static class UsCcssT001AuthoredActivities
{
    public const string Expression = "usccss.t001.g1.write_dot_expression";
    public const string ToolSorting = "usccss.t001.g1.construct_tool_sort";
    public const string Survey = "usccss.t001.g1.collect_survey_graph";
    public const string SurveySolverSentinel = "survey-entry-protocol-v1";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) => family is Expression or ToolSorting or Survey;

    public static Problem Build(string family, Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown T001 authored family.");
        ArgumentNullException.ThrowIfNull(rng);
        if (family == Expression)
        {
            var a = rng.Next(1, 6);
            var b = rng.Next(1, Math.Min(5, 10-a)+1);
            return new Problem(family,
                "Dot cards: "+new string('●',a)+" | "+new string('●',b)+
                ". Build YOUR OWN complete addition expression. Write both addends and the total.",
                "The first addend counts the left card, the second counts the right card; the total counts both.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>(StringComparer.Ordinal) { ["left"]=a,["right"]=b });
        }

        if (family == ToolSorting)
        {
            var size = rng.Next(7, 10);
            var categories = new List<int> { 0, 1, 2 };
            while (categories.Count < size) categories.Add(rng.Next(0,3));
            for (int i=categories.Count-1; i>0; i--)
            {
                var j=rng.Next(i+1);
                (categories[i],categories[j])=(categories[j],categories[i]);
            }
            var labels = new[] { "counter", "pattern block", "inch tile" };
            var parameters = new Dictionary<string,int>(StringComparer.Ordinal) { ["length"]=size };
            for (int i=0;i<size;i++) parameters["item"+i.ToString(CultureInfo.InvariantCulture)]=categories[i];
            return new Problem(family,
                "Items: "+string.Join(", ",categories.Select(x=>labels[x]))+
                ". Sort EVERY tool card by its kind into your OWN three labeled groups.",
                "Group every card exactly once by tool type. Explain your grouping rule to a teacher.",
                AssessmentItemType.ShortAnswer,parameters);
        }

        return new Problem(family,
            "REAL CLASS SURVEY: Ask 5 to 10 participants which fruit they prefer: "+
            "apple, banana or orange. Record each vote once WITHOUT names, "+
            "then build your OWN three-column chart. A teacher must verify "+
            "that actual people were surveyed; the automatic check only "+
            "checks the entered votes against the graph.",
            "Your recorded anonymous votes and chart counts must agree. "+
            "Teacher verification is required for evidence of real participant collection.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>(StringComparer.Ordinal)
            { ["minimumVotes"]=5,["maximumVotes"]=10 });
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Supports(family)) return false;
        if (family == Expression)
            return p.TryGetValue("left",out var a) &&
                p.TryGetValue("right",out var b) &&
                a is >=1 and <=5 && b is >=1 and <=5 && a+b<=10;
        if (family == ToolSorting)
        {
            if (!p.TryGetValue("length",out var count) || count is <7 or >9)
                return false;
            var seen=new int[3];
            for(var i=0;i<count;i++)
            {
                if(!p.TryGetValue("item"+i.ToString(CultureInfo.InvariantCulture),out var kind) ||
                    kind is <0 or >2) return false;
                seen[kind]++;
            }
            return seen.All(x=>x>0);
        }
        return p.TryGetValue("minimumVotes",out var minimum) &&
            p.TryGetValue("maximumVotes",out var maximum) &&
            minimum==5 && maximum==10;
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new InvalidOperationException("Invalid T001 authored question parameters.");
        if (family == Expression)
            return $"{p["left"]}+{p["right"]}={p["left"]+p["right"]}";
        if (family == ToolSorting)
            return string.Join(",",Enumerable.Range(0,p["length"]).Select(i=>
                p["item"+i.ToString(CultureInfo.InvariantCulture)].ToString(CultureInfo.InvariantCulture)));
        // This sentinel is for internal engine self-verification only. It is
        // deliberately rejected as a STUDENT answer.
        return SurveySolverSentinel;
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Valid(family,p) &&
        (string.Equals(answer,Solve(family,p),StringComparison.Ordinal) ||
         (family==Survey && GradeStudentAnswer(family,p,answer)));

    public static bool GradeStudentAnswer(
        string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family,p) || string.IsNullOrWhiteSpace(answer))
            return false;
        if (family != Survey)
            return string.Equals(answer.Trim(),Solve(family,p),StringComparison.Ordinal);

        // Format v:01210|g:2,2,1; digits are anonymous response choices
        // in order, and graph columns are category 0/1/2. No names, IPs,
        // emails, or pupil identifiers are collected.
        var parts=answer.Trim().Split('|');
        if(parts.Length!=2 || !parts[0].StartsWith("v:",StringComparison.Ordinal) ||
            !parts[1].StartsWith("g:",StringComparison.Ordinal)) return false;
        var votes=parts[0].AsSpan(2);
        if(votes.Length<p["minimumVotes"] || votes.Length>p["maximumVotes"])
            return false;
        var collected=new int[3];
        foreach(var digit in votes)
        {
            if(digit is <'0' or >'2') return false;
            collected[digit-'0']++;
        }
        var plotted=parts[1][2..].Split(',');
        if(plotted.Length!=3)return false;
        for(int i=0;i<3;i++)
        {
            if(plotted[i].Length==0 || plotted[i].Length>2 ||
                !plotted[i].All(ch=>ch is >= '0' and <= '9') ||
                !int.TryParse(plotted[i],NumberStyles.None,CultureInfo.InvariantCulture,out var n) ||
                n!=collected[i] ||
                !string.Equals(plotted[i],n.ToString(CultureInfo.InvariantCulture),
                    StringComparison.Ordinal)) return false;
        }
        return true;
    }
}
