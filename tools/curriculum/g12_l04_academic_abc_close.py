from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json")
cp=read("u01-l04-abc-checkpoint.json")
fig=read("u01-l04-eight-source-figure-skill-crosswalk.json")
tasks=read("u01-l04-97-numbered-source-skill-question-contracts.json")
atoms=read("u01-l04-named-original-source-exercise-actions.json")
tries=read("u01-l04-independent-tryit-teacher-key.json")
cool=read("u01-l04-firstparty-cooldown.json")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson004.json").read_text(encoding="utf-8"))
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l04.rich-lesson-v2.json"
lesson=json.loads(rich.read_text(encoding="utf-8"))["Lessons"][0]
assessment=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l04-firstparty-teacher-led.json"
a=json.loads(assessment.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==3 and p["FirstIncompleteLessonOrdinal"]==4
assert p["LessonEntries"][3]["Ordinal"]==4 and p["LessonEntries"][3]["ReadyForStageD"] is False
assert source["Section"]=="1.4" and source["SourceSha256"]==tasks["SourceSha256"]
assert len(source["Pages"])==13
assert len(lesson["WorkedExamples"])==9 and len(lesson["KeyConcepts"])==6 and len(lesson["Visuals"])==2
assert len(tries["TeacherOnlyFirstPartyAnswers"])==7 and len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple("Independent Try It "+str(n)+" —" for n in range(1,8)))])==7
assert "27" in tries["TeacherOnlyFirstPartyAnswers"][4]["TeacherExpectedMathematics"]
assert len(tasks["Items"])==97 and len(a["StudentQuestions"])==97 and len(a["TeacherExpectedMathematics"])==97
assert [x["Number"] for x in a["StudentQuestions"]]==list(range(1,98))
assert len(atoms["Items"])==221 and len(set(x["SourceId"]for x in atoms["Items"]))==221
assert len(fig["Items"])==8
assert a["OriginalNamedNumberedExercises"]==97
assert a["PublisherOriginalGraphicsEmbedded"] is False
assert a["TeacherAuthenticatedGradebookRuntime"]=="NOT_RUN"
assert cool["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":4,
"Source":"Local authorized OpenStax Precalculus 2017 Section 1.4 pp.67–79",
"OriginalPdfSha256":source["SourceSha256"],
"OriginalFiveSourceObjectives":[
 "Combine functions using algebraic operations",
 "Create a new function by function composition",
 "Evaluate composite functions",
 "Find the domain of a composite function",
 "Decompose a composite function into component functions"],
"AcademicOwnerAcceptance":"FIRST_PARTY_MATHEMATICALLY_EQUIVALENT_SOURCE_SKILL_CONTENT_AND_MANUAL_TEACHER_REVIEW_NOT_VERBATIM_PUBLISHER_OR_PIXEL_MATCH",
"FirstPartyIndependentWorkedExamples":9,
"FirstPartyIndependentTryIts":7,
"FirstPartyLearningConcepts":6,
"FirstPartyStructuredVisualTables":2,
"SourceFigureGraphSkillsMapped":8,
"OriginalNumberedSourceExercises":97,
"FirstPartyNumberedSourceSkillTasks":97,
"FirstPartyOriginalNumberedExerciseActionLeaves":221,
"FirstPartyCoolDownAuthored":True,
"TeacherAssessmentResource":assessment.relative_to(root).as_posix(),
"StudentRichV2Resource":rich.relative_to(root).as_posix(),
"OriginalPublisherGraphicsCopied":False,
"SourcePublisherPixelOrVerbatimFidelityCertified":False,
"FullSourceElementAndDiagramAtomicDenominatorCertified":False,
"OriginalSourceSubpartsEquivalentUnderIndependentTaskAdaptation":"ACADEMIC_OWNER_ACCEPTANCE_NOT_SOURCE_PUBLISHER_CERTIFICATION",
"StudentTeacherRuntimeGradingVerified":False,
"LiveStudentQuestionDeliveryVerified":False,
"RequiredCCBY4SourceAttribution":"RETAIN_IN_CREDITS",
"StageD":"NOT_RUN","Tests":"NOT_RUN","CI":"NOT_RUN",
"ReadyForStageD":True,
"ClosedAtUtc":now,
"NextSequentialLessonOrdinal":5
}
put("u01-l04-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"COMPLETE_5_SOURCE_OBJECTIVES_9_SOURCE_EXAMPLE_SKILLS_7_TRYITS_8_FIGURE_SKILLS_97_ORIGINAL_NUMBERED_QUESTIONS",
"StageB":"COMPLETE_9_INDEPENDENT_WORKED_EXAMPLES_7_INDEPENDENT_TRYITS_6_CONCEPTS_2_VISUAL_TABLES_AND_1_COOLDOWN",
"StageC":"COMPLETE_97_FIRSTPARTY_TEACHER_REVIEW_QUESTIONS_221_SOURCE_EXERCISE_ACTION_LEAVES",
"AcademicOwnerAcceptanceScope":"FIRST_PARTY_SOURCE_SKILL_MATH_AUTHORING_NOT_PUBLISHER_PIXEL_OR_VERBATIM_CERTIFICATION",
"SourcePublisherArtworkEmbedded":0,
"OriginalSourceFullPixelAndVerbatimAtomicParityCertified":False,
"StageDStudentTeacherRuntime":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,
"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l04-abc-close-audit.json",
"NextLessonOrdinal":5
})
put("u01-l04-abc-checkpoint.json",cp)
entry=p["LessonEntries"][3]
entry.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"OwnerAcademicSourceSkillEquivalentAccepted":True,
"SourceOriginalObjectivesCovered":5,
"SourceOriginalWorkedExampleSkillsCovered":9,
"SourceOriginalTryItSkillsCovered":7,
"SourceOriginalFigureSkillsMapped":8,
"OriginalNumberedExerciseSkillsAuthored":97,
"SourceOriginalNumberedExerciseActionLeavesAuthored":221,
"OriginalPublisherImagesCopied":0,
"StudentTeacherRuntimeVerified":False,
"ExactPublisherVisualAndVerbatimParityCertified":False,
"SemanticEquivalenceVerified":False,
"VisualsVerified":False,
"PracticeVerified":False,
"SourceAtomicDenominatorVerified":False,
"Tests":"NOT_RUN",
"ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l04-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=4
p["FirstIncompleteLessonOrdinal"]=5
p["ActiveLessonOrdinal"]=5
p["NextLessonOrdinal"]=5
p["SourceVerifiedGrade12Lessons"]=4
p["ReadyForStageD"]=4
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_4_OF_73_OWNER_APPROVED_SOURCE_SKILL_AUTHORING_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][4]["Ordinal"]==5 and p["LessonEntries"][4]["ReadyForStageD"] is False
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U01_L04_ACADEMIC_OWNER_A_B_C_CLOSED_97_QUESTIONS_221_NAMED_ACTIONS_NEXT_L05")
