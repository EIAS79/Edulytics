from pathlib import Path
import json,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson004.json").read_text(encoding="utf-8"))
assert src["Section"]=="3.4"
q={}
def put(n,skill,prompt,ans,actions=None):
 assert n not in q,n
 q[n]={"Number":n,"OriginalSourcePdfPage":269 if n<=47 else 270 if n<=60 else 271,
 "SourceAcademicSkill":skill,"IndependentFirstPartyStudentQuestion":prompt,
 "TeacherExpectedMathematics":ans,
 "NamedSourceCognitiveActions":actions or ["construct or classify exact polynomial algebra","justify zeros and multiplicity"],
 "OriginalPublisherVerbatimMathQuestionCopied":False,
 "OriginalPublisherBitmapGraphicReproduced":False,
 "OriginalPublisherExactNumericAndGraphicParityCertified":False,
 "StageDStudentTeacherRuntime":"NOT_RUN"}
for n,question,answer in[
(1,"Explain the distinction between a polynomial zero and its graph x-intercept.","Zero is real input r with p(r)=0; plotted x-intercept point is (r,0)."),
(2,"What can be said about graph x-intercepts if a degree n polynomial has n distinct real zeros?","There are n distinct real x-intercepts and each simple zero crosses; degree bound saturated."),
(3,"Describe how to establish a real zero in an interval using IVT.","Check polynomial continuity and opposite signs at interval endpoints."),
(4,"How does factored form help graph roots and end behavior?","Factors locate zeros/multiplicity; product leading term determines tails."),
(5,"What does a graph touching and turning at real zero imply?","The zero has even multiplicity (at least2).")
]:put(n,"POLYNOMIAL_GRAPH_CONCEPT",question,answer)
intercepts={
6:("2(t−4)(t+1)(t−6)",[4,-1,6],"4,−1,6 (simple)"),
7:("3(t+2)(t−3)(t+5)",[-2,3,-5],"−5,−2,3 (simple)"),
8:("4t(t−2)²(t+1)",[0,2,-1],"0 simple, 2 double, −1 simple"),
9:("2t(t−3)(t+1)²",[0,3,-1],"0 simple, 3 simple, −1 double"),
10:("2t⁴−8t³+6t²",[0,1,3],"factor 2t²(t−1)(t−3): 0 double,1 simple,3 simple"),
11:("4t⁴+12t³−40t²",[0,-5,2],"factor 4t²(t+5)(t−2):0 double,−5 simple,2 simple"),
12:("x⁴−x²",[-1,0,1],"x²(x−1)(x+1), root0 double and ±1 simple"),
13:("x³+x²−20x",[-5,0,4],"x(x+5)(x−4): simple −5,0,4"),
14:("x³+6x²−7x",[-7,0,1],"x(x+7)(x−1): simple −7,0,1"),
15:("x³+x²−4x−4",[-2,-1,2],"(x+1)(x−2)(x+2): simple −2,−1,2"),
16:("x³+2x²−9x−18",[-3,-2,3],"(x+2)(x−3)(x+3): simple −3,−2,3"),
17:("2x³−x²−8x+4",[-2,F(1,2),2],"(2x−1)(x−2)(x+2): simple −2,1/2,2"),
18:("x⁶−7x³−8",[-1,2],"(x³−8)(x³+1), real zeros −1,2"),
19:("2x⁴+6x²−8",[-1,1],"2(x²+4)(x²−1), real zeros ±1"),
20:("x³−3x²−x+3",[-1,1,3],"(x−3)(x−1)(x+1): simple −1,1,3"),
21:("x⁶−2x⁴−3x²",["0","±√3"],"x²(x²−3)(x²+1):0 double, ±√3 simple"),
22:("x⁶−3x⁴−4x²",[-2,0,2],"x²(x²−4)(x²+1):0 double, ±2 simple"),
23:("x⁵−5x³+4x",[-2,-1,0,1,2],"x(x²−1)(x²−4): five simple zeros 0,±1,±2")
}
for n,(expr,zeros,key)in intercepts.items():
 put(n,"FACTOR_POLYNOMIAL_REAL_X_INTERCEPTS",
     f"Determine all real roots and multiplicities of the independently presented polynomial p(x)={expr}. Show factorization, avoiding nonreal x-intercepts.",
     f"Root set {zeros}; {key}.",
     ["factor into real components","list distinct real intercepts","state repeated zero multiplicities"])
