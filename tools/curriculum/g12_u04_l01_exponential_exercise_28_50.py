from pathlib import Path
import json,math,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l01-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==27
q=[
(28,"Calculate the value of $8,000 at APR 3% monthly compounding for 5 years.",f"A=8000(1+0.03/12)^60 = ${8000*(1+.03/12)**60:.2f}.",f"Source A=10250(1+0.04/12)^120 ≈${10250*(1+.04/12)**120:.2f}."),
(29,"Identify the initial principal from A=7200(1+0.05/4)^24.","P=$7200.","Source coefficient P=$10250."),
(30,"How long is A=7200(1+0.05/4)^24 invested, with quarterly compounding?","24 quarters/4=6 years.","Source 120 months/12=10 years."),
(31,"Find future amount for $5,000 at APR 4% compounded semiannually for 10 years.",f"A=5000(1.02)^20≈${5000*1.02**20:.2f}.",f"Source 6500(1+.036/2)^40≈${6500*(1+.036/2)**40:.2f}."),
(32,"How much more is $5,000 at 4% APR worth after 10 years if compounded weekly instead of semiannually?",f"Difference=5000[(1+.04/52)^520−(1+.04/2)^20]≈${5000*((1+.04/52)**520-(1+.04/2)**20):.2f}.",f"Source difference=6500[(1+.036/52)^1040−(1+.036/2)^40]≈${6500*((1+.036/52)**1040-(1+.036/2)**40):.2f}."),
(33,"Solve A=P(1+r/n)^(nt) for original principal P.","P=A/(1+r/n)^(nt), assuming positive base and n,t positive.","Source same formula P=A/(1+r/n)^(nt)."),
(34,"Find principal needed for $12,000 at 6% APR compounded monthly for 4 years.",f"P=12000/(1+.06/12)^48≈${12000/(1+.06/12)**48:.2f}.",f"Source P=14472.74/(1+.055/12)^60≈${14472.74/(1+.055/12)**60:.0f} nearest dollar."),
(35,"For $5000 at 4% compounded semiannually, find additional gain between years 10 and 15.",f"A15−A10=5000[(1.02)^30−(1.02)^20]≈${5000*(1.02**30-1.02**20):.2f}.",f"Source #31 account additional 5 years: 6500[(1+.036/2)^50−(1+.036/2)^40]≈${6500*((1+.036/2)**50-(1+.036/2)**40):.2f}; source #34 account additional 5 years: 14472.74[(1+.055/12)^60−1]≈${14472.74*((1+.055/12)**60-1):.2f}."),
(36,"Solve A=P(1+r/n)^(nt) for nominal APR r.","r=n[(A/P)^(1/(nt))−1], for P,A,n,t>0.","Source same APR inversion formula."),
(37,"If $8,000 becomes $10,000 in 5 years with semiannual compounding, find APR.",f"r=2[(10000/8000)^(1/10)−1]≈{100*2*((10000/8000)**(1/10)-1):.4f}%.",f"Source r=2[(13373.53/9000)^(1/20)−1]≈{100*2*((13373.53/9000)**(1/20)-1):.4f}%."),
(38,"A deposit of $3,000 grows to $5,000 in 8 years compounded monthly. Find APR.",f"r=12[(5000/3000)^(1/96)−1]≈{100*12*((5000/3000)**(1/96)-1):.4f}%.",f"Source r=12[(38455/5500)^(1/360)−1]≈{100*12*((38455/5500)**(1/360)-1):.4f}%."),
(39,"Classify N(t)=500e^(0.04t).","Continuous exponential growth, rate k=0.04>0.","Source y=3742e^(0.75t) is continuous growth."),
(40,"Classify N(t)=100e^(2/t) for positive t as standard constant-rate growth/decay.","Neither standard constant-rate exponential growth nor decay in t; its exponent 2/t is not kt.","Source 150e^(3.25/t) similarly not in standard a e^(kt) form; for t>0 decreases toward 150 non-constant proportional rate."),
(41,"Classify M(t)=40e^(−0.2t).","Continuous exponential decay, rate k=−0.2.","Source y=2.25e^(−2t) continuous decay."),
(42,"Find account A for $5,000 at 5% continuous interest over 6 years.",f"A=5000e^(0.05·6)≈${5000*math.exp(.3):.2f}.",f"Source 12000e^(.072·30)≈${12000*math.exp(.072*30):.2f}."),
(43,"How much less does $5,000 earn at 5% APR after six years with monthly rather than continuous compounding?",f"Continuous−monthly=5000[e^(.3)−(1+.05/12)^72]≈${5000*(math.exp(.3)-(1+.05/12)**72):.2f}.",f"Source difference=12000[e^2.16−(1+.072/12)^360]≈${12000*(math.exp(2.16)-(1+.072/12)**360):.2f}."),
(44,"Evaluate f(x)=3·4^x at x=−2.","f(−2)=3/16=0.1875.","Source 2·5^(−3)=2/125=0.016."),
(45,"Evaluate f(x)=−5^(2x+3) at x=−1.","f(−1)=−5^1=−5.","Source f(−1)=−4^(2(−1)+3)=−4, negative sign outside exponent."),
(46,"Evaluate f(x)=e^x at x=2.",f"e²≈{math.exp(2):.4f}.",f"Source e³≈{math.exp(3):.4f}."),
(47,"Evaluate f(x)=−3e^(x−2) at x=1.",f"−3/e≈{(-3/math.e):.4f}.",f"Source −2e^(−2)≈{(-2*math.exp(-2)):.4f}."),
(48,"Evaluate f(x)=3·2^(−x+2)+1 at x=−1.","3·2³+1=25.","Source 2.7·4^(−(−2)+1)+1.5=2.7·64+1.5=174.3."),
(49,"Evaluate f(x)=2e^(x)−1 at x=3.",f"2e³−1≈{2*math.exp(3)-1:.4f}.",f"Source 1.2e^(2·3)−0.3≈{1.2*math.exp(6)-.3:.4f}."),
(50,"Evaluate f(x)=−2·4^(−x)+2 at x=2.","−2/16+2=15/8=1.875.","Source −(3/2)3^(−2)+3/2=4/3≈1.3333.")
]
assert [z[0]for z in q]==list(range(28,51))
assert len(q)==23
for n,quest,key,source in q:
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":quest,"TeacherExpectedMath":key,"OriginalSourceMathematicsReviewed":source,"MarkingMode":"TEACHER_REVIEW_EXPONENTIAL_FINANCIAL_UNITS","Runtime":"NOT_RUN"})
 row.update({"SourceMathematicsIndividuallyReviewed":True,"FirstPartyIndependentExerciseAuthored":True,"TeacherAnswerVerified":True})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==50
t["SourceExerciseMathReviewed"]=50;t["IndependentExerciseTasksAuthored"]=50
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=50;a["IndependentExerciseTasksAuthored"]=50
a["BlockingReasons"]=["SOURCE_EXERCISES_51_TO_68_REQUIRE_INDIVIDUAL_MATH_REVIEW","6_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_50_OF_68_MATH_QUESTIONS_TEACHER_KEYS_REVIEWED")
