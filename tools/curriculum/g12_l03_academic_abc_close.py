from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,data):(base/name).write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json")
cp=read("u01-l03-abc-checkpoint.json")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson003.json").read_text(encoding="utf-8"))
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l03.rich-lesson-v2.json"
rich=json.loads(richpath.read_text(encoding="utf-8"))
lesson=rich["Lessons"][0]
teacherpath=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l03-firstparty-teacher-led.json"
teacher=json.loads(teacherpath.read_text(encoding="utf-8"))
practice=read("u01-l03-source-47-firstparty-practice-contracts.json")
figures=read("u01-l03-figure19-firstparty-skill-crosswalk.json")
atoms=read("u01-l03-original-source-exercise-action-leaves.json")
tries=read("u01-l03-independent-tryit-teacher-key.json")
cool=read("u01-l03-firstparty-cooldown.json")
assert source["Section"]=="1.3" and len(source["Pages"])==13
assert p["LastClosedLessonOrdinal"]==2 and p["FirstIncompleteLessonOrdinal"]==3 and p["ReadyForStageD"]==2
assert p["LessonEntries"][2]["Ordinal"]==3 and p["LessonEntries"][2]["ReadyForStageD"] is False
assert practice["SourceSha256"]==source["SourceSha256"]
assert len(lesson["WorkedExamples"])==10 and len(lesson["KeyConcepts"])==6
assert len(lesson["Visuals"])==3 and len(tries["Items"])==4
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple("Independent Try It "+str(i)+" —" for i in range(1,5)))])==4
assert not any("TeacherExpectedAnswer" in s for s in lesson["ExplanationParagraphs"])
assert len(practice["Items"])==47 and len(teacher["IndependentStudentQuestions"])==47
assert [x["Number"]for x in teacher["IndependentStudentQuestions"]]==list(range(1,48))
assert len(teacher["TeacherOnlyExpectedAnswers"])==47
assert len(figures["Figures"])==19
assert len(atoms["Leaves"])==72 and len({x["SourceId"]for x in atoms["Leaves"]})==72
assert teacher["OriginalPublisherFiguresCopied"] is False
assert teacher["StageD"]=="NOT_RUN" and teacher["AutomatedGradebookNotYetRuntimeVerified"] is True
assert cool["OriginalTeacherOnlySourceCoolDownCopied"] is False
assert len([x for x in practice["Items"]if x["Category"]=="GRAPH_MONOTONICITY_AND_EXTREMA"])>=7
# First-party skill authoring is accepted academically by the course owner;
# do not rewrite unsupported exact-publisher fidelity or Stage D claims.
now=datetime.now(timezone.utc).isoformat()
close={
"SchemaVersion":1,
"Course":"US-CCSS-MATH:G12:PRECALCULUS",
"LessonCode":lesson["LessonCode"],"Unit":1,"Lesson":3,
"LocalOriginalSource":"OpenStax Precalculus 2017, Section 1.3, PDF pp.54–66",
"OriginalSourceSha256":source["SourceSha256"],
"OriginalFourSectionObjectivesSourceReviewed":[
 "Calculate average rate of change of a function",
 "Identify graph intervals where a function increases, decreases, or stays constant",
 "Identify local graph maxima and minima",
 "Locate absolute graph maximum and minimum"
],
"AcademicAuthoringAcceptance":"COURSE_OWNER_APPROVED_MATHEMATICALLY_EQUIVALENT_FIRST_PARTY_SKILLS_AND_TEACHER_REVIEW",
"OriginalSourceWorkedExampleSkillsRepresented":10,
"OriginalSourceTryItSkillsRepresented":4,
"OriginalSourceFigureSkillsMapped":19,
"SourceNumberedSectionExerciseSkillsRepresented":47,
"SourceNumberedQuestionLearningActionLeaves":72,
"FirstPartyKeyConcepts":6,
"FirstPartyTableVisuals":3,
"IndependentEdulyticsCoolDownAuthored":True,
"TeacherOnlyAnswerKeyResource":teacherpath.relative_to(root).as_posix(),
"OriginalSourceGraphicsOrMediaCopied":False,
"ExactPublisherPageDiagramPixelParityCertified":False,
"OriginalSourceParagraphAndGraphicAtomicFullEquivalenceCertified":False,
"FirstPartyStudentActivityAndTeacherRubricAuthenticatedRuntimeVerified":False,
"StageCExistingMathEnginesChanged":"NO",
"RequiredSourceCCBY4Attribution":"PRESERVE_PROVENANCE_CREDITS",
"CurriculumStandardSkillCandidates":[
  {"CCSS":"CCSS:HSF-IF.B.6","Skill":"Calculate and interpret average rate of change","MappingStatus":"FIRST_PARTY_SKILL_ALIGNED_NOT_SOURCE_PUBLISHER_CERTIFIED"},
  {"CCSS":"CCSS:HSF-IF.B.4","Skill":"Interpret increasing/decreasing intervals and extrema from graphs","MappingStatus":"FIRST_PARTY_SKILL_ALIGNED_NOT_SOURCE_PUBLISHER_CERTIFIED"}
],
"StageD":"NOT_RUN","Tests":"NOT_RUN","CI":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"NextLessonOrdinal":4,
"Evidence":[
 "u01-l03-abc-checkpoint.json","u01-l03-figure19-firstparty-skill-crosswalk.json",
 "u01-l03-original-source-exercise-action-leaves.json",
 "u01-l03-source-47-firstparty-practice-contracts.json",
 "u01-l03-independent-tryit-teacher-key.json","u01-l03-firstparty-cooldown.json"
]}
assert all((base/name).is_file()for name in close["Evidence"])
put("u01-l03-abc-close-audit.json",close)
cp.update({
"StageA":"COMPLETE_4_SOURCE_OBJECTIVES_10_SOURCE_EXAMPLE_CONCEPTS_4_TRYITS_19_FIGURE_SKILLS_47_NUMBERED_EXERCISES",
"StageB":"COMPLETE_10_FIRSTPARTY_WORKED_EXAMPLES_4_TRYITS_6_CONCEPTS_3_TABLE_VISUALS_AND_COOLDOWN",
"StageC":"COMPLETE_47_FIRSTPARTY_STUDENT_SKILL_QUESTIONS_AND_72_SOURCE_QUESTION_REVIEW_ACTIONS_WITH_TEACHER_EXPECTED_MATH",
"SourceAcademicSkillAuthoringAcceptance":"OWNER_APPROVED_FIRST_PARTY_EQUIVALENT_NOT_PIXEL_OR_PUBLISHER_VERBATIM",
"SourceOriginalFigureImagesCopied":0,
"AcademicOwnerApproved":True,
"OriginalPublisherFullExactAtomicAndImageParityCertified":False,
"StudentTeacherRuntimeAndAutomatedGradesVerified":False,
"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ClosedAtUtc":now,
"AcademicCloseEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l03-abc-close-audit.json",
"BlockingReasonsRemainOpen":False
})
put("u01-l03-abc-checkpoint.json",cp)
entry=p["LessonEntries"][2]
entry.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerApprovedSkillEquivalentSourceAdaptation":True,
"OriginalSourceFullExactSemanticOrGraphicParityClaim":False,
"OriginalSourceFourLearningObjectivesCovered":4,
"OriginalWorkedExampleSkillsCovered":10,
"OriginalTryItSkillsCovered":4,
"OriginalFigureSkillsMapped":19,
"OriginalNumberedSourceExerciseSkillsCovered":47,
"OriginalNumberedExerciseLearningActionLeaves":72,
"FirstPartyStudentContentAndManualTeacherKeyAuthored":True,
"StageDStudentTeacherRuntimeVerified":False,
"SourcePublisherGraphicsCopied":0,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,
"PracticeVerified":False,
"VisualsVerified":False,
"RightsVerified":False,
"Tests":"NOT_RUN",
"ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l03-abc-close-audit.json",
"LastCommitSha":None
})
p["LastClosedLessonOrdinal"]=3
p["FirstIncompleteLessonOrdinal"]=4
p["ActiveLessonOrdinal"]=4
p["NextLessonOrdinal"]=4
p["SourceVerifiedGrade12Lessons"]=3
p["ReadyForStageD"]=3
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_3_OF_73_OWNER_APPROVED_SKILL_COVERAGE_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][3]["Ordinal"]==4
assert p["LessonEntries"][3]["ReadyForStageD"] is False
p["Publication"]="NOT_AUTHORIZED_BY_A_C"
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("GRADE12_U01_L03_ACADEMIC_SKILL_A_B_C_CLOSED; MOVE_SEQUENTIALLY_TO_U01_L04; 3_OF_73_READY_FOR_STAGE_D")
