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

# 29 original publisher lesson targets: exact approved code/family linkage.
# This protects against silently falling back to unrelated generic engines.
SOURCE_SCOPED_FAMILIES = {
    "PED:US-CCSS-MATH:G1:U07:L05": "usccss.b01.triangle.attributes",
    "PED:US-CCSS-MATH:G1:U07:L09": "usccss.b01.partition.halves_fourths",
    "PED:US-CCSS-MATH:G1:U07:L10": "usccss.b01.partition.halves_fourths",
    "PED:US-CCSS-MATH:G2:U04:L02": "usccss.b01.number_line.equal_intervals",
    "PED:US-CCSS-MATH:G2:U06:L11": "usccss.b01.clock.half_quarters",
    "PED:US-CCSS-MATH:G1:U07:L14": "usccss.b01.clock.read_half_hour",
    "PED:US-CCSS-MATH:G1:U07:L15": "usccss.b01.clock.read_half_hour",
    "PED:US-CCSS-MATH:G2:U06:L12": "usccss.b01.clock.read_five_minutes",
    "PED:US-CCSS-MATH:G2:U08:L12": "usccss.b01.rectangle.unit_squares",
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
    "PED:US-CCSS-MATH:G4:U01:L02": "usccss.b01.factors.factor_pairs",
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
        if exact_family is not None and actual_families != [exact_family]:
            flagged.append({"rule":"SOURCE_TASK_EXACT_LESSON_FAMILY_DRIFT",
                            "expected":exact_family, "actual":actual_families})
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
