from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l07-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"));t=json.loads(tp.read_text(encoding="utf-8"))
assert len(t["FirstPartyIndependentStudentMathQuestions"])==56
data=[
(65,"Create a value table for f(x)=1/(x+1) near x=−1 and far from it.","For x=−1.1,−0.9 values −10,+10; as x→−1− value→−∞, x→−1+ value→+∞; as |x|→∞ value→0."),
(66,"Create a table for g(x)=(x+2)/(x−2) near its vertical and horizontal asymptotes.","g=1+4/(x−2), vertical x=2, horizontal y=1; g(1.9)=−39, g(2.1)=41."),
(67,"Use a near-asymptote table to examine h(x)=3x/(x−1).","h=3+3/(x−1), vertical x=1, horizontal y=3; h(0.9)=−27, h(1.1)=33."),
(68,"Examine the limits of k(x)=3x/(x−2)² around x=2 and at infinity.","As x→2 from either side, k→+∞; as |x|→∞, k→0."),
(69,"Examine the limits of r(x)=x²/(x−2)² near x=2 and at infinity.","Both one-sided outputs tend +∞ near x=2; the horizontal asymptote is y=1."),
(70,"Use signs to solve 3/(x−2)>0.","x>2."),
(71,"Use signs to solve 5/(3x+6)>0.","x>−2."),
(72,"Solve 4/((x+2)(x−3))>0.","x<−2 or x>3."),
(73,"Solve (x−1)/((x+2)(x−4))>0 using sign intervals.","−2<x<1 or x>4."),
(74,"Solve (x−2)²/((x+1)²(x−3))>0.","x>3."),
(75,"Locate the removable discontinuity of (x²−9)/(x−3).","Simplifies to x+3 for x≠3; hole at (3,6)."),
(76,"Locate the removable discontinuity of (x³+8)/(x+2).","Simplifies to x²−2x+4 for x≠−2; hole at (−2,12)."),
(77,"Locate the hole of (x²−2x−8)/(x−4).","Simplifies to x+2 for x≠4; hole at (4,6)."),
(78,"Locate the hole of (3x²+10x+3)/(x+3).","Simplifies to 3x+1 for x≠−3; hole at (−3,−8)."),
(79,"Locate the hole of (x³−3x²)/(x−3).","Simplifies to x² for x≠3; hole at (3,9)."),
(80,"A tank has 120 L of solution containing 6 kg solute. Add 4 L/min containing 2 kg/min solute. Find C(t) and its limiting concentration.","C(t)=(6+2t)/(120+4t) kg/L, t≥0. Limit 1/2 kg/L."),
(81,"A tank initially has 80 gal and 12 lb solute. Add 5 gal/min and 1 lb solute/min. Find C(t) and its long-run level.","C(t)=(12+t)/(80+5t) lb/gal, t≥0. Limit 1/5 lb/gal."),
(82,"For a drug model D(t)=3t/(4+t²), describe its concentration for large t and its peak time for t≥0.","D(t)→0 as t→∞. Derivative proportional to 4−t²; maximum at t=2 hours."),
(83,"For D(t)=60t/(t²+36), find the time of maximal concentration for t≥0.","Derivative proportional to 36−t²; maximum at t=6 hours."),
(84,"An open box with square base holds 64 in³. Find dimensions minimizing material surface area.","h=64/x² and S=x²+256/x. At minimum x³=128; x=∛128 inches and h=x/2 inches."),
(85,"A closed box with square base holds 27 ft³. Base material $0.40/ft², top $0.20/ft², sides $0.10/ft². Find least-cost dimensions.","h=27/x² and cost C=0.6x²+10.8/x dollars. Min x³=9, so x=∛9 ft and height h=3x ft."),
(86,"A closed right circular cylinder has volume 54π in³. Find radius and height minimizing its surface area.","h=54/r²; S=2πr²+108π/r. Minimum radius r=3 inches and height h=6 inches."),
(87,"An open-top cylinder has volume 32π m³. Find radius and height minimizing surface area.","h=32/r²; S=πr²+64π/r. Min r=∛32 m and h=r."),
(88,"A closed cylinder has volume 16π in³. Top and bottom material cost 3 cents/in², side material 1 cent/in². Find least-cost radius and height.","h=16/r²; cost C=6πr²+32π/r cents. Min r³=8/3, r=∛(8/3) inches, h=6r inches.")
]
assert [n for n,_,_ in data]==list(range(65,89))
# Narrow explicit arithmetic checks for table evaluations and unit models.
assert Fraction(19,10)/Fraction(-1,10)==-19  # example value does not enter answer tables
assert Fraction(39,10)/Fraction(-1,10)==-39
assert Fraction(41,10)/Fraction(1,10)==41
assert Fraction(27,10)/Fraction(-1,10)==-27
assert Fraction(33,10)/Fraction(1,10)==33
assert Fraction(6,120)==Fraction(1,20)
assert Fraction(12,80)==Fraction(3,20)
assert 3**3==27 and 54//(3**2)==6
t["FirstPartyIndependentStudentMathQuestions"].extend({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":ans,"MarkingMode":"TEACHER_REVIEW","AcademicQuestionStatus":"INDEPENDENT_AUTHORED_TARGETED_MATH_REVIEW","Runtime":"NOT_RUN"}for n,q,ans in data)
assert len(t["FirstPartyIndependentStudentMathQuestions"])==80
t["FirstPartyIndependentStudentQuestionsAuthored"]=80
t["OriginalExerciseFirstPartyCoverageStatus"]="80_OF_88_INDEPENDENT_SOURCE_SKILL_ALTERNATIVES_AUTHORED_GRAPH_57_TO_64_PENDING"
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for item in a["SourceNumberedExerciseLedger"]:
 if 65<=item["Number"]<=88:
  item["FirstPartyIndependentMathQuestionVerified"]=True
  item["FirstPartyIndependentQuestionPath"]=tp.relative_to(root).as_posix()
a["OriginalExerciseAlternativesReviewed"]=80
a["FirstPartyIndependentMathQuestionCount"]=80
a["BlockingReasons"]=["EIGHT_INDEPENDENT_GRAPH_FROM_SOURCE_SKILL_TASKS_57_TO_64_NOT_AUTHORED","23_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_STUDENT_ASSESSMENT_ROUTE_AND_REUSE_CONTRACT_PENDING"]
a["ReadyForStageD"]=False
a["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_80_OF_88_INDEPENDENT_TEACHER_TASKS_AUTHORED")
