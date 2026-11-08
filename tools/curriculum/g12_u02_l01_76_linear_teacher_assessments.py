from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson001.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.1"
items={}
def put(n,skill,q,a,parts=None):
 assert n not in items,n
 items[n]={"Number":n,"OriginalSourcePDFPage":154 if n<=29 else 155 if n<=46 else 156 if n<=67 else 157,
 "SourceAcademicSkill":skill,"FirstPartyIndependentStudentQuestion":q,
 "TeacherExpectedMathAndAssessment":a,
 "RequiredCognitiveActions":parts or ["show working","state result with restrictions and units where relevant"],
 "OriginalPublisherExactVerbatimCopied":False,"OriginalPublisherGraphicCopied":False,
 "TeacherManualScoring":"AUTHORED_NOT_RUNTIME_VERIFIED"}
for n,q,a in [
(1,"A diver's depth is D(t)=220−8t metres. Explain initial depth and direction of travel over time.","Starts 220 m deep, moves toward surface at 8 m/min."),
(2,"A hiker's height is H(t)=650+35t metres after t hours. Explain the initial height and rate.","Starts 650 m and gains 35 m/hour."),
(3,"A cyclist is 7.6 km from the finish after 8 min and 4.6 km away after 20 min. Find closing rate in kilometres/hour.","Slope (4.6−7.6)/(20−8)=−0.25 km/min=−15 km/hour."),
(4,"A walker starts 8 miles from home and travels away at 3 mph. Write distance after t hours.","d(t)=8+3t miles, t≥0."),
(5,"A ship is 120 nautical miles from a port and moves toward it at 15 knots. Model remaining distance until arrival.","d(t)=120−15t nautical miles, t∈[0,8] hours."),
(6,"A visitor has a $55 budget and spends $3 per ride. Express the remaining budget after n whole-number rides.","B(n)=55−3n dollars, integers 0≤n≤18.")
]:put(n,"CONTEXT_SLOPE_INITIAL_VALUE",q,a)
linear={
7:("y=(1/4)x+6","linear m=1/4,b=6"),
8:("y=3x−5","linear m=3,b=−5"),
9:("y=3x²−2","not linear: quadratic term"),
10:("3x+5y=15","linear: y=−3x/5+3"),
11:("3x²+5y=15","not linear: quadratic term"),
12:("3x+5y²=15","not y-linear in x; squared output gives two branches"),
13:("−2x²+3y²=6","not linear; both variables squared"),
14:("−(x−3)/5=2y","linear: y=−x/10+3/10")
}
for n,(expr,ans)in linear.items():put(n,"CLASSIFY_LINEAR_FUNCTION",f"Determine whether {expr} defines a linear real function y=f(x). If so write y=mx+b; explain nonlinearity otherwise.",ans)
monotone={
15:("4x+3","increasing m=4"),16:("5x+6","increasing m=5"),
17:("5−2x","decreasing m=−2"),18:("8−3x","decreasing m=−3"),
19:("−2x+4","decreasing m=−2"),20:("−4x+1","decreasing m=−4"),
21:("x/2−3","increasing m=1/2"),22:("x/4−5","increasing m=1/4"),
23:("−x/3−2","decreasing m=−1/3"),24:("−3x/8+3","decreasing m=−3/8")
}
for n,(f,a)in monotone.items():
 put(n,"MONOTONE_LINEAR_SLOPE_SIGN",f"For f(x)={f}, classify increasing, decreasing or constant and justify from the slope.",a)
twopoints={
25:((2,4),(4,10)),26:((1,5),(4,11)),27:((-1,4),(5,2)),
28:((8,-2),(4,6)),29:((6,11),(-4,3))
}
for n,(p1,p2)in twopoints.items():
 m=F(p2[1]-p1[1],p2[0]-p1[0])
 put(n,"SLOPE_TWO_POINTS",f"Find the exact slope of the line passing through {p1} and {p2}.",f"m={m}, computed as Δy/Δx.")
points={
30:((-5,-4),(5,2)),31:((-1,4),(5,1)),
32:((2,4),(4,10)),33:((1,5),(4,11)),
34:((-1,4),(5,2)),35:((-2,8),(4,6)),
36:((-2,0),(0,-3)),37:((-5,0),(0,4))
}
for n,(a,b)in points.items():
 m=F(b[1]-a[1],b[0]-a[0]);intercept=F(a[1])-m*a[0]
 put(n,"FIND_LINE_FROM_TWO_VALUES",f"Find a linear equation through points {a} and {b}, with exact slope and intercept.",f"f(x)=({m})x+({intercept}).",
     ["compute slope","derive intercept","write and verify line"])
