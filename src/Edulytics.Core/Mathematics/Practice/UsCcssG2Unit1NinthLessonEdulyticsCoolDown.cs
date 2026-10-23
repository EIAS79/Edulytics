using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics Grade 2 U01 L09 class bar/picture interpretation.
/// Original 2021 source mathematics includes season bars, while this
/// independently authored Cool-down uses fictional school-club preferences.
/// Publisher graphics and restricted assessment are not reproduced.
/// </summary>
public static class UsCcssG2Unit1NinthLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L09";
    public const string Version="edulytics-g2-u01-l09-original-bar-graph-v1";
    public static readonly IReadOnlyDictionary<string,int> WarmupPets=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Cat"]=6,["Dog"]=10,["Fish"]=5,["Lizard"]=3};

    // Hypothetical new examples, NOT survey results attributed to actual pupils.
    public static readonly IReadOnlyDictionary<string,int> SampleFieldTripClassA=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Aquarium"]=6,["Library"]=2,["Planetarium"]=8,["Park"]=5};
    public static readonly IReadOnlyDictionary<string,int> SampleFieldTripClassB=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Aquarium"]=4,["Library"]=7,["Planetarium"]=3,["Park"]=6};

    public static readonly IReadOnlyDictionary<string,int> SourceSeasonHeights=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Winter"]=7,["Spring"]=3,["Summer"]=13,["Fall"]=3};
    public static readonly IReadOnlyDictionary<string,int> EdulyticsClubHeights=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Robotics"]=4,["Football"]=7,["Art"]=3,["Music"]=6};

    public sealed record Review(bool MachineScorable,bool Correct,bool TeacherReviewNeeded,string Reason);

    public static Review CheckSeasonGraph(string? summer,string? springAndFall,
        string? pairMakingTen,string? allVotes)
    {
        var numeric=new[]{summer,springAndFall,allVotes};
        if(numeric.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,true,"Enter all category counts and the class total.");
        var values=numeric.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var split=(pairMakingTen??"").Split('+',StringSplitOptions.TrimEntries);
        var pairValid=split.Length==2 && split.Distinct(StringComparer.OrdinalIgnoreCase).Count()==2 &&
            split.All(x=>SourceSeasonHeights.ContainsKey(
                CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x.ToLowerInvariant()))) &&
            split.Sum(x=>SourceSeasonHeights[
                CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x.ToLowerInvariant())])==10;
        var correct=values.SequenceEqual(new[]{13,6,26}) && pairValid;
        return new(true,correct,true,correct
            ?"Read summer as 13, spring plus fall as 6, all seasons as 26; more than one pair totals ten."
            :"Read the bar heights using the numbered axis. Two different valid pairs can total ten.");
    }

    public static Review CheckIndependentSchoolClubCoolDown(
        string? football,string? total,string? footballMinusArt)
    {
        var raw=new[]{football,total,footballMinusArt};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter the requested non-negative counts.");
        var values=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var correct=values.SequenceEqual(new[]{7,20,4});
        return new(true,correct,false,correct
            ?"Football 7, 20 club choices total, and four more football than art preferences."
            :"Read one-unit bar ticks, total the four categories, then compare football with art.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Learner identifies the graph's category labels, population, numbered scale (0 upward by ones), and reads bar heights rather than treating bars as pictures.",
        "Learner compares the SAME pet data in a picture graph and a bar graph while explaining how the picture key differs from the numbered axis.",
        "Learner creates multiple true statements about an independently authored field trip class bar graph, including a combination and a comparison.",
        "Learner uses the original season bar values to justify summer count, fall+spring combination, both valid pairs making ten and total 26.",
        "Teacher reviews mathematical statement writing, graph explanations, group discussion and the independence of source-aligned Edulytics examples."
    ];
}
