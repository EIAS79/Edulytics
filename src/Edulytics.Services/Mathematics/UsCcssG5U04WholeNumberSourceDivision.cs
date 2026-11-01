using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-bounded Grade5 Unit4 exact WHOLE NUMBER DIVISION selected cases,
/// delegated to existing ExactSkillContractQuestionEngine. This is a lesson
/// adapter, not an additional engine. Original diagrams and numerical reasoning
/// require TEACHER_REVIEW_REQUIRED; authored fixtures NOT_RUN.
/// </summary>
public static class UsCcssG5U04WholeNumberSourceDivision
{
    public const string L10Dance="usccss.g5.u04.l10.original_folk_dance_equal_groups";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private sealed record SourceCase(string Family,string Context,string Expression,
        int Result,string Units);
    private static readonly SourceCase[] Cases =
    [
        new(L10Dance,"Original 2021 world-record Peru folk dance: 4704 people in groups of eight",
            "4704 ÷ 8",588,"groups of eight dancers"),
        new(L10Dance,"Original 2021 source same 4704 people in groups of four",
            "4704 ÷ 4",1176,"groups of four dancers"),
        new(L10Dance,"Original 2021 source same 4704 people in groups of two",
            "4704 ÷ 2",2352,"groups of two dancers"),
        new("usccss.g5.u04.l11.source_partial_quotient_totals", "Original G5 U04:L11 partial quotient total source 364 divided by 13", "364 ÷ 13", 28, "groups"),
        new("usccss.g5.u04.l11.source_partial_quotient_totals", "Original G5 U04:L11 partial quotient total source 308 divided by 14", "308 ÷ 14", 22, "groups"),
        new("usccss.g5.u04.l11.source_partial_quotient_totals", "Original G5 U04:L11 partial quotient total source 360 divided by 15", "360 ÷ 15", 24, "groups"),
        new("usccss.g5.u04.l11.source_partial_quotient_totals", "Original G5 U04:L11 partial quotient total source 945 divided by 45", "945 ÷ 45", 21, "groups"),
        new("usccss.g5.u04.l11.source_partial_quotient_totals", "Original G5 U04:L11 partial quotient total source 992 divided by 31", "992 ÷ 31", 32, "groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 130 divided by 13", "130 ÷ 13", 10, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 260 divided by 13", "260 ÷ 13", 20, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 65 divided by 13", "65 ÷ 13", 5, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 39 divided by 13", "39 ÷ 13", 3, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 150 divided by 15", "150 ÷ 15", 10, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 300 divided by 15", "300 ÷ 15", 20, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 60 divided by 15", "60 ÷ 15", 4, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 45 divided by 45", "45 ÷ 45", 1, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 450 divided by 45", "450 ÷ 45", 10, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 900 divided by 45", "900 ÷ 45", 20, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 62 divided by 31", "62 ÷ 31", 2, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 341 divided by 31", "341 ÷ 31", 11, "partial groups"),
        new("usccss.g5.u04.l11.source_convenient_quotient_chunks", "Original source helpful partial quotient chunk 310 divided by 31", "310 ÷ 31", 10, "partial groups"),
        new("usccss.g5.u04.l12.source_elena_partial_quotient_totals", "Original IM 2021 U04:L12 written partial quotient algorithm 448 divided by 16", "448 ÷ 16", 28, "exact whole quotient"),
        new("usccss.g5.u04.l12.source_elena_partial_quotient_totals", "Original IM 2021 U04:L12 written partial quotient algorithm 492 divided by 12", "492 ÷ 12", 41, "exact whole quotient"),
        new("usccss.g5.u04.l12.source_elena_partial_quotient_totals", "Original IM 2021 U04:L12 written partial quotient algorithm 630 divided by 15", "630 ÷ 15", 42, "exact whole quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 320 divided by 16", "320 ÷ 16", 20, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 80 divided by 16", "80 ÷ 16", 5, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 48 divided by 16", "48 ÷ 16", 3, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 240 divided by 12", "240 ÷ 12", 20, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 12 divided by 12", "12 ÷ 12", 1, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 600 divided by 15", "600 ÷ 15", 40, "partial quotient"),
        new("usccss.g5.u04.l12.source_written_partial_quotient_chunks", "Original U04:L12 source partial group chunk 30 divided by 15", "30 ÷ 15", 2, "partial quotient"),
        new("usccss.g5.u04.l13.original_partial_quotient_estimate_and_solve", "Original 2021 G5 U04:L13 Estimate and Solve source quotient 612÷34, explain estimate before partial quotients", "612 ÷ 34", 18, "whole groups"),
        new("usccss.g5.u04.l13.original_partial_quotient_estimate_and_solve", "Original 2021 G5 U04:L13 Estimate and Solve source quotient 529÷23, explain estimate before partial quotients", "529 ÷ 23", 23, "whole groups"),
        new("usccss.g5.u04.l13.original_partial_quotient_estimate_and_solve", "Original 2021 G5 U04:L13 Estimate and Solve source quotient 1044÷29, explain estimate before partial quotients", "1044 ÷ 29", 36, "whole groups"),
        new("usccss.g5.u04.l13.original_division_number_talk", "Original 2021 G5 U04:L13 Number Talk exact quotient 110÷10", "110 ÷ 10", 11, "groups"),
        new("usccss.g5.u04.l13.original_division_number_talk", "Original 2021 G5 U04:L13 Number Talk exact quotient 121÷11", "121 ÷ 11", 11, "groups"),
        new("usccss.g5.u04.l13.original_division_number_talk", "Original 2021 G5 U04:L13 Number Talk exact quotient 132÷12", "132 ÷ 12", 11, "groups"),
        new("usccss.g5.u04.l13.original_division_number_talk", "Original 2021 G5 U04:L13 Number Talk exact quotient 154÷14", "154 ÷ 14", 11, "groups"),
        new("usccss.g5.u04.l14.original_corrected_error_quotients", "Original G5 U04:L14 Find the Mistake division diagram for 1656÷46; source displayed partial quotient work is incorrect", "1656 ÷ 46", 36, "correct whole quotient"),
        new("usccss.g5.u04.l14.original_corrected_error_quotients", "Original G5 U04:L14 Find the Mistake division diagram for 972÷18; source displayed partial quotient work is incorrect", "972 ÷ 18", 54, "correct whole quotient"),
        new("usccss.g5.u04.l14.original_corrected_error_quotients", "Original G5 U04:L14 Find the Mistake division diagram for 744÷24; source displayed partial quotient work is incorrect", "744 ÷ 24", 31, "correct whole quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 920÷46, correct the corresponding multiplier rather than the source's wrong label", "920 ÷ 46", 20, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 230÷46, correct the corresponding multiplier rather than the source's wrong label", "230 ÷ 46", 5, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 184÷46, correct the corresponding multiplier rather than the source's wrong label", "184 ÷ 46", 4, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 900÷18, correct the corresponding multiplier rather than the source's wrong label", "900 ÷ 18", 50, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 72÷18, correct the corresponding multiplier rather than the source's wrong label", "72 ÷ 18", 4, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 480÷24, correct the corresponding multiplier rather than the source's wrong label", "480 ÷ 24", 20, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 240÷24, correct the corresponding multiplier rather than the source's wrong label", "240 ÷ 24", 10, "partial quotient"),
        new("usccss.g5.u04.l14.original_error_analysis_valid_chunks", "Original G5 L14 division source arithmetic chunk 24÷24, correct the corresponding multiplier rather than the source's wrong label", "24 ÷ 24", 1, "partial quotient"),
        new("usccss.g5.u04.l15.original_missing_rectangle_dimensions", "Original source rectangle table: width feet when area816 sq ft and length24 ft", "816 ÷ 24", 34, "source rectangle units as indicated"),
        new("usccss.g5.u04.l15.original_missing_rectangle_dimensions", "Original source rectangle table: length feet when area1248 sq ft and width48 ft", "1248 ÷ 48", 26, "source rectangle units as indicated"),
        new("usccss.g5.u04.l15.original_missing_rectangle_dimensions", "Original source rectangle table: area square feet when length23 ft and width253 ft", "23 × 253", 5819, "source rectangle units as indicated"),
        new("usccss.g5.u04.l15.original_missing_rectangle_dimensions", "Original source rectangle table: length feet when area5796 sq ft and width36 ft", "5796 ÷ 36", 161, "source rectangle units as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: height feet when volume375 cubic ft and base area15 square ft", "375 ÷ 15", 25, "dimension or product as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: base area square feet when volume1176 cubic ft and height28 ft", "1176 ÷ 28", 42, "dimension or product as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: Clare's height feet when prism volume882 cubic ft, length6 ft, width7 ft", "882 ÷ (6 × 7)", 21, "dimension or product as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: unknown width feet when volume936 cubic ft, length8 ft, height9 ft", "936 ÷ (8 × 9)", 13, "dimension or product as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: unknown length feet when volume1536 cubic ft, width48 ft, height2 ft", "1536 ÷ (48 × 2)", 16, "dimension or product as indicated"),
        new("usccss.g5.u04.l15.original_prism_base_and_height", "Original source prism table: product width×height in square feet, NOT uniquely either dimension, when volume1008 cu ft and length36 ft", "1008 ÷ 36", 28, "dimension or product as indicated"),
        new("usccss.g5.u04.l16.source_long_noodle_whole_quotient_parts", "2021 source continuous noodle approximately10119 feet shared equally among400 people: whole FEET per person, not full fractional share", "floor(10119 ÷ 400)", 25, "whole feet per diner, plus fractional remainder"),
        new("usccss.g5.u04.l16.source_long_noodle_whole_quotient_parts", "Source noodle original total10119 feet minus twenty-five whole feet for each of400 diners", "10119 − (25 × 400)", 119, "feet left to distribute fractionally"),
        new("usccss.g5.u04.l16.source_han_estimate_integer_checks", "Source 25 whole feet distributed to400 original noodle diners", "25 × 400", 10000, "feet of the original noodle"),
        new("usccss.g5.u04.l16.source_han_estimate_integer_checks", "Han's source estimate 25 1/4 feet each for400 people; total length for that estimate", "(25 × 400) + ((1/4) × 400)", 10100, "feet total"),
        new("usccss.g5.u04.l16.source_han_estimate_integer_checks", "Difference between original modeled length 10119ft and Han's total estimate10100ft", "10119 − 10100", 19, "feet combined difference"),
        new("usccss.g5.u04.l17.source_fraction_partial_quotient_totals", "Original IM 2021 G5 U04:L17 optional fraction-as-quotient source 78÷6", "78 ÷ 6", 13, "whole-number quotient"),
        new("usccss.g5.u04.l17.source_fraction_partial_quotient_totals", "Original IM 2021 G5 U04:L17 optional fraction-as-quotient source 165÷15", "165 ÷ 15", 11, "whole-number quotient"),
        new("usccss.g5.u04.l17.source_fraction_partial_quotient_totals", "Original IM 2021 G5 U04:L17 optional fraction-as-quotient source 540÷18", "540 ÷ 18", 30, "whole-number quotient"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 60/6 or partial quotient", "60 ÷ 6", 10, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 6/6 or partial quotient", "6 ÷ 6", 1, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 66/6 or partial quotient", "66 ÷ 6", 11, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 12/6 or partial quotient", "12 ÷ 6", 2, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 18/6 or partial quotient", "18 ÷ 6", 3, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 75/15 or partial quotient", "75 ÷ 15", 5, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 30/15 or partial quotient", "30 ÷ 15", 2, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 150/15 or partial quotient", "150 ÷ 15", 10, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 15/15 or partial quotient", "15 ÷ 15", 1, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 180/18 or partial quotient", "180 ÷ 18", 10, "numeric component of open source fraction sum"),
        new("usccss.g5.u04.l17.source_exact_integer_chunks", "Original U04:L17 eligible integer-valued partial fraction 360/18 or partial quotient", "360 ÷ 18", 20, "numeric component of open source fraction sum"),
        // END_G5_U04_DIVISION_SOURCE_CASES
    ];
    public static bool Supports(string? family)=>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(c=>string.Equals(c.Family,family,StringComparison.Ordinal));
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out SourceCase? item)
    {
        item=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int index)||index<0||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        var options=Cases.Where(c=>c.Family==family).ToArray();
        if(index>=options.Length)return false;
        item=options[index];return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported source whole number division.");
        ArgumentNullException.ThrowIfNull(random);
        var selected=Cases.Where(c=>c.Family==family).ToArray();
        int idx=random.Next(selected.Length);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["case"]=idx,["slot"]=random.Next(3)};
        if(!Valid(family,p,out var c)||c is null)throw new InvalidOperationException("Malformed selected division case.");
        int correct=c.Result;
        int wrongA=correct+Math.Max(1,correct/4);
        int wrongB=Math.Max(1,correct-Math.Max(1,correct/4));
        int[] answers=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string prompt=$"{c.Context}. Find the source quotient {c.Expression}. "+
            $"Select 1: {answers[0]}; 2: {answers[1]}; 3: {answers[2]}. "+
            "Pupil's original grouping diagram, interpretation and partner explanations are TEACHER_REVIEW_REQUIRED.";
        string solution=$"{c.Expression}={correct} {c.Units}. "+
            "Use multiplication to check the source's number of groups and preserve the original 4704-person total.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed source division parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),StringComparison.Ordinal);
}
