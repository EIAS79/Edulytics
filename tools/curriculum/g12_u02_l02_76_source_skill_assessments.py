from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson002.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.2" and source["Pages"][0]["PdfPage"]==158 and source["Pages"][-1]["PdfPage"]==176
tasks={}
def put(n,category,prompt,expected,parts=None):
 assert n not in tasks,n
 tasks[n]={"Number":n,"OriginalSourcePDFPage":174 if n<=27 else 175 if n<=60 else 176,
 "OriginalAcademicSkill":category,"IndependentFirstPartyStudentQuestion":prompt,
 "TeacherExpectedMathOrReviewRubric":expected,
 "OriginalSourceNamedCognitiveActions":parts or ["show steps and exact result"],
 "PublisherExactSourceWordingCopied":False,"PublisherOriginalGraphImageReproduced":False,
 "TeacherReview":"AUTHORING_COMPLETE_RUNTIME_NOT_RUN"}
verbal=[
("Explain the relationship between slopes of two distinct parallel nonvertical lines; must their y-intercepts match?","Slopes equal; distinct parallel lines have different y-intercepts."),
("Explain the slope test for perpendicular lines, including a special horizontal/vertical pair.","Nonzero finite slopes m1*m2=−1. Horizontal y=c is perpendicular to vertical x=d."),
("Find where y=a intersects x=a and justify with both coordinate requirements.","(a,a), which satisfies both equations."),
("Describe how to construct a parallel line through a given point, including its equation form.","Reuse original slope m, use y−y1=m(x−x1)."),
("Describe how to construct a perpendicular line through a point to a nonhorizontal/nonvertical line.","Use slope −1/m and point-slope; horizontal ↔ vertical special case.")
]
for n,(q,a)in enumerate(verbal,1):put(n,"VERBAL_SLOPE_RELATIONSHIPS",q,a)
# Compare actual original mathematical line pairs (not publisher prose).
linecoeff={
6:((4,-7,10),(7,4,1)),
7:((1,3,12),(8,1,-1)), # -y=8x+1 -> 8x+y=-1
8:((4,3,12),(8,6,-1)), # -6y=8x+1 ->8x+6y=-1, slope -4/3
9:((6,-9,10),(3,2,1)),
10:((2,-3,-3),(3,2,1)), # y=2/3x+1 ->2x-3y=-3
11:((3,-4,-4),(-3,4,1)) # y=3/4x+1 ->3x-4y=-4
}
for n,(one,two)in linecoeff.items():
 m1=F(-one[0],one[1]);m2=F(-two[0],two[1])
 relation="parallel"if m1==m2 else "perpendicular"if m1*m2==-1 else "neither"
 put(n,"PARALLEL_PERPENDICULAR_FROM_GENERAL_FORM",
     f"Classify lines {one[0]}x+({one[1]})y={one[2]} and {two[0]}x+({two[1]})y={two[2]} as parallel, perpendicular or neither. Show both slopes.",
     f"m1={m1}, m2={m2}; {relation}.",["extract both slopes","justify relationship"])
intercepts={
12:(-1,2),13:(2,4),14:(3,-5),15:(-5,1),
16:(F(2,5),4),17:(F(-7,2),28) # -2x+5y=20 -> y=.4x+4; 7x+2y=56 ->y=-3.5x+28
}
for n,(m,b)in intercepts.items():
 intercept="none"if m==0 else str(-F(b)/F(m))
 put(n,"INTERCEPT_FROM_LINE",
     f"Given y=({m})x+({b}), determine both x-intercept and y-intercept exactly.",
     f"x-intercept ({intercept},0), y-intercept (0,{b}).",
     ["set y=0","set x=0"])
