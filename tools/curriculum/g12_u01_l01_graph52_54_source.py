#!/usr/bin/env python3
"""Reviewed source PDF p36 graphical exercises 52–54, with uncertain x-read fail-closed."""
import json,datetime
from pathlib import Path
base=Path(__file__).resolve().parents[2]/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=base/"u01-l01-element-coverage.json"
j=json.loads(p.read_text(encoding="utf8"))
source={
 52:{
   "observed":"Curved increasing ray starts at x=−2, y=0; at x=−1 its height is about 1; near right edge x≈6 its y approaches 3.",
   "part_a":"f(−1)≈1 from visible grid.",
   "part_b":"Read y=3 at the right edge/extension, but an exact x-coordinate cannot be established reliably from the reproduced graph alone.",
   "all_subparts_verified":False},
 53:{
   "observed":"Symmetric W-shaped curve, central local maximum at (0,1), two minima at (−2,−3) and (2,−3).",
   "part_a":"f(0)=1.",
   "part_b":"f(x)=−3 at x=−2 and x=2.",
   "all_subparts_verified":True},
 54:{
   "observed":"Piecewise linear inverted-V curve with apex (−3,4); left segment slope +1, right segment slope −1.",
   "part_a":"f(4)=−3.",
   "part_b":"f(x)=1 at x=−6 or x=0.",
   "all_subparts_verified":True}
}
for x in j["Elements"]:
 if x["Category"]!="EXERCISE":continue
 n=int(x["SourceElementId"].rsplit("EXERCISE",1)[-1])
 if n not in source:continue
 v=source[n]
 x["SourceVisualMathDescription"]=v["observed"]
 x["SourceMathPartA"]=v["part_a"]
 x["SourceMathPartB"]=v["part_b"]
 x["SourceMathAllSubpartsVerified"]=v["all_subparts_verified"]
 x["VerificationMethod"]="LOCAL_SOURCE_PDF_PAGE36_GRAPH_VISUAL_REVIEW"
 x["Status"]="SOURCE_VERIFIED" if v["all_subparts_verified"] else "SOURCE_IDENTIFIED"
 x["DeliveryStatus"]="NOT_YET_IMPLEMENTED"
 x["ImplementationType"]="GRAPH_TASK_REQUIRES_TEACHER_REVIEW_OR_ACCESSIBLE_GRAPH_INTERACTION"
 x["OriginalImageCopied"]=False
 x["RightsStatus"]="SOURCE_PDF_GRAPH_RIGHTS_REQUIRE_SEPARATE_REVIEW"
j["SourceMathVisuallyInspectedExerciseCount"]=91
j["SourceMathUninspectedOriginalExerciseNumbers"]=[52]
j["PartiallyInterpretedSourceExercises"]=[52]
j["SourceSemanticCoverageIsNotEdulyticsParity"]=True
j["MatchedDigitally"]=j["MatchedTeacherLed"]=0
j["AtomicSourceDenominatorVerified"]=False
j["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
p.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
progress=base/"full-grade-progress.json"
d=json.loads(progress.read_text(encoding="utf8"))
d["LessonEntries"][0]["SourceExercisesIndividuallyInterpreted"]=91
d["LessonEntries"][0]["SourceExercisesPartialNumbers"]=[52]
d["LessonEntries"][0]["SourceElementsIndependentlyMatched"]=0
progress.write_text(json.dumps(d,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
print("G12_U01_L01_SOURCE_MATH_REVIEWED_91_OF_92_EXERCISES_PARTIAL_52_EDULYTICS_MATCHED_ZERO")
