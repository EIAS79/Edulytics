from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
audit_path=base/"u03-l07-source-inventory-and-gap-audit.json"
assessment_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
audit=json.loads(audit_path.read_text(encoding="utf-8"));assessment=json.loads(assessment_path.read_text(encoding="utf-8"))
assert len(assessment["FirstPartyIndependentStudentMathQuestions"])==24
data=[
(25,"Determine local one-sided behavior and long-run level of x/(3x+2).","Vertical x=−2/3; horizontal y=1/3; as x→−2/3− function→+∞ and x→−2/3+ function→−∞."),
(26,"Analyze local and end behavior of 3x/(x−4).","Vertical x=4; horizontal y=3; left side tends −∞ and right side +∞."),
(27,"Analyze local and end behavior of −3x/(x−4).","Vertical x=4; horizontal y=−3; left side tends +∞ and right side −∞."),
(28,"Analyze the local and end behavior of (x²−1)/(x²−9).","Vertical x=±3; horizontal y=1. At −3: left +∞, right −∞. At 3: left −∞, right +∞."),
(29,"Find the vertical and horizontal asymptotes and local signs of (x²−16)/(x²−1).","Vertical x=±1; horizontal y=1. At −1: left −∞, right +∞. At 1: left +∞, right −∞."),
(30,"Find the slant asymptote of (6x²+5x)/(2x+1).","Divide to get 3x+1−1/(2x+1). Slant y=3x+1."),
(31,"Find the slant asymptote of (8x²−3)/(2x−1).","Divide to get 4x+2−1/(2x−1). Slant y=4x+2."),
(32,"Find the slant asymptote of (12x²+8x+1)/(3x+2).","Divide to get 4x+1/(3x+2). Slant y=4x."),
(33,"Find end-behavior slant asymptote of (4x³+x)/(2x²+1).","Divide to get 2x−x/(2x²+1). Slant y=2x."),
(34,"Find the slant asymptote of (x²+3x+1)/(x−2).","Divide to get x+5+11/(x−2). Slant y=x+5."),
(35,"Graph a reciprocal shifted three units down; state both asymptotes.","f(x)=1/x−3; vertical x=0, horizontal y=−3. Use two reciprocal-shaped branches."),
(36,"Graph a reciprocal shifted left two and up one; identify both asymptotes.","f(x)=1/(x+2)+1; vertical x=−2, horizontal y=1."),
(37,"Graph the reciprocal squared shifted three units left; identify asymptotes.","f(x)=1/(x+3)²; vertical x=−3, horizontal y=0. Both branches positive."),
(38,"Graph reciprocal squared shifted right two and up three; identify asymptotes.","f(x)=1/(x−2)²+3; vertical x=2, horizontal y=3. Both branches above y=3."),
(39,"Sketch f=(3x−2)/(x+1) with all intercepts and asymptotes.","x-intercept (2/3,0); y-intercept (0,−2); vertical x=−1; horizontal y=3."),
(40,"Sketch f=(x+4)/(2x−1) with all intercepts and asymptotes.","x-intercept (−4,0); y-intercept (0,−4); vertical x=1/2; horizontal y=1/2."),
(41,"Sketch f=5/(x−1)² with intercepts and asymptotes.","No x-intercept; y-intercept (0,5); vertical x=1; horizontal y=0. Positive on each side."),
(42,"Sketch f=3/(x+2)² with intercepts and asymptotes.","No x-intercept; y-intercept (0,3/4); vertical x=−2; horizontal y=0."),
(43,"Sketch f=(x²−4)/(x²−9) and label key features.","x-intercepts (±2,0); y-intercept (0,4/9); vertical x=±3; horizontal y=1."),
(44,"Sketch g=(2x²−5x−3)/(x²−1) and label key features.","x-intercepts (−1/2,0),(3,0); y-intercept (0,3); vertical x=±1; horizontal y=2."),
(45,"Identify the hole, intercepts and asymptotes of (x²+3x+2)/(x²−1).","After cancelling x+1, hole (−1,−1/2); vertical x=1; x-intercept −2; y-intercept −2; horizontal y=1."),
(46,"Identify the hole, intercepts and asymptotes of (x²−5x+6)/(x²−4).","After cancelling x−2, hole (2,−1/4); vertical x=−2; x-intercept 3; y-intercept −3/2; horizontal y=1."),
(47,"Sketch (x²+4x+3)/(x−2), recording intercepts and end behavior.","x-intercepts −3,−1; y-intercept −3/2; vertical x=2; slant y=x+6."),
(48,"Sketch (2x²+x−3)/(x−3), recording intercepts and end behavior.","x-intercepts −3/2,1; y-intercept 1; vertical x=3; slant y=2x+7."),
(49,"For f=(x−2)(x+1)(x−4)/((x+3)²(x−5)), sketch from factored features.","Zeros 2,−1,4; y-intercept −8/45; vertical x=−3 (double),5; horizontal y=1."),
(50,"For g=(x+3)²(x−2)/((x−4)(x+2)(x+5)), sketch from factored features.","Zeros −3 (double),2; y-intercept 9/20; vertical x=4,−2,−5; horizontal y=1."),
(51,"Construct a rational function with verticals ±2, x-intercepts −1 and 3, y-intercept 3/4.","f=(x+1)(x−3)/((x−2)(x+2)); f(0)=3/4."),
(52,"Construct a rational function with verticals −2 and −5, x-intercepts 1 and 4, y-intercept 2/5.","f=(x−1)(x−4)/((x+2)(x+5)); f(0)=2/5."),
(53,"Construct a rational function with verticals −2 and −6, zeros 3 and −1, horizontal y=5.","f=5(x−3)(x+1)/((x+2)(x+6))."),
(54,"Construct a rational function with verticals −1 and 4, zeros −2 and 2, horizontal y=−3.","f=−3(x+2)(x−2)/((x+1)(x−4))."),
(55,"Construct a rational function with vertical x=−2, double zero x=3, y-intercept 9/2.","f=(x−3)²/(x+2); at x=0 value 9/2."),
(56,"Construct a rational function with vertical x=4, double zero x=−1, y-intercept −1/4.","f=(x+1)²/(x−4); at x=0 value −1/4.")
]
assert [n for n,_,_ in data]==list(range(25,57))
assert Fraction(-8,45)==Fraction((-2)*1*(-4),(3**2)*(-5))
assert Fraction(9,20)==Fraction((3**2)*(-2),(-4)*2*5)
assert Fraction(3,4)==Fraction((1)*(-3),(-2)*(2))
assert Fraction(2,5)==Fraction((-1)*(-4),2*5)
assert Fraction(-1,4)==Fraction((2-3),2+2)
assert Fraction(-1,2)==Fraction((-1+2),(-1-1))
assert Fraction(-3,2)==Fraction(3,-2)
assessment["FirstPartyIndependentStudentMathQuestions"].extend({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":ans,"MarkingMode":"TEACHER_REVIEW","AcademicQuestionStatus":"INDEPENDENT_AUTHORED_TARGETED_MATH_REVIEW","Runtime":"NOT_RUN"}for n,q,ans in data)
assert len(assessment["FirstPartyIndependentStudentMathQuestions"])==56
assessment["FirstPartyIndependentStudentQuestionsAuthored"]=56
assessment["OriginalExerciseFirstPartyCoverageStatus"]="56_OF_88_INDEPENDENT_SOURCE_SKILL_ALTERNATIVES_AUTHORED_32_PENDING"
assessment_path.write_text(json.dumps(assessment,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for row in audit["SourceNumberedExerciseLedger"]:
 if 25<=row["Number"]<=56:
  row["FirstPartyIndependentMathQuestionVerified"]=True
  row["FirstPartyIndependentQuestionPath"]=assessment_path.relative_to(root).as_posix()
audit["OriginalExerciseAlternativesReviewed"]=56
audit["FirstPartyIndependentMathQuestionCount"]=56
audit["BlockingReasons"]=["32_OF_88_FIRST_PARTY_EXERCISE_ALTERNATIVES_NOT_AUTHORED","GRAPHS_57_TO_64_REQUIRE_TEACHER_MATH_AND_INDEPENDENT_GRAPH_MODELS","23_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_STUDENT_ASSESSMENT_ROUTE_AND_REUSE_CONTRACT_PENDING"]
audit["ReadyForStageD"]=False
audit["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit_path.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_56_OF_88_INDEPENDENT_TASKS_TEACHER_GUIDES_AUTHORED_NOT_CLOSED")
