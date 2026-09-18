using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// General lesson-scoped Practice contract used by the runtime capability layer.
/// Official curriculum identifiers remain separate and are never synthesized here.
/// A Supporting lesson may therefore be READY_VERIFIED for Practice without an
/// official OutcomeCode.
/// </summary>
public sealed record LessonPracticeContract(
    string LessonCode,
    string SkillId,
    string Mechanic,
    IReadOnlyList<string> AllowedQuestionFamilies,
    string SourceType,
    string Readiness,
    string ContractVersion)
{
    public IReadOnlyList<string> PrimarySkillIds => [SkillId];
    public IReadOnlyList<string> SecondarySkillIds { get; init; } = [];
    public IReadOnlyList<string> PrerequisiteSkillIds { get; init; } = [];
    public IReadOnlyList<string> ForbiddenQuestionFamilies { get; init; } = [];
    public IReadOnlyDictionary<string, int> PrimaryCoverage { get; init; } =
        new Dictionary<string, int>(StringComparer.Ordinal)
        {
            [SkillId] = 100
        };
    public int? CurriculumLogicalLevel { get; init; }
    public string? SourceReference { get; init; }

    public Stage18PracticeSkillContract ToLegacyStage18Contract() =>
        new(LessonCode, SkillId, Mechanic, AllowedQuestionFamilies);
}

/// <summary>
/// Authoritative runtime registry for lesson-scoped exact Supporting Practice.
/// Entries are admitted only after an explicit mapping decision and exact family
/// coverage. Game routing is presentation-only and never grants capability.
/// </summary>
public static class LessonPracticeContractRegistry
{
    public const string Version = "supporting-lesson-practice-v2";

