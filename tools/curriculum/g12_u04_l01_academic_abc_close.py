from pathlib import Path
import json,datetime,math,re
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson001.json")
s=json.loads(cache.read_text(encoding="utf8"))
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
r=json.loads((root/a["FirstPartyRichV2"]).read_text(encoding="utf8"))
t=json.loads((root/a["FirstPartyTeacherAssessment"]).read_text(encoding="utf8"))
f=json.loads((base/"u04-l01-six-firstparty-figure-skill-alternatives.json").read_text(encoding="utf8"))
c=json.loads((base/"u04-l01-stage-c-68-exponential-assessment-reuse-routes.json").read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==20 and p["FirstIncompleteLessonOrdinal"]==21
assert s["Section"]=="4.1" and s["SourceSha256"]==a["SourceSHA256"]
assert len(s["Pages"])==15 and [s["Pages"][0]["PdfPage"],s["Pages"][-1]["PdfPage"]]==[343,357]
assert a["PdfPages"]==[343,357] and a["SourceBoundaryCorrectedFromBlueprint"]==[344,358]
assert a["OriginalExerciseMathVerified"]==68 and a["IndependentExerciseTasksAuthored"]==68
assert [v["Number"]for v in a["OriginalNumberedExerciseLedger"]]==list(range(1,69))
assert all(v["SourceMathematicsIndividuallyReviewed"] and v["FirstPartyIndependentExerciseAuthored"] and v["TeacherAnswerVerified"] for v in a["OriginalNumberedExerciseLedger"])
assert [v["SourceSkillReferenceExercise"]for v in t["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,69))
assert len(r["Lessons"][0]["WorkedExamples"])==12
assert sum("Independent Try It " in x for x in r["Lessons"][0]["ExplanationParagraphs"])==12
assert len(t["IndependentTryIts"])==12 and len(t["IndependentCoolDown"]["StudentQuestions"])==3
assert not t["IndependentCoolDown"]["PublisherRestrictedCoolDownCopied"]
assert a["OriginalFigureSkillAlternatives"]==6 and len(f["FirstPartyIndependentFigureSkills"])==6
assert [x["SourceFigureNumber"]for x in f["FirstPartyIndependentFigureSkills"]]==list(range(1,7))
assert len(c["IndependentExerciseRoutes"])==68 and c["NewMathEngine"] is False
assert not c["RuntimeAuthenticatedStudentTeacherVerified"] and not c["StudentSubmissionPersistenceVerified"]
assert not a["ReadyForStageD"] and not p["CIRun"] and p["StageD"]=="NOT_RUN"
code=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection41GraphModels.cs").read_text(encoding="utf8")
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf8")
assert len(re.findall(r"\bAdd\([1-6],",code))==6
assert "Grade12PrecalculusSection41GraphModels.All" in view
assert "MathematicalVisualSpecValidator.Validate(model)" in code
assert abs(float(r["Lessons"][0]["WorkedExamples"][8]["Answer"].split("≈$")[1].split(" ")[0]) - 1846.48)<0.005
assert round(2000/(1+.04/12)**24,2)==1846.48
assert round(1200*(1+.06/4)**12,2)==1434.74
assert math.isclose(math.exp(-.42)*100,65.70468198150567)
assert F(750,6)==125
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
close={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":1,
"SourceSection":"4.1","Title":"Exponential Functions",
"Source":"OpenStax Precalculus 2017 original local PDF",
"SourceSHA256":s["SourceSha256"],"CorrectedPdfPages":[343,357],"BlueprintIncorrectPdfPages":[344,358],
"SourceA":{"Objectives":4,"WorkedExamples":12,"TryIts":12,"Figures":6,"NumberedExercises":68,
"OriginalExercisesIndividuallyReviewed":68,"OriginalFiguresVisuallyInspected":True},
"StageB":{"IndependentWorked":12,"IndependentTryIt":12,"IndependentSourceSkillExercises":68,
"IndependentTypedGraphAlternatives":6,"IndependentCoolDownQuestions":3,
"StudentRichV2":a["FirstPartyRichV2"],"TeacherMathRubrics":a["FirstPartyTeacherAssessment"],
"FigureSkillEvidence":a["FigureSkillEvidencePath"]},
"StageC":{"Decision":"REUSE_EXISTING_RICHV2_MANUAL_TEACHER_REVIEW_AND_TYPED_SVG_RENDERER",
"Routes":68,"RouteEvidence":a["StageCRoutesEvidence"],"NewMathEngine":False,
"AutoGradingPromised":False,"AuthenticatedRuntimeVerified":False},
"CCSSCandidates":["CCSS:HSF-LE.A.1","CCSS:HSF-LE.A.2","CCSS:HSA-SSE.B.3c"],
"CompleteFormalCCSSAlignmentCertified":False,
"PublisherOriginalNumericOrGraphicExactParityCertified":False,
"OriginalPublisherImagePixelsCopied":False,
"SourceAttributionAndLicense":"OpenStax/Rice University, CC BY 4.0 retained in course credits",
"TargetedChecksActuallyPerformed":["original PDF Section 4.1 boundary local pages343-357","68 source exercise skill review and independent teacher answers","12 worked 12 try-it 6 figures","independent cool-down","teacher marking reuse contracts","numeric checking of representative exponential and interest expressions","git diff check"],
"RuntimeChecksNotRun":["CI","Build","xUnit","E2E","Authenticated student/teacher","Publishing"],
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialOrdinal":22}
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l01-abc-close-audit.json"
(base/"u04-l01-abc-close-audit.json").write_text(json.dumps(close,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"StageA":"4_OBJECTIVES_12_WORKED_12_TRYIT_6_FIGURE_68_EXERCISE_SKILLS",
"StageB":"12_INDEPENDENT_WORKED_12_TRYIT_68_FIRSTPARTY_TEACHER_MATH_6_TYPED_SVG_GRAPHS_AND_COOLDOWN",
"StageC":"68_EXISTING_RICHV2_AND_TEACHER_MANUAL_MATH_REVIEW_ROUTES","BlockingReasons":[],
"ClosureEvidencePath":close_rel,"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][20]
assert row["Ordinal"]==21 and row["SourceSection"]=="4.1"
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"OriginalMathSourceExercisesReviewed":68,"IndependentMathTeacherRubrics":68,
"SourceFigureSkillsCovered":6,"OriginalPdfPagesVerified":[343,357],
"ClosureEvidencePath":close_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][21]["Ordinal"]==22 and p["LessonEntries"][21]["Unit"]==4
p.update({"LastClosedLessonOrdinal":21,"FirstIncompleteLessonOrdinal":22,
"ActiveLessonOrdinal":22,"NextLessonOrdinal":22,"ReadyForStageD":21,
"SourceVerifiedGrade12Lessons":21,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_21_OF_73",
"UpdatedUtc":now})
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_TARGETED_CLOSURE_PASS_21_OF_73_NEXT_U04L02_PENDING_PUSH")
