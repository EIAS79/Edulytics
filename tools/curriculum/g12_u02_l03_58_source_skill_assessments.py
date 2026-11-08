from pathlib import Path
import json
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson003.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.3" and source["Pages"][0]["PdfPage"]==177
q={}
def put(n,skill,prompt,teacher,actions=None):
 assert n not in q,n
 q[n]={"Number":n,"OriginalSourcePdfPage":185 if n<=18 else 186 if n<=34 else 187 if n<=44 else 188 if n<=52 else 189,
 "SourcePedagogicAction":skill,"IndependentFirstPartyStudentQuestion":prompt,
 "TeacherExpectedMathematicsOrRubric":teacher,
 "OriginalNumberedSourceNamedActions":actions or ["construct or solve model with units"],
 "VerbatimOriginalBookExerciseCopied":False,"PublisherSourceImageReproduced":False,
 "GradingMode":"TEACHER_REVIEW_AUTHORING_COMPLETE_RUNTIME_NOT_RUN"}
for n,text,answer in [
(1,"How do you identify the input in a linear context and name its units?","Find independently varying quantity, commonly elapsed time, items, distance; label x and units."),
(2,"How do you identify the dependent output in a linear cost or population model?","Choose result that depends on x, e.g., total cost in dollars, population in residents."),
(3,"What does b represent in y=mx+b in a real-world model?","The model's output when input is zero, with output units."),
(4,"Explain how to determine the rate in a word problem with two data points.","m=(y2−y1)/(x2−x1), measured as output units per input unit.")
]:put(n,"MODEL_VARIABLES_INTERPRET",text,answer)
geom={
5:("A coordinate parallelogram is bounded by vertical x=0 and x=4 and parallel lines y=2x+1 and y=2x+4. Find exact area.","The two vertical sides have length3; horizontal separation4; area=12 square units."),
6:("A triangle is bounded by y=0, y=6−x/2 and a perpendicular through origin. Find its area.","Perpendicular is y=2x; intersection (12/5,24/5); x-axis base12, height24/5; area=144/5."),
7:("A triangle is bounded by x=0, y=8−2x and the perpendicular line through origin. Calculate its area.","Perpendicular line y=x/2; intersection (16/5,8/5); vertical base8, horizontal altitude16/5; area=64/5."),
8:("A parallelogram is bounded by y=0, y=3, y=2x, and the parallel line passing through (4,1). Find its exact area.","Second slant line y=2x−7; horizontal width7/2, vertical height3, area=21/2.")
}
for n,(text,answer)in geom.items():put(n,"GRAPH_GEOMETRIC_MODEL",text,answer,["find line slopes and intersections","compute correct geometric area"])
# Each 9–30 uses original conceptual model data with first-party paraphrase
contexts={
9:("A municipality falls from 5,900 people in 2010 to 4,700 in 2012, linearly. What does its model forecast for 2016?","P(t)=5900−600t, t years from 2010; P(6)=2300."),
10:("The municipality P(t)=5900−600t is extrapolated from 2010. When does the model predict zero residents? State the extrapolation caveat.","t=59/6 after 2010, around late 2019; outside the 2010–12 observed range."),
11:("A town has 46020 residents in 2010 and 52070 in 2012. If the linear increase continues, predict 2016.","m=3025/year, P(6)=64170."),
12:("When does a linear model P(t)=46020+3025t from 2010 first attain 75000?","t=28980/3025≈9.580 years after 2010."),
13:("A planned town starts with 75,000 people, gains 2,500 each year for five years. Write P(t).","P(t)=75000+2500t, 0≤t≤5."),
14:("For five-year population model P(t)=75000+2500t with 0≤t≤5, give domain and range.","Domain [0,5] years, range [75000,87500] residents."),
15:("For P(t)=75000+2500t, interpret intercepts and whether negative-time x-intercept is physically meaningful.","y-intercept 75000 at t=0; algebraic x-intercept t=−30, outside physical horizon."),
16:("Explain the slope in P(t)=75000+2500t.","2500 residents gained per year."),
17:("When does P(t)=75000+2500t reach 100000? Is it within a five-year planning horizon?","t=10, outside intended 0≤t≤5 domain."),
18:("Predict P(12) for P(t)=75000+2500t and identify extrapolation risk.","105000; extrapolates outside original five-year model."),
19:("A newborn begins at 7.5 lb and gains 0.5 lb/month. Construct model for its first 12 months.","W(t)=7.5+0.5t lb."),
20:("For W(t)=7.5+0.5t during the first year, give domain and range.","Domain [0,12] months; range [7.5,13.5] lb."),
21:("For W(t)=7.5+0.5t, describe intercepts and physical relevance.","y-int 7.5 lb at birth; algebraic x-int t=−15 months, not physically relevant."),
22:("What does slope 0.5 mean for W(t)=7.5+0.5t?","Gains 0.5 lb each month."),
23:("At what age does model W(t)=7.5+0.5t predict 10.4 lb?","t=5.8 months, within first-year domain."),
24:("Estimate newborn weight W(6.2) from W(t)=7.5+0.5t and state units.","10.6 lb at age 6.2 months."),
25:("A health service counted 12025 cases in 2005, decreasing linearly by 205 per year until 2010. Model cases versus t years from 2005.","C(t)=12025−205t, 0≤t≤5."),
26:("Find domain and range for C(t)=12025−205t during calendar years 2005 through 2010.","t∈[0,5], C∈[11000,12025] cases."),
27:("For C(t)=12025−205t, what are the intercepts and which is meaningful within 2005–2010?","y-intercept12025; algebraic x-int t=12025/205≈58.66, outside physical horizon."),
28:("Interpret the slope −205 in C(t)=12025−205t.","Cases decline at rate 205 people/year."),
29:("Extrapolate C(t)=12025−205t to zero cases; explain why the result is outside observed domain.","t=12025/205≈58.66 years after 2005, beyond data and not validated."),
30:("When would extrapolated C(t)=12025−205t reach 9700 cases?","t=(12025−9700)/205=465/41≈11.34 years after 2005; outside 2005–10.")
}
for n,(prompt,expected)in contexts.items():put(n,"MODEL_DOMAIN_RATE_EXTRAPOLATION",prompt,expected)
# Original tasks 31–38 rely on publisher-only graph pixels, so make independent
# graph models that preserve cognitive actions and with clear axes.
for n,rule,answer in [
(31,"company profit y(t)=120+2t thousand dollars, t years since 1980","y(t)=120+2t thousand dollars."),
(32,"company profit y(t)=120+2t thousand dollars, t years since 1980","y-intercept 120 thousand dollars in 1980."),
(33,"company profit y(t)=120+2t thousand dollars, t years since 1980","Algebraic x-intercept t=−60, outside post-1980 context."),
(34,"company profit y(t)=120+2t thousand dollars, t years since 1980","Profit increases 2 thousand dollars/year."),
(35,"company profit y(t)=400−10t thousand dollars, t years since 1980","y(t)=400−10t thousand dollars."),
(36,"company profit y(t)=400−10t thousand dollars, t years since 1980","y-intercept $400k at t=0."),
(37,"company profit y(t)=400−10t thousand dollars, t years since 1980","x-intercept t=40, calendar year 2020."),
(38,"company profit y(t)=400−10t thousand dollars, t years since 1980","Slope −10 thousand dollars/year.")
]:
 question=(f"An independent labelled graph shows {rule}. ")
 if n in (31,35):question+="Determine its linear rule from at least two plotted coordinates."
 elif n in (32,36):question+="Find and interpret its original y-intercept."
 elif n in (33,37):question+="Find and interpret the x-intercept, with horizon caveat."
 else:question+="Find and interpret its annual slope."
 put(n,"INDEPENDENT_GRAPH_LINEAR_MODEL",question,answer,
     ["read graph coordinates","interpret fitted model in original units"])
