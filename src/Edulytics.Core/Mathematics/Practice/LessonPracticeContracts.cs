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
        // OUR/IM 6-8 Grade 7 (2017-2019) U01:L01: exact polygon recognition;
        // constructions, explanations and qualitative feedback need teacher review.
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
