from pathlib import Path
import json,math
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson004.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.4" and (source["Pages"][0]["PdfPage"],source["Pages"][-1]["PdfPage"])==(190,201)
def ls(points):
 assert len(points)>=3
 xs=[float(v[0])for v in points];ys=[float(v[1])for v in points]
 xm=sum(xs)/len(xs);ym=sum(ys)/len(ys)
 sxx=sum((v-xm)**2 for v in xs);syy=sum((v-ym)**2 for v in ys)
 sxy=sum((x-xm)*(y-ym)for x,y in zip(xs,ys))
 assert sxx>0 and syy>0
 slope=sxy/sxx;intercept=ym-slope*xm;r=sxy/math.sqrt(sxx*syy)
 return slope,intercept,r
def fmt(v):return f"{v:.6f}".rstrip("0").rstrip(".")
items={}
def put(n,skill,q,expected,actions=None):
 assert n not in items,n
 items[n]={"Number":n,"OriginalSourcePDFPage":197 if n<=13 else 198 if n<=19 else 199 if n<=26 else 200 if n<=36 else 201,
 "SourceLearningSkill":skill,"IndependentFirstPartyStudentQuestion":q,
 "TeacherExpectedMathematicsAndAnalysis":expected,
 "SourceNamedCognitiveActions":actions or ["calculate and justify source-skill conclusion"],
 "VerbatimPublisherSourceProseCopied":False,"OriginalPublisherFigureReused":False,
 "GradeMode":"TEACHER_REVIEW_AUTHORED_RUNTIME_NOT_RUN"}
verbal=[
("What is a model breakdown and how could a residual plot reveal it?","Model breakdown means the assumed relation fails for new conditions or is systematically misspecified; curved residual patterns warn against a single linear fit."),
("What is interpolation within a data sample?","Predicting y for x inside the observed x-range."),
("What is extrapolation and why may it be unreliable?","Predicting outside observed x-range; trend may not continue."),
("Explain differences between r>0 and r<0 for a scatterplot.","Positive r means upward association, negative r downward association."),
("What does |r| near 1 versus near 0 mean?","Magnitude near 1 implies strong linear association, near0 weak linear association; neither proves causation.")
]
for n,(prompt,expected)in enumerate(verbal,1):put(n,"INTERPRET_SCATTER_REGRESSION",prompt,expected)
put(6,"EVALUATE_REPORTED_REGRESSION",
    "An independently reported regression model has slope −1.341, intercept 32.234 and r=−0.896. Predict y at x=11 and interpret correlation sign.",
    "ŷ(11)=−1.341×11+32.234=17.483; r negative strong linear association.",
    ["predict from stated line","interpret r sign/strength"])
put(7,"EVALUATE_REPORTED_REGRESSION",
    "A tree age model has slope 6.301 years/inch, intercept −1.044 years and reported r=−0.970. Predict age at 10 inches and identify any apparent inconsistency.",
    "ŷ(10)=61.966 years. An observed regression with positive slope and negative r is internally inconsistent (when x,y variances positive, r and slope share sign); report rather than silently trusting the pair.",
    ["predict from slope/intercept","critical consistency check r against slope"])
scatter={
8:([0,2,4,6,8,10],[-22,-19,-15,-11,-6,-2]),
9:([1,2,3,4,5,6],[46,50,59,75,100,136]),
10:([100,250,300,450,600,750],[12,12.6,13.1,14,14.5,15.2]),
11:([1,3,5,7,9,11],[1,9,28,65,125,216])
}
for n,(xs,ys)in scatter.items():
 m,b,r=ls(list(zip(xs,ys)))
 judgement=("roughly linear"if n in (8,10) else "curved/nonlinear")
 put(n,"DRAW_SCATTER_AND_IDENTIFY_TREND",
     f"Graph the independent coordinate pairs x={xs}, y={ys}. Does the plotted relation look approximately linear or clearly curved? Justify, not merely fit a line.",
     f"Visual pattern {judgement}; optional diagnostic least-squares m≈{fmt(m)}, b≈{fmt(b)}, r≈{fmt(r)}. High correlation alone does not eliminate curvature.",
     ["draw paired scatter points","assess linear vs nonlinear visual shape"])
