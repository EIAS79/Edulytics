from pathlib import Path
import math,json,datetime,subprocess,re,time,pypdfium2 as pdfium
start=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==26 and p["FirstIncompleteLessonOrdinal"]==27
row=p["LessonEntries"][26]
assert row["Ordinal"]==27 and row["SourceSection"]=="4.7" and not row["ReadyForStageD"]
pdfpath=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
pdf=pdfium.PdfDocument(str(pdfpath))
get=lambda i:pdf[i].get_textpage().get_text_range()
assert "4.7" in get(416) and "4.8" in get(431)
pages=[{"PdfPage":i,"Text":get(i)} for i in range(416,431)]
full="\n".join(x["Text"]for x in pages)
examples=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",full,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",full,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",full,re.I)})
assert [n for n,_ in examples]==list(range(1,9))
assert tries==list(range(14,21)) and fig==list(range(1,12))
assert "53." in get(430)
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson007.json")
cache.write_text(json.dumps({"Section":"4.7","SourceSha256":p["SourceSnapshotSha256"],"Pages":pages},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
Q=[]
def add(q,ans,source):
 n=len(Q)+1
 assert q and ans and source
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherMath":ans,"OriginalSourceMathReviewed":source,
 "OriginalTextCopied":False,"TeacherManualReview":True,"AuthenticatedRuntimeVerified":False})
