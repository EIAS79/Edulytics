from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson002.json").read_text(encoding="utf-8"))
assert len(source["Pages"])==16 and source["Section"]=="1.2"
items={}
def add(n,kind,prompt,answer,method="Teacher examines the explanation and every algebraic, graph, domain or contextual part."):
    assert n not in items
    items[n]={"Number":n,"Category":kind,"FirstPartyStudentQuestion":prompt,
              "ExpectedTeacherMathematics":answer,"ReviewMethod":method,
              "AutomatedScoreVerified":False,
              "SourceNumberedExerciseMathSkillReviewed":True,
              "FirstPartyAcademicAlternateNotSourceCopy":True,
              "SourcePDFPage":51 if n<=29 else 52 if n<=45 else 53,
              "OriginalFigureCopied":False,
              "SourceMeaningPreservationReviewed":False,
              "StageDAuthenticatedSubmission":"NOT_RUN"}
concepts=[
("Explain why distinct mathematical functions can have different permitted inputs, with two exact counterexamples.","Domain restrictions depend on the formula and context; 1/x excludes 0, while x² allows all real numbers."),
("Describe a repeatable procedure for determining the real domain of a formula containing a denominator and an even radical.","Exclude denominator zeros, require each even radicand >=0, intersect conditions, then express domain in intervals."),
("Compare the real domains of f(x)=∛x and g(x)=√x, including negative inputs.","Cube root domain ℝ; square root domain [0,∞) because negative real radicands have no real even root."),
("Distinguish excluded and included finite interval endpoints and explain the notation at infinity.","Parentheses for strict inequalities or ∞; brackets for included finite bounds."),
("Describe how to combine branches and mark endpoints in a piecewise graph.","Draw each formula only on its prescribed input interval and distinguish open from closed boundary dots.")
]
for n,(q,a) in enumerate(concepts,1):add(n,"VERBAL",q,a)
algebra={
6:("−2x(x−1)(x−2)","(−∞,∞)"),
7:("5−2x²","(−∞,∞)"),
8:("3√(x−2)","[2,∞)"),
9:("3−√(6−2x)","(−∞,3]"),
10:("√(4−3x)","(−∞,4/3]"),
11:("√(x²+4)","(−∞,∞)"),
12:("∛(1−2x)","(−∞,∞)"),
13:("∛(x−1)","(−∞,∞)"),
14:("9/(x−6)","(−∞,6)∪(6,∞)"),
15:("(3x+1)/(4x+2)","(−∞,−1/2)∪(−1/2,∞)"),
16:("√(x+4)/(x−4)","[−4,4)∪(4,∞)"),
17:("(x−3)/(x²+9x−22)","(−∞,−11)∪(−11,2)∪(2,∞)"),
18:("1/(x²−x−6)","(−∞,−2)∪(−2,3)∪(3,∞)"),
19:("(2x³−250)/(x²−2x−15)","(−∞,−3)∪(−3,5)∪(5,∞)"),
20:("5/√(x−3)","(3,∞)"),
21:("(2x+1)/√(5−x)","(−∞,5)"),
22:("√(x−4)/√(x−6)","(6,∞)"),
23:("√(x−6)/√(x−4)","[6,∞)"),
24:("x/x","(−∞,0)∪(0,∞)"),
25:("(x²−9x)/(x²−81)","(−∞,−9)∪(−9,9)∪(9,∞)")
}
assert len(algebra)==20
for n,(formula,answer) in algebra.items():
    extra=(" Show that a cancellation cannot recover an originally undefined input." if n in (19,24,25) else " State the exact excluded/included endpoints.")
    add(n,"ALGEBRAIC_DOMAIN",f"Find the real domain of f(x)={formula}. Express it in correct interval notation.{extra}",
        f"Domain {answer}. Check denominator-zero and even-root restrictions before any simplification.")
add(26,"ALGEBRAIC_AND_GRAPHICAL_DOMAIN",
    "Determine the real domain of f(x)=√(2x³−50x) both by a sign chart and by plotting the radicand; explain each sign interval.",
    "2x(x−5)(x+5)≥0 gives [−5,0]∪[5,∞). Show intervals where the cubic radicand is nonnegative.",
    "Require both algebraic sign analysis and the graph of the radicand.")

