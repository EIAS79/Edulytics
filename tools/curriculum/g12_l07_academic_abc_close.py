from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson007.json").read_text(encoding="utf-8"))
p=read("full-grade-progress.json")
cp=read("u01-l07-abc-checkpoint.json")
q=read("u01-l07-47-original-numbered-skill-assessment-contracts.json")
fig=read("u01-l07-twelve-source-figure-skill-crosswalk.json")
leaves=read("u01-l07-original-numbered-exercise-cognitive-action-leaves.json")
teacher=read("u01-l07-firstparty-nine-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l07.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l07-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==6 and p["FirstIncompleteLessonOrdinal"]==7
assert p["LessonEntries"][6]["Ordinal"]==7 and p["LessonEntries"][6]["ReadyForStageD"] is False
assert source["Section"]=="1.7" and source["SourceSha256"]==q["SourceSHA256"]==fig["SourceSha256"]
assert len(lesson["WorkedExamples"])==10 and len(lesson["KeyConcepts"])==7 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,10)))])==9
assert len(teacher["TeacherOnlyExpectedMathematics"])==9
assert len(q["Items"])==47 and [x["Number"]for x in q["Items"]]==list(range(1,48))
assert len(assessment["StudentQuestions"])==47 and len(assessment["TeacherOnlyExpectedMathematics"])==47
assert len(fig["Items"])==12 and len(leaves["Items"])==83
assert len({x["SourceId"]for x in leaves["Items"]})==83
assert assessment["OriginalPublisherFigureGraphicsReused"] is False
assert assessment["StudentTeacherRuntimeAndAutomatedGrading"]=="NOT_RUN"
assert teacher["IndependentCoolDown"]["OriginalPublisherCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
close={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":7,
"AuthorizedLocalSource":"OpenStax Precalculus 2017, Section 1.7, original PDF pages 116–128",
"SourceSHA256":source["SourceSha256"],
"OriginalFourLearningObjectives":[
 "Verify inverse functions",
 "Determine inverse domains and ranges and restrict original domain to make f one-to-one",
 "Find and evaluate inverse functions",
 "Graph an inverse on the same coordinate plane using reflection across y=x"
],
"AcademicOwnerSkillAlternativeAcceptance":"FIRST_PARTY_EQUIVALENT_MATHEMATICS_AND_TEACHER_REVIEW_AUTHORING_ONLY_NOT_EXACT_PUBLISHER_GRAPH_PIXELS_OR_ORIGINAL_TEXT",
"SourceOriginalWorkedExamples":10,"FirstPartyPedagogicWorkedExamples":10,
"SourceOriginalTryItActivities":9,"FirstPartyIndependentTryItActivities":9,
"SourceOriginalFigureAnchors":12,"FirstPartyFigureLearningSkillAlternatives":12,
"SourceOriginalNumberedExercises":47,
"FirstPartyIndependentTeacherReviewNumberedQuestions":47,
"OriginalNamedExerciseCognitiveActionLeaves":83,
"FirstPartyKeyConcepts":7,"FirstPartyStructuredTables":2,
"FirstPartyIndependentCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"FirstPartyTeacherAssessmentPath":ap.relative_to(root).as_posix(),
"OriginalSourcePublisherGraphicsCopied":False,
"ExactPublisherSourceVerbatimAndPixelParityCertified":False,
"FullPublisherParagraphAndGraphicAtomicDenominatorCertified":False,
"StudentTeacherAuthenticatedRuntimeAndGradebookVerified":False,
"CCBY4Attribution":"RETAIN_IN_SOURCE_CREDITS",
"CCSSCandidate":[
 {"Code":"CCSS:HSF-BF.B.4","Skill":"Find inverse functions and verify by composition"},
 {"Code":"CCSS:HSF-IF.B.4","Skill":"Analyze domain/range and function graph relations"}],
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":8}
put("u01-l07-abc-close-audit.json",close)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"COMPLETE_4_ORIGINAL_OBJECTIVES_10_SOURCE_WORKED_SKILLS_9_TRYITS_12_FIGURES_47_NUMBERED_EXERCISES",
"StageB":"COMPLETE_10_INDEPENDENT_WORKED_EXAMPLES_9_TRYIT_TASKS_7_CONCEPTS_2_TABLE_VISUALS_AND_COOLDOWN",
"StageC":"COMPLETE_47_SOURCE_SKILL_INDEPENDENT_TEACHER_REVIEW_QUESTIONS_AND_83_NAMED_ACTIONS",
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRSTPARTY_SOURCE_SKILL_AUTHORING",
"FullPublisherVerbatimDiagramPixelParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l07-abc-close-audit.json"})
put("u01-l07-abc-checkpoint.json",cp)
e=p["LessonEntries"][6]
e.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerApprovedFirstPartySkillAdaptation":True,
"OriginalFourLearningObjectivesCovered":4,
"OriginalSourceWorkedExampleSkillsCovered":10,
"OriginalSourceTryItSkillsCovered":9,
"OriginalSourceFigureSkillsMapped":12,
"OriginalSourceNumberedExerciseSkillsAuthored":47,
"OriginalSourceNumberedExerciseActionLeavesAuthored":83,
"OriginalSourcePublisherAndThirdPartyImagesCopied":0,
"ExactPublisherSemanticVerbatimAndPixelParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,
"VisualsVerified":False,
"PracticeVerified":False,
"StageDRuntimeAndGradebookVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l07-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=7
p["FirstIncompleteLessonOrdinal"]=8
p["ActiveLessonOrdinal"]=8
p["NextLessonOrdinal"]=8
p["SourceVerifiedGrade12Lessons"]=7
p["ReadyForStageD"]=7
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_7_OF_73_OWNER_APPROVED_FIRSTPARTY_SKILL_AUTHORING_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][7]["Ordinal"]==8 and p["LessonEntries"][7]["ReadyForStageD"] is False
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U01_L07_ABC_ACADEMIC_AUTHORING_CLOSED_7_OF_73_NEXT_U02_L01")
