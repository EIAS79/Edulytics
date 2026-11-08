from pathlib import Path
import json,math,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l01-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==50
def fit(x1,y1,x2,y2):
 b=(y2/y1)**(1/(x2-x1));aa=y1/b**x1
 return aa,b
def fitstr(x1,y1,x2,y2):
 aa,b=fit(x1,y1,x2,y2)
 return f"a≈{aa:.6g}, b≈{b:.6g}; exponential f(x)=a·b^x, positive base. Rounded source input data limit precision."
q=[
(51,"Fit positive exponential model f=ab^x passing through (0,4),(2,100).","a=4, b=5, so f(x)=4·5^x.",fitstr(0,3,3,375)),
(52,"Fit f=ab^x through (2,50),(5,25).",fitstr(2,50,5,25),fitstr(3,222.62,10,77.456)),
(53,"Fit f=ab^x through (10,40),(30,80).",fitstr(10,40,30,80),fitstr(20,29.495,150,730.89)),
(54,"Fit f=ab^x through (1,32),(4,4).","b=1/2; a=64; f(x)=64(1/2)^x.",fitstr(5,2.909,13,.005)),
(55,"Fit f=ab^x through (0,200),(10,300).",fitstr(0,200,10,300),fitstr(11,310.035,25,356.3652)),
(56,"Show formula for annual percentage yield when interest compounded quarterly.","APY=(1+r/4)^4−1 for stated nominal annual rate r.","Source monthly APY=(1+r/12)^12−1."),
(57,"Derive general APY for n interest creditings per year and specialize to daily.","APY_n=(1+r/n)^n−1; daily with n=365.","Source same APY_n; daily n=365 (assuming 365-day convention)."),
(58,"Rewrite f(x)=7(1.05)^x using natural exponential base.","f(x)=7e^(x ln 1.05), since b=e^(ln b).","Source a b^x=a e^((ln b)x), b>0."),
(59,"Rewrite f(x)=100(0.8)^t as e-base continuous decay.","f(t)=100e^((ln 0.8)t), where ln0.8<0.","Source a(1/b)^x=a e^(−(ln b)x) for b>1."),
(60,"For A(t)=500e^(0.03t), prove interest earned as fraction of original principal.","[A(t)−500]/500=e^(0.03t)−1.","Source (A−a)/a=e^(rt)−1."),
(61,"A population begins at 12,000 in 2020 with 4% annual growth. Predict 2028.",f"P(2028)=12000(1.04)^8≈{12000*1.04**8:.0f} individuals.",f"Source 23900(1.09)^8≈{23900*1.09**8:.0f} foxes for2020."),
(62,"A 200 mg radioactive sample halves every 20 hours. What remains after 35 hours?",f"M(35)=200(1/2)^(35/20)≈{200*.5**(35/20):.3f}mg.",f"Source 100(1/2)^(54/35)≈{100*.5**(54/35):.3f}mg."),
(63,"An artwork rises from $20,000 in 2010 to $30,000 in 2020. Compute annual percentage and project 2025.",f"b=(30000/20000)^(1/10)≈{(1.5)**.1:.6f}; r≈{100*((1.5)**.1-1):.3f}%; V2025=20000(1.5)^(15/10)≈${20000*1.5**1.5:.2f}.",f"Source b=(145000/110000)^(1/20), annual rate≈{100*((145000/110000)**(1/20)-1):.3f}%; 2010 value≈${110000*((145000/110000)**(25/20)):.2f}."),
(64,"A car depreciates from $24,000 to $12,000 in 4 years. Project its value after 7 years.",f"V7=24000(1/2)^(7/4)≈${24000*.5**(7/4):.2f}.",f"Source V2017=38000(11000/38000)^(10/6)≈${38000*(11000/38000)**(10/6):.2f}."),
(65,"Find a required deposit for target $30,000 after 4 years at APR 5% compounded daily.",f"P=30000/(1+.05/365)^(365·4)≈${30000/(1+.05/365)**(365*4):.2f}.",f"Source P=54000/(1+.082/365)^(365·5)≈${54000/(1+.082/365)**(365*5):.2f}."),
(66,"What APR compounded monthly raises $8,000 to $10,000 in 5 years?",f"r=12[(10000/8000)^(1/(12·5))−1]≈{100*12*((10000/8000)**(1/60)-1):.4f}%.",f"Source nominal daily r=365[(15000/10000)^(1/(365·6))−1]≈{100*365*((1.5)**(1/(365*6))-1):.4f}%."),
(67,"Compare $10,000 after 10 years at APR 4% with monthly vs continuous compounding.",f"Monthly=10000(1+.04/12)^120≈${10000*(1+.04/12)**120:.2f}; continuous=10000e^0.4≈${10000*math.exp(.4):.2f}; extra≈${10000*(math.exp(.4)-(1+.04/12)**120):.2f}.",f"Source A_monthly=13500(1+.0725/12)^300≈${13500*(1+.0725/12)**300:.2f}; A_continuous=13500e^(0.0725·25)≈${13500*math.exp(.0725*25):.2f}; difference≈${13500*(math.exp(.0725*25)-(1+.0725/12)**300):.2f}."),
(68,"Compare $6,000 after 3 years at 5% APR: annual, quarterly, monthly and continuous compounding.",f"Annual≈${6000*(1+.05)**3:.2f}; quarterly≈${6000*(1+.05/4)**12:.2f}; monthly≈${6000*(1+.05/12)**36:.2f}; continuous≈${6000*math.exp(.05*3):.2f}.",f"Source 4000,7% APR,9 years: annual≈${4000*1.07**9:.2f}, quarterly≈${4000*(1+.07/4)**36:.2f}, monthly≈${4000*(1+.07/12)**108:.2f}, continuous≈${4000*math.exp(.07*9):.2f}.")
]
assert [z[0]for z in q]==list(range(51,69))
assert math.isclose(fit(0,3,3,375)[1],5)
assert math.isclose(fit(5,2.909,13,.005)[1]**8,.005/2.909)
assert 0 < 10000*(math.exp(.4)-(1+.04/12)**120) < 100
for n,quest,key,original in q:
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 row.update({"SourceMathematicsIndividuallyReviewed":True,"FirstPartyIndependentExerciseAuthored":True,"TeacherAnswerVerified":True})
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":quest,"TeacherExpectedMath":key,"OriginalSourceMathematicsReviewed":original,
 "Rubric":"TEACHER_REVIEW_EXPONENTIAL_GROWTH_ROUNDING_UNITS_MODEL_LIMITS","Runtime":"NOT_RUN"})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==68
t["SourceExerciseMathReviewed"]=68;t["IndependentExerciseTasksAuthored"]=68
t["MathSourcePrecisionLimitations"]="Exponential fit source graph coordinate datasets are rounded; use appropriate coefficient precision, not exact numeric parity."
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=68;a["IndependentExerciseTasksAuthored"]=68
a["BlockingReasons"]=["6_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_ALL_68_EXERCISES_MATH_SOURCE_REVIEWED_AND_INDEPENDENT_TEACHER_KEYS_AUTHORED")
