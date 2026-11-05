using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 5 2021 U01 L02: 158 distinct connected 8-12 cube arrangements
/// with uneven rows, rather than twelve repeated scalar examples.
/// Partner "Guess My Prism" shape reconstruction is teacher-reviewed.
/// </summary>
internal static class UsCcssG5Unit1Lesson2Questions
{
    public const string Family="usccss.g5.u01.l02.count_irregular_connected_cubes";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly IReadOnlyList<(int Bottom,int Middle,int Top,int Offset)> Candidates=BuildCandidates();
    private static IReadOnlyList<(int Bottom,int Middle,int Top,int Offset)> BuildCandidates()
    {
        var result=new List<(int Bottom,int Middle,int Top,int Offset)>();
        for(var bottom=5;bottom<=8;bottom++)
        for(var middle=2;middle<=bottom;middle++)
        for(var top=0;top<=middle;top++)
        for(var offset=0;offset<=bottom-middle;offset++)
            if(bottom+middle+top is >=8 and <=12)
                result.Add((bottom,middle,top,offset));
        return result;
    }
    public static bool Supports(string? family)=>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G5 L02");
        ArgumentNullException.ThrowIfNull(random);
        var (bottom,middle,top,offset)=Candidates[random.Next(Candidates.Count)];
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["bottom"]=bottom,["middle"]=middle,["top"]=top,["offset"]=offset};
        var third=top==0?"No third row is needed.":
            $"The third row has {top} cubes stacked on the leftmost part of the middle row.";
        var prompt=$"An irregular solid has a bottom row of {bottom} connected unit cubes. "+
            $"A row of {middle} congruent cubes sits directly on top beginning at "+
            $"bottom cube {offset+1}; each cube shares a face with a cube below. "+
            third+" Every cube is included only once without overlaps. "+
            "How many unit cubes are in the connected solid, including hidden cubes? "+
            "Explain the count and reconstruct the model for teacher review.";
        var solution="Add each nonoverlapping row exactly once. "+
            "The partner's model reconstruction and explanation need teacher assessment.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid irregular cube rows");
        return (p["bottom"]+p["middle"]+p["top"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)
    {
        if(!Supports(family)||p is null||p.Count!=4||
            !p.TryGetValue("bottom",out var bottom)||!p.TryGetValue("middle",out var middle)||
            !p.TryGetValue("top",out var top)||!p.TryGetValue("offset",out var offset))return false;
        return bottom is >=5 and <=8&&middle>=2&&middle<=bottom&&
            top>=0&&top<=middle&&offset>=0&&offset<=bottom-middle&&
            bottom+middle+top is >=8 and <=12;
    }
}
