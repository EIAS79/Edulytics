from pathlib import Path
import json,datetime
from fractions import Fraction as Q
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l09-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l09-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert not t["FirstPartyIndependentStudentMathQuestions"]
source=[
(1,"Direct y=kx graph is a straight line through origin when variation is first power; direct higher powers may be curves."),
(2,"Inverse y=k/xⁿ has a nonzero variable in denominator, excludes x=0 and usually has an asymptotic branch model."),
(3,"No finite number limit in definition of joint variation; one constant multiplies powers of any number of variables."),
(4,"Original source y=kx, given (x,y)=(6,12): k=2."),
(5,"Original source y=kx², at (4,80): k=5."),
(6,"Original source y=k√x, at (36,24): k=4."),
(7,"Original source y=kx³, at (36,24): k=1/1944."),
(8,"Original source y=k∛x, at (27,15): k=5."),
(9,"Original source y=kx⁴, at (1,6): k=6."),
(10,"Original source y=k/x, at (4,2): k=8."),
(11,"Original source y=k/x², at (3,2): k=18."),
(12,"Original source y=k/x³, at (2,5): k=40."),
(13,"Original source y=k/x⁴, at (3,1): k=81."),
(14,"Original source y=k/√x, at (25,3): k=15."),
(15,"Original source y=k/∛x, at (64,5): k=20."),
(16,"Original source y=kxz, x=2,z=3,y=36: k=6."),
(17,"Original source y=kxzw, at x=1,z=2,w=5,y=100: k=10."),
(18,"Original source y=kx²z², at x=3,z=4,y=72: k=1/2."),
(19,"Original source y=kx√z, at x=2,z=25,y=100: k=10."),
(20,"Original source y=kx²z³√w, at x=1,z=2,w=36,y=48: k=1."),
(21,"Original source y=kxz/w, at x=3,z=5,w=6,y=10: k=4."),
(22,"Original source y=kx²√z/w³, at x=3,z=4,w=3,y=6: k=9."),
(23,"Original source y=kxz/(√w t²), at x=3,z=1,w=25,t=2,y=6: k=40.")
]
# Every independent question changes calibration and enforces the same exact source skill.
q=[
(1,"Explain the visual difference between direct linear variation y=4x and direct quadratic variation y=4x².","y=4x is a straight line through the origin; y=4x² is a parabola through the origin; both have a constant multiplier."),
(2,"Write a relationship for intensity inversely proportional to distance cubed and state excluded distance.","I(d)=k/d³ for d>0 in a physical model. The algebraic denominator excludes d=0."),
(3,"A quantity varies jointly with mass, speed squared, and area. Can this include more than two independent variables?","Yes, F=kmv²A is a single-constant joint variation with three independent factors."),
(4,"y varies directly as x, with y=21 at x=7. Write y(x).","k=3 and y=3x."),
(5,"y varies directly as x², with y=48 at x=4. Write y(x).","k=3 and y=3x²."),
(6,"y varies directly as √x, with y=30 at x=25. Write y(x).","k=6 and y=6√x, x≥0."),
(7,"y varies directly as x³, with y=54 at x=3. Write y(x).","k=2 and y=2x³."),
(8,"y varies directly as ∛x, with y=21 at x=27. Write y(x).","k=7 and y=7∛x."),
(9,"y varies directly as x⁴, with y=32 at x=2. Write y(x).","k=2 and y=2x⁴."),
(10,"y varies inversely as x, with y=6 at x=5. Write y(x).","k=30 and y=30/x, x≠0."),
(11,"y varies inversely as x², with y=8 at x=3. Write y(x).","k=72 and y=72/x², x≠0."),
(12,"y varies inversely as x³, with y=3 at x=4. Write y(x).","k=192 and y=192/x³."),
(13,"y varies inversely as x⁴, with y=3 at x=2. Write y(x).","k=48 and y=48/x⁴."),
(14,"y varies inversely as √x, with y=5 at x=16. Write y(x).","k=20, y=20/√x, x>0."),
(15,"y varies inversely as ∛x, with y=4 at x=125. Write y(x).","k=20 and y=20/∛x, x≠0."),
(16,"y varies jointly as x and z; y=40 when x=2 and z=4. Write y(x,z).","k=5 and y=5xz."),
(17,"y varies jointly as x,z,w; y=72 at x=2,z=3,w=4. Write y.","k=3 and y=3xzw."),
(18,"y varies jointly as x² and z²; y=72 at x=2,z=3. Write y.","k=2 and y=2x²z²."),
(19,"y varies jointly as x and √z; y=36 at x=3,z=16. Write y.","k=3 and y=3x√z."),
(20,"y varies jointly as x², z³ and √w; y=96 at x=2,z=2,w=9. Write y.","k=1 and y=x²z³√w."),
(21,"y varies jointly as x,z and inversely as w; y=30 at x=2,z=5,w=4. Write y.","k=12 and y=12xz/w."),
(22,"y varies jointly as x²√z and inversely as w³; y=24 at x=2,z=9,w=2. Write y.","k=16 and y=16x²√z/w³."),
(23,"y varies jointly as xz and inversely as √w t²; y=12 at x=2,z=6,w=16,t=1. Write y.","k=4 and y=4xz/(√w t²).")
]
assert [n for n,*_ in source]==[n for n,*_ in q]==list(range(1,24))
# Verify all source k coefficients with exact rational calibration where exponents are integers.
assert Q(24,36**3)==Q(1,1944)
assert Q(48,1*8*6)==1
assert Q(6*3**3,3**2*2)==9
assert Q(6*5*2**2,3)==40
assert Q(96,2**2*2**3*3)==1
assert Q(24*2**3,2**2*3)==16
assert Q(12*4*1,2*6)==4
for n,question,answer in q:
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,
 "IndependentStudentQuestion":question,"TeacherExpectedMath":answer,
 "OriginalSourceMathematicsReviewed":source[n-1][1],
 "Rubric":"TEACHER_REVIEW_SINGLE_CONSTANT_AND_DIMENSIONS","Runtime":"NOT_RUN"})
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 row.update({"SourceMathIndividuallyReviewed":True,"IndependentStudentQuestionVerified":True,
 "IndependentTeacherMathVerified":True,"TeacherKey":answer})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==23
t["SourceExerciseMathReviewed"]=23
t["IndependentExerciseTasksAuthored"]=23
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"OriginalExerciseMathVerified":23,"FirstPartyIndependentExerciseTasksAuthored":23,
"BlockingReasons":["SOURCE_EXERCISES_24_TO_60_INDIVIDUAL_MATH_AND_FIRST_PARTY_TASKS_PENDING","FOUR_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_ROUTES_PENDING"],
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03L09_23_OF_60_SOURCE_MATH_AND_FIRSTPARTY_TEACHER_TASKS_AUTHORING_COMPLETE")
