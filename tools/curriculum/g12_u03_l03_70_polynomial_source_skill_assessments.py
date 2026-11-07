from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson003.json").read_text(encoding="utf-8"))
assert src["Section"]=="3.3"
questions={}
def add(n,skill,prompt,teacher,parts=None):
 assert n not in questions,n
 questions[n]={
 "Number":n,"OriginalSourcePDFPage":251 if n<=33 else 252 if n<=45 else 253,
 "OriginalSourceAcademicSkill":skill,
 "FirstPartyIndependentStudentQuestion":prompt,
 "TeacherExpectedMathAndRubric":teacher,
 "SourceNamedCognitiveActions":parts or ["identify polynomial structure","derive and justify answer"],
 "OriginalPublisherVerbatimQuestionOrFigureCopied":False,
 "ExactPublisherOriginalNumberAndGraphicPixelParityCertified":False,
 "StudentTeacherRuntime":"NOT_RUN"}
for n,q,a in[
(1,"Explain the difference between coefficient a and exponent n in the power function a·x^n.","a is multiplier/coefficient, n is power/exponent (degree if nonzero coefficient)."),
(2,"How can one determine degree of a factored polynomial without expanding all factors?","Add degrees of nonzero polynomial factors and note nonzero scalar constant."),
(3,"Explain left/right end behavior of odd degree with positive leading coefficient.","Left tail down as x→−∞; right tail up as x→∞."),
(4,"What is the upper bound on turning points of degree n≥1 polynomial?","At most n−1 local turning points."),
(5,"Both ends of a polynomial graph go downward. What degree parity and sign of leading coefficient are possible?","Even degree, negative leading coefficient.")
]:add(n,"POWER_POLYNOMIAL_VERBAL",q,a)
functions={
6:("x⁵","single positive power function and polynomial"),
7:("(x²)³","single power x⁶ and polynomial"),
8:("x−x⁴","polynomial, not a single power"),
9:("x²/(x²−1)","rational, not polynomial or power function"),
10:("2x(x+2)(x−1)²","polynomial, not single power"),
11:("3x+1","polynomial, not a single power function")
}
for n,(expr,cat)in functions.items():
 add(n,"CLASSIFY_POWER_OR_POLYNOMIAL",
     f"Classify f(x)={expr} as power function, polynomial but not one power, or neither; explain using exponents and factor structure.",
     cat,["simplify product or powers","classify and justify"])
degree={
12:("−3x",1,-3),
13:("7−2x²",2,-2),
14:("−2x²−3x⁵+x−6",5,-3),
15:("x(4−x²)(2x+1)",4,-2),
16:("x²(2x−3)²",4,4)
}
for n,(expression,de,leading)in degree.items():
 add(n,"DEGREE_LEADING_COEFFICIENT",
     f"For p(x)={expression} find polynomial degree, leading term and coefficient.",
     f"degree {de}, leading term ({leading})x^{de}, leading coefficient {leading}.",
     ["find highest-degree factor product","state leading term","read leading coefficient"])
ends={
17:("x⁴",4,1),
18:("x³",3,1),
19:("−x⁴",4,-1),
20:("−x⁹",9,-1),
21:("−2x⁴−3x²+x−1",4,-2),
22:("3x²+x−2",2,3),
23:("x²(2x³−x+1)",5,2),
24:("(2−x)⁷",7,-1)
}
for n,(expression,degree,lead)in ends.items():
 left="up"if (degree%2==0 and lead>0)or (degree%2==1 and lead<0)else "down"
 right="up"if lead>0 else "down"
 add(n,"POLYNOMIAL_END_BEHAVIOR",
     f"Explain symbolic x→−∞ and x→+∞ end behavior of p(x)={expression} using leading term, degree parity and sign.",
     f"Leading coefficient {lead}, degree {degree}; left tail {left} ({'+∞'if left=='up'else '−∞'}), right tail {right} ({'+∞'if right=='up'else '−∞'}).",
     ["simplify leading term","classify parity","write both infinity limits"])
