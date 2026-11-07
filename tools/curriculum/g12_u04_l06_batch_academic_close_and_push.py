from pathlib import Path
import json,math,datetime,subprocess,re,time
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson006.json")
a_path=base/"u04-l06-source-inventory-and-corrected-boundary.json"
p_path=base/"full-grade-progress.json"
a=json.loads(a_path.read_text(encoding="utf8"));p=json.loads(p_path.read_text(encoding="utf8"))
s=json.loads(cache.read_text(encoding="utf8"))
t=json.loads((root/a["FirstPartyTeacherAssessment"]).read_text(encoding="utf8"))
r=json.loads((root/a["FirstPartyRichV2"]).read_text(encoding="utf8"))
f=json.loads((root/a["FigureSkillEvidence"]).read_text(encoding="utf8"))
c=json.loads((root/a["StageCAssessmentRoutes"]).read_text(encoding="utf8"))
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf8")
graphpath=root/a["FirstPartyGraphRenderer"]
graph=graphpath.read_text(encoding="utf8")
assert p["LastClosedLessonOrdinal"]==25 and p["FirstIncompleteLessonOrdinal"]==26
assert s["Section"]=="4.6" and s["SourceSha256"]==a["SourceSHA256"]
assert [s["Pages"][0]["PdfPage"],s["Pages"][-1]["PdfPage"]]==[405,415]
assert len(a["OriginalWorkedExampleAnchors"])==13 and len(a["OriginalTryItNumbers"])==13
assert a["OriginalFigureNumbers"]==[1,2,3]
assert a["OriginalSourceMathReviewed"]==81 and a["IndependentSourceMathExercises"]==81
assert len(a["SourceExerciseLedger"])==81
assert [x["Number"]for x in a["SourceExerciseLedger"]]==list(range(1,82))
assert all(x["OriginalSourceMathReviewed"] and x["IndependentStudentQuestionVerified"]and x["TeacherMathVerified"]for x in a["SourceExerciseLedger"])
assert len(t["IndependentStudentMathQuestions"])==81
assert [x["OriginalExerciseNumber"]for x in t["IndependentStudentMathQuestions"]]==list(range(1,82))
assert all(not x["OriginalQuestionCopied"]for x in t["IndependentStudentMathQuestions"])
assert len(r["Lessons"][0]["WorkedExamples"])==13
assert len(t["IndependentTryIt"])==13
assert sum(x.startswith("Independent Try It ")for x in r["Lessons"][0]["ExplanationParagraphs"])==13
assert len(t["IndependentCoolDown"]["StudentQuestions"])==3 and not t["IndependentCoolDown"]["OriginalPublisherCoolDownCopied"]
assert len(f["IndependentSourceSkillAlternatives"])==3 and all(not x["OriginalPhotoCopied"] for x in f["IndependentSourceSkillAlternatives"] if "OriginalPhotoCopied" in x)
assert len(c["Routes"])==81 and c["NewMathEngine"]==False
assert not c["AutoGradingClaimed"]
assert "Grade12PrecalculusSection46GraphModels.All" in view
assert len(re.findall(r"\bAdd\([23],",graph))==2 and "MathematicalVisualSpecValidator.Validate(graph)" in graph
assert "new CoordinatePlaneVisualSpec" in graph
assert p["StageD"]=="NOT_RUN" and not p["CIRun"]
# arithmetic and domain spot checks, not a substitute for Stage D
assert 3**(2*5-1)==3**(5+4)
assert math.isclose(math.log(5)/.1,16.0943791243,rel_tol=1e-9)
assert math.isclose(math.log(13)/4,0.641237,rel_tol=1e-5)
assert math.isclose(1000*(.5)**(15/5),125)
assert math.isclose(math.e**(2*math.log(3)),9)
assert math.isclose(2**(3+2),2**5)
# Correct route prose is qualitative; no authenticated rendering execution is claimed.
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l06-abc-close-audit.json"
close={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":6,"Section":"4.6",
"Title":"Exponential and Logarithmic Equations",
"OriginalSource":"OpenStax Precalculus (2017) original local PDF",
"OriginalSourceSha256":s["SourceSha256"],"VerifiedPdfPages":[405,415],
"StageA":{"LearningObjectives":5,"WorkedExamples":13,"TryIt":13,"Figures":3,"Exercises":81,
"SourceMathExercisesReviewed":81},
"StageB":{"IndependentWorkedExamples":13,"IndependentTryIt":13,
"IndependentSourceSkillStudentQuestions":81,"IndependentTeacherMathKeys":81,
"OriginalFigureSkillAlternatives":3,"IndependentCoolDownQuestions":3,
"StudentRichV2":a["FirstPartyRichV2"],"TeacherMath":a["FirstPartyTeacherAssessment"],
"GraphCode":a["FirstPartyGraphRenderer"]},
"StageC":{"ReuseExistingRichV2AndTeacherReview":True,"ExistingTypedSVGRenderer":True,
"ReuseManualAssessmentRoutes":81,"NewMathEngine":False,"AutoGradingVerified":False,
"GraphRuntimeVerified":False,"RouteEvidence":a["StageCAssessmentRoutes"]},
"CCSSSkillCandidateCodes":["CCSS:HSF-LE.A.4","CCSS:HSA-REI.A.1"],
"FullCCSSAlignmentCertified":False,"ExactPublisherGraphParityCertified":False,
"PublisherPhotoCopied":False,
"SourceCredits":"OpenStax/Rice University CC BY 4.0 course attribution remains applicable",
"TargetedChecksActuallyRun":["Original local source sha/page boundaries",
"81 source-skill math cases and teacher keys","13 worked, 13 Try It and original Cool-down",
"Source figures1-3 independent math alternatives and HTML view reference",
"Basic mathematical numerical/domain checks","Stage C route count and git diff"],
"StageD":"NOT_RUN","CI":"NOT_RUN","Build":"NOT_RUN","E2E":"NOT_RUN",
"AuthenticatedRuntime":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialOrdinal":27}
(root/closure_rel).write_text(json.dumps(close,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidencePath":closure_rel,"Tests":"TARGETED_LOCAL_ONLY","CI":"NOT_RUN","StageD":"NOT_RUN",
"ClosedAtUtc":now,"UpdatedUtc":now})
a_path.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][25]
assert row["Ordinal"]==26 and row["SourceSection"]=="4.6" and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"SourceMathQuestionsReviewed":81,"FirstPartyTeacherKeys":81,"SourceFigureSkillsCovered":3,
"ClosureEvidencePath":closure_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][26]["Ordinal"]==27
p.update({"LastClosedLessonOrdinal":26,"FirstIncompleteLessonOrdinal":27,"ReadyForStageD":26,
"ActiveLessonOrdinal":27,"NextLessonOrdinal":27,
"SourceVerifiedGrade12Lessons":26,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_26_OF_73","UpdatedUtc":now})
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
files=[str(x.relative_to(root))for x in [a_path,p_path,root/closure_rel,root/a["FirstPartyTeacherAssessment"],
root/a["FirstPartyRichV2"],root/a["FigureSkillEvidence"],root/a["StageCAssessmentRoutes"],
graphpath,root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml",root/"tools/curriculum/g12_u04_l06_batch_questions_01_43.py",
root/"tools/curriculum/g12_u04_l06_batch_questions_44_81.py",
root/"tools/curriculum/g12_u04_l06_batch_rich_figures_stage_c.py",Path(__file__)]]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l06): close exponential and logarithmic equations academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L06_CLOSED_AND_PUSHED_NEXT_U04L07_26_OF_73")
print("COMMIT",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("STATUS",subprocess.run(["git","status","--porcelain"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