median={
39:("Mississippi $25200→$71400; Hawaii $74400→$272700 from 1950 to 2000.","Mississippi m=$924/year; Hawaii m=$3966/year; Hawaii faster."),
40:("Mississippi median home value $25200 in 1950 and $71400 in 2000. Estimate 2010 by linear extrapolation.","m=$924/year; 2010 estimate $80640."),
41:("When would historical straight-line Mississippi ($25200→$71400) and Hawaii ($74400→$272700) home values meet, using dates 1950 and 2000?","Solve t from 1950: 25200+924t=74400+3966t, t=−49200/3042≈−16.1749; year≈1933.83, outside observations."),
42:("Indiana home values $37700→$94300; Alabama $27100→$85100 from 1950 to 2000. Which increased faster?","Indiana m=1132/year, Alabama m=1160/year; Alabama faster."),
43:("Project Indiana median home value $37700 in 1950 and $94300 in 2000 into 2010 using same linear trend.","1950+60 years: $37700+60×1132=$105620."),
44:("Under historic linear trends, find year when Indiana ($37700→$94300) and Alabama ($27100→$85100) would coincide.","From 1950, 37700+1132t=27100+1160t => t=10600/28≈378.57, year≈2328.57; unrealistic extrapolation.")
}
for n,(prompt,expected)in median.items():put(n,"COMPARE_TWO_HISTORIC_LINEAR_DATA_SERIES",prompt,expected)
# Six-source-part application tasks
def extended_pair(n,year1,pop1,year2,pop2,forecast):
 slope=F(pop2-pop1,year2-year1);intercept=F(pop1)-slope*(year1-2000)
 return(
 f"An independently phrased school/town case shows population {pop1} in {year1} and {pop2} in {year2}. "
 f"(a) Find the absolute growth, (b) elapsed years, (c) yearly rate, (d) population in 2000, "
 f"(e) model P(t) for t years after 2000, (f) predict population in {forecast} assuming unchanged slope.",
 f"Growth {pop2-pop1}, elapsed {year2-year1} years, slope {slope}/year, P(0)={intercept}, P(t)=({slope})t+({intercept}), forecast P({forecast-2000})={intercept+slope*(forecast-2000)}.",
 ["growth","elapsed years","rate","2000 intercept","model formula","future prediction"])
