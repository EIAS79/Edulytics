from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson005.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.5"
def trim(a):
 i=0
 while i<len(a)-1 and a[i]==0:i+=1
 return a[i:]
def divpoly(a,b):
 a=[F(v)for v in a];b=[F(v)for v in b]
 assert b[0] and len(a)>=len(b)
 rem=a[:];q=[]
 for k in range(len(a)-len(b)+1):
  c=rem[k]/b[0];q.append(c)
  for j in range(len(b)):rem[k+j]-=c*b[j]
 r=trim(rem[len(q):]or[F(0)])
 assert all(v==0 for v in rem[:len(q)])
 return trim(q),r
def mulpoly(a,b):
 ans=[F(0)]*(len(a)+len(b)-1)
 for i,x in enumerate(a):
  for j,y in enumerate(b):ans[i+j]+=F(x)*F(y)
 return ans
def addpoly(a,b):
 a=[F(0)]*(max(len(a),len(b))-len(a))+[F(x)for x in a]
 b=[F(0)]*(max(len(a),len(b))-len(b))+[F(x)for x in b]
 return [x+y for x,y in zip(a,b)]
def verify(a,b,q,r):
 res=addpoly(mulpoly(b,q),r)
 assert trim(res)==trim([F(x)for x in a]),(a,b,q,r)
def show(a):
 n=len(a)-1
 return " + ".join(f"({v})x^{n-i}"for i,v in enumerate(a)if v!=0)or"0"
questions={}
def put(n,skill,prompt,expected,actions=None):
 assert n not in questions,n
 questions[n]={
 "Number":n,"OriginalSourcePdfPage":279 if n<=46 else 280,
 "SourceAcademicMathSkill":skill,"FirstPartyIndependentStudentMathQuestion":prompt,
 "TeacherExactMathAndRubric":expected,
 "NamedSourceCognitiveActions":actions or ["write polynomial division and exact quotient/remainder","verify dividend=divisor·quotient+remainder"],
 "OriginalPublisherVerbatimQuestionCopied":False,"PublisherGraphImagesCopied":False,
 "ExactPublisherNumericGraphPixelInstanceParityCertified":False,
 "StageDStudentTeacherScoring":"NOT_RUN"}
put(1,"FACTOR_REMAINDER_VERBAL",
    "What algebraic conclusion follows when dividing p(x) by a nonzero binomial yields remainder zero?",
    "The binomial divides p(x) exactly; p(x)=d(x)q(x) and is a factor.")
put(2,"QUOTIENT_DEGREE_VERBAL",
    "If degree n polynomial is divided by a degree1 nonzero divisor, what is quotient degree for n≥1?",
    "Degree n−1, provided the original leading coefficient is nonzero.")
input_cases={
3:([1,5,-1],[1,-1]),4:([2,-9,-5],[1,-5]),5:([3,23,14],[1,7]),
6:([4,-10,6],[4,2]),7:([6,-25,-25],[6,5]),8:([-1,0,-1],[1,1]),
9:([2,-3,2],[1,2]),10:([1,0,0,-126],[1,-5]),11:([3,-5,4],[3,1]),
12:([1,-3,5,-6],[1,-2]),13:([2,3,-4,15],[1,3]),
14:([3,-2,1,-4],[1,3]),15:([2,-6,-7,6],[1,-4]),16:([6,-10,-7,-15],[1,1]),
17:([4,-12,-5,-1],[2,1]),18:([9,-9,18,5],[3,-1]),19:([3,-2,1,-4],[1,3]),
20:([-6,1,0,-4],[2,-3]),21:([2,7,-13,-3],[2,-3]),22:([3,-5,2,3],[1,2]),
23:([4,-5,0,13],[1,4]),24:([1,0,-3,2],[1,2]),25:([1,-21,147,-343],[1,-7]),
26:([1,-15,75,-125],[1,-5]),27:([9,0,-1,2],[3,-1]),28:([6,-1,5,2],[3,1]),
29:([1,1,-3,-2,1],[1,1]),30:([1,0,-3,0,1],[1,-1]),
31:([1,2,-3,2,6],[1,3]),32:([1,-10,37,-60,36],[1,-2]),
33:([1,-8,24,-32,16],[1,-2]),34:([1,5,-3,-13,10],[1,5]),
35:([1,-12,54,-108,81],[1,-3]),36:([4,-2,0,-4,2],[2,-1]),
37:([4,2,-4,2,2],[2,1])
}
for n,(a,b)in input_cases.items():
 quo,r=divpoly(a,b);verify(a,b,quo,r)
 put(n,"LONG_OR_SYNTHETIC_POLYNOMIAL_DIVISION",
    f"Divide independent polynomial p(x)={show(a)} by d(x)={show(b)}. Give exact quotient and remainder; use {'long division'if n<=13 else 'synthetic-division reasoning (rescale if needed)'}.",
    f"Quotient={show(quo)}; remainder={show(r)}. Verified identity p=dq+r.",
    ["compute leading-degree quotient","restore all missing zero coefficients","verify remainder degree and identity"])