two_pairs={
18:(((0,6),(3,-24)),((-1,19),(8,-71))),
19:(((-8,-55),(10,89)),((9,-44),(4,-14))),
20:(((2,3),(4,-1)),((6,3),(8,5))),
21:(((1,7),(5,5)),((-1,-3),(1,1))),
22:(((0,5),(3,3)),((1,-5),(3,-2))),
23:(((2,5),(5,-1)),((-3,7),(3,-5)))
}
for n,(pair1,pair2)in two_pairs.items():
 m1=F(pair1[1][1]-pair1[0][1],pair1[1][0]-pair1[0][0])
 m2=F(pair2[1][1]-pair2[0][1],pair2[1][0]-pair2[0][0])
 outcome="parallel"if m1==m2 else "perpendicular"if m1*m2==-1 else "neither"
 put(n,"LINE_RELATION_FROM_FOUR_POINTS",
     f"A line uses {pair1[0]} and {pair1[1]}; another uses {pair2[0]} and {pair2[1]}. Calculate both slopes and classify their relationship.",
     f"m1={m1}, m2={m2}; {outcome}.",["first rise/run","second rise/run","classify"])
for n,base_m,point,category in [
(24,F(-5),(2,-12),"parallel"),(25,F(3),(4,9),"parallel"),
(26,F(-2),(-4,-1),"perpendicular"),(27,F(3),(3,1),"perpendicular")]:
 m=base_m if category=="parallel"else -1/base_m
 b=F(point[1])-m*point[0]
 put(n,"CONSTRUCT_PARALLEL_PERPENDICULAR_LINE",
     f"Construct a line {category} to a given line of slope {base_m}, passing through point {point}. Give slope-intercept and point-slope forms.",
     f"m={m}; y=({m})x+({b}); y−({point[1]})=({m})(x−({point[0]})).",
     ["choose correct slope","substitute given point","state equation"])
pairs={
28:((F(-2),F(-1)),(F(-1),F(0))),
29:((F(2),F(5)),(F(-3),F(-5))),
30:((F(-4,5),F(274,25)),(F(9,4),F(73,10))),
31:((F(7,4),F(457,60)),(F(4,3),F(31,5)))
}
for n,((m1,b1),(m2,b2))in pairs.items():
 x=F(b2-b1,m1-m2);y=m1*x+b1
 put(n,"FIND_INTERSECTION_OF_TWO_LINES",
     f"Find the exact intersection of f(x)=({m1})x+({b1}) and g(x)=({m2})x+({b2}) by algebra.",
     f"x={x}, y={y}.",["set outputs equal","solve input","substitute output"])
# Original graphs 32–37 are publisher-labelled six-line matching. Independent
# own plots use explicit rule and points so students must match model and
# not merely recite an answer embedded in the question.
independent={
32:("negative slope −1 and y-intercept −1","y=−x−1"),
33:("negative slope −2 and y-intercept −1","y=−2x−1"),
34:("negative slope −1/2 and y-intercept −1","y=−x/2−1"),
35:("constant function with y-intercept 2","y=2"),
36:("positive slope +1 and y-intercept 2","y=x+2"),
37:("positive slope +3 and y-intercept 2","y=3x+2")
}
for n,(graph,answer)in independent.items():
 put(n,"MATCH_LINE_TO_INDEPENDENT_PLOT",
     f"An independently drawn coordinate line has {graph}. Determine its exact equation, plot a point, and match it to its own correct slope/intercept category.",
     answer,["read slope from first-party graph description","read intercept","derive equation"])
source_shapes={
38:((-4,0),(0,-2)),39:((-2,0),(0,4)),
40:((0,7),(2,4)),41:((0,3),(5,5)),
42:((-6,-2),(6,-6)),43:((-3,-4),(3,0))
}
for n,(a,b)in source_shapes.items():
 m=F(b[1]-a[1],b[0]-a[0]);y0=F(a[1])-m*a[0]
 put(n,"SKETCH_LINE_FROM_FEATURES",
     f"Sketch a line through exactly marked first-party points {a} and {b}. State its linear equation and check both intercepts.",
     f"m={m}; y=({m})x+({y0}).",["plot two points","calculate slope","derive equation"])
