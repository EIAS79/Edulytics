from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson001.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.1"
ZERO=(F(0),F(0));ONE=(F(1),F(0));I=(F(0),F(1))
def z(a=0,b=0):return F(a),F(b)
def plus(a,b):return a[0]+b[0],a[1]+b[1]
def minus(a,b):return a[0]-b[0],a[1]-b[1]
def mult(a,b):return a[0]*b[0]-a[1]*b[1],a[0]*b[1]+a[1]*b[0]
def div(a,b):
 den=b[0]*b[0]+b[1]*b[1]
 assert den!=0
 return (a[0]*b[0]+a[1]*b[1])/den,(a[1]*b[0]-a[0]*b[1])/den
def power(a,n):
 if n<0:return div(ONE,power(a,-n))
 t=ONE
 for _ in range(n):t=mult(t,a)
 return t
def fmt(v):
 real,im=v
 return f"({real})+({im})i (exact complex standard form)"
q={}
def put(n,skill,prompt,ans,actions=None):
 assert n not in q,n
 q[n]={"Number":n,"SourcePdfPage":221 if n<=34 else 222,
 "SourceOriginalMathematicsSkill":skill,"IndependentFirstPartyStudentTask":prompt,
 "TeacherExpectedMath":ans,
 "StudentCognitiveActions":actions or ["show exact complex algebra and simplify into a+bi"],
 "OriginalPublisherVerbatimQuestionCopied":False,
 "OriginalPublisherFigurePixelsCopied":False,
 "StageDStudentTeacherScoring":"NOT_RUN"}
verbal=[
("Explain the algorithm for adding two complex numbers and demonstrate it.","Add real parts together and imaginary coefficients together; (a+bi)+(c+di)=(a+c)+(b+d)i."),
("What identity is essential when multiplying complex numbers?","Expand by distribution and replace i² with −1."),
("Provide two nonzero imaginary numbers whose product is a nonzero real number.","(2i)(3i)=−6, which is real."),
("Where do real numbers lie when plotted on complex axes?","On horizontal real axis with imaginary coordinate0.")
]
for n,(prompt,ans)in enumerate(verbal,1):put(n,"VERBAL_COMPLEX_FOUNDATIONS",prompt,ans)
subs={
5:("z²+z−4",I and z(0,2),lambda x:minus(plus(power(x,2),x),z(4))),
6:("z³−2",I,lambda x:minus(power(x,3),z(2))),
7:("z²+3z+5",z(2,1),lambda x:plus(plus(power(x,2),mult(z(3),x)),z(5))),
8:("2z²+z−3",z(2,-3),lambda x:minus(plus(mult(z(2),power(x,2)),x),z(3))),
9:("(z+1)/(2−z)",z(0,5),lambda x:div(plus(x,z(1)),minus(z(2),x))),
10:("(1+2z)/(z+3)",z(0,4),lambda x:div(plus(z(1),mult(z(2),x)),plus(x,z(3))))
}
for n,(formula,arg,func)in subs.items():
 put(n,"EVALUATE_COMPLEX_POLYNOMIAL_OR_RATIONAL",
     f"For independent complex input z={fmt(arg)}, evaluate f(z)={formula}; combine real/imaginary components and ensure denominator nonzero.",
     fmt(func(arg)),["substitute complex input","simplify every i² and/or conjugate quotient"])
put(11,"QUADRATIC_ROOTS_FROM_INDEPENDENT_GRAPH",
    "Consider a first-party graph of y=x²+1 with vertex (0,1) and no x-axis crossing. How many real and nonreal complex zeros solve x²+1=0?",
    "0 real roots, 2 nonreal complex roots ±i.")
put(12,"QUADRATIC_ROOTS_FROM_INDEPENDENT_GRAPH",
    "A first-party graph of y=x²−4 crosses the horizontal axis at (−2,0),(2,0). State number and values of real versus nonreal roots.",
    "2 real roots x=±2, no nonreal roots.")
