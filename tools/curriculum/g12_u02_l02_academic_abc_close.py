from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,x):(base/n).write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json")
cp=read("u02-l02-abc-checkpoint.json")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson002.json").read_text(encoding="utf-8"))
q=read("u02-l02-original76-firstparty-question-contracts.json")
fig=read("u02-l02-original24-figure-source-skill-crosswalk.json")
leaves=read("u02-l02-original-numbered-exercise-cognitive-action-leaves.json")
trykeys=read("u02-l02-independent-seven-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l02.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l02-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert source["Section"]=="2.2" and (source["Pages"][0]["PdfPage"],source["Pages"][-1]["PdfPage"])==(158,176)
assert p["LastClosedLessonOrdinal"]==8 and p["FirstIncompleteLessonOrdinal"]==9
assert p["LessonEntries"][8]["Ordinal"]==9 and p["LessonEntries"][8]["ReadyForStageD"] is False
assert (p["LessonEntries"][8]["PdfPageStart"],p["LessonEntries"][8]["PdfPageEnd"])==(158,176)
assert source["SourceSha256"]==fig["OriginalSourceSha256"]==q["OriginalSourceSha256"]
assert len(lesson["WorkedExamples"])==13 and len(lesson["KeyConcepts"])==9 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,8)))])==7
assert len(trykeys["TeacherOnlyAnswers"])==7
assert len(q["Items"])==76 and [x["Number"] for x in q["Items"]]==list(range(1,77))
assert len(assessment["StudentQuestions"])==76 and len(assessment["TeacherOnlyExpectedMathematics"])==76
assert len(fig["Items"])==24 and len(leaves["Items"])==162
assert len({x["SourceID"]for x in leaves["Items"]})==162
assert assessment["PublisherOriginalGraphicBitmapCopied"] is False
assert assessment["RuntimeStudentQuestionDeliveryAndTeacherGrading"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalPublisherRestrictedTeacherOnlyCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":2,
"OriginalLocalSource":"OpenStax Precalculus (2017), Section 2.2, zero-based PDF pages 158–176",
"OriginalSourceSha256":source["SourceSha256"],
"SourceFivePedagogicObjectives":[
"Graph linear functions","Write the equation of a linear function from a line graph",
"Identify parallel and perpendicular lines from their equations",
"Write parallel or perpendicular line equations through prescribed points",
"Solve systems of linear equations"
],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRSTPARTY_MATH_LEARNING_SKILL_EQUIVALENCE_WITH_TEACHER_REVIEW_AUTHORING",
"SourceWorkedExamples":13,"FirstPartyWorkedExamples":13,
"SourceTryItActivities":7,"FirstPartyTryItAlternatives":7,
"SourceFigureAnchors":24,"FirstPartyFigureSkillAlternatives":24,
"SourceNumberedExercises":76,"FirstPartyNumberedTeacherReviewQuestions":76,
"SourceNumberedExerciseCognitiveActions":162,
"FirstPartyKeyConcepts":9,"FirstPartyStructuredVisualTables":2,
"FirstPartyCoolDown":True,
"StudentRichV2":rp.relative_to(root).as_posix(),
"TeacherAssessmentResource":ap.relative_to(root).as_posix(),
"PublisherOriginalGraphImagesCopied":False,
"PublisherSourceExactVerbatimAndNumericGraphicParityCertified":False,
"FullPublisherGraphPixelAndParagraphDenominatorCertified":False,
"CCBY4SourceAttribution":"RETAIN_IN_CREDITS",
"CCSSCandidates":[
"CCSS:HSA-REI.C.6","CCSS:HSA-REI.D.10","CCSS:HSA-CED.A.2"],
"FormalCompleteCCSSMappingVerified":False,
"LiveStudentTeacherAuthenticatedGradebookVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":10
}
put("u02-l02-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"5_SOURCE_OBJECTIVES_13_EXAMPLE_SKILLS_7_TRYITS_24_FIGURE_SKILLS_76_SOURCE_NUMBERED_EXERCISES",
"StageB":"13_FIRSTPARTY_WORKED_7_INDEPENDENT_TRYIT_9_KEY_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN",
"StageC":"76_INDEPENDENT_FIRSTPARTY_TEACHER_REVIEW_QUESTIONS_162_NAMED_COGNITIVE_ACTIONS",
"FullSourcePublisherTextAndGraphicPixelIdentityCertified":False,
"SourceCompleteAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l02-abc-close-audit.json"})
put("u02-l02-abc-checkpoint.json",cp)
row=p["LessonEntries"][8]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentFirstPartyMathSourceSkillAuthoring":True,
"SourceOriginalLearningObjectivesCovered":5,
"SourceWorkedExampleSkillsCovered":13,"SourceTryItSkillsCovered":7,
"SourceFigureSkillsCrosswalked":24,"SourceNumberedExerciseSkillsAuthored":76,
"SourceNumberedExerciseActionLeavesAuthored":162,
"OriginalPublisherFiguresCopied":0,
"ExactPublisherPixelAndVerbatimParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveRuntimeAndTeacherGradingVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l02-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=9
p["FirstIncompleteLessonOrdinal"]=10
p["ActiveLessonOrdinal"]=10
p["NextLessonOrdinal"]=10
p["SourceVerifiedGrade12Lessons"]=9
p["ReadyForStageD"]=9
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_9_OF_73_OWNER_ACCEPTED_FIRSTPARTY_SOURCE_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][9]["Ordinal"]==10 and p["LessonEntries"][9]["Unit"]==2 and p["LessonEntries"][9]["LessonNumber"]==3
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U02_L02_ABC_CLOSED_9_OF_73_NEXT_U02_L03")
