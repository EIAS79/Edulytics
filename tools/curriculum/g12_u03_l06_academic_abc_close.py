from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l06-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson006.json").read_text(encoding="utf-8"))
q=read("u03-l06-original80-polynomial-zeros-question-contracts.json")
f=read("u03-l06-original-five-figure-polynomial-zero-skill-crosswalk.json")
leaves=read("u03-l06-original80-exercise-cognitive-actions.json")
trykeys=read("u03-l06-seven-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l06.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l06-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==16 and p["FirstIncompleteLessonOrdinal"]==17
assert p["LessonEntries"][16]["Ordinal"]==17 and not p["LessonEntries"][16]["ReadyForStageD"]
assert (p["LessonEntries"][16]["PdfPageStart"],p["LessonEntries"][16]["PdfPageEnd"])==(281,292)
assert src["Section"]=="3.6" and src["SourceSha256"]==q["SourceSHA256"]==f["OriginalSourceSHA256"]
assert len(lesson["WorkedExamples"])==9 and len(lesson["KeyConcepts"])==9 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,8)))])==7
assert len(trykeys["TeacherMathExpected"])==7
assert len(q["Items"])==80 and [x["Number"]for x in q["Items"]]==list(range(1,81))
assert len(assessment["FirstPartyIndependentStudentMathQuestions"])==80 and len(assessment["TeacherOnlyExpectedAlgebraAndRubrics"])==80
assert len(f["Items"])==5 and len(leaves["Items"])==246
assert len({x["SourceID"]for x in leaves["Items"]})==246
assert assessment["OriginalPublisherGraphicBitmapsCopied"] is False
assert assessment["StageDStudentTeacherRuntime"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
assert "cubic meters" in assessment["OriginalSourceMixedUnitsExercise76"]
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":6,
"OriginalSource":"OpenStax Precalculus 2017 Section 3.6 Zeros of Polynomial Functions, original local PDF zero-based pages281–292",
"SourceSHA256":src["SourceSha256"],
"OriginalSevenObjectives":["Evaluate polynomials via Remainder Theorem","Use Factor Theorem","Apply Rational Zero Theorem","Find polynomial zeros","Use Linear Factorization Theorem","Use Descartes' Rule of Signs","Solve applications involving polynomial equations"],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRST_PARTY_SOURCE_POLYNOMIAL_MATH_SKILL_AUTHORING_TEACHER_REVIEW_NOT_PUBLISHER_ORIGINAL_EXACT_GRAPHIC_NUMERICAL_PARITY",
"OriginalSourceWorkedExamples":9,"FirstPartyIndependentWorkedExamples":9,
"OriginalTryItActivities":7,"FirstPartyIndependentTryIts":7,
"OriginalFigureAnchors":5,"FirstPartyFigureMathSkillAlternatives":5,
"OriginalNumberedExerciseAnchors":80,"FirstPartyTeacherManualMathStudentQuestions":80,
"SourceNumberedExerciseNamedCognitiveActionLeaves":246,
"FirstPartyKeyConcepts":9,"FirstPartyVisualTables":2,"IndependentFirstPartyCoolDown":True,
"FirstPartyRichV2":rp.relative_to(root).as_posix(),
"TeacherManualMathRubrics":ap.relative_to(root).as_posix(),
"OriginalSourceMixedUnitsExercise76":"Source book says dimensions in inches but volume in cubic meters; equivalent first-party math uses consistent meters, preserves discrepancy in original source evidence",
"OriginalPublisherImagesCopied":False,
"OriginalExactPublisherVerbatimGraphicNumberParityCertified":False,
"FullPublisherProseAndImagePixelAtomicDenominatorVerified":False,
"CCBY4OriginalSourceCredits":"RETAIN_IN_CREDITS",
"CCSSCandidates":["CCSS:HSA-APR.B.2","CCSS:HSA-APR.B.3","CCSS:HSN-CN.C.9"],
"FormalCompleteCCSSAlignmentVerified":False,
"StudentTeacherAuthenticatedRuntimeAndPersistenceVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":18}
put("u03-l06-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"7_SOURCE_OBJECTIVES_9_WORKED_7_TRYITS_5_FIGURES_80_NUMBERED_EXERCISE_MATH_SKILLS",
"StageB":"9_FIRSTPARTY_WORKED_7_FIRSTPARTY_TRYIT_9_CONCEPTS_2_VISUAL_TABLES_COOLDOWN",
"StageC":"80_FIRSTPARTY_TEACHER_MANUAL_MATH_REVIEWED_QUESTION_CONTRACTS_246_EXERCISE_ACTIONS",
"OriginalSourceMixedUnitDiscrepancy":"ORIGINAL_EXERCISE76_FLAGGED_IN_EVIDENCE",
"PublisherOriginalVerbatimNumericAndImagePixelParityCertified":False,
"WholeSourceAtomicProseImageDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l06-abc-close-audit.json"})
put("u03-l06-abc-checkpoint.json",cp)
row=p["LessonEntries"][16]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentPolynomialZeroMathSourceSkills":True,
"OriginalSourceLearningObjectivesCovered":7,
"OriginalWorkedExampleSkillsCovered":9,"OriginalTryItSkillsCovered":7,
"OriginalFigureMathSkillsCrosswalked":5,
"OriginalNumberedSourceExerciseMathSkillsAuthored":80,
"OriginalNumberedSourceExerciseNamedCognitiveActionsAuthored":246,
"OriginalMixedUnitQuestionDiscrepancyDocumented":True,
"OriginalPublisherImagesCopied":0,
"PublisherOriginalNumericGraphicVerbatimParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"AuthenticatedStudentTeacherGradebookRuntimeVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l06-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=17
p["FirstIncompleteLessonOrdinal"]=18
p["ActiveLessonOrdinal"]=18
p["NextLessonOrdinal"]=18
p["SourceVerifiedGrade12Lessons"]=17
p["ReadyForStageD"]=17
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_17_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][17]["Ordinal"]==18 and p["LessonEntries"][17]["Unit"]==3 and p["LessonEntries"][17]["LessonNumber"]==7
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L06_A_B_C_CLOSED_17_OF_73_NEXT_U03_L07")