for n,point in {
13:z(1,-2),14:z(-2,3),15:I,16:z(-3,-4)
}.items():
 put(n,"PLOT_COMPLEX_COORDINATE",
     f"Plot z={fmt(point)} on a labelled real/imaginary coordinate plane. Give exact ordered point.",
     f"Plot ({point[0]},{point[1]}) as (Re(z),Im(z)).",
     ["read real and imaginary components","mark correct quadrant or axis"])
expr={
17:("sum",z(3,2),z(5,-3)),
18:("sum",z(-2,-4),z(1,6)),
19:("difference",z(-5,3),z(6,-1)),
20:("difference",z(2,-3),z(3,2)),
21:("difference",z(-4,4),z(-6,9)),
22:("product",z(2,3),z(0,4)),
23:("product",z(5,-2),z(0,3)),
24:("product",z(6,-2),z(5)),
25:("product",z(-2,4),z(8)),
26:("product",z(2,3),z(4,-1)),
27:("product",z(-1,2),z(-2,3)),
28:("product",z(4,-2),z(4,2)),
29:("product",z(3,4),z(3,-4)),
30:("quotient",z(3,4),z(2)),
31:("quotient",z(6,-2),z(3)),
32:("quotient",z(-5,3),z(0,2)),
33:("quotient",z(6,4),I),
34:("quotient",z(2,-3),z(4,3)),
35:("quotient",z(3,4),z(2,-1)),
36:("quotient",z(2,3),z(2,-3))
}
ops={"sum":plus,"difference":minus,"product":mult,"quotient":div}
for n,(op,a,b)in expr.items():
 put(n,"COMPLEX_"+op.upper(),
     f"Calculate the complex {op} of A={fmt(a)} and B={fmt(b)} exactly. Express in a+bi form and show conjugate where dividing.",
     fmt(ops[op](a,b)),["perform required arithmetic","reduce i² and simplify exact fractions"])
for n,question,answer in[
(37,"Evaluate √(−9)+3√(−16) in standard form.","15i."),
(38,"Evaluate −√(−4)−4√(−25) in standard form.","−22i."),
(39,"Simplify [2+√(−12)]/2.","1+√3·i."),
(40,"Simplify [4+√(−20)]/2.","2+√5·i.")
]:put(n,"COMPLEX_RADICALS",question,answer)
for n,exponent in[(41,8),(42,15),(43,22)]:
 put(n,"IMAGINARY_POWER_CYCLE",
     f"Evaluate i^{exponent} using congruence of exponent modulo four.",
     fmt(power(I,exponent)),["reduce exponent mod4","state value"])
for n,base_number,powers_to_show,predict in [
(44,z(1,1),[4,8,12],16),
(45,z(1,-1),[2,6,10],14)
]:
 answers={e:fmt(power(base_number,e))for e in powers_to_show+[predict]}
 put(n,"COMPLEX_EXPONENTIAL_PATTERN",
     f"Evaluate ({fmt(base_number)}) raised to powers {powers_to_show}, then predict the value at exponent {predict}. State the exact algebraic recurrence.",
     str(answers),["calculate pattern powers","predict next exponent"])
vals={}
for k in (4,8,12,16):
 vals[k]=fmt(minus(power(z(1,1),k),power(z(1,-1),k)))
put(46,"DIFFERENCE_OF_COMPLEX_POWER_PATTERNS",
    "Evaluate (1+i)^k−(1−i)^k for k=4,8,12 and predict k=16. Explain why each result is real or zero.",
    str(vals),["compute three differences","predict k=16"])
put(47,"VERIFY_COMPLEX_ROOT_BY_EXACT_ARGUMENT",
    "Verify the complex unit-circle point z=√3/2+i/2 is a solution of z^6+1=0, using exact trigonometric powers or algebra.",
    "z=cos(π/6)+i sin(π/6); z^6=cosπ+i sinπ=−1; z^6+1=0.",
    ["identify unit magnitude/angle","raise to sixth power","substitute equation"])
