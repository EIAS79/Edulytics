from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json")
cp=read("u01-l05-abc-checkpoint.json")
q=read("u01-l05-source-81-firstparty-question-contracts.json")
leaf=read("u01-l05-original-numbered-exercise-action-leaves.json")
fig=read("u01-l05-32-original-figure-skill-crosswalk.json")
trykey=read("u01-l05-teacher-tryit-11-and-cooldown.json")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson005.json").read_text(encoding="utf-8"))
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l05.rich-lesson-v2.json"
rich=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l05-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert source["Section"]=="1.5" and len(source["Pages"])==25
assert p["LastClosedLessonOrdinal"]==4 and p["FirstIncompleteLessonOrdinal"]==5
assert p["LessonEntries"][4]["Ordinal"]==5 and not p["LessonEntries"][4]["ReadyForStageD"]
assert source["SourceSha256"]==q["SourceSha256"]==fig["SourceSha256"]
assert len(rich["WorkedExamples"])==20 and len(rich["KeyConcepts"])==7 and len(rich["Visuals"])==2
assert len([x for x in rich["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,12)))])==11
assert len(trykey["TeacherTryItAnswers"])==11
assert len(q["Items"])==81 and [x["Number"]for x in q["Items"]]==list(range(1,82))
assert len(assessment["FirstPartyStudentQuestions"])==81 and len(assessment["TeacherOnlyMathAnswers"])==81
assert len(fig["Items"])==32 and len({x["OriginalFigureNumber"]for x in fig["Items"]})==32
assert len(leaf["Items"])==176 and len({x["SourceId"]for x in leaf["Items"]})==176
assert all("An independent source-skill graph" in q["Items"][n-1]["FirstPartyStudentPrompt"]for n in range(33,47))
assert not any("Expected" in s or "TeacherOnly" in s for s in rich["ExplanationParagraphs"])
assert assessment["OriginalPublisherGraphAssetsCopied"] is False
assert assessment["LiveStudentDeliveryAndGradingNotTested"] is True
assert trykey["TeacherCoolDown"]["OriginalPublisherRestrictedCooldownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":5,
"Source":"Authorized local OpenStax Precalculus (2017) Section 1.5 original PDF pp.80–104",
"SourcePdfSHA256":source["SourceSha256"],
"AcademicOwnerApprovedSkillEquivalentAdaptation":True,
"AcceptanceMeaning":"INDEPENDENT_FIRST_PARTY_MATH_PEDAGOGIC_SOURCE_SKILL_COVERAGE_AND_MANUAL_TEACHER_SCORING_AUTHORING_NOT_EXACT_ORIGINAL_BOOK_ART_OR_AUTOGRADING",
"SourceFiveLearningObjectivesVerified":[
"Graph vertical and horizontal shifts",
"Graph reflections about both coordinate axes",
"Classify even, odd or neither from the graph",
"Graph stretches and compressions",
"Combine multiple transformations"
],
"OriginalSourceWorkedExamples":20,"FirstPartyWorkedExampleSkillRepresentations":20,
"OriginalSourceTryIts":11,"FirstPartyTryItAlternatives":11,
"OriginalSourceFigureAnchors":32,"FirstPartyFigureCognitiveSkillsCrosswalked":32,
"OriginalSourceNumberedExercises":81,"FirstPartyTeacherReviewPracticeQuestions":81,
"SourceNumberedExerciseActionLeavesMapped":176,
"FirstPartyCoolDown":True,
"FirstPartyTableVisuals":2,
"StudentRichV2Path":rp.relative_to(root).as_posix(),
"TeacherManualSourceSkillAssessmentResource":ap.relative_to(root).as_posix(),
"OriginalSourcePublisherFiguresCopied":False,
"SourceOriginalFlickrOrThirdPartyImageCopied":False,
"OriginalExactPublisherFigureAndParagraphPixelParityCertified":False,
"SourceCompletePixelAndParagraphAtomicDenominatorCertified":False,
"StageDStudentPortalAuthenticatedRuntimeVerified":False,
"TeacherRubricSubmissionGradebookRuntimeVerified":False,
"LegalSourceCreditsRequired":"CC_BY_4_ATTRIBUTION_RETAINED",
"CCSSCandidate":[
 {"Code":"CCSS:HSF-BF.B.3","Reason":"Identify effect of replacing f(x) with f(x)+k, kf(x), f(kx), f(x+k), and analyze odd/even functions","Status":"SOURCE_SKILL_CANDIDATE_NEEDS_GRADEWIDE_REVIEW"},
 {"Code":"CCSS:HSF-IF.C.7","Reason":"Graph represented toolkit transformations","Status":"SOURCE_SKILL_CANDIDATE_NEEDS_GRADEWIDE_REVIEW"}],
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextLessonOrdinal":6
}
put("u01-l05-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"COMPLETE_5_SOURCE_OBJECTIVES_20_WORKED_SKILLS_11_TRYITS_32_FIGURE_SKILLS_81_NUMBERED_EXERCISES",
"StageB":"COMPLETE_20_INDEPENDENT_WORKED_11_TRYIT_7_KEY_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN",
"StageC":"COMPLETE_81_INDEPENDENT_NUMBERED_TEACHER_REVIEW_QUESTIONS_AND_176_SOURCE_ACTION_LEAVES",
"OriginalPublisherFullArtAndParagraphParityClaim":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l05-abc-close-audit.json"})
put("u01-l05-abc-checkpoint.json",cp)
entry=p["LessonEntries"][4]
entry.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerApprovedFirstPartySkillAdaptation":True,
"OriginalSourceObjectivesCovered":5,
"OriginalSourceWorkedExampleSkillsCovered":20,
"OriginalSourceTryItSkillsCovered":11,
"OriginalSourceFigureSkillsCrosswalked":32,
"OriginalSourceNumberedExerciseSkillsAuthored":81,
"OriginalSourceNumberedExerciseActionLeavesAuthored":176,
"OriginalSourcePublisherAndThirdPartyArtCopied":0,
"SourceFullExactPublisherVisualParagraphParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,
"VisualsVerified":False,
"PracticeVerified":False,
"StageDStudentTeacherRuntimeVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l05-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=5
p["FirstIncompleteLessonOrdinal"]=6
p["ActiveLessonOrdinal"]=6
p["NextLessonOrdinal"]=6
p["SourceVerifiedGrade12Lessons"]=5
p["ReadyForStageD"]=5
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_5_OF_73_OWNER_APPROVED_SKILL_ADAPTATIONS_READY_FOR_D"
p["UpdatedUtc"]=now
assert p["LessonEntries"][5]["Ordinal"]==6 and p["LessonEntries"][5]["ReadyForStageD"] is False
p["StageD"]="NOT_RUN"
p["CIRun"]=False
put("full-grade-progress.json",p)
print("GRADE12_U01_L05_ABC_ACADEMIC_SOURCE_SKILL_CLOSED_NEXT_U01_L06_5_OF_73")
