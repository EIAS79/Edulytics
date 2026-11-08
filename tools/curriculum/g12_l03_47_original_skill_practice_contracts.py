from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson003.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.3"
questions={}
def add(n,typ,question,teacher,review="Teacher manually verifies all mathematical outputs, input intervals and units; no auto-grading claim."):
    assert n not in questions,n
    questions[n]={"Number":n,"Category":typ,"FirstPartyStudentQuestion":question,
                  "TeacherExpectedMathematics":teacher,"TeacherManualReviewGuidance":review,
                  "SourcePDFPage":64 if n<=20 else 65 if n<=27 else 66,
                  "SourceSkillMathReviewed":True,"OriginalPublisherSourceFigureCopied":False,
                  "FullyAutomaticallyGraded":False,"StageDRuntime":"NOT_RUN"}
concept=[
("Does any nonconstant function have a constant average rate of change on every interval? Construct an example.","Yes. Linear f(x)=mx+b has average rate m for every distinct pair of inputs."),
("Suppose a continuous graph rises before x=b and falls after x=b. What can be deduced about an internal turning point at b?","A local maximum occurs at b if f is defined there and the stated behavior holds in a neighborhood."),
("Distinguish local and absolute maxima/minima, including whether a restricted graph endpoint can be absolute.","Local compares nearby values; absolute compares all permitted outputs; included endpoints may realize absolute extrema."),
("Compare the monotonic intervals of y=|x| and y=x² on their usual real domains.","Both decrease on (−∞,0), increase on (0,∞), and attain global minimum zero at x=0; |x| has a cusp.")
]
for i,(q,a)in enumerate(concept,1):add(i,"VERBAL",q,a)
algebra={
5:("4x²−7","[1,b]","4(b+1) for b≠1"),
6:("2x²−9","[4,b]","2(b+4) for b≠4"),
7:("3x+4","[2,2+h]","3 for h≠0"),
8:("4x−2","[3,3+h]","4 for h≠0"),
9:("2x²+1","[x,x+h]","4x+2h for h≠0"),
10:("3x²−2","[x,x+h]","6x+3h for h≠0"),
11:("1/(t+4)","[9,9+h]","−1/[13(13+h)] for h≠0 and h≠−13"),
12:("1/(x+3)","[1,1+h]","−1/[4(4+h)] for h≠0 and h≠−4"),
13:("3x³","[1,1+h]","9+9h+3h² for h≠0"),
14:("4t³","[2,2+h]","48+24h+4h² for h≠0"),
15:("2x²−3x","[x,x+h]","4x+2h−3 for h≠0")
}
for n,(formula,interval,answer)in algebra.items():
    add(n,"SYMBOLIC_AVERAGE_RATE",
        f"Compute the exact secant average rate of change for f(z)={formula} on {interval}. Retain all nonzero denominator and function-domain restrictions, and simplify.",
        answer)
# The original Section 1.3 figures 15, 16 and 17 have source-specific
# multi-turning curves. Independently authored first-party graph-model tasks
# retain reading-slope, monotonicity, and extrema without copying pixels.
add(16,"GRAPH_SECANT",
    "A first-party plotted model contains (1,4),(4,10). Estimate average rate over the displayed [1,4] interval using the two labelled points.",
    "2 output units/input unit; (10−4)/(4−1)=2.","Teacher checks plotted endpoint coordinates.")
add(17,"GRAPH_SECANT",
    "A continuous independent plotted model contains (2,9),(5,3). Compute average rate on [2,5] from the plotted points.",
    "−2 output units/input unit; (3−9)/(5−2)=−2.")
