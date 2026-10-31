using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 G5 U01 L12 OPTIONAL: 60 shipping containers in multiple
/// distinct prism arrangements. Tonnage cannot determine a ship's exact capacity.
/// 54 possible ordered factor triples provide sufficiently diverse exact items;
/// choosing five genuinely noncongruent cuboids is teacher-scored.</summary>
internal static class UsCcssG5Unit1Lesson12Questions
{
    public const string Family="usccss.g5.u01.l12.sixty_container_factor_arrangements";
    private static readonly IReadOnlyList<(int Width,int Depth,int Layers)> Arrangements=BuildArrangements();
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    private static IReadOnlyList<(int Width,int Depth,int Layers)> BuildArrangements()
    {
        var triples=new List<(int Width,int Depth,int Layers)>();
        for(var width=1;width<=60;width++)
        for(var depth=1;depth<=60/width;depth++)
        {
            var baseCount=width*depth;
            if(60%baseCount==0)triples.Add((width,depth,60/baseCount));
        }
        return triples;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 L12");
        ArgumentNullException.ThrowIfNull(random);
        var (width,depth,layers)=Arrangements[random.Next(Arrangements.Count)];
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["width"]=width,["depth"]=depth};
        var prompt=$"The original optional lesson explores arrangements of exactly "+
            $"60 identical shipping containers. Arrange {width} containers across "+
            $"and {depth} rows in each complete layer. How many complete layers "+
            "are needed for exactly 60 containers? Then design five distinct "+
            "prism arrangements and justify your ship-space estimate to a teacher. "+
            "A mass of 3,300 tons does NOT give the ship's capacity in containers.";
        var solution="Original optional activity: display at least five distinct 60-container solid arrangements; evaluate stacking, ship loading, estimates and assumptions with teacher. The 3,300 tons is mass, not a direct cubic-capacity measurement.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad 60-container factors");
        return (60/(p["width"]*p["depth"])).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>
        Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)
    {
        if(!Supports(family)||p is null||p.Count!=2||
            !p.TryGetValue("width",out var width)||!p.TryGetValue("depth",out var depth))
            return false;
        return width is >=1 and <=60&&depth is >=1 and <=60&&
            width*depth<=60&&60%(width*depth)==0;
    }
}