ivts={
24:("x³−9x",lambda x:x**3-9*x,F(-4),F(-2)),
25:("x³−9x",lambda x:x**3-9*x,F(2),F(4)),
26:("x⁵−2x",lambda x:x**5-2*x,F(1),F(2)),
27:("−x⁴+4",lambda x:-x**4+4,F(1),F(3)),
28:("−2x³−x",lambda x:-2*x**3-x,F(-1),F(1)),
29:("x³−100x+2",lambda x:x**3-100*x+2,F(1,100),F(1,10))
}
for n,(expr,fun,a,b)in ivts.items():
 fa=fun(a);fb=fun(b)
 assert fa*fb<0,(n,fa,fb)
 put(n,"INTERMEDIATE_VALUE_THEOREM_ROOT_EXISTS",
     f"Show that polynomial p(x)={expr} has at least one real zero between a={a} and b={b}. Give endpoint signs and cite continuity.",
     f"p({a})={fa}, p({b})={fb}; opposite signs prove some c∈({a},{b}) with p(c)=0.",
     ["evaluate both endpoints","show strict opposite signs","apply IVT"])
multiplicities={
30:(" (x+2)³(x−3)² ",[(-2,3),(3,2)]),
31:(" x²(2x+3)⁵(x−4)² ",[(0,2),(F(-3,2),5),(4,2)]),
32:(" x³(x−1)³(x+2) ",[(0,3),(1,3),(-2,1)]),
33:(" x²(x²+4x+4) ",[(0,2),(-2,2)]),
34:(" (2x+1)³(9x²−6x+1) ",[(F(-1,2),3),(F(1,3),2)]),
35:(" (3x+2)⁵(x²−10x+25) ",[(F(-2,3),5),(5,2)]),
36:(" x(4x²−12x+9)(x²+8x+16) ",[(0,1),(F(3,2),2),(-4,2)]),
37:(" x⁶−x⁵−2x⁴ ",[(0,4),(2,1),(-1,1)]),
38:(" 3x⁴+6x³+3x² ",[(0,2),(-1,2)]),
39:(" 4x⁵−12x⁴+9x³ ",[(0,3),(F(3,2),2)]),
40:(" 2x⁴(x³−4x²+4x) ",[(0,5),(2,2)]),
41:(" 4x⁴(9x⁴−12x³+4x²) ",[(0,6),(F(2,3),2)])
}
for n,(expr,roots)in multiplicities.items():
 ans=[{"Root":str(r),"Multiplicity":m,"Graph":"touch"if m%2==0 else "cross"}for r,m in roots]
 put(n,"FACTOR_AND_CLASSIFY_ROOT_MULTIPLICITY",
     f"Find all real roots, their multiplicities and touch/cross graph behavior for polynomial p(x)={expr}.",
     str(ans),["factor polynomial","combine repeated factors","classify parity"])
graphs={
42:("(x+3)²(x−2)",[(-3,2),(2,1)],3,1),
43:("(x+4)(x−1)²",[(-4,1),(1,2)],3,1),
44:("(x−1)³(x+3)²",[(1,3),(-3,2)],5,1),
45:("(x−3)³(x−2)²",[(3,3),(2,2)],5,1),
46:("−2x(x−1)(x+3)",[(0,1),(1,1),(-3,1)],3,-2),
47:("−3x(x+2)(x−4)",[(0,1),(-2,1),(4,1)],3,-3)
}
for n,(expr,roots,degree,lc)in graphs.items():
 left="up"if (degree%2==0 and lc>0)or(degree%2==1 and lc<0)else "down"
 right="up"if lc>0 else "down"
 put(n,"GRAPH_POLYNOMIAL_MULTIPLICITY_END_BEHAVIOR",
     f"Sketch independently p(x)={expr}. Label all real zeros and multiplicities, y-intercept, crossing/touching and both tails.",
     f"Roots and multiplicities {roots}, p(0) evaluate, degree{degree}, leading{lc}, left {left}, right {right}.",
     ["read or factor real zeros","infer odd/even crossing","label intercepts and tails"])
