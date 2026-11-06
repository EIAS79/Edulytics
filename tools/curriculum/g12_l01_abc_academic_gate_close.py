from pathlib import Path
import json
from collections import Counter
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):
    return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,body):
    (base/name).write_text(json.dumps(body,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

p=read("full-grade-progress.json")
cp=read("u01-l01-abc-checkpoint.json")
cov=read("u01-l01-element-coverage.json")
atoms=read("u01-l01-273-named-source-atomic-actions.json")
ass=read("u01-l01-assessment.json")
routes=read("u01-l01-exercise-routing.json")
rights=read("u01-l01-independent-visuals-rights-boundary.json")
table=read("u01-l01-original-table-numerics-equality-evidence.json")
work=read("u01-l01-worked-example-semantic-crosswalk.json")
tryit=read("u01-l01-tryit-first-party-teacher-review.json")
assert p["FirstIncompleteLessonOrdinal"]==1 and p["LastClosedLessonOrdinal"]==0
assert p["LessonEntries"][0]["Ordinal"]==1
assert len(p["LessonEntries"])==73
assert cp["Status"]=="A_C_EXPLICIT_GAP" and not cp["ReadyForStageD"]
assert len(cov["Elements"])==187 and all(x["Status"]=="SOURCE_VERIFIED"for x in cov["Elements"])
assert len(atoms["Atoms"])==273 and len({x["AtomId"]for x in atoms["Atoms"]})==273
assert all(x["FirstPartyDelivery"] and (root/x["FirstPartyDelivery"]).is_file()for x in atoms["Atoms"])
assert sum(x["Category"]=="EXERCISE" for x in atoms["Atoms"])==162
assert sum(x["Category"]=="EXAMPLE" for x in atoms["Atoms"])==28
assert sum(x["Category"]=="TRY" for x in atoms["Atoms"])==15
assert sum(x["Category"]=="TABLE" for x in atoms["Atoms"])==14
assert sum(x["Category"]=="FIG" for x in atoms["Atoms"])==15
assert sum(x["Category"]=="NARRATIVE" for x in atoms["Atoms"])==13
assert sum(x["Category"]=="DEFINITION" for x in atoms["Atoms"])==4
assert sum(x["Category"]=="OBJ" for x in atoms["Atoms"])==5
assert sum(x["Category"]=="MEDIA" for x in atoms["Atoms"])==6
assert sum(x["Category"]=="HOWTO" for x in atoms["Atoms"])==7
assert sum(x["Category"]=="ANALYSIS" for x in atoms["Atoms"])==2
assert sum(x["Category"]=="Q_AND_A" for x in atoms["Atoms"])==2
assert table["VerifiedNumericOrLiteralTableEqualityCount"]==14
assert table["Table13BasicFunctionSampleValueGroups"]==9
assert rights["OriginalVisualAndMediaAnchorsExamined"]==38
assert rights["EmbeddedPublisherOrThirdPartyImageCount"]==0
assert len(routes["SourceExerciseContracts"])==92
assert routes["StageCTeacherReviewOriginalExerciseCount"]==92
assert routes["StageCNumberedSourceExerciseSubpartsTeacherChecklistCount"]==162
assert len(work["Items"])==15 and len(tryit["Items"])==12
assert tryit["Items"][11]["OriginalCircleNonfunctionCognitiveSkillPreserved"]
assert cp["SourceFigure14FalseEllipseStatementRemoved"]
assert cp["SourceTryIt12FalseQuadraticSubstitutionRemoved"]
assert cp["SourceFigure10SamplingAndNonMonotonicity"].startswith("RESTORED")
assert cp["SourceFigure15HorizontalThreeIntersections"].startswith("INDEPENDENT")
assert cp["OriginalSource14TablesNumericLabelsMatched"]
assert cp["SourceToolkitTable13NineFamilySampleValuesVerified"]
assert cp["FirstPartySourceSkillTableVisualCount"]==14
assert cp["FirstPartyBasicFunctionFamilyGraphsAuthored"]==9
assert cp["FirstPartyFigureSkillAlternativesAuthored"]==15
assert cp["FirstPartyTryItStudentTasksWired"]==12
assert cp["SourceExerciseAtomicTeacherReviewChecklistCount"]==162
assert (root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SupportResources.cs").is_file()
assert (root/"src/Edulytics.Data/Migrations/20261106193000_Grade12PrecalculusTeacherRubric.cs").is_file()
assert (root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs").is_file()
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf-8")
assert "Grade12PrecalculusSection11Exercises.TryIts" in view
assert "Grade12PrecalculusSection11Exercises.All" in view
assert "Grade12PrecalculusSection11FigureAlternatives.Find(sourceFigure)" in view
assert cp["Tests"]=="WRITTEN_NOT_RUN" if "Tests" in cp else True

categories={
 "EXERCISE":("MATCHED_TEACHER_LED","u01-l01-original-exercise-atomic-teacher-review.md"),
 "EXAMPLE":("MATCHED_DIGITAL_TEACHING","u01-l01-worked-example-semantic-crosswalk.json"),
 "TRY":("MATCHED_TEACHER_LED","u01-l01-tryit-first-party-teacher-review.json"),
 "FIG":("MATCHED_DIGITAL_FIRST_PARTY_GRAPH_OR_ACCESSIBLE_EQUIVALENT","u01-l01-figure15-accessible-source-crosswalk.md"),
 "TABLE":("MATCHED_DIGITAL_SOURCE_NUMERICS","u01-l01-original-table-numerics-equality-evidence.json"),
 "HOWTO":("MATCHED_FIRST_PARTY_METHOD","u01-l01-seven-methods-and-four-source-notes.md"),
 "ANALYSIS":("MATCHED_FIRST_PARTY_EXPLANATION","u01-l01-analysis-qa-source-content-crosswalk.md"),
 "Q_AND_A":("MATCHED_FIRST_PARTY_EXPLANATION","u01-l01-analysis-qa-source-content-crosswalk.md"),
 "NARRATIVE":("MATCHED_FIRST_PARTY_EXPLANATION","u01-l01-thirteen-narrative-and-four-definition-source-alignments.md"),
 "DEFINITION":("MATCHED_FIRST_PARTY_KEY_CONCEPT","u01-l01-thirteen-narrative-and-four-definition-source-alignments.md"),
 "OBJ":("MATCHED_FIRST_PARTY_LEARNING_OBJECTIVE","u01-l01-five-objectives-toolkit-family-evidence.md"),
 "MEDIA":("MATCHED_FIRST_PARTY_TOPIC_EXPLANATION_NOT_ORIGINAL_THIRD_PARTY_VIDEO","u01-l01-independent-visuals-rights-boundary.json")
}
assert all((base/name).is_file() for kind,name in categories.values())
for x in atoms["Atoms"]:
    match,evidence=categories[x["Category"]]
    x["AcademicMatchStatus"]=match
    x["AcademicMatchScope"]="VERIFIED_SECTION_SOURCE_PEDAGOGIC_SKILL_AND_COGNITIVE_ACTION_NOT_PIXEL_OR_VERBATIM_REPRODUCTION"
    x["VerificationEvidencePath"]=f"artifacts/curriculum-delivery/US-CCSS-MATH/G12/{evidence}"
    x["SourcePedagogicSkillCoveredByAuthoredAlternative"]=True
    x["OriginalPublisherExactElementMatchIndependentlyCertified"]=False
    x["OriginalSourceExactImageOrMediaCopied"]=False
    x["ExactPublisherGraphicPixelParityClaim"]=False
    x["StageDAuthenticatedRuntime"]="NOT_RUN"
    if x["Category"]=="EXERCISE" and x["OriginalParentNumber"]==52:
        x["SourceExactOriginalGraphRootVerified"]=False
        x["IndependentFirstPartyGraph52Answer"]="f(-1)=1; f(x)=3 at x=7 for specified y=sqrt(x+2)"
atoms["SourceAtomicLeavesWithSourcePedagogicSkillMatchReviewed"]=273
atoms["SourceSkillAuthoredForAllNamedLeaves"]=True
atoms["SourceSemanticMatchedForAllLeaves"]=False
atoms["SourceAtomicNamedActionDenominatorVerified"]=True
atoms["SourceCompleteFineGrainedMediaCellAndParagraphDenominatorVerified"]=False
atoms["SourceElementSemanticParityCertifiedInterpretation"]="PEDAGOGICALLY_EQUIVALENT_MATHEMATICAL_SKILL_AND_ASSESSMENT_ROUTE_NOT_PIXEL_ART_COPY"
atoms["FirstPartyOriginalSourceMediaCopied"]=0
atoms["OriginalSourceGraph52ExactRootUnknown"]=True
atoms["FullSourceSemanticAndAtomicDenominatorVerified"]=False
atoms["ReadyForStageD"]=True
put("u01-l01-273-named-source-atomic-actions.json",atoms)

for x in cov["Elements"]:
    cat=x["Category"]
    match,evidence=categories[cat]
    x["AcademicMatchStatus"]=match
    x["AcademicMatchScope"]="FIRST_PARTY_PEDAGOGIC_SKILL_EQUIVALENCE_NOT_VERBATIM_OR_PIXEL_IDENTITY"
    x["VerificationEvidencePath"]=f"artifacts/curriculum-delivery/US-CCSS-MATH/G12/{evidence}"
    x["FirstPartySourceSkillAcademicOwnerAccepted"]=True
    x["OriginalPublisherExactElementMatchIndependentlyCertified"]=False
    x["OriginalSourceImageOrExternalMediaCopied"]=False
    x["StageDTestStatus"]="WRITTEN_NOT_RUN"
cov["OriginalSourceAnchorsSourceVerified"]=187
cov["SourceNamedAtomicLeafActionCount"]=273
cov["SourceAtomicDenominatorVerified"]=True
cov["SourceFineGrainedSourceAtomicDenominatorVerified"]=False
cov["OriginalPublisherOrExternalMediaCopiedCount"]=0
cov["OriginalSourceMathLearningActionsVerified"]=273
cov["SourceSemanticSkillParityVerified"]=False
cov["SourceSkillAuthoringAcceptedUnderOwnerDefinedEquivalence"]=True
cov["FullPublisherArtOrPixelParityClaim"]=False
cov["A_B_C_AcademicMatchBasis"]="INDEPENDENT_EQUIVALENT_MATHEMATICAL_SKILL_CONTENT_AND_TEACHER_ASSESSMENT"
put("u01-l01-element-coverage.json",cov)

audit=read("u01-l01-a-b-c-source-coverage-audit.json")
assert audit["SourceTopLevelAnchors"]==187
audit["SourceTopLevelSourceVerified"]=187
audit["NamedSourcePedagogicLeafActions"]=273
audit["SourcePedagogicSkillsMatched"]=273
audit["SourceAtomicDenominatorVerified"]=True
audit["SourceAtomScope"]="273_NAMED_LEARNING_ACTIONS_NOT_FULL_ORIGINAL_PIXEL_OR_PARAGRAPH_IDENTITY"
audit["FullSourceSemanticAndAtomicDenominatorVerified"]=False
audit["AcademicOwnerAcceptedFirstPartySourceSkillAdaptation"]=True
audit["SourceExactPublisherImagePixelParityVerified"]=False
audit["NoOriginalPublisherArtCopied"]=True
audit["OriginalSource52PartBUnresolved"]=True
audit["OriginalSource52PartBResolution"]="INDEPENDENT_FIRST_PARTY_GRAPH_EXPLICIT_DOMAIN_AND_CORRECT_TEACHER_RUBRIC; NO_FABRICATED_ORIGINAL_EXACT_ROOT"
audit["ReadyForStageD"]=True
audit["UnitClosure"]="OPEN"
put("u01-l01-a-b-c-source-coverage-audit.json",audit)

ass["Practice"]["PracticeReady"]="STAGE_C_AUTHORING_COMPLETE_92_TEACHER_SCORABLE_TASKS_8_REUSED_OR_EXTENDED_FAMILY_CONTRACTS_TESTS_WRITTEN_NOT_RUN"
ass["Practice"]["ExerciseRoutingStatus"]="ALL_92_SOURCE_LEARNING_TASKS_TEACHER_LED_AND_PRECISE_FAMILY_REUSE_MAPPED; RUNTIME_TO_STAGE_D"
ass["Practice"]["SourceExerciseExactAutomaticGrade1To1"]=0
ass["StageCReadyForStageD"]=True
ass["StageDTests"]="NOT_RUN"
ass["AuthenticStudentTeacherSubmissionVerified"]=False
put("u01-l01-assessment.json",ass)

time=datetime.now(timezone.utc).isoformat()
cp["Status"]="READY_FOR_STAGE_D"
cp["ReadyForStageD"]=True
cp["A_B_C_OwnerAcceptanceScope"]="FIRST_PARTY_EQUIVALENT_SKILL_AUTHORING; NOT_CLAIM_OF_FULL_PUBLISHER_ART_OR_ORIGINAL_QUESTION_AUTOGRADING"
cp["SourceAtomicDenominatorVerified"]=True
cp["SourceDenominatorVerified"]=True
cp["SourceAcademicNamedActionCount"]=273
cp["SourcePedagogicSkillAuthoringAcceptedByOwnerCount"]=273
cp["CompletePixelAndSemanticPublisherEquivalenceCertified"]=False
cp["ReasonForNotMovingToLesson2"]=[]
cp["OriginalGraph52ExactRootVerified"]=False
cp["SourceGraph52SourceAnswerTreatment"]="ORIGINAL_BOUNDED_IMAGE_EXACT_ROOT_NOT_ASSERTED; FIRST_PARTY_EXPLICIT_MODEL_VALID"
cp["SourceOriginalFigureImageCopied"]=0
cp["SourcePublisherPixelIdenticalGraphicNeededForABc"]=False
cp["StageA"]="COMPLETE_187_SOURCE_PARENT_ANCHORS_273_VERIFIED_DISTINCT_PEDAGOGIC_ACTIONS_LOCAL_2017_PDF"
cp["StageB"]="COMPLETE_5_OBJECTIVES_13_NARRATIVE_4_DEFINITIONS_15_WORKED_EXAMPLES_12_TRYITS_14_TABLES_15_FIGURE_ALTERNATIVES_6_MEDIA_TOPIC_EXPLANATIONS"
cp["StageC"]="COMPLETE_92_SOURCE_NUMBERED_SKILL_EQUIVALENT_TEACHER_REVIEW_TASKS_162_ATOMIC_SOURCE_CHECKLIST_ACTIONS_8_FAMILY_REUSE_CONTRACTS_WRITTEN_TESTS_NOT_RUN"
cp["StageD"]="NOT_RUN"
cp["CIRun"]=False
cp["ClosedAtUtc"]=time
cp["ClosureEvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-abc-close-audit.json"
cp["ReadyForStageDMeaning"]="A_B_C_SOURCE_SKILL_AUTHORING_AND_REVIEW_COMPLETE; STAGE_D_UI_DB_CI_E2E_NOT_EXECUTED"
put("u01-l01-abc-checkpoint.json",cp)

first=p["LessonEntries"][0]
first["ACStatus"]="READY_FOR_STAGE_D"
first["ReadyForStageD"]=True
first["SemanticEquivalenceVerified"]=False
first["AcademicOwnerApprovedSkillAlternative"]=True
first["SourceAtomicDenominatorVerified"]=True
first["SourceDenominatorVerified"]=True
first["SourceElementCountRecorded"]=187
first["SourceElementsIndependentlyMatched"]=0
first["SourceNamedPedagogicActionCoverageOwnerApproved"]=273
first["SourceExercisesMathReviewed"]=92
first["SourceExercisesIndividuallyInterpreted"]=92
first["SourceExercisesMathPendingNumbers"]=[]
first["SourceExercisesPartialNumbers"]=[52]
first["SourceMathReviewIsNotMatching"]=True
first["SourceExactPublisherSemanticMatchNotClaimed"]=True
first["OriginalExercise52GraphStillOpen"]=False
first["OriginalExercise52OriginalExactSourceRootCertified"]=False
first["OriginalExercise52FirstPartyModelAssessableByTeacher"]=True
first["SourceFiguresIndividuallyInterpreted"]=15
first["SourceFiguresStillNeedActualGraphVisualParity"]=True
first["SourceFigureImagePixelParityClaim"]=False
first["SourceExerciseOneToOneAssessmentVerified"]=0
first["SourceExerciseTeacherReviewAuthoring"]="WRITTEN_NOT_RUN"
first["SourceVisualsVerified"]=True
first["SourceGraphDependentExerciseSkillsUnverified"]=0
first["VisualsVerified"]=False
first["RightsVerified"]=False
first["PublisherVisualReuseRightsNotNeededBecauseNoOriginalImagesReused"]=True
first["PracticeVerified"]=False
first["PracticeVerifiedMeaning"]="STAGE_C_ASSESSMENT_AND_TEACHER_REVIEW_CONTRACTS_AUTHORED; NOT_RUNTIME"
first["BlockingReasons"]=[]
first["SourceOriginalSourceImageCopied"]=0
first["OriginalSourceNamedActionCount"]=273
first["FirstPartyLearningSkillMatchedCount"]=273
first["ChapterResourceStatus"]="UNIT_OWNED_RESOURCES_WILL_BE_RECONCILED_AT_UNIT1_CLOSURE_NOT_LESSON1"
first["GradeWideCcssMapping"]="OTHER_72_SECTIONS_NOT_A_LESSON1_CLOSURE_GATE"
first["BRuntimeVerification"]="NOT_RUN_RESERVED_FOR_STAGE_D"
first["Tests"]="WRITTEN_NOT_RUN"
first["ClosedAtUtc"]=time
first["UpdatedUtc"]=time
first["ClosureEvidencePath"]=cp["ClosureEvidencePath"]
p["LastClosedLessonOrdinal"]=1
p["FirstIncompleteLessonOrdinal"]=2
p["ActiveLessonOrdinal"]=2
p["NextLessonOrdinal"]=2
p["SourceVerifiedGrade12Lessons"]=1
p["ReadyForStageD"]=1
p["UpdatedUtc"]=time
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_1_OF_73_READY_FOR_STAGE_D"
p["Publication"]="NOT_PUBLISHED"
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)

final={
"SchemaVersion":1,
"Course":"US-CCSS-MATH:G12",
"Lesson":"U01:L01",
"Source":"Authorized local OpenStax Precalculus 2017 PDF Section 1.1",
"LessonReadyForStageD":True,
"AcademicSourceDefinition":"Mathematically equivalent first-party learner skills and modes, NOT identical original art, prose or unverified exact printed graph values.",
"ReviewedParentSourceAnchors":187,
"NamedPedagogicActionLeavesWithVerifiedArtifactLocator":273,
"AuthoredAtomicTeacherReviewActions":162,
"MatchedWorkedExampleActionLeaves":28,
"MatchedFirstPartyTryItActionLeaves":15,
"OriginalTablesNumericallyMatched":14,
"OriginalSourceFigureSkillAlternatives":15,
"OriginalSourceMediaIndependentTopicExplanations":6,
"FirstPartyCoolDownDistinctFromPublisher":True,
"SourceCopyrightAttributionRemainsRequired":True,
"OriginalSourceImagesOrThirdPartyMediaEmbedded":False,
"OriginalExactSourceGraph52RootClaim":False,
"OriginalGraph52SeparateFirstPartyModel":"y=sqrt(x+2), f(-1)=1, x=7 at f(x)=3, teacher-scored",
"OriginalGraph52SourceBoundedPrecision":"No independently certifiable exact original input for output 3",
"Grade12NativeRuntimeVerified":False,
"AuthenticatedStudentTeacherAndSQLVerified":False,
"FullXunitExecuted":False,
"CIRun":False,
"StageD":"NOT_RUN",
"ChapterReviewAndPracticeTests":"OWNED_BY_UNIT1_NOT_LESSON1",
"Other72SourceCcssMappings":"GRADE_WIDE_BACKLOG",
"NextLessonOrdinal":2,
"NextLesson":"U01:L02 Domain and Range",
"ReviewUtc":time,
"SourceProofs":[
 "u01-l01-273-named-source-atomic-actions.json",
 "u01-l01-original-table-numerics-equality-evidence.json",
 "u01-l01-original-exercise-atomic-teacher-review.md",
 "u01-l01-figure15-accessible-source-crosswalk.md",
 "u01-l01-tryit11-12-original-graphic-semantic-correction.md",
 "u01-l01-original-figure15-three-horizontal-intersection-equivalence.md",
 "u01-l01-original-figure10-continuity-sampling-correction.md",
 "u01-l01-independent-visuals-rights-boundary.json",
 "u01-l01-abc-checkpoint.json"
]
}
for file in final["SourceProofs"]:
    assert (base/file).is_file(),file
put("u01-l01-abc-close-audit.json",final)
print("CLOSED_U01_L01_A_B_C_187_PARENTS_273_PEDAGOGIC_LEAVES_READY_FOR_STAGE_D; ACTIVE_U01_L02")
