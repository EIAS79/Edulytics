#!/usr/bin/env python3
"""B01 500-at-once publisher-goal vs *real* Rich V2 representation coverage.

Never certifies lessons. A semantic cue is a pedagogical review requirement,
not proof that any detected mismatch is mathematically incorrect.
"""
from collections import Counter
import argparse
import json
import pathlib
import re

CUES = [
 ("FRACTION_PARTS",r"\bfraction|halves|fourths|tenths|hundredths|equal parts",
  {"FractionBars"}),
 ("RECTANGULAR_ARRAY",r"\barrays?\b|square tiles|unit squares|partition rectangles|tile rectangles",
  {"ShapeDecomposition"}),
 ("NUMBER_LINE",r"\bnumber line\b|\bline plot\b",{"NumberLine","Scale"}),
 ("GRAPH_AND_SCALE",r"\bbar graph\b|\bpicture graph\b|\bscaled graph\b|\bgraph scales?\b",
  {"Scale","BarGraph","RatioTable","ShapeDecomposition"}),
 ("PLACE_VALUE",r"\bplace value\b|expanded form|base.ten|digit value",
  {"PlaceValueChart"}),
 ("AREA_MODEL",r"\barea of\b|\barea by\b|\barea with\b|\barea and\b",
  {"ShapeDecomposition","PlaceValueChart"}),
 ("TRIANGLE_SHAPES",r"\btriangles?\b|defining attributes of shapes",
  {"ShapeDecomposition"}),
 ("MEASUREMENT_SCALE",r"\bruler\b|measur(?:e|ing) lengths|\bmeasurement units\b",
  {"NumberLine","Scale"}),
 ("CLOCK_DIAGRAM",r"\banalog clock\b|\bclock hands\b|\bquarter past\b|\bhalf past\b",
  {"ClockFace"})
]

def analyze(publisher,rich):
    a=publisher.get("rows") or []
    b=rich.get("rows") or []
    if len(a)!=500 or len(b)!=500:
        raise ValueError("B01 must contain 500 unique official sources and 500 real Rich V2 bodies")
    pub={r["lessonCode"]:r for r in a}
    rv={r["lessonCode"]:r for r in b}
    if len(pub)!=500 or len(rv)!=500 or pub.keys()!=rv.keys():
        raise ValueError("B01 lesson identity mismatch or duplicates")
    results=[]
    categories=Counter()
    generic_only=0
    source_sha_mismatches=0
    flagged=0
    for source in a:
        code=source["lessonCode"]
        rendered=rv[code]
        if source["ordinal"]!=rendered["ordinal"]:
            raise ValueError("B01 source and Rich V2 ordering mismatch: "+code)
        sha_match=(source.get("effectiveContentSha256")==
                   rendered.get("effectiveBodyFingerprint"))
        if not sha_match:
            source_sha_mismatches+=1
        kinds=set(rendered.get("visualKinds") or [])
        if kinds=={"EquationSet"}:
            generic_only+=1
        intro=(source.get("publisherTitle") or "")+" "+(source.get("sourcePurposePreview") or "")
        demands=[]
        for name,phrase,suitable in CUES:
            if re.search(phrase,intro,re.I):
                # Publisher time-telling lessons may say "halves and quarters"
                # referring to positions of a clock hand, not fractional
                # shaded-piece task statements. A real ClockFace meets that
                # source need; demanding FractionBars here is a false alarm.
                if (name=="FRACTION_PARTS" and "ClockFace" in kinds and
                    re.search(r"\\bclock|tell time|write times|half.past|quarter.past",intro,re.I)):
                    continue
                if name=="CLOCK_DIAGRAM":
                    # There is no first-party clock-face renderer in current Rich V2.
                    # Generic scales are not a substitute for analog hand placement.
                    compatible="ClockFace" in kinds
                else:
                    compatible=bool(kinds.intersection(suitable))
                if not compatible:
                    demands.append(name)
                    categories[name]+=1
        if demands or not sha_match:
            flagged+=1
        results.append({
            "ordinal":source["ordinal"],"lessonCode":code,
            "officialSourceUrl":source.get("sourceUrl"),
            "publisherTitle":source.get("publisherTitle"),
            "sourceEffectiveContentSha256":source.get("effectiveContentSha256"),
            "richV2EffectiveContentSha256":rendered.get("effectiveBodyFingerprint"),
            "sameEffectiveContent":sha_match,
            "actualRichV2VisualKinds":sorted(kinds),
            "publisherCueVisualReviewTypes":demands,
            "status":"REVIEW_REQUIRED_NOT_CERTIFIED" if (demands or not sha_match)
                else "SCREENED_NOT_CERTIFIED"
        })
    return {
        "kind":"B01_500_REAL_VISUALS_VS_OFFICIAL_LESSON_PURPOSES",
        "expected":500,"observed":len(results),
        "lessonsWithOnlyGenericEquationSet":generic_only,
        "lessonsNeedingPedagogicalVisualReview":flagged,
        "effectiveContentShaMismatches":source_sha_mismatches,
        "visualReviewTypes":dict(sorted(categories.items())),
        "rows":results,
        "academicDecision":"NOT_CERTIFIED",
        "warning":"Absence of a cue does NOT imply L4 PASS. Presence of a component does NOT establish mathematical correctness, accessibility or source activity equivalence. ClockFace is a missing first-party Rich V2 kind."
    }

def main():
    ap=argparse.ArgumentParser()
    ap.add_argument("--publisher",required=True)
    ap.add_argument("--rich",required=True)
    ap.add_argument("--output",default="b01-rich-v2-source-visual-gaps-500.json")
    ap.add_argument("--strict",action="store_true")
    args=ap.parse_args()
    data=analyze(json.loads(pathlib.Path(args.publisher).read_text()),
                 json.loads(pathlib.Path(args.rich).read_text()))
    pathlib.Path(args.output).write_text(json.dumps(data,indent=2,ensure_ascii=False)+"\n")
    print(json.dumps({k:v for k,v in data.items() if k!="rows"},indent=2))
    if args.strict and (data["lessonsNeedingPedagogicalVisualReview"] or
                        data["effectiveContentShaMismatches"]):
        raise SystemExit("B01 L4 REJECTED: publisher-goal visual evidence is incomplete.")
if __name__=="__main__":main()