# 48–56 in printed source are publisher graph-only; instead show
# independent coordinate polynomials with exactly determined data.
independent={
48:([(0,0),(2,0)],"p(x)=(x−1)²−1; zeros0,2; y-int0"),
49:([(0,-2),(2,-2)],"p(x)=(x−1)²−3; roots1±√3; y-int−2"),
50:([(0,2),(2,2)],"p(x)=−(x−1)²+3; roots1±√3; y-int2"),
51:([(0,0),(2,0)],"p(x)=x(x−2); roots0,2; y-int0"),
52:([(0,-6),(2,0)],"p(x)=−(x+1)(x−2)(x−3); roots−1,2,3; y-int−6"),
53:([(-2,0),(1,0)],"p(x)=(x+2)²(x−1), −2 double touch,1 simple cross"),
54:([(-1,0),(3,0)],"p(x)=(x+1)(x−3)², −1 simple cross,3 double touch"),
55:([(0,0),(2,0)],"p(x)=x²(x−2), 0 double touch,2 simple cross"),
56:([(-2,0),(2,0)],"p(x)=(x+2)³(x−2), −2 triple crosses with flattening,2 simple crosses")
}
for n,(points,solution)in independent.items():
 put(n,"READ_FIRSTPARTY_INDEPENDENT_POLYNOMIAL_GRAPH",
     f"An independently constructed graph with labelled coordinates {points} and its displayed smooth polynomial factor curves is supplied. Infer a consistent least-degree polynomial and justify the roots/multiplicity as visible.",
     solution+" (Independent graph alternative; not claimed to match printed source figure.)",
     ["read plotted roots and labelled coordinates","choose minimal-degree factorization","explain multiplicity"])
# reconstruct from printed zero descriptions but present in independently
# authored question language, not source verbatim text.
def polynomial_from_roots(number,root_multiplicities,x0,y0):
 factor_value=F(1)
 for root,m in root_multiplicities:factor_value*=F(x0-root)**m
 assert factor_value!=0
 scalar=F(y0)/factor_value
 return f"p(x)=({scalar})"+"".join(f"(x−({root}))^{m}"for root,m in root_multiplicities),scalar
source57=[
(57,[(-2,1),(1,1),(3,1)],0,-4),
(58,[(-5,1),(-2,1),(1,1)],0,6),
(59,[(3,2),(1,2),(-3,1)],0,9),
(60,[(4,2),(1,1),(-2,1)],0,-3),
(61,[(1,2),(3,3)],2,15),
(62,[(4,1),(3,1),(2,1)],0,-24),
(63,[(-3,1),(-2,1),(1,1)],0,12),
(64,[(-3,2),(2,2),(-2,1)],0,4),
(65,[(F(1,2),2),(6,1),(-2,1)],0,18),
(66,[(-3,2),(0,3)],1,32)]
for n,roots,x0,y0 in source57:
 func,a=polynomial_from_roots(n,roots,F(x0),F(y0))
 put(n,"CONSTRUCT_POLYNOMIAL_FROM_ROOT_MULTIPLICITY_AND_POINT",
     f"Construct an independent polynomial from root/multiplicity pairs {roots} passing through point ({x0},{y0}). Find the unique scalar multiplier for that factor product.",
     f"Leading scalar a={a}; {func}.",
     ["multiply factors with multiplicities","calculate given-point factor product","solve scalar"])
# Local extrema via elementary derivative, report the critical equation
# with approximate numerical outputs computed by a monotone root search.
poly={
67:([1,0,-1,-1],lambda x:x**3-x-1,[(-1/math.sqrt(3)),1/math.sqrt(3)]),
68:([2,0,-3,-1],lambda x:2*x**3-3*x-1,[-1/math.sqrt(2),1/math.sqrt(2)]),
69:([1,0,0,1,0],lambda x:x**4+x,[-(1/4)**(1/3)]),
70:([-1,0,0,3,-2],lambda x:-x**4+3*x-2,[(3/4)**(1/3)]),
71:([1,-1,0,0,1],lambda x:x**4-x**3+1,[0,0.75])
}
for n,(co,fun,crit)in poly.items():
 critical=[(round(x,6),round(fun(x),6))for x in crit]
 if n==71:
  note="At x=0 derivative zero but not a local extremum; at x=3/4 there is a local (global) minimum."
 else:note="Classify critical values by derivative sign and note quartic/cubic end behavior."
 put(n,"CALCULUS_ASSISTED_LOCAL_POLYNOMIAL_EXTREMA",
     f"Investigate local minima/maxima of the polynomial with descending coefficients {co}. Differentiate, solve p′(x)=0, and classify actual local extrema.",
     f"Stationary candidates (x,p(x))≈{critical}. {note}",
     ["differentiate polynomial","solve derivative zero","classify extrema not every stationary point"])
