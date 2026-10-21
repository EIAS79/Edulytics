using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-specific numeric subskills: one-symbol picture representation,
/// unit-one bar height and checking whether two independently made graphs
/// preserve all counts of the same table. Does not auto-grade drawings.
/// </summary>
internal static class UsCcssG2Unit1Lesson10ConstructGraphsQuestions
{
    public const string Symbols="usccss.g2.u01.l10.picture_symbol_count";
    public const string Bar="usccss.g2.u01.l10.bar_height_for_category";
    public const string Same="usccss.g2.u01.l10.picture_bar_same_data";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Symbols or Bar or Same;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G2 L10 graph family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        for(var i=0;i<4;i++)p[$"count{i}"]=random.Next(0,11);
        var src="Edulytics table of four school subject choices: " +
            $"Art {p["count0"]}, Music {p["count1"]}, " +
            $"Science {p["count2"]}, Sports {p["count3"]}. ";
        string prompt,solution;
        if(family==Symbols || family==Bar)
        {
            p["target"]=random.Next(4);
            var category=new[]{"Art","Music","Science","Sports"}[p["target"]];
            prompt=src+(family==Symbols
                ?$"Make a picture graph with key ONE symbol = ONE child. How many symbols must be drawn for {category}?"
                :$"Create a vertical bar graph with numbered scale zero upward by ONES. To which tick should the {category} bar reach?");
            solution="Read the assigned table and preserve this category's exact count in the new drawing.";
        }
        else
        {
            // Compare one four-category picture graph with a bar graph.
            // Some questions differ in exactly one category; do not
            // compare just a single count or assume they always match.
            var change=random.Next(2)==0;
            for(var i=0;i<4;i++)p[$"bar{i}"]=p[$"count{i}"];
            if(change)
            {
                var index=random.Next(4);
                p[$"bar{index}"]=p[$"bar{index}"]==10?9:p[$"bar{index}"]+1;
            }
            prompt=src+"A PICTURE graph uses this table's counts, one symbol per child. " +
                $"A BAR graph shows Art {p["bar0"]}, Music {p["bar1"]}, " +
                $"Science {p["bar2"]}, Sports {p["bar3"]}, with unit-one scale. " +
                "Do BOTH graphs represent precisely the SAME four category totals? Enter 1 for YES or 0 for NO.";
            solution="Match EVERY bar height to the same-category picture-symbol count before deciding.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("G2 L10 invalid graph source parameters");
        var value=family==Same
            ?(Enumerable.Range(0,4).All(i=>p[$"bar{i}"]==p[$"count{i}"])?1:0)
            :p[$"count{p["target"]}"];
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=(family==Same?8:5))return false;
        for(var i=0;i<4;i++)
        {
            if(!p.TryGetValue($"count{i}",out var n)||n is <0 or >10)return false;
            if(family==Same &&
                (!p.TryGetValue($"bar{i}",out var bar)||bar is <0 or >10))
                return false;
        }
        return family==Same || p.TryGetValue("target",out var index) && index is >=0 and <=3;
    }
}
