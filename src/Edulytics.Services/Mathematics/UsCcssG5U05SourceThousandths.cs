using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-specific Grade5 IM2021 U05:L01 thousandths selected choices.
/// Reuses ExactSkillContractQuestionEngine; this is only a bounded lesson adapter.
/// Picture/tiling representations and open statements remain TEACHER_REVIEW_REQUIRED.
/// Written tests are NOT_RUN; not a source picture replacement.
/// </summary>
public static class UsCcssG5U05SourceThousandths
{
    public const string Family="usccss.g5.u05.l01.original_thousandth_decimal_units";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private sealed record SourceCase(string Family,string Context,string Answer,string OtherA,string OtherB);
    private static readonly SourceCase[] Cases =
    [
        new(Family,"Original source: What do you know about one tenth? Choose the decimal for 1/10 of the SAME whole.",
            "0.1","0.01","0.001"),
        new(Family,"Original source: What do you know about one hundredth? Choose the decimal for 1/100 of the SAME whole.",
            "0.01","0.1","0.001"),
        new(Family,"Original source table: 1/1000 = ? as a decimal of the SAME whole.",
            "0.001","0.01","0.1"),
        new(Family,"In the original hundredths grid, ONE of 100 cells is divided into TEN equal pieces. One such part of the ORIGINAL whole is:",
            "0.001","0.01","0.1"),
        new(Family,"Original place value relationship: ten equal thousandths make how much of the original whole?",
            "0.01","0.001","0.1"),
        new(Family,"Original place value relationship: one whole contains how many equal one-thousandth units?",
            "1000","100","10")
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source A fraction 2/10 represented by a grid. A source figure other than explicitly described D must be physically reviewed by teacher", "0.2", "0.02", "0.002"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid B decimal 0.2. A source figure other than explicitly described D must be physically reviewed by teacher", "0.2", "0.02", "0.12"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid C fraction 15/100. A source figure other than explicitly described D must be physically reviewed by teacher", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid D decimal 0.34. A source figure other than explicitly described D must be physically reviewed by teacher", "0.34", "0.034", "0.304"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source diagram D explicitly describes 62 hundredth cells shaded plus five tenth-strips of one more hundredth cell. A source figure other than explicitly described D must be physically reviewed by teacher", "0.625", "0.62", "0.065"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source instruction shade 0.328 on hundredth grid with thousandth stripes. A source figure other than explicitly described D must be physically reviewed by teacher", "0.328", "0.32", "0.038"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Jada says fifteen hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Priya says one hundred fifty thousandths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Tyler says fifteen THOUSANDTHS, a different quantity than the source shaded fifteen hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.015", "0.15", "0.150"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Diego says one tenth and five hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Mai says one tenth and one half of a tenth. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        // END_G5_U05_SOURCE_CASES
    ];
    public static bool Supports(string? family)=>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(c=>string.Equals(c.Family,family,StringComparison.Ordinal));
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,out SourceCase? c)
    {
        c=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int i)||i<0||i>=Cases.Count(x=>x.Family==family)||
           !p.TryGetValue("slot",out int s)||s is <0 or >2)return false;
        c=Cases.Where(x=>x.Family==family).ElementAt(i);return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown source thousandths family.");
        ArgumentNullException.ThrowIfNull(random);
        var i=random.Next(Cases.Count(x=>x.Family==family));
        int slot=random.Next(3);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["case"]=i,["slot"]=slot};
        if(!Valid(family,p,out var c)||c is null)throw new InvalidOperationException("Invalid source decimal case.");
        string[] options=slot switch
        {
            0=>[c.Answer,c.OtherA,c.OtherB],
            1=>[c.OtherA,c.Answer,c.OtherB],
            _=>[c.OtherA,c.OtherB,c.Answer]
        };
        string prompt=$"{c.Context} Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. "+
            "For drawings, shaded mosaics and original poster explanations: TEACHER_REVIEW_REQUIRED.";
        string solution=$"Correct source place-value unit is {c.Answer}. "+
            "One thousandth is one of 1000 equal pieces of the ORIGINAL whole; "+
            "teacher reviews the student's diagrams and explanation.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed source thousandths parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),StringComparison.Ordinal);
}
