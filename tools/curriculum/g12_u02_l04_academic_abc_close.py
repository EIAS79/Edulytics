from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson004.json").read_text(encoding="utf-8"))
p=read("full-grade-progress.json");cp=read("u02-l04-abc-checkpoint.json")
q=read("u02-l04-original42-source-skill-question-contracts.json")
fig=read("u02-l04-original10-figure-source-skill-crosswalk.json")
actions=read("u02-l04-original-numbered-source-exercise-cognitive-action-leaves.json")
trykeys=read("u02-l04-firstparty-two-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l04.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l04-firstparty-teacher-led.json"
teacher=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==10 and p["FirstIncompleteLessonOrdinal"]==11
assert p["LessonEntries"][10]["Ordinal"]==11 and not p["LessonEntries"][10]["ReadyForStageD"]
assert (p["LessonEntries"][10]["PdfPageStart"],p["LessonEntries"][10]["PdfPageEnd"])==(190,201)
assert source["Section"]=="2.4" and source["SourceSha256"]==q["SourceSha256"]==fig["OriginalSourceSha256"]
assert len(lesson["WorkedExamples"])==6 and len(lesson["KeyConcepts"])==9 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {n} —"for n in range(1,3)))])==2
assert len(trykeys["FirstPartyTeacherMathKeys"])==2
assert len(q["Items"])==42 and [x["Number"]for x in q["Items"]]==list(range(1,43))
assert len(teacher["IndependentFirstPartyStudentQuestions"])==42 and len(teacher["TeacherExpectedAnswers"])==42
assert len(fig["Items"])==10 and len(actions["Items"])==91
assert len({x["SourceID"]for x in actions["Items"]})==91
assert teacher["PublisherSourceGraphicImagesReproduced"] is False
assert teacher["StageDStudentTeacherRuntime"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
# Source exercise 7 combines positive slope with negative stated Pearson r.
# The contradiction is documented in the teacher key, not overwritten.
assert "internally inconsistent" in teacher["TeacherExpectedAnswers"][6]["ExpectedMath"]
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":4,
"AuthorizedOriginalLocalPDF":"OpenStax Precalculus (2017), Section 2.4, zero-based pages 190–201",
"OriginalSourceSHA256":source["SourceSha256"],
"OriginalFourPedagogicObjectives":[
"Draw and interpret scatter plots","Find line of best fit",
"Distinguish linear and nonlinear relations","Use linear models for predictions"
],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRST_PARTY_SOURCE_MATH_LEARNING_SKILL_AUTHORING_NOT_ORIGINAL_EXACT_NUMERIC_ITEM_AND_GRAPHIC_PARITY",
"OriginalSourceWorkedExamples":6,"FirstPartyWorkedExampleSkillEquivalents":6,
"SourceTryItActivities":2,"FirstPartyIndependentTryItAlternatives":2,
"SourceFigureAnchors":10,"FirstPartySourceFigureMathSkillAlternatives":10,
"SourceNumberedExercises":42,"FirstPartyTeacherReviewedNumberedQuestions":42,
"OriginalSourceNamedExerciseCognitiveActions":91,
"FirstPartyConcepts":9,"FirstPartyVisualTables":2,"IndependentCoolDownAuthored":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"FirstPartyTeacherManualAnswerRubricPath":ap.relative_to(root).as_posix(),
"RegressionMathTool":"Python first-party deterministic least-squares and Pearson calculation, no runtime application tests or CI",
"OriginalSourceExercise7DataInconsistency":"Given positive regression slope +6.301 and negative Pearson r −0.970; cannot be simultaneously valid for same paired dataset. Preserved as source discrepancy in teacher assessment note.",
"OriginalPublisherGraphicsAndThirdPartyImagesCopied":False,
"PublisherSourceExactVerbatimAndOriginalGraphPixelEqualityCertified":False,
"EntireOriginalBookParagraphAndPixelAtomicDenominatorCertified":False,
"CCBY4SourceCreditsRequired":True,
"CCSSCandidate":["CCSS:HSS-ID.B.6","CCSS:HSS-ID.C.7","CCSS:HSS-ID.C.8"],
"FullFormalCCSSAlignmentVerified":False,
"ProductionAuthenticatedStudentTeacherRuntimeAndPersistenceVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":12}
put("u02-l04-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"4_SOURCE_OBJECTIVES_6_WORKED_EXAMPLE_SKILLS_2_TRYIT_SKILLS_10_FIGURES_42_NUMBERED_EXERCISES",
"StageB":"6_FIRSTPARTY_WORKED_2_FIRSTPARTY_TRYIT_9_CONCEPTS_2_TABLE_VISUALS_AND_COOLDOWN",
"StageC":"42_FIRSTPARTY_MANUAL_TEACHER_REVIEWED_REGRESSION_AND_CORRELATION_QUESTIONS_91_NAMED_ACTIONS",
"SourceRegressionSlopeCorrelationContradictionEx7":"DOCUMENTED_AS_ORIGINAL_SOURCE_CONFLICT",
"OriginalPublisherVerbatimTextAndGraphicPixelParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l04-abc-close-audit.json"})
put("u02-l04-abc-checkpoint.json",cp)
row=p["LessonEntries"][10]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentSourceSkillMathAuthoring":True,
"OriginalSourceLearningObjectivesCovered":4,"OriginalSourceWorkedExampleSkillsCovered":6,
"OriginalSourceTryItSkillsCovered":2,"SourceFigureCognitiveSkillsCrosswalked":10,
"SourceNumberedExerciseSkillsAuthored":42,"SourceExerciseNamedCognitiveActionLeavesAuthored":91,
"SourceExercise7RegressionSignInconsistencyDocumented":True,
"OriginalPublisherFiguresCopied":0,"ExactOriginalPublisherGraphicTextMathItemParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveAuthenticatedStudentTeacherGradebookVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l04-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=11
p["FirstIncompleteLessonOrdinal"]=12
p["ActiveLessonOrdinal"]=12
p["NextLessonOrdinal"]=12
p["SourceVerifiedGrade12Lessons"]=11
p["ReadyForStageD"]=11
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_11_OF_73_OWNER_ACCEPTED_FIRSTPARTY_SOURCE_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][11]["Ordinal"]==12 and p["LessonEntries"][11]["Unit"]==3
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U02_L04_A_B_C_CLOSED_11_OF_73_NEXT_U03_L01")
