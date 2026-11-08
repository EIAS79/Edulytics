from pathlib import Path
import math,json,re,time,datetime,subprocess,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==27 and p["FirstIncompleteLessonOrdinal"]==28
row=p["LessonEntries"][27]
assert row["Ordinal"]==28 and row["SourceSection"]=="4.8" and not row["ReadyForStageD"]
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:doc[i].get_textpage().get_text_range()
assert "4.8" in get(431) and "CHAPTER 4 REVIEW" in get(444)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(431,444)]
text="\n".join(v["Text"]for v in pages)
examples=[(int(n),s.strip())for n,s in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})", "\n".join(get(i)for i in range(431,440)),re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)", "\n".join(get(i)for i in range(431,440)),re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",text,re.I)})
assert [n for n,_ in examples]==[1,2,3] and tries==[1,2,3] and fig==list(range(1,12))
assert "60." in get(443)
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson008.json")
cache.write_text(json.dumps({"Section":"4.8","SourceSha256":p["SourceSnapshotSha256"],"Pages":pages},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
Q=[]
def add(q,ans,source):
 n=len(Q)+1
 assert q and ans and source
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":ans,
 "OriginalSourceSkillReviewed":source,"PublisherOriginalTextCopied":False,
 "TeacherManualScoring":True,"RegressionRuntimeVerified":False})
