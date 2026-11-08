from pathlib import Path
import json,datetime,math
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l08-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l08-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"));t=json.loads(tp.read_text(encoding="utf-8"))
assert a["OriginalSourceMathExercisesIndividuallyVerified"]==31 and len(t["FirstPartyIndependentStudentMathQuestions"])==31
data=[
(32,"Sketch f(x)=x²+5 on x≥0, and sketch its inverse reflection across y=x.","Inverse √(x−5) on input x≥5. Corresponding points (0,5)↔(5,0), (2,9)↔(9,2).","Source #32: x²+2, x≥0 has inverse √(x−2); points swap over y=x."),
(33,"Sketch f(x)=9−x² on x≥0 with its inverse.","Inverse √(9−x) on x≤9; points (0,9)↔(9,0),(3,0)↔(0,3).","Source #33: 4−x² on x≥0 inverse √(4−x), input x≤4."),
(34,"Sketch f(x)=(x+4)² on x≥−4 and its inverse.","Inverse −4+√x on x≥0; points (−4,0)↔(0,−4).","Source #34: (x+3)² x≥−3 inverse −3+√x."),
(35,"Sketch f(x)=(x−5)² on x≥5 and its inverse.","Inverse 5+√x on x≥0; points (5,0)↔(0,5).","Source #35: (x−4)² x≥4 inverse 4+√x."),
(36,"Graph f(x)=x³+2 with its inverse.","Inverse ∛(x−2), all real; points (0,2)↔(2,0), (1,3)↔(3,1).","Source #36: x³+3 inverse ∛(x−3), all real."),
(37,"Graph f(x)=5−x³ with its inverse.","Inverse ∛(5−x), all real; points (1,4)↔(4,1), (0,5)↔(5,0).","Source #37: 1−x³ inverse ∛(1−x)."),
(38,"Sketch f(x)=x²+2x on x≥−1 and its inverse.","Complete square (x+1)²−1; inverse −1+√(x+1), input x≥−1.","Source #38: x²+4x x≥−2 inverse −2+√(x+4)."),
(39,"Sketch f(x)=x²−4x+3 on x≥2 and its inverse.","Complete square (x−2)²−1; inverse 2+√(x+1), x≥−1.","Source #39: x²−6x+1 x≥3 inverse 3+√(x+8)."),
(40,"Sketch f(x)=3/x and its inverse for x≠0.","Self-inverse 3/x; both graphs symmetric about y=x, points (1,3)↔(3,1).","Source #40: 2/x is self-inverse on nonzero reals."),
(41,"Sketch f(x)=4/x² on x>0 and its inverse.","Inverse 2/√x for x>0; points (1,4)↔(4,1). Domain original must exclude x=0.","Source #41: 1/x² restricted x>0 inverse 1/√x for x>0."),
(42,"Find domain of √(((x−2)(x+1))/x) by sign chart.","[−1,0) ∪ [2,∞), exclude pole x=0.","Source #42: √(((x+1)(x−1))/x) domain [−1,0) ∪ [1,∞)."),
(43,"Find real domain of √(((x+3)(x−4))/(x−2)).","[−3,2) ∪ [4,∞).","Source #43: √(((x+2)(x−3))/(x−1)) domain [−2,1) ∪ [3,∞)."),
(44,"Find real domain of √((x(x+2))/(x−3)).","[−2,0] ∪ (3,∞).","Source #44: √(x(x+3)/(x−4)) domain [−3,0] ∪ (4,∞)."),
(45,"Find real domain of √(((x−5)(x+3))/(x−1)).","[−3,1) ∪ [5,∞).","Source #45: √((x²−x−20)/(x−2)) domain [−4,2) ∪ [5,∞)."),
(46,"Find real domain of √((16−x²)/(x+5)).","(−∞,−5) ∪ [−4,4].","Source #46: √((9−x²)/(x+4)) domain (−∞,−4) ∪ [−3,3]."),
(47,"For f(x)=x³−x+1, give inverse-relation points whose inverse y-coordinates are 1,2,3.","Points (1,1),(7,2),(25,3); swap (x,f(x)) into (f(x),x).","Source #47: f=x³−x−2, original inputs1,2,3 give inverse points (−2,1),(4,2),(22,3)."),
(48,"For f(x)=x³+x+2, give inverse points with y=0,1,2.","Points (2,0),(4,1),(12,2).","Source #48: x³+x−2 gives (−2,0),(0,1),(8,2)."),
(49,"For f(x)=x³+2x−3, give inverse points with y=0,1,2.","Points (−3,0),(0,1),(9,2).","Source #49: x³+3x−4 gives (−4,0),(0,1),(10,2)."),
(50,"For f(x)=x³+4x−2, give inverse points with y=−1,0,1.","Points (−7,−1),(−2,0),(3,1).","Source #50: x³+8x−4 gives (−13,−1),(−4,0),(5,1)."),
(51,"For the inverse relation of f(x)=x⁴+4x+2, list three swapped points for original x=−1,0,1 and explain why a domain restriction may be necessary.","Inverse relation points (−1,−1),(2,0),(7,1). f is not globally one-to-one, so these swapped points do not establish a global inverse function.","Source #51: x⁴+5x+1 gives inverse-relation points (−3,−1),(1,0),(7,1); global inverse function requires suitable domain restriction."),
(52,"For a>0 invert f(x)=ax³−b.","f⁻¹(x)=∛((x+b)/a), all real.","Source #52: ax³+b inverse ∛((x−b)/a)."),
(53,"For b>0 and x≥−b, invert f(x)=x²+2bx.","f(x)=(x+b)²−b²; inverse −b+√(x+b²), inverse domain x≥−b².","Source #53: x²+bx not one-to-one over all reals. On x≥−b/2 inverse (−b+√(b²+4x))/2; original missing branch restriction."),
(54,"For a,b>0 and original x≥0, invert f(x)=√(2ax²+b).","f⁻¹(x)=√((x²−b)/(2a)), x≥√b.","Source #54: √(ax²+b) not globally one-to-one. With x≥0, inverse √((x²−b)/a), x≥√b."),
(55,"For a>0 invert f(x)=2∛(ax+b).","f⁻¹(x)=((x/2)³−b)/a for all real x.","Source #55: ∛(ax+b) inverse (x³−b)/a."),
(56,"Assume a,b,c>0 and 2b≠ac. Invert f(x)=(ax+2b)/(x+c).","f⁻¹(x)=(2b−cx)/(x−a), x≠a; requires nonconstant original mapping (2b≠ac).","Source #56: (ax+b)/(x+c) inverse (b−cx)/(x−a) provided b≠ac; if b=ac function is constant a and has no inverse function."),
(57,"An object falls from 180 m with h(t)=180−5t² for t≥0. Express t(h) and time at height80 m.","t(h)=√((180−h)/5) seconds; at h=80, t=√20≈4.472 seconds.","Source #57: t(h)=√((200−h)/4.9), at h50 -> √(150/4.9)≈5.53 s."),
(58,"An object falls from 500 ft with h(t)=500−20t². Express t(h) and time at height320 ft.","t(h)=√((500−h)/20); t=√9=3 seconds.","Source #58: h(t)=600−16t², at h400 gives t=√(200/16)≈3.536s."),
(59,"Find radius r for sphere V=(4π/3)r³ with V=36π ft³.","r(V)=∛(3V/(4π)), r=3 feet.","Source #59: r(V)=∛(3V/(4π)), at V200 ft³ r=∛(150/π) ft."),
(60,"Express sphere radius from its surface area A=4πr². Compute radius if A=144π square inches.","r(A)=√(A/(4π)) for r≥0; r=6 inches.","Source #60 incorrectly says r as function V though the expression is surface area A. Correct r(A)=√(A/(4π)); at A1000 in² r=√(250/π) in."),
(61,"A tank has 100 ml with 20 ml acid and receives n ml solution containing 50% acid. Find n(C) and amount for 40% final concentration.","C(n)=(20+0.5n)/(100+n); n(C)=(100C−20)/(0.5−C). At C=0.4, n=200 ml.","Source #61: C(n)=(25+0.6n)/(100+n), inverse n=(100C−25)/(0.6−C); at C0.5 n250 ml."),
(62,"For a pendulum T(l)=2π√(l/36) seconds, invert to find length for T=π seconds.","l(T)=36(T/(2π))²; at T=π, l=9 feet.","Source #62: l(T)=32.2(T/(2π))²; for T=2, l=32.2/π²≈3.26 ft."),
(63,"A cylinder has height5 m and volume V=πr²h. Express r(V), then calculate for V=45π m³.","r(V)=√(V/(5π)); r=3 m.","Source #63: h6 m, r(V)=√(V/(6π)), at V300 m³ radius √(50/π) m."),
(64,"For a closed cylinder height3 ft, surface area A=2πr²+6πr. Find inverse r(A) and radius at A=20π ft².","r(A)=(-3+√(9+2A/π))/2 ft, r≥0; with A20π, radius2 ft.","Source #64 wrongly says inverse r as function V despite A surface area; for h4, r(A)=−2+√(4+A/(2π)), A200 ft²."),
(65,"A cone has height12 inches and volume16π cubic inches. Find radius with consistent units.","r=√(3V/(πh)); r=√(48π/(12π))=2 inches.","Source #65 mixes 12-foot height with 50 cubic inches volume; must convert h to144 inches before using in³. Source prompt also ambiguously says r in terms h, although h is fixed."),
(66,"A cone has height15 ft and volume45π ft³. Find r(V) and radius.","r(V)=√(3V/(15π)), r=3 feet.","Source #66 with h30 ft: r(V)=√(3V/(30π)); at V1000 ft³ radius √(100/π) ft.")
]
assert [n for n,*_ in data]==list(range(32,67))
assert len(data)==35 and len(a["SourceNumberedExerciseLedger"])==66
assert (-1)**4+4*(-1)+2==-1
assert 1+4+2==7
assert math.isclose(math.sqrt((180-80)/5),math.sqrt(20))
assert 500-20*(3**2)==320
assert 4*math.pi*(6**2)==144*math.pi
assert Fraction(20+Fraction(1,2)*200,100+200)==Fraction(2,5)
assert 2*math.pi*4+6*math.pi*2==20*math.pi
assert Fraction(3*16,12)==4 and Fraction(3*45,15)==9
for n,q,ans,original in data:
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,
 "IndependentStudentQuestion":q,"TeacherExpectedMath":ans,"OriginalSourceMathReviewed":original,
 "MarkingMode":"TEACHER_REVIEW_SYMBOLIC_GRAPH_PHYSICAL_UNITS","Runtime":"NOT_RUN"})
 row=a["SourceNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 row.update({"OriginalMathematicalAnswerIndividuallyVerified":True,
 "FirstPartyIndependentMathQuestionVerified":True,"TeacherRubricVerified":True,
 "TeacherKey":ans,"RequiresTeacherDrawing":32<=n<=46})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==66
