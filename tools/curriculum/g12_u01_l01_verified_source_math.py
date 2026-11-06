#!/usr/bin/env python3
"""Manual checked source math for PDF section 1.1, pp35–37. Not Edulytics parity."""
import json,datetime
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=BASE/"u01-l01-element-coverage.json"
data=json.loads(p.read_text(encoding="utf8"))
answers={}
for num,yes in {
  40:True,41:False,42:True,43:True,44:False,45:True,
  46:True,47:True,48:False,49:True,50:False,51:True
}.items():
 answers[num]={
  "SourceFinding":("Passes" if yes else "Fails")+" the vertical-line test.",
  "SourceVerification":"VISUALLY_INSPECTED_PDF_PAGE35; no original figure copied"}
for num,yes in {55:False,56:False,57:True,58:True,59:False}.items():
 answers[num]={
  "SourceFinding":("One-to-one increasing/decreasing function" if yes else
                  ("Not a function" if num==55 else "Function, not one-to-one")),
  "SourceVerification":"VISUALLY_INSPECTED_PDF_PAGE36; horizontal-line test"}
for num,find in {
  60:"Function: inputs −1, −2, −3 are distinct.",
  61:"Function: inputs 3, 4, 5 are distinct.",
  62:"Not a function: input 7 has outputs 11 and 9.",
  63:"Function: x=5,10,15 maps uniquely to 3,8,14.",
  64:"Function, not one-to-one: 10 and 15 both map to 8.",
  65:"Not a function: x=10 maps to 8 and 14.",
  66:"From Table 14, f(3)=53.",
  67:"From Table 14, f(x)=1 at x=2."
}.items():
 answers[num]={"SourceFinding":find,"SourceVerification":"VISUALLY_INSPECTED_PDF_PAGE36"}
for num,find in {
  68:"f(x)=4−2x for x=−2,−1,0,1,2 gives 8,6,4,2,0.",
  69:"f(x)=8−3x for x=−2,−1,0,1,2 gives 14,11,8,5,2.",
  70:"f(x)=8x²−7x+3 gives 49,18,3,4,21 for inputs −2,−1,0,1,2.",
  71:"f(x)=3+√(x+3) gives 4,3+√2,3+√3,5,3+√5 for inputs −2,−1,0,1,2.",
  72:"f(x)=(x−2)/(x+3) gives −4,−3/2,−2/3,−1/4,0 for inputs −2,−1,0,1,2; domain excludes −3.",
  73:"f(x)=3^x gives 1/9,1/3,1,3,9 for inputs −2,−1,0,1,2.",
  74:"With f(x)=3x−2 and g(x)=5−x², 3f(1)−4g(−2)=−1.",
  75:"With f(x)=3x−2 and h(x)=−2x²+3x−1, f(7/3)−h(−2)=20.",
  76:"For y=x² on [−0.1,0.1], range [0,0.01].",
  77:"For y=x² on [−10,10], range [0,100].",
  78:"For y=x² on [−100,100], range [0,10000].",
  79:"For y=x³ on [−0.1,0.1], range [−0.001,0.001].",
  80:"For y=x³ on [−10,10], range [−1000,1000].",
  81:"For y=x³ on [−100,100], range [−1000000,1000000].",
  82:"For y=√x on [0,0.01], range [0,0.1].",
  83:"For y=√x on [0,100], range [0,10].",
  84:"For y=√x on [0,10000], range [0,100].",
  85:"For y=cuberoot(x) on [−0.001,0.001], range [−0.1,0.1].",
  86:"For y=cuberoot(x) on [−1000,1000], range [−10,10].",
  87:"For y=cuberoot(x) on [−1000000,1000000], range [−100,100].",
  88:"Garbage G=f(p) with p in thousands: f(40)=13 tonnes/week; f(5)=2 means 5,000 people generate 2 tonnes/week.",
  89:"Dirt D=g(a): g(5000)=50 cubic yards; g(100)=1 means 100 square feet needs one cubic yard.",
  90:"f(t) ducks t years after 1990: f(5)=30 means 30 ducks in 1995; f(10)=40 means 40 ducks in 2000.",
  91:"h(t) rocket height in feet t seconds after launch: h(1)=200ft and h(2)=350ft.",
  92:"3(x−5)²+7 is not one-to-one on R: f(4)=f(6)=10."
}.items():
 answers[num]={"SourceFinding":find,
               "SourceVerification":"VISUALLY_INSPECTED_PDF_PAGE37_AND_CHECKED_MATHEMATICS"}
assert len(answers)==50,len(answers)
for item in data["Elements"]:
 if item["Category"]!="EXERCISE":continue
 n=int(item["SourceElementId"].split("EXERCISE")[-1])
 if n in answers:
  assert item["Status"]=="SOURCE_IDENTIFIED"
  item["Status"]="SOURCE_VERIFIED"
  item["SourceMathCheck"]=answers[n]["SourceFinding"]
  item["VerificationMethod"]=answers[n]["SourceVerification"]
  item["DeliveryStatus"]="NOT_YET_IMPLEMENTED"
  item["ImplementationType"]="PENDING_GRADE12_DIGITAL_OR_TEACHER_ASSESSMENT"
# Three original graph examples were reviewed, but their images are not reproduced.
figure_notes={
 12:"Original Fig12 shows a cubic-like function, a decreasing straight-line function, and a circle that fails vertical-line test.",
 13:"Original Fig13 shows two intersections of one vertical line with a circle; not y=f(x).",
 14:"Original Fig14 shows V-shaped absolute-value graph; it passes vertical-line test.",
 15:"Original Fig15 shows a horizontal line intersecting a non-monotone function more than once; not injective."
}
for item in data["Elements"]:
 if item["Category"]!="FIG":continue
 n=int(item["SourceElementId"].split("FIG")[-1])
 if n in figure_notes:
  item["SourceVisualMathDescription"]=figure_notes[n]
  item["SourceVisualInspected"]=True
  item["OriginalImageCopied"]=False
  item["RightsStatus"]="REQUIRES_SEPARATE_VISUAL_PROVENANCE_CHECK"
  item["Status"]="SOURCE_VERIFIED"
  item["ImplementationType"]="UNRENDERED_SOURCE_VISUAL"
data["SourceMathVisuallyInspectedExerciseCount"]=50
data["SourceMathVisuallyInspectedFigureCount"]=4
data["SourceMathInspectedIsNotEdulyticsParity"]=True
data["SourceAtomicDenominatorVerified"]=False
data["MatchedDigitally"]=0
data["MatchedTeacherLed"]=0
data["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
p.write_text(json.dumps(data,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
progress=BASE/"full-grade-progress.json"
j=json.loads(progress.read_text(encoding="utf8"))
j["LessonEntries"][0]["SourceExercisesIndividuallyInterpreted"]=50
j["LessonEntries"][0]["SourceFiguresIndividuallyInterpreted"]=4
j["LessonEntries"][0]["SourceElementsIndependentlyMatched"]=0
progress.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
print("G12_U01_L01_SOURCE_MATH_INSPECTED",50,"EXERCISES",4,"FIGURES","MATCHED_IN_EDULYTICS=0")