pop=[(1990,11500),(1995,12100),(2000,12700),(2005,13000),(2010,13750)]
m,b,r=ls(pop)
put(12,"EXTRAPOLATE_LINEAR_SCATTER_THRESHOLD",
    "A first-party-rephrased town has paired year/population records "+str(pop)+". Is estimating year for 15000 people interpolation or extrapolation? Fit a visually reasonable trend and estimate the year.",
    f"Extrapolation (15000 exceeds observed max13750); least squares y≈{fmt(m)}·year+({fmt(b)}), gives year≈{fmt((15000-b)/m)}; eyeballed line may vary.",
    ["plot scatter","classify extrapolation","estimate threshold year"])
temp=[(16,46),(18,50),(20,54),(25,55),(30,62)]
m,b,r=ls(temp)
put(13,"INTERPOLATE_SCATTER_TEMPERATURE",
    "A first-party temperature/time scatter dataset has temperature x=[16,18,20,25,30]°F and measured duration y=[46,50,54,55,62]sec. Estimate duration at 28°F and classify prediction.",
    f"Interpolation, because 28°F lies inside [16,30]; least squares time≈{fmt(m)}x+({fmt(b)}), prediction at28°F≈{fmt(m*28+b)} seconds.",
    ["draw scatter","predict within observed data window"])
for n,r in [(14,0.95),(15,-0.89),(16,0.26),(17,-0.39)]:
 desc=("strong positive"if r>0.8 else "strong negative"if r<-0.8 else "weak positive"if r>0 else "weak-to-moderate negative")
 put(n,"MATCH_CORRELATION_TO_INDEPENDENT_SCATTER",
     f"A separate first-party scatterplot has a reported sample Pearson coefficient r={r}. Describe expected plot direction and linear strength; select the correct qualitative panel among increasing/decreasing and tight/diffuse options.",
     f"Expected {desc} linear association; a positive sign rises, negative falls, magnitude determines concentration.",
     ["interpret r sign","interpret |r|","choose appropriate graph pattern"])
graphdata={
18:[(0,1),(2,3),(4,6),(6,8),(8,10)],
19:[(0,10),(2,8),(4,7),(6,4),(8,2)],
20:[(0,4),(2,6),(4,7),(6,7),(8,5)],
21:[(0,2),(2,5),(4,9),(6,12),(8,17)]
}
for n,points in graphdata.items():
 m,b,r=ls(points)
 put(n,"DRAW_LINE_OF_BEST_FIT_FROM_INDEPENDENT_POINTS",
     f"An independent scatter plot consists of labelled coordinates {points}. Plot it, estimate a best-fit line, and compare its slope to the least-squares slope.",
     f"Least-squares m≈{fmt(m)}, b≈{fmt(b)}, r≈{fmt(r)}; a reasonable hand-drawn line may differ.",
     ["plot points","draw reasonable trendline","report approximate equation"])
trends={
22:([(1990,21.3),(1992,21.4),(1994,22.2),(1996,23.6),(1998,24.4),(2000,25.6),(2002,26.7),(2004,27.7),(2006,28),(2008,29.4)],35,"percent graduates"),
23:([(1992,2665),(1994,2688),(1996,3565),(1998,4129),(2000,4584),(2002,5655),(2004,6549),(2006,7950),(2008,8487),(2009,9462)],12000,"hectoliters"),
24:([(1990,750),(1992,670),(1994,650),(1996,605),(1998,550),(2000,510),(2002,460),(2004,420),(2006,380),(2008,320)],5,"unemployed residents")
}
for n,(points,threshold,unit)in trends.items():
 m,b,r=ls(points)
 t=(threshold-b)/m
 comparison="exceeds"if m>0 else "reaches"
 put(n,"HISTORICAL_TREND_REGRESSION_THRESHOLD",
     f"Evaluate whether paired historical observations {points} are roughly linear. Under that explicit modeling assumption, estimate the calendar year when predicted {unit} {comparison} {threshold}.",
     f"Least squares m≈{fmt(m)}, intercept≈{fmt(b)}, r≈{fmt(r)}; equality near year {fmt(t)}; confirm strict crossing if exceed, and flag long-horizon extrapolation.",
     ["fit linear model","evaluate correlation/fit","solve threshold year with caution"])