add("Explain how radioactive half-life relates to exponential models.","A(t)=A0(1/2)^(t/T) where T>0 is half-life; decreasing exponential.","Source #1 half-life corresponds to exponential decay and determines decay rate.")
add("Explain why the remaining carbon-14 fraction can date an artifact.","A measured remaining fraction r>0 implies age t=T ln(r)/ln(1/2); assumes a known historical starting fraction and unchanged system.","Source #2 carbon dating uses carbon-14 half-life for appropriate biological archaeological material.")
add("For a population doubling every 8 years, give a model and define doubling time.","P(t)=P0·2^(t/8), t in years; doubling time determines k=ln2/8.","Source #3 doubling time is associated with exponential growth.")
add("Write Newton's cooling law and give three applications.","T(t)=Ts+(T0−Ts)e^(−kt), k>0; cooling soup, hot metal, cooked food.","Source #4 states Newton cooling model and applications.")
add("What is an order of magnitude and why does it matter?","An order-of-magnitude difference is a factor of 10 on a base10 scale; helpful comparing vastly different intensities.","Source #5 orders of magnitude clarify multiplicative scale.")
add("A body has temperature T(t)=20e^(−0.04t)+70 °F. Find after30 minutes.",f"T(30)=20e^(−1.2)+70≈{20*math.exp(-1.2)+70:.0f}°F.","Source #6 T=68e^(−.0174t)+72 at t=90 min.")
logistic=lambda c,a,k,t:c/(1+a*math.exp(-k*t))
add("For P(t)=200/(1+4e^(−0.5t)), find initial population.","P(0)=200/5=40.","Source #7 logistic f=150/(1+8e^(−2x)), f(0)=150/9≈16.7.")
add("For P(t)=200/(1+4e^(−0.5t)), find population at t=6.",f"P(6)=200/(1+4e^(−3))≈{logistic(200,4,.5,6):.1f}.","Source #8 f(4)=150/(1+8e^(−8))≈149.6.")
add("For P(t)=200/(1+4e^(−0.5t)), find carrying capacity.","200, because e^(−0.5t)→0 as t→∞.","Source #9 carrying capacity 150.")
add("Describe graph P(t)=200/(1+4e^(−0.5t)), t≥0.","Initially40, increasing sigmoid, inflection at t=ln4/.5 and P=100, asymptote P=200.","Source #10 graph logistic f=150/(1+8e^(−2t)).")
add("Identify table x=0,1,2,3 with y=3,6,12,24 and fit model.","Exponential ratio2; f(x)=3·2^x.","Source #11 table f(x)≈1.2^x is exponential, ratios near1.2.")
add("Rewrite f(x)=5(0.8)^x as an e-base function.","f(x)=5e^((ln0.8)x), ln0.8<0.","Source #12 1.68(0.65)^x becomes1.68e^((ln.65)x).")
for q,ans,src in [
("Classify data x=1,2,3,4 and y=2,4,6,8.","Linear y=2x.","Source #13 given scatter data resemble logarithmic growth, so use graph shape."),
("Classify data x=0,1,2,3 and y=2,3,4.5,6.75.","Exponential y=2(1.5)^x.","Source #14 table ratio≈1.2 each step, exponential."),
("Classify data x=1,2,4,8 and y=0,1,2,3.","Logarithmic y=log_2(x).","Source #15 curve rises slowly with decreasing increments, logarithmic."),
("Classify data x=1,2,3,4 and y=3,8,13,18.","Linear y=5x−2.","Source #16 table near constant slope ≈3 over x changes, linear.")
]:add(q,ans,src)
add("Explain the graph for fish P(t)=300/(1+5e^(−0.4t)).","P(0)=50, rising sigmoid to carrying capacity300, inflection at t=ln5/.4.","Source #17 graph logistic fish P=1000/(1+9e^(−.6t)).")
add("For that fish model find initial population.","P(0)=300/6=50 fish.","Source #18 P(0)=1000/10=100.")
add("Find time until fish P(t)=300/(1+5e^(−0.4t)) doubles from50 to100.",f"300/(1+5e^(−.4t))=100 => e^(−.4t)=2/5, t=ln(5/2)/.4≈{math.log(2.5)/.4:.1f} years.","Source #19 logistic doubling time is not constant; solve P(t)=200 for source initial100.")
add("Find that fish population at t=3.",f"P(3)=300/(1+5e^(−1.2))≈{logistic(300,5,.4,3):.0f} fish.","Source #20 source logistic at t2 ≈270 fish.")
add("For same model find time when P=270.",f"e^(−.4t)=1/45, t=ln45/.4≈{math.log(45)/.4:.1f} years.","Source #21 solve source logistic when P=900, t=ln81/.6.")
add("State the carrying capacity for fish P(t)=300/(1+5e^(−0.4t)).","300 fish; horizontal long-term asymptote.","Source #22 source carrying capacity1000.")
add("A 64-g isotope sample has half-life3 hours. How long until4 grams remain?","64/4=16=2^4, four half-lives, t=12 hours.","Source #23 132.8 to8.3 factor16, four half-lives times2.045 minutes.")
add("For continuous growth P=P0e^(rt), derive t for multiplication by factorM>1.","M=e^(rt) =>t=lnM/r, r>0.","Source #24 symbolic derivation of multiplication time.")
add("Solve M=(2/3)log10(S/S0) for S.","S=S0·10^(3M/2), S0>0.","Source #25 seismic moment inversion.")
add("Find initial logistic population f(t)=600/(1+11e^(−.5t)).","f(0)=600/12=50.","Source #26 logistic intercept c/(1+a).")
add("Prove b^x=e^(x ln b) for b>0.","b=e^(lnb), so b^x=(e^lnb)^x=e^(xlnb).","Source #27 standard base-conversion identity.")
add("A drug retains 80% each hour. Find its half-life.",f"t=ln(.5)/ln(.8)≈{math.log(.5)/math.log(.8):.2f} hours.","Source #28 drug 70% retained each hour, half-life ln.5/ln.7.")
add("A 100mg drug retains 80% hourly. Model and amount after3 hours.","A(t)=100(.8)^t mg; A(3)=51.2mg.","Source #29 125(.7)^t, t3≈43mg.")
add("Find amount after10 hours in preceding drug model.",f"A(10)=100(.8)^10≈{100*.8**10:.2f}mg.","Source #30 125(.7)^10≈3.53mg.")
add("A decaying sample loses 2% per day continuously. Find half-life.",f"T=ln2/.02≈{math.log(2)/.02:.0f} days.","Source #31 iodine-125 continuous1.15% per day, half-life ln2/.0115.")
add("A 0.8g sample decays at continuous rate2% per day. Find mass after40 days.",f"A(t)=0.8e^(−.02t), A(40)≈{.8*math.exp(-.8):.3f}g.","Source #32 iodine-125 0.5e^(−.0115t), t60.")
add("A 200g sample becomes50g after20 minutes. Fit exponential and find half-life.","A(t)=200e^((ln.25/20)t); half-life10 minutes.","Source #33 radioactive sample 250g to32g in250 min, solve rate and half-life.")
add("A nuclide has half-life1000 years. Find its continuous annual decay rate.",f"k=−ln2/1000≈{-math.log(2)/1000:.6f} per year, decline magnitude≈{100*math.log(2)/1000:.4f}%/year.","Source #34 radium-226 half-life1590 years, rate ln2/1590.")
add("A nuclide has half-life8 hours. Find continuous hourly decay rate.",f"k=−ln2/8≈{-math.log(2)/8:.5f} per hour.","Source #35 erbium-165 half-life10.4h.")
add("An artifact has 50% remaining isotope with half-life5700 years. Find estimated age.","t=5700 years under constant-decay model.","Source #36 C14 remaining60%, age≈5730 ln(.6)/ln(.5) years.")
add("Bacteria start at500 and double every30 minutes. Find count after2h.","4 doublings, 500·2^4=8000 bacteria.","Source #37 1350 initial double20min, after3hours nine doublings.")
add("For population P(5)=200, P(15)=800 fit P(t)=A e^(kt).",f"k=ln4/10≈{math.log(4)/10:.6f}/min; A=200e^(−5k)={100:.0f}.","Source #38 source P5=360,P20=1000, infer A by exp interpolation.")
add("For previous P(t), find doubling time.","T=ln2/k=5 minutes.","Source #39 from same source P5,P20, doubling time15ln2/ln(1000/360).")
# Cooling suite
add("Liquid starts100°F, room70°F; after10min it is90°F. Find cooling model.",f"T(t)=70+30e^(−kt), k=ln(3/2)/10≈{math.log(1.5)/10:.6f}/min.","Source #40 soup 100°F, room69°, after15min95°, fit k.")
add("Using that model, when does liquid reach80°F?","30e^(−kt)=10; t=ln3/k≈27.095min.","Source #41 find time soup cools to80°F with source fitted k.")
add("Using same model, find temperature after60min.",f"T(60)=70+30(2/3)^6≈{70+30*(2/3)**6:.0f}°F.","Source #42 soup temperature at2.5h (150min).")
add("Food begins160°F in a 70°F room; after20min it is130°F. Write cooling model.","T(t)=70+90e^(−kt), k=ln(3/2)/20 per minute.","Source #43 turkey 165°F room75°, after30min145°, solve cooling rate.")
add("Using that food model find temperature after40min.","70+90(2/3)^2=110°F.","Source #44 source turkey temp after50min.")
add("Using that food model, find time to reach100°F.","90e^(−kt)=30 ->t=20ln3/ln(3/2)≈54.19 minutes.","Source #45 source turkey t until110°F.")
# Log scale, intensities
add("Find x where log10(x)=−2.","x=10^(−2)=0.010.","Source #46 read numeric x from marked logarithmic axis.")
add("Find x where log10(x)=1.5.",f"x=10^1.5≈{10**1.5:.3f}.","Source #47 read numeric x from second logarithmic-axis figure.")
add("Put sound intensities10^(−8),10^(−2),10^3 W/m² on a base10 log scale.","Positions −8,−2,+3 (log10 of each intensity).","Source #48 sound logarithmic positions whisper10^−10, vacuum10^−4, jet10².")
add("A second seismic event has 100 times the energy of an event magnitude4.0. Find new magnitude.","M2=4+(2/3)log10(100)=5.333...≈5.33.","Source #49 magnitude3.9 with 750 times energy ->3.9+(2/3)log10(750).")
add("For rumor model N(t)=400/(1+39e^(−.5t)), find starting count.","N(0)=400/40=10 people.","Source #50 rumor N=500/(1+49e^(−.7t)), start10.")
add("Using the rumor model, find people informed after4 days.",f"N(4)=400/(1+39e^−2)≈{logistic(400,39,.5,4):.0f}.","Source #51 source rumor after3days.")
add("What value does N(t)=400/(1+39e^(−.5t)) approach?","400 people as t→∞, the carrying capacity.","Source #52 source rumor tends500 people.")
add("A 20mg dye sample falls to5mg in10 min. Choose a continuous exponential model.","A(t)=20e^(kt), k=ln(5/20)/10=−ln4/10 ≈−0.138629/min; A(t)=20e^(−0.138629t).","Source #53 13mg to4.75mg in12min, select decaying exponential with k=ln(4.75/13)/12≈−0.0839.")
assert len(Q)==53 and [x["SourceExercise"]for x in Q]==list(range(1,54))
# Correct the one hand-computed cooling comparison from exact math
Q[40]["TeacherMath"]=f"30e^(−kt)=10; t=10ln3/ln(3/2)≈{10*math.log(3)/math.log(1.5):.3f} minutes."
Q[44]["TeacherMath"]=f"90e^(−kt)=30; t=20ln3/ln(3/2)≈{20*math.log(3)/math.log(1.5):.3f} minutes."
assert math.isclose(logistic(300,5,.4,0),50)
assert math.isclose(logistic(400,39,.5,0),10)
assert math.isclose(100*math.exp((math.log(.25)/20)*0),100)
assert 64*(.5)**4==4
assert math.isclose(100*(.8)**3,51.2)
assert len(examples)==8 and len(tries)==7
W=[
("Graph exponential growth","For f(t)=15e^(0.3t), find initial level, direction and f(4).",f"f(0)=15, increasing; f(4)=15e^1.2≈{15*math.exp(1.2):.2f}."),
("Radioactive decay model","A 100g sample has half-life5 days. Find A(t).","A(t)=100(1/2)^(t/5)."),
("Find sample age","Half-life100 years, remaining fraction0.3. Find age.",f"t=100ln(.3)/ln(.5)≈{100*math.log(.3)/math.log(.5):.0f} years."),
("Doubling-time model","A bacterial count120 doubles every4h. Model and find at12h.","P(t)=120·2^(t/4); P(12)=960."),
("Newton cooling","Temperature initially95°, room25°, after5min it is60°. Find decay rate.","e^(−5k)=(60−25)/(95−25)=1/2; k=ln2/5 per minute."),
("Logistic growth","P=600/(1+5e^(−.7t)); initial and carrying capacity.","P(0)=100; carrying capacity600."),
("Choose model from data","x=1,2,4,8; y=0,2,4,6. Choose model.","Logarithmic y=2log_2(x)."),
("Convert model to e base","Rewrite 9(1.2)^t using e.","9e^((ln1.2)t).")
]
T=[
("Give model for100g, half-life4h.","100(1/2)^(t/4)."),
("Time until25% remains if half-life8 years.","16 years."),
("Doubling time for P(t)=20e^(0.1t).",f"ln2/.1≈{math.log(2)/.1:.3f} time units."),
("Cool liquid50° in room70°; after1h it is60°. When will it be65°?","T=70−20·(1/2)^t; reach65 after2 hours."),
("Estimate P(2) for P=200/(1+3e^(−.8t)).",f"≈{logistic(200,3,.8,2):.1f}."),
("Classify data x=1,2,3 y=4,8,16.","Exponential f(x)=2·2^x."),
("Rewrite f(x)=4(0.7)^x in e-base form.","4e^((ln0.7)x).")
]
assert len(W)==8 and len(T)==7
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L07"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l07.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l07-firstparty-teacher-led.json"
visuals=[]
for n,label,points in [
(1,"Independent radioactive inventory starting100g, half-life2h",["100","50","25","12.5"]),
(2,"Independent growth 2e^(0.5t)",["2","3.297","5.437","8.963"]),
(3,"Independent decay 3e^(−0.5t)",["3","1.820","1.104","0.669"]),
(4,"Growth/decay compared at time t",["8","12","18","27"]),
(5,"Independent doubling 10·2^t",["10","20","40","80"]),
(6,"Logistic carrying capacity200",[f"{logistic(200,4,.5,t):.3f}" for t in range(4)]),
(7,"Logistic 300/(1+9e^(−t))",[f"{logistic(300,9,1,t):.3f}" for t in range(4)]),
(8,"Logarithmic sample points x=1,2,4,8",["0","1","2","3"]),
(9,"Logarithmic model 2log₂ x",["0","2","4","6"]),
(10,"Positive-side ln(x²) sample",["0","1.386","2.773","4.159"]),
(11,"Negative-side ln(x²) sample",["0","1.386","2.773","4.159"])
]:
 visuals.append({"Kind":"RatioTable","Title":label,
 "Description":f"Independently authored table for source Figure {n} mathematical skill; not publisher image.",
 "PrimaryLabel":"Input sample","SecondaryLabel":"Modeled output",
 "Rows":[{"Label":"Input","Cells":(["1","2","4","8"] if n in (8,9,10) else (["−1","−2","−4","−8"] if n==11 else ["0","1","2","3"]))},{"Label":"Value","Cells":points}],
 "Items":[],"SecondaryItems":[]})
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L07",
"ContentVersion":"g12-u04-l07-firstparty-models-abc",
"Lessons":[{"LessonCode":lesson_code,"CultureCode":"en","Title":"Exponential and Logarithmic Models",
"ExplanationParagraphs":["Exponential growth/decay f(t)=A0e^(kt) has initial A0>0 and rate k, positive for growth, negative for decay.",
"Half-life T>0 leads to A(t)=A0(1/2)^(t/T). Doubling time for growth k>0 is ln2/k.",
"Newton's cooling is T(t)=Ts+(T0−Ts)e^(−kt); the sign of T0−Ts determines approach direction.",
"Logistic growth L/(1+ae^(−kt)), L>0, a>0, k>0, approaches carrying capacity L; time to double depends on starting point.",
"Fit a model to observed data by using constant ratios, constant differences, or logarithmic shape. Do not extrapolate beyond meaningful physical conditions.",
"With b>0, b!=1, b^t=e^((ln b)t); y=ln(x²) is defined for x≠0, while 2lnx requires x>0.",
"Orders of magnitude compare multiplicative factors of 10. Plot powers of ten by their common logarithms.",
"Independent Cool-down: (a) 50g with half-life4h at12h, (b) find a logistic carrying capacity, (c) compare domains of ln(x²) and2lnx."]+
[f"Independent Try It {i}: {q}" for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Choose exponential, decay, logistic or logarithmic representation and preserve physical domain.","Steps":[ans],"Answer":ans,"Check":"Reevaluate input values, units, and limiting behavior."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Exponential models","Definition":"f(t)=A0e^(kt), A0>0.","Rule":"Positive k growth; negative k decay.","Example":"20e^(−0.2t)."},
{"Title":"Half-life and doubling","Definition":"T_half=ln2/|k| for exponential decay; T_double=ln2/k for growth.","Rule":"Use correct units and positive time.","Example":"Four half-lives leaves1/16."},
{"Title":"Logistic capacity","Definition":"P(t)=L/(1+ae^(−kt)), L,a,k>0.","Rule":"P(t) approaches L, not infinity.","Example":"200/(1+4e^(−t))."},
{"Title":"Model fitting","Definition":"Choose linear, exponential, logarithmic or logistic according to observed change and context.","Rule":"Verify domain and recognize extrapolation limits.","Example":"log₂(x) fits doubling x with additive y increments."}],
"Visuals":visuals,"Videos":[],"SummaryPoints":["Growth and decay rates determine time behavior.","Half-life uses fraction1/2.","Newton cooling approaches room temperature.","Logistic models have carrying capacity.","Select models from data and interpret log scale."],
"CommonMistakes":[{"Mistake":"Treat logistic doubling time as constant.","WhyWrong":"Growth slows near carrying capacity.","Correction":"Solve for target population using the specific logistic equation."},
{"Mistake":"Treat an indefinite exponential forecast as guaranteed.","WhyWrong":"Finite resources and changing physical conditions invalidate long-term extrapolation.","Correction":"State model and valid context."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson_code,
"FirstPartyIndependentStudentMathQuestions":Q,"OriginalSourceExercisesReviewed":53,
"IndependentTryIt":[{"Number":n,"SourceTryItReference":tries[n-1],"Question":q,"TeacherExpectedMath":ans}for n,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["50g sample has half-life4h; how much at12h?","Find carrying capacity of P(t)=700/(1+6e^(−.2t)).","Compare real domains ln(x²) and2lnx."],
"TeacherExpectedMath":["50/8=6.25g.","700.","ln(x²) defined x≠0; 2lnx only x>0."],"OriginalPublisherCoolDownCopied":False},
"StageC":"REUSE_RICHV2_TEACHER_MANUAL_MATH_AND_RATIO_TABLES","StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich);save(root/teacher_rel,teacher)
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l07-eleven-source-figure-skill-alternatives.json"
fig_roles=["Original credited research-reactor photo replaced by mathematical decay table",
"Exponential growth graph","Exponential decay graph","Growth-decay conceptual comparison",
"Doubling growth graph","Logistic capacity and inflection","Logistic case model fit",
"Observed log-form data scatter","Logarithmic curve data fit","ln(x²) positive-domain branch",
"ln(x²) negative-domain branch"]
save(root/figure_rel,{"SchemaVersion":1,"Section":"4.7","SourceFigureCount":11,
"FirstPartyIndependentFigureSkillModels":[{"SourceFigure":i+1,"Skill":role,"Alternative":"Independent student mathematical table",
"SourceImageCopied":False,"PublisherPixelEquivalenceClaimed":False}for i,role in enumerate(fig_roles)],
"AuthoredTabularAlternatives":11,"GraphRuntimeVerified":False})
routes_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l07-stage-c-53-teacher-review-routes.json"
save(root/routes_rel,{"SchemaVersion":1,"Count":53,"Decision":"REUSE_EXISTING_RICHV2_TEACHER_SCORING_AND_RATIO_TABLE",
"NewMathEngine":False,"Routes":[{"Source":i,"TeacherMathPresent":True,"FirstPartyQuestion":True,
"RuntimeVerified":False}for i in range(1,54)],"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l07-abc-close-audit.json"
save(root/closure_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":7,
"OriginalSource":"OpenStax Precalculus 2017 original local PDF",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[416,430],
"SourceWorkedExamples":8,"SourceTryItNumbers":tries,"SourceFigures":11,"NumberedExercises":53,
"OriginalMathematicsSourceSkillReviewed":53,"IndependentWorked":8,"IndependentTryIt":7,
"IndependentQuestionsAndKeys":53,"FigureSkillAlternatives":11,"FigureAlternativeMode":"FIRST_PARTY_MATH_TABLES",
"PublisherOriginalImageCopied":False,"RuntimeGraphVerified":False,"NewMathEngine":False,
"CCSSCandidateCodes":["CCSS:HSF-LE.A.1","CCSS:HSF-LE.A.2"],
"FormalCCSSAlignmentCertified":False,
"ChecksActuallyRun":["Local source boundaries+numbered source exercises",
"53 source skill/math answer cases","8 worked 7 Try It source skill alternatives",
"11 independent mathematical figure skill tables","Stage C teacher routes","Numeric model spot-check",
"Git diff check"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now})
audit_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l07-source-review-and-gap-audit.json"
save(root/audit_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":7,"Section":"4.7",
"Title":"Exponential and Logarithmic Models","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[416,430],"IncorrectBlueprintPages":[417,431],
"OriginalWorkedExamples":[{"Number":n,"Title":title}for n,title in examples],
"OriginalTryItNumbers":tries,"SourceFigures":fig,
"OriginalSourceMathExercises":[{"Number":q["SourceExercise"],"SourceMathReviewed":True,"TeacherMathVerified":True}for q in Q],
"FirstPartyRichV2":rich_rel,"FirstPartyTeacherAssessment":teacher_rel,"FigureSkillEvidence":figure_rel,
"StageCRoutes":routes_rel,"ReadyForStageD":True,"Status":"READY_FOR_STAGE_D",
"BlockingReasons":[],"ClosureEvidencePath":closure_rel,"CI":"NOT_RUN","StageD":"NOT_RUN",
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert row["Ordinal"]==27 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":416,"PdfPageEnd":430,
"BlockingReasons":[],"SourceExerciseMathReviewed":53,"FirstPartyTeacherRubrics":53,
"FigureSkillAlternatives":11,"EvidencePath":audit_rel,"ClosureEvidencePath":closure_rel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][27]["Ordinal"]==28
p.update({"LastClosedLessonOrdinal":27,"FirstIncompleteLessonOrdinal":28,
"ActiveLessonOrdinal":28,"NextLessonOrdinal":28,"ReadyForStageD":27,
"SourceVerifiedGrade12Lessons":27,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_27_OF_73",
"UpdatedUtc":now})
save(p_path,p)
files=[str(p_path.relative_to(root)),rich_rel,teacher_rel,figure_rel,routes_rel,closure_rel,audit_rel,str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l07): close exponential and logarithmic models academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L07_ACADEMIC_A_B_C_CLOSED_PUSH_VERIFIED_27_OF_73")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("ELAPSED",round(time.monotonic()-start,2))
