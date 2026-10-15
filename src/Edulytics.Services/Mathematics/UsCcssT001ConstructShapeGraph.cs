using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Constructed, three-category shape graph for T001 Grade 1 Unit 1 Lesson 8.
/// Server-owned mathematics. Student responses are count vectors, never answer-choice indices.
/// </summary>
internal static class UsCcssT001ConstructShapeGraph
{
    public const string Family = "usccss.t001.g1.construct_shape_graph";
    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string family) => string.Equals(family, Family, StringComparison.Ordinal);
    public static Problem Build(Random rng)
    {
        ArgumentNullException.ThrowIfNull(rng);
        int a,b,c;
        do { a=rng.Next(1,6); b=rng.Next(1,6); c=rng.Next(1,6); }
        while(a+b+c>10);
        var cards=Enumerable.Repeat("▲",a)
            .Concat(Enumerable.Repeat("■",b))
            .Concat(Enumerable.Repeat("●",c)).ToArray();
        for(int i=cards.Length-1;i>0;i--)
        {
            int j=rng.Next(i+1);
            (cards[i],cards[j])=(cards[j],cards[i]);
        }
        return new Problem(Family,
            "Sort these shape cards. Cards: "+string.Join(", ",cards)+
            ". Build your OWN graph with three labeled columns: Triangles, Squares, Circles. "+
            "Use one mark for each card, then check your drawing.",
            "Count each triangle, square and circle once. Each graph mark represents one card.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>(StringComparer.Ordinal)
            { ["first"]=a,["second"]=b,["third"]=c });
    }
    private static bool Valid(IReadOnlyDictionary<string,int> p) =>
        p.TryGetValue("first",out var a) && p.TryGetValue("second",out var b) &&
        p.TryGetValue("third",out var c) &&
        a is >=1 and <=5 && b is >=1 and <=5 && c is >=1 and <=5 && a+b+c<=10;
    public static string Solve(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p)) throw new InvalidOperationException("Invalid L08 shape graph parameters.");
        return string.Join(",", new[]{p["first"],p["second"],p["third"]}
            .Select(x=>x.ToString(CultureInfo.InvariantCulture)));
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Supports(family) && Valid(p) && string.Equals(answer,Solve(p),StringComparison.Ordinal);
}
