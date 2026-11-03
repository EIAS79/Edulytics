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