graph={
27:("A finite decreasing line segment from open (2,8) to filled (8,6).","Domain (2,8]; range [6,8)."),
28:("A finite decreasing segment from filled (4,8) to open (8,2).","Domain [4,8); range (2,8]."),
29:("An upper semicircular arc from (−4,0) to (4,0), with highest point (0,2), both endpoints filled.","Domain [−4,4]; range [0,2]."),
30:("An upward parabola segment with closed endpoints (2,−2) and (6,−2) and vertex (4,−6).","Domain [2,6]; range [−6,−2]."),
31:("A U-shaped parabola segment from filled (−5,2) to open (3,2), with minimum (−1,0).","Domain [−5,3); range [0,2]."),
32:("A concave-down arc beginning at filled (−3,−5), rising to its peak (0,4), then ending at open (2,0).","Domain [−3,2); range [−5,4]."),
33:("A strictly decreasing square-root-style curve extending indefinitely left/up and ending at filled (1,0).","Domain (−∞,1]; range [0,∞)."),
34:("An increasing concave-down curve beginning at filled (−4,−2), rising without a right endpoint while approaching horizontal level 4 from below.","Domain [−4,∞); range [−2,4)."),
35:("The reciprocal curve y=1/x restricted to x∈[−6,−1/6]∪[1/6,6], with all four endpoints filled.","Domain [−6,−1/6]∪[1/6,6]; range [−6,−1/6]∪[1/6,6]."),
36:("An open left endpoint (−2,9), a W-shaped continuous branch attaining minimum −4, and a right ray rising without bound.","Domain (−2,∞); range [−4,∞)."),
37:("A continuous piecewise rising/flat/rising curve starting at filled (−3,0), passing (0,5), flat to (3,5), then extending upward to the right.","Domain [−3,∞); range [0,∞).")
}
assert len(graph)==11
for n,(shape,answer) in graph.items():
    add(n,"GRAPH_DOMAIN_RANGE",
        f"Using an independently drawn and correctly labelled coordinate model: {shape} Determine the exact input domain and attained output range, preserving open and closed endpoint notation.",
        answer,
        "Teacher verifies the student's actual graph or coordinate model and all endpoint/range claims; text descriptions alone are not certified original plot reproductions.")
piece={
38:("x+1 when x<−2; −2x−3 when x≥−2","(−∞,∞)","at −2 left limit −1 excluded, actual f(−2)=1 included"),
39:("2x−1 when x<1; x+1 when x≥1","(−∞,∞)","at 1 left limit 1 excluded, actual f(1)=2 included"),
40:("x+1 when x<0; x−1 when x>0","(−∞,0)∪(0,∞)","both (0,1) and (0,−1) are open; x=0 excluded"),
41:("3 when x<0; √x when x≥0","(−∞,∞)","at 0 left y=3 is open, actual f(0)=0 included"),
42:("x² when x<0; 1−x when x>0","(−∞,0)∪(0,∞)","both boundary points omitted at x=0"),
43:("x² when x<0; x+2 when x≥0","(−∞,∞)","at 0 (0,0) open and (0,2) filled"),
44:("x+1 when x<1; x³ when x≥1","(−∞,∞)","at 1 left y=2 open and actual f(1)=1 filled"),
45:("|x| when x<2; 1 when x≥2","(−∞,∞)","at 2 (2,2) open and (2,1) filled")
}
for n,(pieces,domain,boundary) in piece.items():
    add(n,"PIECEWISE_GRAPH_AND_DOMAIN",
        f"Sketch a fully labelled two-branch graph with f(x)={pieces}. Give the domain in interval notation and mark every open/filled endpoint.",
        f"Domain {domain}; {boundary}.",
        "Check both branches only on their specified input conditions and the marker at each domain boundary.")

values={
46:("x+1 for x<−2; −2x−3 for x≥−2","−2,1,−1,−3",[-3,-2,-1,0]),
47:("1 for x≤−3; 0 for x>−3","1,0,0,0",[-3,-2,-1,0]),
48:("−2x²+3 for x≤−1; 5x−7 for x>−1","−15,−5,1,−7",[-3,-2,-1,0]),
49:("7x+3 for x<0; 7x+6 for x≥0","−4,6,20,34",[-1,0,2,4]),
50:("x²−2 for x<2; 4+|x−5| for x≥2","−1,−2,7,5",[-1,0,2,4]),
51:("5x for x<0; 3 for 0≤x≤3; x² for x>3","−5,3,3,16",[-1,0,2,4])
}
for n,(pieces,outputs,inputs) in values.items():
    add(n,"PIECEWISE_FOUR_EVALUATIONS",
        f"Using f(x)={pieces}, evaluate f at the four ordered inputs {inputs}. Show which single branch applies at each value and report all four outputs.",
        f"Ordered outputs: {outputs}. The branch boundary must be evaluated from the correct inequality.",
        "Teacher scores each of four values separately and checks boundary inclusion.")
domains={
52:("x+1 if x<−2; −2x−3 if x≥−2","(−∞,∞)"),
53:("x²−2 if x<1; −x²+2 if x>1","(−∞,1)∪(1,∞)"),
54:("2x−3 if x<0; −3x² if x≥2","(−∞,0)∪[2,∞)")
}
for n,(pieces,domain) in domains.items():
    add(n,"PIECEWISE_DOMAIN",
        f"Without graphing first, compute the exact real domain of the two-branch rule f(x)={pieces}. Indicate missing boundary inputs in intervals.",
        f"Domain {domain}.")
