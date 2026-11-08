from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson002.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.2" and len(source["Pages"])==19
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U02:L02"
examples=[
("Plot a line from three points","Graph f(x)=−x/2+3 using x=0,2,4.","Use (0,3),(2,2),(4,1), all collinear."),
("Slope and intercept graph method","Plot g(x)=−(2/3)x+4 from the intercept and slope.","Mark (0,4), go right3 down2 to (3,2) and (6,0)."),
("Transform identity to a line","Transform f(x)=x to h(x)=3x−2 by scaling and shifting.","Stretch output 3 then shift down2; intercept −2."),
("Match formulas to slopes and intercepts","Compare y=2x+1, y=−2x+1, y=2x−3 and y=x/2+1 on independent axes.","Two positive slopes 2 are parallel; negative slope −2 falls, 1/2 rises gently; intercepts 1 and −3."),
("Find an x-intercept","For f(x)=3x/4−6 find the horizontal intercept.","Solve 0=3x/4−6, x=8; intercept (8,0)."),
("Read constant horizontal line","What equation is represented by a horizontal line through (0,−5)?","y=−5 for all real x; slope zero."),
("Read a vertical line","Write the line through points (3,2),(3,−1) and explain why it is not y=f(x).","x=3; multiple y for one x means not single-valued as y=f(x)."),
("Identify slope relationships","Compare f(x)=2x+1, g(x)=2x−3 and h(x)=−x/2+4.","f and g parallel; f and h perpendicular as 2·(−1/2)=−1."),
("Parallel through point","Find line through (2,−1) parallel to y=4x+2.","y+1=4(x−2); y=4x−9."),
("Perpendicular through point","Find line through (6,0) perpendicular to y=3x+2.","m=−1/3; y=−(x−6)/3=−x/3+2."),
("Perpendicular to line through points","Find perpendicular to the line through (−2,3),(4,5), passing through (3,2).","Original m=(5−3)/(4−(−2))=1/3; perpendicular m=−3; y=−3x+11."),
("Solve two lines' intersection","Find the intersection of y=2x−3 and y=5−x.","2x−3=5−x gives x=8/3, y=7/3."),
("Break-even linear business","Fixed cost $2500, production $6/unit and revenue $16/unit. Find break-even units and revenue.","C(n)=2500+6n,R(n)=16n, equality at n=250, amount $4000.")
]
tries=[
("Graph by coordinates","Plot f(x)=−3x/4+6 using x=0,4,8.","Points (0,6),(4,3),(8,0)."),
("Find negative-input point","For f(x)=−2x/3+5 find a point with negative x.","For example x=−3 gives f(−3)=7."),
("Transform a line","Describe identity transformations producing y=2x+4.","Vertical stretch2 then shift up4."),
("Find zero","Find x-intercept of y=x/4−4.","(16,0)."),
("Parallel and perpendicular through origin","Given h(x)=2x−4, find a parallel line through origin and a perpendicular one.","y=2x; y=−x/2."),
("Perpendicular to two-point line","Line through (−2,−15),(2,−3): find a perpendicular through (6,4).","Original slope3; perpendicular m=−1/3, y=−x/3+6."),
("Analyze two intersecting models","For j(t)=3t−4, h(t)=5−t: find each intercept, slope signs and intersection.","j y-int −4, x-int 4/3, m=3 increasing; h y-int5,x-int5,m=−1 decreasing; meet at (9/4,11/4).")
]
concept=[
("Graphing by points","Substitute 2 or more x-coordinates into a linear function.","Three points provide a useful error check.","f(x)=x+1 goes through (0,1),(1,2)."),
("Slope-intercept graph","Plot b at (0,b) then use slope m as rise/run.","A negative slope lowers y as x increases.","m=−2/3 descends2 for 3 right."),
("Line from graph","Read any two distinct plotted points to find slope, then solve intercept.","Avoid reading noisy grid labels without point confirmation.","(0,4),(2,8) yields y=2x+4."),
("Special lines","y=k is horizontal and constant; x=k is vertical and not a function y=f(x).","Horizontal slope0; vertical undefined.","x=3 fails vertical line test."),
("Parallel relationships","Distinct nonvertical lines are parallel when their slopes coincide.","Distinct y-intercepts distinguish parallel lines.","2x+1 and 2x−3."),
("Perpendicular relationships","Nonvertical/nonhorizontal perpendicular slopes multiply to −1.","Vertical is perpendicular to horizontal.","m=3 and m=−1/3."),
("Equation through point","Use y−y1=m(x−x1) with the required slope.","For a perpendicular slope take negative reciprocal.","m=4 through (2,−1) yields 4x−9."),
("Linear systems","At intersection the outputs of both functions are equal.","Solve f(x)=g(x) and substitute into one.","2x−3=5−x at x=8/3."),
("Break-even","Sales revenue equals total fixed plus variable production cost.","Check units and admissible nonnegative quantities.","16n=2500+6n gives n=250.")
]
visuals=[
{"Kind":"RatioTable","Title":"Same slope, different intercepts","Description":"Parallel first-party lines share slope but not offset.","PrimaryLabel":"x","SecondaryLabel":"function","Rows":[{"Label":"x","Cells":["0","1","2"]},{"Label":"f=2x+1","Cells":["1","3","5"]},{"Label":"g=2x−3","Cells":["−3","−1","1"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"Intersecting line outputs","Description":"Two lines meet when their y-output is the same.","PrimaryLabel":"x","SecondaryLabel":"y","Rows":[{"Label":"x","Cells":["0","2","8/3"]},{"Label":"2x−3","Cells":["−3","1","7/3"]},{"Label":"5−x","Cells":["5","3","7/3"]}],"Items":[],"SecondaryItems":[]}
]
cool={"Prompt":"(a) Sketch y=−2x+4 from its intercept and rise/run. (b) Find a perpendicular through (1,2) to y=2x−3. (c) Solve intersection of y=x+1 and y=5−x.","TeacherExpectedMathematics":"(a) points (0,4),(1,2),(2,0); (b) y−2=−(x−1)/2, y=−x/2+5/2; (c) (2,3).","OriginalPublisherRestrictedTeacherOnlyCoolDownCopied":False}
lesson={"CultureCode":"en","LessonCode":code,"Title":"Graphs of Linear Functions",
"ExplanationParagraphs":[
"Learning goals: graph lines by plotting points and using slope/intercepts; infer a line formula from a graph; identify parallel and perpendicular relationships; construct lines through points; solve two-line systems.",
"To graph f(x)=mx+b, mark (0,b), then move horizontally and vertically according to rise/run. Check a second point for accuracy.",
"Two distinct nonvertical lines are parallel iff they have equal slope. Perpendicular nonzero slopes multiply to −1. A vertical line x=c is perpendicular to a horizontal y=k.",
"Find line formulas from two coordinates or from an intercept and slope. A vertical line cannot be expressed as a single-valued y=f(x) over its x-coordinate.",
"The intersection of two lines has the same x-input and y-output for both equations. Algebraically solve f(x)=g(x) and substitute back.",
"Independent Edulytics Cool-down: "+cool["Prompt"]]+[f"Independent Try It {i} — {t}. {q}"for i,(t,q,a)in enumerate(tries,1)],
"WorkedExamples":[{"Title":t,"Question":q,"Method":"Use coordinates, equation slope, intercepts and exact linear algebra.","Steps":[answer],"Answer":answer,"Check":"Verify stated coordinates or substitute into both lines."}for t,q,answer in examples],
"KeyConcepts":[{"Title":t,"Definition":definition,"Rule":rule,"Example":example}for t,definition,rule,example in concept],
"Visuals":visuals,"Videos":[],
"SummaryPoints":["Plot at least two exact points for each line.","Slope/intercept determine its graph.","Equal slopes yield parallel lines; negative reciprocal slopes yield perpendicular lines.","Vertical lines are not functions y=f(x).","Intersection solves f(x)=g(x)."],
"CommonMistakes":[{"Mistake":"Reading slope as run/rise.","WhyWrong":"Slope m equals output change divided by input change.","Correction":"Use Δy/Δx."},
{"Mistake":"Calling any crossing lines perpendicular.","WhyWrong":"Only right-angle intersections are perpendicular.","Correction":"Check m1*m2=−1 or horizontal/vertical pair."},
{"Mistake":"Using a rounded plotted intersection as an exact algebraic solution.","WhyWrong":"Small graph scale errors can change the answer.","Correction":"Solve f(x)=g(x) exactly."}]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l02.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U02:L02","ContentVersion":"g12-u02-l02-source-skill-independent-authoring-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u02-l02-independent-seven-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalSourceTryItSkillCount":7,"IndependentFirstPartyTryIts":7,
"TeacherOnlyAnswers":[{"Number":i,"Answer":a}for i,(t,q,a)in enumerate(tries,1)],
"IndependentCoolDown":cool,"OriginalPublisherGraphicsCopied":False,"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":2,
"OriginalBookSection":"2.2","SourceSHA256":source["SourceSha256"],
"SourcePdfPages":[158,176],
"SourceWorkedExamples":13,"SourceTryIts":7,"SourceNumberedExercises":76,
"FirstPartyWorkedExamples":13,"FirstPartyTryIts":7,"FirstPartyKeyConcepts":9,
"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"SourceNumberedAssessmentSkillReview":"PENDING","SourceGraphFigureSkillReview":"PENDING",
"FullPublisherTextImagePixelParityCertified":False,
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u02-l02-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==8 and p["FirstIncompleteLessonOrdinal"]==9
e=p["LessonEntries"][8]
e["ACStatus"]="A_C_IN_PROGRESS";e["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
e["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l02-abc-checkpoint.json"
e["ReadyForStageD"]=False
e["BlockingReasons"]=["76 source-numbered exercise skill teacher review contracts pending","24 original numbered figure skill anchors pending"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U02_L02_RICHV2_13_EXAMPLES_7_TRYITS_9_CONCEPTS_AND_COOLDOWN_AUTHORED")
