namespace Edulytics.Core.Mathematics.Assessment;

/// <summary>
/// Exact Assessment Builder contract for an official LearningOutcome that can
/// be generated from READY_VERIFIED SkillContracts without inventing mappings.
/// </summary>
public sealed record Stage19AssessmentSkillContract(
    string OutcomeCode,
    string SkillId,
    IReadOnlyList<string> AllowedQuestionFamilies,
    IReadOnlyList<string> EvidenceLessonCodes);

public static class Stage19AssessmentSkillContracts
{
    public const string GenerationMethod = "skill-contract-assessment-solver-verified-v1";
    public const string SolverIdentifier = "stage19-exact-assessment-solver-v1";
    public const string VerifierIdentifier = "stage19-independent-assessment-verifier-v1";

    private static readonly Stage19AssessmentSkillContract[] Entries =
    [
        new(
            "CCSS:3.NF.A.3",
            "fractions.equivalent",
            [
                "fractions.equivalent.missing_value",
                "fractions.equivalent.recognize",
                "fractions.equivalent.generate_multiple",
                "fractions.equivalent.number_line"
            ],
            [
                "PED:US-CCSS-MATH:G3:U05:L10",
                "PED:US-CCSS-MATH:G3:U05:L11",
                "PED:US-CCSS-MATH:G3:U05:L12"
            ]),
        new(
            "CCSS:4.NF.A.1",
            "fractions.equivalent",
            [
                "fractions.equivalent.missing_value",
                "fractions.equivalent.recognize",
                "fractions.equivalent.generate_multiple",
                "fractions.equivalent.number_line",
                "fractions.equivalent.reduce_common_factor"
            ],
            [
                "PED:US-CCSS-MATH:G4:U02:L07",
                "PED:US-CCSS-MATH:G4:U02:L08",
                "PED:US-CCSS-MATH:G4:U02:L10",
                "PED:US-CCSS-MATH:G4:U02:L11"
            ]),
        new(
            "CCSS:6.RP.A.2",
            "ratio.unit_rate",
            [
                "ratio.unit_rate.direct",
                "ratio.unit_rate.equivalent_ratio"
            ],
            [
                "PED:US-CCSS-MATH:G6:U03:L07"
            ]),
        new(
            "CCSS:6.RP.A.3",
            "ratio.unit_rate",
            [
                "ratio.unit_rate.direct",
                "ratio.unit_rate.equivalent_ratio"
            ],
            [
                "PED:US-CCSS-MATH:G6:U03:L07"
            ])
    ];

    private static readonly IReadOnlyDictionary<string, Stage19AssessmentSkillContract> ByOutcomeCode =
        Entries.ToDictionary(x => x.OutcomeCode, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyList<Stage19AssessmentSkillContract> All => Entries;

    public static bool TryResolve(string? outcomeCode, out Stage19AssessmentSkillContract? contract)
    {
        if (string.IsNullOrWhiteSpace(outcomeCode))
        {
            contract = null;
            return false;
        }

        return ByOutcomeCode.TryGetValue(outcomeCode.Trim(), out contract);
    }
}
