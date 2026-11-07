from pathlib import Path
import json,datetime,math,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
s=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson002.json").read_text(encoding="utf8"))
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
rp=root/a["StudentRichV2"];tp=root/a["TeacherAssessment"]
rich=json.loads(rp.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
fig=json.loads((base/"u04-l02-thirteen-firstparty-graph-figure-skill-alternatives.json").read_text(encoding="utf8"))
routing=json.loads((base/"u04-l02-stage-c-54-teacher-assessment-reuse-routes.json").read_text(encoding="utf8"))
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==21 and p["FirstIncompleteLessonOrdinal"]==22
assert s["Section"]=="4.2" and s["SourceSha256"]==a["SourceSHA256"]
assert s["Pages"][0]["PdfPage"]==358 and s["Pages"][-1]["PdfPage"]==369
assert a["OriginalSourcePdfPages"]==[358,369] and a["IncorrectBlueprintPdfPages"]==[359,370]
assert len(a["OriginalWorkedExamples"])==6 and len(a["OriginalTryItNumbers"])==6
assert len(rich["Lessons"][0]["WorkedExamples"])==6
assert sum(x.startswith("Independent Try It ")for x in rich["Lessons"][0]["ExplanationParagraphs"])==6
assert len(t["IndependentTryIt"])==6
assert len(t["IndependentCoolDown"]["StudentQuestions"])==3
assert not t["IndependentCoolDown"]["OriginalPublisherCoolDownCopied"]
assert a["OriginalExerciseMathVerified"]==54 and a["FirstPartyIndependentExercisesAuthored"]==54
assert [x["SourceSkillReferenceExercise"]for x in t["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,55))
assert [x["Number"]for x in a["OriginalNumberedExerciseLedger"]]==list(range(1,55))
assert all(x["OriginalSourceMathematicsVerified"]and x["IndependentStudentExerciseAuthored"] and x["TeacherExpectedMathVerified"]for x in a["OriginalNumberedExerciseLedger"])
assert a["OriginalFigureSkillAlternatives"]==13 and len(fig["FirstPartyAlternatives"])==13
assert [x["SourceFigureNumber"]for x in fig["FirstPartyAlternatives"]]==list(range(1,14))
assert all(not x["PublisherGraphicCopied"]for x in fig["FirstPartyAlternatives"])
assert len(routing["FirstPartyIndependentAssessmentRoutes"])==54
assert not routing["NewEngineIntroduced"] and not routing["StudentRuntimeVerified"]
assert not p["CIRun"] and p["StageD"]=="NOT_RUN" and not a["ReadyForStageD"]
code=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection42GraphModels.cs").read_text(encoding="utf8")
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-3]),",code))==13
assert "Grade12PrecalculusSection42GraphModels.All" in view
assert "MathematicalVisualSpecValidator.Validate(graph)" in code
assert round(math.log(10,2),3)==3.322 and round(math.log(7,3),3)==1.771
assert abs(2**(-2)+3-3.25)<1e-12
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":2,"Section":"4.2",
"Title":"Graphs of Exponential Functions","OriginalSource":"OpenStax Precalculus 2017 authorized local PDF",
"SourceSHA256":s["SourceSha256"],"CorrectedPdfPages":[358,369],"IncorrectBlueprintPages":[359,370],
"StageA":{"LearningObjectives":2,"OriginalWorkedExamples":6,"OriginalTryIts":6,"OriginalFigures":13,"OriginalNumberedExercises":54,
"SourceExerciseMathematicsReviewed":54,"OriginalGraphicMathSkillsReviewedByLocalSource":13},
"StageB":{"IndependentWorkedExamples":6,"IndependentTryIts":6,"IndependentSourceSkillExercises":54,"IndependentFigureSkillGraphModels":13,
"IndependentCoolDownQuestions":3,"StudentRichV2":a["StudentRichV2"],"TeacherManualRubrics":a["TeacherAssessment"]},
"StageC":{"ExistingRichV2Reused":True,"TypedSVGRendererReused":True,"ManualTeacherAssessmentRoutes":54,
"RouteEvidence":a["StageCAssessmentRoutes"],"NewMathEngine":False,
"AutoGradeClaimed":False,"AuthenticatedRuntimeVerified":False},
"CCSSCandidates":["CCSS:HSF-IF.C.7e","CCSS:HSF-BF.B.3","CCSS:HSF-LE.A.1"],
"FormalComprehensiveCCSSCertification":False,
"ExactPublisherGraphicPixelNumericalParityCertified":False,
"PublisherOriginalImageCopied":False,"CopyrightCredits":"OpenStax/Rice University CC BY 4.0 retained in course credits",
"ChecksActuallyRun":["local-source-boundary and SHA cache","54 unique source-skill question contracts","6 worked 6 tryit 13 original figure skill alternatives",
"54 manual teacher route contracts","three-part cool-down answer review",
"selected exponential arithmetic and graph feature checks","git diff check"],
"TestsNotRun":["CI","Full build","xUnit","E2E","Authenticated runtime","Production publish"],
"Tests":"TARGETED_LOCAL_ONLY","CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialOrdinal":23}
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l02-abc-close-audit.json"
(base/"u04-l02-abc-close-audit.json").write_text(json.dumps(closure,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"StageA":"2_OBJECTIVES_6_WORKED_6_TRYIT_13_FIGURE_54_EXERCISES",
"StageB":"6_INDEPENDENT_WORKED_6_TRYIT_54_TEACHER_MATH_13_FIRSTPARTY_SVG_MODELS_COOLDOWN",
"StageC":"54_TEACHER_MANUAL_ASSESSMENT_ROUTES_REUSE_EXISTING_COMPONENTS",
"BlockingReasons":[],"ClosureEvidencePath":close_rel,"Tests":"TARGETED_LOCAL_ONLY",
"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][21]
assert row["Ordinal"]==22 and row["SourceSection"]=="4.2"
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"BlockingReasons":[],"OriginalSourceMathReviewed":54,"FirstPartyTeacherManualQuestionCount":54,
"OriginalFigureMathSkillsCovered":13,"CorrectedPdfPages":[358,369],
"ClosureEvidencePath":close_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][22]["Ordinal"]==23 and p["LessonEntries"][22]["Unit"]==4
p.update({"LastClosedLessonOrdinal":22,"FirstIncompleteLessonOrdinal":23,
"ActiveLessonOrdinal":23,"NextLessonOrdinal":23,"ReadyForStageD":22,
"SourceVerifiedGrade12Lessons":22,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_22_OF_73",
"UpdatedUtc":now})
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_TARGETED_ACADEMIC_CLOSE_PASS_22_OF_73_NEXT_U04L03_PENDING_GITHUB_PUSH")