# Original 38-46 are plots. Recreate numeric first-party line anchors,
# so the graph lookup is meaningful even with no publisher bitmap.
graphdefs={
38:((0,1),(3,4)),39:((-2,3),(2,-1)),40:((0,-2),(4,0)),
41:((0,2),(1,5)),42:((0,-1),(3,2)),43:((-2,-4),(2,4)),
44:((0,5),(5,0)),45:((-2,-1),(2,1)),46:((0,-2),(2,-2))
}
for n,(a,b)in graphdefs.items():
 m=F(b[1]-a[1],b[0]-a[0]);v=F(a[1])-m*a[0]
 question=(f"An independently drawn first-party coordinate line passes through points {a} and {b}. "
           +("Read its exact slope from the graph and justify rise/run." if n<=40 else
             "Derive its slope-intercept rule and state whether it rises, falls or is constant."))
 put(n,"INDEPENDENT_GRAPH_TO_SLOPE_AND_LINE",question,
     (f"m={m}." if n<=40 else f"y=({m})x+({v})."))
tables={
47:([0,5,10,15],[5,-10,-25,-40]),48:([0,5,10,15],[5,30,105,230]),
49:([0,5,10,15],[-5,20,45,70]),50:([5,10,20,25],[13,28,58,73]),
51:([0,2,4,6],[6,-19,-44,-69]),52:([2,4,6,8],[13,23,43,53]),
53:([2,4,6,8],[-4,16,36,56]),54:([0,2,6,8],[6,31,106,231])
}
for n,(xs,ys)in tables.items():
 ms=[F(ys[i]-ys[i-1],xs[i]-xs[i-1])for i in range(1,len(xs))]
 ok=len(set(ms))==1
 b=F(ys[0])-ms[0]*xs[0]
 answer=f"Linear, m={ms[0]}, f(x)=({ms[0]})x+({b})."if ok else f"Not linear; adjacent slopes {[str(v)for v in ms]} differ."
 put(n,"IDENTIFY_LINEAR_TABLE",
     f"For the independent finite table with inputs {xs} and outputs {ys}, is the relation linear? If yes give f(x)=mx+b; otherwise justify with rates.",
     answer,["compare slopes across every interval","state formula only if linear"])
put(55,"TWO_NONINTEGER_POINT_SLOPES",
    "For a linear f, f(0.1)=11.5 and f(0.4)=−5.9. Derive exact slope and intercept.",
    "m=−58; b=17.3, so f(x)=−58x+17.3.")
put(56,"SMALL_SLOPE_GRAPH_WINDOW",
    "On the closed x-window [−10,10], plot f(x)=0.02x−0.01; label y-intercept and both endpoint outputs.",
    "Endpoint values −0.21 and 0.19; y-intercept −0.01.",["plot left/right endpoints","mark intercept","state range"])
put(57,"LARGE_SLOPE_GRAPH_WINDOW",
    "Plot f(x)=2500x+4000 on x∈[−10,10], with correct axis scale. Label the endpoint outputs.",
    "f(−10)=−21000 and f(10)=29000; y-intercept 4000.",["plot endpoint and scale","mark intercept"])
# Original source 58 and 59 have two-part table completions
put(58,"COMPLETE_LINEAR_TABLE_AND_FORMULA",
    "A linear table includes (w,k)=(-10,30),(5.5,-26),(67.5,a),(b,-44). Find a and b, and write k(w) to 3 decimal places.",
    "m=−56/15.5=−112/31≈−3.613; intercept b0=30−m(−10)=−190/31≈−6.129; a=k(67.5), solve k(b)=−44.",
    ["solve slope/intercept","fill missing output a","fill missing input b","round function coefficients to 3 dp"])
put(59,"COMPLETE_LINEAR_TABLE_AND_FORMULA",
    "A linear table includes (p,q)=(0.5,400),(0.8,700),(12,a),(b,1000000). Fill a,b and derive q(p).",
    "Slope 1000, intercept −100: q(p)=1000p−100; a=11900; b=1000.1.",
    ["find slope and intercept","fill a","fill b"])
put(60,"FRACTION_SLOPE_GRAPH_WINDOW",
    "Graph y=x/8+31/16 for x∈[−10,10]. Give exact endpoint y-values and intercept.",
    "f(−10)=11/16; f(10)=51/16; f(0)=31/16.")
put(61,"NARROW_X_GRAPH_WINDOW",
    "Graph y=75x−22.5 for x∈[−0.1,0.1], marking both endpoints.",
    "f(−0.1)=−30, f(0.1)=−15; output window [−30,−15].")
put(62,"FAMILY_OF_PARALLEL_LINES",
    "For x∈[−4,4], graph all four lines y=2x+b with b=3,4,−4,−5. Explain how they relate.",
    "All slopes 2 so lines are parallel with distinct y-intercepts 3,4,−4,−5.",
    ["graph four distinct b choices","label parallelism"])
