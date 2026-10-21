using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independently authored Grade 2 U01 L08 reading/answerability exercises.
/// Source-specific goals: category-count, combine/compare, same vs different
/// populations, and a question graph cannot answer. Publisher graphs and
/// restricted Cool-down are not copied.
/// </summary>
public static class UsCcssG2Unit1EighthLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L08";
    public const string Version="edulytics-g2-u01-l08-original-pictograph-cooldown-v1";

    // Every self-authored symbol counts as one surveyed preference.
    public static readonly IReadOnlyDictionary<string,int> ChildrenSports =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Soccer"]=6,["Basketball"]=4,["Swimming"]=3};
    public static readonly IReadOnlyDictionary<string,int> AdultsSports =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Soccer"]=2,["Basketball"]=5,["Swimming"]=4};
    public static readonly IReadOnlyDictionary<string,int> ArtSupplies =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Markers"]=7,["Crayons"]=5,["Paint"]=4,["Clay"]=2};

    public sealed record Review(bool MachineScorable,bool Correct,bool TeacherReviewNeeded,string Reason);

    public static Review CheckSportsReading(string? adultsBasketball,string? soccerComparison)
        => CheckAnswers([adultsBasketball,soccerComparison],[5,4],
            "Five adults prefer basketball; six children versus two adults prefer soccer, a difference of four.");

    public static Review CheckArtQuantities(string? markers,string? crayonsAndClay,
        string? moreMarkersThanCrayons,string? paintAndMarkers)
        => CheckAnswers([markers,crayonsAndClay,moreMarkersThanCrayons,paintAndMarkers],
            [7,7,2,11],
            "Read the key (one symbol is one response): markers 7, crayons plus clay 7, markers exceed crayons by 2, paint plus markers 11.");

    public static Review CheckWhichQuestionsGraphCanAnswer(
        string? one,string? two,string? three,string? four,string? five)
    {
        // Original-learning-goal analogue: counts and arithmetic are
        // answerable; personal motivations aren't recorded by a category chart.
        var values=new[]{one,two,three,four,five};
        var expected=new[]{"1","1","0","1","1"};
        var valid=values.All(x=>x is "0" or "1");
        return new(true,valid && values.SequenceEqual(expected),false,
            "The chart answers category counts, combinations, and differences; it does not tell us WHY someone chose a category.");
    }

    public static Review CheckOriginalCoolDown(string? total,string? categoryDifference)
        => CheckAnswers([total,categoryDifference],[18,5],
            "The art supply graph contains 18 survey responses; 7 markers minus 2 clay equals a difference of five.");

    private static Review CheckAnswers(string?[] raw,int[] expected,string positive)
    {
        if(raw.Length!=expected.Length ||
            raw.Any(x=>!int.TryParse(x,NumberStyles.None,CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter all requested non-negative whole numbers.");
        var values=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        return new(true,values.SequenceEqual(expected),false,
            values.SequenceEqual(expected)?positive:
                "Use the stated one-picture-equals-one-person key, and check which categories the question names.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Student reads the graph title, key (one picture means one observation) and category labels before counting.",
        "Student distinguishes child and adult populations and does not merge their counts without being asked.",
        "Student combines or compares categories and uses pictures, numbers or words to justify the result.",
        "Student identifies questions the graph CANNOT answer, such as a person's reason for choosing a category, without inventing evidence.",
        "Teacher evaluates the student's original explanation, open responses and partner conversation; numerical checks do not auto-score that reasoning."
    ];
}
