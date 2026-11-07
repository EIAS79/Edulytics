from pathlib import Path
import json,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson006.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.6"
def fmtpoly(coeffs):
 degree=len(coeffs)-1
 return " + ".join(f"({v})x^{degree-i}"for i,v in enumerate(coeffs)if v!=0)or"0"
def evalp(p,x):
 result=F(0)
 for c in p:result=result*x+c
 return result
def divisors(n):
 n=abs(int(n))
 return [i for i in range(1,n+1)if n%i==0]if n else[1]
def rational_candidates(p):
 if p[-1]==0:return[F(0)]
 return sorted({F(s*a,b)for a in divisors(p[-1])for b in divisors(p[0])for s in(1,-1)})
def sydivide(p,k):
 q=[F(p[0])]
 for c in p[1:]:q.append(F(c)+k*q[-1])
 return q[:-1],q[-1]
def factor_linear(p):
 a=list(map(F,p));roots=[]
 while len(a)>3:
  k=next((v for v in rational_candidates(a)if evalp(a,v)==0),None)
  if k is None:break
  quotient,remainder=sydivide(a,k)
  assert remainder==0
  roots.append(k);a=quotient
 return roots,a
def solve_quadratic(a):
 if len(a)==1:return []
 if len(a)==2:return [f"{-F(a[1])/a[0]}"]
 if len(a)!=3:return["remaining higher-degree factor "+fmtpoly(a)]
 A,B,C=a
 delta=B*B-4*A*C
 if delta>=0:
  root=math.isqrt(delta.numerator);d=math.isqrt(delta.denominator)
  if root*root==delta.numerator and d*d==delta.denominator:
   return[f"{(-B+F(root,d))/(2*A)}",f"{(-B-F(root,d))/(2*A)}"]
  return[f"({-B}±√({delta}))/{2*A} (two real roots)"]
 return[f"({-B}±i√({-delta}))/{2*A} (two nonreal complex roots)"]
def realroots(p):
 r,a=factor_linear(p)
 tail=solve_quadratic(a)
 if any("nonreal complex" in v for v in tail):tail=[]
 if any("remaining higher" in v for v in tail):return[r],"remainder "+fmtpoly(a)
 return[str(v)for v in r]+tail,""
q={}
def put(n,skill,prompt,answer,actions=None):
 assert n not in q,n
 q[n]={"Number":n,"OriginalBookPdfPage":291 if n<=51 else 292,
 "SourceMathematicalCognitiveSkill":skill,"IndependentFirstPartyStudentTask":prompt,
 "TeacherExpectedMathAndExplanation":answer,
 "SourceCognitiveActions":actions or ["calculate using polynomial algebra","justify exact zeros or source theorem"],
 "OriginalPublisherVerbatimQuestionCopied":False,
 "OriginalPublisherGraphicBitmapCopied":False,
 "OriginalExactSourceMathNumericalInstanceAndFigurePixelParityCertified":False,
 "LiveStudentTeacherRuntime":"NOT_RUN"}
for n,prompt,ans in[
(1,"What does the Remainder Theorem compute without polynomial long division?","p(k) is the remainder of p(x)/(x−k)."),
(2,"Explain why a rational-root candidate list need not contain all real zeros.","Irrational real roots and nonreal complex roots are possible; candidates also may not be zeros."),
(3,"Explain distinction between real zeros and rational zeros.","Rational zeros are ratios of integers; all rational zeros are real but not all real roots rational."),
(4,"What does 0 or 1 Descartes sign change imply about positive roots?","0 changes forces 0 positive roots; 1 change forces exactly 1 positive real root, counting multiplicity."),
(5,"Why might synthetic division by x−r succeed twice?","Repeated zero r may have multiplicity ≥2; test same root again on quotient.")
]:put(n,"RATIONAL_ZERO_THEOREM_VERBAL",prompt,ans)
remainders={
6:([1,0,-9,0,14],F(2)),
7:([3,-2,1,-4],F(-3)),
8:([1,5,0,-4,-17],F(-1)),
9:([-3,6,24],F(4)),
10:([5,-4,3,-2,1,-1],F(-6)),
11:([1,0,0,0,-1],F(4)),
12:([3,4,-8,2],F(3)),
13:([4,5,-2,7],F(-2))
}
for n,(p,k)in remainders.items():
 result=evalp(p,k)
 put(n,"EVALUATE_REMAINDER_THEOREM",
    f"Determine remainder of dividing p(x)={fmtpoly(p)} by x−({k}), using direct evaluation.",
    f"Remainder p({k})={result}.",["substitute divisor root","calculate exact result"])
