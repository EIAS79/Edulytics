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
            "G5_SOURCE_UNIT7_LESSON_13_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l13.source_rectangle_area_perimeter_coordinate_pairs"],
            "IM2021_G5_U07_L13_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L12: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L12"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L12", "supporting.geometry.properties",
            "G5_SOURCE_UNIT7_LESSON_12_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l12.source_coin_graph_context"],
            "IM2021_G5_U07_L12_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L11: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L11"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L11", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_11_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l11.source_corresponding_pattern_coordinate_pairs"],
            "IM2021_G5_U07_L11_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L10: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L10"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L10", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_10_GEOMETRY_ATTRIBUTES",
            ["usccss.g5.u07.l10.source_recursive_patterns_offset"],
            "IM2021_G5_U07_L10_GEOMETRY_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L09: source-aligned bounded geometry
        // source subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L09"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L09", "supporting.sequences.core",
            "G5_SOURCE_UNIT7_LESSON_09_GEOMETRY_ATTRIBUTES",
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
            "PED:US-CCSS-MATH:G5:U07:L03", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_03_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u07.l03.source_shared_coordinate_lines"],
            "IM2021_G5_U07_L03_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L02: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L02"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L02", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_02_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u07.l02.original_plot_specific_points"],
            "IM2021_G5_U07_L02_DECIMAL_TEACHER_REVIEW",
            "READY_VERIFIED", Version);
        // IM K5 2021 G5 U07:L01: source-aligned selected decimal
        // numeric subskills, with original drawings/reasoning TEACHER_REVIEW_REQUIRED.
        byLesson["PED:US-CCSS-MATH:G5:U07:L01"] = new LessonPracticeContract(
            "PED:US-CCSS-MATH:G5:U07:L01", "supporting.decimals.core",
            "G5_SOURCE_UNIT5_LESSON_01_DECIMAL_PLACE_VALUE",
            ["usccss.g5.u07.l01.coordinates_description_vocabulary"],
            "IM2021_G5_U07_L01_DECIMAL_TEACHER_REVIEW",
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