zeros={
25:("2(t−1)(t+2)(t−3)",["1","−2","3"],12),
26:("−2(3n−1)(2n+1)",["1/3","−1/2"],2),
27:("x⁴−16",["−2","2"],-16),
28:("x³+27",["−3"],27),
29:("x(x²−2x−8)",["0","−2","4"],0),
30:("(x+3)(4x²−1)",["−3","−1/2","1/2"],-3)
}
for n,(expression,roots,y0)in zeros.items():
 add(n,"REAL_X_INTERCEPTS_FACTORIZATION",
     f"For independent presentation p(x)={expression}, find all real zeros and y-intercept; explain factorization.",
     f"Real zero set {roots}; y-intercept (0,{y0}).",
     ["factor and list distinct real zeros","evaluate at zero"])
# The original item 31–45 is graph-only, so independently model a graph
# via explicitly labelled polynomial formula, turning-point count and
# root multiplicity. Does not claim publisher's plotted pixels.
graphmodels={
31:("x²−4",2,1),32:("x³−3x",3,2),33:("−x⁴+2x²",4,3),
34:("x⁵−4x³",5,4),35:("2x−1",1,0),36:("x⁴−3x²",4,3),
37:("−x³+x",3,2),38:("x⁶−5x⁴+4x²",6,5),
39:("x²+1",2,1),40:("x³−x",3,2),41:("abs(x)",None,None),
42:("1/(x−1)",None,None),43:("x⁴−4x²",4,3),
44:("x²+2x+1",2,1),45:("x³−3x²+3x−1",3,2)
}
for n,(expression,degree,bound)in graphmodels.items():
 if degree is None:
  if n==41:
   expected="Not a polynomial: absolute-value V graph has a sharp corner."
  else:
   expected="Not a polynomial: rational graph has a vertical asymptote and a discontinuity."
 else:
  expected=f"Polynomial of degree {degree}; theoretical max turning points {bound}; graph must be smooth and continuous."
 add(n,"INDEPENDENT_POLYNOMIAL_GRAPH_CLASSIFICATION",
     f"Analyze an independently plotted graph of f(x)={expression}. Is it polynomial? If yes, give degree, smoothness and a defensible bound on turning points; if not, explain failure.",
     expected,["inspect graph continuity/smoothness","infer polynomial classification","bound turning-point count"])
from math import isclose
tables={
46:("−x³",lambda x:-x**3,-3,-1),
47:("x⁴−5x²",lambda x:x**4-5*x**2,4,1),
48:("x²(1−x)²",lambda x:x*x*(1-x)**2,4,1),
49:("(x−1)(x−2)(3−x)",lambda x:(x-1)*(x-2)*(3-x),3,-1),
50:("x⁵/10−x⁴",lambda x:F(x**5,10)-x**4,5,F(1,10))
}
for n,(expr,fun,degree,lead)in tables.items():
 xs=[-10,-5,0,5,10]
 ys=[str(fun(x))for x in xs]
 left="up"if (degree%2==0 and lead>0)or (degree%2==1 and lead<0)else "down"
 right="up"if lead>0 else "down"
 add(n,"NUMERIC_TABLE_CONFIRM_END_BEHAVIOR",
     f"Create a first-party end-behavior table for p(x)={expr} using x={xs}; determine both infinite tails from the leading term.",
     f"Outputs {ys}; degree{degree} leading{lead}, left {left}, right {right}.",
     ["evaluate data table at negative/positive inputs","calculate degree/leading sign","infer tails"])
graphed={
51:("x³(x−2)",["0 (multiplicity 3)","2"],4,1,0),
52:("x(x−3)(x+3)",["−3","0","3"],3,1,0),
53:("x(14−2x)(10−2x)",["0","5","7"],3,4,0),
54:("x(14−2x)(10−2x)²",["0","5 (double)","7"],4,-8,0),
55:("x³−16x",["−4","0","4"],3,1,0),
56:("x³−27",["3"],3,1,-27),
57:("x⁴−81",["−3","3"],4,1,-81),
58:("−x³+x²+2x",["−1","0","2"],3,-1,0),
59:("x³−2x²−15x",["−3","0","5"],3,1,0),
60:("x³−0.01x",["−0.1","0","0.1"],3,1,0)
}
for n,(formula,roots,degree,lead,yint)in graphed.items():
 tail_left="up"if (degree%2==0 and lead>0)or (degree%2==1 and lead<0)else "down"
 tail_right="up"if lead>0 else "down"
 add(n,"GRAPH_POLYNOMIAL_REAL_ROOTS_AND_TAILS",
     f"Sketch an independent coordinate graph of p(x)={formula}. Label real roots, intercepts, and both end behaviors.",
     f"Roots {roots}, y-intercept (0,{yint}), degree{degree}, leading{lead}, left {tail_left}, right {tail_right}.",
     ["factor or solve real zeros","plot and label intercepts","infer leading-term end behavior"])