functions={
44:(F(-2),F(-1)),45:(F(-3),F(2)),46:(F(1,3),F(2)),
47:(F(2,3),F(-3)),48:(F(2),F(3)),49:(F(3),F(-2)),
50:"x=3",51:"x=−2",52:(F(0),F(4)),53:(F(0),F(3)),
54:(F(-4,9),F(4)),55:(F(4,3),F(-4)),
56:(F(3,5),F(-3)),57:"x=5",58:(F(0),F(4))
}
for n,value in functions.items():
 if isinstance(value,str):
  answer=value+" vertical; undefined slope, not y=f(x)."
  question=f"Sketch and classify the coordinate relation {value}; show intercept and whether y is a function of x."
 else:
  m,b=value
  answer=f"y=({m})x+({b}), slope {m}, y-intercept (0,{b})."
  question=f"Sketch f(x)=({m})x+({b}) using two exact labelled coordinates; identify slope and y-intercept."
 put(n,"GRAPH_GENERAL_OR_SPECIAL_LINE",question,answer)
put(59,"COMPOSE_LINEAR_GRAPH_TRANSFORMATIONS",
    "From identity f(x)=x, vertically compress by factor 3/4, move right2 and down4. Find formula, slope and y-intercept.",
    "g(x)=(3/4)(x−2)−4=(3/4)x−11/2, slope 3/4 and y-intercept −11/2.",
    ["derive translated formula","find slope","find y-intercept"])
put(60,"COMPOSE_LINEAR_GRAPH_TRANSFORMATIONS",
    "From identity f(x)=x, vertically compress by 1/3, move left1 and up3. Determine g, its slope and y-intercept.",
    "g(x)=(x+1)/3+3=x/3+10/3, slope 1/3 and intercept 10/3.",
    ["derive translated formula","find slope","find y-intercept"])
# Draw separate source-skill own graphs, not copied publisher plots
for n,pair in {
61:((0,-2),(3,1)),62:((0,3),(2,-1)),
63:((-2,-4),(2,4)),64:((0,1),(4,3))
}.items():
 a,b=pair
 m=F(b[1]-a[1],b[0]-a[0]);k=F(a[1])-m*a[0]
 put(n,"READ_EQUATION_FROM_INDEPENDENT_GRAPH",
     f"On a first-party graph with labelled distinct points {a} and {b}, obtain the equation y=mx+b from graph coordinates. Verify both points.",
     f"m={m}, b={k}, line y=({m})x+({k}).",
     ["read two graphed coordinates","calculate slope","derive intercept"])
system={
65:((F(3,4),F(1)),(F(3,4),F(3))),
66:((F(2,3),F(-4)),(F(-1,5),F(6))), # 2x-3y=12 y=(2/3)x-4; x+5y=30 y=6-x/5
67:((F(2),F(3)),(F(-4),F(15))),
68:((F(1,2),F(-1,2)),(F(1),F(-3))), # x-2y=1 y=x/2-1/2; x-y=3 y=x-3
69:((F(-5,3),F(-65,3)),(F(1),F(5))) # 5x+3y=-65 => y=-5x/3-65/3; x-y=-5 => y=x+5
}
for n,((m1,b1),(m2,b2))in system.items():
 if m1==m2:
  ans="No intersection: two distinct parallel lines." if b1!=b2 else "Infinitely many shared points: same line."
 else:
  x=(b2-b1)/(m1-m2);y=m1*x+b1;ans=f"Intersection ({x},{y})."
 put(n,"SOLVE_GENERAL_TWO_LINE_SYSTEM",
     f"Find the exact intersection if any of y=({m1})x+({b1}) and y=({m2})x+({b2}). Explain parallel/coincident cases.",
     ans,["classify slopes","solve f=g or state no unique intersection"])
put(70,"PARALLEL_SMALL_SLOPE_THROUGH_POINT",
    "Find a line parallel to y=−0.01x+2.01 that passes through (1,2).",
    "y=−0.01x+2.01; it coincides with the reference line.")
