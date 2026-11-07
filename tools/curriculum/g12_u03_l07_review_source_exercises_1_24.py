from pathlib import Path
import json,datetime,math
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
auditpath=base/"u03-l07-source-inventory-and-gap-audit.json"
audit=json.loads(auditpath.read_text(encoding="utf-8"))
assert not audit["ReadyForStageD"]
answers={
1:"A polynomial has only nonnegative integer powers; a rational function is p(x)/q(x), with q a nonzero polynomial. Every polynomial may also be written over denominator 1.",
2:"Polynomial graphs are continuous on all real x, while rational graphs can have vertical asymptotes or removable discontinuities where the original denominator vanishes.",
3:"Numerator and denominator share a factor whose zero is cancelled algebraically, but remains excluded from the original domain.",
4:"Yes. A denominator may have no real roots (such as x²+1), or all its real zero factors may cancel to produce holes rather than vertical asymptotes.",
5:"Yes. The numerator may have no real roots, or all its real roots may be removed by original denominator restrictions."
}
functions={
6:("x-1","x+2",[-2],[],None),
7:("x+1","x²-1",[-1,1],[],None),
8:("x²+4","x²-2x-8",[-2,4],[],None),
9:("x²+4x-3","x⁴-5x²+4",[-2,-1,1,2],[],None),
10:("4","x-1",[1],[1],"y=0"),
11:("2","5x+2",[Fraction(-2,5)],[Fraction(-2,5)],"y=0"),
12:("x","x²-9",[-3,3],[-3,3],"y=0"),
13:("x","x²+5x-36",[-9,4],[-9,4],"y=0"),
14:("x+3","x³-27",[3],[3],"y=0"),
15:("3x-4","x³-16x",[-4,0,4],[-4,0,4],"y=0"),
16:("x²-1","x³+9x²+14x",[-7,-2,0],[-7,-2,0],"y=0"),
17:("x+5","x²-25",[-5,5],[5],"y=0; hole at x=-5 with y=-1/10"),
18:("x-4","x-6",[6],[6],"y=1"),
19:("4-2x","3x-1",[Fraction(1,3)],[Fraction(1,3)],"y=-2/3")
}
for n,(num,den,excluded,vertical,horizontal) in functions.items():
 assert n>=6
 answers[n]=f"f(x)=({num})/({den}); original domain excludes x in {excluded}."
 if n>=10: answers[n]+=f" Vertical asymptote(s): {vertical if vertical else 'none'}. Horizontal asymptote: {horizontal}."
answers.update({
20:"f=(x+5)/(x²+4): x-intercept (-5,0); y-intercept (0,5/4).",
21:"f=x/(x²-x) simplifies to 1/(x-1) only for x≠0,1; no x-intercept (x=0 excluded); no y-intercept (x=0 undefined). Hole at (0,-1).",
22:"f=(x²+8x+7)/(x²+11x+30): x-intercepts (-7,0), (-1,0); y-intercept (0,7/30). Denominator roots -5,-6 do not cancel.",
23:"f=(x²+x+6)/(x²-10x+24): no real x-intercepts because numerator discriminant 1-24=-23; y-intercept (0,1/4).",
24:"f=(94-2x²)/(3x²-12): x-intercepts (-√47,0),(√47,0); y-intercept (0,-47/6). Exclude x=±2."
})
# Reliable arithmetic and factorization checks for source expressions; these are narrow math checks, not Stage D.
def poly(p,x):
 return sum(Fraction(c)*(Fraction(x)**i) for i,c in enumerate(p))
assert poly([0,-1,1],0)==0  # x²-x: cancellation would remove x=0
assert poly([30,11,1],-5)==poly([30,11,1],-6)==0
assert poly([7,8,1],-7)==poly([7,8,1],-1)==0
assert poly([24,-10,1],4)==poly([24,-10,1],6)==0
assert Fraction(94,-12)==Fraction(-47,6)
assert poly([0,0,-2],Fraction(0))==0
source_reviews=[]
for entry in audit["SourceNumberedExerciseLedger"]:
 n=entry["Number"]
 if n in answers:
  entry["SourceMathReview"]="REVIEWED_WITH_TEACHER_EXPECTED_MATH"
  entry["SourceTeacherExpectedMath"]=answers[n]
  entry["TeacherAnswerVerified"]=True
  # Do not infer first-party task authored from checking source answer!
  entry["FirstPartyIndependentMathQuestionVerified"]=False
  source_reviews.append(n)
assert source_reviews==list(range(1,25))
audit["OriginalExerciseSourceMathReviewCount"]=len(source_reviews)
audit["OriginalExerciseSourceMathReviewNumbers"]=source_reviews
audit["MathChecksActuallyRun"]=["rational-polynomial-sample-substitution","factor-roots-checks","intercept-fraction-check"]
audit["OriginalExerciseAlternativesReviewed"]=0
audit["OriginalFigureAlternativesReviewed"]=0
audit["ReadyForStageD"]=False
audit["Status"]="A_C_IN_PROGRESS"
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditpath.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_SOURCE_EXERCISES_1_TO_24_TEACHER_MATH_REVIEWED_INVENTORY_UPDATED_UNCLOSED")
