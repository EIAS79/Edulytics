from pathlib import Path
import json,re,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source_path=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson007.json")
s=json.loads(source_path.read_text(encoding="utf-8"))
assert s["Section"]=="3.7" and (s["Pages"][0]["PdfPage"],s["Pages"][-1]["PdfPage"])==(293,313)
assert s["SourceSha256"]=="f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d"
text="\n".join(p["Text"] for p in s["Pages"])
examples=[(int(n),t.strip()) for n,t in re.findall(r"Example\s+(\d+)\s+([^\n]{1,130})",text,re.I)]
tryits=sorted({int(n) for n in re.findall(r"Try It\s*#?(\d+)\b",text,re.I)})
figures=sorted({int(n) for n in re.findall(r"Figure\s+(\d+)\b",text,re.I)})
assert [n for n,_ in examples]==list(range(1,13))
assert tryits==list(range(1,9)) and figures==list(range(1,24))
categories=[("VERBAL",1,5),("DOMAIN",6,9),("DOMAIN_ASYMPTOTES",10,19),("INTERCEPTS",20,24),("LOCAL_END_BEHAVIOR",25,34),("GRAPH_TRANSFORMS",35,38),("SKETCH_GRAPH_FROM_FEATURES",39,50),("CONSTRUCT_FROM_FEATURES",51,54),("INTERPRET_GIVEN_GRAPH",55,64),("NUMERIC_LIMIT_TABLES",65,69),("GRAPH_SIGN_INEQUALITY",70,74),("REMOVABLE_HOLES",75,79),("REAL_WORLD_RATIONAL_MODEL",80,88)]
assert sum(b-a+1 for _,a,b in categories)==88
exercises=[{"Number":n,"SourceSection":"3.7","TaskFamily":label,"AcademicSourceVerified":True,"FirstPartyIndependentMathQuestionVerified":False,"TeacherAnswerVerified":False,"RequiresGraph":label in ("GRAPH_TRANSFORMS","SKETCH_GRAPH_FROM_FEATURES","INTERPRET_GIVEN_GRAPH","GRAPH_SIGN_INEQUALITY"),"SourceOriginalGraphicCopied":False} for label,a,b in categories for n in range(a,b+1)]
assert [r["Number"] for r in exercises]==list(range(1,89))
objectives=["Use arrow notation and limits near discontinuities","Solve applications involving rational functions","Determine rational function domains including excluded inputs and holes","Find vertical asymptotes from uncancelled denominator factors","Find horizontal and slant asymptotes using polynomial degrees and division","Graph rational functions using intercepts, signs, holes and asymptotic behavior"]
worked=[
("Arrow notation","For f(x)=1/(x−2), determine behavior as x approaches 2 from each side.","As x→2−, f(x)→−∞; as x→2+, f(x)→+∞.","The denominator changes from negative to positive through zero."),
("Transform a reciprocal","Graph g(x)=−2/(x+3)+1 and state both asymptotes.","Vertical x=−3; horizontal y=1; g(−2)=−1.","Apply translation left 3, up 1 and vertical reflection/stretch by 2."),
("Rational concentration model","A tank has 12 kg solute in 100 L and adds 2 kg/min and 5 L/min. Express the concentration after t minutes and its long-run level.","C(t)=(12+2t)/(100+5t) kg/L, t≥0; as t→∞, C(t)→2/5 kg/L.","Divide total solute mass by total volume; compare leading coefficients."),
("Domain restrictions","Find the domain of h(x)=(x+1)/(x²−9).","All real x except −3 and 3.","Factor denominator (x−3)(x+3); never include zero-denominator points."),
("Vertical asymptotes","Find vertical asymptotes of (x+1)/((x−2)(x+4)).","x=2 and x=−4.","Neither denominator factor cancels with the numerator."),
("Hole versus asymptote","For f(x)=((x−2)(x+3))/((x−2)(x+1)), identify the hole and vertical asymptote.","Hole at (2,5/3); vertical asymptote x=−1; domain excludes both 2 and −1.","Cancel only to compute the limit; original function remains undefined at x=2."),
("Slant asymptote","Find the end behavior of (x²+2x+3)/(x+1).","f(x)=x+1+2/(x+1); slant asymptote y=x+1.","Polynomial division gives the linear quotient and vanishing remainder ratio."),
("Horizontal asymptote","Find the horizontal asymptote of (3x²+1)/(2x²+5).","y=3/2.","Equal polynomial degrees: leading coefficient ratio."),
("All vertical and horizontal asymptotes","Analyze (x²−1)/(x²−4).","Vertical asymptotes x=−2, x=2; horizontal y=1; zeros x=±1.","Factor numerator and denominator and compare degrees."),
("Intercepts of rational function","Find intercepts and asymptotes of (x−4)/(x+2).","x-intercept (4,0), y-intercept (0,−2), vertical x=−2, horizontal y=1.","Numerator zero produces x-intercept only if denominator nonzero."),
("Graph from critical points","Sketch f(x)=x/(x²−1) with intercepts and asymptotes.","Intercept (0,0); vertical x=±1; horizontal y=0. On (1,∞) graph is positive, on (0,1) negative.","Use signs between critical inputs to place branches."),
("Construct a rational function","Construct f with simple vertical asymptotes −2 and 3, zero at 1 and y-intercept 1/3.","f(x)=2(x−1)/((x+2)(x−3)).","Choose factored numerator/denominator and solve f(0)=1/3 for scale.")
]
try_tasks=[
("One-sided behavior","For f(x)=−1/(x+4), determine f(x) as x→−4− and x→−4+.","Left +∞, right −∞."),
("Transformation","For g(x)=3/(x−1)−2 identify both asymptotes.","Vertical x=1; horizontal y=−2."),
("Concentration","A tank holds 30 g in 60 L; add 3 g/min and 2 L/min. Write c(t) and its long-run level.","(30+3t)/(60+2t), tends to 3/2 g/L."),
("Domain","Find domain of (x−5)/(x²−16).","All reals except −4 and 4."),
("Removable discontinuity","For (x²−9)/(x−3), locate the hole.","Hole at (3,6); the original domain excludes 3."),
("Asymptotes by degree","Find end asymptote for (4x²+3)/(x²+1).","Horizontal y=4."),
("Intercepts","Find x/y intercepts of (x+3)/(x−2).","x-intercept (−3,0); y-intercept (0,−3/2)."),
("Design rational model","Construct a rational function with zero x=2, vertical asymptote x=−1, horizontal asymptote y=3.","f(x)=3(x−2)/(x+1).")
]
assert len(worked)==12 and len(try_tasks)==8
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L07"
explanations=[
"A rational function is a quotient of polynomials; the original denominator cannot be zero, even where a common factor cancels.",
"Arrow notation x→a− or x→a+ describes one-sided input behavior. A vertical asymptote often signals diverging output, while a removable discontinuity is a finite-valued hole.",
"After factoring numerator and denominator, cancel common factors to locate holes but retain original domain exclusions. Remaining real denominator zeros yield vertical asymptotes.",
"Horizontal asymptotes follow degree comparison: numerator degree lower gives y=0; equal gives the ratio of leading coefficients; higher generally requires polynomial division for polynomial end behavior.",
"A graph can cross a horizontal asymptote but does not take values at a vertical asymptote's x-coordinate; a hole is an excluded point on the simplified curve.",
"Use intercepts, limits, sign charts and asymptotes to sketch each branch. Contextual units and positive-domain restrictions are essential in rational models."
]+["Independent Try It "+str(i)+": "+q for i,(_,q,a) in enumerate(try_tasks,1)]
visual={"Kind":"RatioTable","Title":"Rational behavior by numerator/denominator degree","Description":"Independent degree classification rather than a copied source illustration.","PrimaryLabel":"Degrees","SecondaryLabel":"Long-run behavior","Rows":[{"Label":"deg numerator < deg denominator","Cells":["horizontal asymptote","y=0"]},{"Label":"equal degree","Cells":["horizontal asymptote","leading-coefficient ratio"]},{"Label":"degree numerator one higher","Cells":["linear slant if quotient has degree 1","polynomial division"]}],"Items":[],"SecondaryItems":[]}
lesson={"LessonCode":lesson_code,"CultureCode":"en","Title":"Rational Functions","ExplanationParagraphs":explanations,"WorkedExamples":[{"Title":a,"Question":b,"Method":"Factor and analyze the original domain, limits, intercepts and end behavior as appropriate.","Steps":[c],"Answer":c,"Check":d}for a,b,c,d in worked],"KeyConcepts":[{"Title":"Rational function and domain","Definition":"A quotient p(x)/q(x), q(x)≠0.","Rule":"Exclude every zero of the original denominator.","Example":"(x−2)/(x²−4) excludes x=−2 and x=2."},{"Title":"Removable hole","Definition":"A common factor cancels while its root remains excluded from the domain.","Rule":"Evaluate the simplified expression at the excluded input to find the hole's y-coordinate.","Example":"(x−2)(x+3)/((x−2)(x+1)) has hole (2,5/3)."},{"Title":"Vertical asymptote","Definition":"A vertical line where the function grows unbounded on at least one side.","Rule":"Check uncancelled real denominator factors and one-sided behavior.","Example":"1/(x+2) has vertical x=−2."},{"Title":"Horizontal/slant asymptotes","Definition":"Lines approached as input grows without bound.","Rule":"Use degree comparisons and long division; horizontal lines may be crossed.","Example":"(x²+2x+3)/(x+1)=x+1+2/(x+1)."}],"Visuals":[visual],"Videos":[],"SummaryPoints":["Original denominator zeros are always excluded.","Cancel common factors to distinguish holes from vertical asymptotes.","A horizontal asymptote can be crossed.","Use intercepts and branch signs in addition to asymptotes.","Context models require units and permissible input ranges."],"CommonMistakes":[{"Mistake":"Treating a cancelled factor as an allowed input.","WhyWrong":"Cancelling does not restore undefined domain values.","Correction":"Keep the original denominator restrictions and graph a hole."},{"Mistake":"Claiming every horizontal asymptote is uncrossable.","WhyWrong":"Horizontal asymptotes describe end behavior, not forbidden output values.","Correction":"Solve f(x)=asymptote value and check any finite intersections."},{"Mistake":"Forgetting that a denominator zero may create a hole.","WhyWrong":"A common numerator factor cancels and leaves finite limit.","Correction":"Factor first and distinguish cancelled from remaining factors."}]}
content_path=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l07.rich-lesson-v2.json"
content_path.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L07","ContentVersion":"g12-u03-l07-firstparty-rational-functions-draft-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
teacher_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
teacher_path.parent.mkdir(parents=True,exist_ok=True)
teacher_path.write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson_code,"OriginalNumberedSourceExercises":88,"IndependentTryItStudentQuestions":[{"Number":i,"Question":q,"TeacherExpectedMath":a,"TeacherReviewStatus":"DRAFT_NOT_STUDENT_RUNTIME_VERIFIED"}for i,(_,q,a) in enumerate(try_tasks,1)],"OriginalExerciseFirstPartyCoverageStatus":"PENDING_PER_EXERCISE_MATH_AND_GRAPH_CHECK","OriginalSourceImagesCopied":False,"StageDStudentTeacherRuntime":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":7,"SourceSection":"3.7","SourceSHA256":s["SourceSha256"],"PdfPages":[293,313],"StartedAtUtc":now,"SourceLearningObjectives":objectives,"SourceWorkedExampleAnchors":[{"Number":n,"SourceTitle":t}for n,t in examples],"SourceTryItNumbers":tryits,"SourceFigureNumbers":figures,"SourceNumberedExerciseLedger":exercises,"SourceExerciseCategoryCounts":[{"Family":c,"First":a,"Last":b,"Count":b-a+1}for c,a,b in categories],"FirstPartyDraftRichV2":content_path.relative_to(root).as_posix(),"FirstPartyDraftTeacherGuide":teacher_path.relative_to(root).as_posix(),"OriginalSourcePdfReRead":False,"OriginalSourceGraphicsCopied":False,"OriginalExerciseAlternativesReviewed":0,"OriginalFigureAlternativesReviewed":0,"CCSSAlignment":"PENDING_INDIVIDUAL_SKILL_CONFIRMATION","BlockingReasons":["88_ORIGINAL_EXERCISES_NEED_INDIVIDUAL_FIRST_PARTY_MATH_AND_TEACHER_RUBRIC_REVIEW","23_SOURCE_FIGURE_SKILLS_NEED_INDEPENDENT_GRAPH_ALTERNATIVES_AND_MATH_CHECK","STAGE_C_REUSE_EXTEND_DECISIONS_AND_STUDENT_ASSESSMENT_ROUTE_NOT_COMPLETE"],"Status":"A_C_IN_PROGRESS","ReadyForStageD":False,"Tests":"NOT_RUN","CIRun":False,"StageD":"NOT_RUN"}
(base/"u03-l07-source-inventory-and-gap-audit.json").write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=json.loads((base/"full-grade-progress.json").read_text(encoding="utf-8"))
assert progress["LastClosedLessonOrdinal"]==17 and progress["FirstIncompleteLessonOrdinal"]==18
entry=progress["LessonEntries"][17]
assert entry["Ordinal"]==18 and entry["SourceSection"]=="3.7" and not entry["ReadyForStageD"]
entry.update({"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,"EvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-source-inventory-and-gap-audit.json","FirstPartyRichV2Path":content_path.relative_to(root).as_posix(),"SourceWorkedExampleAnchorsIdentified":12,"SourceTryItActivitiesIdentified":8,"SourceFigureAnchorsIdentified":23,"SourceExercisesIdentified":88,"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
progress["UpdatedUtc"]=now
(base/"full-grade-progress.json").write_text(json.dumps(progress,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03L07_DRAFT_AUTHORED_12_EXAMPLES_8_TRYITS_88_LEDGER_STILL_OPEN")