factor_known={
14:([2,-9,13,-6],F(1)),15:([2,1,-5,2],F(-2)),
16:([3,1,-20,12],F(-3)),17:([2,3,1,6],F(-2)),
18:([-5,16,0,-9],F(3)),19:([1,3,4,12],F(-3)),
20:([4,0,-7,3],F(1)),21:([2,5,-12,-30],F(-5,2))
}
for n,(p,k)in factor_known.items():
 r,rest=factor_linear(p)
 check=evalp(p,k)
 assert check==0,(n,check)
 allroots= [str(x)for x in r]+solve_quadratic(rest)
 put(n,"USE_FACTOR_THEOREM_TO_SOLVE_POLYNOMIAL",
    f"Given polynomial p(x)={fmtpoly(p)} and confirmed candidate linear factor x−({k}), find all its real roots by exact synthetic division and factorization.",
    f"p({k})=0. Rational factors found {list(map(str,r))}; remaining factor {fmtpoly(rest)}, solutions {allroots}.",
    ["confirm p(k)=0","perform synthetic division","solve remaining quadratic and retain only real roots"])
allpolys={
22:[1,-3,-10,24],23:[2,7,-10,-24],24:[1,2,-9,-18],25:[1,5,-16,-80],
26:[1,-3,-25,75],27:[2,-3,-32,-15],28:[2,1,-7,-6],29:[2,-3,-1,1],
30:[3,-1,-11,-6],31:[2,-5,9,-9],32:[2,-3,4,3],
33:[1,-2,-7,8,12],34:[1,2,-9,-2,8],35:[4,4,-25,-1,6],36:[2,-3,-15,32,-12],
37:[1,2,-4,-10,-5],38:[4,0,-3,1],39:[8,26,39,26,6],
40:[1,1,1,1],41:[1,-8,25,-26],42:[1,13,57,85],43:[3,-4,11,10],
44:[1,2,22,50,-75],45:[2,-3,32,17]}
for n,p in allpolys.items():
 rational,tail=factor_linear(p)
 tailroots=solve_quadratic(tail)
 if n<=39:
  tailroots=[x for x in tailroots if "nonreal complex"not in x]
  prompt=f"Using the Rational Zero Theorem, find all REAL zeros of p(x)={fmtpoly(p)} and state whether remaining factors yield irrational or nonreal solutions."
 else:
  prompt=f"Find all COMPLEX (real and nonreal) zeros of p(x)={fmtpoly(p)}; apply Rational Zero Theorem and solve the final factor."
 put(n,"RATIONAL_ZERO_FACTORING_AND_COMPLETE_ROOTS",
     prompt,
     f"Tested rational roots {list(map(str,rational))}; remaining factor {fmtpoly(tail)}; additional roots {tailroots}.",
     ["list/test rational candidates","synthetically divide repeated roots","solve residual quadratic","include correct real/complex root scope"])