datasets={
25:([8,15,26,31,56],[23,41,53,72,103]),
26:([5,7,10,12,15],[4,12,17,22,24]),
27:(list(range(3,19)),[21.9,22.22,22.74,22.26,20.78,17.6,16.52,18.54,15.76,13.68,14.1,14.02,11.94,12.76,11.28,9.1]),
28:(list(range(4,14)),[44.8,43.1,38.8,39,38,32.7,30.1,29.3,27,25.8]),
29:([21,25,30,31,40,50],[17,11,2,-1,-18,-40]),
30:([100,80,60,55,40,20],[2000,1798,1589,1580,1390,1202]),
31:([900,988,1000,1010,1200,1205],[70,80,82,84,105,108])
}
for n,(xs,ys)in datasets.items():
 m,b,r=ls(list(zip(xs,ys)))
 put(n,"COMPUTE_LEAST_SQUARES_AND_PEARSON_R",
     f"Compute the least-squares linear regression y=mx+b and Pearson correlation r to three decimals for independent paired observations: x={xs}, y={ys}. Interpret direction and any fit limits.",
     f"m≈{fmt(m)}, b≈{fmt(b)}, r≈{r:.3f}; recompute unrounded coefficients before final rounding.",
     ["calculate slope and intercept","calculate r","round r to 3 decimals"])
for n,m,b in[(32,0.5,10),(33,-2,-10)]:
 xs=[-2,1,5,6,9];points=[(x,m*x+b)for x in xs]
 put(n,"VERIFY_EXACT_LINEAR_REGRESSION",
     f"Create an independent dataset by evaluating f(x)=({m})x+({b}) at x={xs}. Regress resulting five points and compare to original.",
     f"Data {points}, regression exactly m={m}, b={b}, r={'1'if m>0 else '−1'}; all residuals zero.",
     ["calculate five coordinates","fit least-squares","explain perfect correlation"])
prof34=[(46,600),(48,550),(50,505),(52,540),(54,495)]
m34,b34,r34=ls(prof34)
put(34,"REGRESS_DECLINING_PROFIT",
    f"For independent first-party business data (units hundreds, profit thousands) {prof34}, fit P(x)=mx+b and state units.",
    f"P(x)≈{fmt(m34)}x+({fmt(b34)}) thousand dollars, where x is hundreds of units, r≈{fmt(r34)}.")
put(35,"REGRESSION_X_INTERCEPT",
    "Using the least-squares line of independent data (46,600),(48,550),(50,505),(52,540),(54,495), estimate the x-intercept to one decimal and interpret units.",
    f"x≈{fmt(-b34/m34)} hundreds of units at fitted zero profit; outside observed range if appropriate.")
put(36,"REGRESSION_Y_INTERCEPT",
    "Using the same independently fitted five-point declining-profit model, estimate y-intercept to one decimal and explain its physical reliability.",
    f"y-intercept b≈{fmt(b34)} thousand dollars at x=0 hundreds of units; extrapolation far outside observed data.")
pop37=[(2500,2000),(2650,2001),(3000,2003),(3500,2006),(4200,2010)]
m37,b37,r37=ls(pop37)
put(37,"REGRESS_YEAR_AS_OUTPUT",
    f"Pairs (population,calendar year) are {pop37}. Regress year y as function of population x (not the other direction) to 3 decimal places.",
    f"year≈({m37:.3f})population+({b37:.3f}), unrounded slope={fmt(m37)}, intercept={fmt(b37)}, r≈{fmt(r37)}.")
