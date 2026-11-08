from pathlib import Path
import json,datetime,math
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert len(a["OriginalWorkedExamples"])==6 and a["OriginalExerciseMathVerified"]==0
worked=[
("Graph exponential growth and decay","Graph f(x)=3^x and g(x)=(1/3)^x for x=-2,-1,0,1,2.","For f use 1/9,1/3,1,3,9; g uses 9,3,1,1/3,1/9. Both have domain R, range positive, horizontal y=0; reflection across y-axis."),
("Translate exponential graph","Graph g(x)=2^(x−2)+3 from parent f(x)=2^x.","Shift right 2 and up 3, horizontal y=3; domain R, range (3,∞), y-intercept (0,13/4)."),
("Approximate equation from graph","Estimate x satisfying 2^x=10 to three decimals using a function graph.","x≈3.322, between 3 and 4. Compare 2^3=8 and 2^4=16; use numerical refinement."),
("Apply vertical stretch","Graph g(x)=3·2^x and compare with f(x)=2^x.","Every y-value is multiplied by3; y-intercept (0,3); horizontal y=0; domain R, range (0,∞)."),
("Reflect exponential graph","Reflect f(x)=2^(x+1) about the x-axis.","g(x)=−2^(x+1), y-intercept (0,−2), horizontal y=0; range (−∞,0)."),
("Create from transformation description","Base 2^x: shift left1, reflect in x-axis, shift up4.","g(x)=−2^(x+1)+4, horizontal y=4, range (−∞,4), y-intercept (0,2).")
]
tries=[
("Growth and reciprocal-base reflection","Compare y=4^x and y=(1/4)^x at x=−1,0,1.","(1/4,1,4) versus (4,1,1/4); reflect in y-axis."),
("Vertical and horizontal shift","Graph h(x)=3^(x−1)−2 and give asymptote and range.","Shift right1/down2; y=−2; range (−2,∞)."),
("Solve graph equation","Approximate x where 3^x=7.","x≈ln7/ln3≈1.771."),
("Vertical stretch","Give formula and intercept for stretching y=(1/2)^x by factor 5.","y=5(1/2)^x, y-intercept (0,5)."),
("Reflection","Reflect y=3^x about the x-axis and identify range.","y=−3^x; range (−∞,0)."),
("Function from description","Reflect y=2^x across y-axis then shift up1.","g(x)=2^(−x)+1; horizontal y=1, range (1,∞).")
]
assert len(worked)==6 and len(tries)==6
assert abs(math.log(10,2)-3.321928094887362)<1e-12
assert abs(math.log(7,3)-1.7712437491614221)<1e-10
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L02","ContentVersion":"g12-u04-l02-firstparty-exponential-graphs-draft",
"Lessons":[{"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L02","CultureCode":"en",
"Title":"Graphs of Exponential Functions","ExplanationParagraphs":[
"Exponential graphs f(x)=a·b^x have domain all real values and, when a>0, strictly positive outputs; their horizontal asymptote is y=0.",
"For f(x)=a·b^(x−h)+k, horizontal translation h and vertical translation k move the asymptote to y=k. A negative a reflects across the horizontal baseline.",
"Reflection across the y-axis replaces x by −x; f(−x) changes growth direction without changing the y-intercept.",
"A vertical scale factor multiplies the distances of curve points from their baseline; use transformed anchor points and explicit range/domain.",
"To estimate solutions of a b^x+d=c graphically, locate curve/line intersection, then refine numerically to the required precision.",
"Independent Cool-down: (1) Graph g(x)=2^(x−1)−3 and give domain/range/asymptote. (2) Reflect f(x)=3^x about y-axis. (3) Estimate 2^x=6 from graphical reasoning."
]+[f"Independent Try It {i}: {q}"for i,(_,q,ans)in enumerate(tries,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Transform model graph and check asymptote, domain, range and intercepts.","Steps":[ans],"Answer":ans,"Check":"Calculate plotted anchor values and check transformation direction."}for name,q,ans in worked],
"KeyConcepts":[{"Title":"Parent exponential graph","Definition":"f(x)=b^x, b>0, b≠1.","Rule":"Domain R, range (0,∞), y-intercept 1, horizontal asymptote 0.","Example":"3^x."},
{"Title":"Horizontal/vertical shift","Definition":"g(x)=a b^(x−h)+k shifts parent curve by (h,k).","Rule":"Horizontal asymptote is y=k; sign a sets above/below.","Example":"2^(x−2)+3."},
{"Title":"Reflections","Definition":"f(−x) reflects across y-axis and −f(x) reflects across x-axis.","Rule":"Keep careful track of sign placement inside versus outside exponent.","Example":"−2^(x+1)."},
{"Title":"Visual numerical approximation","Definition":"Solve f(x)=c by reading intersection; refine to requested precision.","Rule":"Graph-only values are approximate and should be checked numerically.","Example":"2^x=10 gives x≈3.322."}],
"Visuals":[{"Kind":"RatioTable","Title":"Exponential y-axis reflection","Description":"Independent table for 3^x and 3^(−x).","PrimaryLabel":"Input x","SecondaryLabel":"Outputs","Rows":[{"Label":"x","Cells":["−2","−1","0","1","2"]},{"Label":"3^x","Cells":["1/9","1/3","1","3","9"]},{"Label":"3^(−x)","Cells":["9","3","1","1/3","1/9"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Use transformed anchor points.","Do not mistake horizontal asymptotes for points the curve must cross.","Reflections in x/y axes differ.","Range shifts depend on sign of vertical multiplier.","Approximate graphical roots need refinement."],
"CommonMistakes":[{"Mistake":"Shift right when exponent is x+2.","WhyWrong":"f(x+2) shifts LEFT two units.","Correction":"Use x−h for right shift by h."},{"Mistake":"Treat −2^x as (−2)^x.","WhyWrong":"The first is reflected positive-base exponential; the second is not real for many noninteger x.","Correction":"Keep the negative scalar outside the exponential."}]}]}
rp_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l02.rich-lesson-v2.json"
(root/rp_rel).write_text(json.dumps(rich,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
tp_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l02-firstparty-teacher-led.json"
(root/tp_rel).write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
"LessonCode":rich["Lessons"][0]["LessonCode"],
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(_,q,ans)in enumerate(tries,1)],
"IndependentCoolDown":{"StudentQuestions":["Analyze g(x)=2^(x−1)−3 for asymptote, intercept, domain, range.","Reflect f(x)=3^x over the y-axis and give formula.","Estimate intersection of y=2^x and y=6 to 3 decimals."],
"TeacherExpectedMath":["Asymptote y=−3, intercept (0,−5/2), domain R, range (−3,∞).","g(x)=3^(−x)=(1/3)^x.","x≈2.585."],
"OriginalPublisherCoolDownCopied":False},
"FirstPartyIndependentStudentMathQuestions":[],"AuthenticatedRuntime":"NOT_RUN","StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["FirstPartyWorkedExamplesAuthored"]=6;a["IndependentTryItsAuthored"]=6
a["StudentRichV2"]=rp_rel;a["TeacherAssessment"]=tp_rel
a["BlockingReasons"]=["54_SOURCE_EXERCISES_REQUIRE_INDIVIDUAL_MATH_REVIEW","13_ORIGINAL_FIGURES_REQUIRE_SKILL_ALTERNATIVES","STAGE_C_TEACHER_MARKING_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_6_WORKED_6_TRYIT_DRAFT_AUTHORED_UNCLOSED")