descartes={
46:([1,0,0,-1],"positive1 negative0"),
47:([1,0,-1,0,-1],"positive1 negative1, x≈±1.272020"),
48:([1,-2,-5,6],"positive2 negative1, roots −2,1,3"),
49:([1,-2,1,-1],"positive1 negative0, positive x≈1.754878"),
50:([1,2,-12,14,-5],"positive1 negative1, x=1 (triple), x=−5"),
51:([2,37,200,300],"positive0 negative3, roots −10,−6,−5/2"),
52:([1,-2,-16,32],"positive2 negative1, roots −4,2,4"),
53:([2,-5,-5,5,3],"positive2 negative2, roots −1,−1/2,1,3"),
54:([2,-5,-14,20,8],"positive2 negative2, approx −2.245988,−0.332882,1.491279,3.587591"),
55:([10,0,-21,0,11],"positive2 negative2, roots ±1 and ±√(11/10)")
}
def changes(vals):
 signs=[1 if v>0 else -1 for v in vals if v!=0]
 return sum(signs[i]!=signs[i-1]for i in range(1,len(signs)))
def choices(n):return list(range(n,-1,-2))
for n,(p,actual)in descartes.items():
 sign_pos=changes(p)
 deg=len(p)-1
 neg=[c*((-1)**(deg-i))for i,c in enumerate(p)]
 sign_neg=changes(neg)
 put(n,"DESCARTES_RULE_SIGNS_AND_ACTUAL_ROOT_COUNTS",
     f"For p(x)={fmtpoly(p)}, determine Descartes permitted positive/negative real root counts, then compare to actual numerical or algebraic roots.",
     f"p(x) sign changes={sign_pos}, positive possibilities {choices(sign_pos)}; p(−x) sign changes={sign_neg}, negative possibilities {choices(sign_neg)}; actual {actual}.",
     ["count coefficient sign changes ignoring zero","apply p(−x)","distinguish possible from actual zeros"])
numeric={
56:[1,3,0,-4,4],
57:[2,3,-8,5],
58:[3,5,-5,4],
59:[6,0,-10,13,1],
60:[4,-10,8,1,0,-8]
}
for n,p in numeric.items():
 candidates=rational_candidates(p)
 put(n,"ENUMERATE_RATIONAL_ZERO_CANDIDATES",
     f"List all possible reduced rational zeros of polynomial p(x)={fmtpoly(p)} according to Rational Zero Theorem; do not declare untested values as roots.",
     "Reduced candidates "+str(list(map(str,candidates)))+".",
     ["factor leading and constant coefficients","reduce ±p/q to unique values"])
technology={
61:[6,-7,0,1],62:[4,-4,-13,-5],63:[8,-6,-23,6],
64:[12,55,12,-117,54],65:[16,-24,1,-15,25]
}
for n,p in technology.items():
 rat,remaining=factor_linear(p)
 d=remaining
 roots= [str(r)for r in rat]+[s for s in solve_quadratic(d)if "nonreal complex"not in s]
 put(n,"EXACT_RATIONAL_ZERO_VERIFICATION_FROM_GRAPH",
     f"A graph suggests rational x-intercepts for p(x)={fmtpoly(p)}. Find every exact REAL rational root algebraically and verify by substitution.",
     f"Rational roots {list(map(str,rat))}, residual {fmtpoly(d)}, remaining real roots {roots}; verify p(r)=0.",
     ["read candidate from independent first-party plotted graph","synthetic divide and solve residual","give exact roots"])
construct=[
(66,[(F(-1),1),(F(1),1),(F(3),1)],F(2),F(4)),
(67,[(F(-1),2),(F(1),1)],F(2),F(4)),
(68,[(F(-2),1),(F(1,2),2)],F(-3),F(5)),
(69,[(F(-1,2),1),(F(0),1),(F(1,2),1)],F(-2),F(6)),
(70,[(F(-4),1),(F(-1),1),(F(1),1),(F(4),1)],F(-2),F(10))
]
for n,roots,x0,y0 in construct:
 factor=F(1)
 for r,m in roots:factor*=(x0-r)**m
 assert factor!=0
 a=y0/factor
 put(n,"CONSTRUCT_POLYNOMIAL_FROM_ZEROS_AND_POINT",
     f"Construct a least-degree real polynomial whose zeros (root,multiplicity) are {[(str(r),m)for r,m in roots]} and that passes through ({x0},{y0}).",
     f"Scalar a={a}; polynomial p(x)=({a})"+"".join(f"(x−({r}))^{m}"for r,m in roots)+".",
     ["multiply required root factors","evaluate at given nonzero point","solve scalar and state degree"])
