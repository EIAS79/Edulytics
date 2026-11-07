from pathlib import Path
import json,datetime,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson009.json").read_text(encoding="utf8"))
ap=base/"u03-l09-source-inventory-and-gap-audit.json"
a=json.loads(ap.read_text(encoding="utf8"))
tp=root/a["FirstPartyTeacherAssessment"]
rp=root/a["FirstPartyRichV2"]
t=json.loads(tp.read_text(encoding="utf8"))
r=json.loads(rp.read_text(encoding="utf8"))
fp=base/"u03-l09-four-original-figure-firstparty-skill-alternatives.json"
sp=base/"u03-l09-stage-c-reuse-60-teacher-assessment-routes.json"
fig=json.loads(fp.read_text(encoding="utf8"));route=json.loads(sp.read_text(encoding="utf8"))
p_path=base/"full-grade-progress.json";p=json.loads(p_path.read_text(encoding="utf8"))
assert src["Section"]=="3.9" and src["SourceSha256"]==a["SourceSha256"]
assert (src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"])==(325,331)
assert p["LastClosedLessonOrdinal"]==19 and p["FirstIncompleteLessonOrdinal"]==20
assert len(a["OriginalNumberedExerciseLedger"])==60
assert a["OriginalExerciseMathVerified"]==60 and a["FirstPartyIndependentExerciseTasksAuthored"]==60
assert all(x["SourceMathIndividuallyReviewed"] and x["IndependentStudentQuestionVerified"] and x["IndependentTeacherMathVerified"] for x in a["OriginalNumberedExerciseLedger"])
assert [x["SourceSkillReferenceExercise"] for x in t["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,61))
assert len(r["Lessons"][0]["WorkedExamples"])==4
assert len(t["IndependentTryIt"])==3
assert len(t["IndependentCoolDown"]["StudentQuestions"])==3 and not t["IndependentCoolDown"]["CopiedRestrictedPublisherCoolDown"]
assert len(fig["FirstPartyFigureAlternatives"])==4 and not fig["ExactPublisherImageParityClaimed"]
assert len(route["Routes"])==60 and route["NoNewMathEngine"] and route["StageD"]=="NOT_RUN"
assert p["StageD"]=="NOT_RUN" and not p["CIRun"]
assert all(not x["OriginalTextCopied"] for x in a["OriginalNumberedExerciseLedger"])
assert F(25,8)*6**3==675 and F(200,6**3)==F(25,27)
assert math.isclose((142/93)**1.5,1.886,abs_tol=.003)
assert F(800)*F(15**2,10**2)==F(1800)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
close={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":9,"Section":"3.9","Title":"Modeling Using Variation",
"Source":"OpenStax Precalculus (2017) authorized local book","OriginalSourceSha256":src["SourceSha256"],"OriginalPdfPages":[325,331],
"StageA":{"LearningObjectives":3,"WorkedExamples":4,"TryIt":3,"Figures":4,"Exercises":60,
"OriginalExerciseMathematicsIndividuallyReviewed":60},
"StageB":{"IndependentWorkedExamples":4,"IndependentTryIt":3,"IndependentSourceSkillExercises":60,
"IndependentFigureAlternatives":4,"OriginalIndependentCoolDownQuestions":3,
"FirstPartyStudentRichV2":a["FirstPartyRichV2"],"TeacherAssessment":a["FirstPartyTeacherAssessment"]},
"StageC":{"ExistingRichV2AndTeacherReviewReused":True,"IndependentTeacherRoutes":60,
"RoutingEvidence":sp.relative_to(root).as_posix(),"NewMathEngine":False,"AutoGradingClaimed":False},
"CCSSSkillCandidateCodes":["CCSS:HSF-BF.A.1a","CCSS:HSF-IF.B.4","CCSS:HSA-CED.A.2"],
"CCSSFormalCompleteAlignmentVerified":False,
"PublisherGraphicAndExactQuestionParityCertified":False,"OriginalImagesCopied":False,
"SourceAttribution":"OpenStax/Rice University, CC BY 4.0 in relevant course credits",
"AcademicChecksActuallyRun":["source-SHA256-cache-check","local-page-boundary-check","4-worked-3-tryit-4-figure-60-numbered-math-coverage",
"60-independent-questions-and-rubrics-check","four-figure-math-alternatives-and-stage-C-routes-check",
"fractions-and-application-example-checks","git-diff-check"],
"RuntimeChecksNotRun":["CI","Build","xUnit","E2E","Authenticated student/teacher workflow","Publishing"],
"Tests":"TARGETED_LOCAL_ONLY","CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":21}
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l09-abc-close-audit.json"
(base/"u03-l09-abc-close-audit.json").write_text(json.dumps(close,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"StageA":"3_OBJECTIVES_4_WORKED_3_TRYIT_4_FIGURE_60_EXERCISE_SOURCE_REVIEW",
"StageB":"4_WORKED_3_TRYIT_4_FIRSTPARTY_FIGURE_SKILL_60_TEACHER_MATH_3_COOLDOWN",
"StageC":"60_TEACHER_REVIEW_ROUTES_REUSE_EXISTING_COMPONENTS",
"BlockingReasons":[],"ClosureEvidencePath":close_rel,"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][19]
assert row["Ordinal"]==20 and row["SourceSection"]=="3.9" and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"SourceMathQuestionsVerified":60,"FirstPartyIndependentTeacherRubricCount":60,"OriginalFigureSkillAlternativesVerified":4,
"ClosureEvidencePath":close_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][20]["Ordinal"]==21
p.update({"LastClosedLessonOrdinal":20,"FirstIncompleteLessonOrdinal":21,
"ActiveLessonOrdinal":21,"NextLessonOrdinal":21,"ReadyForStageD":20,
"SourceVerifiedGrade12Lessons":20,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_20_OF_73",
"UpdatedUtc":now})
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03L09_TARGETED_A_B_C_CLOSED_LOCALLY_20_OF_73_NEXT_ORDINAL_21")