for n,points,zeros,ans in [
(72,[(0,8),(F(2,3),0)],["−2/3","2/3"],"A quadratic model with roots ±2/3 and y-intercept8 is p(x)=18(4/9−x²)=8−18x²."),
(73,[(0,50000000),(500,0)],[500],"Least-degree linear representative p(x)=50000000(1−x/500); a cubic would require additional shape constraints not supplied."),
(74,[(100,0),(0,-90000),(-300,0)],[100,-300],"One quadratic alternative through these labelled points is p(x)=3(x−100)(x+300), p(0)=−90000.")
]:
 put(n,"RECONSTRUCT_FROM_FIRSTPARTY_GRAPH_ANCHORS",
     f"Using only independently labelled coordinate anchors {points} and x-intercept set {zeros}, propose a least-degree polynomial consistent with them. State when the inputs are underdetermined.",
     ans,["select factors from indicated zeros","solve scale from nonzero point","check consistency and nonuniqueness"])
applications=[
(75,"Cut squares of side x from corners of 10×8 sheet, fold into open box. Give polynomial volume with feasible x.","V(x)=x(10−2x)(8−2x)=4x³−36x²+80x; 0<x<4."),
(76,"Cut squares of side 2x from corners of a 10×8 sheet. Give polynomial volume and domain.","V(x)=2x(10−4x)(8−4x)=32x³−144x²+160x; 0<x<2."),
(77,"On a 12×12 sheet, cut corner squares side x+1 and fold. Find box volume and domain with x≥0.","V(x)=(x+1)(10−2x)², 0≤x<5; nonzero box volume."),
(78,"A cylinder has radius r=x+2 units and height 3 greater than radius. Express its volume.","V(x)=π(x+2)²(x+5) cubic units, x≥0."),
(79,"A right circular cone has radius r=3x+6 units and height three units less than radius. Express volume polynomial.","V(x)=(π/3)(3x+6)²(3x+3)=9π(x+2)²(x+1) cubic units, x≥0.")
]
for n,question,ans in applications:
 put(n,"GEOMETRIC_POLYNOMIAL_VOLUME_MODEL",
     question,ans,
     ["derive valid physical dimensions","multiply area by height or use volume formula","state cubic polynomial and admissible domain"])
assert sorted(q)==list(range(1,80)),len(q)
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l04-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L04",
"OriginalNumberedExerciseAnchors":79,
"FirstPartyIndependentStudentQuestions":[{"Number":n,"AcademicSkill":v["SourceAcademicSkill"],"Prompt":v["IndependentFirstPartyStudentQuestion"],"GradingMode":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(q.items())],
"TeacherOnlyMathScoring":[{"Number":n,"Expected":v["TeacherExpectedMathematics"],"Actions":v["NamedSourceCognitiveActions"]}for n,v in sorted(q.items())],
"PublisherOriginalGraphImagesCopied":False,
"ExactPublisherOriginalNumericAndGraphicTextParityVerified":False,
"StageDStudentTeacherRuntime":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l04-original79-polynomial-graph-exercise-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"OriginalSourceSHA256":src["SourceSha256"],
"OriginalNumberedExerciseAnchors":79,"IndependentFirstPartySourceMathSkillQuestionCount":79,
"OriginalPublisherVerbatimNumericAndGraphPixelParityCertified":False,
"PublisherImagesCopied":False,"StageD":"NOT_RUN",
"Items":[q[n]for n in sorted(q)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l04-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["SourceOriginal79QuestionMathSkillContracts"]="79_INDEPENDENT_FIRSTPARTY_POLYNOMIAL_SOURCE_SKILL_STUDENT_TEACHER_RUBRICS_AUTHORED"
c["StudentTeacherManualAssessment"]=native.relative_to(root).as_posix()
c["OriginalNumberedSourceExerciseMathSkillContract"]=manifest.relative_to(root).as_posix()
c["OriginalSourceFigureMathSkills"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L04_79_POLYNOMIAL_GRAPH_SOURCE_SKILL_FIRSTPARTY_TEACHER_MATH_QUESTION_CONTRACTS")