put(45,"SIX_ACTION_LINEAR_POPULATION",*extended_pair(45,2004,1001,2008,1697,2011))
put(46,"SIX_ACTION_LINEAR_POPULATION",*extended_pair(46,2003,1431,2007,2134,2014))
put(47,"THREE_ACTION_LINEAR_PHONE_PLAN",
    "A mobile plan costs $71.50 for 410 minutes and $118 for 720 minutes. (a) Fit C(m), (b) explain monthly fixed charge and rate, (c) calculate C(687).",
    "m=0.15 dollars/min, intercept $10, C(m)=10+0.15m; C(687)=$113.05.",
    ["derive slope/intercept equation","interpret both parameters","calculate output"])
put(48,"THREE_ACTION_LINEAR_DATA_PLAN",
    "Mobile data costs $10 baseline plus a variable charge; 20 MB costs $11.20 and 130 MB costs $17.80. (a) Fit rule, (b) interpret rate and baseline, (c) calculate 250 MB.",
    "m=$0.06/MB, C(x)=10+0.06x, C(250)=$25.",
    ["derive function","interpret units","predict value"])
put(49,"TWO_ACTION_WILDLIFE_TREND",
    "Moose count grew from 4360 in 1991 to 5880 in 1999. (a) Find the linear model with t=years since 1990 and (b) forecast 2003.",
    "Annual slope190; P(t)=4170+190t; 2003 t=13 gives 6640.",
    ["find model with epoch offset","forecast count"])
put(50,"TWO_ACTION_WILDLIFE_TREND",
    "Owls decline from 340 in 2003 to 285 in 2007. (a) Fit model t years since 2003; (b) forecast 2012.",
    "P(t)=340−13.75t, P(9)=216.25 (round if reporting actual birds).",
    ["fit linear model","extrapolate with count caveat"])
