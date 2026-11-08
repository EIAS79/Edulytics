from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,data):(base/name).write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json")
c=read("u01-l02-abc-checkpoint.json")
book=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson002.json").read_text(encoding="utf-8"))
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
rich=json.loads(richpath.read_text(encoding="utf-8"))
lesson=rich["Lessons"][0]
assessment_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l02-firstparty-teacher-led.json"
a=json.loads(assessment_path.read_text(encoding="utf-8"))
source_q=read("u01-l02-source-61-first-party-practice-contracts.json")
atoms=read("u01-l02-104-original-exercise-leaf-actions.json")
figures=read("u01-l02-figure26-first-party-skill-crosswalk.json")
tries=read("u01-l02-independent-tryit-teacher-key.json")
worked=read("u01-l02-example13-source-skill-crosswalk.json")
cool=read("u01-l02-first-party-cooldown.json")
assert p["LastClosedLessonOrdinal"]==1 and p["FirstIncompleteLessonOrdinal"]==2 and p["ReadyForStageD"]==1
assert p["LessonEntries"][1]["LessonNumber"]==2 and p["LessonEntries"][1]["Unit"]==1
assert book["Section"]=="1.2" and len(book["Pages"])==16
assert book["SourceSha256"]==source_q["SourceSha256"]
assert len(lesson["WorkedExamples"])==13 and len(worked["Items"])==13
assert len(lesson["KeyConcepts"])==7 and len(lesson["Visuals"])==8
assert len(tries["Items"])==8
assert len(figures["Rows"])==26 and len(set(x["OriginalSourceFigureNumber"]for x in figures["Rows"]))==26
assert len(source_q["OriginalNamedExerciseItems"])==61 and len(a["StudentPracticeQuestions"])==61
assert len(a["TeacherOnlyExpectedMathematics"])==61
assert sorted(x["Number"]for x in a["StudentPracticeQuestions"])==list(range(1,62))
assert len(atoms["Leaves"])==104
assert len({x["SourceElementId"]for x in atoms["Leaves"]})==104
assert all(x["SourceLearningAction"] and x["FirstPartyTeacherKeyAvailable"]for x in atoms["Leaves"])
assert a["StudentPortalRuntimeBound"] is False
assert a["GradebookSubmissionAuthenticated"] is False
assert a["StageDTestsAndCINotRun"] is True
assert cool["OriginalPublisherRestrictedCoolDownCopied"] is False
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple("Independent Try It "+str(n)+" —" for n in range(1,9)))])==8
assert "Expected result for teacher review" not in " ".join(lesson["ExplanationParagraphs"])
assert a["OriginalPublisherImagesReused"] is False
assert figures["OriginalPublisherThirdPartyAssetsCopied"]==0
# Academic A-B-C is owner-approved FIRST-PARTY skill equivalence and authored
# pedagogy, not full exact source-text/image fidelity, automated grading, CI,
# live SQL persistence or production distribution. Keep all explicit boundaries.
now=datetime.now(timezone.utc).isoformat()
close={
"SchemaVersion":1,
"Course":"US-CCSS-MATH:G12:PRECALCULUS",
"Unit":1,"Lesson":2,
"LessonCode":lesson["LessonCode"],
"OriginalSource":"Authorized OpenStax Precalculus 2017 local PDF Section 1.2",
"OriginalSourceSHA256":book["SourceSha256"],
"SourcePagesPdf":[38,53],
"AcademicOwnerApprovalScope":"FIRST_PARTY_MATHEMATICALLY_EQUIVALENT_SKILL_CONTENT_NOT_PIXEL_OR_VERBATIM_PUBLISHER_MATCH",
"OriginalSourceTwoLearningObjectivesCovered":[
 "Determine real domains for equations, graphs and contextual input restrictions",
 "Graph piecewise-defined functions with correct branch endpoint membership"
],
"IndependentWorkedExamplesAuthored":13,
"FirstPartyIndependentTryItsAuthored":8,
"FirstPartySourceSkillGraphFiguresMapped":26,
"FirstPartyOriginalNumberedExerciseSkillAlternatives":61,
"OriginalNumberedExerciseNamedSubpartActionsAuthored":104,
"FirstPartyToolkitDomainRangeProfiles":9,
"FirstPartyTableVisuals":8,
"FirstPartyIndependentCoolDown":True,
"StageCFirstPartyAssessmentResource":assessment_path.relative_to(root).as_posix(),
"PedagogicAuthoringSourceChecks":"13 examples, 8 Try Its, 26 figure concepts, 61 numbered skill equivalents, 104 non-overlapping exercise actions, original mathematical domain/range and piecewise endpoint checks",
"OriginalPublisherGraphicsCopied":False,
"OriginalPublisherExactFigureAndVerbatimSemanticParityCertified":False,
"CompleteOriginalPublisherTextAndPixelAtomDenominatorCertified":False,
"RequiredCCBY4AttributionMustRemainInSourceCredits":True,
"OriginalSourceThirdPartyChartsCopied":False,
"StageCCodeEngineChanges":"NONE; uses existing first-party RichV2 and teacher-review assessment JSON convention",
"StudentTeacherAuthenticatedRuntimeVerified":False,
"TeacherGradebookPersistenceVerified":False,
"CI":"NOT_RUN",
"Tests":"NOT_RUN",
"StageD":"NOT_RUN",
"ReadyForStageD":True,
"AcademicClosedAtUtc":now,
"NextLessonOrdinal":3,
"EvidenceFiles":[
 "u01-l02-abc-checkpoint.json",
 "u01-l02-figure26-first-party-skill-crosswalk.json",
 "u01-l02-example13-source-skill-crosswalk.json",
 "u01-l02-independent-tryit-teacher-key.json",
 "u01-l02-source-61-first-party-practice-contracts.json",
 "u01-l02-104-original-exercise-leaf-actions.json",
 "u01-l02-first-party-cooldown.json"
]}
for f in close["EvidenceFiles"]:assert (base/f).is_file()
put("u01-l02-abc-close-audit.json",close)
c.update({
"AcademicOwnerAcceptanceStatus":"A_B_C_COMPLETE_FIRST_PARTY_PEDAGOGIC_MATH_SKILL_EQUIVALENCE",
"StageA":"COMPLETE_2_LEARNING_OBJECTIVES_13_ORIGINAL_EXAMPLE_CONCEPTS_8_TRYITS_26_FIGURE_CONCEPTS_61_EXERCISE_SKILLS_LOCAL_PDF",
"StageB":"COMPLETE_13_FIRSTPARTY_WORKED_EXAMPLES_8_TRYITS_7_CONCEPTS_8_VISUAL_TABLES_AND_INDEPENDENT_COOLDOWN",
"StageC":"COMPLETE_61_FIRSTPARTY_SOURCE_SKILL_ASSESSMENT_QUESTIONS_WITH_104_ORIGINAL_ACTION_CHECKLIST_AND_TEACHER_EXPECTED_MATH",
"SourceAcademicNamedExerciseActionCoverageComplete":True,
"SourceExactVerbatimPixelParityCertified":False,
"OriginalPublisherVisualReproductionRequired":False,
"StudentPortalRuntimeBound":False,
"TeacherDatabasePersistenceVerified":False,
"StageD":"NOT_RUN",
"Tests":"NOT_RUN",
"ReadyForStageD":True,
"Status":"READY_FOR_STAGE_D",
"BlockingReasonsRemainOpen":False,
"ReasonForNotMovingToLesson3":[],
"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l02-abc-close-audit.json"
})
put("u01-l02-abc-checkpoint.json",c)
entry=p["LessonEntries"][1]
entry.update({
"ACStatus":"READY_FOR_STAGE_D",
"ReadyForStageD":True,
"AcademicOwnerApprovedSkillEquivalentSourceAdaptation":True,
"OriginalSourceFullExactSemanticOrGraphicParityClaim":False,
"OriginalNumberedSourceExercisesAcademicSkillAlternatives":61,
"OriginalNamedExerciseSubpartActionsAuthored":104,
"OriginalSourceFiguresFirstPartyAlternatives":26,
"SourceWorkedExamplesCovered":13,
"SourceTryItsCovered":8,
"FirstPartyCoolDownAuthored":True,
"OriginalPublisherThirdPartyArtCopied":0,
"FirstPartyStudentAssessmentAuthoringComplete":True,
"StudentAssessmentWiredRuntime":False,
"RuntimePracticeAndGradebookVerified":False,
"PracticeVerified":False,
"SemanticEquivalenceVerified":False,
"VisualsVerified":False,
"RightsVerified":False,
"SourceAtomicDenominatorVerified":False,
"Tests":"NOT_RUN",
"ClosedAtUtc":now,
"UpdatedUtc":now,
"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l02-abc-close-audit.json",
"LastCommitSha":None
})
p["LastClosedLessonOrdinal"]=2
p["FirstIncompleteLessonOrdinal"]=3
p["ActiveLessonOrdinal"]=3
p["NextLessonOrdinal"]=3
p["SourceVerifiedGrade12Lessons"]=2
p["ReadyForStageD"]=2
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_2_OF_73_OWNER_APPROVED_SKILL_COVERAGE_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][2]["Ordinal"]==3
assert p["LessonEntries"][2]["ReadyForStageD"] is False
p["Publication"]="NOT_AUTHORIZED_BY_A_C"
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("GRADE12_U01_L02_ACADEMIC_A_B_C_OWNER_SKILL_AUTHORING_CLOSED; NEXT_SEQUENTIAL_U01_L03; 2_OF_73_READY_FOR_STAGE_D")