factor_cases={
38:([4,-3,-8,4],F(2)),39:([3,-6,0,-5,10],F(2)),
40:([-4,5,0,8],F(-3)),41:([4,0,-15,0,-4],F(2)),
42:([2,-1,0,2,-1],F(1,2)),
43:([3,1,0,-3,1],F(-1,3))}
for n,(a,k)in factor_cases.items():
 q,r=divpoly(a,[1,-k]);verify(a,[1,-k],q,r)
 yes=r==[0]
 put(n,"SYNTHETIC_TEST_PROPOSED_LINEAR_FACTOR",
    f"Determine by synthetic division whether x−({k}) divides p(x)={show(a)} exactly; if yes supply factorization.",
    f"p({k})=remainder {show(r)}; {'factor yes: p=(x−('+str(k)+'))('+show(q)+')'if yes else 'not a factor (nonzero remainder)'}.",
    ["evaluate synthetic remainder","decide exact factor","give quotient if factor"])
# Original 44–48 are graph-only x-coordinate tests with a provided quadratic
# factor. We supply independent first-party root anchors and original
# quadratic factor academic skill rather than publisher diagram pixels.
graphcases={
44:([1,-1,3],2),
45:([1,2,4],-1),
46:([1,2,5],3),
47:([1,1,1],-3),
48:([1,2,2],1)
}
for n,(quad,k)in graphcases.items():
 polynomial=mulpoly(quad,[1,-k])
 put(n,"POLYNOMIAL_FROM_INDEPENDENT_GRAPH_ROOT_AND_QUADRATIC_FACTOR",
    f"An independent graph labels a cubic real zero at x={k} and gives quadratic factor q(x)={show(quad)}. Find the complete monic cubic polynomial p(x) and verify by expansion.",
    f"p(x)=({show(quad)})(x−({k}))={show(polynomial)}.",
    ["use independently labelled real root","multiply given factor","verify degree and leading coefficient"])
for n,a,b in[
(49,[4,0,0,-33],[1,-2]),
(50,[2,0,0,25],[1,3]),
(51,[3,0,2,-5],[1,-1]),
(52,[-4,-1,0,-12],[1,4]),
(53,[1,0,0,0,-22],[1,2])
]:
 quo,r=divpoly(a,b);verify(a,b,quo,r)
 put(n,"POLYNOMIAL_DIVISION_EXTENSION",
    f"Divide p(x)={show(a)} by d(x)={show(b)}; state quotient, remainder and verification.",
    f"q={show(quo)}; r={show(r)}.")
for n,a,b,summary in[
(54,[1,0,0,0,-1],[1,-1],"Geometric series of powers for x⁴−1."),
(55,[1,0,0,0,0,0,0,1],[1,1],"Alternating quotient of x⁷+1 by x+1."),
(56,[1,0,0,0,-256],[1,-4],"Difference of fourth powers x⁴−4⁴."),
(57,[1,0,0,0,0],[1,1],"Quotient and nonzero remainder for x⁴/(x+1)."),
(58,[1,0,0,0,0],[1,-1],"Quotient and nonzero remainder for x⁴/(x−1).")
]:
 quo,r=divpoly(a,b);verify(a,b,quo,r)
 put(n,"SYNTHETIC_DIVISION_SEQUENCES",
    f"Study the independent division pattern {show(a)} ÷ ({show(b)}). Predict quotient and remainder and explain observed polynomial sequence.",
    f"Quotient {show(quo)}, remainder {show(r)}. {summary}",
    ["recognize pattern","predict quotient","verify with exact multiplication"])
complex_cases={
59:([1,1],1j),
60:([1,0,1],1j),
61:([1,1],-1j),
62:([1,0,1],-1j),
63:([1,0,0,1],1j)}
def complex_show(a):
 def single(c):
  x=round(c.real);y=round(c.imag)
  assert abs(x-c.real)<1e-10 and abs(y-c.imag)<1e-10
  return f"({x})+({y})i"
 return " + ".join(single(v)+f"x^{len(a)-i-1}"for i,v in enumerate(a))or"0"
for n,(a,k)in complex_cases.items():
 vals=[complex(x)for x in a]
 q=[vals[0]]
 for x in vals[1:]:q.append(x+k*q[-1])
 rem=q.pop()
 put(n,"SYNTHETIC_DIVISION_WITH_COMPLEX_LINEAR_DIVISOR",
     f"Divide the polynomial with descending real coefficients {a} by x−({round(k.real)}+{round(k.imag)}i). Use complex synthetic division and return a polynomial quotient and remainder.",
     f"Quotient={complex_show(q)}; remainder={complex_show([rem])}.",
     ["use k=±i as synthetic root","compute exact Gaussian-integer coefficients","verify complex quotient identity"])