put(48,"VERIFY_EIGHTH_ROOT_OF_UNITY",
    "Verify z=√2/2+i√2/2 solves z^8−1=0 using argument and modulus.",
    "z=cos(π/4)+i sin(π/4); z^8=cos2π+i sin2π=1; difference zero.",
    ["identify unit magnitude/angle","raise to eighth","verify"])
extras={
49:("1/i+4/i³",lambda:plus(div(ONE,I),div(z(4),power(I,3)))),
50:("1/i¹¹−1/i²¹",lambda:minus(div(ONE,power(I,11)),div(ONE,power(I,21)))),
51:("i⁷(1+i²)",lambda:mult(power(I,7),plus(ONE,power(I,2)))),
52:("i^(−3)+5i⁷",lambda:plus(power(I,-3),mult(z(5),power(I,7)))),
53:("[(2+i)(4−2i)]/(1+i)",lambda:div(mult(z(2,1),z(4,-2)),z(1,1))),
54:("[(1+3i)(2−4i)]/(1+2i)",lambda:div(mult(z(1,3),z(2,-4)),z(1,2))),
55:("(3+i)²/(1+2i)²",lambda:div(power(z(3,1),2),power(z(1,2),2))),
56:("(3+2i)/(2+i)+(4+3i)",lambda:plus(div(z(3,2),z(2,1)),z(4,3))),
57:("(4+i)/i+(3−4i)/(1−i)",lambda:plus(div(z(4,1),I),div(z(3,-4),z(1,-1)))),
58:("(3+2i)/(1+2i)−(2−3i)/(3+i)",lambda:minus(div(z(3,2),z(1,2)),div(z(2,-3),z(3,1))))
}
for n,(expr,fn)in extras.items():
 put(n,"COMPLEX_MULTISTEP_FRACTIONS_AND_IMAGINARY_POWERS",
     f"Simplify the independent complex expression {expr} to a+bi with exact rational components, using conjugates and the four-power cycle.",
     fmt(fn()),["simplify intermediate powers or products","rationalize every complex denominator","combine real and imaginary results"])
assert sorted(q)==list(range(1,59))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l01-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists() or len(json.loads(native.read_text(encoding='utf-8'))['FirstPartyStudentQuestions'])==58
native.write_text(json.dumps({
"SchemaVersion":1,"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L01",
"OriginalSourceNumberedExercises":58,
"FirstPartyStudentQuestions":[{"Number":n,"SourceSkill":t["SourceOriginalMathematicsSkill"],"Question":t["IndependentFirstPartyStudentTask"],"GradeMode":"MANUAL_TEACHER_REVIEW"}for n,t in sorted(q.items())],
"TeacherOnlyExpectedMath":[{"Number":n,"Expected":t["TeacherExpectedMath"],"Actions":t["StudentCognitiveActions"]}for n,t in sorted(q.items())],
"OriginalPublisherBitmapOrImageReproduced":False,
"OriginalPublisherExactTextGraphicNumericInstanceParityCertified":False,
"StudentTeacherRuntime":"NOT_RUN","StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u03-l01-source58-independent-complex-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],"OriginalNumberedExerciseAnchors":58,
"FirstPartyIndependentSourceComplexSkillQuestions":58,
"PublisherOriginalFiguresCopied":False,
"ExactPublisherVerbatimTextAndImageParityCertified":False,
"StageD":"NOT_RUN","Items":[q[n]for n in sorted(q)]
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u03-l01-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["SourceOriginalNumberedAssessmentReview"]="58_FIRSTPARTY_COMPLEX_SOURCE_SKILL_STUDENT_QUESTIONS_AND_TEACHER_EXACT_FRACTION_MATH_KEYS_AUTHORING_COMPLETE"
c["NativeFirstPartyTeacherAssessmentPath"]=native.relative_to(root).as_posix()
c["OriginalSource58QuestionSkillContractPath"]=manifest.relative_to(root).as_posix()
c["OriginalFigureCrosswalk"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L01_58_ORIGINAL_MATH_COMPLEX_SOURCE_SKILL_QUESTION_CONTRACTS_FIRSTPARTY_EXACT_RATIONAL_MATH_KEYS")