put(71,"PERPENDICULAR_SMALL_SLOPE_THROUGH_POINT",
    "Find the line perpendicular to y=−0.01x+2.01 passing through (1,2).",
    "Negative reciprocal slope 100, y=100x−98.")
x=F(1999,201);y=F(200)-x/F(10)
put(72,"DECIMAL_INTERSECTION_MODEL",
    "Find exact intersection of f(x)=−0.1x+200 and g(x)=20x+0.1.",
    f"x={x}, y={y}; compare substituted values.")
put(73,"COMPARE_TWO_LINEAR_MODELS",
    "For f(x)=−0.1x+200 and g(x)=20x+0.1, determine where f>g and g>f over real inputs.",
    f"f>g for x<{x}; g>f for x>{x}, equality at {x}.",
    ["solve equality threshold","classify intervals"])
put(74,"BREAK_EVEN_RENTAL_PLANS",
    "Two rental plans are A(m)=30+0.18m and B(m)=50 dollars/day; at what nonnegative mileage does B save money?",
    "B<A if m>1000/9≈111.111 miles; minimum whole mile 112.",
    ["form costs","solve strict savings inequality","round for integer miles"])
put(75,"BREAK_EVEN_MESSAGE_PLANS",
    "A cellular plan charges $20 plus $1 per 100 messages, or $50 unlimited. Under a continuous per-message rate, when is unlimited strictly cheaper?",
    "A(n)=20+0.01n and B=50; unlimited cheaper if n>3000 messages.",
    ["write rates with assumptions","solve strict inequality"])
put(76,"BREAK_EVEN_TWO_TEXT_PLANS",
    "A(n)=15+(2/300)n versus B(n)=25+(0.5/100)n dollars for n messages. Under proportional billing, find where B is strictly cheaper.",
    "B<A when n>6000, so for whole messages n≥6001.",
    ["convert per-block to proportional rate","solve strict inequality"])
assert sorted(tasks)==list(range(1,77)),len(tasks)
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l02-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L02",
"OriginalNumberedSourceExerciseSkills":76,"FirstPartyIndependentSkillQuestions":76,
"StudentQuestions":[{"Number":n,"SourceSkill":v["OriginalAcademicSkill"],"Prompt":v["IndependentFirstPartyStudentQuestion"],"Grading":"TEACHER_REVIEW"}for n,v in sorted(tasks.items())],
"TeacherOnlyExpectedMathematics":[{"Number":n,"Answer":v["TeacherExpectedMathOrReviewRubric"],"OriginalSourceCognitiveActions":v["OriginalSourceNamedCognitiveActions"]}for n,v in sorted(tasks.items())],
"PublisherOriginalGraphicBitmapCopied":False,
"PublisherVerbatimAndPixelNumericParityCertified":False,
"RuntimeStudentQuestionDeliveryAndTeacherGrading":"NOT_RUN",
"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u02-l02-original76-firstparty-question-contracts.json"
manifest.write_text(json.dumps({"SchemaVersion":1,"OriginalSourceSha256":source["SourceSha256"],
"OriginalSourceNumberedExercises":76,"FirstPartyIndependentStudentQuestionCount":76,
"OriginalPublisherFiguresAndThirdPartyMediaCopied":0,
"SourceExactVerbatimNumericalAndGraphicParityClaim":False,"StageD":"NOT_RUN",
"Items":[tasks[n]for n in sorted(tasks)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u02-l02-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["SourceNumberedAssessmentSkillReview"]="76_FIRSTPARTY_SOURCE_SKILL_STUDENT_TASKS_WITH_TEACHER_MATH_RUBRICS_AUTHORED"
c["FirstPartyTeacherAssessmentResource"]=native.relative_to(root).as_posix()
c["Source76QuestionContractPath"]=manifest.relative_to(root).as_posix()
c["SourceGraphFigureSkillReview"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L02_76_SOURCE_SKILL_FIRSTPARTY_QUESTION_CONTRACTS_AND_TEACHER_RUBRICS_AUTHORED")