put(38,"REGRESS_YEAR_AT_POPULATION_THRESHOLD",
    "Using independent population-year data (2500,2000),(2650,2001),(3000,2003),(3500,2006),(4200,2010), predict calendar year at population8000 using year as dependent variable.",
    f"Unrounded year prediction≈{fmt(m37*8000+b37)}, a long extrapolation outside the sampled population range.")
profit39=[(46,250),(48,305),(50,350),(52,390),(54,410)]
m39,b39,r39=ls(profit39)
put(39,"REGRESS_INCREASING_PROFIT",
    f"Fit least-squares profit in thousands versus units in hundreds for independent points {profit39}.",
    f"P(x)≈{fmt(m39)}x+({fmt(b39)}) thousands; r≈{fmt(r39)}.")
put(40,"REGRESSION_SALES_PROFIT_TARGET",
    "For fitted rising-profit model from (46,250),(48,305),(50,350),(52,390),(54,410), estimate number of hundreds of units required for profit greater than 1000 thousand dollars.",
    f"P(x)>1000 for x>{fmt((1000-b39)/m39)} hundreds of units, under extrapolation assumptions.")
profit41=[(46,250),(48,225),(50,205),(52,180),(54,165)]
m41,b41,r41=ls(profit41)
put(41,"REGRESS_FALLING_PROFIT",
    f"Fit least-squares profit in thousands versus units in hundreds for independent points {profit41}.",
    f"P(x)≈{fmt(m41)}x+({fmt(b41)}), slope negative, r≈{fmt(r41)}.")
put(42,"REGRESSION_DECREASING_PROFIT_THRESHOLD",
    "For the independent declining-profit observations (46,250),(48,225),(50,205),(52,180),(54,165), estimate the units-in-hundreds level where predicted profit dips below $25,000.",
    f"P(x)<25 thousand for x>{fmt((25-b41)/m41)} hundreds of units (negative slope), extrapolation beyond observations.")
assert sorted(items)==list(range(1,43))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l04-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L04",
"OriginalSectionNumberedExercises":42,
"IndependentFirstPartyStudentQuestions":[{"Number":n,"Skill":v["SourceLearningSkill"],"Question":v["IndependentFirstPartyStudentQuestion"],"GradeMode":"MANUAL_TEACHER_REVIEW"}for n,v in sorted(items.items())],
"TeacherExpectedAnswers":[{"Number":n,"ExpectedMath":v["TeacherExpectedMathematicsAndAnalysis"],"CognitiveActions":v["SourceNamedCognitiveActions"]}for n,v in sorted(items.items())],
"PublisherSourceGraphicImagesReproduced":False,
"OriginalSourceExactVerbatimAndGraphPixelParityCertified":False,
"StageDStudentTeacherRuntime":"NOT_RUN","CI":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u02-l04-original42-source-skill-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],
"OriginalSourceNumberedExerciseParents":42,"IndependentFirstPartySourceMathSkillQuestionContracts":42,
"OriginalPublisherTextNumericalAndGraphPixelParityCertified":False,
"OriginalPublisherThirdPartyImagesCopied":0,
"StageD":"NOT_RUN","Items":[items[n]for n in sorted(items)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u02-l04-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["OriginalSourceNumberedExerciseSkillAssessment"]="42_SOURCE_SKILL_INDEPENDENT_STATISTICS_QUESTIONS_WITH_TEACHER_REGRESSION_R_KEYS_AUTHORED"
c["TeacherManualAssessmentPath"]=native.relative_to(root).as_posix()
c["SourceNumberedExerciseContractPath"]=manifest.relative_to(root).as_posix()
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L04_42_SOURCE_SKILL_STATISTICS_QUESTION_CONTRACTS_AND_LEAST_SQUARES_MATH_KEYS_AUTHORED")