context=[
(71,"A rectangular box length=2w and height=w+2 inches with width w inches has volume192 cubic inches. Find positive dimensions.","w=4in, length8in, height6in, check 4×8×6=192."),
(72,"A box has consecutive positive integer dimensions and volume120 cubic units. Find the dimensions.","4,5,6 units, 4×5×6=120."),
(73,"A box length is one inch greater than width, which is one inch greater than height; volume86.625 cubic inches.","height3.5in, width4.5in, length5.5in."),
(74,"A box has length=3h, height=h, width=h+1 inches and volume108 cubic inches.","h=3in, width4in,length9in."),
(75,"A box length=w+3, width=h+2, and height=h inches has volume120 cubic inches.","h=3in,width5in,length8in."),
(76,"Use CONSISTENT metre units (printed source mixes inches with cubic metres): a cylinder radius is h+3 metres, height h metres, volume16π cubic metres. Find positive dimensions.","h=1m,r=4m, volume16πm³; original mixed-units discrepancy preserved in teacher note."),
(77,"A cylinder height h=r/2−1 metres, radius r metres and volume72π cubic metres. Find dimensions.","r=6m,h=2m, volume72πm³."),
(78,"A cylinder radius r=h+1 metres, volume48π cubic metres. Find dimensions.","h=3m,r=4m."),
(79,"A cylinder height h=r+2 metres and volume28.125π cubic metres. Find dimensions.","r=2.5m,h=4.5m."),
(80,"A cylinder radius r=h+1/3 metres and volume(98/9)π cubic metres. Find exact dimensions.","h=2m,r=7/3m.")
]
for n,prompt,answer in context:
 put(n,"APPLIED_CUBIC_BOX_AND_CYLINDER_DIMENSIONS",prompt,answer,
     ["translate geometric constraints to cubic equation","find positive real root","calculate physical dimensions and units"])
assert sorted(q)==list(range(1,81)),len(q)
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l06-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L06",
"OriginalNumberedSourceExercises":80,
"FirstPartyIndependentStudentMathQuestions":[{"Number":n,"SourceSkill":v["SourceMathematicalCognitiveSkill"],"Question":v["IndependentFirstPartyStudentTask"],"GradingMode":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(q.items())],
"TeacherOnlyExpectedAlgebraAndRubrics":[{"Number":n,"Expected":v["TeacherExpectedMathAndExplanation"],"NamedCognitiveActions":v["SourceCognitiveActions"]}for n,v in sorted(q.items())],
"OriginalPublisherGraphicBitmapsCopied":False,
"ExactPublisherOriginalVerbatimNumericQuestionAndGraphicPixelParityCertified":False,
"OriginalSourceMixedUnitsExercise76":"Original mixes inches for dimensions with cubic meters for volume; first-party equivalent uses consistent meter units",
"StageDStudentTeacherRuntime":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l06-original80-polynomial-zeros-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],
"OriginalSourceNumberedExercises":80,"FirstPartySourceMathSkillQuestions":80,
"OriginalPublisherExactVerbatimGraphicNumericParityClaim":False,
"SourceOriginalImagesCopied":0,"StageD":"NOT_RUN",
"Items":[q[n]for n in sorted(q)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l06-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Original80SourceNumberedQuestionTeacherMathRubrics"]="80_INDEPENDENT_FIRSTPARTY_POLYNOMIAL_REMAINDER_FACTOR_RATIONAL_COMPLEX_ROOTS_TEACHER_RUBRICS"
c["FirstPartyTeacherMathRubricPath"]=native.relative_to(root).as_posix()
c["Source80NumberedQuestionContractPath"]=manifest.relative_to(root).as_posix()
c["OriginalSourceFigureSkillCrosswalk"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L06_80_SOURCE_SKILL_POLYNOMIAL_ROOTS_INDEPENDENT_FIRSTPARTY_TEACHER_EXACT_MATH_KEYS_AUTHORING_COMPLETE")
