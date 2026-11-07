from pathlib import Path
import json,math
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson004.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.4" and (source["Pages"][0]["PdfPage"],source["Pages"][-1]["PdfPage"])==(190,201)
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L04"
data=[(0,3),(1,5),(2,8),(3,9),(4,12)]
def fit(data):
 n=len(data);x=sum(a for a,b in data)/n;y=sum(b for a,b in data)/n
 xx=sum((a-x)**2 for a,b in data);yy=sum((b-y)**2 for a,b in data)
 xy=sum((a-x)*(b-y)for a,b in data)
 return xy/xx,y-xy*x/xx,xy/(xx*yy)**0.5
m,b,r=fit(data)
assert abs(m-2.2)<1e-10 and abs(b-3)<1e-10 and r>0.99
worked=[
("Draw and interpret a scatterplot","Plot the first-party points (0,3),(1,5),(2,8),(3,9),(4,12); describe direction and whether a line makes sense.","The pattern is strongly increasing and near-linear, but not perfectly collinear."),
("Fit a line by eye","Choose a plausible straight line for data (0,3),(1,5),(2,8),(3,9),(4,12), and give its slope/intercept.","A line of rough fit y≈2.2x+3; a visual estimate need not equal least squares exactly."),
("Distinguish interpolation/extrapolation","Using observed x=0 through4, predict y at x=2.5 and x=7 from y≈2.2x+3.","y(2.5)≈8.5 interpolation; y(7)≈18.4 extrapolation with higher uncertainty."),
("Compute least squares without library","For first-party scatter data x=0,1,2,3,4; y=3,5,8,9,12, compute least-squares line.","m=2.2,b=3.0; minimize Σ(y−mx−b)²."),
("Interpret Pearson correlation","For the same five points compute r and interpret it.","Pearson r≈"+str(round(r,3))+"; positive, close to one; correlation does not prove causation."),
("Predict under the fitted model","Using least-squares y≈2.2x+3, estimate x=6 and state whether within observed window.","y(6)=16.2; x6 exceeds observed [0,4], so extrapolation requiring domain caveat.")
]
tries=[
("Predict within observed x-range","A fitted line for x∈[0,4] is y=2.2x+3. Evaluate at x=2 and classify prediction.","7.4, interpolation (the actual observed y at x2 is 8; residual +0.6)."),
("Extrapolate line with model caution","For y=2.2x+3 fitted on x=0..4, predict at x=6; identify whether extrapolation.","16.2, extrapolation outside [0,4]; cannot guarantee model remains valid.")
]
concept=[
("Scatterplot","Each paired numerical observation (x,y) is plotted as one point.","Do not connect observations unless their order and process justify it.","(1,5),(2,8)."),
("Visual best-fit line","A line that follows the pattern of a scatterplot; not necessarily a perfect match.","Predict only when the relationship appears sufficiently straight.","y≈2.2x+3."),
("Least-squares regression","Choose coefficients m,b minimizing the sum of squared vertical residuals.","m=Σ[(x−x̄)(y−ȳ)]/Σ[(x−x̄)²]; b=ȳ−m x̄.","First-party data has m2.2,b3."),
("Residual","Observed output minus model-predicted output.","A pattern in residuals can indicate an inadequate linear model.","At x2: 8−7.4=0.6."),
("Pearson correlation","r measures direction and strength of linear association, between −1 and 1.","Large |r| means close to line, not proof of causation.","r≈0.992 strongly positive."),
("Interpolation","Estimating within the span of observed independent inputs.","Predictions can still be wrong even inside observed span.","Estimate at x2.5 with 0≤x≤4."),
("Extrapolation","Estimating beyond original independent input range.","Explicitly flag changing conditions and weaker reliability.","Predict x6 from x∈[0,4]."),
("Nonlinear relations","Curved scatter patterns are not well described by one affine rule.","Inspect scatter and residual shape before using regression.","x² data curve instead of straight line."),
("Correlation sign","Positive correlation indicates y grows on average with x; negative falls.","r near 0 means weak linear relation, not necessarily no relation.","r≈−0.9 indicates strong negative trend.")
]
visuals=[
{"Kind":"RatioTable","Title":"First-party five-observation scatter data","Description":"A near-linear dataset rather than an identical publisher chart.","PrimaryLabel":"x","SecondaryLabel":"y","Rows":[{"Label":"x","Cells":["0","1","2","3","4"]},{"Label":"Observed y","Cells":["3","5","8","9","12"]},{"Label":"Least-squares predicted y","Cells":["3","5.2","7.4","9.6","11.8"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"Interpolation and extrapolation","Description":"The source dataset has measured inputs from zero through four.","PrimaryLabel":"Input","SecondaryLabel":"Prediction","Rows":[{"Label":"x","Cells":["2","2.5","6"]},{"Label":"y=2.2x+3","Cells":["7.4","8.5","16.2"]},{"Label":"Type","Cells":["Interpolation","Interpolation","Extrapolation"]}],"Items":[],"SecondaryItems":[]}
]
cool={"StudentPrompt":"Given paired data (0,2),(1,4),(2,6),(3,8): (a) fit an exact linear rule, (b) identify r, (c) predict at x=2.5 and x=7 and classify each prediction.","TeacherExpectedMath":"y=2x+2, r=1; x2.5 gives 7 (interpolation), x7 gives 16 (extrapolation).","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"LessonCode":code,"CultureCode":"en","Title":"Fitting Linear Models to Data",
"ExplanationParagraphs":[
 "Learning aims: draw and interpret scatterplots, identify a reasonable line of best fit, distinguish linear versus nonlinear patterns and predict from a linear model.",
 "A least-squares model y=mx+b minimizes the sum of squared vertical residuals; an approximate hand-drawn line is different from a mathematical regression computation.",
 "Pearson's r ranges from −1 through +1, with sign indicating association direction and magnitude reflecting linear association strength. Correlation does not establish causation.",
 "Interpolation predicts at an x within the observed window. Extrapolation predicts outside it and requires an explicit caution about model reliability.",
 "Always state the data units, observed x-range, the fitted slope/intercept and whether the graph has a curved residual pattern.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {prompt}"for i,(title,prompt,key)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Make paired-coordinate scatter observations; compute regression and correlation when appropriate.","Steps":[answer],"Answer":answer,"Check":"Compare fitted model to observed points and inspect extrapolation risk."}for title,question,answer in worked],
"KeyConcepts":[{"Title":title,"Definition":definition,"Rule":rule,"Example":example}for title,definition,rule,example in concept],
"Visuals":visuals,"Videos":[],
"SummaryPoints":["Plot observations as paired points.","Determine whether linear fit is sensible.","Least squares minimizes vertical squared residuals.","Correlation sign and magnitude describe linear association, not causation.","Explicitly label interpolation vs extrapolation."],
"CommonMistakes":[
 {"Mistake":"Assuming an r near one means the slope must be one.","WhyWrong":"Correlation is unitless; slope is output/input units.","Correction":"Compute slope and r separately."},
 {"Mistake":"Claiming correlation proves causal influence.","WhyWrong":"A third variable or coincidence can create association.","Correction":"Describe association without causal claim."},
 {"Mistake":"Treating long-distance extrapolation as confirmed fact.","WhyWrong":"The observed relationship may break outside the sampled range.","Correction":"State data window, forecast assumptions and caution."}
 ]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l04.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U02:L04","ContentVersion":"g12-u02-l04-independent-statistics-authoring-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u02-l04-firstparty-two-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalTryItSkills":2,"FirstPartyIndependentTryItAlternatives":2,
"FirstPartyTeacherMathKeys":[{"Number":i,"Answer":key}for i,(_,_,key)in enumerate(tries,1)],
"IndependentCoolDown":cool,"PublisherGraphsCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Unit":2,"Lesson":4,"SourceSection":"2.4",
"SourceSHA256":source["SourceSha256"],"SourcePdfPages":[190,201],
"SourceWorkedExampleSkills":6,"SourceTryItSkills":2,"OriginalNumberedExercises":42,
"FirstPartyWorkedExamples":6,"FirstPartyTryIts":2,"FirstPartyKeyConcepts":9,
"FirstPartyTableVisuals":2,"IndependentCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"OriginalSourceNumberedExerciseSkillAssessment":"PENDING",
"OriginalSourceFigureSkillCrosswalk":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u02-l04-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==10 and p["FirstIncompleteLessonOrdinal"]==11
row=p["LessonEntries"][10]
row["ACStatus"]="A_C_IN_PROGRESS";row["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l04-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["42 named source statistical question skills need individual math teacher keys and regression numerics","Original source figures need review"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L04_RICH_6_WORKED_2_TRYIT_9_CONCEPTS_2_VISUALS_COOLDOWN_AUTHORING_COMPLETE_REGRESSION_R",round(r,6))
