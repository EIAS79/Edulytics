namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Exact Practice migration contract for one READY_VERIFIED lesson.
/// AllowedQuestionFamilies is authoritative for Stage 18 Practice selection:
/// Lesson -> SkillContract -> allowed family -> solver -> verifier.
/// </summary>
public sealed record Stage18PracticeSkillContract(
    string LessonCode,
    string SkillId,
    string Mechanic,
    IReadOnlyList<string> AllowedQuestionFamilies)
{
    public IReadOnlyList<string> SkillIds { get; init; } = [SkillId];
}

public static class Stage18PracticeSkillContracts
{
    public const string GenerationMethod = "skill-contract-solver-verified-v1";
    public const string SolverIdentifier = "stage18-exact-practice-solver-v1";
    public const string VerifierIdentifier = "stage18-independent-practice-verifier-v1";

    private static readonly Stage18PracticeSkillContract[] Entries =
    [
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY",
            "algebra.relationships.two_unknowns",
            "TWO_UNKNOWNS",
            ["algebra.relationships.two_unknowns.total_difference"]),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"]),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            ["fractions.compare.unlike.common_denominator"]),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            ["fractions.compare.unlike.common_denominator"]),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        new(
            "PED:US-CCSS-MATH:G3:U05:L10",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        new(
            "PED:US-CCSS-MATH:G3:U05:L11",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.generate_multiple", "fractions.equivalent.missing_value"]),
        new(
            "PED:US-CCSS-MATH:G3:U05:L12",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.number_line"]),
        new(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"]),
        new(
            "PED:US-CCSS-MATH:G4:U02:L08",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.number_line"]),
        new(
            "PED:US-CCSS-MATH:G4:U02:L10",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.generate_multiple"]),
        new(
            "PED:US-CCSS-MATH:G4:U02:L11",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.reduce_common_factor"]),
        new(
            "PED:US-CCSS-MATH:G6:U03:L07",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"])
    ];

    private static readonly IReadOnlyDictionary<string, Stage18PracticeSkillContract> ByLessonCode =
        Entries.ToDictionary(x => x.LessonCode, StringComparer.Ordinal);

    public static IReadOnlyList<Stage18PracticeSkillContract> All => Entries;

    public static bool TryResolve(string? lessonCode, out Stage18PracticeSkillContract? contract)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            contract = null;
            return false;
        }

        return ByLessonCode.TryGetValue(lessonCode.Trim(), out contract);
    }
}