put(51,"THREE_ACTION_RESOURCE_DEPLETION",
    "A gas reserve contains 16 billion cubic feet in 2010, depleted by 2.1 billion/year. (a) Model t years since 2010, (b) evaluate 2015, (c) predict empty time.",
    "R(t)=16−2.1t, R(5)=5.5 billion ft³; depleted t=160/21≈7.619 years after 2010.",
    ["model physical stock","evaluate year","solve zero boundary"])
put(52,"THREE_ACTION_RESOURCE_DEPLETION",
    "An oil reserve has 1820 billion barrels in 2014 and declines 25 billion/year. (a) Model t years after 2014; (b) seven-year reserve; (c) estimated depletion time.",
    "R(t)=1820−25t; R(7)=1645; zero at t=72.8 years after 2014.",
    ["construct model","evaluate","solve depletion time"])
for n,priceA,flat,priceB in [
(53,F("0.26"),F("19.95"),F("0.11")),
(54,F("5"),F("40"),F("3"))
]:
 threshold=flat/(priceA-priceB)
 unit="minutes"if n==53 else "windows"
 put(n,"COMPETING_COST_MODEL",
     f"Compare price A({unit})=({priceA})·{unit} with B({unit})={flat}+({priceB})·{unit}. Find the strict threshold at which B is cheaper.",
     f"B<A when {unit}>{threshold}; if whole {unit}, round upward to the next allowable integer.",
     ["write both costs","solve strict inequality"])
for n,baseA,comA,baseB,comB in [
(55,17000,F("0.12"),20000,F("0.05")),
(56,14000,F("0.10"),19000,F("0.04")),
(57,20000,F("0.12"),26000,F("0.03")),
(58,10000,F("0.09"),20000,F("0.04"))
]:
 t=F(baseB-baseA)/(comA-comB)
 put(n,"COMPETING_SALARY_COMMISSION",
     f"Compare annual pay A(S)={baseA}+({comA})S with B(S)={baseB}+({comB})S where S is dollar sales. For what sales is A strictly greater?",
     f"A>B iff S>{t} dollars; equality at S={t}.",
     ["set up pay models","solve exact strict sales threshold"])
assert sorted(q)==list(range(1,59)), sorted(q)
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l03-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"OriginalSourceSection":"2.3","OriginalNumberedExercises":58,
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L03",
"StudentFirstPartyQuestions":[{"Number":n,"Skill":x["SourcePedagogicAction"],"Prompt":x["IndependentFirstPartyStudentQuestion"],"Scoring":"TEACHER_REVIEW"}for n,x in sorted(q.items())],
"TeacherOnlyMathRubrics":[{"Number":n,"Expected":x["TeacherExpectedMathematicsOrRubric"],"NamedSourceActions":x["OriginalNumberedSourceNamedActions"]}for n,x in sorted(q.items())],
"OriginalPublisherGraphBitmapsCopied":False,
"PublisherSourceNumericalItemVerbatimAndGraphicPixelParityClaim":False,
"LiveStudentTeacherRuntime":"NOT_RUN","StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u02-l03-source58-firstparty-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"OriginalSourceSha256":source["SourceSha256"],"OriginalNumberedSourceExerciseParents":58,
"FirstPartyIndependentNumberedAcademicSkillTasks":58,
"PublisherImagesAndThirdPartyPhotosCopied":0,
"OriginalExactPublisherQuestionAndGraphicParityCertified":False,
"StageD":"NOT_RUN","Items":[q[n]for n in sorted(q)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u02-l03-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Original58SourceExerciseMathSkillContracts"]="58_FIRSTPARTY_MATH_SKILL_QUESTIONS_WITH_TEACHER_RUBRICS_AUTHORED"
c["NativeTeacherAssessmentPath"]=native.relative_to(root).as_posix()
c["OriginalNumberedExerciseSkillContractPath"]=manifest.relative_to(root).as_posix()
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U02_L03_58_SOURCE_SKILL_FIRSTPARTY_QUESTION_CONTRACTS_AUTHORED")
