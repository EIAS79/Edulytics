from pathlib import Path
import json
from fractions import Fraction as F
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson001.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.1" and len(source["Pages"])==17
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L01"
examples=[
("Interpret linear depth model","A sensor reports pressure P(d)=0.45d+15 (psi), where d is depth in feet. Interpret its initial value and slope.","P(0)=15 psi; pressure increases by 0.45 psi per foot."),
("Increasing/decreasing/constant models","Compare number-of-items models A(t)=45t, B(t)=240−12t and C(t)=18. Classify their monotonicity.","Increasing A; decreasing B; constant C."),
("Slope between two points","Find slope of the line through (2,−1) and (7,3), and describe direction.","m=(3−(−1))/(7−2)=4/5, increasing."),
("Rate from dated population samples","Population changes from 14,200 in 2018 to 16,600 in 2022. Assume a linear model and give annual slope.","(16600−14200)/(2022−2018)=600 people/year."),
("Point-slope to slope-intercept","Write a line with slope 2 passing through (5,−1).","y+1=2(x−5); y=2x−11."),
("Line through two points","Find a line through (2,3),(6,11) in point-slope and slope-intercept forms.","m=2; y−3=2(x−2), y=2x−1."),
("Read a line from graphed intercepts","A first-party plotted line contains (0,4) and (−2,−2). Find its function.","slope 3, f(x)=3x+4."),
("Constant cost and per-item cost","A small workshop pays $900 fixed rent and $25 per item. Express C(n) and say what slope represents.","C(n)=900+25n dollars, n≥0 items; slope $25/item."),
("Solve two plotted values for a linear rule","Suppose f(2)=−1 and f(7)=9 for a linear f. Find a slope-intercept equation.","m=2; f(x)=2x−5."),
("Total from initial count and monthly additions","A library begins with 300 recordings and adds 12 monthly. Find N(t) and N(12).","N(t)=300+12t, N(12)=444."),
("Infer base fee and commission from two invoices","An employee is paid $680 for four sales and $840 for eight sales. Find base plus per-sale commission.","slope $40/sale; base $520; I(n)=520+40n."),
("Convert a table to one linear function","A recorded sequence has weeks w=0,2,4,6 and values 900,960,1020,1080. Find model.","P(w)=900+30w; uniform rate 30/week.")
]
tryits=[
("Identify slope and initial value","For f(t)=8t+15, state initial value, rate, and behavior.","b=15, m=8 per input, increasing."),
("Classify by slope","Classify f(x)=−3x+7, g(x)=0x+5 and h(x)=2x−1.","f decreasing; g constant; h increasing."),
("Two-point slope","Compute slope through (−2,4),(2,10).","m=3/2."),
("Point-slope equation","Find line slope 4 through (2,−3).","y+3=4(x−2), y=4x−11."),
("Cost from fixed charge","With initial charge $40 and $12 per item, write C(n) and compute C(5).","C(n)=40+12n; C(5)=100."),
("Table linearity","A table x=0,2,4,6 has outputs 3,7,11,15. Find f.","f(x)=2x+3.")
]
concept=[
("Linear equation","A linear function has form f(x)=mx+b with a constant rate.","Slope m describes output change per input; b=f(0).","f(x)=2x+3."),
("Slope from coordinates","m=(y2−y1)/(x2−x1) for distinct inputs.","Both coordinate differences must follow matching endpoint order.","From (1,3) to (3,7), m=2."),
("Increasing/decreasing","Sign of m controls monotonicity.","Positive slopes increase, negatives decrease; zero slope is constant.","−4x+3 decreases."),
("Point-slope form","A line through (x1,y1) with slope m satisfies y−y1=m(x−x1).","Convert to y=mx+b by expanding.","y−2=3(x−1)."),
("Intercepts","b=f(0), while x-intercept solves mx+b=0.","For m≠0, x-intercept is −b/m.","2x+6 intercept at x=−3."),
("Linear model units","Each unit increase in x changes output by m output units.","State physical domain, units and initial value.","C(n)=900+25n for nonnegative item counts."),
("Linear table test","Successive data pairs lie on one line iff all pairwise slopes match.","Use Δy/Δx even for unequal input gaps.","x=0,2,4; f=1,5,9."),
("Vertical and horizontal lines","A vertical line x=c is not a function y=f(x) on a multi-y graph.","A horizontal line y=c is a constant function.","x=2 is vertical."),
]
visuals=[
 {"Kind":"RatioTable","Title":"First-party uniform linear growth","Description":"Equal input increments yield equal output changes.","PrimaryLabel":"week","SecondaryLabel":"count","Rows":[{"Label":"Week","Cells":["0","2","4","6"]},{"Label":"Count","Cells":["900","960","1020","1080"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Linear and nonlinear table distinction","Description":"The middle row demonstrates constant difference; bottom row does not.","PrimaryLabel":"Input","SecondaryLabel":"Output","Rows":[{"Label":"x","Cells":["0","1","2","3"]},{"Label":"Linear","Cells":["4","7","10","13"]},{"Label":"Nonlinear","Cells":["4","5","8","13"]}],"Items":[],"SecondaryItems":[]}
]
cool={
"StudentPrompt":"(a) Find f(x)=mx+b through (−1,2) and (3,10). (b) What do slope and intercept mean in C(n)=150+8n? (c) Decide if x=5 represents y as a function of x.","TeacherExpectedMath":"(a) f(x)=2x+4. (b) Fixed $150 and $8 per item. (c) No, x=5 permits many y-values.",
"OriginalPublisherRestrictedCoolDownCopied":False}
lesson={
"CultureCode":"en","Title":"Linear Functions","LessonCode":code,
"ExplanationParagraphs":[
 "Original learning aims: identify and interpret linear models f(x)=mx+b, determine whether behavior increases/decreases/is constant, calculate rate from points, write slope-intercept and point-slope equations, and model tables/contexts.",
 "Slope is a constant rate (change in output)/(change in input). The sign determines increasing or decreasing behavior. A line has slope m and initial value b=f(0).",
 "Given a point and slope, use y−y1=m(x−x1). Given two points, first compute m, then solve for b.",
 "An actual model should state units, physical domain and the meaning of its initial value. Tables require a constant rate on every pair of neighboring measurements.",
 "Independent Edulytics cool-down: "+cool["StudentPrompt"],
 ]+[f"Independent Try It {i} — {t}. {p}"for i,(t,p,a)in enumerate(tryits,1)],
"WorkedExamples":[{"Title":t,"Question":p,"Method":"Compute slope/intercept or apply the linear function skill with units.","Steps":[a],"Answer":a,"Check":"Validate the model with the given points and units."}for t,p,a in examples],
"KeyConcepts":[{"Title":t,"Definition":de,"Rule":ru,"Example":ex}for t,de,ru,ex in concept],
"SummaryPoints":["Linear functions have form mx+b.","Slope is a constant output change per unit input.","Positive/negative/zero slopes imply increasing/decreasing/constant.","Two distinct points determine one line.","Modeling requires appropriate units and admissible domains."],
"Visuals":visuals,"Videos":[],
"CommonMistakes":[
 {"Mistake":"Interchanging Δx and Δy.","WhyWrong":"Units and slope are inverted.","Correction":"Calculate (y2−y1)/(x2−x1)."},
 {"Mistake":"Assuming a nonlinear table is linear.","WhyWrong":"Slopes differ across equal input changes.","Correction":"Compare all adjacent slopes."},
 {"Mistake":"Calling x=5 a linear function y=f(x).","WhyWrong":"A vertical line has many outputs for the same input.","Correction":"Use vertical-line test."}
 ]}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l01.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U02:L01","ContentVersion":"g12-u02-l01-original-math-source-skill-authoring-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u02-l01-tryit-six-teacher-key-and-cooldown.json").write_text(json.dumps({"SourceTryItCount":6,"IndependentFirstPartyTryIts":6,"TeacherKeys":[{"Number":i,"ExpectedMath":a}for i,(t,p,a)in enumerate(tryits,1)],"CoolDown":cool,"SourcePublisherArtworkReproduced":False},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":1,"SourceSection":"2.1",
"LocalSourceSHA256":source["SourceSha256"],"OriginalPDFPages":[142,157],
"SourceWorkedExamples":12,"SourceTryIts":6,"SourceNumberedExercises":76,
"FirstPartyWorkedExamples":12,"FirstPartyTryIts":6,"FirstPartyKeyConcepts":8,"VisualTables":2,"IndependentCoolDown":True,
"RichV2Path":rich.relative_to(root).as_posix(),
"OriginalSectionFigureSkillReview":"PENDING","OriginalNumberedExerciseSkillReview":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u02-l01-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==7 and p["FirstIncompleteLessonOrdinal"]==8
row=p["LessonEntries"][7]
row["ACStatus"]="A_C_IN_PROGRESS";row["RichV2Path"]=cp["RichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l01-abc-checkpoint.json"
row["BlockingReasons"]=["Section 2.1 original 76 numbered exercise skills and figure skills still need complete independent source skill review"]
row["ReadyForStageD"]=False
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L01_12_FIRSTPARTY_WORKED_6_TRYIT_8_CONCEPTS_2_VISUALS_AND_COOLDOWN_AUTHORING_COMPLETE")
