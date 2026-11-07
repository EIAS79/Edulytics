from pathlib import Path
import json,re,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
s=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson009.json").read_text(encoding="utf8"))
p=json.loads((base/"full-grade-progress.json").read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==19 and p["FirstIncompleteLessonOrdinal"]==20 and s["Section"]=="3.9" and len(s["Pages"])==7
t="\n".join(page["Text"]for page in s["Pages"])
examples=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\n]{1,100})",t,re.I)]
tryits=sorted({int(x)for x in re.findall(r"Try It\s*#?(\d+)",t,re.I)})
figs=sorted({int(x)for x in re.findall(r"Figure\s+(\d+)",t,re.I)})
assert len(examples)==4 and tryits==[1,2,3] and figs==[1,2,3,4]
categories=[("VERBAL_VARIATION",1,3),("FORMULA_DIRECT_VARIATION",4,9),("FORMULA_INVERSE_VARIATION",10,15),("FORMULA_JOINT_VARIATION",16,23),("NUMERIC_DIRECT_VARIATION",24,28),("NUMERIC_INVERSE_VARIATION",29,33),("NUMERIC_JOINT_VARIATION",34,40),("GRAPH_VARIATION",41,45),("KEPLER_LAW",46,50),("REAL_WORLD_VARIATION",51,60)]
assert sum(b-a+1 for _,a,b in categories)==60
tasks=[{"Number":n,"Family":category,"SourceSection":"3.9","SourceMathIndividuallyReviewed":False,
"IndependentStudentQuestionVerified":False,"IndependentTeacherMathVerified":False,"OriginalTextCopied":False}for category,a,b in categories for n in range(a,b+1)]
assert [x["Number"]for x in tasks]==list(range(1,61))
worked=[
("Direct variation and commission","Commission C varies directly with sale price s. If a sale of $5000 pays $400, find C for a $7500 sale.","k=400/5000=0.08; C=0.08s; for s=7500, C=$600."),
("Power variation","y varies directly with x cubed. If y=16 at x=2, find y at x=4.","k=16/8=2; y=2x³, so y(4)=128."),
("Inverse variation","Intensity varies inversely with square of distance. If I=27 at distance 2 m, find I at distance 6 m.","k=27×4=108, I=108/d²; at d=6, I=3."),
("Joint variation","Cost C varies jointly with workers w and hours h and inversely with productivity p. If C=60 when w=3,h=4,p=2, compute C for w=5,h=6,p=3.","C=kwh/p, k=60×2/(3×4)=10; C=10×5×6/3=100.")
]
trytasks=[
("Direct square","y varies directly as x². When x=3, y=18. Find y for x=5.","k=2, y(5)=50."),
("Inverse cube","y varies inversely as x³. When x=2, y=24. Find y for x=4.","k=192, y(4)=3."),
("Joint mixed powers","q varies directly as a²b and inversely as c. If q=12 at a=2,b=3,c=2, find q at a=3,b=4,c=6.","k=2, q=2×9×4/6=12.")
]
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L09"
lesson={"LessonCode":lesson_code,"CultureCode":"en","Title":"Modeling Using Variation",
"ExplanationParagraphs":["Three original-source goals: model direct, inverse, and joint variation, determine the shared constant and evaluate the model at changed inputs.",
"Direct variation is y=kxⁿ; for n=1 the graph is a line through the origin. For other powers the graph may be nonlinear even though variation is direct.",
"Inverse variation is y=k/xⁿ, where x≠0, and its graph displays domain exclusions and decreasing or increasing branch behavior depending on signs and powers.",
"Joint variation uses a single nonzero constant of variation and products of powers or quotients; use source values to find k and preserve physical units.",
"An applied law can have direct dependence on some variables and inverse dependence on others; do not confuse 'inverse variation' with 'inverse function'.",
"Independent first-party Cool-down: (a) Model revenue varying directly with units. (b) Model light intensity varying inversely with squared distance. (c) A jointly varying force F=kv²A is calibrated and evaluated."]+[f"Independent Try It {i}: {q}"for i,(_,q,ans)in enumerate(trytasks,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Translate variation wording, solve k from a measured point, substitute the target values and check units.","Steps":[answer],"Answer":answer,"Check":"Substitute the calibration point and verify the resulting constant."}for title,question,answer in worked],
"KeyConcepts":[{"Title":"Direct variation","Definition":"y=kxⁿ for a specified n and k≠0.","Rule":"Compute k=y/xⁿ, x≠0.","Example":"y=3x²."},
{"Title":"Inverse variation","Definition":"y=k/xⁿ, x≠0.","Rule":"Compute k=yxⁿ.","Example":"y=12/x²."},
{"Title":"Joint variation","Definition":"One quantity is proportional to a product or quotient of powers of multiple quantities.","Rule":"Use one constant k, not one per independent variable.","Example":"F=kv²A."}],
"Visuals":[{"Kind":"RatioTable","Title":"Distinguish direct versus inverse variation","Description":"First-party variation sample based on exact values.","PrimaryLabel":"x","SecondaryLabel":"y value","Rows":[{"Label":"x","Cells":["1","2","4"]},{"Label":"Direct y=8x","Cells":["8","16","32"]},{"Label":"Inverse y=8/x","Cells":["8","4","2"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Identify direct/inverse/joint dependence and powers.","Use one constant k.","Check dimensions and calibration measurements.","Direct variation may be nonlinear for powers other than one.","Avoid conflating inverse variation and inverse function."],
"CommonMistakes":[{"Mistake":"Model all direct variation with y=kx only.","WhyWrong":"Direct powers x², x³ and radicals are valid models.","Correction":"Preserve the specified exponent n."},{"Mistake":"Assign separate constants to each jointly varying factor.","WhyWrong":"A joint variation law uses one k.","Correction":"Write y=kx^az^b/w^c then solve k once."}]}
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l09.rich-lesson-v2.json"
(root/rich_rel).write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L09","ContentVersion":"g12-u03-l09-independent-variation-abc-draft-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
t_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l09-firstparty-teacher-led.json"
(root/t_rel).write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson_code,
"OriginalSourceNumberedExercises":60,"IndependentTryIt":[{"Number":n,"StudentQuestion":q,"TeacherExpectedMath":answer}for n,(_,q,answer)in enumerate(trytasks,1)],
"IndependentCoolDown":{"StudentQuestions":["Commission R=ks, given R=90 at s=1500, evaluate for s=2000.","I=kd⁻²; I=20 at d=2, evaluate at d=5.","F=kv²A; F=36 at v=3,A=2, evaluate at v=6,A=4."],
"TeacherExpectedMath":["k=0.06; R(2000)=120.","k=80; I(5)=80/25=3.2.","k=2; F=2×36×4=288."],"CopiedRestrictedPublisherCoolDown":False},
"FirstPartyIndependentStudentMathQuestions":[],"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l09-source-inventory-and-gap-audit.json"
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":9,"SourceSection":"3.9",
"SourceSha256":s["SourceSha256"],"PdfPages":[325,331],"StartedAtUtc":now,
"LearningObjectives":["Solve direct variation","Solve inverse variation","Solve joint variation"],
"OriginalWorkedExamples":examples,"OriginalTryItNumbers":tryits,"OriginalFigureNumbers":figs,
"OriginalNumberedExerciseLedger":tasks,"IndependentWorkedExamples":4,"IndependentTryIts":3,
"FirstPartyRichV2":rich_rel,"FirstPartyTeacherAssessment":t_rel,
"OriginalExerciseMathVerified":0,"FirstPartyIndependentExerciseTasksAuthored":0,
"OriginalFigureSkillAlternatives":0,"StageC":"PENDING",
"BlockingReasons":["60_SOURCE_EXERCISES_REQUIRE_MATH_REVIEW_AND_INDEPENDENT_TEACHER_KEYS","FOUR_ORIGINAL_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_REUSE_ASSESSMENT_ROUTES_PENDING"],
"ReadyForStageD":False,"CI":"NOT_RUN","StageD":"NOT_RUN","Status":"A_C_IN_PROGRESS"}
(base/"u03-l09-source-inventory-and-gap-audit.json").write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][19]
assert row["Ordinal"]==20 and row["SourceSection"]=="3.9" and not row["ReadyForStageD"]
row.update({"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,"FirstPartyRichV2Path":rich_rel,
"EvidencePath":audit_rel,"OriginalSourceWorkedExamplesLocated":4,"OriginalSourceTryItsLocated":3,
"OriginalSourceFiguresLocated":4,"OriginalSourceExercisesLocated":60,
"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
(base/"full-grade-progress.json").write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03L09_60_SOURCE_INVENTORY_AND_4_WORKED_3_TRYIT_DRAFT_STILL_OPEN")
