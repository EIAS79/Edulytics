from pathlib import Path
import json,datetime,math
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u03-l07-source-inventory-and-gap-audit.json"
audit=json.loads(path.read_text(encoding="utf-8"))
assert audit["OriginalExerciseSourceMathReviewNumbers"]==list(range(1,43))
answers={
43:"f=(3x²−14x−5)/(3x²+8x−16)=((3x+1)(x−5))/((3x−4)(x+4)); x-intercepts −1/3,5; y-intercept 5/16; vertical x=4/3,−4; horizontal y=1.",
45:"a=(x²+2x−3)/(x²−1)=((x+3)(x−1))/((x−1)(x+1)); hole at (1,2); vertical x=−1; x-intercept −3; y-intercept 3; horizontal y=1.",
46:"b=(x²−x−6)/(x²−4)=((x−3)(x+2))/((x−2)(x+2)); hole (−2,5/4); vertical x=2; x-intercept 3; y-intercept 3/2; horizontal y=1.",
47:"h=(2x²+x−1)/(x−4): zeros −1,1/2, y-intercept 1/4, vertical x=4, slant y=2x+9 (remainder35).",
48:"k=(2x²−3x−20)/(x−5): zeros −5/2,4, y-intercept 4, vertical x=5, slant y=2x+7 (remainder15).",
49:"w=(x−1)(x+3)(x−5)/((x+2)²(x−4)): x-intercepts 1,−3,5; y-intercept −15/16; vertical x=−2 (order2),4; horizontal y=1.",
50:"z=(x+2)²(x−5)/((x−3)(x+1)(x+4)): x-intercepts −2 double,5; y-intercept 5/3; vertical x=3,−1,−4; horizontal y=1.",
51:"Independent construction satisfying original prescribed features: f(x)=50(x−2)(x+1)/((x−5)(x+5)); f(0)=4.",
52:"Independent construction satisfying original prescribed features: f(x)=(28/5)(x−1)(x−5)/((x+4)(x+1)); f(0)=7.",
53:"With verticals −4,−5, zeros 4,−6 and horizontal y=7: f(x)=7(x−4)(x+6)/((x+4)(x+5)).",
54:"With verticals −3,6, zeros −2,1 and horizontal y=−2: f(x)=−2(x+2)(x−1)/((x+3)(x−6)).",
55:"With vertical x=−1, double zero x=2 and y-intercept 2: f(x)=(x−2)²/(2(x+1)).",
56:"With vertical x=3, double zero x=1 and y-intercept 4: f(x)=−12(x−1)²/(x−3).",
65:"f=1/(x−2); near x=2 from left output −∞ and from right +∞; end behavior tends 0.",
66:"f=x/(x−3)=1+3/(x−3); near x=3 left −∞, right +∞; tends 1 at both ends.",
67:"f=2x/(x+4)=2−8/(x+4); near x=−4 from left +∞, right −∞; tends 2 at both ends.",
68:"f=2x/(x−3)²; near x=3 from either side +∞; as |x|→∞, f→0 with sign of x.",
69:"f=x²/(x+1)²; near x=−1 from both sides +∞; as |x|→∞, f→1.",
70:"2/(x+1)>0 exactly when x>−1.",
71:"4/(2x−3)>0 exactly when x>3/2.",
72:"2/((x−1)(x+2))>0 on (−∞,−2) ∪ (1,∞).",
73:"(x+2)/((x−1)(x−4))>0 on (−2,1) ∪ (4,∞).",
74:"(x+3)²/((x−1)²(x+1))>0 on (−1,1) ∪ (1,∞).",
75:"(x²−4)/(x−2)=(x+2) for x≠2; removable hole (2,4).",
76:"(x³+1)/(x+1)=x²−x+1 for x≠−1; removable hole (−1,3).",
77:"(x²+x−6)/(x−2)=x+3 for x≠2; removable hole (2,5).",
78:"(2x²+5x−3)/(x+3)=2x−1 for x≠−3; removable hole (−3,−7).",
79:"(x³+x²)/(x+1)=x² for x≠−1; removable hole (−1,1).",
80:"Tank concentration C(t)=(10+3t)/(200+10t) pounds/gallon for t≥0; asymptotic level 3/10 pounds/gallon.",
81:"Tank concentration C(t)=(8+2t)/(300+20t) pounds/gallon for t≥0; asymptotic level 1/10 pounds/gallon.",
82:"C(t)=2t/(3+t²) tends to 0 as t→∞, even though it initially increases. For t≥0 peak occurs at t=√3 h.",
83:"C(t)=100t/(2t²+75) peaks where 75−2t²=0: t=√(75/2)≈6.124 hours.",
84:"V=x²h=108 in³ gives h=108/x²; surface area S=x²+432/x. Minimum when x³=216: base 6 by 6 inches, height 3 inches.",
85:"V=x²h=20 ft³; base/top/sides total cost C=0.5x²+8/x dollars. Minimum at x³=8: 2 by 2 ft base, height 5 ft.",
86:"V=πr²h=100 in³, surface area 2πr²+200/r. Minimum r=(50/π)^(1/3) inches, height h=2r.",
87:"Open top V=πr²h=50 m³, surface area πr²+100/r. Minimum radius r=(50/π)^(1/3) meters, height h=r.",
88:"V=πr²h=40 in³, material cost in cents C=8πr²+80/r. Minimum r=(5/π)^(1/3) inches, height h=8r."
}
# Preserve source transcription ambiguity instead of silently inventing denominator term.
question44="Source OCR renders g denominator as '3x² − 14 + 15', apparently missing an x term; exact denominator and source function must be verified visually from original page before teacher key."
# Targeted exact arithmetic proof checks.
def f(c,x):return sum(Fraction(a)*Fraction(x)**i for i,a in enumerate(c))
assert f([-5,-14,3],5)==0 and f([-5,-14,3],Fraction(-1,3))==0
assert f([-16,8,3],Fraction(4,3))==0 and f([-16,8,3],-4)==0
assert f([-3,2,1],1)==0 and f([-1,0,1],1)==0
assert f([-6,-1,1],-2)==0 and f([-4,0,1],-2)==0
assert f([-1,1,2],Fraction(1,2))==0 and f([-1,1,2],-1)==0
assert f([-20,-3,2],4)==0 and f([-20,-3,2],Fraction(-5,2))==0
assert Fraction(50*(-2)*1,(-5)*5)==4
assert Fraction(28,5)*Fraction((-1)*(-5),4*1)==7
assert Fraction((-12)*(-1)**2,(-3))==4
assert Fraction(94,-12)==Fraction(-47,6)
for n in (75,76,77,78,79): assert "hole" in answers[n]
assert len(answers)==37
changed=[]
for item in audit["SourceNumberedExerciseLedger"]:
 n=item["Number"]
 if n in answers:
  item["SourceMathReview"]="REVIEWED_WITH_TEACHER_EXPECTED_MATH"
  item["SourceTeacherExpectedMath"]=answers[n]
  item["TeacherAnswerVerified"]=True
  item["FirstPartyIndependentMathQuestionVerified"]=False
  changed.append(n)
 elif n==44:
  item["SourceMathReview"]="PENDING_ORIGINAL_FIGURE_OR_EQUATION_VISUAL_CONFIRMATION"
  item["TeacherAnswerVerified"]=False
  item["SourceTranscriptionRisk"]=question44