for n,a,b in[
(64,[2,9,-5],[1,5]),
(65,[4,10,6,15],[2,5]),
(66,[6,-8,9,-9,-4],[3,-4])
]:
 quo,r=divpoly(a,b);verify(a,b,quo,r)
 put(n,"RECTANGLE_AREA_DIVIDE_TO_FIND_MISSING_WIDTH",
     f"A rectangle has algebraic area A(x)={show(a)} and known side L(x)={show(b)}. Divide to find its other side and report if remainder prevents exact side.",
     f"Other side W(x)={show(quo)}, remainder {show(r)}; physical side lengths must be positive.",
     ["form A/L","divide","state positivity restrictions"])
boxes={
67:([12,20,-21,-36],[2,3],[3,-4]),
68:([18,-21,-40,48],[3,-4],[3,-4]),
69:([10,27,2,-24],[5,-4],[2,3]),
70:([10,30,-8,-24],[2],[1,3])
}
for n,(v,l,w)in boxes.items():
 divisor=mulpoly(l,w)
 quo,r=divpoly(v,divisor);verify(v,divisor,quo,r)
 put(n,"RECTANGULAR_PRISM_VOLUME_DIVIDE_TO_HEIGHT",
     f"Find the height of a prism with volume V(x)={show(v)}, length L(x)={show(l)} and width W(x)={show(w)}, assuming positive nonzero dimensions.",
     f"H(x)={show(quo)}; remainder {show(r)}, verify L·W·H=V.",
     ["multiply known side polynomials","divide volume by cross-sectional area","state nonzero/positive domain"])
for n,v,radius in[
(71,[25,-65,-29,-3],[5,1]),
(72,[4,12,-15,-50],[2,5]),
(73,[3,24,46,-16,-32],[1,4])
]:
 divisor=mulpoly(radius,radius)
 quo,r=divpoly(v,divisor);verify(v,divisor,quo,r)
 put(n,"CYLINDER_VOLUME_DIVIDE_RADIUS_SQUARED",
     f"A cylinder has volume V(x)=π({show(v)}) and radius r(x)={show(radius)}. Find the exact algebraic height H(x)=V/[πr²], checking geometry constraints.",
     f"Height H(x)={show(quo)}, remainder {show(r)}; source model assumes r>0 and H>0.",
     ["square polynomial radius","cancel π and divide volume","enforce positive physical dimensions"])
assert sorted(questions)==list(range(1,74)),len(questions)
assessment=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l05-firstparty-teacher-led.json"
assessment.parent.mkdir(parents=True,exist_ok=True)
assert not assessment.exists()
assessment.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L05",
"OriginalNumberedSourceExercises":73,
"FirstPartyIndependentStudentMathTasks":[{"Number":n,"SourceSkill":v["SourceAcademicMathSkill"],"Question":v["FirstPartyIndependentStudentMathQuestion"],"Grading":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(questions.items())],
"TeacherOnlyExactPolynomialDivisionAnswers":[{"Number":n,"ExpectedMath":v["TeacherExactMathAndRubric"],"NamedSourceActions":v["NamedSourceCognitiveActions"]}for n,v in sorted(questions.items())],
"OriginalPublisherDiagramBitmapCopied":False,
"PublisherOriginalQuestionVerbatimNumericGraphicPixelParityCertified":False,
"LiveStudentTeacherGradebookRuntime":"NOT_RUN","StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l05-original73-polynomial-division-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"OriginalSourceSha256":source["SourceSha256"],
"OriginalSourceNumberedExerciseParents":73,
"IndependentFirstPartyAcademicMathSkillStudentQuestions":73,
"OriginalPublisherExactVerbatimAndGraphicPixelQuestionParityCertified":False,
"SourceImagesCopied":0,"StageD":"NOT_RUN",
"Items":[questions[n]for n in sorted(questions)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l05-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Original73SourceExerciseAcademicSkillRubrics"]="73_FIRSTPARTY_INDEPENDENT_EXACT_POLYNOMIAL_DIVISION_STUDENT_TEACHER_SCORING_CONTRACTS_AUTHORED"
c["TeacherManualAssessmentPath"]=assessment.relative_to(root).as_posix()
c["Source73ExerciseSkillContractPath"]=manifest.relative_to(root).as_posix()
c["OriginalSourceFigureCrosswalk"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L05_73_NUMBERED_EXACT_POLYNOMIAL_LONG_SYNTHETIC_COMPLEX_DIVISION_FIRSTPARTY_TEACHER_RUBRICS_AUTHORED")