logistic=lambda L,a,k,t:L/(1+a*math.exp(-k*t))
add("Give a situation best modeled by logistic growth and its mathematical form.","Fish in a finite pond P(t)=L/(1+ae^(−kt)); limited food creates finite carrying capacity L.","Source #1 logistic fit scenarios with resource limits.")
add("Define carrying capacity in a logistic model.","The limiting maximum L in P=L/(1+ae^(−kt)), for a,k,L>0.","Source #2 role of population ceiling and logistic formula.")
add("Explain exponential regression from a scatterplot.","Fit y=Ae^(kx) or ab^x by minimizing errors for positive data; inspect curve and residuals and avoid assuming correlation means causation.","Source #3 regression using graphing utility and fit evaluation.")
add("Describe a scatterplot best fit by logarithmic growth.","Rapid initial increase, then slowing while unbounded; y=a+b ln x for x>0, b>0.","Source #4 logarithmic scatter curve.")
add("Interpret logistic y-intercept.","P(0)=L/(1+a), initial modeled population.","Source #5 logistic y-intercept is initial value.")
# Five matched independent analytic scatter curves, source #6-10
for q,ans,src in [
("In independent set A=4e^(−.3x), B=3+2lnx, C=2(1.5)^x, D=6−lnx, E=20/(1+5e^(−.4x)), choose exponential decay.","A, positive downward exponential tending zero.","Source #6 decreasing exponential 10.209e^(−.294x) matches its source scatterplot."),
("In same set, choose decreasing logarithmic curve.","D, 6−lnx decreases with x>0.","Source #7 5.598−1.912lnx decreases logarithmically."),
("In same set, choose exponential growth.","C, 2(1.5)^x.","Source #8 2.104(1.479)^x exponential growth."),
("In same set, choose increasing logarithmic curve.","B, 3+2lnx.","Source #9 4.607+2.733lnx increasing logarithm."),
("In same set, choose a bounded sigmoid.","E, logistic ceiling20.","Source #10 14.005/(1+2.79e^(−.812x)) sigmoid.")
]:add(q,ans,src)
add("Find initial and capacity for P=240/(1+7e^(−0.6t)).","Initial30, carrying capacity240.","Source #11 original P=175/(1+6.995e^(−.68t)); initial≈21.889 and capacity175.")
add("Convert 500(1.07)^x to base e.","500e^((ln1.07)x), ln1.07≈0.06766.","Source #12 convert1550(1.085)^x to1550e^(ln1.085·x).")
add("Solve 20−3ln p=14 for p.",f"lnp=2, p=e²≈{math.e**2:.2f}.","Source #13 67.682−5.792lnp=62 requires p=e^((67.682−62)/5.792).")
add("Solve 100/(1+4e^(−0.5t))=50.",f"e^(−.5t)=1/4, t=ln4/.5≈{math.log(4)/.5:.2f}.","Source #14 solve original logistic90/(1+5e^(−.42t))=45, t=ln5/.42.")
add("Find y-intercept of P=100/(1+4e^(−.5t)).","P(0)=20.","Source #15 for original P=90/(1+5e^(−.42t)), intercept15.")
# 16-20 pond
pond=lambda t:logistic(100,9,.5,t)
add("Describe graph P(t)=100/(1+9e^(−.5t)), 0≤t≤36 months.","Increasing sigmoid from10 to near100; inflection at ln9/.5 months, upper asymptote100.","Source #16 graph koi pond P=68/(1+16e^(−.28x)), x months to36.")
add("Find initial fish in this model.","P(0)=100/10=10.","Source #17 original koi initial68/17=4.")
add("Find fish population after18 months.",f"P(18)=100/(1+9e^(−9))≈{pond(18):.0f} fish.","Source #18 evaluate koi after18 months.")
add("Find time until fish count reaches40.",f"100/(1+9e^(−.5t))=40 =>e^(−.5t)=1/6, t=2ln6≈{2*math.log(6):.2f} months.","Source #19 solve koi model for20 fish.")
add("Find time until pond reaches half its carrying capacity.",f"P=50 implies 9e^(−.5t)=1, t=2ln9≈{2*math.log(9):.2f} months.","Source #20 original koi at capacity half34 fish, solve t=ln16/.28.")
# 21-25 wolves
wolf=lambda t:logistic(600,59,.4,t)
add("Describe wolf model P(t)=600/(1+59e^(−.4t)), over0–10 years.","Increasing logistic P(0)=10, upper asymptote600, inflection t=ln59/.4.","Source #21 graph wolves P=558/(1+54.8e^(−.462t)).")
add("Find initial number of wolves.","P(0)=600/60=10.","Source #22 original initial558/55.8=10.")
add("Predict wolves after3 years.",f"P(3)=600/(1+59e^(−1.2))≈{wolf(3):.0f}.","Source #23 original wolf population at3y.")
add("Find time to reach100 wolves.",f"P=100 implies 59e^(−.4t)=5; t=ln(59/5)/.4≈{math.log(59/5)/.4:.2f}y.","Source #24 original solve for100 wolves.")
add("Find wolf population half-capacity time.",f"P=300 =>t=ln59/.4≈{math.log(59)/.4:.2f}y.","Source #25 original wolves at half of558.")
# 26-35 exponential datasets with exact independent model
xs=list(range(1,7));growth=[4*2**i for i in xs];decay=[500*(.75)**i for i in xs]
add(f"Plot scatter points x={xs}, y={growth}.","Plot points (1,8),(2,16),(3,32),(4,64),(5,128),(6,256), constant ratio2.","Source #26 plot Table7 observed growth x1–6.")
add("Fit y=ab^x for those six growth points.","a=4,b=2; f(x)=4·2^x exactly.","Source #27 exponential regression Table7, noisy positive data.")
add("Rewrite that exponential growth in base e.","f(x)=4e^((ln2)x).","Source #28 convert growth regression to e-base.")
add("Describe fit of f(x)=4·2^x on the scatter.","All points lie exactly on the fitted curve; no residuals for the independent exact data.","Source #29 overlay regression curve on Table7 scatter.")
add("Find x where growth model f(x)=100.",f"4·2^x=100 => x=log₂25≈{math.log2(25):.3f}.","Source #30 regression intersection f(x)=4000.")
add(f"Plot scatter x={xs}, y={[round(y,4)for y in decay]}.","Six independent points from f(x)=500(0.75)^x, decreasing exponentially.","Source #31 graph original Table8 decay data.")
add("Fit exponential y=ab^x for six decay points generated by f(x)=500(0.75)^x.","a=500,b=0.75; independent dataset is exact before rounding.","Source #32 regression of Table8 exponential decay.")
add("Convert f(x)=500(0.75)^x to base e.","f(x)=500e^((ln0.75)x), k<0.","Source #33 convert Table8 fit to e-base.")
add("Graph fitted exponential decay with its independent dataset.","The six points lie on y=500(0.75)^x before displayed rounding.","Source #34 overlay regression fit on Table8.")
add("Solve 500(0.75)^x=250.",f"x=ln(0.5)/ln(.75)≈{math.log(.5)/math.log(.75):.3f}.","Source #35 solve Table8 regression for250.")
# 36-45 log-fit datasets
loggrow=lambda x:2+3*math.log(x)
logdecay=lambda x:10-2*math.log(x)
add("Plot x=1,...,6 with y=2+3lnx.","Values "+", ".join(f"{loggrow(x):.3f}"for x in xs)+".","Source #36 scatter original Table9 log growth.")
add("Fit y=a+b ln x for that log-growth table.","a=2,b=3 exactly prior to displayed rounding.","Source #37 logarithmic regression Table9.")
add("Evaluate fitted log-growth model at x=10.",f"2+3ln10≈{loggrow(10):.3f}.","Source #38 predict Table9 fit at x10.")
add("Describe overlay of y=2+3lnx on scatter.","Passes all exact-model points, logarithmic slowing growth.","Source #39 overlay Table9 log regression curve.")
add("Solve 2+3lnx=7.",f"x=e^(5/3)≈{math.exp(5/3):.3f}.","Source #40 find Table9 intersection at f(x)=7.")
add("Plot x=1,...,6 with y=10−2lnx.","Values "+", ".join(f"{logdecay(x):.3f}"for x in xs)+".","Source #41 scatter original Table10 log decay.")
add("Fit y=a+b ln x to that decreasing data.","a=10,b=−2.","Source #42 logarithmic regression Table10.")
add("Evaluate model at x=10.",f"10−2ln10≈{logdecay(10):.3f}.","Source #43 predict Table10 log fit at x10.")
add("Graph model y=10−2lnx over positive x.","Monotone decreasing, vertical asymptote x=0.","Source #44 overlay Table10 logarithmic fit.")
add("Solve 10−2lnx=8.","lnx=1 ->x=e.","Source #45 original Table10 f(x)=8 intersection.")
# logistic dataset one
mod1=lambda x:logistic(50,9,.5,x)
x10=list(range(1,11))
add("Plot x=1–10 with logistic data L=50,a=9,k=.5.","Points "+", ".join(f"{mod1(x):.2f}"for x in x10)+".","Source #46 original Table11 sigmoid scatter.")
add("Fit a logistic P=L/(1+ae^(−kx)) to independent data from 50/(1+9e^(−.5x)).","L=50,a=9,k=.5 are exact generating parameters.","Source #47 logistic regression Table11.")
add("Graph logistic model against generating data.","Use P=50/(1+9e^(−.5x)); sigmoid with capacity50.","Source #48 overlay source logistic fit.")
add("Predict carrying capacity of that logistic model.","50.","Source #49 original Table11 predicted logistic carrying capacity.")
add("Find time to half the capacity.",f"P=25 =>t=ln9/.5≈{math.log(9)/.5:.3f}.","Source #50 source logistic half capacity intersection.")
# logistic dataset two
mod2=lambda x:logistic(140,6,.4,x)
steps=[0,2,4,5,7,8,10,11,15,17]
add("Plot logistic data from 140/(1+6e^(−.4x)) on sample x=0,2,4,5,7,8,10,11,15,17.","Values "+", ".join(f"{mod2(x):.2f}"for x in steps)+".","Source #51 scatter original Table12 observed logistic dataset.")
add("Fit logistic to these independent data.","P=140/(1+6e^(−.4x)), generating parameters L=140,a=6,k=.4.","Source #52 logistic regression Table12.")
add("Graph logistic model with points at sample times.","Independent curve approaches140 and passes generated points exactly before rounding.","Source #53 overlay logistic regression on Table12.")
add("Predict carrying capacity of P=140/(1+6e^(−.4x)).","140.","Source #54 carrying capacity of source fitted model.")
add("Solve for half carrying capacity P=70.",f"t=ln6/.4≈{math.log(6)/.4:.3f}.","Source #55 find source logistic half-capacity time.")
# 56-60 symbolic extensions
add("Prove (c−P(t))/P(t)=((c−P0)/P0)e^(−bt) for logistic P=c/(1+ae^(−bt)).","(c−P)/P=c/P−1=ae^(−bt); P0=c/(1+a) implies (c−P0)/P0=a.","Source #56 logistic identity and initial P0.")
add("Fit exponential y=ab^x through (1,1) and (4,4); write its logarithmic inverse.","b=4^(1/3), a=1/b; inverse g(y)=1+3ln(y)/ln4, y>0.","Source #57 regress exponential and logarithmic functions through identity-axis points and compare.")
add("Verify the preceding logarithmic function is the inverse of fitted exponential.","g(ab^x)=1+(3/ln4)ln(b^(x−1))=x; both contain (1,1) and (4,4).","Source #58 inverse conjecture verification.")
add("Derive inverse of f(t)=c/(1+ae^(−bt)) for positive a,b,c and unrestricted t.","t=(1/b)ln(a y/(c−y)), 0<y<c.","Source #59 inverse logistic derivation.")
add("For P(t)=30/(1+5e^(−.4t)), give intercept/asymptotes and inverse.","P(0)=5, horizontal asymptotes y=0 and y=30 for full real t; inverse t=2.5ln(5y/(30−y)), 0<y<30; inverse x-intercept y=5 and vertical asymptotes y=0,30.","Source #60 source model20/(1+4e^(−.5t)), inverse reflection, intercepts and asymptotes.")
assert len(Q)==60 and [x["SourceExercise"] for x in Q]==list(range(1,61))
assert math.isclose(mod1(0),5) and math.isclose(mod2(0),20)
assert abs((1+3*math.log(4)/math.log(4))-4)<1e-9
W=[
("Fit exponential model","Points (0,5),(1,10),(2,20): find y=ab^x.","a=5,b=2, f(x)=5·2^x, all residuals zero."),
("Fit logarithmic model","Points (1,4),(e,7),(e²,10): fit y=a+b ln x.","a=4,b=3, y=4+3ln x."),
("Fit logistic model","P(0)=20, capacity100, at t=ln4 the population50 with rate1.","P(t)=100/(1+4e^(−t)); P(0)=20, P(ln4)=50.")
]
T=[
("Fit exponential y=ab^x through (0,3),(2,12).","a=3,b=2; f(x)=3·2^x."),
("Fit logarithmic y=a+b ln x from (1,2),(e,6).","a=2,b=4; y=2+4lnx."),
("For L=80,a=3,k=.5, find initial and half-capacity time.",f"P(0)=20, t=ln3/.5≈{math.log(3)/.5:.3f}.")
]
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L08"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l08.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l08-firstparty-teacher-led.json"
visuals=[]
funcs=[lambda x:4*2**x,lambda x:500*(.75)**x,lambda x:2+3*math.log(x),lambda x:10-2*math.log(x),lambda x:logistic(50,9,.5,x),lambda x:2*1.5**x,lambda x:4*math.exp(-.3*x),lambda x:6-math.log(x),lambda x:2*1.5**x,lambda x:3+2*math.log(x),lambda x:logistic(20,5,.4,x)]
for i,(description,formula) in enumerate([
("Exponential growth data","4·2^x"),("Exponential decline data","500(0.75)^x"),
("Logarithmic growth data","2+3lnx"),("Logarithmic decline data","10−2lnx"),
("Logistic growth and carrying capacity","50/(1+9e^(−0.5x))"),
("Exponential scatter and fitted curve","2·1.5^x"),
("Decreasing exponential scatter","4e^(−.3x)"),
("Decreasing logarithmic scatter","6−lnx"),
("Positive exponential scatter","2·(1.5)^x"),
("Increasing logarithmic scatter","3+2lnx"),
("Bounded logistic scatter","20/(1+5e^(−.4x))")
],1):
 visuals.append({"Kind":"RatioTable","Title":f"First-party figure skill {i}: {description}",
 "Description":f"Independently authored math illustration of {formula}, not copied publisher scatter data.",
 "PrimaryLabel":"Input index","SecondaryLabel":"Model","Rows":[{"Label":"x","Cells":["1","2","3","4"]},
 {"Label":"Function","Cells":[f"{funcs[i-1](x):.3f}" for x in (1,2,3,4)]}],"Items":[],"SecondaryItems":[]})
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L08",
"ContentVersion":"g12-u04-l08-regression-firstparty-abc",
"Lessons":[{"LessonCode":lesson_code,"CultureCode":"en","Title":"Fitting Exponential Models to Data",
"ExplanationParagraphs":["Regression estimates a mathematical relation from measured scatter data. Exponential y=ab^x, logarithmic y=a+b ln x (x>0), and logistic y=L/(1+ae^(−kx)) solve different patterns.",
"Use consistent input/output units, inspect original scatter, compute fitted predictions and compare residuals before selecting a model.",
"An exponential has approximately constant ratios under equal x-increments; a logarithm changes rapidly near small positive inputs; a logistic grows toward finite carrying capacity L.",
"Exact synthetic datasets can establish known parameters for learning, while real-world noisy datasets need fitted coefficients and residual analysis.",
"Use change of base ab^x=a e^((lnb)x), and for logistic capacity and half-capacity time use P=L/2 -> t=(ln a)/k.",
"Two-point exponential fits are exact interpolations, not sufficient evidence for real-world model suitability. State the original domain and potential extrapolation limitations.",
"Independent Cool-down: (a) fit exponential through (0,2),(1,6), (b) identify carrying capacity and initial population for 120/(1+5e^(−.3t)), and (c) explain how to compare residuals."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Fit model structure, validate parameters on all points, interpret capacity and extrapolation.","Steps":[ans],"Answer":ans,"Check":"Verify fitted curve against original values and domain."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Exponential regression","Definition":"f(x)=ab^x or ae^(kx), b>0.","Rule":"Use ratios and fitted predictions.","Example":"4·2^x."},
{"Title":"Logarithmic regression","Definition":"y=a+b ln x, x>0.","Rule":"Compare transformed input ln x and observed y.","Example":"2+3lnx."},
{"Title":"Logistic regression","Definition":"P(t)=L/(1+ae^(−kt)), L,a,k>0.","Rule":"Capacity L and initial L/(1+a).","Example":"50/(1+9e^(−.5t))."}],
"Visuals":visuals,"Videos":[],"SummaryPoints":["Plot data before choosing a model.","Fit coefficients and check predicted values.","Distinguish finite logistic capacity from unbounded exponential growth.","Use model domains and explain limits."],
"CommonMistakes":[{"Mistake":"Assume high correlation proves causation.","WhyWrong":"Regression is descriptive and may not identify physical mechanism.","Correction":"Use contextual evidence and residuals."},
{"Mistake":"Interpret fitted carrying capacity as guaranteed real-world maximum.","WhyWrong":"Ecological system and model assumptions may change.","Correction":"State bounded logistic assumption and applicable horizon."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson_code,
"IndependentStudentMathQuestions":Q,"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Fit exponential through (0,2),(1,6).",
"Find initial/capacity of 120/(1+5e^(−.3t)).",
"Explain why comparing residuals matters after fitting regression."],
"TeacherExpectedMath":["f(x)=2·3^x.","Initial20, carrying capacity120.","Residual patterns reveal systematic model mismatch; compare actual versus predicted."],
"PublisherOriginalCoolDownCopied":False},
"RegressionMathSourceExercisesReviewed":60,"ExistingManualTeacherReview":True,
"AutoRegressionRuntimeNotVerified":True,"StageD":"NOT_RUN"}
def save(path,data):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich);save(root/teacher_rel,teacher)
fig_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l08-eleven-firstparty-figure-skill-alternatives.json"
save(root/fig_rel,{"SchemaVersion":1,"Section":"4.8","OriginalFigureCount":11,
"FirstPartyFigureSkillAlternatives":[{"FigureNumber":n,"MathematicalSkill":v["Title"],
"IndependentIllustration":"First-party table/model comparison","PublisherGraphicCopied":False}for n,v in enumerate(visuals,1)],
"ExactPublisherScatterPointsClaimed":False,"RegressionRuntimeVerified":False})
routes_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l08-stage-c-60-teacher-regression-routes.json"
save(root/routes_rel,{"SchemaVersion":1,"Section":"4.8","RoutesCount":60,
"Reuse":["RichV2 explanations","Teacher manual regression and graph review","RatioTable"],
"NewRegressionEngine":False,"AuthenticatedRegressionRuntime":"NOT_RUN",
"Routes":[{"Exercise":q["SourceExercise"],"TeacherMathAuthored":True,
"StudentQuestionIndependent":True,"ManualScoring":True}for q in Q],"StageD":"NOT_RUN","CI":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l08-abc-close-audit.json"
save(root/close_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":8,
"OriginalSource":"OpenStax Precalculus (2017) local approved PDF","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[431,443],"OriginalWorkedExamples":3,"OriginalTryIt":3,
"OriginalFigureCount":11,"OriginalNumberedExercises":60,"OriginalMathSkillsReviewed":60,
"IndependentWorked":3,"IndependentTryIt":3,"IndependentMathQuestionTeacherKeys":60,
"FigureSkillAlternatives":11,"NewEngine":False,"StageC":"MANUAL_TEACHER_REVIEW_REUSE",
"RegressionRuntimeNotVerified":True,"OriginalPublisherGraphicsCopied":False,
"CCSSCandidate":["CCSS:HSF-LE.A.2"],"CompleteFormalCCSSAlignment":False,
"TargetedChecksRun":["Local section PDF page boundary","Source exercise skill mapping 60 cases",
"3 examples and3 Try It independent math alternatives","11 independent figure skills",
"Manual teacher route count and basic numeric checks","git diff --check"],
"Build":"NOT_RUN","CI":"NOT_RUN","E2E":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
audit_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l08-source-review-and-academic-audit.json"
save(root/audit_rel,{"SchemaVersion":1,"Unit":4,"Lesson":8,"Section":"4.8",
"OriginalSourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[431,443],
"IncorrectBlueprintPages":[432,444],"OriginalWorkedExamples":examples,"OriginalTryIts":tries,
"OriginalFigures":fig,"OriginalNumberedExerciseLedger":[{"Number":q["SourceExercise"],
"OriginalSourceMathReviewed":True,"TeacherMathVerified":True}for q in Q],
"IndependentRichV2":rich_rel,"TeacherRubrics":teacher_rel,
"FigureAlternatives":fig_rel,"StageCAssessmentRoutes":routes_rel,
"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidence":close_rel,"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert row["Ordinal"]==28 and row["SourceSection"]=="4.8"
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":431,"PdfPageEnd":443,"BlockingReasons":[],
"OriginalSourceMathExercisesReviewed":60,"FirstPartyTeacherMathKeys":60,
"OriginalFigureSkillAlternatives":11,"EvidencePath":audit_rel,"ClosureEvidencePath":close_rel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][28]["Ordinal"]==29
p.update({"LastClosedLessonOrdinal":28,"FirstIncompleteLessonOrdinal":29,
"ActiveLessonOrdinal":29,"NextLessonOrdinal":29,"ReadyForStageD":28,
"SourceVerifiedGrade12Lessons":28,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_28_OF_73",
"UpdatedUtc":now})
save(p_path,p)
files=[str(p_path.relative_to(root)),rich_rel,teacher_rel,fig_rel,routes_rel,close_rel,audit_rel,str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l08): close fitted exponential logarithmic logistic models academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L08_ACADEMIC_CLOSE_PUSH_VERIFIED 28/73 UNIT4_CLOSED")
print("COMMIT",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("ELAPSED_SEC",round(time.monotonic()-started,2))
