from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
rpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
j=json.loads(rpath.read_text(encoding="utf-8"))
lesson=j["Lessons"][0]
assert len(lesson["WorkedExamples"])==13 and len(lesson["Visuals"])==4
row=lambda lab,items:{"Label":lab,"Cells":list(items)}
visuals=[
 {"Kind":"RatioTable","Title":"Basic toolkit function real domain and range","Description":"Independent first-party mathematical relationships matching all nine source toolkit skills on pages 45–46.","PrimaryLabel":"Family","SecondaryLabel":"Domain and range","Rows":[
 row("Family",["Constant","Identity","Absolute value","Quadratic","Cubic","Reciprocal","Reciprocal squared","Square root","Cube root"]),
 row("Formula",["f(x)=c","f(x)=x","f(x)=|x|","f(x)=x²","f(x)=x³","f(x)=1/x","f(x)=1/x²","f(x)=√x","f(x)=∛x"]),
 row("Real domain",["(−∞,∞)","(−∞,∞)","(−∞,∞)","(−∞,∞)","(−∞,∞)","(−∞,0)∪(0,∞)","(−∞,0)∪(0,∞)","[0,∞)","(−∞,∞)"]),
 row("Real range",["{c}","(−∞,∞)","[0,∞)","[0,∞)","(−∞,∞)","(−∞,0)∪(0,∞)","(0,∞)","[0,∞)","(−∞,∞)"])
 ],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Interval notation and endpoint inclusion","Description":"A bracket includes the finite endpoint; a parenthesis excludes it, including both infinite endpoints.","PrimaryLabel":"Inequality","SecondaryLabel":"Interval","Rows":[
 row("Condition",["x>a","x≤b","a<x≤b","a≤x<b","x≠c","x∈ℝ"]),
 row("Interval",["(a,∞)","(−∞,b]","(a,b]","[a,b)","(−∞,c)∪(c,∞)","(−∞,∞)"])
 ],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Piecewise branch boundary and output","Description":"The branches are disjoint by endpoint conditions; strict inequalities omit the endpoint.","PrimaryLabel":"Branch","SecondaryLabel":"Expression","Rows":[
 row("Allowed input",["x≤0","0<x≤3","x>3"]),
 row("f(x)",["x²","2","x−1"]),
 row("Endpoint",["filled (0,0)","open (0,2) and filled (3,2)","open (3,2)"])
 ],"Items":[],"SecondaryItems":[]}
]
lesson["Visuals"].extend(visuals)
cooldown={
"Title":"Edulytics cool-down: domain, range and one piecewise boundary",
"Prompt":"(a) State the domain of f(x)=(x+1)/(x−2). (b) Give domain and range of g(x)=−√(9−x). (c) For h(x)=x+2 when x<0 and h(x)=x−1 when x≥0, evaluate h(0) and explain which point is closed.",
"ExpectedMathematics":"(a) (−∞,2)∪(2,∞); (b) domain (−∞,9], range (−∞,0]; (c) h(0)=−1, with (0,−1) filled and (0,2) open.",
"TeacherReviewMode":"MANUAL_THREE_SUBPARTS",
"OriginalPublisherRestrictedCoolDownCopied":False,
"SourceSectionOriginalCoolDownAvailable":False,
"StageDRuntime":"NOT_RUN"
}
lesson["ExplanationParagraphs"].append("Independent Edulytics cool-down — "+cooldown["Prompt"]+" Submit separate rational-domain, radical-range and piecewise-endpoint explanations for teacher assessment.")
lesson["CommonMistakes"].append({
 "Mistake":"Using an included branch boundary to assign two different values at the same x.",
 "WhyWrong":"A piecewise function must have a single active formula at every permitted input.",
 "Correction":"Before evaluating a boundary, check exactly one inequality; a strict endpoint uses an open circle."
})
j["ContentVersion"]="g12-u01-l02-source-nine-toolkit-ranges-and-firstparty-cooldown-20261106"
rpath.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l02-first-party-cooldown.json").write_text(json.dumps(cooldown,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l02-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["SourceToolkitFunctionFamiliesWithDomainRange"]=9
c["FirstPartyIntervalAndEndpointVisualTablesAuthored"]=2
c["FirstPartyCoolDownAuthored"]=True
c["FirstPartyRichV2VisualTablesAuthored"]=7
c["SourceOriginalMediaCopied"]=0
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==1 and p["FirstIncompleteLessonOrdinal"]==2
e=p["LessonEntries"][1]
e["FirstPartyRichV2VisualTablesAuthored"]=7
e["SourceToolkitFunctionFamiliesWithDomainRange"]=9
e["FirstPartyCoolDownAuthored"]=True
e["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U01_L02_TOOLKIT_9_FUNCTION_DOMAIN_RANGE_PROFILES_AND_3_PART_COOLDOWN_AUTHORED")
