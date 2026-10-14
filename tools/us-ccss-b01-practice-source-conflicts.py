#!/usr/bin/env python3
"""Independent B01 whole-batch Practice question-family vs publisher target audit.

This explicitly refuses to treat a correct answer as proof that the question
belongs to the lesson. Output 500 evidence rows, never 500 certifications.
"""
import argparse, json, pathlib, re

RULES = (
 ("CIRCLE_PI_NOT_SOURCE_TARGET",
  lambda f,t: ("circle_circumference_pi_" in f or "circle_area_pi_" in f)
      and "circumference" not in t and "area of a circle" not in t),
 ("UNIT_RATES_NOT_SOURCE_TARGET",
  lambda f,t: f.startswith("ratio.unit_rate.") and
      not any(x in t for x in ("unit rate","rate per","cost per","speed per"))),
 ("SURDS_NOT_SOURCE_TARGET",
  lambda f,t: f.startswith("supporting.surds.") and
      not any(x in t for x in ("surds","square roots","radicals","irrational"))),
 ("MEAN_MEDIAN_NOT_SOURCE_TARGET",
  lambda f,t: f.startswith("statistics.center_spread.") and
      not any(x in t for x in ("mean and median","median and mean","find the mean","find the median"))),
 ("ALGEBRA_FACTORING_NOT_SOURCE_TARGET",
  lambda f,t: f.startswith("supporting.algebra.factor") and
      not any(x in t for x in ("factor a polynomial","factor expressions","factoring algebraic"))),
 ("TRIANGLE_PARALLELOGRAM_AREA_NOT_SOURCE_TARGET",
  lambda f,t: f in ("supporting.geometry.parallelogram_area","supporting.geometry.triangle_area") and
      not any(x in t for x in ("parallelogram","area of a triangle","triangles and area"))),
 ("GCF_LCM_NOT_SOURCE_TARGET",
  lambda f,t: f in ("supporting.number.gcf","supporting.number.lcm") and
      not any(x in t for x in ("greatest common factor","least common multiple","gcf","lcm"))),
 ("COMBINATORICS_NOT_SOURCE_TARGET",
  lambda f,t: f.startswith("supporting.combinatorics.") and
      not any(x in t for x in ("combinations of","choose two","pairs from a set"))),
)

