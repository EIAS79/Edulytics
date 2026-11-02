using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics Grade2 U01 L10 construction checker: create one-symbol
/// pictograph and numbered unit-one bar graph from assigned category data.
/// Source contains student-created drawings and peer review, which remain
/// teacher-scored separately. Tables below are fictional learning examples,
/// never attributed to actual students or copied from publisher art.
/// </summary>
public static class UsCcssG2Unit1TenthLessonGraphConstruction
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L10";
    public const string Version="edulytics-original-g2-u01-l10-build-picture-and-bar-v1";
    public static readonly IReadOnlyDictionary<string,int> TableA=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Comics"]=6,["Science"]=3,["Animals"]=5,["Poetry"]=2};
    public static readonly IReadOnlyDictionary<string,int> TableB=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Music"]=4,["Drawing"]=6,["Soccer"]=3,["Reading"]=7};
    public static readonly IReadOnlyDictionary<string,int> ExitTicketTable=
        new Dictionary<string,int>(StringComparer.Ordinal)
        {["Rice"]=4,["Pasta"]=6,["Salad"]=3,["Soup"]=2};

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Reason);

    public static IReadOnlyDictionary<string,int> DataFor(int tableId)=>tableId switch
    {
        1=>TableA,
        2=>TableB,
        3=>ExitTicketTable,
        _=>throw new InvalidOperationException("Unsupported Grade2 L10 data table")
    };

    public static Review VerifyPictureGraph(
        int tableId,string? title,string? symbol,string? keyValue,
        IReadOnlyList<string?>? counts)
    {
        if(!GoodTitle(title)||string.IsNullOrWhiteSpace(symbol)||symbol.Length>12)
            return new(true,false,true,"Give your own graph a useful title and a short visual symbol.");
        if(keyValue?.Trim()!="1")
            return new(true,false,true,"Each picture symbol must have key value ONE response.");
        return CheckCategoryValues(tableId,counts,
            "Your picture graph has one original symbol for every response in all four categories.");
    }

    public static Review VerifyBarGraph(
        int tableId,string? title,string? scaleStart,
        string? scaleStep,IReadOnlyList<string?>? counts)
    {
        if(!GoodTitle(title)||scaleStart?.Trim()!="0"||
            scaleStep?.Trim()!="1")
            return new(true,false,true,"Give the graph a title; bars use a numbered axis starting at zero with one response per tick.");
        return CheckCategoryValues(tableId,counts,
            "Every original bar height equals its source table value, using an equally spaced one-response scale.");
    }

    private static Review CheckCategoryValues(int tableId,IReadOnlyList<string?>? raw,
        string success)
    {
        if(tableId is <1 or >3)
            return new(false,false,true,"Only verified independent Edulytics data tables are available.");
        if(raw is null || raw.Count!=4 ||
            raw.Any(x=>!int.TryParse(x,NumberStyles.None,
                CultureInfo.InvariantCulture,out _)))
            return new(true,false,true,"Enter a non-negative count for every one of the four categories.");
        var given=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var expected=DataFor(tableId).Values.ToArray();
        var correct=given.SequenceEqual(expected);
        return new(true,correct,true,correct?success:
            "Compare your four category counts to the ASSIGNED table, not another partner's table.");
    }
    private static bool GoodTitle(string? title) =>
        !string.IsNullOrWhiteSpace(title) && title.Trim().Length is >=4 and <=120;

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Activity 1 learner authors a picture graph from assigned table A with an original key, title, labels, and exactly one symbol per response.",
        "Activity 2 learner swaps data tables and creates a bar graph from table B with a correctly named category axis and equally spaced scale starting at zero.",
        "Learner checks the picture counts/bar heights against the appropriate original table and can explain how two representations of SAME data retain every category value.",
        "Learner can give multiple mathematically justified answers to Which One Doesn't Belong? based on meaningful display features.",
        "Teacher checks genuine student-created drawings, peer feedback, revised labels and explanation separately; companion numeric checks do not assert this classroom performance."
    ];
}
