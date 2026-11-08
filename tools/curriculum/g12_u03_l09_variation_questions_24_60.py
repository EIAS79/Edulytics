from pathlib import Path
import json, math, datetime
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l09-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l09-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==23 and len(t["FirstPartyIndependentStudentMathQuestions"])==23
entries=[
(24,"Direct y=kx, y=18 at x=3. Find y at x=10.","k=6; y=60.","Original k=12/3=4; y(20)=80."),
(25,"Direct square variation, y=27 at x=3. Find y at x=5.","k=3; y=75.","Original k=16/4=4; y(8)=256."),
(26,"Direct cube variation, y=16 at x=2. Find y at x=3.","k=2; y=54.","Original k=5/27; y(4)=320/27."),
(27,"Direct square root variation, y=9 at x=9. Find y at x=25.","k=3; y=15.","Original k=4/4=1; y(36)=6."),
(28,"Direct cube root variation, y=8 at x=64. Find y at x=125.","k=2; y=10.","Original k=15/5=3; y(1000)=30."),
(29,"Inverse variation, y=10 at x=2. Find y at x=5.","k=20; y=4.","Original k=3*2=6; y(1)=6."),
(30,"Inverse-square variation, y=5 at x=2. Find y at x=4.","k=20; y=5/4.","Original k=3*16=48; y(2)=12."),
(31,"Inverse-cube variation, y=4 at x=2. Find y at x=4.","k=32; y=1/2.","Original k=1*27=27; y(1)=27."),
(32,"Inverse square-root variation, y=9 at x=16. Find y at x=81.","k=36; y=4.","Original k=12*8=96; y(36)=16."),
(33,"Inverse cube-root variation, y=6 at x=8. Find y at x=27.","k=12; y=4.","Original k=5*3=15; y(125)=3."),
(34,"Joint y=kxz, y=24 at x=3,z=4. Find y for x=2,z=5.","k=2; y=20.","Original k=16/(4*2)=2; y(3,3)=18."),
(35,"Joint y=kxzw, y=30 at x=2,z=3,w=5. Find y for x=3,z=2,w=2.","k=1; y=12.","Original k=72/(2*1*12)=3; y(1,2,3)=18."),
(36,"Joint y=kxz², y=54 at x=3,z=3. Find y for x=2,z=5.","k=2; y=100.","Original k=144/(2*16)=9/2; y(4,5)=450."),
(37,"Joint y=kx²√z, y=36 at x=2,z=9. Find y at x=3,z=16.","k=3; y=108.","Original k=24/(4*3)=2; y(3,25)=90."),
(38,"Joint/inverse y=kxz/w, y=20 at x=4,z=5,w=2. Find y at x=3,z=8,w=6.","k=2; y=8.","Original k=4*20/(5*2)=8; y(3,8,48)=4."),
(39,"Joint/inverse y=kx²z³/√w, y=16 at x=2,z=2,w=16. Find at x=1,z=3,w=9.","k=2; y=18.","Original k=12*8/(4*8)=3; y(1,3,4)=81/2."),
(40,"Joint/inverse y=kx²z/(√w*t), y=6 at x=2,z=3,w=16,t=1. Find for x=3,z=2,w=9,t=2.","k=2; y=6.","Original y=kx²z/(√w*t), k=1*4*3/(4*3)=1; new y=9*2/(6*5)=3/5."),
(41,"Graph direct square variation y=12 at x=2, with vertex and sample symmetric points.","y=3x², vertex (0,0), points (−2,12),(2,12).","Original y=(3/4)x² with points ±2 mapped to3."),
(42,"Graph direct cube variation y=16 at x=2, with sign and origin.","y=2x³; passes (−2,−16),(0,0),(2,16).","Original y=(1/2)x³ passes (2,4)."),
(43,"Graph direct square-root variation y=9 at x=9; state domain.","y=3√x, domain x≥0; points (0,0),(4,6),(9,9).","Original y=(1/3)√x because at x=36, y=2."),
(44,"Graph inverse variation with y=8 at x=2; state asymptotes.","y=16/x, x≠0; asymptotes x=0 and y=0.","Original y=12/x with x=6,y=2."),
(45,"Graph inverse-square variation y=3 at x=2; state signs.","y=12/x², x≠0; both branches positive, asymptotes x=0 and y=0.","Original y=4/x² with x=1,y=4."),
(46,"Kepler: orbital years T and astronomical distance a obey T²=ka³. Earth (1,1). Find equation.","T²=a³ for a≥0, T≥0; T=a^(3/2).","Source Earth 1 yr,93 million miles: T²=(a/93)^3 when a in million miles."),
(47,"Find orbital period for a planet 4 AU from Sun under T²=a³.","T=8 years.","Source Mars: T=(142/93)^(3/2)≈1.886 years."),
(48,"Use Earth as (T=1 yr,a=150 million km) to define Kepler variation.","T²=(a/150)^3, T in years, a in million km.","Source Earth150 million km: T²=(a/150)^3."),
(49,"Find orbital period at a=60 million km under Earth distance150 million km.","T=(60/150)^(3/2)= (2/5)^(3/2) ≈0.253 years.","Source Venus: T=(108/150)^(3/2)≈0.611 years."),
(50,"Using T²=a³ in AU/years, find period for planet at a=4 AU.","T=8 years.","Source Saturn at9.54 AU: T=9.54^(3/2)≈29.46 years."),
(51,"An object falls s=kt², traveling 25 ft at t=1s. Find time for s=225ft.","k=25 ft/s²; t=√(225/25)=3 s.","Source 16 ft in1s, to144 ft: t=3 s."),
(52,"Velocity v=kt, v=30 ft/s at 2 s. Find v at 6s.","k=15 ft/s²; v=90 ft/s.","Source v=64 at t=2 -> v=160 ft/s at5 s."),
(53,"String rate f=k/L, L=30 cm gives f=120Hz. Find L for 60Hz.","k=3600 Hz·cm; L=60cm.","Source length24in,rate128Hz -> at64Hz length48in."),
(54,"At constant temperature gas V=k/P, V=900cm³ at P=300 mmHg. Find V at P=450.","k=270000; V=600cm³.","Source V=1200 at P=200 -> V=800cm³ at P300."),
(55,"Weight varies inverse-square with center distance, W=80 lb at d=4000 miles. Find W at d=5000.","W=80(4000/5000)²=51.2lb.","Source W=50(3960/3970)² pounds, approximately49.748."),
(56,"Light intensity inverse-square; I=0.12 foot-candles at d=2 m. Find intensity at d=6m.","I=0.12(2/6)²=1/75≈0.013333 foot-candles.","Source I=0.08(3/8)²=0.01125 foot-candles."),
(57,"Electrical current I=k/R; I=25A at R=8Ω. Find I at R=20Ω.","k=200 A·Ω; I=10A.","Source I=40*10/12=100/3 A."),
(58,"Wind force F=kAv²; F=20 lb at A=50ft²,v=10mph. Find at A=40ft²,v=20mph.","Ratio (40/50)*(20/10)²=3.2; F=64lb.","Source F=15*(65/40)*(30/20)²=54.84375 pounds."),
(59,"Shaft power P=knd³. A 2-in shaft passes40hp at 100rpm. Find d for P=135hp at 100rpm.","d=2*(135/40)^(1/3)≈3 inches (since ratio27/8).","Source d=3*((60/45)*(100/150))^(1/3)=3*(8/9)^(1/3) inches."),
(60,"Kinetic energy K=kmv². At fixed mass, K=800J when v=10 m/s. Find K at 15 m/s.","K=800*(15/10)²=1800J.","Source K=1000*(20/15)²=16000/9≈1777.78J. 40 kilograms is mass not weight.")
]
assert [x[0] for x in entries]==list(range(24,61))
assert len(entries)==37
assert F(15)*F(65,40)*F(30**2,20**2)==F(1755,32)
assert F(1000)*F(20**2,15**2)==F(16000,9)
assert F(12)*F(1**2*3**3,2**2*2**3)*F(8,2)==F(81,2)
assert math.isclose((142/93)**1.5,1.886,abs_tol=.003)
assert math.isclose((9.54)**1.5,29.47,abs_tol=.1)
for n,q,answer,source_answer in entries:
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":answer,"OriginalSourceMathematicsReviewed":source_answer,"Rubric":"TEACHER_REVIEW_FORMULA_CONSTANT_DOMAIN_UNITS","Runtime":"NOT_RUN"})
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 row.update({"SourceMathIndividuallyReviewed":True,"IndependentStudentQuestionVerified":True,"IndependentTeacherMathVerified":True,"OriginalTextCopied":False})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==60
assert [q["SourceSkillReferenceExercise"] for q in t["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,61))
t["SourceExerciseMathReviewed"]=60;t["IndependentExerciseTasksAuthored"]=60
t["SourceOriginalScientificClaimsRestricted"]="Kepler formula exercises are idealized two-body models; physical cases use positive distances and periods. Kinetic mass in kg is not weight."
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=60
a["FirstPartyIndependentExerciseTasksAuthored"]=60
a["BlockingReasons"]=["FOUR_ORIGINAL_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_REUSE_ASSESSMENT_ROUTES_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03_L09_SOURCE_60_OF_60_MATH_TASKS_REVIEWED_AND_INDEPENDENT_KEYS_AUTHORED_PENDING_FIGURES_AND_STAGE_C")