independent_graphs={
18:("A smooth continuous curve rises on (−∞,−2), falls (−2,1), then rises (1,∞).","Increasing (−∞,−2)∪(1,∞), decreasing (−2,1)."),
19:("A V-shaped absolute-value curve with vertex at x=1, extending indefinitely left/right.","Decreasing (−∞,1), increasing (1,∞)."),
20:("A strictly increasing S-shaped cubic from negative to positive outputs on all reals.","Increasing (−∞,∞); no decreasing intervals."),
21:("A piecewise continuous line descends to x=−1, stays constant on (−1,2), then rises for x>2.","Decreasing (−∞,−1), constant (−1,2), increasing (2,∞)."),
22:("A smooth curve rises toward a local max at x=−1, falls to a local min at x=2, then rises again.","Increasing (−∞,−1)∪(2,∞), decreasing (−1,2)."),
23:("For independent f(x)=x³−3x, find local extrema and their x/y coordinates.","Local maximum at (−1,2); local minimum at (1,−2)."),
24:("Use a complete continuous independent graph on [−3,3] with points (−3,0),(−2,16),(-1,10),(0,0),(1,10),(2,16),(3,−10) connected in order. Determine increasing/decreasing intervals.","Increasing (−3,−2)∪(0,2); decreasing (−2,0)∪(2,3), with appropriate segmented monotonic subintervals."),
25:("On the same complete bounded first-party graph find the absolute maximum and absolute minimum and all attaining input values.","Absolute max 16 at x=−2,2; absolute min −10 at x=3.")
}
for n,(prompt,answer)in independent_graphs.items():add(n,"GRAPH_MONOTONICITY_AND_EXTREMA",prompt,answer,
"Teacher checks independent graph/coordinate model and source-skill behavior; original publisher graphic not copied.")
add(26,"TABLE_AVERAGE_RATE_TWO_PERIODS",
    "An independently recorded sales table lists: 2020:$243m, 2021:$249m, 2022:$251m, 2023:$249m. Find average change (a) 2020–21 and (b) 2020–23.",
    "a=6 million dollars/year; b=(249−243)/3=2 million dollars/year.")
add(27,"TABLE_AVERAGE_RATE_TWO_PERIODS",
    "A population chart (thousands) records 2020:83, 2022:77, 2024:78. Find average changes (a) 2020–22 and (b) 2020–24.",
    "a=−3 thousand people/year; b=(78−83)/4=−5/4 thousand people/year.")
numeric={
28:("x²","[1,5]","6"),
29:("5−2x²","[−2,4]","−4"),
30:("x³","[−4,2]","12"),
31:("3x³−1","[−3,3]","27"),
32:("1/x","[1,3]","−1/3"),
33:("((t²−4)(t+1))/(t²+3)","[−3,1]","−1/6"),
34:("6t²+4/t³","[−1,3]","352/27")
}
for n,(formula,window,expected)in numeric.items():
    add(n,"NUMERIC_AVERAGE_RATE",
        f"Evaluate the endpoints and compute exact average rate of change of f(z)={formula} on {window}. Show the secant formula and avoid invalid inputs.",
        expected)
tech={
35:("x⁴−4x³+5","Graph the quartic and locate its local minimum; distinguish stationary non-extremum points.","Local minimum at x=3, f(3)=−22. At x=0 the derivative vanishes but no local extremum is attained."),
36:("x⁵+5x⁴+10x³+10x²−1","Use graphing or analysis to identify local extrema and increasing/decreasing intervals.","Local max at (−2,7), local min at (0,−1); increasing (−∞,−2) and (0,∞), decreasing (−2,0)."),
37:("t√(t+3)","Find the real domain and one interior local extremum of a root-based function.","Domain [−3,∞); interior local min (−2,−2); decrease (−3,−2), increase (−2,∞)."),
38:("3t^(2/3)−t","Inspect a fractional-power graph's nondifferentiable extremum and another turning point.","Local min at (0,0) cusp; local max at (8,4); decreasing (−∞,0), increasing (0,8), decreasing (8,∞)."),
39:("x⁴+2x³−12x²−10x+4","Use a graphing utility or numerical root solver to identify every local extremum and increasing/decreasing interval.","Solve 4x³+6x²−24x−10=0; classify all real critical roots by derivative sign; numeric estimates require source-aware precision."),
40:("x⁴−8x³+18x²−6x+2","Use numerical analysis to find local extrema and monotonic intervals; justify any rounded estimates.","Solve 4x³−24x²+36x−6=0; classify derivative sign intervals and label rounded coordinates.")
}
for n,(formula,prompt,answer)in tech.items():
    add(n,"TECHNOLOGY_GRAPHED_EXTREMA",
        f"For first-party model f(x)={formula}: {prompt}",answer,
        "Teacher checks mathematical graph and derivative/critical points; for complex quartics precise numeric values must be computed rather than invented.")
