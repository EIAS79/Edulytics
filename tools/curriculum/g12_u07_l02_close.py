from pathlib import Path
import json,math,subprocess,datetime
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pfile=b/"full-grade-progress.json";p=json.loads(pfile.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==36 and p["FirstIncompleteLessonOrdinal"]==37
qpath=b/"u07-l02-independent-questions.json";q=json.loads(qpath.read_text(encoding="utf8"))
Q=q["IndependentTasks"];assert len(Q)==56
Q[21]["TeacherMath"]="sin(π/2−π/3)=sin(π/6)=1/2."
Q[34]["TeacherMath"]="Not identical: x=π/18 makes sin3x cos6x=1/4 and its negative=−1/4."
assert math.isclose(math.sin(math.pi/2-math.pi/3),.5)
qpath.write_text(json.dumps(q,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
W=[
("Cosine difference","Compute cos(60°−45°) exactly.","(sqrt6+sqrt2)/4"),
("Cosine sum","Compute cos(60°+45°) exactly.","(sqrt2−sqrt6)/4"),
("Sine difference","Compute sin(75°−30°).","sqrt2/2"),
("Inverse-trig sum","Compute sin(arccos3/5+arcsin5/13).","(4/5)(12/13)+(3/5)(5/13)=63/65."),
("Tangent addition","Find tan(45°+30°).","2+sqrt3."),
("Multiple angles","Expand sin(a+b−c).","sin(a+b)cosc−cos(a+b)sinc, expand pair."),
("Cofunction","Express sin(π/7) using cosine.","cos(π/2−π/7)=cos(5π/14)."),
("Prove sine identity","Verify sin(π−t)=sint.","sinπcost−cosπsint=sint."),
("Prove tangent identity","Verify tan(π−t)=−tant.","tan(-t) after periodicity."),
("Applied slopes","For lines slopes1 and0, find angle between them.","tanθ=|1−0|/(1+0)=1 => θ=π/4."),
("Guy wire","Two rays are at20° and50° above horizontal; find angle between.","50°−20°=30°.")
]
T=[("Compute cos75°.","(sqrt6−sqrt2)/4"),("Compute sin15°.","(sqrt6−sqrt2)/4"),
("Compute tan75°.","2+sqrt3"),("Write sin(π/6) as cosine.","cosπ/3"),
("Verify tan(π−x)=−tanx.","By sine/cosine sum identities.")]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L02"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l02.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l02-firstparty-teacher-led.json"
def save(f,data):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L02",
"ContentVersion":"g12-u07-l02-independent-sum-difference-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Sum and Difference Identities",
"ExplanationParagraphs":["For two angles a,b, sin(a±b)=sina cosb ± cosa sinb.",
"For cosine, cos(a±b)=cosa cosb ∓ sina sinb. The sine sign and cosine sign patterns differ.",
"For tangent, tan(a±b)=(tana±tanb)/(1∓tana tanb) when all relevant tangents and denominator exist.",
"Use unit-circle special angles, quadrants and inverse-trig reference triangles for exact values.",
"Cofunction identities follow from complementary acute angles. Sum/difference formulas also establish trig parity.",
"Apply angle differences to slopes of two lines and angles between sightlines; retain orientation and domain constraints.",
"Independent Cool-down: compute sin75°, expand cos(x−π/6), and prove sin(π−x)=sinx."]+
[f"Independent Try It {i}: {question}"for i,(question,answer)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Use exact angle identities and check signs.",
"Steps":[answer],"Answer":answer,"Check":"Validate special angle values and original denominators."}for title,question,answer in W],
"KeyConcepts":[{"Title":"Sine addition","Definition":"sin(a+b)=sina cosb+cosa sinb","Rule":"Sign reverses for difference.","Example":"sin75°."},
{"Title":"Cosine addition","Definition":"cos(a+b)=cosa cosb−sina sinb","Rule":"Cos difference uses plus.","Example":"cos15°."},
{"Title":"Tangent addition","Definition":"tan(a+b)=(tana+tanb)/(1−tana tanb)","Rule":"Check individual tangent domains.","Example":"tan75°=2+sqrt3."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent special-angle addition and subtraction",
"Description":"First-party angle and exact sine/cosine table.",
"PrimaryLabel":"Angle","SecondaryLabel":"Values","Rows":[{"Label":"Angle","Cells":["15°","75°","105°"]},
{"Label":"sin","Cells":["(sqrt6−sqrt2)/4","(sqrt6+sqrt2)/4","(sqrt6+sqrt2)/4"]},
{"Label":"cos","Cells":["(sqrt6+sqrt2)/4","(sqrt6−sqrt2)/4","(sqrt2−sqrt6)/4"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Use sum/difference identities with proper signs.","Express exact values with unit circle.","Use inverse triangle ratios and retain tangent domains."],
"CommonMistakes":[{"Mistake":"Treat cos(a+b) as cosa cosb+sina sinb","WhyWrong":"Cosine sum uses negative cross term.","Correction":"Cos difference is the one with plus cross term."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":question,"TeacherExpectedMath":answer}for i,(question,answer)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Compute sin75° exact.","Expand cos(x−π/6).","Prove sin(π−x)=sinx."],
"TeacherExpectedMath":["(sqrt6+sqrt2)/4.","sqrt3/2 cosx+1/2 sinx.","sinπ cosx−cosπ sinx=sinx."],
"OriginalPublisherCoolDownCopied":False},"SourceMathSkillTasks":56,"StageD":"NOT_RUN"}
save(r/richrel,rich);save(r/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l02-eight-firstparty-figure-skills.json"
roles=["Source credited Denali photograph replaced with angle-of-elevation model",
"Independent unit circle reference table","Two independent unit circle vectors for cosine addition",
"Reference triangle QI with 3-4-5 sides","Reference triangle QIII with 5-12-13 sides",
"Complementary angles in independent right triangle","Two lines with specified slopes",
"Two guy-wire sightline angles"]
save(r/figrel,{"SchemaVersion":1,"Section":"7.2","OriginalFigures":8,
"IndependentAlternatives":[{"OriginalFigure":i,"Skill":role,"IndependentMathematicalIllustration":True,
"SourcePublisherImageCopied":False}for i,role in enumerate(roles,1)]})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l02-stage-c-56-teacher-routes.json"
save(r/routes,{"Section":"7.2","Count":56,"NewEngine":False,"Reuse":"RichV2 and manual teacher scoring",
"Routes":[{"SourceExercise":i,"TeacherAnswer":True,"RuntimeVerified":False}for i in range(1,57)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l02-source-and-academic-audit.json"
save(r/auditrel,{"Section":"7.2","SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPages":[585,598],
"BlueprintIncorrectPages":[586,599],"SourceWorked":11,"SourceTryIt":5,"SourceFigureSkills":8,
"NumberedExercises":56,"OriginalSourceMathSkillTasksReviewed":56,
"FirstPartyRichV2":richrel,"TeacherMathAssessment":trel,"FigureAlternatives":figrel,
"StageC":routes,"ReadyForStageD":True,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closure="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l02-abc-close-audit.json"
save(r/closure,{"Section":"7.2","Source":"OpenStax Precalculus 2017 original local book",
"LocalPdfPages":[585,598],"OriginalExercisesReviewed":56,"IndependentMathQuestionsWithKeys":56,
"IndependentWorked":11,"IndependentTryIt":5,"IndependentFigureSkillAlternatives":8,
"IndependentCoolDown":3,"SourcePublisherGraphicsCopied":False,
"NewMathEngine":False,"TeacherManualReuse":True,"Tests":"TARGETED_LOCAL_ONLY",
"ChecksActuallyRun":["Source section local page/exercise review","56 independent mathematical problems and teacher keys",
"11 worked 5 Try It","Angle special value spot-check and source-skill model descriptions",
"Git staged diff check"],"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row=p["LessonEntries"][36];assert row["Ordinal"]==37 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":585,"PdfPageEnd":598,
"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closure,"ClosedAtUtc":now})
assert p["LessonEntries"][37]["Ordinal"]==38
p.update({"LastClosedLessonOrdinal":37,"FirstIncompleteLessonOrdinal":38,"ActiveLessonOrdinal":38,
"NextLessonOrdinal":38,"ReadyForStageD":37,"SourceVerifiedGrade12Lessons":37,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_37_OF_73","UpdatedUtc":now})
save(pfile,p)
files=[str(pfile.relative_to(r)),richrel,trel,figrel,routes,auditrel,closure,str(qpath.relative_to(r)),str(Path(__file__).relative_to(r)),"tools/curriculum/g12_u07_l02_questions.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=r,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=r,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l02): close sum and difference identities academic ABC [skip ci]"],cwd=r,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=r,check=True)
print("U07L02_CLOSE_PUSH_VERIFIED 37/73")