assert changed==[43]+list(range(45,57))+list(range(65,89))
audit["OriginalExerciseSourceMathReviewNumbers"]=list(range(1,44))+list(range(45,57))+list(range(65,89))
audit["OriginalExerciseSourceMathReviewCount"]=len(audit["OriginalExerciseSourceMathReviewNumbers"])
assert audit["OriginalExerciseSourceMathReviewCount"]==79
audit["SourceExercise44TranscriptionBlocker"]=question44
audit["SourceGraphDependentExerciseNumbersStillUnreviewed"]=list(range(57,65))
audit["OriginalExerciseAlternativesReviewed"]=0
audit["OriginalFigureAlternativesReviewed"]=0
audit["MathChecksActuallyRun"].extend(["targeted-rational-factor-roots-and-design-intercepts","optimization-symbolic-derivative-steps-reviewed-manually"])
audit["BlockingReasons"]=["SOURCE_EXERCISE44_EXACT_DENOMINATOR_VISUAL_VERIFICATION_PENDING","ORIGINAL_EXERCISE_GRAPHS_57_TO_64_NEED_VISUAL_REVIEW","ALL_88_FIRST_PARTY_EXERCISE_ALTERNATIVES_AND_TEACHER_RUBRICS_NOT_AUTHORED","23_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_STUDENT_ASSESSMENT_ROUTE_AND_REUSE_CONTRACT_PENDING"]
audit["Status"]="A_C_IN_PROGRESS"
audit["ReadyForStageD"]=False
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
path.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_79_OF_88_SOURCE_MATH_REVIEWED_44_AND_57_TO_64_OPEN_NOT_CLOSED")
