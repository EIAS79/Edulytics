from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson003.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.3" and source["Pages"][0]["PdfPage"]==177 and source["Pages"][-1]["PdfPage"]==189
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L03"
examples=[
("Extrapolate population using a linear model","A town has 7,500 people at t=0 and 8,700 at t=4 years. Assume a linear model. Find P(t) and forecast the year t=8.","Slope 300 residents/year; P(t)=7500+300t; P(8)=9900. Valid only if the linear trend persists."),
("Model perpendicular walking paths","Two people start together; one walks east 6 km/h and another north 8 km/h. Build a function for the straight-line separation after t hours.","Distance d(t)=√((6t)²+(8t)²)=10t km for t≥0, a linear distance model."),
("Geometric projection in a road network","A straight route to a location 40 km east and 30 km north has a total length 50 km. Find the projection of a point (16,12) on the route and its distance along the line.","The point lies at two-fifths of the route, so distance from start =20 km; slope 3/4 from coordinates."),
("Choose between two affine service plans","Plan A charges fixed $40 and $1.50 per kilometre; plan B $75 and $0.80/km. Find the break-even travel distance and preferred plan afterward.","40+1.5d=75+0.8d; d=50 km; B cheaper for d>50."),
]
tries=[
("Model fixed and variable manufacturing cost","A workshop pays $30,000 rent and $0.40 for each item. Write cost C(n), initial value and units.","C(n)=30000+0.40n dollars for n≥0 items; intercept $30000."),
("Forecast and threshold from a two-year trend","A town has 20,000 people in 2020 and 24,000 in 2024. Under a linear continuation find 2026 population and year of 30,000.","Slope 1000/year; 2026: 26000; 2030: 30000."),
("Use perpendicular projection geometry","Road segment points from town A=(0,0) to B=(30,40) km. Find travelled straight-line length to a junction at (12,16).","Total road length 50 km, junction at 20 km from A.")
]
concept=[
("Identify variables","The independent quantity is the controllable input and the dependent quantity changes with it.","Choose units and realistic nonnegative domain.","Number of items n → cost C(n)."),
("Constant-rate model","Linear model y=mx+b when the rate Δy/Δx is constant.","Estimate m from two observed points, then determine b.","P(t)=7500+300t."),
("Extrapolation caveat","Forecasting a linear trend beyond observations assumes the rate stays unchanged.","An extrapolation is a model forecast, not a guaranteed fact.","P(8)=9900 assumes annual growth remains 300."),
("Graph and geometry model","Perpendicular displacement components can form a right triangle.","Pythagorean length may simplify to a linear time function.","East6t, north8t gives 10t."),
("Parallel/perpendicular route","Coordinate geometry can express route slopes and projection points.","Use dot product or right-angle geometry for perpendicular lines.","Slope3/4 has perpendicular slope −4/3."),
("System of linear models","Two competing cost plans meet at equal cost.","Solve C1(x)=C2(x), then compare the two sides of threshold.","$40+1.5d versus $75+0.8d break even at d=50."),
("Intercept and rate in context","A model's intercept is its value at input zero.","State both quantities in physical/financial units.","Rent is $30,000 even if zero items produced."),
("Domain/range and units","A realistic function may be limited to the duration of a measured trend.","Do not extrapolate a nonnegative population into negative numbers.","Restrict t and explain model horizon.")
]
visuals=[
{"Kind":"RatioTable","Title":"Independent town population model","Description":"Uniform annual increments of 300 residents.","PrimaryLabel":"Years","SecondaryLabel":"Population","Rows":[{"Label":"t","Cells":["0","2","4","8"]},{"Label":"P(t)","Cells":["7500","8100","8700","9900"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"First-party two-plan cost comparison","Description":"The lower-cost plan changes after equality at 50 km.","PrimaryLabel":"Distance km","SecondaryLabel":"Cost $","Rows":[{"Label":"d","Cells":["0","25","50","100"]},{"Label":"Plan A","Cells":["40","77.50","115","190"]},{"Label":"Plan B","Cells":["75","95","115","155"]}],"Items":[],"SecondaryItems":[]}
]
cool={"Prompt":"(a) A supply model has $125 fixed cost and $3 per unit. Find C(40). (b) A population is 600 in year0 and 750 in year3; find P(t). (c) Compare A(d)=20+2d with B(d)=60+d and find when B saves money.","TeacherExpectedMath":"(a) $245. (b) P(t)=600+50t. (c) Equal at d=40; B cheaper when d>40.","OriginalRestrictedTeacherOnlyPublisherCoolDownCopied":False}
lesson={
"CultureCode":"en","LessonCode":code,"Title":"Modeling with Linear Functions",
"ExplanationParagraphs":[
"Learning aims: identify model inputs and outputs, translate verbal descriptions into affine equations, use diagrams for quantities and spatial measurements, and build/solve systems to compare practical choices.",
"The statement 'starts at b and gains m per unit' corresponds to y=mx+b. Explain the rate and initial value with units.",
"Two data points identify the linear slope. Check whether the resulting formula should apply only within a specific time interval or other realistic domain.",
"Geometric modeling may require a Pythagorean distance formula before simplification. Distances, times and cardinal directions must retain their units.",
"Competing plans should be compared as two functions of the same input variable. Their break-even point solves C1(x)=C2(x), but preferring one plan requires evaluating the appropriate inequality.",
"Independent Edulytics cool-down: "+cool["Prompt"]]+[f"Independent Try It {i} — {t}. {q}"for i,(t,q,a)in enumerate(tries,1)],
"WorkedExamples":[{"Title":t,"Question":q,"Method":"Identify the independent input, create an equation or geometric relationship, and check units and constraints.","Steps":[answer],"Answer":answer,"Check":"Verify known data points and contextual interpretation."}for t,q,answer in examples],
"KeyConcepts":[{"Title":t,"Definition":definition,"Rule":rule,"Example":ex}for t,definition,rule,ex in concept],
"Visuals":visuals,"Videos":[],
"SummaryPoints":["Define input/output variables and units.","Use constant slope and intercept to build a model.","Confirm the chosen physical domain and extrapolation assumptions.","Solve C1=C2 before comparing costs.","A geographic diagram can reveal lengths and perpendicular relationships."],
"CommonMistakes":[
{"Mistake":"Assuming a trend extrapolates indefinitely.","WhyWrong":"Linear models can break beyond observed conditions.","Correction":"State model assumptions and plausible horizon."},
{"Mistake":"Comparing two prices with different input units.","WhyWrong":"Both expressions must use the same independent quantity.","Correction":"Convert to one shared variable and unit."},
{"Mistake":"Treating a positive break-even equality as the cheaper-plan interval.","WhyWrong":"Equality alone says neither plan is cheaper.","Correction":"Solve a strict inequality for preferred option."}
]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l03.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U02:L03","ContentVersion":"g12-u02-l03-math-source-skill-authoring-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u02-l03-firstparty-three-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"SourceTryItSkillCount":3,"IndependentFirstPartyTryItCount":3,
"TeacherOnlyMathKeys":[{"Number":i,"ExpectedMath":a}for i,(t,q,a)in enumerate(tries,1)],
"IndependentCoolDown":cool,"OriginalPublisherOrThirdPartyImagesCopied":False,"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":3,
"SourceSection":"2.3","SourceSHA256":source["SourceSha256"],"SourcePdfPages":[177,189],
"SourceWorkedExamples":4,"SourceTryItActivities":3,"SourceNumberedExercises":58,
"FirstPartyWorkedExamples":4,"FirstPartyTryIts":3,"FirstPartyConcepts":8,"FirstPartyVisualTables":2,
"FirstPartyCoolDown":True,"RichV2Path":rp.relative_to(root).as_posix(),
"Original58SourceExerciseMathSkillContracts":"PENDING","OriginalSourceFigureSkills":"PENDING",
"ReadyForStageD":False,"Tests":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u02-l03-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==9 and p["FirstIncompleteLessonOrdinal"]==10
row=p["LessonEntries"][9]
row["ACStatus"]="A_C_IN_PROGRESS";row["RichV2Path"]=cp["RichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l03-abc-checkpoint.json"
row["BlockingReasons"]=["58 source-numbered exercise cognitive actions need independent teacher scored problems","Source figure skill anchors pending"]
row["ReadyForStageD"]=False
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L03_4_WORKED_3_TRYIT_8_CONCEPTS_2_VISUALS_AND_COOLDOWN_AUTHORING_COMPLETE")