    private static readonly LessonPracticeContract[] Entries =
    [
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY",
            "algebra.relationships.two_unknowns",
            "TWO_UNKNOWNS",
            ["algebra.relationships.two_unknowns.total_difference"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            ["fractions.compare.unlike.common_denominator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            ["fractions.compare.unlike.common_denominator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        Ready(
            "PED:US-CCSS-MATH:G7:U06:L15",
            "algebra.linear.inequality.solve",
            "LINEAR_INEQUALITY",
            ["algebra.linear.inequality.ax_plus_b_relation_c"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_10"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.contextual_across_10"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.contextual_within_100"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.contextual_within_100"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_10"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_10"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.columnar"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.columnar"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_10"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_10"]),
        Ready(
            "PED:UAE-MOE-MATH:L10:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L10:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L11:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L11:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L12:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L12:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L3:COMMON:02:02:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple"]),
        Ready(
            "PED:UAE-MOE-MATH:L4:COMMON:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:ADVANCED:03:03:UNIT-RATE",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:GENERAL:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:GENERAL:03:03:UNIT-RATE",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L7:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L7:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L8:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L8:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L9:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:UAE-MOE-MATH:L9:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-1:BUILD",
            "fractions.notation",
            "STANDARD_EXACT",
            ["fractions.notation.identify_part"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-1:APPLY",
            "fractions.notation",
            "STANDARD_EXACT",
            ["fractions.notation.identify_part"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:BUILD",
            "fractions.of_quantity",
            "STANDARD_EXACT",
            ["fractions.of_quantity.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:APPLY",
            "fractions.of_quantity",
            "STANDARD_EXACT",
            ["fractions.of_quantity.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:BUILD",
            "fractions.of_quantity",
            "STANDARD_EXACT",
            ["fractions.of_quantity.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:APPLY",
            "fractions.of_quantity",
            "STANDARD_EXACT",
            ["fractions.of_quantity.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L2:COMMON:03:02:FRACTIONS-OF-QUANTITIES",
            "fractions.of_quantity",
            "STANDARD_EXACT",
            ["fractions.of_quantity.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-3:BUILD",
            "fractions.number_line",
            "STANDARD_EXACT",
            ["fractions.number_line.read"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-3:APPLY",
            "fractions.number_line",
            "STANDARD_EXACT",
            ["fractions.number_line.read"]),
        Ready(
            "PED:UAE-MOE-MATH:L3:COMMON:02:01:FRACTIONS-ON-A-NUMBER-LINE",
            "fractions.number_line",
            "STANDARD_EXACT",
            ["fractions.number_line.read"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-1:BUILD",
            "fractions.mixed_numbers.number_line",
            "STANDARD_EXACT",
            ["fractions.mixed_numbers.number_line.improper_numerator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-1:APPLY",
            "fractions.mixed_numbers.number_line",
            "STANDARD_EXACT",
            ["fractions.mixed_numbers.number_line.improper_numerator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:BUILD",
            "fractions.mixed_improper.convert",
            "STANDARD_EXACT",
            ["fractions.mixed_improper.convert_to_improper_numerator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:APPLY",
            "fractions.mixed_improper.convert",
            "STANDARD_EXACT",
            ["fractions.mixed_improper.convert_to_improper_numerator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-1:BUILD",
            "fractions.simplify",
            "STANDARD_EXACT",
            ["fractions.simplify.lowest_terms"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-1:APPLY",
            "fractions.simplify",
            "STANDARD_EXACT",
            ["fractions.simplify.lowest_terms"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:ADVANCED:02:01:SIMPLIFYING-FRACTIONS",
            "fractions.simplify",
            "STANDARD_EXACT",
            ["fractions.simplify.lowest_terms"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:01:SIMPLIFYING-FRACTIONS",
            "fractions.simplify",
            "STANDARD_EXACT",
            ["fractions.simplify.lowest_terms"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD",
            "fractions.common_denominator",
            "STANDARD_EXACT",
            ["fractions.common_denominator.missing_numerator"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-2:APPLY",
            "fractions.common_denominator",
            "STANDARD_EXACT",
            ["fractions.common_denominator.missing_numerator"]),
        Ready(
            "PED:UAE-MOE-MATH:L2:COMMON:03:03:EQUIVALENT-SIMPLE-FRACTIONS",
            "fractions.equivalent",
            "STANDARD_EXACT",
            ["fractions.equivalent.missing_value"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:03:MULTIPLY-FRACTIONS-BY-WHOLE-NUMBERS",
            "fractions.multiply_by_whole",
            "STANDARD_EXACT",
            ["fractions.multiply_by_whole.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:GENERAL:02:03:MULTIPLY-FRACTIONS-BY-WHOLE-NUMBERS",
            "fractions.multiply_by_whole",
            "STANDARD_EXACT",
            ["fractions.multiply_by_whole.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:ADVANCED:02:03:MULTIPLY-FRACTIONS",
            "fractions.multiply",
            "STANDARD_EXACT",
            ["fractions.multiply.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:03:MULTIPLY-FRACTIONS",
            "fractions.multiply",
            "STANDARD_EXACT",
            ["fractions.multiply.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:ADVANCED:02:04:DIVIDE-FRACTIONS-BY-WHOLE-NUMBERS",
            "fractions.divide_by_whole",
            "STANDARD_EXACT",
            ["fractions.divide_by_whole.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:04:DIVIDE-FRACTIONS-BY-WHOLE-NUMBERS",
            "fractions.divide_by_whole",
            "STANDARD_EXACT",
            ["fractions.divide_by_whole.exact"]),
        Ready(
            "PED:US-CCSS-MATH:G7:U06:L13",
            "algebra.linear.inequality.solve",
            "STANDARD_EXACT",
            ["algebra.linear.inequality.ax_plus_b_relation_c"]),
        Ready(
            "PED:US-CCSS-MATH:G8:U03:L12",
            "algebra.linear.solve",
            "STANDARD_EXACT",
            ["algebra.linear.ax_plus_b_equals_c"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-4:BUILD",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-4:APPLY",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-3:BUILD",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-3:APPLY",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L3:COMMON:02:04:ADD-AND-SUBTRACT-RELATED-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L4:COMMON:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L5:GENERAL:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:ADVANCED:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS",
            "fractions.add_subtract",
            "STANDARD_EXACT",
            ["fractions.add_subtract.exact"]),
        Ready(
            "PED:UAE-MOE-MATH:L3:COMMON:02:03:COMPARE-FRACTIONS",
            "fractions.compare",
            "STANDARD_EXACT",
            ["fractions.compare.general"]),
        Ready(
            "PED:US-CCSS-MATH:G4:U02:L06",
            "fractions.compare.benchmark",
            "STANDARD_EXACT",
            ["fractions.compare.benchmark_half"])
    ];

    private static readonly IReadOnlyDictionary<string, LessonPracticeContract> ByLessonCode =
        Entries.ToDictionary(x => x.LessonCode, StringComparer.Ordinal);

    public static IReadOnlyList<LessonPracticeContract> All => Entries;

    public static bool TryResolve(string? lessonCode, out LessonPracticeContract? contract)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            contract = null;
            return false;
        }

        return ByLessonCode.TryGetValue(lessonCode.Trim(), out contract);
    }

    private static LessonPracticeContract Ready(
        string lessonCode,
        string skillId,
        string mechanic,
        IReadOnlyList<string> families) =>
        new(
            lessonCode,
            skillId,
            mechanic,
            families,
            "SupportingLesson",
            "READY_VERIFIED",
            Version)
        {
            CurriculumLogicalLevel = InferLogicalLevel(lessonCode)
        };

    private static int? InferLogicalLevel(string lessonCode)
    {
        foreach (var pattern in new[]
                 {
                     @":CAMBRIDGE-INTL-MATH:S(?<level>\d+):",
                     @":CAMBRIDGE-INTL-MATH:L(?<level>\d+):",
                     @":UAE-MOE-MATH:L(?<level>\d+):",
                     @":US-CCSS-MATH:G(?<level>\d+):",
                     @":US-CCSS-MATH:HS"
                 })
        {
            var match = Regex.Match(lessonCode, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (!match.Success)
                continue;
            if (match.Groups["level"].Success &&
                int.TryParse(match.Groups["level"].Value, out var level))
            {
                return level;
            }
            if (pattern.EndsWith(":HS", StringComparison.Ordinal))
                return 10;
        }
        return null;
    }
}