# 50 original publisher lesson targets: exact approved code/family linkage.
# This protects against silently falling back to unrelated generic engines.
SOURCE_SCOPED_FAMILIES = {
    "PED:US-CCSS-MATH:G1:U03:L12": "usccss.b01.g1.teen_no_regroup",
    "PED:US-CCSS-MATH:G1:U03:L22": "usccss.b01.g1.teen_subtract_decompose_ten",
    "PED:US-CCSS-MATH:G2:U02:L03": "usccss.b01.g2.story_without_regroup100",
    "PED:US-CCSS-MATH:G2:U02:L11": "usccss.b01.g2.story_regroup50",
    "PED:US-CCSS-MATH:G4:U01:L01": "usccss.b01.g4.rectangle_area_is_multiple",
    "PED:US-CCSS-MATH:G3:U04:L10": "usccss.b01.g3.distributive_area_partition",
    "PED:US-CCSS-MATH:G3:U07:L10": "usccss.b01.g3.rectangle_area_perimeter_story",
    "PED:US-CCSS-MATH:G4:U01:L02": "usccss.b01.g4.factor_pairs_two_rectangles",
    "PED:US-CCSS-MATH:G3:U07:L11": "usccss.b01.g3.same_perimeter_different_area",
    "PED:US-CCSS-MATH:G3:U07:L12": "usccss.b01.g3.same_area_different_perimeter",
    "PED:US-CCSS-MATH:G2:U06:L06": "usccss.b01.g2.compose_equal_square_shapes",
    "PED:US-CCSS-MATH:G3:U08:L10": "usccss.b01.g3.multiplication_division_inverse_array",
    "PED:US-CCSS-MATH:G2:U03:L17": ["usccss.b01.g2.add_subtract_without_regroup", "usccss.b01.ruler.measure_without_zero", "usccss.b01.g2.measurement.read_line_plot"],
    "PED:US-CCSS-MATH:G4:U04:L03": "usccss.b01.g4.decimal_number_line_greater",
    "PED:US-CCSS-MATH:G1:U05:L04": "usccss.b01.g1.add_within100_no_compose_ten",
    "PED:US-CCSS-MATH:G3:U06:L04": "usccss.b01.g3.quarter_inch_line_plot_frequency",
    "PED:US-CCSS-MATH:G3:U06:L05": "usccss.b01.g3.quarter_inch_line_plot_frequency",
    "PED:US-CCSS-MATH:G4:U03:L13": [
        "usccss.b01.g3.quarter_inch_line_plot_frequency",
        "usccss.b01.g4.eighth_inch_line_plot_difference",
    ],
    "PED:US-CCSS-MATH:G2:U07:L04": "usccss.b01.g2.add_subtract_1000_no_regroup",
    "PED:US-CCSS-MATH:G2:U05:L09": "usccss.b01.g2.order_three_digits_number_line",
    "PED:US-CCSS-MATH:G4:U03:L10": "usccss.b01.g4.whole_minus_proper_fraction",
    "PED:US-CCSS-MATH:G1:U04:L17": "usccss.b01.g1.compare_order_tens_ones",
    "PED:US-CCSS-MATH:G2:U02:L01": "usccss.b01.g2.add_subtract_without_regroup",
    "PED:US-CCSS-MATH:G2:U02:L07": "usccss.b01.g2.subtract_decompose_ten",
    "PED:US-CCSS-MATH:G1:U04:L03": "usccss.b01.g1.add_subtract_10_tens",
    "PED:US-CCSS-MATH:G1:U04:L04": "usccss.b01.g1.add_subtract_tens_multiples",
    "PED:US-CCSS-MATH:G3:U02:L07": "usccss.b01.geometry.rectangle_area_real_world_units",
    "PED:US-CCSS-MATH:G3:U02:L10": "usccss.b01.geometry.rectangle_area_real_world_units",
    "PED:US-CCSS-MATH:G3:U02:L05": "usccss.b01.geometry.rectangle_area_grid60",
    "PED:US-CCSS-MATH:G3:U02:L09": "usccss.b01.geometry.rectangle_area_ungridded",
    "PED:US-CCSS-MATH:G2:U01:L15": "usccss.b01.g2.compare_tape_unknown_positions",
    "PED:US-CCSS-MATH:G2:U01:L16": "usccss.b01.g2.compare_tape_unknown_positions",
    "PED:US-CCSS-MATH:G2:U03:L15": "usccss.b01.g2.measurement.read_line_plot",
    "PED:US-CCSS-MATH:G2:U03:L16": "usccss.b01.g2.measurement.plot_interpret_nonzero_start",
    "PED:US-CCSS-MATH:G3:U01:L03": "usccss.b01.g3.picture_graph.scaled_compare",
    "PED:US-CCSS-MATH:G3:U01:L04": "usccss.b01.g3.picture_graph.construct",
    "PED:US-CCSS-MATH:G2:U01:L14": "usccss.b01.g2.compare_tape_difference",
    "PED:US-CCSS-MATH:G2:U03:L14": "usccss.b01.g2.measurement.read_line_plot",
    "PED:US-CCSS-MATH:G2:U03:L03": "usccss.b01.ruler.compare_centimeters",
    "PED:US-CCSS-MATH:G2:U03:L10": "usccss.b01.ruler.measure_without_zero",
    "PED:US-CCSS-MATH:G1:U07:L13": "usccss.b01.clock.read_on_hour",
    "PED:US-CCSS-MATH:G2:U06:L13": "usccss.b01.clock.read_five_minutes_am_pm",
    "PED:US-CCSS-MATH:G3:U05:L12": "fractions.equivalent.number_line",
    "PED:US-CCSS-MATH:G4:U02:L04": "fractions.equivalent.number_line",
    "PED:US-CCSS-MATH:G4:U02:L05": "fractions.equivalent.number_line",
    "PED:US-CCSS-MATH:G4:U02:L08": "fractions.equivalent.number_line",
    "PED:US-CCSS-MATH:G3:U03:L03": "usccss.b01.g3.add_within1000",
    "PED:US-CCSS-MATH:G3:U03:L05": "usccss.b01.g3.add_regroup1000",
    "PED:US-CCSS-MATH:G3:U03:L07": "usccss.b01.g3.subtract_within1000",
    "PED:US-CCSS-MATH:G3:U03:L08": "usccss.b01.g3.subtract_regroup1000",
    "PED:US-CCSS-MATH:G3:U03:L09": "usccss.b01.g3.subtract_regroup1000",
    "PED:US-CCSS-MATH:G3:U03:L10": "usccss.b01.g3.subtract_regroup1000",
    "PED:US-CCSS-MATH:G3:U03:L11": "usccss.b01.g3.subtract_across_zero",
    "PED:US-CCSS-MATH:G3:U03:L12": "usccss.b01.g3.subtract_within1000",
    "PED:US-CCSS-MATH:G4:U03:L16": "usccss.b01.fraction.tenths_hundredths_within_one",
    "PED:US-CCSS-MATH:G4:U03:L17": "usccss.b01.fraction.tenths_hundredths_over_one",
    "PED:US-CCSS-MATH:G2:U04:L01": "usccss.b01.number_line.locate_whole_to100",
    "PED:US-CCSS-MATH:G2:U04:L03": "usccss.b01.number_line.locate_whole_to100",
    "PED:US-CCSS-MATH:G2:U04:L04": "usccss.b01.number_line.move_left_right_within100",
    "PED:US-CCSS-MATH:G2:U04:L05": "usccss.b01.number_line.estimate_location_within100",
    "PED:US-CCSS-MATH:G2:U04:L06": [
        "usccss.b01.number_line.locate_whole_to100",
        "number.whole.add_subtract.within_100.apply",
        "number.whole.add_subtract.within_100.build",
    ],
    "PED:US-CCSS-MATH:G2:U04:L10": [
        "number.whole.add_subtract.within_100.apply",
        "number.whole.add_subtract.within_100.build",
    ],
    "PED:US-CCSS-MATH:G2:U05:L08": "usccss.b01.number_line.locate_three_digits",
    "PED:US-CCSS-MATH:G4:U04:L11": "usccss.b01.number_line.locate_large_numbers",
    "PED:US-CCSS-MATH:G2:U06:L01": "usccss.b01.shape.polygon_sides",
    "PED:US-CCSS-MATH:G3:U07:L02": "usccss.b01.shape.specific_subcategory",
    "PED:US-CCSS-MATH:G3:U02:L14": "usccss.b01.area.missing_composite_side",
    # Official IM G1 U01 L02: dot pictures, missing dots and Check It Off.
    "PED:US-CCSS-MATH:G1:U01:L02": [
        "usccss.b01.g1.match_dot_cubes_expression",
        "usccss.b01.g1.complete_missing_dots",
        "usccss.b01.g1.check_off_add_within10",
    ],
    "PED:US-CCSS-MATH:G3:U02:L04": "usccss.b01.geometry.rectangle_area_grid60",
    "PED:US-CCSS-MATH:G3:U02:L08": "usccss.b01.geometry.rectangle_area_ungridded",
    "PED:US-CCSS-MATH:G3:U02:L12": "usccss.b01.geometry.composite_rectangle_area",
    "PED:US-CCSS-MATH:G3:U02:L13": "usccss.b01.geometry.composite_rectangle_area",
    "PED:US-CCSS-MATH:G3:U01:L07": "usccss.b01.graph.scaled_compare",
    "PED:US-CCSS-MATH:G1:U04:L08": "usccss.b01.place_value.tens_ones",
    "PED:US-CCSS-MATH:G1:U04:L09": "usccss.b01.place_value.tens_ones",
    "PED:US-CCSS-MATH:G1:U04:L10": "usccss.b01.place_value.tens_ones",
    "PED:US-CCSS-MATH:G1:U08:L07": "usccss.b01.place_value.count_to120",
    "PED:US-CCSS-MATH:G1:U08:L08": "usccss.b01.place_value.tens_ones",
    "PED:US-CCSS-MATH:G2:U05:L04": "usccss.b01.place_value.three_digits",
    "PED:US-CCSS-MATH:G3:U05:L02": "usccss.b01.fraction.unit_parts",
    "PED:US-CCSS-MATH:G2:U08:L07": "usccss.b01.array.equal_groups",
    "PED:US-CCSS-MATH:G2:U08:L08": "usccss.b01.array.columns",
    "PED:US-CCSS-MATH:G2:U08:L09": "usccss.b01.array.equal_addends",
    "PED:US-CCSS-MATH:G2:U08:L10": "usccss.b01.array.equal_equation",
    "PED:US-CCSS-MATH:G2:U08:L11": "usccss.b01.rectangle.unit_squares",
    "PED:US-CCSS-MATH:G3:U01:L17": "usccss.b01.array.equal_groups",
    "PED:US-CCSS-MATH:G3:U01:L19": "usccss.b01.array.unknown_factor",
    "PED:US-CCSS-MATH:G3:U01:L20": "usccss.b01.array.commutativity",
    "PED:US-CCSS-MATH:G3:U02:L02": "usccss.b01.rectangle.unit_squares",
    "PED:US-CCSS-MATH:G1:U07:L05": "usccss.b01.triangle.attributes",
    "PED:US-CCSS-MATH:G1:U07:L09": "usccss.b01.partition.halves_fourths",
    "PED:US-CCSS-MATH:G1:U07:L10": "usccss.b01.partition.halves_fourths",
    "PED:US-CCSS-MATH:G2:U04:L02": "usccss.b01.number_line.equal_intervals",
    "PED:US-CCSS-MATH:G2:U06:L11": "usccss.b01.clock.half_quarters",
    "PED:US-CCSS-MATH:G1:U07:L14": "usccss.b01.clock.read_half_hour",
    "PED:US-CCSS-MATH:G1:U07:L15": "usccss.b01.clock.read_half_hour",
    "PED:US-CCSS-MATH:G2:U06:L12": "usccss.b01.clock.read_five_minutes",
    "PED:US-CCSS-MATH:G2:U08:L12": "usccss.b01.rectangle.unit_squares",
    "PED:US-CCSS-MATH:G1:U06:L07": [
        "supporting.measurement.unit_iteration",
        "usccss.b01.g1.measure.same_length_different_units",
    ],
    "PED:US-CCSS-MATH:G2:U03:L06": "usccss.b01.g2.length_compare.all_unknown_positions100",
    "PED:US-CCSS-MATH:G2:U03:L05": "usccss.b01.g2.measure.reptile_meterstick",
    "PED:US-CCSS-MATH:G2:U03:L04": [
        "usccss.b01.g2.measurement.estimate_cm",
        "usccss.b01.g2.measurement.iterate_ruler_cm",
    ],
    "PED:US-CCSS-MATH:G2:U09:L04": "usccss.b01.g2.measurement.pencil_line_plot",
    "PED:US-CCSS-MATH:G2:U09:L03": "usccss.b01.measure.map_lengths",
    "PED:US-CCSS-MATH:G3:U01:L06": "usccss.b01.graph.choose_scale",
    "PED:US-CCSS-MATH:G3:U01:L16": "usccss.b01.array.equal_groups",
    "PED:US-CCSS-MATH:G3:U01:L18": "usccss.b01.array.equal_groups",
    "PED:US-CCSS-MATH:G3:U01:L21": "usccss.b01.array.equal_groups",
    "PED:US-CCSS-MATH:G3:U02:L03": "usccss.b01.rectangle.unit_squares",
    "PED:US-CCSS-MATH:G3:U02:L11": "usccss.b01.rectangle.unit_squares",
    "PED:US-CCSS-MATH:G3:U02:L15": "usccss.b01.geometry.composite_rectangle_area",
    "PED:US-CCSS-MATH:G3:U04:L06": "usccss.b01.division.unknown_factor",
    "PED:US-CCSS-MATH:G3:U05:L01": "usccss.b01.fraction.name_parts",
    "PED:US-CCSS-MATH:G3:U06:L02": "usccss.b01.measure.quarter_inch",
    "PED:US-CCSS-MATH:G4:U01:L03": "usccss.b01.factors.prime_composite",
    "PED:US-CCSS-MATH:G4:U01:L06": "usccss.b01.factors.locker_square",
    "PED:US-CCSS-MATH:G4:U01:L07": "usccss.b01.factors.factors_multiples",
    "PED:US-CCSS-MATH:G4:U01:L08": "usccss.b01.factors.factor_pairs",
    "PED:US-CCSS-MATH:G4:U03:L04": "usccss.b01.fraction.whole_multiply",
    "PED:US-CCSS-MATH:G4:U03:L07": "usccss.b01.fraction.decompose_same_denominator",
    "PED:US-CCSS-MATH:G4:U04:L10": "usccss.b01.place_value.ten_times",
    "PED:US-CCSS-MATH:G2:U02:L16": "usccss.b01.market.inventory_100",
}

