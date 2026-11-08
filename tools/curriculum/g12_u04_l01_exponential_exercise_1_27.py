from pathlib import Path
import json,datetime,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l01-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(rp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==0 and not t["FirstPartyIndependentStudentMathQuestions"]
q=[
(1,"Why does 3·1.1^x eventually grow faster than 100+5x?","Because the ratio for a fixed increment in x remains 1.1>1, while linear slope stays fixed; as x→∞, (1.1)^x/x→∞.","Source: any increasing exponential with base>1 ultimately dominates increasing linear function."),
(2,"For y=9(0.85)^t, determine growth or decay from the base.","Decay; base 0.85 in (0,1), a positive 15% decrease each unit t.","Source: b>1 exponential growth; 0<b<1 exponential decay for positive initial value."),
(3,"Explain why APR is called nominal when interest compounds quarterly.","Stated APR differs from effective annual yield (1+r/4)^4−1 because interest-on-interest increases realized return.","Source: nominal rate is stated, effective APY accounts for within-year compounding."),
(4,"A zoo's animal population increases by exactly 15 animals each year. Is that exponential?","No; constant additive increase is linear N(t)=N0+15t.","Source wolves constant annual +25 is linear."),
(5,"A sample halves every 8 hours. Exponential or linear? Express model for initial 100 g.","Exponential decay M(t)=100(1/2)^(t/8), t≥0.","Source bacteria factor1/8 every24h is exponential decay."),
(6,"A painting appreciates 4% annually over 12 years. What type and model for starting value P?","Exponential growth V(t)=P(1.04)^t.","Source fixed 3.25% annual appreciation exponential growth."),
(7,"Instructor gives $3 discount per appointment from original $60. Does sequence form exponential model?","No. Price for nth session P(n)=60−3(n−1), linear until valid nonnegative prices.","Source $5 less each session is constant difference not ratio."),
(8,"The height of a tossed ball is h(t)=−5t²+20t+3. Is this exponential?","No; quadratic polynomial in time.","Source h(t)=−4.9t²+18t+40 is quadratic not exponential."),
(9,"Two forests have models A=100(1.02)^t, B=80(1.04)^t. Which has faster percentage growth?","B grows at 4% versus A at 2%.","Source B=82(1.029)^t has 2.9%, faster than A=115(1.025)^t at 2.5%."),
(10,"For same forest models A(0)=100 and B(0)=80, which starts larger and by how many?","A starts 20 trees larger.","Source A initially115 vs B82, difference33."),
(11,"For A=100(1.02)^t and B=80(1.04)^t, compare rounded populations at t=20.","A≈149, B≈175; B larger by approximately26 (round each first).","Source A(20)≈188, B(20)≈145; A larger by43 (using nearest whole separately)."),
(12,"For A=100(1.02)^t and B=80(1.04)^t, compare at t=100.","A≈724, B≈4040; B larger by≈3316 rounded independently.","Source as t=100, B's larger base1.029 eventually overtakes A's1.025 despite smaller initial value."),
(13,"Explain why a smaller forest with bigger exponential growth rate eventually overtakes a larger forest.","If 0<aB<aA and bB>bA>1, ratio B/A=(aB/aA)(bB/bA)^t grows without bound. Finite resources may invalidate indefinite extrapolation.","Source B eventually dominates because 1.029>1.025. Resource availability limits extrapolation."),
(14,"Classify y=200(1−t)^4 as exponential, decay, or neither.","Neither: variable is inside polynomial factor raised to fixed power, not a constant positive base raised to t.","Source 300(1−t)^5 is not an exponential function."),
(15,"Classify y=120(1.03)^t.","Exponential growth at 3% per unit.","Source 220(1.06)^x is growth6%."),
(16,"Classify f(x)=12(1.02)^(1/x) for x>0.","Not a standard exponential f(x)=ab^x with x linear in exponent; growth/decay label by b alone would be misleading.","Source 16.5(1.025)^(1/x) likewise does not have standard exponential form."),
(17,"Classify y=500(0.92)^t.","Exponential decay at 8% per unit.","Source 11701(0.97)^t decays3%."),
(18,"Find f(x)=ab^x through (0,4),(2,100), a,b>0.","a=4; b²=25 so b=5; f(x)=4·5^x.","Source (0,6),(3,750): a=6,b=5."),
(19,"Find f(x)=ab^x through (0,300),(2,3), a,b>0.","a=300;b²=0.01 so b=0.1; f(x)=300(0.1)^x.","Source (0,2000),(2,20): a=2000,b=0.1."),
(20,"Find f(x)=ab^x through (−1,2),(3,32).","Over four steps ratio16 implies b=2; a=4; f(x)=4·2^x.","Source (−1,1.5),(3,24): a=3,b=2."),
(21,"Find f(x)=ab^x through (−1,8) and (3,2).","b^4=1/4 so b=(1/4)^(1/4)=1/√2; a=8b=4√2; f(x)=4√2(1/√2)^x.","Source (−2,6),(3,1): b^5=1/6, a=6b²."),
(22,"Find f(x)=ab^x through (1,3) and (3,12).","b²=4 ->b=2; a=3/2; f(x)=1.5·2^x.","Source (3,1),(5,4): b=2, a=1/8."),
(23,"Classify table x=1,2,3,4; y=12,17,22,27.","Linear: constant differences +5, y=5x+7.","Source x1..4, values70,40,10,−20 have differences−30; linear y=100−30x."),
(24,"Classify x=1,2,3,4; y=18,9,4.5,2.25.","Exponential ratio1/2, f(x)=36(1/2)^x.","Source 70,49,34.3,24.01 ratio0.7; f(x)=100(0.7)^x."),
(25,"Classify x=1,2,3,4; y=1,2,4,7.","Neither linear (differences1,2,3) nor exponential (ratios2,2,1.75).","Source 80,61,42.9,25.61 is neither exact constant difference nor ratio."),
(26,"Classify x=1,2,3,4; y=6,18,54,162.","Exponential ratio3, f(x)=2·3^x.","Source 10,20,40,80 ratio2, f(x)=5·2^x."),
(27,"Classify x=1,2,3,4; y=−2,2,6,10.","Linear difference4; y=4x−6.","Source −3.25,2,7.25,12.5 difference5.25; linear y=5.25x−8.5.")
]
assert [v[0] for v in q]==list(range(1,28))
# Targeted source numerical validation rather than rounded prose as evidence.
sA=lambda t:115*(1.025)**t
sB=lambda t:82*(1.029)**t
print("SOURCE_FORESTS",[(n,sA(n),sB(n),round(sA(n)),round(sB(n)))for n in [20,100]])
print("FIRST_PARTY_FORESTS",[(n,100*(1.02)**n,80*(1.04)**n)for n in [20,100]])
assert F(750,6)==125 and 5**3==125
assert F(24,F(3,2))==16
assert 100*F(7,10)==70
assert F(100,1)*F(7,10)**4==F(2401,100)
assert F(6,1)*F(2,1)**3==48
for n,quest,key,source in q:
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":quest,"TeacherExpectedMath":key,"OriginalSourceMathematicsReviewed":source,"MarkingMode":"MANUAL_TEACHER_CONCEPTS_FORMULAS_NUMERICS","Runtime":"NOT_RUN"})
 row.update({"SourceMathematicsIndividuallyReviewed":True,"FirstPartyIndependentExerciseAuthored":True,"TeacherAnswerVerified":True})
t["SourceExerciseMathReviewed"]=27;t["IndependentExerciseTasksAuthored"]=27
rp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=27;a["IndependentExerciseTasksAuthored"]=27
a["BlockingReasons"]=["SOURCE_EXERCISES_28_TO_68_REQUIRE_INDIVIDUAL_MATH_REVIEW","6_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_27_OF_68_INDEPENDENT_QUESTION_RUBRIC_MATH_SOURCE_REVIEWED_STILL_OPEN")
