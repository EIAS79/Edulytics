from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson006.json").read_text(encoding="utf-8"))
p=read("full-grade-progress.json")
cp=read("u01-l06-abc-checkpoint.json")
q=read("u01-l06-64-original-numbered-exercise-skill-contracts.json")
fig=read("u01-l06-twelve-source-figure-skill-crosswalk.json")
leaves=read("u01-l06-original-exercise-action-leaves.json")
teacher=read("u01-l06-firstparty-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l06.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l06-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==5 and p["FirstIncompleteLessonOrdinal"]==6
assert p["LessonEntries"][5]["Ordinal"]==6 and p["LessonEntries"][5]["ReadyForStageD"] is False
assert source["Section"]=="1.6" and len(source["Pages"])==11
assert source["SourceSha256"]==q["SourceSha256"]==fig["SourceSHA256"]
assert len(lesson["WorkedExamples"])==7 and len(lesson["KeyConcepts"])==7 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {n} —"for n in range(1,8)))])==7
assert len(teacher["TeacherAnswers"])==7
assert len(q["Items"])==64 and [x["Number"]for x in q["Items"]]==list(range(1,65))
assert len(assessment["StudentQuestions"])==64 and len(assessment["TeacherExpectedMath"])==64
assert len(fig["Items"])==12 and len(leaves["Items"])==157
assert len({x["SourceId"]for x in leaves["Items"]})==157
assert assessment["OriginalPublisherVisualsCopied"] is False
assert assessment["StudentTeacherRuntimeAndGradebook"]=="NOT_RUN"
assert teacher["IndependentCoolDown"]["OriginalTeacherOnlyPublisherCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":6,
"AuthorizedLocalSource":"OpenStax Precalculus 2017 Section 1.6, PDF pages 105–115",
"SourceSHA256":source["SourceSha256"],
"SourceThreeLearningObjectives":["Graph an absolute value function","Solve an absolute value equation","Solve an absolute value inequality"],
"AcademicOwnerAcceptedScope":"INDEPENDENT_FIRST_PARTY_MATHEMATICALLY_EQUIVALENT_LEARNING_SKILL_AND_TEACHER_REVIEW_AUTHORING",
"OriginalWorkedExampleSkillCount":7,
"FirstPartyWorkedExamples":7,
"OriginalTryItSkillCount":7,"FirstPartyTryItAlternatives":7,
"OriginalSourceFigureAnchors":12,"FirstPartyFigureConceptAlternatives":12,
"OriginalSourceNumberedExerciseAnchors":64,
"FirstPartyManualAssessmentQuestionCount":64,
"OriginalNamedExerciseCognitiveActions":157,
"FirstPartyKeyConcepts":7,"FirstPartyVisualTables":2,
"IndependentCoolDownAuthored":True,
"FirstPartyRichV2":rp.relative_to(root).as_posix(),
"FirstPartyTeacherRubric":ap.relative_to(root).as_posix(),
"OriginalPublisherAndThirdPartyImageCopied":False,
"ExactPublisherOriginalImageAndProseSemanticParityCertified":False,
"CompletePublisherPixelAndParagraphSourceDenominatorCertified":False,
"CCSSCandidate":["CCSS:HSF-IF.C.7","CCSS:HSA-REI.B.3"],
"CandidateMappingReview":"GRADE_WIDE_ALIGNMENT_STILL_TO_BE_VERIFIED",
"RequiredCCBY4SourceCredits":"RETAIN",
"StudentTeacherAuthenticatedRuntimeVerified":False,
"TeacherPersistentSubmissionAndGradingVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextLessonOrdinal":7}
put("u01-l06-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"COMPLETE_3_SOURCE_OBJECTIVES_7_EXAMPLE_SKILLS_7_TRYIT_SKILLS_12_FIGURE_SKILLS_64_NUMBERED_EXERCISES",
"StageB":"COMPLETE_7_FIRSTPARTY_WORKED_EXAMPLES_7_TRYITS_7_KEY_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN",
"StageC":"COMPLETE_64_FIRSTPARTY_TEACHER_SCORABLE_QUESTIONS_157_NAMED_EXERCISE_ACTIONS",
"OriginalSourceThirdPartyMediaCopied":0,
"OriginalSourceFullPublisherArtAndPixelParityVerified":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l06-abc-close-audit.json"})
put("u01-l06-abc-checkpoint.json",cp)
e=p["LessonEntries"][5]
e.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerApprovedFirstPartySkillAdaptation":True,
"SourceOriginalLearningObjectivesCovered":3,
"OriginalWorkedExampleSkillsCovered":7,
"OriginalTryItSkillsCovered":7,
"OriginalFigureSkillsCrosswalked":12,
"OriginalNumberedSourceExerciseSkillsAuthored":64,
"OriginalNumberedExerciseActionLeavesAuthored":157,
"OriginalPublisherThirdPartyGraphicsCopied":0,
"SourceExactPublisherSemanticAndPixelParityClaim":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"StageDRuntimeAndGradingVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l06-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=6
p["FirstIncompleteLessonOrdinal"]=7
p["ActiveLessonOrdinal"]=7
p["NextLessonOrdinal"]=7
p["SourceVerifiedGrade12Lessons"]=6
p["ReadyForStageD"]=6
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_6_OF_73_OWNER_APPROVED_FIRSTPARTY_SKILL_AUTHORING_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][6]["Ordinal"]==7 and p["LessonEntries"][6]["ReadyForStageD"] is False
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U01_L06_ABC_CLOSED_NEXT_L07_6_OF_73_ACADEMIC_SKILL_READY_FOR_STAGE_D")