add(55,"RECIPROCAL_SQUARED_WINDOW",
    "Draw two distinct source-skill coordinate windows for y=1/x² restricted to x∈[−1/2,−1/10] and x∈[1/10,1/2]. Find the attained output range on the union.",
    "Combined range [4,100]. Mark all window endpoints and ensure no x=0 point is drawn.",
    "Teacher verifies two independently constructed bounded graphs, axes and exact endpoint values.")
add(56,"RECIPROCAL_WINDOW",
    "Draw two bounded coordinate windows for y=1/x restricted to x∈[−1/2,−1/10] and x∈[1/10,1/2]. Give the combined output range.",
    "Combined range [−10,−2]∪[2,10], not one positive continuous range.")
add(57,"RANGE_ABSOLUTE_TRANSFORM",
    "A continuous-output function attains every real y∈[−5,8]. Determine the exact range of |f(x)| and explain the lower/upper endpoints.",
    "[0,8]. The original range contains zero and its maximum absolute magnitude is eight.")
add(58,"CREATE_NONNEGATIVE_RANGE",
    "Construct a nonconstant real function whose output range is exactly all nonnegative real numbers, and prove both inclusion and attainment.",
    "Example f(x)=x² on ℝ has range [0,∞).")
add(59,"CREATE_STRICT_DOMAIN",
    "Construct a real-valued function with domain exactly (2,∞) and explain why the boundary input x=2 is not permitted.",
    "Example f(x)=1/√(x−2) has domain (2,∞).")
add(60,"PROJECTILE_PHYSICAL_DOMAIN",
    "A vertically launched ball has height h(t)=−16t²+96t feet after t seconds. Find the physical flight-time domain and explain how it differs from the polynomial's algebraic domain.",
    "Height≥0 and t≥0 imply 0≤t≤6 seconds; physical domain [0,6]. Algebraic polynomial domain is all real numbers.")
add(61,"APPLIED_COST_MULTIPART",
    "A factory's cost is C(n)=10n+500 dollars for n manufactured items. (a) Find the setup cost. (b) Find cost of 25 items. (c) If C≤1500 and n is a nonnegative whole number, describe the feasible input and output sets; then state their continuous-model intervals.",
    "a=$500; b=$750; c=integer n∈{0,…,100} with C∈{500,510,…,1500}; continuous-model domain [0,100], range [500,1500].",
    "Teacher scores all three parts, unit dollars, physical nonnegative item count and discrete vs continuous distinction.")
assert sorted(items)==list(range(1,62)),set(range(1,62))-set(items)
path=base/"u01-l02-source-61-first-party-practice-contracts.json"
data={
"SchemaVersion":1,
"Course":"US-CCSS-MATH:G12",
"Unit":1,"Lesson":2,"OriginalSection":"1.2",
"LocalOriginalSource":"OpenStax Precalculus 2017, authorized local PDF pp.38–53",
"SourceSha256":source["SourceSha256"],
"OriginalNumberedSourceExerciseAnchors":61,
"FirstPartyOriginalSkillAlignedQuestionItems":61,
"StudentDelivery":"CONTRACT_AUTHORING_NOT_STUDENT_PORTAL_WIRED",
"TeacherReview":"CONTRACT_AUTHORED_NOT_PERSISTED_OR_RUNTIME_VERIFIED",
"AutomatedGrading":False,
"OriginalFiguresReproduced":False,
"OriginalExerciseGraphs27To37SourceVisuallyInspected":True,
"OriginalPiecewiseEndpointTasks38To54SourceVisuallyInspected":True,
"SourceExercisesFullySemanticOneToOneCertified":False,
"SourceAcademicSkillCoverageCandidate":True,
"SourceAtomicSubpartDenominatorComplete":False,
"StageD":"NOT_RUN","ReadyForStageD":False,
"OriginalNamedExerciseItems":[items[n] for n in range(1,62)]
}
path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l02-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["SourceOriginalGraphExercises27To37ImagesVisuallyInspected"]=11
c["FirstPartySectionQuestionSkillsAuthored"]=61
c["OriginalSourceNumberedExerciseItemsLocated"]=61
c["FirstPartyPracticeContractPath"]=path.relative_to(root).as_posix()
c["StageC"]="61_FIRST_PARTY_SOURCE_SKILL_ALIGNED_PRACTICE_CONTRACTS_AUTHORED_NOT_STUDENT_WIRED_OR_DB_VERIFIED"
c["OriginalNamedExerciseAtomicSubpartsVerified"]=False
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["FirstIncompleteLessonOrdinal"]==2 and p["LastClosedLessonOrdinal"]==1
row=p["LessonEntries"][1]
row["FirstPartySectionQuestionSkillsAuthored"]=61
row["OriginalSectionGraphExercisesVisuallyInspected"]=11
row["FirstPartyPracticeContractPath"]=path.relative_to(root).as_posix()
row["StageCStatus"]=c["StageC"]
row["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L02_61_FIRSTPARTY_SOURCE_SKILL_QUESTION_CONTRACTS_11_ORIGINAL_GRAPHS_REVIEWED_23_PIECEWISE_AND_CONTEXT_ITEMS")
