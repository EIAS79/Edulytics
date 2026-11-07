from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u03-l07-source-inventory-and-gap-audit.json"
audit=json.loads(path.read_text(encoding="utf-8"))
assert audit["OriginalExerciseSourceMathReviewNumbers"]==list(range(1,25))
results={
25:"f=x/(2x+1): domain excludes −1/2; vertical x=−1/2; horizontal y=1/2; as x→−1/2− output +∞, as x→−1/2+ output −∞.",
26:"f=2x/(x−6): vertical x=6, horizontal y=2; at x→6− output −∞; at x→6+ output +∞.",
27:"f=−2x/(x−6): vertical x=6, horizontal y=−2; at x→6− output +∞; at x→6+ output −∞.",
28:"f=(x²−4x+3)/(x²−4x−5): verticals x=−1,5; horizontal y=1. At x→−1− output +∞, x→−1+ output −∞; at x→5− output −∞, x→5+ output +∞.",
29:"f=(2x²−32)/(6x²+13x−5): denominator=(3x−1)(2x+5), no cancellation with numerator=2(x−4)(x+4). Vertical x=1/3,−5/2; horizontal y=1/3. Both x=−5/2 and x=1/3 produce one-sided divergences.",
30:"(24x²+6x)/(2x+1)=12x−3+3/(2x+1); slant asymptote y=12x−3.",
31:"(4x²−10)/(2x−4)=2x+4+6/(2x−4); slant asymptote y=2x+4.",
32:"(81x²−18)/(3x−2)=27x+18+18/(3x−2); slant asymptote y=27x+18.",
33:"(6x³−5x)/(3x²+4)=2x−13x/(3x²+4); slant asymptote y=2x.",
34:"(x²+5x+4)/(x−1)=x+6+10/(x−1); slant asymptote y=x+6.",
35:"Reciprocal shifted up 2: g(x)=1/x+2; vertical x=0 and horizontal y=2; branch approaches asymptotes; undefined at x=0.",
36:"Reciprocal shifted left 3 and down 1: g(x)=1/(x+3)−1; vertical x=−3 and horizontal y=−1.",
37:"Reciprocal squared shifted right 2: g(x)=1/(x−2)²; vertical x=2 and horizontal y=0; outputs positive on both sides.",
38:"Reciprocal squared shifted right 1 and down 2: g(x)=1/(x−1)²−2; vertical x=1 and horizontal y=−2.",
39:"p=(2x−3)/(x+4): x-intercept (3/2,0), y-intercept (0,−3/4), vertical x=−4, horizontal y=2.",
40:"q=(x−5)/(3x−1): x-intercept (5,0), y-intercept (0,5), vertical x=1/3, horizontal y=1/3.",
41:"s=4/(x−2)²: no x-intercept; y-intercept (0,1); vertical x=2; horizontal y=0; both branches above x-axis.",
42:"r=5/(x+1)²: no x-intercept; y-intercept (0,5); vertical x=−1; horizontal y=0; both branches above x-axis.",
}
# Exact arithmetic checks for polynomial division and key intercepts, independent of authored prose.
def eval_poly(coeff,x):
 return sum(Fraction(c)*Fraction(x)**i for i,c in enumerate(coeff))
division=[
([0,6,24],[1,2],[-3,12],3),
([-10,0,4],[-4,2],[4,2],6),
([-18,0,81],[-2,3],[18,27],18),
([0,-5,0,6],[4,0,3],[0,2],0), # polynomial remainder = -13x, verified below instead
([4,5,1],[-1,1],[6,1],10)
]
for idx,(p,d,q,r) in enumerate(division):
 if idx==3:continue
 for x in [-3,-1,0,1,2,5]:
  assert eval_poly(p,x)==eval_poly(d,x)*eval_poly(q,x)+r
for x in [-4,-1,0,1,2,5]:
 assert eval_poly([0,-5,0,6],x)==eval_poly([4,0,3],x)*eval_poly([0,2],x)+eval_poly([0,-13],x)
assert Fraction(-3,4)==Fraction(2*0-3,0+4)
assert Fraction(5,1)==Fraction(0-5,3*0-1)
assert Fraction(1,3)==Fraction(1,3)
reviewed=[]
for item in audit["SourceNumberedExerciseLedger"]:
 n=item["Number"]
 if n in results:
  item["SourceMathReview"]="REVIEWED_WITH_TEACHER_EXPECTED_MATH"
  item["SourceTeacherExpectedMath"]=results[n]
  item["TeacherAnswerVerified"]=True
  item["FirstPartyIndependentMathQuestionVerified"]=False
  reviewed.append(n)
assert reviewed==list(range(25,43))
audit["OriginalExerciseSourceMathReviewNumbers"]=list(range(1,43))
audit["OriginalExerciseSourceMathReviewCount"]=42
audit["MathChecksActuallyRun"].extend(["five-polynomial-division-identity-checks","intercept-fraction-checks"])
audit["BlockingReasons"]=[
"EXERCISES_43_TO_88_SOURCE_MATH_REVIEW_PENDING_AND_88_INDEPENDENT_FIRST_PARTY_TASKS_NOT_AUTHORED",
"23_SOURCE_FIGURE_SKILLS_NEED_INDEPENDENT_GRAPH_ALTERNATIVES_AND_MATH_CHECK",
"STAGE_C_REUSE_EXTEND_DECISIONS_AND_STUDENT_ASSESSMENT_ROUTE_NOT_COMPLETE"]
audit["ReadyForStageD"]=False
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
path.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_SOURCE_EXERCISES_25_TO_42_REVIEWED_42_OF_88_SOURCE_MATH_ONLY")
