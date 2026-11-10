namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// General lesson-scoped Practice contract used by the runtime capability layer.
/// It is curriculum-neutral: official outcome mappings remain separate and are
/// never synthesized here. A Supporting lesson may therefore be READY_VERIFIED
/// for Practice without an official OutcomeCode.
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
    public IReadOnlyList<string> SkillIds { get; init; } = [SkillId];

    public Stage18PracticeSkillContract ToLegacyStage18Contract() =>
        new(LessonCode, SkillId, Mechanic, AllowedQuestionFamilies)
        {
            SkillIds = SkillIds
        };
}

/// <summary>
/// First generalized production registry for Supporting lessons that are already
/// mathematically READY_VERIFIED. This registry is authoritative for lesson-scoped
/// exact Practice availability; game routing is presentation-only.
/// </summary>
public static class LessonPracticeContractRegistry
{
    public const string Version = "supporting-lesson-practice-v4";

    private static readonly LessonPracticeContract[] Entries =
    [
        // IM 2021 G1 U01 L01: two original activity goals, no subtraction.
        new(
            "PED:US-CCSS-MATH:G1:U01:L01",
            "number.whole.add_subtract",
            "COUNT_COLLECTIONS_AND_DOT_CUBES",
            [
                "usccss.g1.u01.l01.count_collection_to20",
                "usccss.g1.u01.l01.add_dot_cubes_to10"
            ],
            "IM2021_G1_U01_L01_SOURCE_REVIEWED",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY",
            "algebra.relationships.two_unknowns",
            "TWO_UNKNOWNS",
            ["algebra.relationships.two_unknowns.total_difference"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY",
            "supporting.number.place_value_rounding",
            "ROUNDING",
            ["supporting.number.rounding"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            [
                "fractions.compare.unlike.common_denominator",
                "fractions.compare.unlike.select_greater",
                "fractions.compare.unlike.true_false",
                "fractions.compare.unlike.order_three"
            ],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY",
            "fractions.compare.unlike_denominators",
            "FRACTION_COMPARE_UNLIKE",
            [
                "fractions.compare.unlike.common_denominator",
                "fractions.compare.unlike.select_greater",
                "fractions.compare.unlike.true_false",
                "fractions.compare.unlike.order_three"
            ],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:US-CCSS-MATH:G7:U06:L15",
            "algebra.linear.inequality.solve",
            "LINEAR_INEQUALITY",
            ["algebra.linear.inequality.ax_plus_b_relation_c"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_ten.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_ten.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_100.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_10.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.within_10.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.columnar.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.columnar.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_ten.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.across_ten.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L10:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L10:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L11:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L11:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L12:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L12:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L3:COMMON:02:02:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L4:COMMON:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:ADVANCED:03:03:UNIT-RATE",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:GENERAL:02:01:EQUIVALENT-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize", "fractions.equivalent.generate_multiple", "fractions.equivalent.reduce_common_factor"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:GENERAL:03:03:UNIT-RATE",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L7:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L7:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L8:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L8:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L9:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L9:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-2:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.comparative.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-2:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.comparative.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-1:BUILD",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.complement_100.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-1:APPLY",
            "number.whole.add_subtract",
            "WHOLE_ADD_SUBTRACT",
            ["number.whole.add_subtract.complement_100.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:BUILD",
            "fractions.of_quantity",
            "FRACTION_OF_QUANTITY",
            ["fractions.of_quantity.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:APPLY",
            "fractions.of_quantity",
            "FRACTION_OF_QUANTITY",
            ["fractions.of_quantity.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-2:BUILD",
            "number.whole.multiply",
            "MULTIPLICATION_FACTS",
            ["number.whole.multiply.fact_recall.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-2:APPLY",
            "number.whole.multiply",
            "MULTIPLICATION_FACTS",
            ["number.whole.multiply.fact_recall.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NPV-4:BUILD",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3NPV-4:APPLY",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:BUILD",
            "number.whole.multiply",
            "MULTIPLICATION_FACTS",
            ["number.whole.multiply.fact_recall.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:APPLY",
            "number.whole.multiply",
            "MULTIPLICATION_FACTS",
            ["number.whole.multiply.fact_recall.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NF-2:BUILD",
            "number.whole.divide",
            "DIVISION_REMAINDER",
            ["number.whole.divide.with_remainder.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NF-2:APPLY",
            "number.whole.divide",
            "DIVISION_REMAINDER",
            ["number.whole.divide.with_remainder.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NPV-4:BUILD",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4NPV-4:APPLY",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:BUILD",
            "fractions.of_quantity",
            "FRACTION_OF_QUANTITY",
            ["fractions.of_quantity.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:APPLY",
            "fractions.of_quantity",
            "FRACTION_OF_QUANTITY",
            ["fractions.of_quantity.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-4:BUILD",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-4:APPLY",
            "measurement.scale.read_equal_intervals",
            "SCALE_READING",
            ["measurement.scale.equal_intervals.read_value"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-5:BUILD",
            "supporting.decimals.core",
            "DECIMAL_ROUNDING",
            ["supporting.decimals.round"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-5:APPLY",
            "supporting.decimals.core",
            "DECIMAL_ROUNDING",
            ["supporting.decimals.round"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD",
            "supporting.number.place_value_rounding",
            "ROUNDING",
            ["supporting.number.rounding"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L2:COMMON:03:02:FRACTIONS-OF-QUANTITIES",
            "fractions.of_quantity",
            "FRACTION_OF_QUANTITY",
            ["fractions.of_quantity.build", "fractions.of_quantity.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L2:COMMON:03:03:EQUIVALENT-SIMPLE-FRACTIONS",
            "fractions.equivalent",
            "FRACTION_EQUIVALENT",
            ["fractions.equivalent.missing_value", "fractions.equivalent.recognize"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L6:ADVANCED:03:03:UNITARY-METHOD",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L6:GENERAL:03:03:UNITARY-METHOD",
            "ratio.unit_rate",
            "UNIT_RATE",
            ["ratio.unit_rate.direct", "ratio.unit_rate.equivalent_ratio"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:US-CCSS-MATH:G7:U06:L13",
            "algebra.linear.inequality.solve",
            "LINEAR_INEQUALITY",
            ["algebra.linear.inequality.ax_plus_b_relation_c"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:US-CCSS-MATH:G8:U03:L12",
            "algebra.linear.solve",
            "LINEAR_EQUATION",
            ["algebra.linear.ax_plus_b_equals_c"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-4:BUILD",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.within_one.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3F-4:APPLY",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.within_one.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-3:BUILD",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.same_denominator.mixed.build"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-3:APPLY",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.same_denominator.mixed.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L3:COMMON:02:03:COMPARE-FRACTIONS",
            "fractions.compare.unlike_denominators",
            "SUPPORTING_EXACT",
            [
                "fractions.compare.unlike.common_denominator",
                "fractions.compare.unlike.select_greater",
                "fractions.compare.unlike.true_false",
                "fractions.compare.unlike.order_three"
            ],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L3:COMMON:02:04:ADD-AND-SUBTRACT-RELATED-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.related.build", "fractions.add_subtract.related.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L4:COMMON:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.common_denominator.build", "fractions.add_subtract.common_denominator.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.common_denominator.build", "fractions.add_subtract.common_denominator.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L5:GENERAL:02:02:ADD-AND-SUBTRACT-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.common_denominator.build", "fractions.add_subtract.common_denominator.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L6:ADVANCED:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.common_denominator.build", "fractions.add_subtract.common_denominator.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS",
            "fractions.add_subtract",
            "SUPPORTING_EXACT",
            ["fractions.add_subtract.common_denominator.build", "fractions.add_subtract.common_denominator.apply"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:US-CCSS-MATH:G4:U02:L06",
            "fractions.compare.benchmark",
            "SUPPORTING_EXACT",
            ["fractions.compare.benchmark"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD",
            "algebra.relationships.two_unknowns",
            "TWO_UNKNOWNS",
            ["algebra.relationships.two_unknowns.total_difference"],
            "SupportingLesson",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L10:CORE:02:07:STRAIGHT-LINE-GRAPHS",
            "geometry.coordinate.straight_line",
            "STRAIGHT_LINE",
            [
                "geometry.coordinate.gradient_between_points",
                "geometry.coordinate.evaluate_linear_rule",
                "geometry.coordinate.y_intercept_from_rule"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:02:07:STRAIGHT-LINE-GRAPHS",
            "geometry.coordinate.straight_line",
            "STRAIGHT_LINE",
            [
                "geometry.coordinate.gradient_between_points",
                "geometry.coordinate.evaluate_linear_rule",
                "geometry.coordinate.y_intercept_from_rule"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L11:CORE:02:07:CONSOLIDATING-STRAIGHT-LINE-GRAPHS",
            "geometry.coordinate.straight_line",
            "STRAIGHT_LINE",
            [
                "geometry.coordinate.gradient_between_points",
                "geometry.coordinate.evaluate_linear_rule",
                "geometry.coordinate.y_intercept_from_rule"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:02:07:CONSOLIDATING-STRAIGHT-LINE-GRAPHS",
            "geometry.coordinate.straight_line",
            "STRAIGHT_LINE",
            [
                "geometry.coordinate.gradient_between_points",
                "geometry.coordinate.evaluate_linear_rule",
                "geometry.coordinate.y_intercept_from_rule"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:10:05:QUADRATIC-FORMULA",
            "supporting.algebra.equations",
            "QUADRATIC",
            ["supporting.algebra.quadratic_larger_root"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:10:05:CONSOLIDATING-QUADRATIC-FORMULA",
            "supporting.algebra.equations",
            "QUADRATIC",
            ["supporting.algebra.quadratic_larger_root"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:03:DECIMALS-AND-ROUNDING",
            "supporting.decimals.core",
            "DECIMAL_COMPARE_ROUND",
            [
                "supporting.decimals.compare",
                "supporting.decimals.round"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:03:DECIMALS-AND-ROUNDING",
            "supporting.decimals.core",
            "DECIMAL_COMPARE_ROUND",
            [
                "supporting.decimals.compare",
                "supporting.decimals.round"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:03:DECIMALS-AND-ROUNDING",
            "supporting.decimals.core",
            "DECIMAL_COMPARE_ROUND",
            [
                "supporting.decimals.compare",
                "supporting.decimals.round"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "supporting.indices.roots_standard_form",
            "STANDARD_FORM",
            ["supporting.standard_form.power10_exponent"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "supporting.indices.roots_standard_form",
            "STANDARD_FORM",
            ["supporting.standard_form.power10_exponent"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "supporting.indices.roots_standard_form",
            "STANDARD_FORM",
            ["supporting.standard_form.power10_exponent"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3G-2:BUILD",
            "geometry.angles.relationships",
            "PARALLEL_PERPENDICULAR",
            ["geometry.angles.parallel_lines"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S3:3G-2:APPLY",
            "geometry.angles.relationships",
            "PARALLEL_PERPENDICULAR",
            ["geometry.angles.parallel_lines"],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-1:BUILD",
            "supporting.indices.roots_standard_form",
            "POWERS_OF_TEN",
            [
                "supporting.powers10.evaluate",
                "supporting.powers10.multiply",
                "supporting.powers10.divide",
                "supporting.powers10.missing_exponent"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
        new(
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-1:APPLY",
            "supporting.indices.roots_standard_form",
            "POWERS_OF_TEN",
            [
                "supporting.powers10.evaluate",
                "supporting.powers10.multiply",
                "supporting.powers10.divide",
                "supporting.powers10.missing_exponent"
            ],
            "OfficialMappingUpgrade",
            "READY_VERIFIED",
            Version),
    ];

    private static readonly LessonPracticeContract[] AllEntries = BuildAllEntries();

    private static readonly IReadOnlyDictionary<string, LessonPracticeContract> ByLessonCode =
        AllEntries.ToDictionary(x => x.LessonCode, StringComparer.Ordinal);

    public static IReadOnlyList<LessonPracticeContract> All => AllEntries;

    private static LessonPracticeContract[] BuildAllEntries()
    {
        var byLesson = Entries
            .Where(x => !x.LessonCode.StartsWith(
                "PED:UAE-MOE-MATH:",
                StringComparison.Ordinal))
            .ToDictionary(x => x.LessonCode, StringComparer.Ordinal);
        foreach (var projected in LessonPracticeContractProjection.Load())
        {
            // Hand-authored contracts remain authoritative where they already exist.
            // The explicit mapping projection supplements the registry first.
            byLesson.TryAdd(projected.LessonCode, projected);
        }

        foreach (var projected in PolishLessonPracticeContractProjection.Load())
        {
            // Polish learner-facing lessons are authorized only by the reviewed
            // exact OutcomeCode map. Generated Phase-29 titles are never used.
            byLesson.TryAdd(projected.LessonCode, projected);
        }

        foreach (var projected in OfficialLessonPracticeRuleProjection.Load())
        {
            // Official lessons may be promoted only by reviewed anchored
            // exact-title rules. Broad keyword matching is never authoritative.
            byLesson.TryAdd(projected.LessonCode, projected);
        }

        foreach (var projected in SupportingLessonPracticeRuleProjection.Load())
        {
            // Reviewed Supporting target rules fill only lessons that still do not
            // have an explicit hand-authored or approved-mapping contract.
            byLesson.TryAdd(projected.LessonCode, projected);
        }

        // Original IM 2021 Grade 1 Unit 1 lesson-code exact overrides.
        // Pilot L01 is deliberately untouched; only L02 through L10.
        foreach (var reviewed in UsCcssT001PracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "T001 source lesson missing original contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // Incremental Grade 1 batch 03: exact original lesson codes only.
        foreach (var reviewed in UsCcssG1Batch03PracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "G1 B03 source lesson missing original contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // B001 first source-reviewed unshipped grade positions 16â€“18.
        foreach (var reviewed in UsCcssG1Unit2StoryPracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "G1 U02 source lesson missing existing contract: " + reviewed.LessonCode);
            byLesson[reviewed.LessonCode] = reviewed;
        }
        // Four original IM 2021 G2/G3 source-goal repairs:
        // do not silently fall back to generic outcome projections.
        foreach (var reviewed in UsCcssB001SourceGoalPracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "B001 original source lesson missing practice contract: " + reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // Grade4 U02 L08: existing source-aligned exact numeric families.
        foreach(var reviewed in UsCcssG4Unit2EighthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L15: reuse exact common-denominator and unlike comparison numeric families.
        // Grade4 U02 L16: source four fraction sets, reused exact 2-3 value checks.
        // G4 ordinal 25: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal25SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 26: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal26SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 27: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal27SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 28: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal28SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 29: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal29SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 30: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal30SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 31: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal31SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 32: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal32SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 33: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal33SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 34: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal34SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 35: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal35SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 36: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal36SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 37: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal37SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 38: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal38SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 39: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal39SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 40: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal40SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 41: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal41SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 42: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal42SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 43: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal43SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 44: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal44SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 47: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal47SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 48: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal48SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 49: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal49SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 ordinal 50: source-specific exact numeric SUBSKILL, original open tasks teacher-reviewed.
        foreach(var reviewed in UsCcssG4Ordinal50SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        foreach(var reviewed in UsCcssG4Unit2SixteenthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        foreach(var reviewed in UsCcssG4Unit2FifteenthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L14: reuse exact comparator and benchmark; mixed li/km is teacher led.
        foreach(var reviewed in UsCcssG4Unit2FourteenthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L13: source same-denominator equivalent comparisons and original true/false.
        foreach(var reviewed in UsCcssG4Unit2ThirteenthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L12: reuse exact half/one benchmark and fraction comparison, original25-grid teacher-assessed.
        foreach(var reviewed in UsCcssG4Unit2TwelfthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L11: reuse exact common-factor reduction and multiple generation; optional card art teacher supplied.
        foreach(var reviewed in UsCcssG4Unit2EleventhLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L10: original common-multiplier fractions; reuse existing exact generator.
        foreach(var reviewed in UsCcssG4Unit2TenthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 U02 L09: reuse exact fraction equivalence and number-line Practice; six poster reasoning sets remain teacher-reviewed.
        foreach(var reviewed in UsCcssG4Unit2NinthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L07: existing exact equivalent-number-line and fraction generation; teacher original visuals.
        foreach(var reviewed in UsCcssG4Unit2SeventhLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L06: reuse benchmark numerical Practice, teacher-assess source drawings and optional cards.
        foreach(var reviewed in UsCcssG4Unit2SixthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L05: original source equivalent number line points and running story.
        foreach(var reviewed in UsCcssG4Unit2FifthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L04: reuse equivalence and number line subskills; source visual work teacher assessed.
        foreach(var reviewed in UsCcssG4Unit2FourthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 U02 L03: original same-numerator and same-denominator comparisons.
        foreach(var reviewed in UsCcssG4Unit2ThirdLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 U02 L02: reuse exact fraction bar and equivalence checks;
        // the original >1 multi-whole diagrams remain teacher-reviewed.
        foreach(var reviewed in UsCcssG4Unit2SecondLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 U02 L01: reuse existing exact fraction-bar identification; paper strips and explanation remain teacher-reviewed.
        foreach(var reviewed in UsCcssG4Unit2FirstLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Optional Grade4 U01 L08: existing numeric area and prime classification only; teacher reviews artwork.
        foreach(var reviewed in UsCcssG4Unit1EighthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade 4 U01 L07: preserve source factor/multiple direction and teacher-led Stage2.
        foreach(var reviewed in UsCcssG4Unit1SeventhLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 U01 L06: exact locker divisors and open/closed parity.
        foreach(var reviewed in UsCcssG4Unit1SixthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G4 U01 L05: source contexts need exact and minimum distinction.
        foreach(var reviewed in UsCcssG4Unit1FifthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade4 optional U01 L04: use existing exact fact-recall families.
        foreach(var reviewed in UsCcssG4Unit1FourthLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade 4 Unit 1 Lesson 3: source-specific prime/composite rectangle subskills.
        foreach (var reviewed in UsCcssG4Unit1ThirdLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade 4 Unit 1 Lesson 2: explicit factor-pair source-goal override.
        foreach(var reviewed in UsCcssG4Unit1SecondLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade 4 Unit 1 Lesson 1: strictly source-bound rectangle-area subskills.
        // Hand constructions, open explanations, and publisher Cool-down remain teacher-led.
        foreach (var reviewed in UsCcssG4Unit1FirstLessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        // Grade 5 U01 L02: irregular connected cube counting; partner builds need teacher.
        foreach (var reviewed in UsCcssG5Unit1Lesson2PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        // 2021 G5 U01 L03 complete prism drawings, include hidden layers.
        foreach (var reviewed in UsCcssG5Unit1Lesson3PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L04 exact complete-prism layer reasoning; incomplete cards are teacher-led.
        foreach (var reviewed in UsCcssG5Unit1Lesson4PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L05 2021 fixed 4x10 base, variable prism height; rotate bases manually.
        foreach(var reviewed in UsCcssG5Unit1Lesson5PracticeContracts.Reviewed) byLesson[reviewed.LessonCode]=reviewed;
        // 2021 G5 U01 L06 volume-expression card matching; teacher reviews explanations.
        foreach(var reviewed in UsCcssG5Unit1Lesson6PracticeContracts.Reviewed) byLesson[reviewed.LessonCode]=reviewed;
        // G5 Unit 1 Lesson 7 source-exact digital subskill; original open reasoning teacher-led.
        foreach(var reviewed in UsCcssG5Unit1Lesson7PracticeContracts.Reviewed) byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L08 exact non-overlapping attached-prism addition.
        foreach(var reviewed in UsCcssG5Unit1Lesson8PracticeContracts.Reviewed)byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L09 exact source measured composite dissection totals.
        foreach(var reviewed in UsCcssG5Unit1Lesson9PracticeContracts.Reviewed)byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L10 original 95/168 composite volume expressions; explanation teacher assessed.
        foreach(var reviewed in UsCcssG5Unit1Lesson10PracticeContracts.Reviewed)byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L11 source-prism contexts, teacher evaluates garden design.
        foreach(var reviewed in UsCcssG5Unit1Lesson11PracticeContracts.Reviewed)byLesson[reviewed.LessonCode]=reviewed;
        // G5 U01 L12 original optional 60-container arrangements; subjective ship estimate is teacher-led.
        foreach(var reviewed in UsCcssG5Unit1Lesson12PracticeContracts.Reviewed)byLesson[reviewed.LessonCode]=reviewed;
        // Grade 5 Unit 2 exact source-aligned digital subskills; teacher reviews open tasks.
        foreach(var reviewed in UsCcssG5Unit2SourcePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode]=reviewed;
        // Grade 5 U01 L01 exact source-goal subskill; teacher-scored work is separate.
        foreach (var reviewed in UsCcssG5Unit1Lesson1PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        // IM 2021 G2 U01 L01: reviewed grade-specific source-goal override.
        foreach (var reviewed in UsCcssG2Unit1FirstLessonPracticeContracts.Reviewed)
        {
            // G2 L01 may previously have only a broad projected mapping.
            // Insert this exact source-goal contract without assuming a prior entry.
            byLesson[reviewed.LessonCode] = reviewed;
        }
        // IM 2021 G2 U01 L02/L03, exact 10- and 20-cube lesson overrides.
        foreach (var reviewed in UsCcssG2Unit1Lesson2And3PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson4PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson5PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson6PracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson8PictureGraphContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson9BarGraphContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson10ConstructGraphsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson11GraphQuestionsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson12SortAndDisplayContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson13BarCompareContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson14TapeDiagramContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson15CompareContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson16CompareContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson17CentersContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit1Lesson18ClassSurveysContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson1NoRegroupContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson2UnknownAddendContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson3ZooStoryContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson4OptionalCentersContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson5DecomposeTenContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson6TargetNumbersContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson7SubtractTwoDigitsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson8DifferentWaysContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson9MixedAddSubtractContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson10TargetStage5Contracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson11SeedsStoryContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson12MancalaContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson13StoryEquationsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson14TwoStepSeedsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson15TapeStoriesCenterContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit2Lesson16MarketInventoryContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson1StandardUnitsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson2MeasureCmContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson3BuildRulerContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson4EstimateMeasureCmContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson5MetersContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson6ReptileStoriesContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson7EstimateMeasureStage2Contracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson8WhatIsAnInchContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson9FishFeetContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson10TornTapeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson11SareeRibbonContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson12FriendshipContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson13CenterDay2Contracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson14HandSpanLinePlotContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson15PencilLinePlotContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson16PlantHeightsContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson17CenterPlotContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit3Lesson18YardstickContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4Lesson01NumberLineContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4Lesson02NumberLineFeaturesContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4Lesson03UnlabeledTicksContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4NextThreePracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4ArrowLessonContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit4FinalSixContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit5FirstSixContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit5ComparisonContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit6LessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit7FirstFourContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit7NextFourContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit7LastTenContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit8LessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        foreach (var reviewed in UsCcssG2Unit9LessonPracticeContracts.Reviewed)
            byLesson[reviewed.LessonCode] = reviewed;
        // V1 G1-009: exact original 2021 G1 ordinals 41â€“45 (U03 L04â€“L08).
        // V1 G1-010: original 2021 ordinals 46-50 only (U03 L09-L13).
        foreach(var reviewed in UsCcssG1FullGradeUnit7GeometryTimePracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))throw new InvalidOperationException("G1 U07 source lesson missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeUnit6MeasurementPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))throw new InvalidOperationException("G1 U06 original lesson missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeUnit8ReviewPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))throw new InvalidOperationException("Missing G1 U08 source lesson: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeUnit4FinalPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))throw new InvalidOperationException("Missing original G1 U04 L16â€“23 practice base lesson: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeUnit5AdditionPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("G1 Unit 5 original lesson missing base Practice contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeTwoDigitOperationsPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("Missing canonical new G1 U04 L11-15 Practice lesson: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeTensAndOnesPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("Missing canonical G1 U04 L06â€“10 source lesson Practice contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeStrategyAndTensPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("Original Grade 1 base lesson Practice identity missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeNextPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("Grade-1 full-grade lesson 56-60 missing canonical contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1FullGradeStartPracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("Missing canonical G1 L51-L55 base contract: " + reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1TeenFivePracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("V1 G1-010 missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        foreach(var reviewed in UsCcssG1SourceFiveNextContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("V1 G1-009 missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // V1 G1-008: original 2021 G1 ordinals 36â€“40, including optional L22.
        foreach(var reviewed in UsCcssG1NextSourceFivePracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("V1 G1-008 missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // V1 G1-007: five consecutive original 2021 lessons L16â€“L20.
        foreach(var reviewed in UsCcssG1Unit2EquationsAndStoriesPracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException("V1 G1-007 missing: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // V1 G1-006: first-edition 2021 G1 Unit 2 lessons 11-15.
        foreach(var reviewed in UsCcssG1Unit2CompareFivePracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "V1 G1-006 missing original lesson: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // V1 G1-005: source-ordered Grade 1 Unit 2 lessons 6â€“10.
        // Replace only the five existing generic contracts, preserve prior grades.
        foreach (var reviewed in UsCcssG1Unit2NextFivePracticeContracts.Reviewed)
        {
            if (!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "V1 G1-005 missing existing lesson contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // V2 B001 first 20: ONLY G1 U02 L04-L05 (L06 onward remains B002).
        foreach(var reviewed in UsCcssG1Unit2Part2PracticeContracts.Reviewed)
        {
            if(!byLesson.ContainsKey(reviewed.LessonCode))
                throw new InvalidOperationException(
                    "G1 U02 L04-L05 missing existing Practice contract: "+reviewed.LessonCode);
            byLesson[reviewed.LessonCode]=reviewed;
        }
        // OUR/IM 6-8 Grade 7 (2017-2019) U01:L01: exact polygon recognition;
        // constructions, explanations and qualitative feedback need teacher review.

        // IM K5 2021 G5 U05:L26: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L26"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L26", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_26_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l26.source_book_fair_margin_examples"],
            "IM2021_G5_U05_L26_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L25: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L25"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L25", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_25_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l25.source_decimal_divided_by_decimal"],
            "IM2021_G5_U05_L25_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L24: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L24"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L24", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_24_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l24.source_decimal_divided_by_whole"],
            "IM2021_G5_U05_L24_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L23: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L23"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L23", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_23_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l23.source_whole_by_fractional_decimal"],
            "IM2021_G5_U05_L23_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L22: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L22"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L22", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_22_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l22.source_whole_by_decimal_unit_division"],
            "IM2021_G5_U05_L22_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L21: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L21", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_21_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l21.source_equivalent_decimal_products"],
            "IM2021_G5_U05_L21_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L20: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L20", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_20_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l20.source_tenths_times_tenths"],
            "IM2021_G5_U05_L20_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L19: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L19", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_19_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l19.source_whole_decimal_properties"],
            "IM2021_G5_U05_L19_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L18: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L18", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_18_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l18.source_whole_number_properties"],
            "IM2021_G5_U05_L18_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L17: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L17", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_17_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l17.source_whole_times_decimals"],
            "IM2021_G5_U05_L17_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L16: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L16", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_16_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l16.original_mixed_decimal_arithmetic"],
            "IM2021_G5_U05_L16_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L15: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L15", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_15_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l15.original_decimal_subtraction"],
            "IM2021_G5_U05_L15_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L14: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L14", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_14_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l14.source_decimal_subtraction_and_truth"],
            "IM2021_G5_U05_L14_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L13: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L13", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_13_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l13.original_place_aligned_additions", "usccss.g5.u05.l13.original_elena_andre_error"],
            "IM2021_G5_U05_L13_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L12: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L12", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_12_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l12.original_decimal_sums", "usccss.g5.u05.l12.source_nearest_whole_estimates"],
            "IM2021_G5_U05_L12_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L11: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L11", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_11_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l11.original_shaded_hundredth_grid_sums", "usccss.g5.u05.l11.original_decimal_addition"],
            "IM2021_G5_U05_L11_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L10: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L10", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_10_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l10.source_luge_time_rounding", "usccss.g5.u05.l10.source_luge_speeds_and_precision"],
            "IM2021_G5_U05_L10_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L09: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L09", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_09_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l09.source_decimal_inequality_truth", "usccss.g5.u05.l09.source_ordering_and_between"],
            "IM2021_G5_U05_L09_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L08: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L08", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_08_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l08.source_round_primary_examples", "usccss.g5.u05.l08.source_optional_rounding_table"],
            "IM2021_G5_U05_L08_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L07: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L07", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_07_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l07.original_doubloon_rounding", "usccss.g5.u05.l07.original_exact_vs_approximate"],
            "IM2021_G5_U05_L07_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L06: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L06", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_06_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l06.original_number_line_comparison", "usccss.g5.u05.l06.original_nested_line_place_values"],
            "IM2021_G5_U05_L06_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L05: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L05", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_05_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l05.original_decimal_true_false", "usccss.g5.u05.l05.original_frisbee_swim_comparisons"],
            "IM2021_G5_U05_L05_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L04: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L04", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_04_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l04.source_gold_weight_conversions", "usccss.g5.u05.l04.source_place_value_nugget_ratios"],
            "IM2021_G5_U05_L04_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L03: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L03", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_03_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l03.original_decimal_expanded_forms", "usccss.g5.u05.l03.source_wodb_and_mai_error"],
            "IM2021_G5_U05_L03_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U05:L02: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U05:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L02", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_02_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u05.l02.original_grid_thousandth_values", "usccss.g5.u05.l02.original_fifteen_hundredths_critic"],
            "IM2021_G5_U05_L02_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L18: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L18", "geometry.volume",
            "G5_SOURCE_UNIT8_LESSON_18_MATH_APPLICATION",
            ["usccss.g5.u08.l18.source_student_created_volume_wodb"],
            "IM2021_G5_U08_L18_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L17: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L17", "fractions.add_subtract",
            "G5_SOURCE_UNIT8_LESSON_17_MATH_APPLICATION",
            ["usccss.g5.u08.l17.source_student_generated_fraction_truths"],
            "IM2021_G5_U08_L17_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L16: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L16", "number.whole.divide",
            "G5_SOURCE_UNIT8_LESSON_16_MATH_APPLICATION",
            ["usccss.g5.u08.l16.source_student_generated_division_number_talks"],
            "IM2021_G5_U08_L16_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L15: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L15", "number.whole.multiply",
            "G5_SOURCE_UNIT8_LESSON_15_MATH_APPLICATION",
            ["usccss.g5.u08.l15.source_student_created_estimation_exploration"],
            "IM2021_G5_U08_L15_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L14: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L14", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT8_LESSON_14_MATH_APPLICATION",
            ["usccss.g5.u08.l14.source_student_created_notice_wonder"],
            "IM2021_G5_U08_L14_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L13: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L13", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT8_LESSON_13_MATH_APPLICATION",
            ["usccss.g5.u08.l13.source_fraction_product_spinner_strategy"],
            "IM2021_G5_U08_L13_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L12: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L12", "supporting.decimals.core",
            "G5_SOURCE_UNIT8_LESSON_12_MATH_APPLICATION",
            ["usccss.g5.u08.l12.source_original_decimal_race_place_rules"],
            "IM2021_G5_U08_L12_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L11: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L11", "fractions.add_subtract",
            "G5_SOURCE_UNIT8_LESSON_11_MATH_APPLICATION",
            ["usccss.g5.u08.l11.source_fraction_subtraction_comparison_games"],
            "IM2021_G5_U08_L11_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L10: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L10", "fractions.add_subtract",
            "G5_SOURCE_UNIT8_LESSON_10_MATH_APPLICATION",
            ["usccss.g5.u08.l10.source_fraction_sum_strategic_games"],
            "IM2021_G5_U08_L10_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L09: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L09", "geometry.volume",
            "G5_SOURCE_UNIT8_LESSON_09_MATH_APPLICATION",
            ["usccss.g5.u08.l09.source_rainwater_millimeter_centimeter_units"],
            "IM2021_G5_U08_L09_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L08: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L08", "geometry.volume",
            "G5_SOURCE_UNIT8_LESSON_08_MATH_APPLICATION",
            ["usccss.g5.u08.l08.source_sand_bags_boxes_volume"],
            "IM2021_G5_U08_L08_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L07: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L07", "geometry.volume",
            "G5_SOURCE_UNIT8_LESSON_07_MATH_APPLICATION",
            ["usccss.g5.u08.l07.source_wagon_photo_estimation"],
            "IM2021_G5_U08_L07_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L06: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L06", "geometry.volume",
            "G5_SOURCE_UNIT8_LESSON_06_MATH_APPLICATION",
            ["usccss.g5.u08.l06.source_126_cube_prism_and_building_volume"],
            "IM2021_G5_U08_L06_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L05: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L05", "number.whole.divide",
            "G5_SOURCE_UNIT8_LESSON_05_MATH_APPLICATION",
            ["usccss.g5.u08.l05.source_division_partial_quotient_error_analysis"],
            "IM2021_G5_U08_L05_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L04: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L04", "number.whole.divide",
            "G5_SOURCE_UNIT8_LESSON_04_MATH_APPLICATION",
            ["usccss.g5.u08.l04.source_division_estimates_missing_factors"],
            "IM2021_G5_U08_L04_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L03: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L03", "number.whole.multiply",
            "G5_SOURCE_UNIT8_LESSON_03_MATH_APPLICATION",
            ["usccss.g5.u08.l03.source_multiplication_strategy_source_factors"],
            "IM2021_G5_U08_L03_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L02: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L02", "number.whole.multiply",
            "G5_SOURCE_UNIT8_LESSON_02_MATH_APPLICATION",
            ["usccss.g5.u08.l02.source_placevalue_partial_products"],
            "IM2021_G5_U08_L02_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U08:L01: source-aligned bounded math practice
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U08:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U08:L01", "number.whole.multiply",
            "G5_SOURCE_UNIT8_LESSON_01_MATH_APPLICATION",
            ["usccss.g5.u08.l01.source_digit_arrangement_maximum_products"],
            "IM2021_G5_U08_L01_SOURCE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L13: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L13", "geometry.perimeter_area",
            "G5_SOURCE_UNIT7_LESSON_13_RECTANGLE_AREA_PERIMETER",
            ["usccss.g5.u07.l13.source_rectangle_area_perimeter_coordinate_pairs"],
            "IM2021_G5_U07_L13_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L12: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L12", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_12_COIN_COORDINATES",
            ["usccss.g5.u07.l12.source_coin_graph_context"],
            "IM2021_G5_U07_L12_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L11: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L11", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_11_SEQUENCE_CORRESPONDING_TERMS",
            ["usccss.g5.u07.l11.source_corresponding_pattern_coordinate_pairs"],
            "IM2021_G5_U07_L11_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L10: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L10", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_10_SEQUENCE_CORRESPONDING_TERMS",
            ["usccss.g5.u07.l10.source_recursive_patterns_offset"],
            "IM2021_G5_U07_L10_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L09: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L09", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_09_SEQUENCE_CORRESPONDING_TERMS",
            ["usccss.g5.u07.l09.source_generated_patterns_two_rules"],
            "IM2021_G5_U07_L09_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L08: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L08", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_08_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l08.source_triangle_side_and_angle_types"],
            "IM2021_G5_U07_L08_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L07: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L07", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_07_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l07.source_quadrilateral_always_sometimes_never"],
            "IM2021_G5_U07_L07_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L06: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L06", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_06_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l06.source_hierarchy_square_rhombus_rectangle"],
            "IM2021_G5_U07_L06_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L05: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L05", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_05_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l05.source_trapezoid_two_definitions"],
            "IM2021_G5_U07_L05_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L04: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L04", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_04_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l04.source_quadrilateral_sort_attributes"],
            "IM2021_G5_U07_L04_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L03: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L03", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_03_COORDINATE_POINTS",
            ["usccss.g5.u07.l03.source_shared_coordinate_lines"],
            "IM2021_G5_U07_L03_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L02: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L02", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_02_COORDINATE_POINTS",
            ["usccss.g5.u07.l02.original_plot_specific_points"],
            "IM2021_G5_U07_L02_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L01: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L01", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_01_COORDINATE_POINTS",
            ["usccss.g5.u07.l01.coordinates_description_vocabulary"],
            "IM2021_G5_U07_L01_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L21: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L21", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_21_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l21.original_weekend_fraction_sixty"],
            "IM2021_G5_U06_L21_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L20: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L20", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_20_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l20.original_general_product_inequalities"],
            "IM2021_G5_U06_L20_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L19: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L19", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_19_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l19.original_fraction_distributivity"],
            "IM2021_G5_U06_L19_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L18: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L18", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_18_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l18.original_compare_without_multiplying"],
            "IM2021_G5_U06_L18_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L17: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L17", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_17_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l17.original_products_diagrams_runners"],
            "IM2021_G5_U06_L17_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L16: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L16", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_16_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l16.original_fraction_product_comparisons"],
            "IM2021_G5_U06_L16_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L15: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L15", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_15_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l15.original_apricot_lineplot"],
            "IM2021_G5_U06_L15_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L14: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L14", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_14_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l14.original_egg_fraction_lineplot"],
            "IM2021_G5_U06_L14_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L13: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L13", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_13_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l13.original_common_denominator_critique"],
            "IM2021_G5_U06_L13_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L12: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L12", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_12_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l12.original_salad_dressing_fraction_problems"],
            "IM2021_G5_U06_L12_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L11: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L11", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_11_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l11.original_mixed_fraction_differences"],
            "IM2021_G5_U06_L11_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L10: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L10", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_10_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l10.original_common_denominator_fraction_tasks"],
            "IM2021_G5_U06_L10_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L09: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L09", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_09_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l09.original_equivalent_fraction_expressions"],
            "IM2021_G5_U06_L09_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L08: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L08", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_08_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l08.original_fraction_sums_differences"],
            "IM2021_G5_U06_L08_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L07: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L07", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_07_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l07.original_customary_mile_laps"],
            "IM2021_G5_U06_L07_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L06: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L06", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_06_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l06.source_metric_liquid_volume"],
            "IM2021_G5_U06_L06_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L05: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L05", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_05_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l05.source_multistep_metric_distance"],
            "IM2021_G5_U06_L05_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L04: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L04", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_04_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l04.original_metric_divisions"],
            "IM2021_G5_U06_L04_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L03: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L03", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_03_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l03.original_metric_conversions"],
            "IM2021_G5_U06_L03_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L02: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L02", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_02_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l02.original_powers_of_ten"],
            "IM2021_G5_U06_L02_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U06:L01: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U06:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U06:L01", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_01_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u06.l01.source_tenfold_place_value"],
            "IM2021_G5_U06_L01_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U05:L01 source one thousandth with teacher-drawn
        // mosaic/100-by-10 grid interpretation; numeric selected Practice only.
        byLesson["PED:US-CCSS-MATH:G5:U05:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U05:L01", "supporting.decimals.core",
            "G5_SOURCE_U05_L01_ONE_THOUSANDTH_SAME_WHOLE",
            ["usccss.g5.u05.l01.original_thousandth_decimal_units"],
            "IM2021_G5_U05_L01_THOUSANDTH_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L21: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L21", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_21_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l21.source_food_waste_annual_ounces", "usccss.g5.u04.l21.independent_group_annual_estimates"],
            "IM2021_G5_U04_L21_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L20: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L20", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_20_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l20.original_cargo_container_division"],
            "IM2021_G5_U04_L20_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L19: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L19", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_19_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l19.source_state_rectangular_area_products"],
            "IM2021_G5_U04_L19_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L18: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L18", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_18_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l18.edulytiks_conditional_milk_days"],
            "IM2021_G5_U04_L18_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L17: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L17", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_17_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l17.source_fraction_partial_quotient_totals", "usccss.g5.u04.l17.source_exact_integer_chunks"],
            "IM2021_G5_U04_L17_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L16: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L16", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_16_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l16.source_long_noodle_whole_quotient_parts", "usccss.g5.u04.l16.source_han_estimate_integer_checks"],
            "IM2021_G5_U04_L16_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L15: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L15", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_15_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l15.original_missing_rectangle_dimensions", "usccss.g5.u04.l15.original_prism_base_and_height"],
            "IM2021_G5_U04_L15_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L14: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L14", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_14_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l14.original_corrected_error_quotients", "usccss.g5.u04.l14.original_error_analysis_valid_chunks"],
            "IM2021_G5_U04_L14_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L13: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L13", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_13_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l13.original_partial_quotient_estimate_and_solve", "usccss.g5.u04.l13.original_division_number_talk"],
            "IM2021_G5_U04_L13_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L12: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L12", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_12_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l12.source_elena_partial_quotient_totals", "usccss.g5.u04.l12.source_written_partial_quotient_chunks"],
            "IM2021_G5_U04_L12_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L11: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L11", "number.whole.divide",
            "G5_SOURCE_UNIT4_LESSON_11_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l11.source_partial_quotient_totals", "usccss.g5.u04.l11.source_convenient_quotient_chunks"],
            "IM2021_G5_U04_L11_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U04:L10 source 4704-person folk dance divisions,
        // partner diagrams and strategy TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L10", "number.whole.divide",
            "G5_SOURCE_FOLK_DANCE_EQUAL_SIZE_DIVISION_GROUPS",
            ["usccss.g5.u04.l10.original_folk_dance_equal_groups"],
            "IM2021_G5_U04_L10_BOUNDED_DIVISION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L09: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L09", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_09_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l09.source_birdhouse_min_volumes", "usccss.g5.u04.l09.source_birdhouse_max_volumes"],
            "IM2021_G5_U04_L09_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L08: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L08", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_08_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l08.original_distributive_number_talk", "usccss.g5.u04.l08.original_digits_target_products"],
            "IM2021_G5_U04_L08_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L07: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L07", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_07_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l07.source_composing_products_and_commutativity"],
            "IM2021_G5_U04_L07_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L06: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L06", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_06_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l06.original_carried_multiplication", "usccss.g5.u04.l06.original_three_factor_number_talk"],
            "IM2021_G5_U04_L06_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L05: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L05", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_05_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l05.original_no_composing_products", "usccss.g5.u04.l05.original_number_talk_and_diego_correction"],
            "IM2021_G5_U04_L05_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L04: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L04", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_04_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l04.original_carried_multi_digit_products", "usccss.g5.u04.l04.original_warmup_partial_products"],
            "IM2021_G5_U04_L04_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L03: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L03", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_03_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l03.original_partial_product_algorithm", "usccss.g5.u04.l03.original_245_by_35_partial_cells"],
            "IM2021_G5_U04_L03_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U04:L02: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L02", "number.whole.multiply",
            "G5_SOURCE_UNIT4_LESSON_02_WHOLE_NUMBER_ARITHMETIC",
            ["usccss.g5.u04.l02.original_partial_products_totals", "usccss.g5.u04.l02.original_area_diagram_cells"],
            "IM2021_G5_U04_L02_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 Grade 5 U04:L01 number talk, source estimates, and
        // source products; student strategies TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U04:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U04:L01", "number.whole.multiply",
            "G5_SOURCE_MULTIDIGIT_PRODUCT_ESTIMATE_AND_EXACT_CALCULATION",
            ["usccss.g5.u04.l01.source_multi_digit_product_values", "usccss.g5.u04.l01.source_reasonable_product_estimate"],
            "IM2021_G5_U04_L01_EXACT_SELECTED_AND_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L20: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L20", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_20_BOUNDED_FRACTION",
            ["usccss.g5.u03.l20.source_unknown_class_whole", "usccss.g5.u03.l20.source_calcium_rocket_total"],
            "IM2021_G5_U03_L20_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L19: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L19", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_19_BOUNDED_FRACTION",
            ["usccss.g5.u03.l19.source_estimation_exact", "usccss.g5.u03.l19.original_digit_optimization_extrema"],
            "IM2021_G5_U03_L19_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L18: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L18", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_18_BOUNDED_FRACTION",
            ["usccss.g5.u03.l18.original_warmup_and_picnic", "usccss.g5.u03.l18.original_partner_contexts"],
            "IM2021_G5_U03_L18_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L17: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L17", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_17_BOUNDED_FRACTION",
            ["usccss.g5.u03.l17.source_multiply_divide_warmup", "usccss.g5.u03.l17.source_real_world_fraction_contexts"],
            "IM2021_G5_U03_L17_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L16: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L16", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_16_BOUNDED_FRACTION",
            ["usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "usccss.g5.u03.l16.source_ninetyeight_fifth_estimate"],
            "IM2021_G5_U03_L16_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L15: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L15", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_15_BOUNDED_FRACTION",
            ["usccss.g5.u03.l15.source_four_division_number_talk", "usccss.g5.u03.l15.teacher_authored_card_sort_equivalents"],
            "IM2021_G5_U03_L15_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L14: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L14", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_14_BOUNDED_FRACTION",
            ["usccss.g5.u03.l14.same_divisor_quarter_pattern", "usccss.g5.u03.l14.same_dividend_three_pattern", "usccss.g5.u03.l14.snack_servings_story_selection"],
            "IM2021_G5_U03_L14_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L13: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L13", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_13_BOUNDED_FRACTION",
            ["usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups"],
            "IM2021_G5_U03_L13_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U03:L12: source-specific selected exact
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L12", "supporting.fractions.multiply_divide",
            "G5_SOURCE_UNIT3_LESSON_12_BOUNDED_FRACTION",
            ["usccss.g5.u03.l12.third_division_teacher_diagrams", "usccss.g5.u03.l12.quarter_division_pattern"],
            "IM2021_G5_U03_L12_AUTHORING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L11 source half-pan and exact four-divisor
        // warmup; teacher reviews drawings and MLR7 comparison.
        byLesson["PED:US-CCSS-MATH:G5:U03:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L11", "supporting.fractions.multiply_divide",
            "G5_SOURCE_HALF_PAN_FRACTION_DIVIDED_BY_WHOLE",
            ["usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares", "usccss.g5.u03.l11.four_divisor_number_talk"],
            "IM2021_G5_U03_L11_EXACT_NUMERIC_AND_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L10 optional exact sharing, original diagrams
        // and quotient ordering justification teacher-reviewed.
        byLesson["PED:US-CCSS-MATH:G5:U03:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L10", "supporting.fractions.multiply_divide",
            "G5_SOURCE_PRETZELS_AND_THIRDS_SHARING_QUOTIENTS",
            ["usccss.g5.u03.l10.source_pretzel_share_per_student", "usccss.g5.u03.l10.source_divide_by_three_pattern"],
            "IM2021_G5_U03_L10_TEACHER_LED_AND_EXACT_SELECTED",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L09 OPTIONAL open flag design is teacher led;
        // only a separate fixed first-party support model is selected Practice.
        byLesson["PED:US-CCSS-MATH:G5:U03:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L09", "supporting.fractions.multiply_divide",
            "G5_OPTIONAL_ORIGINAL_FLAG_DESIGN_TEACHER_REVIEW_PLUS_DIGITAL_SUPPORT",
            ["usccss.g5.u03.l09.first_party_flag_band_fabric_area_support"],
            "IM2021_G5_U03_L09_OPEN_DESIGN_HUMAN_REVIEW_REQUIRED",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L08 Thailand/Colombia flag exact measurements,
        // original artwork and open source questions TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L08", "supporting.fractions.multiply_divide",
            "G5_SOURCE_FLAG_MEASURED_FRACTIONAL_AREA_AND_LENGTH",
            ["usccss.g5.u03.l08.original_flags_fraction_measure"],
            "IM2021_G5_U03_L08_EXACT_FLAG_PRODUCTS_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L07 source fraction products and missing
        // factors, with original image explanations teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G5:U03:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L07", "supporting.fractions.multiply_divide",
            "G5_SOURCE_NONUNIT_FRACTION_PRODUCT_AND_MISSING_FACTOR",
            ["usccss.g5.u03.l07.source_fraction_products", "usccss.g5.u03.l07.missing_fraction_factors"],
            "IM2021_G5_U03_L07_EXACT_SELECTED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 U03:L06 source 8/30 and 24/20; area images and
        // original WODB interpretations remain TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L06", "supporting.fractions.multiply_divide",
            "G5_SOURCE_TWO_NONUNIT_FRACTION_SHADED_AREA",
            ["usccss.g5.u03.l06.nonunit_fraction_area_source_products"],
            "IM2021_G5_U03_L06_EXACT_DIGITAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L05 source A and Han six-fifteenths; teacher
        // evaluates source B/C art, estimation and student drawings.
        byLesson["PED:US-CCSS-MATH:G5:U03:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L05", "supporting.fractions.multiply_divide",
            "G5_UNIT_RECTANGLE_SHADED_FIFTEENTHS",
            ["usccss.g5.u03.l05.shaded_unit_rectangle_product"],
            "IM2021_G5_U03_L05_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G5 U03:L04: exact source park digital subskills,
        // soccer 1/2 of 2/3 and Elena's 5-row x 2-column colored park.
        // DRAWING / ORIGINAL IMAGE / PEER EXPLANATION TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L04",
            "supporting.fractions.multiply_divide",
            "G5_SOURCE_PARK_ORIGINAL_WHOLE_TWO_CONTEXT_NESTED_FRACTION",
            ["usccss.g5.u03.l04.park_nested_original_whole_fraction", "usccss.g5.u03.l04.elena_park_expression_region"],
            "IM2021_G5_U03_L04_PARK_EXACT_DIGITAL_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // IM 2021 Grade 5 U03:L03: preserve four equal columns for 2-5
        // source rows, predict next six rows, and solve the three actual
        // Activity 2 unit-fraction equations relative to the SAME whole.
        // Source image interpretations, estimates and explanations are
        // TEACHER_REVIEW_REQUIRED, never scored by objective choices.
        byLesson["PED:US-CCSS-MATH:G5:U03:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L03",
            "supporting.fractions.multiply_divide",
            "G5_ORIGINAL_WHOLE_UNIT_FRACTION_GRID_PATTERN",
            [
                "usccss.g5.u03.l03.four_columns_unit_fraction_pattern",
                "usccss.g5.u03.l03.unit_fraction_equation_value"
            ],
            "IM2021_G5_U03_L03_EXACT_DIGITAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // IM 2021 Grade 5 U03:L02: one fourth OF one third and
        // one third OF one fourth = 1/12 of the ORIGINAL whole.
        // Priya's one fifth OF one half and reversed = 1/10.
        // Digital exact selections only; open drawings, source image
        // interpretation and written explanations are teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G5:U03:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L02",
            "supporting.fractions.multiply_divide",
            "G5_ORIGINAL_WHOLE_ORDERED_UNIT_FRACTION_AREA_GRID",
            [
                "usccss.g5.u03.l02.unit_fraction_of_unit_fraction",
                "usccss.g5.u03.l02.same_original_whole_order_comparison"
            ],
            "IM2021_G5_U03_L02_EXACT_DIGITAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // IM 2021 Grade 5 U03:L01: one piece of one selected half
        // measured against the ORIGINAL whole (1/6 or 1/8).
        // Digital objective selections are exact; drawings and explanations
        // remain TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U03:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U03:L01",
            "supporting.fractions.multiply_divide",
            "G5_ORIGINAL_WHOLE_FRACTION_OF_HALF_GRID",
            [
                "usccss.g5.u03.l01.fraction_of_half_original_whole",
                "usccss.g5.u03.l01.compare_original_whole_grids"
            ],
            "IM2021_G5_U03_L01_EXACT_DIGITAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // IM 2021 Grade 7 U01:L01: reuse released exact polygon-copy route.
        byLesson["PED:US-CCSS-MATH:G7:U01:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L01",
            "geometry.similarity.scale_factor",
            "G7_EXACT_SCALED_COPY_CORRESPONDENCE",
            ["usccss.g7.u01.l01.scaled_copy_correspondence"],
            "OUR_IM_G7_2017_2019_L01_EXACT_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM 6-8 Grade 7 (2017-2019) U01:L02. Exact numeric
        // correspondence and rational factor selections are supported by
        // independently authored triangles. Drawing, angle tracing and
        // explanations remain teacher reviewed, not machine graded.
        byLesson["PED:US-CCSS-MATH:G7:U01:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L02",
            "geometry.similarity.scale_factor",
            "G7_EXACT_CORRESPONDING_PARTS_AND_SCALE",
            [
                "usccss.g7.u01.l02.corresponding_parts",
                "usccss.g7.u01.l02.scale_factor"
            ],
            "OUR_IM_G7_2017_2019_L02_AUTHORING_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM 6-8 Grade 7 U01:L03: source-grounded first-party scaling.
        // The L02 rational scale-factor family is reused without duplication.
        // Drawings and written critiques require teacher review.
        byLesson["PED:US-CCSS-MATH:G7:U01:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L03",
            "geometry.similarity.scale_factor",
            "G7_EXACT_SCALED_L_DRAWING_AND_MULTIPLICATIVE_NOT_ADDITIVE",
            [
                "usccss.g7.u01.l03.scaled_l_polygon",
                "usccss.g7.u01.l03.multiplicative_vs_additive",
                "usccss.g7.u01.l02.scale_factor"
            ],
            "OUR_IM_G7_2017_2019_L03_SOURCE_SCOPED_NUMERIC_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM 6-8 G7 U01:L04: any two corresponding points scale.
        // The prior polygon oracle also checks angles/diagonals rather than
        // confusing equal angles alone with a valid scaled copy.
        byLesson["PED:US-CCSS-MATH:G7:U01:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L04",
            "geometry.similarity.scale_factor",
            "G7_HIDDEN_CORRESPONDING_DISTANCES_AND_ANGLES",
            [
                "usccss.g7.u01.l04.hidden_corresponding_distance",
                "usccss.g7.u01.l04.angle_and_distance_scaling",
                "usccss.g7.u01.l01.scaled_copy_correspondence"
            ],
            "OUR_IM_G7_2017_2019_L04_FIRST_PARTY_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM G7 U01:L05, 2017–2019: classify factor size and
        // reciprocal direction; reuse precise L02 rational scale choices.
        // Teacher supervises original physical card sort and puzzle.
        byLesson["PED:US-CCSS-MATH:G7:U01:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L05",
            "geometry.similarity.scale_factor",
            "G7_FACTOR_SIZE_AND_INVERSE",
            [
                "usccss.g7.u01.l05.factor_size_and_reciprocal",
                "usccss.g7.u01.l02.scale_factor"
            ],
            "OUR_IM_G7_2017_2019_L05_SOURCE_SCOPED_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM G7 U01:L06 optional-but-canonical source: area is
        // original area times squared scale factor. Teacher reviews block
        // construction and original drawn triangles/parallelograms.
        byLesson["PED:US-CCSS-MATH:G7:U01:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L06",
            "geometry.area.scaled_figures",
            "G7_EXACT_SQUARED_AREA_FACTOR_AND_PATTERN_BLOCKS",
            ["usccss.g7.u01.l06.scaled_area_square_factor"],
            "OUR_IM_G7_2017_2019_L06_AREA_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM G7 U01:L07 scale drawings: retain real-vs-drawn
        // physical units; reuse earlier polygon distortion family for
        // identifying non-scale diagrams. Actual ruler work is teacher-led.
        byLesson["PED:US-CCSS-MATH:G7:U01:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L07",
            "geometry.scale_drawings",
            "G7_DRAWING_CM_TO_ACTUAL_METRES_UNITS",
            [
                "usccss.g7.u01.l07.drawing_actual_distance_units",
                "usccss.g7.u01.l04.angle_and_distance_scaling"
            ],
            "OUR_IM_G7_2017_2019_L07_ORIGINAL_FIRST_PARTY_DIAGRAM",
            "READY_VERIFIED",
            Version);
        // OUR/IM 6-8 G7 U01:L08 optional: measure road polylines
        // separately from direct routes and convert minutes to hours.
        byLesson["PED:US-CCSS-MATH:G7:U01:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L08",
            "geometry.scale_drawings",
            "G7_MAP_ROUTE_DISTANCE_SPEED_AND_TIME",
            ["usccss.g7.u01.l08.map_route_rate_time"],
            "OUR_IM_G7_2017_2019_L08_FIRST_PARTY_MAP_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);
        // OUR/IM Grade 7 U01:L09, create and compare actual-vs-drawing
        // lengths; physical construction and teacher rubric remain required.
        byLesson["PED:US-CCSS-MATH:G7:U01:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L09",
            "geometry.scale_drawings",
            "G7_CREATE_SCALED_PLAN_AND_COMPARE_SCALES",
            [
                "usccss.g7.u01.l09.create_drawing_scale_lengths",
                "usccss.g7.u01.l07.drawing_actual_distance_units"
            ],
            "OUR_IM_G7_2017_2019_L09_FIRST_PARTY_PLAN_AND_TEACHER",
            "READY_VERIFIED",
            Version);
        // OUR/IM Grade 7 U01:L10 changing drawings: length ratios
        // and square-area factors. Reuse exact L06 area family; human
        // ruler work/drawing is a separately assessed task.
        byLesson["PED:US-CCSS-MATH:G7:U01:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L10",
            "geometry.scale_drawings",
            "G7_NEW_DRAWING_SCALE_AREA_AND_MEASUREMENT_PRECISION",
            [
                "usccss.g7.u01.l10.change_scale_length_and_area",
                "usccss.g7.u01.l06.scaled_area_square_factor"
            ],
            "OUR_IM_G7_2017_2019_L10_FIRST_PARTY_AND_TEACHER",
            "READY_VERIFIED",
            Version);
        // OUR/IM G7 U01:L11. A unitless scale is a same-unit
        // ratio; source-specific models also require foot/inch conversion.
        byLesson["PED:US-CCSS-MATH:G7:U01:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L11",
            "geometry.scale_drawings",
            "G7_UNITLESS_SCALE_AND_PHYSICAL_UNIT_CONVERSION",
            [
                "usccss.g7.u01.l11.unitless_scale_and_conversions",
                "usccss.g7.u01.l05.factor_size_and_reciprocal"
            ],
            "OUR_IM_G7_2017_2019_L11_INDEPENDENT_DIAGRAM_TEACHER",
            "READY_VERIFIED",
            Version);
        // OUR/IM Grade 7 2017-2019 U01:L12: equivalent customary/metric
        // source ratios and squared actual areas; reuse the existing L11
        // unitless exact engine and L06 exact area family.
        byLesson["PED:US-CCSS-MATH:G7:U01:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L12",
            "geometry.scale_drawings",
            "G7_UNIT_CONVERSION_EQUIVALENT_SCALES_AND_REAL_AREA",
            ["usccss.g7.u01.l12.equivalent_scales_area","usccss.g7.u01.l11.unitless_scale_and_conversions",
             "usccss.g7.u01.l06.scaled_area_square_factor"],
            "OUR_IM_G7_2017_2019_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM Grade 7 U01:L13 (2017-2019): teacher-scored floor-plan
        // capstone with exact numeric scale choice and paper-fit checks.
        // L09 and L06 bounded families are reused, NOT substituted for
        // classroom measurement, peer critique or drawing evaluation.
        byLesson["PED:US-CCSS-MATH:G7:U01:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U01:L13",
            "geometry.scale_drawings",
            "G7_FLOOR_PLAN_SCALE_FIT_TILE_AREA_AND_TEACHER_REVIEW",
            ["usccss.g7.u01.l13.floor_plan_scale_fit","usccss.g7.u01.l09.create_drawing_scale_lengths",
             "usccss.g7.u01.l06.scaled_area_square_factor"],
            "OUR_IM_G7_2017_2019_L13_ORIGINAL_PROJECT_AUTHORING",
            "READY_VERIFIED",Version);
        // OUR/IM 6-8 Grade7 U02:L01 (2017-2019) source: double number
        // lines, ingredient ratios and curved scaled copies. A wrong generic
        // single-ratio route cannot certify the drawing or mixture experiment.
        byLesson["PED:US-CCSS-MATH:G7:U02:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L01",
            "proportions.equivalent_ratios",
            "G7_ORIGINAL_MIXTURE_DOUBLE_LINE_AND_UNIFORM_CRESCENT",
            ["usccss.g7.u02.l01.ratio_mixtures_crescent"],
            "OUR_IM_G7_2017_2019_U02_L01_FIRST_PARTY_AND_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM 6–8 Grade7 U02:L02: exact proportion-table and inverse
        // constants. The original towel source table has one anomalous
        // row (2 cases,36 rolls); never silently rewrite it to 24.
        byLesson["PED:US-CCSS-MATH:G7:U02:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L02",
            "proportions.equivalent_ratios",
            "G7_PROPORTIONAL_TABLES_CONSTANT_AND_SOURCE_ANOMALY",
            ["usccss.g7.u02.l02.proportional_tables"],
            "OUR_IM_G7_2017_2019_U02_L02_FIRST_PARTY_AND_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM Grade7 U02:L03 (2017-2019): reciprocal cm/mm constants,
        // speed versus pace and area-square conversion in source tables.
        // Maps/drawings and contextual units need teacher evaluation.
        byLesson["PED:US-CCSS-MATH:G7:U02:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L03",
            "proportions.unit_rate",
            "G7_UNIT_CONVERSION_REVERSE_CONSTANT_AND_SPEED",
            ["usccss.g7.u02.l03.unit_rates_reciprocal"],
            "OUR_IM_G7_2017_2019_U02_L03_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM G7 U02:L04 (2017-2019): equations y=kx from
        // tables, unit-row interpretation, proportional and inverse
        // constants, flight distance and fractional servings.
        // Written equations, drawings and oral explanations need educator review.
        byLesson["PED:US-CCSS-MATH:G7:U02:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L04", "proportions.equivalent_ratios",
            "G7_TABLE_TO_PROPORTIONAL_EQUATION_AND_VARIABLE_DIRECTION",
            ["usccss.g7.u02.l04.proportional_equations", "usccss.g7.u02.l03.unit_rates_reciprocal",
             "usccss.g7.u02.l02.proportional_tables"],
            "OUR_IM_G7_2017_2019_U02_L04_EQUATIONS_AND_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM Grade 7 U02:L05 (2017-2019): two equivalent equations
        // for the same proportional relationship and reciprocal constants.
        // Meter-centimeter, water flow, shrimp feeding and independent
        // measurement tables require educator review for explanations.
        // Reuse the L04 exact proportional-equation engine.
        byLesson["PED:US-CCSS-MATH:G7:U02:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L05", "proportions.equivalent_ratios",
            "G7_FORWARD_AND_REVERSE_PROPORTIONAL_EQUATIONS",
            ["usccss.g7.u02.l05.reciprocal_proportional_equations", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l03.unit_rates_reciprocal"],
            "OUR_IM_G7_2017_2019_U02_L05_RECIPROCALS",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 Grade 7 U02 L06. Exact y=kx and inverse
        // revenue, recycler mass/can, and market rates; context units,
        // discrete counts and written algebraic reasoning teacher assessed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L06", "proportions.equivalent_ratios",
            "G7_RATE_EQUATIONS_AND_INVERSE_TICKET_RECYCLE_COUNTS",
            ["usccss.g7.u02.l06.concert_recycling_equations", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l05.reciprocal_proportional_equations"],
            "OUR_IM_G7_2017_2019_U02_L06_SOURCE_AUTHORING",
            "READY_VERIFIED", Version);
        // OUR/IM G7 U02:L07 2017-2019. A finite table with equal
        // quotients only COULD be proportional, whereas unequal
        // quotients refute the claim. Qualitative taste and outdoor
        // measurement contexts require human review.
        byLesson["PED:US-CCSS-MATH:G7:U02:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L07", "proportions.equivalent_ratios",
            "G7_TABLE_PROPORTIONALITY_CONSISTENT_VS_IMPOSSIBLE",
            ["usccss.g7.u02.l07.table_proportional_or_not", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l02.proportional_tables"],
            "OUR_IM_G7_2017_2019_U02_L07_SOURCE_AND_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 Grade 7 U02 L08: compare equation forms.
        // E=12s is proportional, A=6s^2 and V=s^3 are not;
        // temperature has a nonzero intercept. Original tactile cube
        // constructions and explanations require human assessment.
        byLesson["PED:US-CCSS-MATH:G7:U02:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L08", "proportions.equivalent_ratios",
            "G7_CLASSIFY_PROPORTIONAL_EQUATIONS_AFFINE_AND_NONLINEAR",
            ["usccss.g7.u02.l08.classify_equations", "usccss.g7.u02.l07.table_proportional_or_not",
             "usccss.g7.u02.l04.proportional_equations"],
            "OUR_IM_G7_2017_2019_U02_L08_SOURCE_AND_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U02:L09 source information gaps and
        // rate comparisons: no replacement for teacher-led role questions.
        byLesson["PED:US-CCSS-MATH:G7:U02:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L09", "proportions.unit_rate",
            "G7_MISSING_INFORMATION_CONSTANT_RATE_AND_RANKINGS",
            ["usccss.g7.u02.l09.missing_info_rate_comparison", "usccss.g7.u02.l07.table_proportional_or_not",
             "usccss.g7.u02.l03.unit_rates_reciprocal"],
            "OUR_IM_G7_2017_2019_U02_L09_SOURCE_AND_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM 2017-2019 G7 U02 L10: graph-origin/slope checks with
        // a source-independent first-party coordinate diagram; physical
        // plots, matching cards and explanations remain teacher assessed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L10", "proportions.equivalent_ratios",
            "G7_GRAPH_ORIGIN_SLOPE_TABLE_COORDINATE_PROPORTIONS",
            ["usccss.g7.u02.l10.graphs_origin_slope","usccss.g7.u02.l07.table_proportional_or_not",
             "usccss.g7.u02.l08.classify_equations"],
            "OUR_IM_G7_2017_2019_U02_L10_GRAPH_AND_TEACHER_REVIEW",
            "READY_VERIFIED",Version);
        // OUR/IM 2017-2019 G7 U02 L11: interpret points and (1,k) on
        // proportional graphs, connect k to context, and write y=kx. The
        // existing exact graph/equation/rate families already cover the
        // machine-scorable mathematics; invented contexts and plotted graphs
        // remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L11", "proportions.equivalent_ratios",
            "G7_INTERPRET_GRAPH_POINT_ONE_K_CONTEXT_AND_EQUATION",
            ["usccss.g7.u02.l10.graphs_origin_slope", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l03.unit_rates_reciprocal"],
            "OUR_IM_G7_2017_2019_U02_L11_GRAPH_INTERPRETATION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U02 L12: compare proportional graphs on
        // identical axes, connect steepness with k, and reject a delayed-start
        // graph as proportional to a shared clock. This extends the existing
        // G7 graph engine instead of creating a parallel mechanism.
        byLesson["PED:US-CCSS-MATH:G7:U02:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L12", "proportions.equivalent_ratios",
            "G7_COMPARE_PROPORTIONAL_GRAPHS_SAME_AXES_RATE_AND_DELAYED_START",
            ["usccss.g7.u02.l12.compare_same_axes_rates", "usccss.g7.u02.l10.graphs_origin_slope",
             "usccss.g7.u02.l09.missing_info_rate_comparison"],
            "OUR_IM_G7_2017_2019_U02_L12_GRAPH_COMPARISON_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U02 L13: the same proportional
        // relationship is graphed in two directions. Existing reciprocal-rate
        // and proportional-equation families already score the exact core;
        // graph construction, axis labels and interpretations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L13", "proportions.equivalent_ratios",
            "G7_TWO_GRAPH_AXIS_REVERSAL_RECIPROCAL_RATE_AND_EQUATION",
            ["usccss.g7.u02.l05.reciprocal_proportional_equations", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l03.unit_rates_reciprocal"],
            "OUR_IM_G7_2017_2019_U02_L13_RECIPROCAL_GRAPHS_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U02 L14: connect context, table,
        // graph and equation while distinguishing proportional from
        // nonproportional relationships. Existing classification, table and
        // graph families score only closed subresults; authored scenarios and
        // poster critique stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L14", "proportions.equivalent_ratios",
            "G7_FOUR_REPRESENTATIONS_CONTEXT_TABLE_GRAPH_EQUATION_CRITIQUE",
            ["usccss.g7.u02.l08.classify_equations", "usccss.g7.u02.l07.table_proportional_or_not",
             "usccss.g7.u02.l10.graphs_origin_slope"],
            "OUR_IM_G7_2017_2019_U02_L14_FOUR_REPRESENTATIONS_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U02 L15: open water-use modelling with
        // assumptions, rate equations and reciprocal units. The local student
        // edition has no separate L15 practice.html; selected first-party exact
        // Practice reuses existing rate/equation families while investigation
        // quality, evidence and conclusions remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U02:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U02:L15", "proportions.unit_rate",
            "G7_WATER_USE_ASSUMPTIONS_RATE_RECIPROCAL_EQUATIONS",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l05.reciprocal_proportional_equations",
             "usccss.g7.u02.l09.missing_info_rate_comparison"],
            "OUR_IM_G7_2017_2019_U02_L15_OPEN_MODEL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L01: measurement scatter around
        // proportional square side/diagonal/perimeter relationships and a
        // nonproportional area relationship. Physical measurement and
        // explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L01", "proportions.equivalent_ratios",
            "G7_SQUARE_MEASUREMENT_APPROXIMATE_PROPORTION_AND_AREA",
            ["usccss.g7.u02.l07.table_proportional_or_not", "usccss.g7.u02.l10.graphs_origin_slope",
             "supporting.geometry.parallelogram_area"],
            "OUR_IM_G7_2017_2019_U03_L01_MEASUREMENT_ERROR_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L02: defining circle features,
        // radius/diameter measurement and construction. The general supporting
        // engine is extended only for exact d=2r conversion; circle sorting,
        // drawing and measurement quality remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L02", "supporting.geometry.circles_loci",
            "G7_CIRCLE_DEFINITION_RADIUS_DIAMETER_AND_CONSTRUCTION",
            ["usccss.g7.u03.l02.radius_diameter_relation"],
            "OUR_IM_G7_2017_2019_U03_L02_CIRCLE_CONSTRUCTION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L03: measured circumference and
        // diameter, C/d near pi, proportional graph structure and inaccurate
        // measurements. Existing exact circumference and table families cover
        // closed subresults; physical measurement and explanation remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L03", "supporting.geometry.circles_loci",
            "G7_CIRCUMFERENCE_DIAMETER_PI_PROPORTIONAL_MEASUREMENT",
            ["supporting.geometry.circle_circumference_pi_coefficient", "usccss.g7.u03.l02.radius_diameter_relation",
             "usccss.g7.u02.l07.table_proportional_or_not"],
            "OUR_IM_G7_2017_2019_U03_L03_CIRCUMFERENCE_MEASUREMENT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L04: apply circumference with
        // exact pi or a context-appropriate approximation. Exact circle
        // circumference and radius/diameter conversion are machine scored;
        // composite boundaries and approximation-choice explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L04", "supporting.geometry.circles_loci",
            "G7_APPLY_CIRCUMFERENCE_PI_APPROXIMATION_AND_COMPOSITE_BOUNDARY",
            ["supporting.geometry.circle_circumference_pi_coefficient", "usccss.g7.u03.l02.radius_diameter_relation"],
            "OUR_IM_G7_2017_2019_U03_L04_CIRCUMFERENCE_APPLICATION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L05: one wheel rotation travels
        // one circumference, so rolling distance and rotation count are
        // proportional. The existing supporting engine is extended with this
        // exact product/division family; physical rolling observations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L05", "supporting.geometry.circles_loci",
            "G7_WHEEL_CIRCUMFERENCE_ROTATIONS_DISTANCE_AND_SPEED",
            ["usccss.g7.u03.l05.wheel_rotations_distance", "supporting.geometry.circle_circumference_pi_coefficient",
             "usccss.g7.u03.l02.radius_diameter_relation"],
            "OUR_IM_G7_2017_2019_U03_L05_WHEEL_ROLLING_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L06: estimate irregular areas by
        // decomposing or enclosing them with familiar polygons. Existing
        // compound, triangle and parallelogram area families score exact
        // subregions; the choice and defensibility of an estimate stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L06", "geometry.perimeter_area",
            "G7_ESTIMATE_IRREGULAR_AREAS_BY_POLYGON_DECOMPOSITION",
            ["supporting.geometry.compound_area", "supporting.geometry.triangle_area",
             "supporting.geometry.parallelogram_area"],
            "OUR_IM_G7_2017_2019_U03_L06_AREA_ESTIMATION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L07: estimate circle area, compare
        // radius/diameter with area, and recognize that area grows with the
        // square of the linear measure. Existing exact circle-area,
        // radius/diameter, and proportional-table families cover closed subresults.
        byLesson["PED:US-CCSS-MATH:G7:U03:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L07", "supporting.geometry.circles_loci",
            "G7_EXPLORE_CIRCLE_AREA_RADIUS_SQUARED_AND_NONPROPORTIONAL_DIAMETER",
            ["supporting.geometry.circle_area_pi_coefficient", "usccss.g7.u03.l02.radius_diameter_relation",
             "usccss.g7.u02.l07.table_proportional_or_not"],
            "OUR_IM_G7_2017_2019_U03_L07_CIRCLE_AREA_ESTIMATION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L08: rearrange circle sectors to
        // motivate A=pi r^2 from half-circumference times radius. Existing
        // exact circle-area, circumference and radius/diameter families score
        // closed values; the geometric derivation and sector-limit explanation stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L08", "supporting.geometry.circles_loci",
            "G7_DERIVE_CIRCLE_AREA_FROM_REARRANGED_SECTORS_AND_CIRCUMFERENCE",
            ["supporting.geometry.circle_area_pi_coefficient", "supporting.geometry.circle_circumference_pi_coefficient",
             "usccss.g7.u03.l02.radius_diameter_relation"],
            "OUR_IM_G7_2017_2019_U03_L08_SECTOR_REARRANGEMENT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L09: exact full, half and quarter
        // circle areas and composite regions. The existing supporting engine
        // is extended only for half/quarter-disk pi coefficients; full circle
        // and compound-area families are reused.
        byLesson["PED:US-CCSS-MATH:G7:U03:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L09", "supporting.geometry.circles_loci",
            "G7_APPLY_CIRCLE_AREA_PARTIAL_DISKS_AND_COMPOSITE_REGIONS",
            ["supporting.geometry.circle_area_pi_coefficient", "usccss.g7.u03.l09.circle_fraction_area_pi_coefficient",
             "supporting.geometry.compound_area"],
            "OUR_IM_G7_2017_2019_U03_L09_COMPOSITE_CIRCLE_AREA_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L10: distinguish boundary length
        // from covered area and select the appropriate circle measure in a
        // context. Exact circumference, area and radius/diameter calculations
        // are machine scored; measure selection and justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U03:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L10", "supporting.geometry.circles_loci",
            "G7_DISTINGUISH_CIRCUMFERENCE_AND_AREA_IN_CONTEXT",
            ["supporting.geometry.circle_circumference_pi_coefficient", "supporting.geometry.circle_area_pi_coefficient",
             "usccss.g7.u03.l02.radius_diameter_relation"],
            "OUR_IM_G7_2017_2019_U03_L10_BOUNDARY_VS_COVERAGE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // OUR/IM 2017-2019 G7 U03 L11: open stained-glass design and
        // cost modelling using circle area, composite regions and proportional
        // price relationships. The local student edition has no separate
        // practice.html; exact first-party Practice reuses existing families.
        byLesson["PED:US-CCSS-MATH:G7:U03:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U03:L11", "supporting.geometry.circles_loci",
            "G7_STAINED_GLASS_CIRCLE_AREA_DESIGN_AND_COST_MODEL",
            ["supporting.geometry.circle_area_pi_coefficient", "supporting.geometry.compound_area",
             "usccss.g7.u02.l04.proportional_equations"],
            "OUR_IM_G7_2017_2019_U03_L11_STAINED_GLASS_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L01: scaled rectangle dimensions plus fraction/percent
        // of area. Reuse existing scale-factor and percent-equivalent families.
        byLesson["PED:US-CCSS-MATH:G7:U04:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L01", "proportions.equivalent_ratios",
            "G7_FLAGS_SCALE_DIMENSIONS_AND_PERCENT_AREA",
            ["usccss.g7.u01.l02.scale_factor", "usccss.g7.u01.l01.scaled_copy_correspondence",
             "supporting.fdp.percent_equivalent"],
            "G7_U04_L01_FLAGS_SCALE_AND_PERCENT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L02: unit rates with fractional and decimal quantities.
        // Reuse the Grade 7 reciprocal unit-rate family and supporting fraction division.
        byLesson["PED:US-CCSS-MATH:G7:U04:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L02", "proportions.unit_rate",
            "G7_FRACTIONAL_RATIOS_AND_UNIT_RATES",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "supporting.fractions.divide_whole",
             "supporting.fractions.multiply"],
            "G7_U04_L02_FRACTIONAL_RATE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L03: revisit constants of proportionality across
        // rate, table, equation and context. Existing Grade 7 U02 families cover it.
        byLesson["PED:US-CCSS-MATH:G7:U04:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L03", "proportions.equivalent_ratios",
            "G7_REVISIT_PROPORTIONAL_CONSTANTS_TABLES_AND_EQUATIONS",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l04.proportional_equations",
             "usccss.g7.u02.l02.proportional_tables"],
            "G7_U04_L03_PROPORTIONAL_REVIEW_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L04: fractional increases/decreases and equivalent
        // multiplicative representations. Extend the existing supporting exact
        // Practice engine with one bounded fractional-change family.
        byLesson["PED:US-CCSS-MATH:G7:U04:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L04", "percentages.core",
            "G7_FRACTIONAL_INCREASE_DECREASE_AND_MULTIPLIER",
            ["usccss.g7.u04.fractional_change", "supporting.fractions.multiply",
             "usccss.g7.u02.l04.proportional_equations"],
            "G7_U04_L04_FRACTIONAL_CHANGE_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L05: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L05", "percentages.core",
            "G7_DECIMAL_AND_FRACTION_CHANGE_FACTORS",
            ["usccss.g7.u04.fractional_change", "supporting.fdp.percent_equivalent", "usccss.number.rational_terminating_decimal"],
            "G7_U04_L05_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L06: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L06", "percentages.core",
            "G7_PERCENT_INCREASE_DECREASE_TAPE_DIAGRAMS",
            ["percentages.core.increase_decrease", "percentages.core.of_quantity", "usccss.g7.u04.fractional_change"],
            "G7_U04_L06_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L07: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L07", "percentages.core",
            "G7_RECOVER_ONE_HUNDRED_PERCENT_REFERENCE",
            ["percentages.core.increase_decrease", "usccss.g7.u04.reverse_percent_original"],
            "G7_U04_L07_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L08: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L08", "percentages.core",
            "G7_PERCENT_CHANGE_EQUATIONS_AND_REVERSE_MULTIPLIERS",
            ["percentages.core.increase_decrease", "usccss.g7.u04.reverse_percent_original", "usccss.g7.u02.l04.proportional_equations"],
            "G7_U04_L08_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L09: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L09", "percentages.core",
            "G7_NONWHOLE_AND_SUB_ONE_PERCENTAGES",
            ["usccss.g7.u04.nonwhole_percent_of_quantity", "supporting.fdp.percent_equivalent"],
            "G7_U04_L09_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L10: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L10", "percentages.core",
            "G7_TAX_TIP_RATE_AMOUNT_AND_TOTAL",
            ["usccss.g7.u04.nonwhole_percent_of_quantity", "usccss.g7.u04.percent_rate_from_values", "percentages.core.increase_decrease"],
            "G7_U04_L10_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L11: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L11", "percentages.core",
            "G7_MARKUP_DISCOUNT_COMMISSION_AND_INTEREST",
            ["usccss.g7.u04.reverse_percent_original", "usccss.g7.u04.nonwhole_percent_of_quantity", "percentages.core.increase_decrease"],
            "G7_U04_L11_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L12: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L12", "percentages.core",
            "G7_FIND_PERCENT_FROM_ORIGINAL_AND_CHANGED_VALUES",
            ["usccss.g7.u04.percent_rate_from_values", "usccss.g7.u04.reverse_percent_original"],
            "G7_U04_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L13: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L13", "percentages.core",
            "G7_MEASUREMENT_ERROR_AS_PERCENT_OF_ACTUAL",
            ["usccss.g7.u04.percent_error", "percentages.core.of_quantity"],
            "G7_U04_L13_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L14: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L14", "percentages.core",
            "G7_PERCENT_ERROR_CONTEXTS_AND_REVERSE_ESTIMATES",
            ["usccss.g7.u04.percent_error", "usccss.g7.u04.reverse_percent_original"],
            "G7_U04_L14_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L15: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L15", "percentages.core",
            "G7_PERCENT_ERROR_INTERVALS_AND_TOLERANCE",
            ["usccss.g7.u04.error_interval_bound", "usccss.g7.u04.percent_error"],
            "G7_U04_L15_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U04 L16: source-scoped percentage Practice. Open diagrams,
        // explanations, information-gap dialogue, and authored displays stay teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U04:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U04:L16", "percentages.core",
            "G7_POSE_AND_CRITIQUE_PERCENTAGE_PROBLEMS",
            ["usccss.g7.u04.percent_rate_from_values", "usccss.g7.u04.nonwhole_percent_of_quantity", "usccss.g7.u04.reverse_percent_original"],
            "G7_U04_L16_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L01: interpret and compare signed rational numbers in
        // temperature/elevation contexts. Extend the existing supporting engine
        // for exact rational comparison; plotting and contextual justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L01", "supporting.number.directed_operations",
            "G7_INTERPRET_COMPARE_AND_PLOT_SIGNED_RATIONALS",
            ["usccss.g7.u05.compare_signed_rational", "usccss.number.rational_terminating_decimal"],
            "G7_U05_L01_SIGNED_NUMBER_INTERPRETATION_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L02: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L02", "supporting.number.directed_operations",
            "G7_SIGNED_TEMPERATURE_CHANGES_NUMBER_LINE",
            ["usccss.g7.u05.signed_rational_add_subtract", "supporting.number.negative_operation"],
            "G7_U05_L02_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L03: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L03", "supporting.number.directed_operations",
            "G7_SIGNED_ELEVATION_ADDITION_GENERALIZATION",
            ["usccss.g7.u05.signed_rational_add_subtract", "usccss.g7.u05.compare_signed_rational"],
            "G7_U05_L03_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L04: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L04", "supporting.number.directed_operations",
            "G7_MONEY_CREDITS_DEBTS_SIGNED_BALANCE",
            ["usccss.g7.u05.signed_rational_add_subtract", "supporting.number.negative_operation"],
            "G7_U05_L04_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L05: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L05", "supporting.number.directed_operations",
            "G7_SUBTRACTION_AS_ADDING_THE_OPPOSITE",
            ["usccss.g7.u05.signed_rational_add_subtract"],
            "G7_U05_L05_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L06: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L06", "supporting.number.directed_operations",
            "G7_SIGNED_DIFFERENCE_VERSUS_DISTANCE",
            ["usccss.g7.u05.signed_rational_add_subtract", "usccss.g7.u05.signed_rational_distance"],
            "G7_U05_L06_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L07: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L07", "supporting.number.directed_operations",
            "G7_SIGNED_GAINS_LOSSES_AND_NET_CHANGE",
            ["usccss.g7.u05.signed_rational_add_subtract", "usccss.g7.u05.signed_rational_distance"],
            "G7_U05_L07_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L08: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L08", "supporting.fractions.multiply_divide",
            "G7_SIGNED_POSITION_VELOCITY_AND_DISPLACEMENT",
            ["usccss.number.signed_rate_displacement", "usccss.number.signed_rational_multiply"],
            "G7_U05_L08_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L09: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L09", "supporting.fractions.multiply_divide",
            "G7_SIGNED_RATIONAL_MULTIPLICATION_SIGN_RULE",
            ["usccss.number.signed_rational_multiply", "usccss.number.signed_rate_displacement"],
            "G7_U05_L09_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L10: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L10", "supporting.fractions.multiply_divide",
            "G7_SIGNED_RATIONAL_MULTIPLICATION_FLUENCY",
            ["usccss.number.signed_rational_multiply"],
            "G7_U05_L10_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L11: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L11", "supporting.fractions.multiply_divide",
            "G7_SIGNED_RATIONAL_DIVISION_AND_RECIPROCAL",
            ["usccss.number.signed_rational_divide", "usccss.g7.u05.signed_rational_distance"],
            "G7_U05_L11_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L12: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L12", "supporting.fractions.multiply_divide",
            "G7_NEGATIVE_RATES_AND_SIGNED_DISPLACEMENT",
            ["usccss.number.signed_rate_displacement", "usccss.number.signed_rational_multiply", "usccss.number.signed_rational_divide"],
            "G7_U05_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L13: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L13", "supporting.number.directed_operations",
            "G7_RATIONAL_EXPRESSIONS_FOUR_OPERATIONS",
            ["usccss.g7.u05.signed_rational_add_subtract", "usccss.number.signed_rational_multiply", "usccss.number.signed_rational_divide"],
            "G7_U05_L13_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L14: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L14", "supporting.number.directed_operations",
            "G7_MULTI_STEP_RATIONAL_NUMBER_CONTEXTS",
            ["usccss.g7.u05.signed_rational_add_subtract", "usccss.number.signed_rational_multiply", "usccss.number.signed_rational_divide", "usccss.number.signed_rate_displacement"],
            "G7_U05_L14_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L15: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L15", "supporting.algebra.equations",
            "G7_ONE_STEP_RATIONAL_EQUATIONS_INVERSE_OPERATIONS",
            ["usccss.g7.u05.rational_linear_equation", "usccss.g7.u05.signed_rational_add_subtract", "usccss.number.signed_rational_divide"],
            "G7_U05_L15_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L16: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L16", "supporting.algebra.equations",
            "G7_SIGNED_CONTEXTS_TO_RATIONAL_EQUATIONS",
            ["usccss.g7.u05.rational_linear_equation", "usccss.number.signed_rate_displacement", "usccss.g7.u05.signed_rational_add_subtract"],
            "G7_U05_L16_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U05 L17: source-scoped signed-rational Practice.
        // Open diagrams, explanations and contextual models remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U05:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U05:L17", "percentages.core",
            "G7_STOCK_GAINS_LOSSES_AND_PERCENT_CHANGES",
            ["percentages.core.increase_decrease", "usccss.g7.u04.percent_rate_from_values", "usccss.g7.u05.signed_rational_add_subtract"],
            "G7_U05_L17_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L01: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L01", "supporting.algebra.equations",
            "G7_NONPROPORTIONAL_RELATIONSHIPS_AND_UNKNOWN_QUANTITIES",
            ["usccss.g7.u02.l08.classify_equations", "supporting.algebra.equations.mixed"],
            "G7_U06_L01_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L02: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L02", "algebra.linear.solve",
            "G7_TAPE_DIAGRAMS_FOR_PX_PLUS_Q_AND_P_X_PLUS_Q",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.equations.mixed"],
            "G7_U06_L02_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L03: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L03", "algebra.linear.solve",
            "G7_MATCH_TAPE_DIAGRAMS_AND_EQUATIONS",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expand"],
            "G7_U06_L03_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L04: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L04", "algebra.linear.solve",
            "G7_SOLVE_AX_PLUS_B_EQUALS_C_FROM_CONTEXT",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expressions.mixed"],
            "G7_U06_L04_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L05: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L05", "algebra.linear.solve",
            "G7_SOLVE_P_X_PLUS_Q_EQUALS_R",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expand"],
            "G7_U06_L05_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L06: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L06", "algebra.linear.solve",
            "G7_DISTINGUISH_AX_PLUS_B_FROM_A_X_PLUS_B",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expand"],
            "G7_U06_L06_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L07: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L07", "algebra.linear.solve",
            "G7_BALANCED_HANGER_LINEAR_EQUATIONS",
            ["algebra.linear.ax_plus_b_equals_c"],
            "G7_U06_L07_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L08: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L08", "algebra.linear.solve",
            "G7_HANGERS_PARENTHESES_AND_DISTRIBUTION",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expand"],
            "G7_U06_L08_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L09: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L09", "algebra.linear.solve",
            "G7_LINEAR_EQUATIONS_WITH_NEGATIVE_VALUES",
            ["algebra.linear.ax_plus_b_equals_c", "usccss.g7.u05.signed_rational_add_subtract"],
            "G7_U06_L09_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L10: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L10", "algebra.linear.solve",
            "G7_COMPARE_VALID_LINEAR_EQUATION_SOLUTION_PATHS",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.expand"],
            "G7_U06_L10_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L11: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L11", "algebra.linear.solve",
            "G7_CONTEXT_TO_LINEAR_EQUATION_AND_SOLUTION",
            ["algebra.linear.ax_plus_b_equals_c", "supporting.algebra.equations.mixed"],
            "G7_U06_L11_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L12: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L12", "algebra.linear.solve",
            "G7_PERCENT_CHANGE_WORD_PROBLEMS_WITH_EQUATIONS",
            ["algebra.linear.ax_plus_b_equals_c", "percentages.core.increase_decrease", "usccss.g7.u04.reverse_percent_original"],
            "G7_U06_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L14: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L14", "algebra.linear.inequality.solve",
            "G7_CONTEXTUAL_LINEAR_INEQUALITY_CONSTRAINTS",
            ["algebra.linear.inequality.ax_plus_b_relation_c"],
            "G7_U06_L14_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L16: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L16", "algebra.linear.inequality.solve",
            "G7_MATCH_CONTEXTS_TO_INEQUALITIES",
            ["algebra.linear.inequality.ax_plus_b_relation_c"],
            "G7_U06_L16_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L17: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L17", "algebra.linear.inequality.solve",
            "G7_MODEL_REAL_WORLD_CONSTRAINTS_WITH_INEQUALITIES",
            ["algebra.linear.inequality.ax_plus_b_relation_c"],
            "G7_U06_L17_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L18: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L18", "supporting.algebra.expressions",
            "G7_SUBTRACTION_AS_ADDITION_AND_DISTRIBUTION",
            ["supporting.algebra.expand", "supporting.algebra.simplify"],
            "G7_U06_L18_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L19: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L19", "supporting.algebra.expressions",
            "G7_EXPAND_FACTOR_WITH_SIGNED_COEFFICIENTS",
            ["supporting.algebra.expand", "supporting.algebra.factor"],
            "G7_U06_L19_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L20: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L20", "supporting.algebra.expressions",
            "G7_COMBINE_LIKE_TERMS_AND_TEST_EQUIVALENCE",
            ["supporting.algebra.simplify", "supporting.algebra.expressions.mixed"],
            "G7_U06_L20_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L21: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L21", "supporting.algebra.expressions",
            "G7_COMBINE_TERMS_WITH_SUBTRACTION_AND_DISTRIBUTION",
            ["supporting.algebra.simplify", "supporting.algebra.expand"],
            "G7_U06_L21_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L22: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L22"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L22", "supporting.algebra.expressions",
            "G7_EQUIVALENT_EXPRESSIONS_FEWER_TERMS",
            ["supporting.algebra.simplify", "supporting.algebra.expand", "supporting.algebra.factor"],
            "G7_U06_L22_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U06 L23: source-scoped algebra Practice.
        // Tape diagrams, hangers, authored contexts, graphs and explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U06:L23"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U06:L23", "supporting.algebra.expressions",
            "G7_APPLY_EQUIVALENT_EXPRESSIONS_TO_COMPARE_OPTIONS",
            ["supporting.algebra.expressions.mixed", "supporting.algebra.simplify"],
            "G7_U06_L23_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L01: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L01", "geometry.angles.relationships",
            "G7_RIGHT_STRAIGHT_FULL_TURN_AND_ADJACENT_ANGLES",
            ["supporting.geometry.angle_around_point", "supporting.geometry.angle_classify", "geometry.angles.supplementary"],
            "G7_U07_L01_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L02: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L02", "geometry.angles.relationships",
            "G7_COMPLEMENTARY_AND_SUPPLEMENTARY_ADJACENT_ANGLES",
            ["usccss.g7.u07.complementary_angle", "geometry.angles.supplementary"],
            "G7_U07_L02_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L03: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L03", "geometry.angles.relationships",
            "G7_VERTICAL_COMPLEMENTARY_AND_SUPPLEMENTARY_ANGLE_PAIRS",
            ["usccss.g7.u07.vertical_angle", "usccss.g7.u07.complementary_angle", "geometry.angles.supplementary"],
            "G7_U07_L03_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L04: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L04", "geometry.angles.relationships",
            "G7_MULTI_STEP_UNKNOWN_ANGLE_EQUATIONS",
            ["geometry.angles.algebraic_supplementary", "algebra.linear.ax_plus_b_equals_c", "usccss.g7.u07.vertical_angle"],
            "G7_U07_L04_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L05: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L05", "geometry.angles.relationships",
            "G7_CRITIQUE_AND_SOLVE_ANGLE_EQUATIONS",
            ["geometry.angles.algebraic_supplementary", "algebra.linear.ax_plus_b_equals_c", "usccss.g7.u07.complementary_angle"],
            "G7_U07_L05_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L06: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L06", "geometry.triangles.construct",
            "G7_POLYGON_CONSTRUCTION_AND_SIDE_CONSTRAINTS",
            ["usccss.g7.u07.triangle_side_feasible", "supporting.geometry.angle_around_point"],
            "G7_U07_L06_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L07: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L07", "geometry.triangles.construct",
            "G7_TRIANGLE_INEQUALITY_AND_MULTIPLE_CONSTRUCTIONS",
            ["usccss.g7.u07.triangle_side_feasible", "usccss.g7.u07.complementary_angle"],
            "G7_U07_L07_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L08: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L08", "geometry.congruence.identify",
            "G7_TRIANGLE_UNIQUENESS_FROM_COMMON_MEASURES",
            ["geometry.congruence.identify_criterion", "usccss.g7.u07.triangle_side_feasible"],
            "G7_U07_L08_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L09: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L09", "geometry.triangles.construct",
            "G7_TRIANGLE_ANGLE_SUM_AND_UNIQUENESS",
            ["geometry.congruence.identify_criterion", "usccss.g7.u07.triangle_side_feasible", "geometry.angles.supplementary"],
            "G7_U07_L09_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L10: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L10", "geometry.triangles.construct",
            "G7_TRIANGLE_CONSTRUCTION_SSS_ASA_AND_AAA",
            ["geometry.congruence.identify_criterion", "usccss.g7.u07.triangle_side_feasible"],
            "G7_U07_L10_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L11: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L11", "geometry.solids.cross_sections",
            "G7_CROSS_SECTIONS_OF_PRISMS_AND_PYRAMIDS",
            ["usccss.g7.u07.parallel_base_cross_section_sides", "supporting.geometry.angle_classify"],
            "G7_U07_L11_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L12: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L12", "geometry.volume",
            "G7_RIGHT_PRISM_VOLUME_BASE_AREA_TIMES_HEIGHT",
            ["usccss.g7.u07.prism_volume_base_height", "supporting.geometry.cuboid_volume"],
            "G7_U07_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L13: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L13", "geometry.volume",
            "G7_DECOMPOSE_BASE_AREA_THEN_FIND_PRISM_VOLUME",
            ["usccss.g7.u07.prism_volume_base_height", "supporting.geometry.compound_area", "supporting.geometry.triangle_area"],
            "G7_U07_L13_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L14: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L14", "geometry.surface_area",
            "G7_RIGHT_PRISM_SURFACE_AREA_FROM_BASE_AND_LATERAL_FACES",
            ["usccss.g7.u07.prism_surface_area_base_perimeter_height", "geometry.surface_area.rectangular_prism"],
            "G7_U07_L14_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L15: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L15", "geometry.surface_area_volume",
            "G7_DISTINGUISH_VOLUME_FROM_SURFACE_AREA_IN_CONTEXT",
            ["usccss.g7.u07.prism_volume_base_height", "usccss.g7.u07.prism_surface_area_base_perimeter_height", "percentages.core.increase_decrease"],
            "G7_U07_L15_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L16: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L16", "geometry.surface_area_volume",
            "G7_APPLY_PRISM_VOLUME_AND_SURFACE_AREA_TO_DESIGN_CONTEXTS",
            ["usccss.g7.u07.prism_volume_base_height", "usccss.g7.u07.prism_surface_area_base_perimeter_height", "supporting.geometry.compound_area"],
            "G7_U07_L16_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U07 L17: source-scoped geometry Practice.
        // Physical constructions, diagrams, nets, slicing and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U07:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U07:L17", "geometry.solids.construction",
            "G7_BUILD_AND_COMBINE_TRIANGULAR_PRISM_NETS",
            ["usccss.g7.u07.triangle_side_feasible", "usccss.g7.u07.prism_volume_base_height", "usccss.g7.u07.prism_surface_area_base_perimeter_height"],
            "G7_U07_L17_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L01: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L01", "supporting.probability_statistics.core",
            "G7_INFER_UNKNOWN_COMPOSITION_FROM_RANDOM_DRAWS",
            ["supporting.statistics.experimental_probability", "supporting.probability.expected_frequency"],
            "G7_U08_L01_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L02: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L02", "probability.theoretical.core",
            "G7_INTERPRET_PROBABILITY_AS_FRACTION_DECIMAL_PERCENT",
            ["probability.theoretical.simple_event", "supporting.fdp.percent_equivalent"],
            "G7_U08_L02_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L03: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L03", "supporting.probability.sample_space",
            "G7_LIST_SAMPLE_SPACE_AND_CALCULATE_SIMPLE_PROBABILITY",
            ["supporting.probability.sample_space_count", "probability.theoretical.simple_event"],
            "G7_U08_L03_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L04: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L04", "supporting.statistics.experimental_probability",
            "G7_LONG_RUN_EXPERIMENTAL_PROBABILITY_AND_EXPECTED_FREQUENCY",
            ["supporting.statistics.experimental_probability", "supporting.probability.expected_frequency"],
            "G7_U08_L04_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L05: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L05", "supporting.statistics.experimental_probability",
            "G7_NON_EQUALLY_LIKELY_OUTCOMES_FROM_EXPERIMENTAL_DATA",
            ["supporting.statistics.experimental_probability", "probability.theoretical.simple_event"],
            "G7_U08_L05_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L06: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L06", "supporting.probability_statistics.core",
            "G7_DESIGN_VALID_SIMULATION_FOR_STATED_PROBABILITY",
            ["probability.theoretical.simple_event", "supporting.statistics.experimental_probability"],
            "G7_U08_L06_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L07: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L07", "supporting.probability.events",
            "G7_MULTI_STEP_SIMULATION_AND_EMPIRICAL_EVENT_PROBABILITY",
            ["supporting.statistics.experimental_probability", "supporting.probability.independent_product", "supporting.probability.sample_space_count"],
            "G7_U08_L07_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L08: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L08", "supporting.probability.sample_space",
            "G7_ORGANIZE_MULTI_STEP_SAMPLE_SPACES",
            ["supporting.probability.sample_space_count", "supporting.probability.independent_product"],
            "G7_U08_L08_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L09: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L09", "supporting.probability.events",
            "G7_COMPOUND_PROBABILITY_FROM_EQUALLY_LIKELY_OUTCOMES",
            ["supporting.probability.independent_product", "probability.theoretical.complement", "supporting.probability.sample_space_count"],
            "G7_U08_L09_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L10: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L10", "supporting.probability_statistics.core",
            "G7_CRITIQUE_AND_DESIGN_SIMULATIONS",
            ["probability.theoretical.simple_event", "supporting.statistics.experimental_probability", "supporting.probability.sample_space_count"],
            "G7_U08_L10_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L11: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L11", "statistics.center_spread.core",
            "G7_COMPARE_GROUPS_USING_CENTER_AND_SPREAD",
            ["statistics.center_spread.mean", "statistics.center_spread.median", "supporting.statistics.compare_range"],
            "G7_U08_L11_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L12: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L12", "supporting.statistics.data_collection",
            "G7_SAMPLE_VERSUS_POPULATION_AND_INFERENCE",
            ["supporting.statistics.collection_method", "statistics.center_spread.mean"],
            "G7_U08_L12_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L13: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L13", "supporting.statistics.data_collection",
            "G7_REPRESENTATIVE_SAMPLE_AND_SAMPLING_BIAS",
            ["supporting.statistics.collection_method", "supporting.statistics.compare_range"],
            "G7_U08_L13_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L14: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L14", "supporting.statistics.data_collection",
            "G7_RANDOM_SAMPLE_EQUAL_SELECTION_CHANCE",
            ["supporting.statistics.collection_method", "supporting.probability.sample_space_count"],
            "G7_U08_L14_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L15: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L15", "statistics.center_spread.core",
            "G7_ESTIMATE_POPULATION_CENTER_AND_COMPARE_VARIABILITY",
            ["statistics.center_spread.mean", "statistics.center_spread.median", "supporting.statistics.quartile", "supporting.statistics.compare_range"],
            "G7_U08_L15_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L16: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L16", "supporting.statistics.experimental_probability",
            "G7_SAMPLE_PROPORTION_AS_POPULATION_ESTIMATE",
            ["supporting.statistics.experimental_probability", "supporting.statistics.collection_method"],
            "G7_U08_L16_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L17: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L17", "statistics.center_spread.core",
            "G7_SAMPLING_VARIABILITY_AND_SAMPLE_MEAN_DISTRIBUTION",
            ["statistics.center_spread.mean", "supporting.statistics.compare_range", "supporting.statistics.collection_method"],
            "G7_U08_L17_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L18: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L18", "statistics.center_spread.core",
            "G7_COMPARE_POPULATIONS_WITH_SAMPLE_CENTER_AND_VARIATION",
            ["statistics.center_spread.mean", "statistics.center_spread.median", "supporting.statistics.compare_range"],
            "G7_U08_L18_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L19: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L19", "statistics.center_spread.core",
            "G7_INFORMATION_GAP_POPULATION_COMPARISON",
            ["statistics.center_spread.mean", "statistics.center_spread.median", "supporting.statistics.compare_range", "supporting.statistics.collection_method"],
            "G7_U08_L19_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U08 L20: source-scoped probability/statistics Practice.
        // Simulations, sampling plans, graphs, data displays, inference and written explanations remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U08:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U08:L20", "supporting.probability_statistics.core",
            "G7_INTEGRATE_RANDOM_SAMPLING_PROBABILITY_AND_INFERENCE",
            ["supporting.probability_statistics.mixed", "supporting.statistics.collection_method", "statistics.center_spread.mean", "supporting.statistics.experimental_probability"],
            "G7_U08_L20_SOURCE_SCOPED_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L01: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L01", "proportions.unit_rate",
            "G7_RECIPE_SERVINGS_NUTRITION_AND_PROPORTIONAL_SCALING",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l04.proportional_equations", "percentages.core.of_quantity"],
            "G7_U09_L01_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L02: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L02", "proportions.unit_rate",
            "G7_RECIPE_COST_PER_SERVING_AND_SPREADSHEET_FORMULAS",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l04.proportional_equations", "supporting.fractions.divide_whole"],
            "G7_U09_L02_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L03: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L03", "proportions.equivalent_ratios",
            "G7_RESTAURANT_FIXED_VARIABLE_COSTS_AND_PROPORTIONALITY",
            ["usccss.g7.u02.l08.classify_equations", "usccss.g7.u02.l04.proportional_equations", "percentages.core.increase_decrease"],
            "G7_U09_L03_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L04: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L04", "geometry.scale.construction",
            "G7_RESTAURANT_SCALE_DRAWING_AREA_AND_LAYOUT",
            ["usccss.g7.u01.l02.scale_factor", "supporting.geometry.compound_area", "usccss.g7.u01.l06.scaled_area_square_factor"],
            "G7_U09_L04_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L05: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L05", "proportions.unit_rate",
            "G7_DOT_HOUSING_AND_POPULATION_DENSITY",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l02.proportional_tables"],
            "G7_U09_L05_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L06: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L06", "proportions.unit_rate",
            "G7_FERMI_ESTIMATION_ASSUMPTIONS_AND_REASONABLE_PRECISION",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l04.proportional_equations", "supporting.reasoning.multistep"],
            "G7_U09_L06_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L07: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L07", "supporting.algebra.expressions",
            "G7_CULMINATING_EXPRESSIONS_EQUATIONS_AND_EQUIVALENT_FORMS",
            ["supporting.algebra.simplify", "supporting.algebra.expand", "supporting.algebra.equations.mixed"],
            "G7_U09_L07_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L08: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L08", "percentages.core",
            "G7_MEASUREMENT_ESTIMATE_MAXIMUM_PERCENT_ERROR",
            ["usccss.g7.u04.percent_error", "usccss.g7.u04.error_interval_bound"],
            "G7_U09_L08_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L09: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L09", "percentages.core",
            "G7_PROPAGATED_MEASUREMENT_ERROR_AREA_AND_VOLUME",
            ["usccss.g7.u04.percent_error", "supporting.geometry.compound_area", "supporting.geometry.cuboid_volume"],
            "G7_U09_L09_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L10: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L10", "proportions.unit_rate",
            "G7_LONG_DISTANCE_MEASUREMENT_METHODS_AND_ERROR",
            ["usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u04.percent_error"],
            "G7_U09_L10_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L11: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L11", "geometry.circles_loci",
            "G7_TRUNDLE_WHEEL_CIRCUMFERENCE_AND_DISTANCE_PER_ROTATION",
            ["supporting.geometry.circle_circumference_pi_coefficient", "usccss.g7.u03.l05.wheel_rotations_distance"],
            "G7_U09_L11_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L12: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L12", "geometry.circles_loci",
            "G7_TRUNDLE_WHEEL_ROTATIONS_TO_LONG_DISTANCE",
            ["usccss.g7.u03.l05.wheel_rotations_distance", "usccss.g7.u02.l03.unit_rates_reciprocal"],
            "G7_U09_L12_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // Grade 7 U09 L13: culminating project Practice reuses prior Grade 7 engines.
        // Spreadsheet work, physical measurement, design, estimation assumptions and written project justification remain teacher reviewed.
        byLesson["PED:US-CCSS-MATH:G7:U09:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G7:U09:L13", "geometry.scale.construction",
            "G7_5K_ROUTE_SCALE_MAP_TRUNDLE_AND_PROPORTIONAL_REASONING",
            ["usccss.g7.u01.l02.scale_factor", "usccss.g7.u02.l03.unit_rates_reciprocal", "usccss.g7.u02.l04.proportional_equations"],
            "G7_U09_L13_CULMINATING_PROJECT_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM 2021 G6 U01:L01: source-specific machine Practice checks tile-area
        // units and structured 2x2 coverage separately; actual drawing,
        // student explanation and the eight-point rubric are teacher reviewed.


        // IM 2021 Grade 6 U01:L01: reuse released two-family tiling route.

        // IM 2021 G6 U01:L01: source-specific machine Practice checks tile-area
        // units and structured 2x2 coverage separately; actual drawing,
        // student explanation and the eight-point rubric are teacher reviewed.

        byLesson["PED:US-CCSS-MATH:G6:U01:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L01",
            "geometry.area_tiling",
            "G6_SOURCE_TILING_TRIANGULAR_UNITS_AND_EXACT_COVERAGE",
            [
                "usccss.g6.u01.l01.triangular_unit_tile_area",
                "usccss.g6.u01.l01.structured_tiling_coverage"
            ],
            "IM2021_G6_U01_L01_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);




        // Original IM 6-8 (OUR/Kendall Hunt, 2017-2019) G6 U01:L02:
        // exact piece areas and numerical area equivalence only. Physical/digital
        // tangram outlines, distinct shapes, no gaps or overlaps, and proofs
        // remain teacher-reviewed rather than falsely marked by the numeric engine.
        byLesson["PED:US-CCSS-MATH:G6:U01:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L02",
            "geometry.area_tiling",
            "G6_SOURCE_TANGRAM_AREA_CONSERVATION_EXACT_NUMERIC_TEACHER_REVIEW",
            [
                "usccss.g6.u01.l02.tangram_piece_area",
                "usccss.g6.u01.l02.tangram_equal_area"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L02_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original IM 6-8 G6 U01:L03: calculate known shaded areas by
        // decomposition, enclosing/subtracting and rearrangement. Original
        // plots, diagrams and justifications are still teacher-reviewed.
        byLesson["PED:US-CCSS-MATH:G6:U01:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L03",
            "geometry.area_tiling",
            "G6_SOURCE_SHADED_AREA_DECOMPOSE_SUBTRACT_REARRANGE_EXACT",
            [
                "usccss.g6.u01.l03.source_shaded_area",
                "usccss.g6.u01.l03.area_conservation"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L03_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original IM 6-8 G6 U01:L04 Parallelograms: classify both
        // opposite parallel side pairs, numeric area via dissection/enclosure,
        // and area invariance under true perpendicular height. Drawings,
        // constructions, distinct decompositions and proofs stay teacher-scored.
        byLesson["PED:US-CCSS-MATH:G6:U01:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L04",
            "geometry.area_tiling",
            "G6_SOURCE_PARALLELOGRAM_CLASSIFY_AREA_REARRANGE_EXACT",
            [
                "usccss.g6.u01.l04.parallelogram_exact_area",
                "usccss.g6.u01.l04.two_pairs_parallel",
                "usccss.g6.u01.l04.equal_area_rearrangement"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L04_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM 6-8 Grade 6 U01:L05: two corresponding base/height
        // pairs, perpendicularity (including outside heights), exact area,
        // scaling and source practice fractional triangle area. The source's
        // drawn heights, algebraic explanations and teacher-only Cool-down
        // cannot be certified by numeric auto-grading.
        byLesson["PED:US-CCSS-MATH:G6:U01:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L05",
            "geometry.area_tiling",
            "G6_SOURCE_BASE_HEIGHT_PERPENDICULAR_AREA_AND_SCALING_EXACT",
            [
                "usccss.g6.u01.l05.parallelogram_base_height_area",
                "usccss.g6.u01.l05.corresponding_height",
                "usccss.g6.u01.l05.perpendicular_height_claim",
                "usccss.g6.u01.l05.area_scaling",
                "usccss.g6.u01.l05.small_triangle_fraction"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L05_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original IM/OUR G6 U01:L06 numerical area, missing corresponding
        // dimensions and equal-area reasoning. Drawings/justifications remain
        // teacher reviewed; the publisher's 6.3 Cool-down remains protected.
        byLesson["PED:US-CCSS-MATH:G6:U01:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L06",
            "geometry.area_tiling",
            "G6_SOURCE_PARALLELOGRAM_AREA_MISSING_DIMENSION_COMPOSITE",
            [
                "usccss.g6.u01.l06.area_missing_dimension",
                "usccss.g6.u01.l06.equal_area_reasoning",
                "usccss.g6.u01.l06.composite_area"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L06_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L07 triangle/diagonal composition; exact
        // fractions and numeric responses only. Drawing and congruence proof
        // are assessed by a teacher using the source-matched rubric.
        byLesson["PED:US-CCSS-MATH:G6:U01:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L07",
            "geometry.area_tiling",
            "G6_SOURCE_TRIANGLE_DIAGONAL_CONGRUENCE_AND_FRACTIONS",
            [
                "usccss.g6.u01.l07.parallelogram_diagonal_half",
                "usccss.g6.u01.l07.original_fraction_dimensions",
                "usccss.g6.u01.l07.triangle_composition_logic"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L07_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original IM/OUR G6 U01:L08: two COMPLETE matching triangles form
        // doubled-area parallelogram; cut pieces of ONE triangle rearrange
        // to a SAME-area parallelogram. Keep these distinct pedagogically.
        byLesson["PED:US-CCSS-MATH:G6:U01:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L08",
            "geometry.area_tiling",
            "G6_SOURCE_TRIANGLE_HALF_AREA_VS_SAME_AREA_REARRANGEMENT",
            [
                "usccss.g6.u01.l08.triangle_half_area",
                "usccss.g6.u01.l08.decomposition_invariance",
                "usccss.g6.u01.l08.original_decimal_practice"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L08_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original IM 6-8 Grade6 U01:L09: reuse the selected L08 triangle
        // formula exact adapter; teacher must review student diagrams,
        // three possible base/height choices, and derivation of one half.
        byLesson["PED:US-CCSS-MATH:G6:U01:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L09",
            "geometry.area_tiling",
            "G6_ORIGINAL_TRIANGLE_AREA_FORMULA_AND_HEIGHT_VALIDITY",
            [
                "usccss.g6.u01.l09.triangle_formula_source_area",
                "usccss.g6.u01.l09.triangle_height_validity"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L09_SOURCE_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L10: reuse the existing triangle-area
        // adapter for source-grid dimensions and perpendicular-height checks.
        // Student construction and reasoning remain teacher-reviewed.
        byLesson["PED:US-CCSS-MATH:G6:U01:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L10",
            "geometry.area_tiling",
            "G6_ORIGINAL_TRIANGLE_PERPENDICULAR_HEIGHT_OUTSIDE_AREA",
            [
                "usccss.g6.u01.l10.triangle_corresponding_height_area",
                "usccss.g6.u01.l10.height_construction_validity"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L10_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L11: classify simple polygons and compute
        // areas of first-edition source quadrilaterals, hexagon, trapezoid,
        // triangle, pinwheel. Drawings/proofs are teacher-assessed.
        byLesson["PED:US-CCSS-MATH:G6:U01:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L11",
            "geometry.area_tiling",
            "G6_SOURCE_POLYGON_CLASSIFICATION_AND_COMPOSITE_AREA",
            [
                "usccss.g6.u01.l11.polygon_classification",
                "usccss.g6.u01.l11.source_polygon_area"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L11_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM Grade6 U01:L12 surface area of rectangular prisms
        // and exposed unit cube faces. User-built figures and reasoning require
        // teacher review; only source numerical / true-false skills auto-check.
        byLesson["PED:US-CCSS-MATH:G6:U01:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L12",
            "geometry.area_tiling",
            "G6_ORIGINAL_EXPOSED_FACE_PRISM_SURFACE_AREA",
            [
                "usccss.g6.u01.l12.prism_exposed_face_area",
                "usccss.g6.u01.l12.surface_area_meaning"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L12_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM Grade6 U01:L13 Polyhedra: existing solid engine
        // now covers source polyhedron classification, nets and face/edge/
        // vertex counts; actual cutting/folding requires teacher review.
        byLesson["PED:US-CCSS-MATH:G6:U01:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L13",
            "geometry.area_tiling",
            "G6_SOURCE_POLYHEDRON_PRISM_PYRAMID_NET_COUNTS",
            [
                "usccss.g6.u01.l13.polyhedron_and_net_logic",
                "usccss.g6.u01.l13.source_faces_edges_vertices"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L13_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L14: reuse the exact solid model from L12/L13
        // for original nets A/B/C and source homework, but teacher marks the
        // actual foldability, adjacency and sketched proof.
        byLesson["PED:US-CCSS-MATH:G6:U01:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L14",
            "geometry.area_tiling",
            "G6_ORIGINAL_POLYHEDRA_NET_FACE_AREA_AND_FOLD_LOGIC",
            [
                "usccss.g6.u01.l14.source_net_surface_area",
                "usccss.g6.u01.l14.net_folding_face_logic"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L14_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L15: original box nets and capacities,
        // Jada's false triangle area, stepped cube faces and correct folding.
        // Reuse prior solid exact-question service. Actual drawn nets teacher marked.
        byLesson["PED:US-CCSS-MATH:G6:U01:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L15",
            "geometry.area_tiling",
            "G6_ORIGINAL_NETS_BOX_SURFACE_VS_VOLUME",
            [
                "usccss.g6.u01.l15.source_box_nets_surface_volume",
                "usccss.g6.u01.l15.net_design_and_volume_reasoning"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L15_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original optional OUR/IM G6 U01:L16: same-volume eight cube
        // constructs and source equal-area 1×11,2×7,3×5 base rectangles.
        // Actual physical arrangements, notebooks and reasons need a teacher.
        byLesson["PED:US-CCSS-MATH:G6:U01:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L16",
            "geometry.area_tiling",
            "G6_ORIGINAL_OPTIONAL_SURFACE_AREA_VS_VOLUME",
            [
                "usccss.g6.u01.l16.source_equal_surface_different_volume",
                "usccss.g6.u01.l16.surface_volume_unit_logic"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L16_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM Grade6 U01:L17 perfect squares/cubes,
        // exponent factors and 32-cube physical model. Exact numeric/boolean
        // grading only; geometric layer construction teacher marked.
        byLesson["PED:US-CCSS-MATH:G6:U01:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L17",
            "indices.integer_power.evaluate",
            "G6_ORIGINAL_SQUARE_AND_CUBE_REPEATED_FACTORS",
            [
                "usccss.g6.u01.l17.original_square_cube_values",
                "usccss.g6.u01.l17.square_cube_exponent_meaning"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L17_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U01:L18 cube surface and volume. Exact fractions
        // in source P1 retain 1/27 without decimal approximation. Teacher
        // reviews six-face foldable nets, symbols and squared/cubic units.
        byLesson["PED:US-CCSS-MATH:G6:U01:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L18",
            "geometry.area_tiling",
            "G6_ORIGINAL_CUBE_NET_SURFACE_AND_VOLUME",
            [
                "usccss.g6.u01.l18.source_cube_surface_and_volume",
                "usccss.g6.u01.l18.cube_net_dimension_and_units"
            ],
            "IM_ORIGINAL_6_8_G6_U01_L18_NUMERIC_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // Grade 6 U01:L19: open-ended student-designed tent capstone.
        // Selected Practice reinforces polygon-area decomposition under named
        // assumptions; the student project, drawing and peer review remain
        // TEACHER_REVIEW_REQUIRED, never certified by a numeric score.
        byLesson["PED:US-CCSS-MATH:G6:U01:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U01:L19",
            "geometry.area_tiling",
            "G6_TENT_CAPSTONE_FLOOR_ROOF_ENDS_AND_DESIGN",
            [
                "usccss.g6.u01.l19.first_party_tent_panel_area",
                "usccss.g6.u01.l19.open_design_reasoning"
            ],
            "G6_U01_L19_OWNER_ORIGINAL_MATH_TEACHER_REVIEW_OPEN_PROJECT",
            "READY_VERIFIED",
            Version);

        // Original OUR/IM G6 U02:L01. Ratios are ordered comparisons of
        // named object categories; the 24-unit two-color model is source skill.
        // Student sorting, diagrams, other-shape shading and explanation are
        // teacher-reviewed; exact Practice checks the typed ratios/counts only.
        byLesson["PED:US-CCSS-MATH:G6:U02:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_ORDERED_CATEGORY_RATIOS_AND_AREA_COLOR_MODEL",
            [
                "usccss.g6.u02.l01.ordered_collection_ratios",
                "usccss.g6.u02.l01.shaded_area_ratio_counts"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 U02:L02 local original diagrams: source snap cubes, 2 cups white
        // paint vs 6 tablespoons blue, recipe card diagrams with ordered 3 terms.
        // Typed ratios are auto-scored, drawing/card matching teacher-scored.
        byLesson["PED:US-CCSS-MATH:G6:U02:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_DISCRETE_TWO_ROW_AND_THREE_INGREDIENT_DIAGRAMS",
            [
                "usccss.g6.u02.l02.labeled_two_row_ratios",
                "usccss.g6.u02.l02.three_ingredient_ratio_cards"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 U02:L03 Recipes: scaling all ingredient quantities by equal
        // factors; numeric practice and taste equivalence; student models and
        // actual taste claims remain teacher-reviewed, not certified by grader.
        byLesson["PED:US-CCSS-MATH:G6:U02:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_RECIPE_BATCHES_AND_EQUAL_TASTE",
            [
                "usccss.g6.u02.l03.recipe_batch_scale",
                "usccss.g6.u02.l03.same_taste_equivalent_ratios"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 U02:L04 Color Mixtures. Original first-edition teacher
        // model uses physical paint/colored water and optional accessible dough.
        // Exact selected math checks quantities and proportionality, not shade.
        byLesson["PED:US-CCSS-MATH:G6:U02:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_COLOR_MIXTURES_SAME_SHADE_AND_SCALE",
            [
                "usccss.g6.u02.l04.color_mixture_quantities",
                "usccss.g6.u02.l04.same_shade_proportions"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 U02:L05 source first-edition equivalent-ratio definitions;
        // numeric selected Practice plus teacher-scored multiple-batch drawings.
        byLesson["PED:US-CCSS-MATH:G6:U02:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_EQUIVALENT_RATIO_DEFINITION_AND_SCALE",
            [
                "usccss.g6.u02.l05.equivalent_ratio_definition",
                "usccss.g6.u02.l05.scaled_ratio_missing_term"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_DOUBLE_NUMBER_LINE_RECIPE_BATCH_PAIRS",
            [
                "usccss.g6.u02.l06.double_number_line_amounts",
                "usccss.g6.u02.l06.double_number_line_structure"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONSTRUCT_LABELED_EQUAL_SPACED_DOUBLE_LINES",
            [
                "usccss.g6.u02.l07.created_double_number_line_values",
                "usccss.g6.u02.l07.double_number_line_units_per"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_UNIT_PRICE_AND_COST_PER_ITEM",
            [
                "usccss.g6.u02.l08.unit_prices_and_counts",
                "usccss.g6.u02.l08.unit_price_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONSTANT_SPEED_DISTANCE_TIME_DOUBLE_LINES",
            [
                "usccss.g6.u02.l09.constant_speed_quantities",
                "usccss.g6.u02.l09.constant_speed_equivalence"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMPARE_EQUAL_RATES_SPEED_COST_RECIPES",
            [
                "usccss.g6.u02.l10.rate_comparison_quantities",
                "usccss.g6.u02.l10.compare_rates_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_EQUIVALENT_RATIO_TABLES",
            [
                "usccss.g6.u02.l11.table_equivalent_ratio_values",
                "usccss.g6.u02.l11.ratio_table_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_RATIO_TABLE_FIND_UNKNOWN",
            [
                "usccss.g6.u02.l12.table_missing_quantity",
                "usccss.g6.u02.l12.table_equivalence_methods"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_TABLE_VS_DOUBLE_NUMBER_LINE_CHOICE",
            [
                "usccss.g6.u02.l13.table_line_ratio_values",
                "usccss.g6.u02.l13.representation_selection"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_WHAT_WHY_INFO_GAP_EQUIVALENT_RATIOS",
            [
                "usccss.g6.u02.l14.info_gap_ratio_quantities",
                "usccss.g6.u02.l14.rate_information_requests"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_TAPE_DIAGRAM_PART_PART_WHOLE",
            [
                "usccss.g6.u02.l15.tape_diagram_part_whole",
                "usccss.g6.u02.l15.part_to_whole_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_CHOOSE_RATIO_DIAGRAM_TABLE_LINE",
            [
                "usccss.g6.u02.l16.multistep_ratio_totals",
                "usccss.g6.u02.l16.ratio_tool_selection"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit2 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U02:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U02:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_OPEN_ENDED_FERMI_PROJECT_WITH_BOUND_ASSUMPTION_PRACTICE",
            [
                "usccss.g6.u02.l17.first_party_fermi_assumption_arithmetic",
                "usccss.g6.u02.l17.first_party_fermi_sensitivity"
            ],
            "OUR_IM_ORIGINAL_G6_U02_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_REAL_WORLD_PER_ONE_RATE_SCALING",
            [
                "usccss.g6.u03.l01.per_one_source_rate",
                "usccss.g6.u03.l01.source_rate_meaning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_BENCHMARK_LENGTH_CAPACITY_MASS_UNITS",
            [
                "usccss.g6.u03.l02.unit_magnitude_and_conversion",
                "usccss.g6.u03.l02.choose_appropriate_unit_category"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_MEASUREMENT_STATIONS_AND_UNIT_SCALE",
            [
                "usccss.g6.u03.l03.measurement_source_quantity",
                "usccss.g6.u03.l03.measurement_attribute_identification"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_CROSS_SYSTEM_UNIT_CONVERSION_RATE_PER_ONE",
            [
                "usccss.g6.u03.l04.source_ratio_unit_conversions",
                "usccss.g6.u03.l04.conversion_direction_and_units"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMPARE_PER_ONE_SPEED_AND_PRICE",
            [
                "usccss.g6.u03.l05.source_speed_price_values",
                "usccss.g6.u03.l05.same_rate_price_speed_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_RECIPROCAL_RATES_PER_ONE",
            [
                "usccss.g6.u03.l06.source_reciprocal_rate_values",
                "usccss.g6.u03.l06.reciprocal_rate_meaning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_EQUIVALENT_RATIO_UNIT_RATE_TABLES",
            [
                "usccss.g6.u03.l07.unit_rate_table_quantities",
                "usccss.g6.u03.l07.unit_rate_equivalence"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONSTANT_SPEED_PACE_DISTANCE_TIME",
            [
                "usccss.g6.u03.l08.speed_distance_time_quantities",
                "usccss.g6.u03.l08.speed_pace_comparison"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_UNSCAFFOLDED_UNIT_RATE_CHOICE",
            [
                "usccss.g6.u03.l09.source_unit_rate_calculations",
                "usccss.g6.u03.l09.solve_rate_comparison"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENT_AS_RATE_PER_ONE_HUNDRED",
            [
                "usccss.g6.u03.l10.percent_coin_values",
                "usccss.g6.u03.l10.percent_comparison_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENTAGES_WITH_DOUBLE_NUMBER_LINES",
            [
                "usccss.g6.u03.l11.percent_scaled_amounts",
                "usccss.g6.u03.l11.percent_of_quantity_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENT_TAPE_PART_TO_WHOLE",
            [
                "usccss.g6.u03.l12.tape_diagram_percent_amounts",
                "usccss.g6.u03.l12.percent_tape_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_BENCHMARK_PERCENT_OF_QUANTITY",
            [
                "usccss.g6.u03.l13.benchmark_percent_values",
                "usccss.g6.u03.l13.benchmark_percent_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENT_FIND_PART_WHOLE_OR_RATE",
            [
                "usccss.g6.u03.l14.percent_unknowns",
                "usccss.g6.u03.l14.percent_compare_and_inverse"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_EFFICIENT_PERCENT_MULTIPLICATION",
            [
                "usccss.g6.u03.l15.percent_of_quantity_exact",
                "usccss.g6.u03.l15.percent_of_quantity_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENT_FROM_PART_DIVIDED_BY_REFERENCE",
            [
                "usccss.g6.u03.l16.find_percent_from_two_amounts",
                "usccss.g6.u03.l16.percent_division_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U03:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U03:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_CAPSTONE_ROOM_AREA_PAINT_PURCHASE_PERCENT_TIME_MODEL",
            [
                "usccss.g6.u03.l17.first_party_bounded_room_paint_math",
                "usccss.g6.u03.l17.first_party_paint_model_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U03_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMPARE_DIVISION_QUOTIENT_MAGNITUDE",
            [
                "usccss.g6.u04.l01.division_and_source_quantities",
                "usccss.g6.u04.l01.predict_quotient_size"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_MISSING_NUMBER_OF_GROUPS_OR_GROUP_SIZE",
            [
                "usccss.g6.u04.l02.division_groups_and_size",
                "usccss.g6.u04.l02.division_interpretations"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_DIVISION_DIAGRAM_EQUATION_CONTEXT",
            [
                "usccss.g6.u04.l03.division_contextual_results",
                "usccss.g6.u04.l03.division_explanation_checks"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_FRACTION_DIVISION_EQUAL_GROUP_PATTERN_BLOCKS",
            [
                "usccss.g6.u04.l04.fraction_groups_exact",
                "usccss.g6.u04.l04.fraction_division_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_NONINTEGER_FRACTION_GROUPS_PATTERN_BLOCK",
            [
                "usccss.g6.u04.l05.fraction_group_quotients",
                "usccss.g6.u04.l05.fractional_group_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_FRACTION_DIVISION_TAPE_DIAGRAMS",
            [
                "usccss.g6.u04.l06.fraction_division_values",
                "usccss.g6.u04.l06.fraction_tape_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_FRACTIONAL_NUMBER_OF_GROUPS_LESS_THAN_ONE",
            [
                "usccss.g6.u04.l07.fraction_of_group_numeric",
                "usccss.g6.u04.l07.fractional_group_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_MISSING_GROUP_SIZE_FRACTION_DIVISION",
            [
                "usccss.g6.u04.l08.fraction_group_size",
                "usccss.g6.u04.l08.fraction_group_size_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_UNSCAFFOLDED_FRACTION_DIVISION_CONTEXT",
            [
                "usccss.g6.u04.l09.contextual_fraction_division",
                "usccss.g6.u04.l09.division_word_model_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_DIVIDE_FRACTION_MULTIPLY_BY_RECIPROCAL",
            [
                "usccss.g6.u04.l10.fraction_division_steps",
                "usccss.g6.u04.l10.division_reciprocal_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_FRACTION_DIVISION_RECIPROCAL_ALGORITHM",
            [
                "usccss.g6.u04.l11.divide_fractions_exact",
                "usccss.g6.u04.l11.fraction_algorithm_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_MULTIPLICATIVE_COMPARISON_FRACTION_LENGTHS",
            [
                "usccss.g6.u04.l12.fractional_length_comparisons",
                "usccss.g6.u04.l12.fractional_comparison_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_FIND_UNKNOWN_RECTANGLE_SIDE_FRACTION_DIVISION",
            [
                "usccss.g6.u04.l13.rectangle_area_fractional_sides",
                "usccss.g6.u04.l13.rectangle_area_diagram_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_TRIANGLE_BASE_HEIGHT_AND_FRACTIONAL_PRISM_VOLUME",
            [
                "usccss.g6.u04.l14.triangle_height_and_cube_counts",
                "usccss.g6.u04.l14.cube_and_triangle_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_VOLUME_PRISM_FRACTIONAL_EDGES",
            [
                "usccss.g6.u04.l15.fractional_prism_volume",
                "usccss.g6.u04.l15.prism_unit_cube_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMBINE_FRACTION_OPERATIONS_CONTEXT",
            [
                "usccss.g6.u04.l16.fraction_operations_context",
                "usccss.g6.u04.l16.fraction_ops_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit3 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U04:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U04:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_CAPSTONE_FRACTIONAL_BOX_PACKING_COST_MODEL",
            [
                "usccss.g6.u04.l17.first_party_fixed_orientation_packing_math",
                "usccss.g6.u04.l17.first_party_orientation_model_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U04_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_BUDGETING_ESTIMATION_AND_EXACT_OPERATIONS",
            [
                "usccss.g6.u05.l01.shopping_decimal_arithmetic",
                "usccss.g6.u05.l01.shopping_reasonableness"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_BASE_TEN_DIAGRAM_AND_VERTICAL_DECIMALS",
            [
                "usccss.g6.u05.l02.base_ten_decimal_arithmetic",
                "usccss.g6.u05.l02.align_decimal_place_values"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_REGROUPING_ADD_SUBTRACT",
            [
                "usccss.g6.u05.l03.decimal_sum_difference_results",
                "usccss.g6.u05.l03.placevalue_borrowing_correctness"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_ALIGNMENT_MANY_NONZERO_DIGITS",
            [
                "usccss.g6.u05.l04.precise_decimal_add_subtract",
                "usccss.g6.u05.l04.placevalue_high_precision_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_MULTIPLICATION_POWERS_OF_TEN",
            [
                "usccss.g6.u05.l05.decimal_products_base_ten",
                "usccss.g6.u05.l05.decimal_product_placevalue_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_PRODUCTS_FRACTIONS_AREA_DIAGRAMS",
            [
                "usccss.g6.u05.l06.decimal_multiplication_methods",
                "usccss.g6.u05.l06.decimal_product_scale_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_AREA_PARTIAL_PRODUCTS",
            [
                "usccss.g6.u05.l07.decimal_partial_product_values",
                "usccss.g6.u05.l07.partial_product_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_DECIMAL_MULTIPLICATION_ALGORITHM_AND_PARTIAL_AREAS",
            [
                "usccss.g6.u05.l08.multiplication_calculation_values",
                "usccss.g6.u05.l08.decimal_product_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_BASE_TEN_PARTIAL_QUOTIENTS",
            [
                "usccss.g6.u05.l09.partial_quotient_results",
                "usccss.g6.u05.l09.partial_quotient_principles"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_LONG_DIVISION_PLACE_VALUE_DIGITS",
            [
                "usccss.g6.u05.l10.long_division_quotient_results",
                "usccss.g6.u05.l10.long_division_placevalue_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_TERMINATING_DECIMAL_QUOTIENT_FROM_WHOLE_DIVISION",
            [
                "usccss.g6.u05.l11.whole_division_decimal_results",
                "usccss.g6.u05.l11.decimal_quotient_placevalue_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_DIVIDE_DECIMAL_BY_WHOLE_AND_SCALE_BOTH",
            [
                "usccss.g6.u05.l12.decimal_divided_by_whole",
                "usccss.g6.u05.l12.division_scale_equivalence"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_SCALE_DECIMAL_DIVISION_BY_TEN_POWERS",
            [
                "usccss.g6.u05.l13.decimal_divisor_quotients",
                "usccss.g6.u05.l13.decimal_divisor_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONTEXTUAL_DECIMAL_OPERATIONS_ESTIMATES",
            [
                "usccss.g6.u05.l14.decimal_context_calculations",
                "usccss.g6.u05.l14.decimal_application_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit5 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U05:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U05:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_OPTIONAL_DECIMAL_MEASUREMENT_ORIGAMI_CAPSTONE",
            [
                "usccss.g6.u05.l15.first_party_fixed_paper_box_scaling",
                "usccss.g6.u05.l15.measurement_precision_and_area_scaling"
            ],
            "OUR_IM_ORIGINAL_G6_U05_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_TAPE_DIAGRAM_ADDITIVE_MULTIPLICATIVE_EQUATIONS",
            [
                "usccss.g6.u06.l01.tape_equation_unknown_values",
                "usccss.g6.u06.l01.tape_diagram_equation_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_EQUATION_TRUTH_VARIABLE_SUBSTITUTION",
            [
                "usccss.g6.u06.l02.variable_equation_solutions",
                "usccss.g6.u06.l02.equation_truth_and_model_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_BALANCED_HANGER_EQUAL_OPS_BOTH_SIDES",
            [
                "usccss.g6.u06.l03.hanger_equation_unknowns",
                "usccss.g6.u06.l03.balance_equation_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_FLEXIBLE_EQUATION_REPRESENTATION_AND_SOLUTION",
            [
                "usccss.g6.u06.l04.equation_context_solutions",
                "usccss.g6.u06.l04.equation_choice_and_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_GENERAL_FRACTION_BAR_DIVISION_OF_RATIONALS",
            [
                "usccss.g6.u06.l05.rational_fraction_bar_division",
                "usccss.g6.u06.l05.fraction_bar_meaning_and_solutions"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_VARIABLE_EXPRESSIONS_FROM_RELATIONSHIPS",
            [
                "usccss.g6.u06.l06.expression_evaluation_contexts",
                "usccss.g6.u06.l06.expression_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_PERCENT_OF_UNKNOWN_EQUATIONS",
            [
                "usccss.g6.u06.l07.percent_equation_unknown_whole",
                "usccss.g6.u06.l07.percent_equation_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_EQUIVALENCE_FOR_EVERY_VARIABLE_VALUE",
            [
                "usccss.g6.u06.l08.equivalent_expression_evaluation",
                "usccss.g6.u06.l08.equivalent_for_all_values_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_DISTRIBUTIVE_PROPERTY_RECTANGLE_AREA_NUMERIC",
            [
                "usccss.g6.u06.l09.distributive_numeric_products",
                "usccss.g6.u06.l09.distributive_area_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_VARIABLE_DISTRIBUTION_RECTANGLE_AREAS",
            [
                "usccss.g6.u06.l10.variable_distributive_values",
                "usccss.g6.u06.l10.variable_distributive_equivalence"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_OPTIONAL_DISTRIBUTIVE_EXPAND_FACTOR_EQUIVALENCE",
            [
                "usccss.g6.u06.l11.expansion_factor_values",
                "usccss.g6.u06.l11.distributive_equivalence_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_EXPONENT_NOTATION_REPEATED_MULTIPLICATION",
            [
                "usccss.g6.u06.l12.exponents_numeric_values",
                "usccss.g6.u06.l12.exponent_and_distributive_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_EXPONENTS_RATIONAL_BASES_AND_TRUTH",
            [
                "usccss.g6.u06.l13.exponents_with_rational_bases",
                "usccss.g6.u06.l13.exponent_equation_truth"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_EXPONENTS_EVALUATE_BEFORE_MULTIPLICATION",
            [
                "usccss.g6.u06.l14.exponent_order_numeric",
                "usccss.g6.u06.l14.exponent_evaluation_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_EXPONENT_WITH_VARIABLE_EVALUATION",
            [
                "usccss.g6.u06.l15.exponential_variable_values",
                "usccss.g6.u06.l15.exponential_expression_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_PROPORTIONAL_TWO_VARIABLE_TABLE_GRAPH_EQUATION",
            [
                "usccss.g6.u06.l16.related_quantities_numeric",
                "usccss.g6.u06.l16.two_variable_equation_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONSTANT_SPEED_TWO_VARIABLE_EQUATIONS",
            [
                "usccss.g6.u06.l17.constant_speed_numeric",
                "usccss.g6.u06.l17.constant_speed_equation_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson18; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L18",
            "ratio.unit_rate",
            "G6_ORIGINAL_OPTIONAL_INVERSE_AREA_VOLUME_EXPONENTIAL_RELATIONS",
            [
                "usccss.g6.u06.l18.related_area_volume_growth",
                "usccss.g6.u06.l18.inverse_and_exponential_relationships"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L18_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit6 Lesson19; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U06:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U06:L19",
            "ratio.unit_rate",
            "G6_ORIGINAL_CAPSTONE_COMPARE_EQUATIONS_TABLES_AND_GRAPHS",
            [
                "usccss.g6.u06.l19.first_party_equation_table_examples",
                "usccss.g6.u06.l19.first_party_relationship_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U06_L19_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_SIGNED_TEMPERATURE_ELEVATION_NUMBER_LINE",
            [
                "usccss.g6.u07.l01.signed_temperatures_elevation",
                "usccss.g6.u07.l01.signed_number_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_SIGNED_RATIONAL_NUMBER_OPPOSITES",
            [
                "usccss.g6.u07.l02.signed_opposites_values",
                "usccss.g6.u07.l02.opposite_number_line_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_SIGNED_RATIONAL_INEQUALITY_NUMBER_LINE",
            [
                "usccss.g6.u07.l03.signed_order_comparisons",
                "usccss.g6.u07.l03.signed_inequality_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_ORDER_SIGNED_RATIONALS_AND_OPPOSITES",
            [
                "usccss.g6.u07.l04.order_signed_numbers_source",
                "usccss.g6.u07.l04.signed_ordering_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_SIGNED_CHANGE_MONEY_INVENTORY_TEMPERATURE",
            [
                "usccss.g6.u07.l05.positive_negative_changes",
                "usccss.g6.u07.l05.signed_context_representations"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_ABSOLUTE_VALUE_DISTANCE_FROM_ZERO",
            [
                "usccss.g6.u07.l06.absolute_distance_values",
                "usccss.g6.u07.l06.absolute_value_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_ORDER_VS_ABSOLUTE_MAGNITUDE",
            [
                "usccss.g6.u07.l07.signed_comparisons_absolute",
                "usccss.g6.u07.l07.order_and_absolute_value_inequalities"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_INEQUALITY_OPEN_CLOSED_BOUNDS_CONTEXT",
            [
                "usccss.g6.u07.l08.inequality_bound_examples",
                "usccss.g6.u07.l08.inequality_constraints_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_INEQUALITY_SOLUTION_SETS_CONTEXT",
            [
                "usccss.g6.u07.l09.inequality_solution_values",
                "usccss.g6.u07.l09.inequality_solution_set_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONTEXTUAL_INEQUALITY_DISCRETE_AND_RELATIONAL",
            [
                "usccss.g6.u07.l10.inequality_context_solutions",
                "usccss.g6.u07.l10.inequality_context_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_COORDINATE_PLOT_ALL_FOUR_QUADRANTS",
            [
                "usccss.g6.u07.l11.coordinates_and_axis_locations",
                "usccss.g6.u07.l11.coordinate_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_FOUR_QUADRANT_AXES_SCALE_SELECTION",
            [
                "usccss.g6.u07.l12.four_quadrant_plot_and_axis_scale",
                "usccss.g6.u07.l12.axis_precision_and_plot_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_CONTEXTUAL_COORDINATE_PLANE_QUADRANTS_AND_SCALES",
            [
                "usccss.g6.u07.l13.coordinate_context_numeric",
                "usccss.g6.u07.l13.interpret_axes_and_extrema"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_AXIS_ALIGNED_DISTANCE_COORDINATES",
            [
                "usccss.g6.u07.l14.horizontal_vertical_coordinate_distances",
                "usccss.g6.u07.l14.distance_same_axis_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_COORDINATE_POLYGON_PERIMETER",
            [
                "usccss.g6.u07.l15.polygon_perimeter_signed_coordinates",
                "usccss.g6.u07.l15.polygon_coordinate_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_GREATEST_COMMON_FACTOR_EQUAL_GROUPS",
            [
                "usccss.g6.u07.l16.gcf_source_numbers_and_groups",
                "usccss.g6.u07.l16.factor_context_and_sign_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_LEAST_COMMON_MULTIPLE_REPEATING_CYCLES",
            [
                "usccss.g6.u07.l17.least_common_multiple_source_cycles",
                "usccss.g6.u07.l17.multiple_cycle_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson18; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L18",
            "ratio.unit_rate",
            "G6_ORIGINAL_CHOOSE_GCF_VERSUS_LCM_IN_CONTEXT",
            [
                "usccss.g6.u07.l18.gcf_lcm_context_problems",
                "usccss.g6.u07.l18.factor_multiple_selection_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L18_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit7 Lesson19; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U07:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U07:L19",
            "ratio.unit_rate",
            "G6_ORIGINAL_COORDINATE_IMAGE_CAPSTONE_TEACHER_SCORED",
            [
                "usccss.g6.u07.l19.first_party_plotting_coordinates",
                "usccss.g6.u07.l19.first_party_picture_precision_rules"
            ],
            "OUR_IM_ORIGINAL_G6_U07_L19_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_CATEGORICAL_VERSUS_NUMERICAL_SURVEY_DATA",
            [
                "usccss.g6.u08.l01.numerical_vs_categorical_sources",
                "usccss.g6.u08.l01.data_type_and_dotplot_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_STATISTICAL_QUESTIONS_EXPECTED_VARIABILITY",
            [
                "usccss.g6.u08.l02.statistical_question_classification",
                "usccss.g6.u08.l02.expected_data_variation_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_FREQUENCY_TABLE_TO_DOT_PLOT",
            [
                "usccss.g6.u08.l03.dotplot_frequency_counts",
                "usccss.g6.u08.l03.frequency_plot_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_DOTPLOT_TYPICAL_VALUE_AND_SPREAD",
            [
                "usccss.g6.u08.l04.source_dotplot_comparisons",
                "usccss.g6.u08.l04.dotplot_center_spread_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_DOTPLOT_CONTEXT_COMPARE_CENTER_SPREAD",
            [
                "usccss.g6.u08.l05.dotplot_source_comparisons",
                "usccss.g6.u08.l05.dotplot_center_spread_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_HISTOGRAM_GROUPED_FREQUENCY_BINS",
            [
                "usccss.g6.u08.l06.histogram_bin_frequencies",
                "usccss.g6.u08.l06.histogram_vs_dotplot_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson7; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L07",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMPARE_HISTOGRAM_SHAPE_CENTER_SPREAD",
            [
                "usccss.g6.u08.l07.histogram_center_spread_numeric",
                "usccss.g6.u08.l07.center_spread_and_inequality_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L07_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson8; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L08",
            "ratio.unit_rate",
            "G6_ORIGINAL_HISTOGRAM_SHAPE_SYMMETRY_GAPS_PEAKS",
            [
                "usccss.g6.u08.l08.histogram_shape_and_cumulative",
                "usccss.g6.u08.l08.histogram_shape_explanations"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L08_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson9; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L09",
            "ratio.unit_rate",
            "G6_ORIGINAL_MEAN_AS_EQUAL_REDISTRIBUTION",
            [
                "usccss.g6.u08.l09.mean_and_balanced_total",
                "usccss.g6.u08.l09.mean_fair_share_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L09_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson10; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L10",
            "ratio.unit_rate",
            "G6_ORIGINAL_MEAN_BALANCE_POSITIVE_NEGATIVE_DEVIATIONS",
            [
                "usccss.g6.u08.l10.mean_balance_source_data",
                "usccss.g6.u08.l10.balance_point_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L10_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson11; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L11",
            "ratio.unit_rate",
            "G6_ORIGINAL_MAD_AVERAGE_ABSOLUTE_DISTANCE_FROM_MEAN",
            [
                "usccss.g6.u08.l11.mean_absolute_deviation_amounts",
                "usccss.g6.u08.l11.mad_interpretation_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L11_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson12; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L12",
            "ratio.unit_rate",
            "G6_ORIGINAL_MEAN_MAD_COMPARE_CONTEXTUAL_DOT_PLOTS",
            [
                "usccss.g6.u08.l12.mean_mad_source_datasets",
                "usccss.g6.u08.l12.mean_mad_spread_context"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L12_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson13; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L13",
            "ratio.unit_rate",
            "G6_ORIGINAL_MEDIAN_ODD_EVEN_DATASET_CENTER",
            [
                "usccss.g6.u08.l13.median_sorted_samples",
                "usccss.g6.u08.l13.median_interpretations"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L13_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson14; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L14",
            "ratio.unit_rate",
            "G6_ORIGINAL_CHOOSE_MEAN_VERSUS_MEDIAN_CONTEXT_OUTLIERS",
            [
                "usccss.g6.u08.l14.mean_median_outlier_changes",
                "usccss.g6.u08.l14.mean_median_choice_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L14_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson15; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L15",
            "ratio.unit_rate",
            "G6_ORIGINAL_QUARTILES_MIDDLE_HALF_RANGE_AND_IQR",
            [
                "usccss.g6.u08.l15.quartiles_five_number_summary",
                "usccss.g6.u08.l15.quartile_spread_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L15_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson16; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L16",
            "ratio.unit_rate",
            "G6_ORIGINAL_BOXPLOT_MINIMUM_QUARTILES_MEDIAN_MAXIMUM",
            [
                "usccss.g6.u08.l16.box_plot_source_quartiles",
                "usccss.g6.u08.l16.five_number_box_plot_interpretation"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L16_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson17; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L17",
            "ratio.unit_rate",
            "G6_ORIGINAL_COMPARE_BOXPLOTS_MEDIAN_IQR",
            [
                "usccss.g6.u08.l17.boxplot_athlete_pineapple_statistics",
                "usccss.g6.u08.l17.interpret_center_and_spread_boxplots"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L17_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit8 Lesson18; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U08:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U08:L18",
            "ratio.unit_rate",
            "G6_ORIGINAL_CAPSTONE_SELECT_MEASURE_CENTER_SPREAD_AND_GRAPH",
            [
                "usccss.g6.u08.l18.first_party_statistics_context",
                "usccss.g6.u08.l18.first_party_statistical_report_selection"
            ],
            "OUR_IM_ORIGINAL_G6_U08_L18_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson1; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L01",
            "ratio.unit_rate",
            "G6_ORIGINAL_FERMI_ESTIMATES_RATES_GEOMETRY_ASSUMPTIONS",
            [
                "usccss.g6.u09.l01.first_party_fermi_source_rates",
                "usccss.g6.u09.l01.first_party_fermi_modeling_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L01_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson2; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L02",
            "ratio.unit_rate",
            "G6_ORIGINAL_POPULATION_CLASS_RATIO_PERCENT_MODELING",
            [
                "usccss.g6.u09.l02.first_party_population_ratio_quantities",
                "usccss.g6.u09.l02.first_party_population_model_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L02_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson3; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L03",
            "ratio.unit_rate",
            "G6_ORIGINAL_EUCLIDEAN_SQUARE_TILING_GCF_FRACTION_CONNECTIONS",
            [
                "usccss.g6.u09.l03.first_party_rectangle_square_decomposition",
                "usccss.g6.u09.l03.first_party_square_tiling_fraction_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L03_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson4; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L04",
            "ratio.unit_rate",
            "G6_ORIGINAL_TWO_CHOICE_VOTING_RATIO_SUPERMAJORITY_SAMPLING",
            [
                "usccss.g6.u09.l04.first_party_two_choice_voting_math",
                "usccss.g6.u09.l04.first_party_supermajority_and_sampling_logic"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L04_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson5; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L05",
            "ratio.unit_rate",
            "G6_ORIGINAL_PLURALITY_RUNOFF_RANKED_SCORE_COMPARISON",
            [
                "usccss.g6.u09.l05.first_party_multichoice_voting_arithmetic",
                "usccss.g6.u09.l05.first_party_multichoice_election_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L05_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // G6 original first edition Unit9 Lesson6; exact math only.
        // Student diagrams and contextual explanations require teacher assessment.
        byLesson["PED:US-CCSS-MATH:G6:U09:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G6:U09:L06",
            "ratio.unit_rate",
            "G6_ORIGINAL_OPTIONAL_PROPORTIONAL_REPRESENTATION_FAIR_ALLOCATION",
            [
                "usccss.g6.u09.l06.first_party_proportional_allocation_math",
                "usccss.g6.u09.l06.first_party_representatives_fairness_reasoning"
            ],
            "OUR_IM_ORIGINAL_G6_U09_L06_FIRST_PARTY_COOLDOWN_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);


        // IM Algebra 1 U01:L01 source P3: exactly six first-party choices;
        // unordered three-choice response set. Statistics explanation, class
        // survey, P1/P2 and protected Cool-down remain TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L01",
            "supporting.statistics.statistical_questions",
            "ALG1_SOURCE_SIX_CHOICE_STATISTICAL_SET_P3",
            ["usccss.alg1.u01.l01.statistical_questions_p3"],
            "IM2021_ALG1_U01_L01_EXACT_P3_SET_PLUS_HUMAN_REVIEW",
            "READY_VERIFIED",
            Version);

        // ALG1 U01:L02: exact numerical components of first-party data displays.
        // Student-created graphs and written interpretation remain teacher-scored.
        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L02",
            "supporting.statistics.statistical_questions",
            "ALG1_DATA_REPRESENTATIONS_NUMERIC_COMPONENTS_PLUS_HUMAN_GRAPH_REVIEW",
            [
                "usccss.alg1.u01.l02.dot_frequency",
                "usccss.alg1.u01.l02.histogram_half_open_bin_count",
                "usccss.alg1.u01.l02.five_number_component"
            ],
            "IM_ALG1_U01_L02_INDEPENDENT_EQUIVALENT_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // ALG1 U01:L03: reuse proven center/spread exact families for the
        // objectively gradable components; creation/critique of displays stays human.
        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L03",
            "statistics.center_spread",
            "ALG1_GALLERY_CENTER_SPREAD_PLUS_HUMAN_DISPLAY_CRITIQUE",
            [
                "statistics.center_spread.median",
                "statistics.center_spread.range"
            ],
            "IM_ALG1_U01_L03_FIRST_PARTY_GALLERY_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L04",
            "statistics.distribution_shape",
            "ALG1_DISTRIBUTION_SHAPE_CLASSIFICATION_PLUS_HUMAN_CONTEXT_REASONING",
            ["statistics.distribution_shape.classify"],
            "IM_ALG1_U01_L04_FIRST_PARTY_FREQUENCY_PATTERNS_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L05",
            "statistics.center_spread",
            "ALG1_CENTER_VARIABILITY_EXACT_COMPONENTS_PLUS_HUMAN_INTERPRETATION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.center_spread.iqr",
                "statistics.center_spread.mad"
            ],
            "IM_ALG1_U01_L05_OPTIONAL_REVIEW_CENTER_SPREAD_PLUS_TEACHER_INTERPRETATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L06",
            "spreadsheet.formula_reasoning",
            "ALG1_CELL_REFERENCE_FORMULA_EVALUATION_PLUS_HUMAN_SPREADSHEET_EXPLORATION",
            ["spreadsheet.cell_reference.evaluate"],
            "IM_ALG1_U01_L06_OPTIONAL_SPREADSHEET_RULE_DISCOVERY_PLUS_TEACHER_SOFTWARE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L07",
            "spreadsheet.formula_reasoning",
            "ALG1_CONTEXTUAL_SPREADSHEET_CALCULATION_PLUS_HUMAN_MODEL_SELECTION",
            ["spreadsheet.cell_reference.evaluate"],
            "IM_ALG1_U01_L07_OPTIONAL_SPREADSHEET_CALCULATOR_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L08",
            "spreadsheet.recursive_reasoning",
            "ALG1_RELATIVE_FILL_RECURSIVE_VALUE_PLUS_HUMAN_SPREADSHEET_SHORTCUT",
            ["spreadsheet.recursive_fill.evaluate"],
            "IM_ALG1_U01_L08_OPTIONAL_RELATIVE_FILL_RECURRENCE_PLUS_TEACHER_SOFTWARE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L09",
            "statistics.data_representation",
            "ALG1_TECHNOLOGY_STATS_AND_BIN_COUNTS_PLUS_HUMAN_GRAPH_CREATION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.center_spread.iqr",
                "statistics.histogram.half_open_bin_count"
            ],
            "IM_ALG1_U01_L09_TECH_GRAPHING_NUMERIC_COMPONENTS_PLUS_TEACHER_SOFTWARE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L10",
            "statistics.extreme_value_effect",
            "ALG1_EXTREME_EFFECT_ON_CENTER_PLUS_HUMAN_ROBUSTNESS_REASONING",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.extreme_value.mean_after_replacement"
            ],
            "IM_ALG1_U01_L10_FIRST_PARTY_EXTREME_DATA_PLUS_TEACHER_CONSTRUCTION_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L11",
            "statistics.center_spread",
            "ALG1_VARIABILITY_COMPARISON_MAD_IQR_PLUS_HUMAN_DISPLAY_REASONING",
            [
                "statistics.center_spread.mad",
                "statistics.center_spread.iqr"
            ],
            "IM_ALG1_U01_L11_FIRST_PARTY_EQUAL_CENTER_DIFFERENT_SPREAD_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L12",
            "statistics.standard_deviation",
            "ALG1_POPULATION_SD_EXACT_PLUS_HUMAN_TECHNOLOGY_INTERPRETATION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.population_sd"
            ],
            "IM_ALG1_U01_L12_POPULATION_SD_N_DENOMINATOR_PLUS_TEACHER_TECH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L13",
            "statistics.standard_deviation",
            "ALG1_CONTEXTUAL_SD_COMPARISON_PLUS_HUMAN_INFORMATION_GAP",
            ["statistics.center_spread.population_sd"],
            "IM_ALG1_U01_L13_CONTEXTUAL_SD_INFORMATION_GAP_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L14",
            "statistics.outliers",
            "ALG1_IQR_OUTLIER_DETECTION_PLUS_HUMAN_DATA_QUALITY_DECISION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.center_spread.iqr",
                "statistics.outliers.iqr_fence",
                "statistics.outliers.classify_iqr_flag"
            ],
            "IM_ALG1_U01_L14_OUTLIER_FENCES_EFFECTS_PLUS_TEACHER_RETAIN_EXCLUDE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L15",
            "statistics.compare_distributions",
            "ALG1_COMPARE_CENTER_SD_IQR_PLUS_HUMAN_DECISION_CRITERION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.center_spread.iqr",
                "statistics.center_spread.population_sd"
            ],
            "IM_ALG1_U01_L15_FIRST_PARTY_EQUAL_CENTER_DIFFERENT_VARIABILITY_PLUS_TEACHER_DECISION_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L16",
            "statistics.experimental_analysis",
            "ALG1_TWO_CONDITION_SUMMARIES_PLUS_HUMAN_EXPERIMENT_DESIGN_COLLECTION",
            [
                "statistics.center_spread.mean",
                "statistics.center_spread.median",
                "statistics.center_spread.mad",
                "statistics.center_spread.iqr"
            ],
            "IM_ALG1_U01_L16_RULER_DROP_SUMMARIES_PLUS_TEACHER_AUTHENTIC_EXPERIMENT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L01",
            "algebra.modeling.constraints",
            "ALG1_CONTEXT_CONSTRAINT_FEASIBILITY_PLUS_HUMAN_MODEL_AUTHORING",
            ["algebra.modeling.constraints.feasible_pair"],
            "IM_ALG1_U02_L01_FIRST_PARTY_PIZZA_STYLE_CONSTRAINTS_PLUS_TEACHER_MODEL_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L02",
            "algebra.modeling.linear_relationship",
            "ALG1_CONTEXT_TO_LINEAR_EQUATION_PLUS_HUMAN_CONSTANT_VARIABLE_REASONING",
            ["algebra.modeling.linear_relationship.select_equation"],
            "IM_ALG1_U02_L02_FIRST_PARTY_CONTEXT_EQUATION_PLUS_TEACHER_QUANTITY_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L03",
            "algebra.modeling.patterns",
            "ALG1_TABLE_PATTERN_CLASSIFICATION_PLUS_HUMAN_EQUATION_GENERALIZATION",
            ["algebra.modeling.pattern.classify_relation"],
            "IM_ALG1_U02_L03_FIRST_PARTY_TABLE_PATTERNS_PLUS_TEACHER_GENERALIZATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L04",
            "algebra.equations.solutions",
            "ALG1_SOLUTION_SUBSTITUTION_CHECKS_PLUS_HUMAN_CONTEXT_INTERPRETATION",
            [
                "algebra.equation.one_variable.solution_check",
                "algebra.equation.two_variable.solution_check"
            ],
            "IM_ALG1_U02_L04_FIRST_PARTY_SOLUTION_CHECKS_PLUS_TEACHER_INTERPRETATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L05",
            "algebra.equations.graphs",
            "ALG1_GRAPH_POINT_SOLUTION_CHECK_PLUS_HUMAN_GRAPH_CREATION",
            ["algebra.equation.two_variable.solution_check"],
            "IM_ALG1_U02_L05_FIRST_PARTY_POINT_ON_GRAPH_EQUIVALENCE_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L06",
            "algebra.equations.equivalence",
            "ALG1_EQUIVALENT_EQUATION_CLASSIFICATION_PLUS_HUMAN_OPERATION_JUSTIFICATION",
            ["algebra.equations.equivalence.classify"],
            "IM_ALG1_U02_L06_FIRST_PARTY_EQUIVALENT_EQUATIONS_PLUS_TEACHER_JUSTIFICATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L07",
            "algebra.equations.rewriting",
            "ALG1_REWRITE_STEP_VALIDITY_PLUS_HUMAN_EXPLANATION",
            [
                "algebra.equations.equivalence.classify",
                "algebra.equation.one_variable.solution_check"
            ],
            "IM_ALG1_U02_L07_VALID_INVALID_REWRITES_PLUS_TEACHER_REASONING",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L08",
            "algebra.literal_equations.isolate",
            "ALG1_LITERAL_EQUATION_TARGET_ISOLATION_PLUS_HUMAN_DERIVATION",
            ["algebra.literal_equation.isolate.select_form"],
            "IM_ALG1_U02_L08_FIRST_PARTY_TARGET_VARIABLE_REWRITES_PLUS_TEACHER_DERIVATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L09",
            "algebra.literal_equations.isolate",
            "ALG1_CONTEXTUAL_TARGET_VARIABLE_ISOLATION_PLUS_HUMAN_TOOL_INTERPRETATION",
            ["algebra.literal_equation.isolate.select_form"],
            "IM_ALG1_U02_L09_CONTEXTUAL_REWRITES_PLUS_TEACHER_TECHNOLOGY_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L10",
            "algebra.linear.graph_connections",
            "ALG1_STANDARD_FORM_TO_SLOPE_INTERCEPT_PLUS_HUMAN_GRAPH_CONTEXT",
            [
                "algebra.linear.standard_form.slope_intercept",
                "algebra.equation.two_variable.solution_check"
            ],
            "IM_ALG1_U02_L10_STANDARD_FORM_INTERCEPTS_SLOPE_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L11",
            "algebra.linear.graph_connections",
            "ALG1_REWRITE_TO_SLOPE_INTERCEPT_PLUS_HUMAN_GRAPH_INTERPRETATION",
            [
                "algebra.linear.standard_form.slope_intercept",
                "algebra.equation.two_variable.solution_check"
            ],
            "IM_ALG1_U02_L11_SLOPE_INTERCEPT_REWRITE_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L12",
            "algebra.linear.systems.two_by_two.solve",
            "ALG1_SYSTEM_INTERSECTION_EXACT_PAIR_PLUS_TEACHER_GRAPH_MODEL",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "ALG1_U02_L12_REUSE_SYSTEM_SKILL_PLUS_TEACHER_MODEL_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L01",
            "functions.relations.definition",
            "ALG1_UNIQUE_OUTPUT_CLASSIFICATION_PLUS_HUMAN_GRAPHS",
            ["functions.relations.unique_output.classify"],
            "ALG1_U04_L01_FUNCTION_DEFINITION_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L02",
            "functions.notation.interpret",
            "ALG1_FUNCTION_NOTATION_EVALUATION_PLUS_TEACHER_CONTEXT",
            ["functions.notation.evaluate_context"],
            "ALG1_U04_L02_NOTATION_INPUT_OUTPUT_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L03",
            "functions.notation.interpret",
            "ALG1_FUNCTION_NOTATION_GRAPH_CONNECTIONS_PLUS_TEACHER_SKETCH",
            ["functions.notation.evaluate_context"],
            "ALG1_U04_L03_NOTATION_GRAPH_CONNECTIONS_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L04",
            "functions.rules.linear",
            "ALG1_FUNCTION_RULE_EVALUATE_AND_REVERSE_PLUS_TEACHER_REPRESENTATIONS",
            ["functions.notation.evaluate_context", "algebra.linear.ax_plus_b_equals_c"],
            "ALG1_U04_L04_RULE_EVALUATE_SOLVE_PLUS_TEACHER_GRAPH_TABLE",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L05",
            "functions.linear.evaluate_and_solve",
            "ALG1_LINEAR_FUNCTION_EVALUATE_SOLVE_PLUS_TEACHER_GRAPH_TECH",
            ["functions.notation.evaluate_context", "algebra.linear.ax_plus_b_equals_c"],
            "ALG1_U04_L05_LINEAR_EVALUATE_SOLVE_PLUS_TEACHER_TECHNOLOGY_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L06",
            "functions.graph.key_features",
            "ALG1_GRAPH_FEATURES_INTERCEPT_EXACT_PLUS_TEACHER_EXTREMA_INTERVALS",
            ["geometry.coordinate.y_intercept_from_rule"],
            "ALG1_U04_L06_INTERCEPT_PLUS_TEACHER_GRAPH_FEATURE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L07",
            "functions.average_rate_of_change",
            "ALG1_AVERAGE_RATE_EXACT_SLOPE_PLUS_TEACHER_CONTEXT",
            ["geometry.coordinate.gradient_between_points"],
            "ALG1_U04_L07_AVERAGE_RATE_FROM_POINTS_PLUS_TEACHER_UNITS",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L08",
            "functions.graph.model_and_interpret",
            "ALG1_GRAPH_MODEL_PLUS_AVERAGE_RATE_EXACT_AND_TEACHER_SKETCH",
            ["geometry.coordinate.gradient_between_points"],
            "ALG1_U04_L08_RATE_PLUS_TEACHER_GRAPH_MODEL_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L09",
            "functions.compare.intersection_and_rate",
            "ALG1_COMPARE_FUNCTIONS_INTERSECTION_RATE_PLUS_TEACHER_CONTEXT",
            ["supporting.coordinate.line_intersection", "geometry.coordinate.gradient_between_points"],
            "ALG1_U04_L09_INTERSECTION_RATE_PLUS_TEACHER_GRAPH_COMPARISON",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L10",
            "functions.domain_range.context",
            "ALG1_DOMAIN_RANGE_CONTEXT_BOUNDS_PLUS_TEACHER_DISCRETE_CONTINUOUS",
            ["functions.domain_range.context_bounds"],
            "ALG1_U04_L10_DOMAIN_RANGE_BOUNDS_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L11",
            "functions.domain_range.graph",
            "ALG1_DOMAIN_RANGE_BOUNDS_PLUS_TEACHER_GRAPH_ENDPOINTS",
            ["functions.domain_range.context_bounds"],
            "ALG1_U04_L11_DOMAIN_RANGE_PLUS_TEACHER_ENDPOINT_HOLE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L12",
            "functions.piecewise.interpret",
            "ALG1_PIECEWISE_INTERVAL_EVALUATION_PLUS_TEACHER_ENDPOINT_GRAPH",
            ["functions.piecewise.evaluate_interval"],
            "ALG1_U04_L12_PIECEWISE_RULE_SELECTION_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L13",
            "functions.absolute_value.error",
            "ALG1_ABSOLUTE_ERROR_EXACT_PLUS_TEACHER_SCATTER_PATTERN",
            ["functions.absolute_error.from_target"],
            "ALG1_U04_L13_ABSOLUTE_ERROR_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L14",
            "functions.absolute_value.translate",
            "ALG1_ABSOLUTE_VALUE_TRANSLATED_EVALUATION_PLUS_TEACHER_GRAPH",
            ["functions.absolute_value.translate_evaluate", "functions.absolute_error.from_target"],
            "ALG1_U04_L14_ABSOLUTE_DISTANCE_TRANSLATION_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15",
            "functions.inverse.composition",
            "ALG1_INVERSE_COMPOSITION_TABLE_RESTRICTED_QUADRATIC_PLUS_TEACHER_UNDO_CONTEXT",
            ["usccss.hs.inverse_composition", "usccss.hs.inverse_table", "usccss.hs.restricted_inverse"],
            "ALG1_U04_L15_EXACT_RESTRICTED_INVERSE_EXTENSION_PLUS_TEACHER_PROCESS_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16",
            "functions.inverse.table",
            "ALG1_INVERSE_FUNCTION_TABLE_EXACT_PLUS_TEACHER_RULE_CONTEXT",
            ["usccss.hs.inverse_table"],
            "ALG1_U04_L16_INVERSE_TABLE_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17",
            "functions.inverse.model",
            "ALG1_EXACT_INVERSE_COMPOSITION_PLUS_TEACHER_MODEL_CONTEXT",
            ["usccss.hs.inverse_composition"],
            "ALG1_U04_L17_INVERSE_MODEL_COMPOSITION_PLUS_TEACHER_ROLE_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L18",
            "functions.model.linear_data",
            "ALG1_LINEAR_MODEL_PREDICT_RESIDUAL_PLUS_TEACHER_MODEL_CRITIQUE",
            ["statistics.linear_model.predict", "statistics.linear_model.residual"],
            "ALG1_U04_L18_BATTERY_MODEL_PLUS_TEACHER_ASSUMPTION_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L01",
            "functions.growth.compare",
            "ALG1_LINEAR_EXPONENTIAL_COMPARE_PLUS_TEACHER_GRAPHS",
            ["functions.growth.linear_exponential.compare"],
            "ALG1_U05_L01_GROWTH_COMPARISON_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L02",
            "functions.growth.compare",
            "ALG1_CONSTANT_DIFFERENCE_FACTOR_PLUS_TEACHER_TABLE_EXPRESSIONS",
            ["functions.growth.linear_exponential.compare"],
            "ALG1_U05_L02_PATTERN_TABLES_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L03",
            "functions.exponential.growth",
            "ALG1_EXPONENTIAL_GROWTH_EVALUATE_PLUS_TEACHER_GRAPH_CONTEXT",
            ["functions.exponential.growth.evaluate"],
            "ALG1_U05_L03_INITIAL_FACTOR_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L04",
            "functions.exponential.decay",
            "ALG1_DECAY_RETAINED_PERCENT_PLUS_TEACHER_REPEATED_MULTIPLICATION",
            ["percentages.core.increase_decrease"],
            "ALG1_U05_L04_PERCENT_REMAINING_PLUS_TEACHER_DECAY_REASONING",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L05",
            "functions.exponential.decay",
            "ALG1_EXPONENTIAL_DECAY_EXACT_PLUS_TEACHER_GRAPH_FACTOR",
            ["functions.exponential.decay.half_evaluate"],
            "ALG1_U05_L05_HALF_DECAY_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L06",
            "functions.exponential.graph.compare",
            "ALG1_U05_L06_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.decay.half_evaluate", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L06_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L07",
            "functions.exponential.negative_exponents",
            "ALG1_U05_L07_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.decay.half_evaluate"],
            "ALG1_U05_L07_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L08",
            "functions.exponential.notation",
            "ALG1_U05_L08_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.relations.unique_output.classify", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L08_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L09",
            "functions.exponential.graph.interpret",
            "ALG1_U05_L09_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.growth.evaluate", "functions.exponential.decay.half_evaluate"],
            "ALG1_U05_L09_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L10",
            "functions.exponential.rate",
            "ALG1_U05_L10_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["geometry.coordinate.gradient_between_points", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L10_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L11",
            "functions.exponential.model.choose",
            "ALG1_U05_L11_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.growth.evaluate", "functions.exponential.decay.half_evaluate"],
            "ALG1_U05_L11_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L12",
            "functions.exponential.graph.parameters",
            "ALG1_U05_L12_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.growth.evaluate"],
            "ALG1_U05_L12_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L13",
            "functions.exponential.two_point.factor",
            "ALG1_U05_L13_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.growth.evaluate"],
            "ALG1_U05_L13_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L14",
            "functions.percent.change",
            "ALG1_U05_L14_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["percentages.core.increase_decrease"],
            "ALG1_U05_L14_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L15",
            "functions.percent.repeated",
            "ALG1_U05_L15_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["percentages.core.increase_decrease", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L15_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L16",
            "functions.percent.compounding",
            "ALG1_U05_L16_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["percentages.core.increase_decrease", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L16_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L17",
            "functions.percent.compounding.intervals",
            "ALG1_U05_L17_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["percentages.core.increase_decrease", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L17_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L18",
            "functions.exponential.equivalent.expressions",
            "ALG1_U05_L18_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.exponential.growth.evaluate", "percentages.core.increase_decrease"],
            "ALG1_U05_L18_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L19",
            "functions.growth.compare",
            "ALG1_U05_L19_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.growth.linear_exponential.compare"],
            "ALG1_U05_L19_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L20",
            "functions.equal_intervals.rate",
            "ALG1_U05_L20_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["geometry.coordinate.gradient_between_points", "functions.exponential.growth.evaluate"],
            "ALG1_U05_L20_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:L21",
            "functions.model.population",
            "ALG1_U05_L21_EXACT_COMPONENT_PLUS_TEACHER_REVIEW",
            ["functions.growth.linear_exponential.compare", "statistics.linear_model.predict"],
            "ALG1_U05_L21_REVIEWED_SOURCE_AND_EXACT_COMPONENT",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L01",
            "functions.quadratic.context",
            "ALG1_U06_L01_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L01_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L02",
            "functions.quadratic.context",
            "ALG1_U06_L02_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex", "functions.growth.linear_exponential.compare"],
            "ALG1_U06_L02_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L03",
            "functions.quadratic.context",
            "ALG1_U06_L03_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["functions.quadratic.expand.middle_coefficient", "supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L03_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L04",
            "functions.quadratic.context",
            "ALG1_U06_L04_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["functions.growth.linear_exponential.compare", "supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L04_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L05",
            "functions.quadratic.context",
            "ALG1_U06_L05_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L05_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L06",
            "functions.quadratic.context",
            "ALG1_U06_L06_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.quadratic_larger_root", "supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L06_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L07",
            "functions.quadratic.context",
            "ALG1_U06_L07_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U06_L07_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L08",
            "functions.quadratic.context",
            "ALG1_U06_L08_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U06_L08_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L09",
            "functions.quadratic.context",
            "ALG1_U06_L09_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U06_L09_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L10",
            "functions.quadratic.context",
            "ALG1_U06_L10_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.quadratic_larger_root", "supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L10_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L11",
            "functions.quadratic.context",
            "ALG1_U06_L11_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U06_L11_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L12",
            "functions.quadratic.context",
            "ALG1_U06_L12_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L12_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L13",
            "functions.quadratic.context",
            "ALG1_U06_L13_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex", "functions.quadratic.expand.middle_coefficient"],
            "ALG1_U06_L13_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L14",
            "functions.quadratic.context",
            "ALG1_U06_L14_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L14_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L15",
            "functions.quadratic.context",
            "ALG1_U06_L15_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L15_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L16",
            "functions.quadratic.context",
            "ALG1_U06_L16_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L16_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:L17",
            "functions.quadratic.context",
            "ALG1_U06_L17_EXACT_SUBSKILLS_PLUS_TEACHER_REVIEW",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U06_L17_QUADRATIC_SOURCE_ALIGNED_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L01",
            "functions.quadratic.equations",
            "ALG1_U07_L01_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root", "supporting.functions.polynomial_value"],
            "ALG1_U07_L01_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L02",
            "functions.quadratic.equations",
            "ALG1_U07_L02_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L02_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L03",
            "functions.quadratic.equations",
            "ALG1_U07_L03_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L03_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L04",
            "functions.quadratic.equations",
            "ALG1_U07_L04_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L04_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L05",
            "functions.quadratic.equations",
            "ALG1_U07_L05_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root", "supporting.algebra.complete_square_vertex"],
            "ALG1_U07_L05_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L06",
            "functions.quadratic.equations",
            "ALG1_U07_L06_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U07_L06_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L07",
            "functions.quadratic.equations",
            "ALG1_U07_L07_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U07_L07_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L08",
            "functions.quadratic.equations",
            "ALG1_U07_L08_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U07_L08_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L09",
            "functions.quadratic.equations",
            "ALG1_U07_L09_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L09_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L10",
            "functions.quadratic.equations",
            "ALG1_U07_L10_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["functions.quadratic.expand.middle_coefficient", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L10_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L11",
            "functions.quadratic.equations",
            "ALG1_U07_L11_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["functions.quadratic.expand.middle_coefficient"],
            "ALG1_U07_L11_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L12",
            "functions.quadratic.equations",
            "ALG1_U07_L12_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L12_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L13",
            "functions.quadratic.equations",
            "ALG1_U07_L13_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L13_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L14",
            "functions.quadratic.equations",
            "ALG1_U07_L14_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L14_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L15",
            "functions.quadratic.equations",
            "ALG1_U07_L15_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.surds.coefficient", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L15_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L16",
            "functions.quadratic.equations",
            "ALG1_U07_L16_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root", "supporting.surds.coefficient"],
            "ALG1_U07_L16_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L17",
            "functions.quadratic.equations",
            "ALG1_U07_L17_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.quadratic_larger_root", "supporting.functions.polynomial_value"],
            "ALG1_U07_L17_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L18",
            "functions.quadratic.equations",
            "ALG1_U07_L18_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.functions.polynomial_value", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L18_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L19",
            "functions.quadratic.equations",
            "ALG1_U07_L19_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root"],
            "ALG1_U07_L19_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L20",
            "functions.quadratic.equations",
            "ALG1_U07_L20_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.surds.coefficient"],
            "ALG1_U07_L20_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L21",
            "functions.quadratic.equations",
            "ALG1_U07_L21_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.surds.coefficient"],
            "ALG1_U07_L21_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L22"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L22",
            "functions.quadratic.equations",
            "ALG1_U07_L22_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "functions.quadratic.expand.middle_coefficient"],
            "ALG1_U07_L22_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L23"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L23",
            "functions.quadratic.equations",
            "ALG1_U07_L23_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex"],
            "ALG1_U07_L23_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L24"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:L24",
            "functions.quadratic.equations",
            "ALG1_U07_L24_EXACT_COMPONENT_PLUS_TEACHER_PROOF",
            ["supporting.algebra.complete_square_vertex", "supporting.algebra.quadratic_larger_root", "supporting.functions.polynomial_value"],
            "ALG1_U07_L24_SOURCE_GOALS_PLUS_TEACHER_EVALUATION",
            "READY_VERIFIED",
            Version);

        // IM 2021 Grade 5 U03:L01: one piece of one selected half
        // measured against the ORIGINAL whole (1/6 or 1/8).
        // Digital objective selections are exact; drawings and explanations
        // remain TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L13"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L13",
            "algebra.linear.systems.two_by_two.solve",
            "ALG1_SYSTEM_SUBSTITUTION_EXACT_PAIR_PLUS_TEACHER_METHOD",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "ALG1_U02_L13_SOURCE_ALIGNED_METHOD_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L14"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L14",
            "algebra.linear.systems.two_by_two.solve",
            "ALG1_SYSTEM_ELIMINATION_ADD_SUBTRACT_EXACT_PAIR_PLUS_TEACHER_METHOD",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "ALG1_U02_L14_SOURCE_ALIGNED_METHOD_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L15",
            "algebra.linear.systems.two_by_two.solve",
            "ALG1_ELIMINATION_CONTEXT_EXACT_PAIR_PLUS_TEACHER_METHOD",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "ALG1_U02_L15_OBJECTIVE_PAIR_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L16",
            "algebra.linear.systems.two_by_two.solve",
            "ALG1_ELIMINATION_SCALING_EXACT_PAIR_PLUS_TEACHER_METHOD",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "ALG1_U02_L16_OBJECTIVE_PAIR_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L17",
            "algebra.linear.systems.classification",
            "ALG1_ZERO_ONE_INFINITE_SOLUTION_COUNT_PLUS_GRAPH_REASONING",
            ["algebra.linear.systems.solution_count.classify"],
            "ALG1_U02_L17_SOLUTION_COUNT_PLUS_TEACHER_JUSTIFICATION",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L18"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L18",
            "algebra.inequalities.modeling",
            "ALG1_CONTEXT_TO_INEQUALITY_OPERATOR_CHOICE_PLUS_TEACHER_MODEL",
            ["algebra.inequality.context.operator_choice"],
            "ALG1_U02_L18_CONTEXT_OPERATOR_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L19"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L19",
            "algebra.inequalities.one_variable",
            "ALG1_ONE_VARIABLE_INEQUALITY_CANDIDATE_PLUS_HUMAN_INTERVAL_REASONING",
            ["algebra.inequality.one_variable.candidate_check"],
            "ALG1_U02_L19_SOURCE_INEQUALITY_PLUS_TEACHER_INTERVAL_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L20"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L20",
            "algebra.inequalities.one_variable",
            "ALG1_ONE_VARIABLE_INEQUALITY_CANDIDATE_PLUS_HUMAN_INTERVAL_REASONING",
            ["algebra.inequality.one_variable.candidate_check", "algebra.inequality.context.operator_choice"],
            "ALG1_U02_L20_SOURCE_INEQUALITY_PLUS_TEACHER_INTERVAL_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L21"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L21",
            "algebra.inequalities.two_variable",
            "ALG1_HALF_PLANE_POINT_MEMBERSHIP_PLUS_HUMAN_GRAPH_CONTEXT",
            ["algebra.inequality.two_variable.candidate_check"],
            "ALG1_U02_L21_EXACT_POINT_MEMBERSHIP_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L22"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L22",
            "algebra.inequalities.two_variable",
            "ALG1_HALF_PLANE_POINT_MEMBERSHIP_PLUS_HUMAN_GRAPH_CONTEXT",
            ["algebra.inequality.two_variable.candidate_check"],
            "ALG1_U02_L22_EXACT_POINT_MEMBERSHIP_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L23"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L23",
            "algebra.inequalities.two_variable",
            "ALG1_INEQUALITY_CANDIDATE_MEMBERSHIP_PLUS_TEACHER_GRAPH_CONTEXT",
            ["algebra.inequality.two_variable.candidate_check"],
            "ALG1_U02_L23_EXACT_FEASIBILITY_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L24"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L24",
            "algebra.inequalities.systems",
            "ALG1_INEQUALITY_CANDIDATE_MEMBERSHIP_PLUS_TEACHER_GRAPH_CONTEXT",
            ["algebra.inequality.system.candidate_check"],
            "ALG1_U02_L24_EXACT_FEASIBILITY_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L25"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L25",
            "algebra.inequalities.systems",
            "ALG1_SYSTEM_BOUNDARY_FEASIBILITY_PLUS_TEACHER_CONTEXT_INTERPRETATION",
            ["algebra.inequality.system.candidate_check"],
            "ALG1_U02_L25_SYSTEMS_WITH_ENDPOINT_PRECISION_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L26"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:L26",
            "algebra.inequalities.systems",
            "ALG1_CUSTOM_TRAIL_MIX_CONSTRAINTS_PLUS_TEACHER_MODEL_CREATION",
            ["algebra.inequality.system.candidate_check"],
            "ALG1_U02_L26_INDEPENDENT_CONSTRAINT_MODEL_PLUS_TEACHER_GRAPH_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L01",
            "statistics.categorical.two_way",
            "ALG1_TWO_WAY_MARGINAL_COUNTS_PLUS_TEACHER_TABLE_CREATION",
            ["statistics.two_way.marginal_count"],
            "ALG1_U03_L01_CATEGORICAL_COUNTS_PLUS_TEACHER_INFORMATION_GAP",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L02",
            "statistics.categorical.relative_frequency",
            "ALG1_U03_CATEGORICAL_EXACT_COMPONENT_PLUS_TEACHER_INTERPRETATION",
            ["statistics.two_way.conditional_relative"],
            "ALG1_U03_L02_SOURCE_CATEGORICAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L03",
            "statistics.categorical.association",
            "ALG1_U03_CATEGORICAL_EXACT_COMPONENT_PLUS_TEACHER_INTERPRETATION",
            ["statistics.two_way.association"],
            "ALG1_U03_L03_SOURCE_CATEGORICAL_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L04",
            "statistics.bivariate.linear_model",
            "ALG1_U03_BIVARIATE_EXACT_COMPONENT_PLUS_TEACHER_GRAPH_INTERPRETATION",
            ["statistics.linear_model.predict"],
            "ALG1_U03_L04_SOURCE_BIVARIATE_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L05",
            "statistics.bivariate.regression",
            "ALG1_U03_BIVARIATE_EXACT_COMPONENT_PLUS_TEACHER_GRAPH_INTERPRETATION",
            ["statistics.linear_model.fit_select"],
            "ALG1_U03_L05_SOURCE_BIVARIATE_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L06",
            "statistics.bivariate.residual",
            "ALG1_U03_EXACT_CORRELATION_OR_RESIDUAL_PLUS_TEACHER_GRAPH_REASONING",
            ["statistics.linear_model.residual"],
            "ALG1_U03_L06_SOURCE_STATISTICS_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L07",
            "statistics.bivariate.correlation",
            "ALG1_U03_EXACT_CORRELATION_OR_RESIDUAL_PLUS_TEACHER_GRAPH_REASONING",
            ["statistics.correlation.direction"],
            "ALG1_U03_L07_SOURCE_STATISTICS_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L08",
            "statistics.bivariate.correlation",
            "ALG1_U03_CORRELATION_CAUSALITY_OBJECTIVE_PLUS_TEACHER_REASONING",
            ["statistics.correlation.rounded"],
            "ALG1_U03_L08_SOURCE_STATISTICS_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L09",
            "statistics.bivariate.causality",
            "ALG1_U03_CORRELATION_CAUSALITY_OBJECTIVE_PLUS_TEACHER_REASONING",
            ["statistics.association.confounder"],
            "ALG1_U03_L09_SOURCE_STATISTICS_PLUS_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:L10",
            "statistics.bivariate.project",
            "ALG1_AUTHENTIC_DATA_PROJECT_NUMERIC_COMPONENTS_PLUS_TEACHER_INVESTIGATION",
            [
                "statistics.linear_model.predict",
                "statistics.linear_model.residual",
                "statistics.correlation.rounded"
            ],
            "ALG1_U03_L10_FOSSILS_FLAGS_PROJECT_PLUS_TEACHER_DATA_PROVENANCE_REVIEW",
            "READY_VERIFIED",
            Version);

        // IM 2021 G4 U06:L15: official outcome/title fallback would route
        // unrelated general Practice before the source-specific mural/area
        // interaction and persisted grading are approved. Keep it fail-closed.
        // The numeric quotient verifier is focused-tested; the complete
        // source teaching, teacher-led drawings and Gradebook are not.
        byLesson["PED:US-CCSS-MATH:G4:U06:L15"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G4:U06:L15",
            "geometry.perimeter_area",
            "AREA_PARTIAL_QUOTIENTS_AND_MURAL_COMPLETE_COLUMNS",
            ["usccss.g4.u06.l15.area_partial_quotients", "usccss.g4.u06.l15.mural_complete_columns"],
            "IM2021_G4_U06_L15_SOURCE_ALIGNED_NUMERIC_PLUS_TEACHER_LED",
            "READY_VERIFIED",
            Version);

        // OpenStax Precalculus 2017 Section 1.1 (Grade 12): reuse only four
        // independently implemented function family mechanics. This is a
        // PARTIAL skill-scoped digital route, NOT an assertion that all 92
        // original exercises, graph drawings, answers or teacher rubrics are
        // digitally graded or that the lesson passed Stage D.
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01"] =
            new LessonPracticeContract(
                "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01",
                "supporting.functions.core",
                "PRECALC_FUNCTION_RELATIONS_INJECTIVITY_IMPLICIT_TABLE_ROOTS_PARTIAL",
                [
                    "functions.relations.unique_output.classify",
                    "functions.relations.one_to_one.classify",
                    "functions.relations.implicit.classify",
                    "functions.table.lookup.invert",
                    "functions.quadratic.output_roots.solve",
                    "functions.notation.evaluate_context",
                    "functions.domain_range.context_bounds",
                    "functions.absolute_value.translate_evaluate"
                ],
                "G12_SECTION_1_1_FOUR_REUSED_FOUR_EXTENDED_FAMILIES_NOT_92_SOURCE_PARITY",
                "READY_VERIFIED",
                Version)
            {
                SkillIds = ["supporting.functions.core", "functions.evaluate.exact"]
            };

        // Stage D lesson-specific exact subset. These contracts only certify the
        // tested numerical generator family. Graphs, explanations, source
        // exercise mapping and teacher rubric remain separately assessed.
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L04",
            "functions.inverse.composition",
            "COMPOSITION_INVERSE_EXACT_PARTIAL",
            ["usccss.hs.inverse_composition"],
            "FIRSTPARTY_SECTION_1_4_COMPOSITION_EXACT_SUBSET_TEACHER_PROSE",
            "READY_VERIFIED",
            Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L05"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L05",
            "functions.absolute_value.translate",
            "TRANSFORMATION_TRANSLATION_EXACT_PARTIAL",
            ["functions.absolute_value.translate_evaluate"],
            "FIRSTPARTY_SECTION_1_5_FUNCTION_TRANSLATION_SUBSET_TEACHER_GRAPH",
            "READY_VERIFIED",
            Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L06",
            "functions.absolute_value.translate",
            "ABSOLUTE_VALUE_TRANSLATION_EXACT_PARTIAL",
            ["functions.absolute_value.translate_evaluate", "functions.absolute_error.from_target"],
            "FIRSTPARTY_SECTION_1_6_ABSOLUTE_VALUE_SUBSET_TEACHER_GRAPH",
            "READY_VERIFIED",
            Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L01",
            "functions.average_rate_of_change",
            "LINEAR_SLOPE_EXACT_PARTIAL",
            ["geometry.coordinate.gradient_between_points"],
            "FIRSTPARTY_SECTION_2_1_SLOPE_SUBSET_TEACHER_LINEAR_MODELS",
            "READY_VERIFIED",
            Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L02",
            "functions.graph.key_features",
            "LINEAR_INTERCEPT_EXACT_PARTIAL",
            ["geometry.coordinate.y_intercept_from_rule"],
            "FIRSTPARTY_SECTION_2_2_Y_INTERCEPT_SUBSET_TEACHER_GRAPH",
            "READY_VERIFIED",
            Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L04",
            "functions.model.linear_data",
            "LINEAR_REGRESSION_PREDICTION_EXACT_PARTIAL",
            ["statistics.linear_model.predict", "statistics.linear_model.residual"],
            "FIRSTPARTY_SECTION_2_4_LINEAR_MODEL_SUBSET_TEACHER_FIT",
            "READY_VERIFIED",
            Version);

        // Additional Stage D exact numeric subsets selected for matching
        // independently authored Grade12 lesson skills. These are not a
        // blanket adaptation of a textbook exercise bank and do not grade
        // proofs, source illustrations, submissions, or teacher rubrics.
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L08", "functions.inverse.composition",
            "RESTRICTED_QUADRATIC_INVERSE_EXACT_PARTIAL",
            ["usccss.hs.restricted_inverse"],
            "G12_SECTION_3_8_RESTRICTED_INVERSE_SUBSET_TEACHER_RADICAL_GRAPHS", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L01", "functions.exponential.growth",
            "EXPONENTIAL_GROWTH_EVALUATION_EXACT_PARTIAL",
            ["functions.exponential.growth.evaluate"],
            "G12_SECTION_4_1_EXPONENTIAL_EVALUATION_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L02", "functions.exponential.graph.interpret",
            "EXPONENTIAL_GROWTH_EVALUATION_EXACT_PARTIAL",
            ["functions.exponential.growth.evaluate"],
            "G12_SECTION_4_2_EXPONENTIAL_GRAPH_NUMERICAL_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L07", "functions.exponential.model.choose",
            "EXPONENTIAL_GROWTH_MODEL_EVALUATION_EXACT_PARTIAL",
            ["functions.exponential.growth.evaluate"],
            "G12_SECTION_4_7_EXPONENTIAL_MODELS_EXACT_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L01", "algebra.linear.systems.two_by_two.solve",
            "TWO_BY_TWO_LINEAR_SYSTEM_EXACT_PARTIAL",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "G12_SECTION_9_1_TWO_VARIABLE_LINEAR_SYSTEM_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L06", "algebra.linear.systems.two_by_two.solve",
            "GAUSSIAN_TWO_BY_TWO_ELIMINATION_EXACT_PARTIAL",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "G12_SECTION_9_6_ELIMINATION_TWO_VARIABLE_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L08", "algebra.linear.systems.two_by_two.solve",
            "CRAMER_TWO_BY_TWO_NUMERICAL_SUBSET",
            ["algebra.linear.systems.two_by_two.integer_coefficients"],
            "G12_SECTION_9_8_CRAMER_TWO_VARIABLE_SYSTEM_SUBSET_TEACHER_DETERMINANT", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L07"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L07", "probability.theoretical.core",
            "PROBABILITY_COMPLEMENT_EXACT_PARTIAL",
            ["probability.theoretical.complement"],
            "G12_SECTION_11_7_PROBABILITY_COMPLEMENT_SUBSET", "READY_VERIFIED", Version);

        // Stage D: arithmetic/sequence/vector/trigonometry exact subsets.
        // No source-atom, diagram, proof, human rubric or full digital parity
        // certification can be inferred from these numerical mechanics.
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L03", "functions.quadratic.equations",
            "POLYNOMIAL_QUADRATIC_SPECIAL_CASE_EXACT_PARTIAL",
            ["functions.quadratic.output_roots.solve"],
            "G12_SECTION_3_3_QUADRATIC_POLYNOMIAL_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L06"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L06", "functions.quadratic.equations",
            "POLYNOMIAL_ZERO_QUADRATIC_SPECIAL_CASE_EXACT_PARTIAL",
            ["functions.quadratic.output_roots.solve"],
            "G12_SECTION_3_6_POLYNOMIAL_QUADRATIC_ROOT_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L04"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L04", "trigonometry.right_triangle.ratio",
            "RIGHT_TRIANGLE_TRIGONOMETRY_EXACT_PARTIAL",
            ["trigonometry.right_triangle.ratio_exact", "trigonometry.right_triangle.find_side_exact"],
            "G12_SECTION_5_4_RIGHT_TRIANGLE_MEASUREMENT_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L08"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L08", "vectors.operations",
            "VECTOR_EXACT_ARITHMETIC_PARTIAL",
            ["vectors.add.exact_rational", "vectors.subtract.exact_rational", "vectors.magnitude.exact", "vectors.dot.exact_rational"],
            "G12_SECTION_8_8_VECTOR_ARITHMETIC_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L01", "sequences.core",
            "EXPLICIT_SEQUENCE_NTH_TERM_EXACT_PARTIAL",
            ["sequences.core.arithmetic_nth_term", "sequences.core.geometric_nth_term"],
            "G12_SECTION_11_1_SEQUENCE_NOTATION_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L02", "sequences.arithmetic",
            "ARITHMETIC_NTH_TERM_EXACT_PARTIAL",
            ["sequences.core.arithmetic_nth_term", "sequences.core.find_position_arithmetic"],
            "G12_SECTION_11_2_ARITHMETIC_SEQUENCE_SUBSET", "READY_VERIFIED", Version);
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L03"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U11:L03", "sequences.geometric",
            "GEOMETRIC_NTH_TERM_EXACT_PARTIAL",
            ["sequences.core.geometric_nth_term"],
            "G12_SECTION_11_3_GEOMETRIC_SEQUENCE_SUBSET", "READY_VERIFIED", Version);

        // These 31 distinct mathematical question families implement an
        // exact, bounded original skill from the corresponding Grade12
        // section. They are subject to independent runtime certification and
        // never auto-grade source figures, proofs or teacher-led assignments.
        const string grade12Root =
            "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";
        foreach (var (subskill, section) in
            Grade12PrecalculusNumericalFamilyCatalog.All)
        {
            var code = grade12Root + section;
            if (byLesson.ContainsKey(code))
                throw new InvalidOperationException(
                    $"Grade12 contract collision: {code}");
            var family = Grade12PrecalculusNumericalFamilyCatalog.Prefix + subskill;
            byLesson[code] = new LessonPracticeContract(
                code,
                "g12.precalculus.exact." + subskill,
                "FIRST_PARTY_G12_EXACT_NUMERICAL_SUBSKILL_NOT_FULL_SOURCE_PARITY",
                [family],
                "GRADE12_SECTION_MATCHED_MATHEMATICAL_SKILL_TEACHER_REVIEW_FOR_OPEN_TASKS",
                "READY_VERIFIED",
                Version);
        }

        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:01:01",
            "number.place_value.to_thousands",
            "FOUR_DIGIT_PLACE_VALUE_COMPOSITION",
            ["uae.g3.u01.l01.digit_value", "uae.g3.u01.l01.compose_four_digits", "uae.g3.u01.l01.expanded_to_standard"],
            "UAE_G3_2025_2026_ARABIC_STUDENT_PP9_14_STAGE_D_PENDING",
            "READY_VERIFIED",
            Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:01:02",
            "number.place_value.comparison_to_9999",
            "FOUR_DIGIT_RELATION_AND_SELECTION",
            ["uae.g3.u01.l02.compare_4digit_relation", "uae.g3.u01.l02.find_greater", "uae.g3.u01.l02.find_smaller"],
            "UAE_G3_2025_2026_ARABIC_STUDENT_PP15_20_STAGE_D_PENDING",
            "READY_VERIFIED",
            Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:01:03","number.order.to_thousands","ORDER_THREE_ASC_DESC_AND_MIDDLE",
            ["uae.g3.u01.l03.order_three_ascending", "uae.g3.u01.l03.order_three_descending", "uae.g3.u01.l03.identify_middle_number"],
            "UAE_G3_2025_2026_STUDENT_PP21_26_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:01:04","number.rounding.nearest_ten","ROUND_TO_NEAREST_TEN",
            ["uae.g3.u01.l04.round_nearest_ten"],"G3_2025_2026_ARABIC_SOURCE_PP29_34_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:01:05","number.rounding.nearest_hundred","ROUND_TO_NEAREST_HUNDRED",
            ["uae.g3.u01.l05.round_nearest_hundred"],"G3_2025_2026_STUDENT_PP35_40_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:01","number.addition.properties","COMMUTATIVE_ASSOCIATIVE_IDENTITY",
            ["uae.g2.u01.l01.commutative_add","uae.g2.u01.l01.additive_zero","uae.g3.u02.l01.associative_add"],
            "G3_U02_L01_2025_2026_STUDENT_PP55_60_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:02","number.addition.table_patterns","SUM_MISSING_ADDEND_PARITY_PATTERN",
            ["uae.g3.u02.l02.addition_table_sum", "uae.g3.u02.l02.missing_table_addend", "uae.g3.u02.l02.even_odd_step2"],
            "G3_2025_2026_ARABIC_STUDENT_PP61_66_AND_OLD_TEACHER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:03","number.addition.place_patterns","ONE_TEN_HUNDRED_THOUSAND_INCREMENT",
            ["uae.g3.u02.l03.add_place_step", "uae.g3.u02.l03.find_missing_step", "uae.g3.u02.l03.complete_pattern"],
            "G3_2025_2026_ARABIC_STUDENT_PP67_72_TEACHER_GUIDE_OLDER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:04","number.addition.mental_3digit","DECOMPOSE_AND_COMPENSATE_EXACT",
            ["uae.g3.u02.l04.mental_addition"],"G3_2025_2026_STUDENT_PP73_78_STAGE_D_NOT_RUN","READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:05","number.addition.estimate_sum","ROUND_BOTH_ADDENDS_FIRST_THEN_SUM",
            ["uae.g3.u02.l05.estimate_sum_by_rounding"],"G3_STUDENT_PP81_86_TEACHER_81A_B_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:06","number.addition.base_ten_model","BASE_TEN_BLOCKS_REGROUP_ONES_TENS",
            ["uae.g2.u06.l04.regroup_ones_add", "uae.g2.u06.l05.regroup_tens_add", "uae.g2.u06.l06.add_three_digits"],
            "G3_STUDENT_PP87_92_TEACHER_87A_88_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:07","number.addition.three_digit_standard","PLACE_VALUE_VERTICAL_AND_REGROUP",
            ["uae.g2.u06.l07.vertical_three_digit_add", "uae.g2.u06.l04.regroup_ones_add", "uae.g2.u06.l05.regroup_tens_add"],
            "G3_STUDENT_PP93_98_TEACHER_PP93_96_OLD_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:08","number.addition.four_digit_with_regroup","VERTICAL_FOUR_DIGIT_PLACE_VALUE_AND_CARRY",
            ["uae.g3.u02.l08.add_four_digit"],"G3_STUDENT_PP101_106_TEACHER_101_104_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:02:09"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:02:09",
            "number.addition.reasonable_answers",
            "ESTIMATE_EVALUATE_AND_EXPLAIN_APPROXIMATE_SUM",
            ["uae.g3.u02.l05.estimate_sum_by_rounding", "uae.g3.u02.l09.reasonable_estimate_verdict"],
            "UAE_G3_2025_2026_ARABIC_STUDENT_PP107_112_2026_2027_PARITY_UNKNOWN_STAGE_D_NOT_RUN",
            "READY_VERIFIED",
            Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:01",
            "number.subtraction.mental_strategies",
            "SPLIT_TENS_ONES_AND_COMPENSATE_99",
            ["uae.g2.u07.l01.decompose_hundred_subtract", "uae.g2.u07.l03.subtract_ten_hundred_mental", "uae.g3.u03.l01.mental_split_subtract"],
            "UAE_G3_2025_2026_STUDENT_PP127_132_2026_2027_SOURCE_PARITY_UNKNOWN_STAGE_D_PENDING",
            "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:02","number.subtraction.estimate_differences","ESTIMATE_DIFFERENCE_TENS_HUNDREDS_HIGHEST_PLACE",
            ["uae.g3.u03.l02.estimate_difference_by_rounding"],"G3_LOCAL_2025_2026_STUDENT_PP133_138_2026_2027_PARITY_UNKNOWN_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:03", "number.subtraction.estimate_or_exact_decision",
            "DECIDE_ESTIMATE_OR_EXACT_BY_CONTEXT",
            ["uae.g3.u03.l02.estimate_difference_by_rounding", "uae.g2.u07.l08.story_equation_sub", "uae.g3.u03.l03.choose_estimate_or_exact"],
            "UAE_G3_STUDENT_2025_2026_PP139_144_STAGE_D_PENDING", "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:04","number.subtraction.regroup_base_ten_models","REGROUP_TEN_TO_ONES_AND_HUNDRED_TO_TENS",
            ["uae.g2.u07.l04.regroup_tens_for_ones", "uae.g2.u07.l05.regroup_hundreds_for_tens", "uae.g2.u07.l06.subtract_three_digit"],
            "UAE_G3_2025_2026_STUDENT_PP147_152_BASE_TEN_MODELS_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:05","number.subtraction.three_digit_exact","THREE_DIGIT_VERTICAL_REGROUP_AND_CHECK",
            ["uae.g2.u07.l04.regroup_tens_for_ones", "uae.g2.u07.l05.regroup_hundreds_for_tens", "uae.g2.u07.l06.subtract_three_digit", "uae.g2.u07.l07.vertical_three_digit_sub"],
            "G3_STUDENT_2025_2026_PP153_158_GRADE2_EXACT_REUSE_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:06", "number.subtraction.four_digit_regroup",
            "FOUR_DIGIT_VERTICAL_SUBTRACT_WITH_INVERSE_CHECK",
            ["uae.g3.u03.l06.four_digit_subtract_exact","uae.g2.u07.l08.story_equation_sub"],
            "UAE_G3_2025_2026_STUDENT_PP159_164_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:03:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:03:07","number.subtraction.cross_zero_regroup",
            "VERTICAL_SUBTRACT_CHAINED_ZERO_REGROUP_AND_INVERSE_VERIFY",
            ["uae.g3.u03.l06.four_digit_subtract_exact","uae.g3.u03.l07.subtract_across_zeros_exact"],
            "UAE_G3_2025_2026_ARABIC_STUDENT_PP165_170_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:01","number.multiplication.equal_groups",
            "CONCRETE_EQUAL_GROUPS_TO_REPEATED_ADDITION_AND_MULTIPLICATION",
            ["uae.g2.u02.l04.repeated_add_equal_groups"],
            "UAE_G3_2025_2026_STUDENT_PP187_192_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:02","number.multiplication.repeated_addition",
            "CONVERT_EQUAL_GROUPS_REPEATED_ADDITION_TO_MULTIPLICATION",
            ["uae.g2.u02.l04.repeated_add_equal_groups"],
            "UAE_G3_2025_2026_STUDENT_PP193_198_TEACHER_2020_2021_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:03","number.multiplication.arrays",
            "MODEL_ROWS_COLUMNS_REPEATED_ADDITION_AND_COMMUTATIVITY",
            ["uae.g2.u02.l05.repeated_add_arrays"],
            "UAE_G3_2025_2026_STUDENT_PP199_204_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:04","number.multiplication.array_factors",
            "ARRAY_PRODUCT_COMMUTATIVE_PROPERTY_AND_MISSING_FACTOR",
            ["uae.g2.u02.l05.repeated_add_arrays","uae.g3.u04.l04.find_missing_array_factor"],
            "G3_2025_2026_STUDENT_PP205_210_2020_2021_TEACHER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:05", "number.multiplication.table_strategy", "TWO_CATEGORY_CROSS_TABLE_AND_CUMULATIVE_SEQUENCE",
            ["uae.g2.u02.l05.repeated_add_arrays","uae.g3.u04.l04.find_missing_array_factor"],
            "UAE_G3_2025_2026_STUDENT_PP213_218_OLD_TEACHER_TABLE_STRATEGY_STAGE_D_NOT_RUN",
            "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:04:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:04:06", "number.multiplication.combinations_tree",
            "TWO_STAGE_COMBINATIONS_TREE_LEAF_COUNT_AND_FACTOR_PRODUCT",
            ["uae.g2.u02.l05.repeated_add_arrays","uae.g3.u04.l04.find_missing_array_factor"],
            "UAE_G3_ARABIC_STUDENT_2025_2026_PP219_224_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:01", "number.division.equal_groups_models", "MODEL_DIVIDEND_DIVISOR_QUOTIENT_IN_EQUAL_GROUPS",
            ["uae.g3.u05.l01.model_equal_groups_division"],
            "UAE_G3_2025_2026_STUDENT_PP239_244_TEACHER_2020_2021_STAGE_D_NOT_RUN",
            "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:02", "number.division.equal_parts", "EQUAL_SHARE_GROUPS_AND_MISSING_DIVISOR",
            ["uae.g3.u05.l01.model_equal_groups_division"],
            "UAE_G3_STUDENT_2025_2026_PP245_250_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED", Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:03","number.division.repeated_subtraction","REPEATED_SUBTRACT_TO_ZERO_AND_COUNT_JUMPS",
            ["uae.g3.u05.l01.model_equal_groups_division","uae.g3.u05.l03.repeated_subtraction_division"],
            "G3_2025_2026_STUDENT_PP251_256_TEACHER_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:04", "number.division.inverse_multiplication", "ARRAY_PRODUCT_AND_INVERSE_DIVISION",
            ["uae.g3.u05.l01.model_equal_groups_division","uae.g3.u04.l04.find_missing_array_factor"],
            "UAE_G3_STUDENT_2025_2026_PP259_264_OLDER_TEACHER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:05","number.division.multiplication_fact_families","TWO_MULTIPLICATION_TWO_DIVISION_INVERSE_FACTS",
            ["uae.g3.u05.l01.model_equal_groups_division","uae.g3.u04.l04.find_missing_array_factor"],
            "UAE_G3_STUDENT_2025_2026_PP265_270_OLD_TEACHER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:05:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:05:06","number.division.model_word_problem",
            "TAPE_DIAGRAM_EQUAL_GROUPS_AND_TWO_STEP_CONTEXT",
            ["uae.g3.u05.l01.model_equal_groups_division","uae.g3.u05.l06.modeled_division_word_problem"],
            "UAE_G3_LOCAL_STUDENT_2025_2026_PP271_276_TEACHER_OLDER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:01","number.multiplication.table_patterns",
            "TABLE_ROW_COLUMN_INTERSECTION_MISSING_FACTOR_AND_PATTERN",
            ["uae.g3.u06.l01.multiplication_table_pattern"],
            "UAE_G3_2025_2026_STUDENT_PRINTED_289_294_TEACHER_OLD_2020_2021_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:02","number.multiplication.times_two","DOUBLES_ARRAYS_EQUAL_GROUPS_REPEATED_ADDITION",
            ["uae.g3.u06.l02.multiply_by_two"],"G3_STUDENT_2025_2026_295_300_STAGE_D_PENDING","READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:03","number.division.by_two","TWO_EQUAL_GROUPS_AND_INVERSE_MULTIPLICATION",
            ["uae.g3.u06.l03.divide_by_two","uae.g3.u05.l01.model_equal_groups_division"],
            "UAE_G3_LOCAL_2025_2026_STUDENT_PP301_306_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:04","number.multiplication.by_five","SKIP_COUNT_FIVE_ARRAY_COIN_AND_WORD_PROBLEM",
            ["uae.g3.u06.l04.multiply_by_five"],
            "UAE_G3_LOCAL_2025_2026_STUDENT_PP307_312_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:05","number.division.by_five","EQUAL_GROUP_SHARE_FIVE_REPEATED_SUBTRACT_INVERSE_FACT",
            ["uae.g3.u06.l05.divide_by_five","uae.g3.u05.l01.model_equal_groups_division"],
            "UAE_G3_LOCAL_2025_2026_STUDENT_PP313_318_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:06","number.patterns.multiplication_division",
            "LOOK_FOR_PATTERN_ADDITIVE_DOUBLING_HALVING_REPETITION",
            ["uae.g3.u06.l06.find_mul_div_pattern"],"UAE_G3_2025_2026_STUDENT_PP321_326_TEACHER_OLDER_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:07","number.multiplication.by_ten","TENS_GROUPS_SKIP_COUNT_AND_INVERSE_FACTOR",
            ["uae.g3.u06.l07.multiply_by_ten","uae.g3.u06.l01.multiplication_table_pattern"],
            "UAE_G3_2025_2026_STUDENT_PP327_332_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:08","number.multiplication.multiples_of_ten","BASIC_FACT_TIMES_TEN_PLACE_VALUE_AND_WORD_PROBLEM",
            ["uae.g3.u06.l08.multiply_by_tens","uae.g3.u06.l07.multiply_by_ten"],
            "UAE_G3_2025_2026_STUDENT_PP333_338_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:06:09"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:06:09","number.division.by_ten","TEN_EQUAL_GROUPS_REPEATED_SUBTRACTION_INVERSE_FACT",
            ["uae.g3.u06.l09.divide_by_ten"],
            "UAE_G3_2025_2026_STUDENT_PP339_344_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:01","number.multiplication.times_three",
            "THREE_EQUAL_GROUPS_NUMBERLINE_ARRAY_AND_INVERSE_MISSING_FACTOR",
            ["uae.g3.u07.l01.multiply_by_three"],
            "UAE_G3_LOCAL_VOLUME2_PP359_364_YEAR_UNVERIFIED_STAGE_D_PENDING",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:02","number.division.by_three",
            "SHARE_IN_THREE_EQUAL_GROUPS_REPEATED_SUBTRACTION_INVERSE",
            ["uae.g3.u07.l02.divide_by_three"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_365_370_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:03","number.multiplication.doubling_fact",
            "DOUBLE_2X_FACT_TO_4X_ARRAY_AND_PART_WHOLE",
            ["uae.g3.u07.l03.double_known_fact"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_371_376_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:04","number.multiplication.times_four",
            "FOUR_EQUAL_GROUPS_ARRAY_DOUBLE_TWO_FACT",
            ["uae.g3.u07.l04.multiply_by_four"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_377_382_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:05","number.division.by_four",
            "FOUR_EQUAL_GROUPS_MULTIPLICATION_INVERSE",
            ["uae.g3.u07.l05.divide_by_four"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_383_388_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:06","number.problem_solving.info_sufficiency",
            "IDENTIFY_RELEVANT_EXTRA_OR_MISSING_QUANTITY_TEACHER_REASON",
            ["uae.g3.u07.l06.information_sufficient"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_391_396_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:07","number.multiplication.zero_one_identity",
            "ZERO_PROPERTY_MULTIPLICATIVE_IDENTITY_MODELS",
            ["uae.g3.u07.l07.multiply_zero_one"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_397_402_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:07:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:07:08","number.division.zero_one_identity",
            "ZERO_DIVIDEND_ONE_DIVISOR_SELF_DIVISION_UNDEFINED_ZERO_DIVISOR",
            ["uae.g3.u07.l08.divide_zero_one"],
            "UAE_G3_LOCAL_ARABIC_VOLUME2_PRINTED_403_408_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:01","number.multiplication.times_six",
            "SIX_EQUAL_GROUPS_DECOMPOSE_THREE_PLUS_THREE",
            ["uae.g3.u08.l01.multiply_six"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_423_428_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:02","number.multiplication.times_seven",
            "SEVEN_GROUPS_FIVE_PLUS_TWO_ARRAY",
            ["uae.g3.u08.l02.multiply_seven"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_429_434_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:03","number.division.by_six_seven",
            "INVERSE_FACTS_DIVISION_BY_SIX_AND_SEVEN",
            ["uae.g3.u08.l03.divide_six_seven"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_435_440_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:04","number.multiplication.times_eight",
            "DOUBLE_FOUR_FACT_EIGHT_GROUPS",
            ["uae.g3.u08.l04.multiply_eight"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_443_448_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:05","number.multiplication.times_nine",
            "TEN_MINUS_ONE_PROPERTY_NINE_GROUPS",
            ["uae.g3.u08.l05.multiply_nine"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_449_454_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:06"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:06","number.division.by_eight_nine",
            "INVERSE_FACTS_DIVISION_BY_EIGHT_AND_NINE",
            ["uae.g3.u08.l06.divide_eight_nine"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_455_460_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:07"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:07","number.problem_solving.organized_lists",
            "SYSTEMATIC_ENUMERATION_MULTIPLICATIVE_COMBINATIONS",
            ["uae.g3.u08.l07.organized_list_combinations"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_463_468_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:08"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:08","number.multiplication.eleven_twelve",
            "BREAK_TEN_PLUS_ONE_OR_TWO_FACTS",
            ["uae.g3.u08.l08.multiply_eleven_twelve"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_469_474_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:08:09"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:08:09","number.division.eleven_twelve",
            "INVERSE_MULTIPLICATION_DIVISION_BY_ELEVEN_TWELVE",
            ["uae.g3.u08.l09.divide_eleven_twelve"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_475_480_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:09:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:09:01","number.multiplication.decompose",
            "SPLIT_FACTOR_INTO_PARTS_MULTIPLICATION_ARRAYS",
            ["uae.g3.u09.l01.decompose_multiplication"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_495_500_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:09:02"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:09:02","number.multiplication.distributive",
            "MULTIPLY_SUM_BY_DISTRIBUTING_FACTOR",
            ["uae.g3.u09.l02.distributive_property"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_501_506_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:09:03"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:09:03","number.multiplication.three_factors",
            "MODEL_PRODUCT_OF_THREE_FACTORS_WITH_GROUPS",
            ["uae.g3.u09.l03.multiply_three_factors"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_507_512_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:09:04"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:09:04","number.multiplication.associative",
            "GROUP_THREE_FACTORS_WITHOUT_CHANGING_PRODUCT",
            ["uae.g3.u09.l04.associative_property"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_513_518_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        byLesson["PED:UAE-MOE-MATH:L3:COMMON:09:05"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L3:COMMON:09:05","number.algebra.write_expressions",
            "TRANSLATE_WORDS_TO_NUMERIC_EXPRESSIONS",
            ["uae.g3.u09.l05.write_expression"],
            "UAE_G3_VOLUME2_SOURCE_SCOPED_PRINTED_521_526_STAGED_STAGE_D_NOT_RUN",
            "READY_VERIFIED",Version);
        return byLesson.Values
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();
    }

    public static bool TryResolve(string? lessonCode, out LessonPracticeContract? contract)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            contract = null;
            return false;
        }

        return ByLessonCode.TryGetValue(lessonCode.Trim(), out contract);
    }
}
