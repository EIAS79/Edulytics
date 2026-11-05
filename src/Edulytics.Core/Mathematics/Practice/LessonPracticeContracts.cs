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
            "functions.inverse.linear",
            "ALG1_INVERSE_LINEAR_EXACT_PLUS_TEACHER_UNDO_CONTEXT",
            ["supporting.functions.inverse_linear"],
            "ALG1_U04_L15_INVERSE_LINEAR_PLUS_TEACHER_PROCESS_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16",
            "functions.inverse.linear",
            "ALG1_INVERSE_LINEAR_SOLVE_PLUS_TEACHER_RULE_CONTEXT",
            ["supporting.functions.inverse_linear"],
            "ALG1_U04_L16_INVERSE_SOLVE_PLUS_TEACHER_CONTEXT_REVIEW",
            "READY_VERIFIED",
            Version);

        byLesson["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17",
            "functions.inverse.model",
            "ALG1_LINEAR_MODEL_AND_INVERSE_EXACT_PLUS_TEACHER_CONTEXT",
            ["statistics.linear_model.predict", "supporting.functions.inverse_linear"],
            "ALG1_U04_L17_MODEL_PREDICT_INVERSE_PLUS_TEACHER_ROLE_REVIEW",
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
            "IM2021_G7_U01_L01_EXACT_AND_TEACHER_REVIEW",
            "READY_VERIFIED",
            Version);

        // IM 2021 Grade 6 U01:L01: reuse released two-family tiling route.
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