def analyze(runtime, publisher):
    a=runtime["lessonRows"]; b=publisher["rows"]
    if len(a)!=500 or len(b)!=500:
        raise ValueError("B01 must have 500 real runtime and 500 official source records")
    by_code={x["lessonCode"]:x for x in b}
    if len(by_code)!=500:
        raise ValueError("Duplicate or missing publisher lesson identities")
    results=[]
    for x in a:
        origin=by_code.get(x["lessonCode"])
        if not origin:raise ValueError("Missing source for "+x["lessonCode"])
        purpose=(origin.get("sourcePurposePreview") or "").lower()
        title=(origin.get("publisherTitle") or "").lower()
        context=title+" "+purpose
        flagged=[]
        actual_families = x.get("actualQuestionFamilies") or []
        exact_family = SOURCE_SCOPED_FAMILIES.get(x["lessonCode"])
        expected_families = (
            exact_family if isinstance(exact_family,list) else
            [exact_family] if exact_family is not None else None
        )
        if expected_families is not None and actual_families != expected_families:
            flagged.append({"rule":"SOURCE_TASK_EXACT_LESSON_FAMILY_DRIFT",
                            "expected":expected_families, "actual":actual_families})
        if exact_family is None and any(f.startswith("usccss.b01.") for f in actual_families):
            flagged.append({"rule":"SOURCE_TASK_LEAKED_OUTSIDE_EXACT_LESSON_SCOPE",
                            "actual":actual_families})
        for family in actual_families:
            for key,fn in RULES:
                if fn(family,context):
                    flagged.append({"rule":key,"family":family})
        status="HIGH_CONFIDENCE_PRACTICE_SOURCE_CONFLICT" if flagged else "NOT_FLAGGED_NOT_CERTIFIED"
        results.append({"ordinal":x["ordinal"],"lessonCode":x["lessonCode"],
                        "publisherTitle":origin.get("publisherTitle"),
                        "officialSourceUrl":origin.get("sourceUrl"),
                        "contentSha256":x.get("effectiveContentSha256"),
                        "currentSkillId":x.get("practiceSkillId"),
                        "families":x.get("actualQuestionFamilies"),
                        "status":status,"conflicts":flagged})
    from collections import Counter
    grouped=Counter(f["rule"] for r in results for f in r["conflicts"])
    return {
      "kind":"B01_500_PRACTICE_SOURCE_SEMANTIC_GUARD",
      "total":500,"checked":len(results),
      "sourceTaskMappingsRequired":len(SOURCE_SCOPED_FAMILIES),
      "conflictingLessons":sum(bool(x["conflicts"]) for x in results),
      "conflictsByCategory":dict(grouped),"rows":results,
      "academicDecision":"NOT_CERTIFIED",
      "note":"High confidence contradiction rules; a row with zero flags is NOT academically certified or proven aligned. Publisher preparation texts, full activities and reviewer/independent semantic oracle still needed."
    }

def main():
    ap=argparse.ArgumentParser()
    ap.add_argument("--runtime",required=True)
    ap.add_argument("--publisher",required=True)
    ap.add_argument("--output",default="b01-practice-source-conflicts-500.json")
    ap.add_argument("--strict",action="store_true")
    args=ap.parse_args()
    data=analyze(json.loads(pathlib.Path(args.runtime).read_text()),
                 json.loads(pathlib.Path(args.publisher).read_text()))
    pathlib.Path(args.output).write_text(json.dumps(data,indent=2,ensure_ascii=False)+"\n")
    print(json.dumps({k:v for k,v in data.items() if k!="rows"},indent=2))
    if args.strict and data["conflictingLessons"]>0:
        raise SystemExit(f"RELEASE BLOCKED: {data['conflictingLessons']} lesson-scoped Practice targets contradict publisher purpose. L5 cannot be PASS.")
if __name__=="__main__":main()