construct={
61:("−4",[-2,2],"x²−4",2,1),
62:("9",[-3,3],"−x²+9",2,-1),
63:("0",[0,2],"x²(x−2)",3,1),
64:("1",[1],"−(x−1)³",3,-1),
65:("1",[],"x⁴+1",4,1)
}
for n,(y0,rootset,model,degree,lead)in construct.items():
 add(n,"CONSTRUCT_POLYNOMIAL_FROM_GRAPH_PROPERTIES",
     f"Construct one polynomial with degree {degree}, leading coefficient {'+'if lead>0 else '−'}1, y-intercept {y0}, and real x-intercepts {rootset}. Provide factored or general form and explain multiplicity where needed.",
     f"One acceptable model p(x)={model}; verify p(0)={y0} and root set {rootset}.",
     ["use factors and multiplicities","fix leading coefficient","verify intercepts"])
contexts=[
(66,"A circular oil-slick radius grows at 20 m/day starting from zero. Model area as polynomial A(d).","A(d)=π(20d)²=400πd² square metres for d≥0 days."),
(67,"A cube edge begins 3 ft and increases 2 ft per minute. Write cubic volume V(m).","V(m)=(3+2m)³ cubic feet, m≥0."),
(68,"A 10-inch by 6-inch rectangle has length increased by x and width by 2x. Write area A(x).","A(x)=(10+x)(6+2x)=2x²+26x+60 square inches, x≥0."),
(69,"Create an open box by cutting x-inch squares from each corner of an 8×8 inch sheet. Derive V(x) and domain.","V(x)=x(8−2x)² cubic inches, 0<x<4."),
(70,"An original rectangle has length twice its width w feet. Remove 2-foot-square corners and fold open box. Find volume as function of original width.","V(w)=2(w−4)(2w−4)=4(w−4)(w−2) cubic feet, original width w>4 feet.")
]
for n,q,a in contexts:
 add(n,"APPLIED_POLYNOMIAL_GEOMETRIC_MODEL",q,a,
     ["define geometric variables and units","construct product of dimensions","expand or state factored polynomial with physical domain"])
assert sorted(questions)==list(range(1,71))
assessment=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l03-firstparty-teacher-led.json"
assessment.parent.mkdir(parents=True,exist_ok=True)
assert not assessment.exists()
assessment.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L03",
"OriginalSourceNumberedExercises":70,
"FirstPartyIndependentStudentMathQuestions":[{"Number":n,"Skill":v["OriginalSourceAcademicSkill"],"Question":v["FirstPartyIndependentStudentQuestion"],"Grading":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(questions.items())],
"TeacherOnlyMathematicsRubrics":[{"Number":n,"Answer":v["TeacherExpectedMathAndRubric"],"OriginalSourceActions":v["SourceNamedCognitiveActions"]}for n,v in sorted(questions.items())],
"OriginalPublisherGraphBitmapsCopied":False,
"ExactOriginalPublisherVerbatimNumericAndGraphicQuestionParityCertified":False,
"StageDStudentTeacherAppRuntime":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l03-original70-power-polynomial-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceBookSHA256":src["SourceSha256"],
"OriginalNumberedSourceExerciseParents":70,
"IndependentFirstPartySourceSkillStudentQuestionCount":70,
"OriginalPublisherVerbatimNumericalInstanceAndGraphicPixelParityClaim":False,
"OriginalSourceImagesCopied":0,"StageD":"NOT_RUN",
"Items":[questions[n]for n in sorted(questions)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l03-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Original70ExerciseSkillTeacherRubrics"]="70_FIRSTPARTY_INDEPENDENT_POLYNOMIAL_SOURCE_SKILL_QUESTIONS_TEACHER_RUBRICS_AUTHORED"
c["TeacherAssessmentPath"]=assessment.relative_to(root).as_posix()
c["SourceExerciseContractPath"]=manifest.relative_to(root).as_posix()
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L03_70_NUMBERED_POWER_POLYNOMIAL_SOURCE_SKILL_FIRSTPARTY_TEACHER_MATH_CONTRACTS_AUTHORED")
