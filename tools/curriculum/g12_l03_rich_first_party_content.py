from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson003.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.3" and len(source["Pages"])==13
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L03"
def ex(title,q,steps,answer,check):
 return {"Title":title,"Question":q,"Method":"Compute, interpret and justify every mathematical conclusion with correct input and output units and graph limitations.","Steps":steps,"Answer":answer,"Check":check}
worked=[
ex("Average change in a time series","An independently collected annual cost series rises from $2.50 per unit in 2020 to $3.10 per unit in 2024. Find average change per year.",
["The dependent value changes by $0.60.","The year input changes by four years.","Average rate is $0.60/4 years."],"$0.15 per unit per year","The rate is positive because the dependent cost increased."),
ex("Average slope from two plotted points","A plotted continuous function passes through (−1,4) and (2,1). Compute average rate on [−1,2].",
["Read both endpoint coordinates.","Compute Δy=1−4=−3.","Compute Δx=2−(−1)=3 and divide."],"−1 output unit per input unit","Reversing exactly one subtraction changes the sign incorrectly."),
ex("Average speed from measured distance","A vehicle's distance from the starting position changes from 6 miles at t=0 hours to 276 miles at t=6 hours. Find average speed.",
["Total distance change=276−6=270 miles.","Elapsed time=6 hours.","Rate=270/6=45 miles per hour."],"45 miles per hour","Average speed over the whole interval is not necessarily the instantaneous speed."),
ex("Average change from a formula","For f(x)=x²−2/x, find the average rate over [2,4].",
["f(2)=4−1=3.","f(4)=16−1/2=31/2.","Divide (31/2−3) by 2."],"25/4","Both endpoints lie in the domain (x≠0)."),
ex("Average inverse-square force change","A hypothetical force obeys F(d)=6/d² newtons for distance d>0 cm. Find the average force change over [2,6].",
["F(2)=6/4=3/2 newtons.","F(6)=6/36=1/6 newton.","Rate=(1/6−3/2)/4=−1/3 newton/cm."],"−1/3 newton per centimeter","Sign negative reflects force decreasing with distance."),
ex("Symbolic endpoint rate","For g(t)=t²+4t+2, compute the average change from t=0 to t=a, a≠0.",
["g(a)=a²+4a+2 and g(0)=2.","Divide a²+4a by a.","Factor a and cancel only because a≠0."],"a+4, for a≠0","At a=3 average rate is 7."),
ex("Monotonic intervals from a graph","A continuous graph rises over (1,3), falls over (3,4) and rises again for t>4. Identify all increasing intervals.",
["Read graph direction left to right.","Increasing where output rises, not where the graph lies above x-axis.","Exclude turning-point boundaries under conventional open interval reporting."],"(1,3) and (4,∞)","A graph can decrease while staying above zero."),
ex("Local extrema of a rational-plus-linear rule","For f(x)=3/x+x/2, x≠0, locate local extrema and intervals of increasing/decreasing.",
["The derivative for x≠0 is −3/x²+1/2.","Stationary inputs satisfy x²=6, giving x=±√6.","The derivative is positive where |x|>√6 and negative where 0<|x|<√6."],
"Local maximum (−√6,−√6); local minimum (√6,√6). Increasing (−∞,−√6) and (√6,∞); decreasing (−√6,0) and (0,√6).",
"Never cross the excluded input x=0 when writing monotonic intervals."),
ex("Local extrema from an independently specified cubic","For f(x)=x³−3x, identify local extrema and their values.",
["Derivative f'(x)=3x²−3 vanishes at x=−1 and x=1.","Sign changes from positive to negative at −1, and negative to positive at 1.","Evaluate f(−1)=2 and f(1)=−2."],
"Local maximum at (−1,2); local minimum at (1,−2).",
"Local extrema need not be absolute extrema on the full real domain."),
ex("Absolute extrema on a complete displayed bounded model","Connect successive independent graph points (−3,0),(−2,16),(−1,10),(0,0),(1,10),(2,16),(3,−10) with straight segments over [−3,3]. Find absolute max/min.",
["All seven points and connecting segments lie in the declared domain [−3,3].","The greatest displayed output is 16 at two inputs −2 and 2.","The least displayed output is −10 at the right included endpoint x=3."],
"Absolute maximum 16 attained at x=−2 and x=2; absolute minimum −10 at x=3.",
"On a continuous piecewise-linear graph, extrema are at vertices or included endpoints.")
]
assert len(worked)==10
tries=[
("Compute a rate from recorded prices","A fare rises from $2.20 in 2019 to $2.80 in 2023. Find the average change per year.","$0.15 per year."),
("Average rate with a radical","For f(x)=x−2√x, compute average rate on [1,9].","1/2."),
("Symbolic average rate","For f(x)=x²+2x−8 find average rate on [5,a] with a≠5.","a+7."),
("Use local extrema to classify monotonicity","Graph f(x)=x³−6x²−15x+20. Approximate the two local extrema and increasing/decreasing intervals.",
 "Local maximum at (−1,28), local minimum at (5,−80); increasing (−∞,−1) and (5,∞); decreasing (−1,5).")
]
key=[
("Average rate of change","The secant slope between inputs a and b is (f(b)−f(a))/(b−a).","Use the SAME ordered endpoints in numerator and denominator.","f(2)=7 and f(5)=1 implies average rate −2."),
("Rate units and sign","A change in output per one input unit has output-units/input-units.","Negative average rate means outputs fall as inputs increase across the interval.","Dollars per year and miles per hour are different units."),
("Increasing, decreasing and constant","Increasing means outputs rise as x moves right; decreasing means outputs fall; constant means no change.","Monotonic intervals are input intervals, not output ranges.","y=x² is decreasing for x<0 and increasing for x>0."),
("Local extrema","A local maximum/minimum is the highest/lowest value among nearby inputs in the domain.","Local extrema do not necessarily dominate all values in the full domain.","x³−3x has local maximum 2 at x=−1 and local minimum −2 at x=1."),
("Absolute extrema","The absolute maximum/minimum is greatest/least output over the ENTIRE given domain.","A bounded domain can attain its extremum at a boundary; an unbounded curve might not attain an absolute maximum.","The bounded graph with outputs −10..16 attains both absolute extremes."),
("Domain-aware graphical reasoning","A continuous graph may have turning points, jumps, asymptotes and clipped viewing windows.","Do not infer global extrema from a clipped plot, and do not join intervals across excluded x.","For 3/x+x/2, never combine monotonicity across x=0.")
]
rows=[
 {"Kind":"RatioTable","Title":"Independent annual cost table for rate calculation","Description":"A source-skill-aligned independent time series, not a copied petrol-price dataset.","PrimaryLabel":"Year","SecondaryLabel":"Cost","Rows":[{"Label":"Year","Cells":["2020","2021","2022","2023","2024"]},{"Label":"Cost in dollars","Cells":["2.50","2.65","2.80","2.90","3.10"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Independent distance readings","Description":"Measure average velocity using two readings, not the slope at one instant.","PrimaryLabel":"Hour","SecondaryLabel":"Miles","Rows":[{"Label":"Time (hours)","Cells":["0","1","2","3","4","5","6"]},{"Label":"Distance (miles)","Cells":["6","43","95","143","170","215","276"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Local and absolute extrema values on different models","Description":"Extrema require a declared domain and inspection of neighboring or all outputs.","PrimaryLabel":"Model","SecondaryLabel":"Extremum","Rows":[{"Label":"Type","Cells":["Local maximum","Local minimum","Absolute maximum","Absolute minimum"]},{"Label":"Value","Cells":["2 near x=−1","−2 near x=1","16 at x=−2,2","−10 at x=3"]}],"Items":[],"SecondaryItems":[]}
]
cool={
"Prompt":"(a) Compute average change of h(t)=t²+2t from t=1 to t=4. (b) Classify increasing/decreasing intervals for f(x)=x² on ℝ. (c) Explain why an increasing graph may have a negative output.",
"TeacherExpectedAnswer":"(a) h(1)=3,h(4)=24, rate=(24−3)/3=7; (b) decreases on (−∞,0), increases on (0,∞), minimum 0; (c) increasing describes output changes as x increases, not the output's sign.",
"TeacherGraded":True,"OriginalTeacherOnlySourceCoolDownCopied":False
}
paras=[
"Learning goals: calculate average rates from tables, formulas and plotted endpoints, interpret change signs and physical units, identify increasing/decreasing/constant intervals, estimate local extrema and distinguish them from absolute extrema.",
"Secant slope between a<b is Δy/Δx=(f(b)−f(a))/(b−a). Use the same input order for both differences; use output-units per input-units.",
"A positive rate describes increasing net output over an interval; a negative rate describes decreasing net output. A local slope may vary inside the interval.",
"Increasing/decreasing intervals describe behavior reading the graph from left to right, not whether y is positive or negative. Report open input intervals around turning points when appropriate.",
"Local extrema depend on a neighborhood; absolute extrema compare all permitted inputs. Always state the graph's domain, including any open endpoints and asymptotes.",
"Independent Edulytics Cool-down — "+cool["Prompt"],
] + [f"Independent Try It {i} — {title}. {prompt}"for i,(title,prompt,ans)in enumerate(tries,1)]
mistakes=[
("Reversing only the denominator endpoints in a secant slope.","This reverses the sign incorrectly.","Use (f(b)−f(a))/(b−a) consistently."),
("Interpreting a negative output as a decreasing function.","The sign of f(x) and the direction of change are different properties.","Compare outputs as input increases."),
("Treating a local maximum as an absolute maximum on ℝ.","A graph can rise higher outside the local neighborhood.","State domain and compare all outputs before claiming absolute."),
("Connecting increasing intervals across an excluded input.","An asymptote separates the permitted domain into distinct pieces.","Report separate intervals around a missing input.")
]
lesson={
"CommonMistakes":[{"Mistake":m,"WhyWrong":why,"Correction":fix}for m,why,fix in mistakes],
"CultureCode":"en",
"ExplanationParagraphs":paras,
"KeyConcepts":[{"Title":t,"Definition":de,"Rule":ru,"Example":ex}for t,de,ru,ex in key],
"LessonCode":code,
"SummaryPoints":[
 "Average rate of change equals output change divided by input change; units are output units per input unit.",
 "Use the same two endpoints in the numerator and denominator of the secant slope.",
 "Increasing/decreasing/constant intervals depend on the graph's direction, not the sign of the output.",
 "Local extrema describe a neighborhood; absolute extrema compare every permitted output.",
 "Use domain restrictions and included endpoints when reasoning about global extrema.",
 "First-party graph sketches and open-ended extrema are teacher-reviewed in Stage C."
],
"Title":"Rates of Change and Behavior of Graphs",
"Videos":[],
"Visuals":rows,
"WorkedExamples":worked
}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l03.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"ContentVersion":"g12-u01-l03-firstparty-source-skill-a-b-c-20261106","Lessons":[lesson],"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L03","SchemaVersion":2},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
teacher=base/"u01-l03-independent-tryit-teacher-key.json"
teacher.write_text(json.dumps({"Unit":1,"Lesson":3,"OriginalSourceTryIts":4,"FirstPartyIndependentTryItTasks":4,
"StudentPromptDoesNotRevealTeacherAnswers":True,
"Items":[{"SourceOriginalTryItNumber":i,"StudentPrompt":prompt,"FirstPartyTeacherAnswer":ans,
"OriginalSourcePdfPage":[55,57,58,60][i-1],"FirstPartyAlternativeNotSourceCopy":True,"RuntimeVerified":False}
for i,(title,prompt,ans)in enumerate(tries,1)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l03-firstparty-cooldown.json").write_text(json.dumps(cool,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint={
"SchemaVersion":1,"Unit":1,"Lesson":3,
"OriginalSourceSection":"1.3","OriginalLocalPdfPages":[54,66],
"OriginalSourceSha256":source["SourceSha256"],
"FirstPartyRichV2Path":rich.relative_to(root).as_posix(),
"OriginalSourceWorkedExamples":10,
"OriginalSourceTryIts":4,
"OriginalSourceNumberedExercises":47,
"FirstPartyWorkedExamplesAuthored":10,"FirstPartyTryItsAuthored":4,
"FirstPartyKeyConceptsAuthored":6,"FirstPartyTableVisualsAuthored":3,
"IndependentCoolDownAuthored":True,
"OriginalSourceImagesCopied":0,
"SourceExerciseReview":"NOT_YET_FULL_47",
"SourceFigureAndGraphSkillCrosswalk":"PENDING",
"StageA":"SOURCE_SECTION_ORIGINAL_PDF_SECTION_1_3_IDENTIFIED",
"StageB":"TEN_FIRSTPARTY_EXAMPLES_FOUR_TRYIT_SIX_CONCEPTS_THREE_TABLE_VISUALS",
"StageC":"PENDING_SOURCE_47_EXERCISE_ITEMS",
"ReadyForStageD":False,
"Tests":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"UpdatedUtc":datetime.now(timezone.utc).isoformat()
}
(base/"u01-l03-abc-checkpoint.json").write_text(json.dumps(checkpoint,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==2 and p["FirstIncompleteLessonOrdinal"]==3
row=p["LessonEntries"][2]
row["ACStatus"]="A_C_IN_PROGRESS"
row["OriginalSourceWorkedExamplesLocated"]=10
row["OriginalSourceTryItsLocated"]=4
row["OriginalSourceNumberedExercisesLocated"]=47
row["RichV2Path"]=checkpoint["FirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l03-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=[
 "47 original source exercise skills and their separate teacher assessment need authoring",
 "19 source figures and graph-based extrema need source-skill visual review",
 "Original named action denominator and teacher-facing answer rubric not yet verified"]
row["UpdatedUtc"]=checkpoint["UpdatedUtc"]
p["UpdatedUtc"]=checkpoint["UpdatedUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L03_SOURCE_SKILL_RICH_FIRSTPARTY_10_EXAMPLES_4_TRY_6_CONCEPTS_3_TABLES_IN_PROGRESS")