add(41,"GRAPH_SCREEN_EXTREMA",
    "A graphing device identifies a peak at approximately (1.333,5.185). Is this evidence of a local maximum, a zero, or necessarily an absolute global maximum?",
    "Evidence supports a local maximum. A device window alone does not prove an absolute maximum over an unrestricted domain.")
add(42,"RECIPROCAL_RATE_INVERSE",
    "For f(x)=1/x determine c≠1 such that average rate on [1,c] is −1/4, with nonzero inputs.",
    "Average rate =−1/c; c=4.")
add(43,"RECIPROCAL_RATE_INVERSE",
    "For f(x)=1/x find b≠2 such that average rate on [2,b] equals −1/10, with nonzero inputs.",
    "Average rate =−1/(2b); b=5.")
add(44,"CONTEXT_AVERAGE_SPEED",
    "A trip odometer rises from 21,395 to 22,125 miles over 13.5 hours. Compute the average speed in miles/hour.",
    "730/13.5=1460/27≈54.07 miles/hour.")
add(45,"CONTEXT_FLOW_RATE",
    "A pump delivers 10.7 US gallons in four minutes. Compute the average flow rate with correct units.",
    "10.7/4=2.675 gallons per minute.")
add(46,"CONTEXT_AVERAGE_VELOCITY",
    "Near the lunar surface, falling distance is d(t)=2.6667t² feet. Find average velocity from t=1 to t=2 seconds.",
    "d(2)−d(1)=2.6667(4−1)=8.0001 feet; divide by one second, giving ≈8.0001 ft/s.")
add(47,"CONTEXT_DECAY_GRAPH_RATE",
    "An independent graph of a decaying quantity shows 12 mg at t=5 days and 4 mg at t=15 days. Estimate average decay rate with the sign and units.",
    "(4−12)/(15−5)=−0.8 milligrams/day; decaying means a negative time rate.")
assert len(questions)==47 and sorted(questions)==list(range(1,48))
manifest={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":3,
"OriginalBook":"OpenStax Precalculus 2017 local PDF Section 1.3",
"SourceSha256":source["SourceSha256"],
"OriginalSourceNumberedExerciseItems":47,
"FirstPartySkillEquivalentPracticeItems":47,
"SourceFigureCopies":0,
"AutoGrading":False,
"TeacherReviewContractAuthored":True,
"OriginalPublisherExactGraphPixelEqualityClaim":False,
"StageD":"NOT_RUN",
"Items":[questions[i] for i in range(1,48)]
}
(base/"u01-l03-source-47-firstparty-practice-contracts.json").write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l03-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["FirstPartyOriginalNumberedExerciseSkillQuestionsAuthored"]=47
c["StageC"]="47_SOURCE_SKILL_MAPPED_FIRST_PARTY_ASSESSMENTS_AND_MANUAL_TEACHER_KEYS_AUTHORING_COMPLETE_NOT_RUNTIME_TESTED"
c["SourceExercisePracticeContractPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l03-source-47-firstparty-practice-contracts.json"
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==2 and p["FirstIncompleteLessonOrdinal"]==3
r=p["LessonEntries"][2]
r["FirstPartyOriginalNumberedExerciseSkillQuestionsAuthored"]=47
r["StageCStatus"]=c["StageC"]
r["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L03_ORIGINAL_47_SOURCE_NUMBERED_EXERCISE_SKILLS_FIRSTPARTY_AUTHORED_WITH_TEACHER_EXPECTED_MATH")
