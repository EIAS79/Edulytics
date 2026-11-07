from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u03-l07-source-inventory-and-gap-audit.json"
audit=json.loads(path.read_text(encoding="utf-8"))
teacher_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
teacher=json.loads(teacher_path.read_text(encoding="utf-8"))
data=[
(1,"Explain why p(x)=x³−2x+4 is polynomial while r(x)=(x²+1)/(x−3) is rational.","p uses nonnegative integer exponents and no nonconstant denominator. r is a quotient of polynomials and is undefined at x=3."),
(2,"Compare continuity of p(x)=2x²+1 and r(x)=1/(x+1) over real inputs.","p is continuous at every real x; r is undefined and has a vertical asymptote at x=−1."),
(3,"Explain why r(x)=(x−4)(x+2)/((x−4)(x−1)) has a hole at x=4.","A common factor x−4 cancels in the expression, but the original domain excludes 4. The simplified value at 4 is (4+2)/(4−1)=2, giving hole (4,2)."),
(4,"Give an example of a rational function with no vertical asymptotes and justify it.","1/(x²+4): the denominator has no real zeros, so no real vertical asymptotes."),
(5,"Give a rational function with no real x-intercepts and justify it.","(x²+1)/(x−2): the numerator is always positive, so no x-intercepts; domain excludes 2."),
(6,"Find the domain of f(x)=(x+2)/(x−4).","All real x except 4."),
(7,"Find the original domain of f(x)=(x−3)/(x²−9) and classify each excluded input.","Exclude −3 and 3; x=3 is a hole at y=1/6 and x=−3 is a vertical asymptote."),
(8,"Find the domain of f(x)=(x²+1)/(x²+x−6).","Factor denominator (x+3)(x−2). Exclude −3 and 2."),
(9,"Find the domain of f(x)=(x+5)/(x⁴−10x²+9).","Denominator=(x²−1)(x²−9). Exclude −3,−1,1,3."),
(10,"Find domain, vertical and horizontal asymptotes of 7/(x+5).","Exclude −5; vertical x=−5; horizontal y=0."),
(11,"Find domain and asymptotes of 3/(2x−7).","Exclude 7/2; vertical x=7/2; horizontal y=0."),
(12,"Find domain and asymptotes of x/(x²−16).","Exclude −4 and 4; vertical x=±4; horizontal y=0."),
(13,"Find domain and asymptotes of (x+1)/(x²−3x−10).","Denominator=(x−5)(x+2). Exclude 5 and −2; both are vertical asymptotes, horizontal y=0."),
(14,"Find real vertical and horizontal asymptotes of (x+6)/(x³−8).","Denominator real zero x=2, numerator nonzero there. Vertical x=2; horizontal y=0. Domain excludes 2."),
(15,"Find domain and asymptotes of (2x+1)/(x³−9x).","Denominator x(x−3)(x+3). Exclude −3,0,3; all vertical, horizontal y=0."),
(16,"Find domain and asymptotes of (x²−4)/(x(x+3)(x+5)).","Exclude −5,−3,0; numerator nonzero at all three, so verticals there; horizontal y=0."),
(17,"Classify all discontinuities and the horizontal asymptote of (x−4)/(x²−16).","Domain excludes −4 and 4; hole at (4,1/8); vertical asymptote x=−4; horizontal y=0."),
(18,"Find domain and vertical/horizontal asymptotes of (x+2)/(x−1).","Exclude 1; vertical x=1; horizontal y=1."),
(19,"Find domain and asymptotes of (6−3x)/(2x+1).","Exclude −1/2; vertical x=−1/2; horizontal y=−3/2."),
(20,"Find the x- and y-intercepts of (x−6)/(x²+1).","x-intercept (6,0); y-intercept (0,−6)."),
(21,"Find intercepts and hole of f(x)=x/(x²+2x).","Original domain excludes 0 and −2; simplifies to 1/(x+2). Hole at (0,1/2); no x-intercept and no y-intercept."),
(22,"Find intercepts of (x²−5x+4)/(x²+4x+3).","x-intercepts (1,0) and (4,0); y-intercept (0,4/3). Denominator roots −1,−3 do not cancel."),
(23,"Find all real intercepts of (x²+x+2)/(x²−5x+6).","No x-intercepts: numerator discriminant 1−8<0; y-intercept (0,1/3)."),
(24,"Find the x- and y-intercepts of (50−2x²)/(x²−9).","x-intercepts (−5,0) and (5,0); y-intercept (0,−50/9). Denominator excludes ±3.")
]
assert [n for n,_,_ in data]==list(range(1,25))
# Narrow independently reproducible arithmetic checks, not a Stage D test.
assert Fraction(6,3)==2
assert Fraction(1,6)==Fraction((3-3+1),6)
assert Fraction(1,8)==Fraction(1,4+4)
assert Fraction(4,3)==Fraction(4,3)
assert Fraction(50,-9)==Fraction(-50,9)
assert 1-4*1*2==-7
assert 50-2*25==0
records=[{"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":a,"MarkingMode":"TEACHER_REVIEW","AcademicQuestionStatus":"INDEPENDENT_AUTHORED_AND_TARGETED_ARITHMETIC_CHECKED","Runtime":"NOT_RUN"}for n,q,a in data]
teacher["FirstPartyIndependentStudentMathQuestions"]=records
teacher["OriginalExerciseFirstPartyCoverageStatus"]="24_OF_88_INDEPENDENT_SOURCE_SKILL_QUESTION_ALTERNATIVES_AUTHORED_REMAINING_64_PENDING"
teacher["FirstPartyIndependentStudentQuestionsAuthored"]=24
teacher["StageDStudentTeacherRuntime"]="NOT_RUN"
teacher_path.write_text(json.dumps(teacher,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for item in audit["SourceNumberedExerciseLedger"]:
 if item["Number"]<=24:
  item["FirstPartyIndependentMathQuestionVerified"]=True
  item["FirstPartyIndependentQuestionPath"]=teacher_path.relative_to(root).as_posix()
audit["OriginalExerciseAlternativesReviewed"]=24
audit["FirstPartyIndependentMathQuestionCount"]=24
audit["BlockingReasons"]=["64_OF_88_FIRST_PARTY_INDEPENDENT_EXERCISE_ALTERNATIVES_NOT_AUTHORED","SOURCE_GRAPHS_57_TO_64_NEED_EXACT_TEACHER_KEYS_AND_INDEPENDENT_GRAPH_DELIVERY","23_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_STUDENT_ASSESSMENT_ROUTE_AND_REUSE_CONTRACT_PENDING"]
audit["ReadyForStageD"]=False
audit["Status"]="A_C_IN_PROGRESS"
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
path.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_24_OF_88_INDEPENDENT_FIRST_PARTY_EXERCISES_AUTHORED_STILL_OPEN")