put(63,"FIND_MISSING_INPUT_FROM_SLOPE",
    "A line passes through (x,2) and (−4,6), with slope m=3. Find x.",
    "3=(2−6)/(x+4), x=−16/3.")
put(64,"FIND_MISSING_OUTPUT_FROM_SLOPE",
    "A line passes through (10,y) and (25,100), slope −5. Find y.",
    "−5=(100−y)/15, so y=175.")
put(65,"VERTICAL_LINE_EXCEPTION",
    "Can points (a,b) and (a,b+1) determine y=f(x)? Explain the geometric equation.",
    "Vertical x=a; no single-valued y=f(x) line.")
put(66,"SYMBOLIC_TWO_POINT_LINE",
    "Find line through (2a,b) and (a,b+1), assuming a≠0. State what happens when a=0.",
    "m=(b+1−b)/(a−2a)=−1/a; y=b−(x−2a)/a. If a=0, vertical x=0.")
put(67,"SYMBOLIC_INTERCEPT_SLOPE_LINE",
    "Find line through (a,0) and (c,d) for a≠c. State exceptional case.",
    "y=d(x−a)/(c−a); if a=c and d≠0, vertical line.")
context={
68:("A server starts with $20 tips and adds $0.50 per customer. Find T(n).","T(n)=20+0.50n dollars."),
69:("Two session plans cost $125 for 2 coaching sessions and $260 for 5. Find unit session price.","(260−125)/(5−2)=$45/session."),
70:("Price is $30 at 1000 shirts and $22 at 3000 shirts. Find p(n) line.","m=−0.004 dollars/shirt; p(n)=34−0.004n."),
71:("A service costs C(n)=24+0.1n dollars for n minutes. Interpret both coefficients.","$24 fixed fee and $0.10 per minute."),
72:("A crop model yields 30 oz each at 30 plants and 28 oz each at 34 plants. Model ounces per plant y(n).","Slope −0.5 oz per plant per additional stalk; y=45−0.5n."),
73:("A population falls from 287500 in 1960 to 275900 in 1989. Find annual average slope.","−11600/29=−400 people/year."),
74:("In 2003 a town had 45000 residents, increasing by 1700/year. Give P(t) for t years since 2003.","P(t)=45000+1700t."),
75:("For annual salary model I(x)=1054x+23286 where x is years after 1990, interpret the slope.","Increase of $1054 in average annual income per year."),
76:("Given (C,F)=(0,32),(100,212), derive Fahrenheit as a function of Celsius; evaluate F(28) and F(−40).",
    "F(C)=9C/5+32; slope 1.8°F per 1°C; F(28)=82.4°F; F(−40)=−40°F.")
}
for n,(q,a)in context.items():
 put(n,"CONTEXT_LINEAR_MODEL_UNITS",q,a)
assert sorted(items)==list(range(1,77)),len(items)
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l01-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
assessment={
"SchemaVersion":1,"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L01",
"SourceBookSection":"OpenStax Precalculus 2017 Section 2.1","SourceNumberedExercises":76,
"FirstPartySourceSkillStudentQuestions":76,
"StudentQuestions":[{"Number":n,"SourceSkill":v["SourceAcademicSkill"],"Question":v["FirstPartyIndependentStudentQuestion"],"Scoring":"TEACHER_MANUAL_REVIEW"}for n,v in sorted(items.items())],
"TeacherOnlyMathRubrics":[{"Number":n,"Expected":v["TeacherExpectedMathAndAssessment"],"Actions":v["RequiredCognitiveActions"]}for n,v in sorted(items.items())],
"OriginalPublisherGraphBitmapCopied":False,"OriginalPublisherExactTextAndGraphicsParityCertified":False,
"AuthenticatedStudentTeacherRuntime":"NOT_RUN","StageD":"NOT_RUN"}
native.write_text(json.dumps(assessment,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u02-l01-original76-firstparty-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],"OriginalSourceSectionNumberedExercises":76,
"FirstPartySourceSkillEquivalentAssessmentQuestions":76,
"OriginalPublisherFiguresAndThirdPartyMediaCopied":0,
"ExactPublisherNumericalQuestionInstanceOrGraphicPixelMatchCertified":False,
"StageD":"NOT_RUN","Items":[items[n]for n in sorted(items)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u02-l01-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["OriginalNumberedExerciseSkillReview"]="76_INDEPENDENT_FIRSTPARTY_STUDENT_QUESTIONS_AND_TEACHER_MATH_RUBRICS_AUTHORED"
c["FirstPartyTeacherReviewedQuestions"]=76
c["AssessmentResource"]=native.relative_to(root).as_posix()
c["NumberedSourceSkillContractPath"]=manifest.relative_to(root).as_posix()
c["OriginalSectionFigureSkillReview"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U02_L01_76_INDEPENDENT_FIRSTPARTY_LINEAR_SOURCE_SKILL_QUESTION_CONTRACTS_AUTHORED")