t["FirstPartyIndependentStudentQuestionsAuthored"]=66
t["SourceMathReviewedCount"]=66
t["SourceOriginalAmbiguitiesDisclosed"]={
 "SourceExercise51":"Global x^4+5x+1 is not one-to-one without restricting domain. Three points belong to inverse relation only.",
 "SourceExercise53":"Original x²+bx needs monotone branch restriction for function inverse.",
 "SourceExercise54":"Original sqrt(ax²+b) needs sign branch restriction.",
 "SourceExercise56":"Original parameters allow b=ac where function is constant; exclude to invert.",
 "SourceExercise60":"Source asks radius as a function of V despite describing sphere area A; inverse uses A.",
 "SourceExercise64":"Source asks radius as a function of V despite describing cylinder area A; inverse uses A.",
 "SourceExercise65":"Source uses feet height with cubic inches volume; convert units before solving."}
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
a["OriginalSourceMathExercisesIndividuallyVerified"]=66
a["FirstPartyIndependentSourceExerciseMathAuthored"]=66
a["SourceOriginalAmbiguitiesLogged"]=list(t["SourceOriginalAmbiguitiesDisclosed"])
a["BlockingReasons"]=["9_SOURCE_FIGURE_MATH_ALTERNATIVES_AND_GRAPH_MODELS_PENDING","STAGE_C_PRACTICE_AND_TEACHER_REVIEW_ROUTES_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L08_ALL_66_SOURCE_EXERCISE_MATH_REVIEWED_INDEPENDENT_TEACHER_KEYS_AUTHORED_STILL_OPEN")
