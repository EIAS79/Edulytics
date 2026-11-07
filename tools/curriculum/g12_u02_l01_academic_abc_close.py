from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,data):(base/name).write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson001.json").read_text(encoding="utf-8"))
p=read("full-grade-progress.json");cp=read("u02-l01-abc-checkpoint.json")
qs=read("u02-l01-original76-firstparty-question-contracts.json")
fig=read("u02-l01-original12-figure-source-skill-crosswalk.json")
leaf=read("u02-l01-original-exercise-action-leaves.json")
teacher=read("u02-l01-tryit-six-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l01.rich-lesson-v2.json"
rich=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l01-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==7 and p["FirstIncompleteLessonOrdinal"]==8
assert p["LessonEntries"][7]["Ordinal"]==8 and not p["LessonEntries"][7]["ReadyForStageD"]
assert source["Section"]=="2.1" and source["Pages"][0]["PdfPage"]==141 and source["Pages"][-1]["PdfPage"]==157
assert source["SourceSha256"]==qs["SourceSha256"]==fig["SourceSha256"]
assert (p["LessonEntries"][7]["PdfPageStart"],p["LessonEntries"][7]["PdfPageEnd"])==(141,157)
assert len(rich["WorkedExamples"])==12 and len(rich["KeyConcepts"])==8 and len(rich["Visuals"])==2
assert len([x for x in rich["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,7)))])==6
assert len(teacher["TeacherKeys"])==6
assert len(qs["Items"])==76 and [x["Number"]for x in qs["Items"]]==list(range(1,77))
assert len(a["StudentQuestions"])==76 and len(a["TeacherOnlyMathRubrics"])==76
assert len(fig["Items"])==12 and len(leaf["Items"])==164
assert len({x["SourceID"]for x in leaf["Items"]})==164
assert "−250" in a["TeacherOnlyMathRubrics"][57]["Expected"]
assert a["AuthenticatedStudentTeacherRuntime"]=="NOT_RUN"
assert a["OriginalPublisherGraphBitmapCopied"] is False
assert teacher["CoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":1,
"OriginalLocalPdf":"OpenStax Precalculus 2017, 2.1 Linear Functions, zero-based PDF pages 141–157",
"SourceSHA256":source["SourceSha256"],
"LocalOriginalFivePedagogicObjectives":[
"Represent a linear function",
"Determine whether linear function increases, decreases or is constant",
"Calculate and interpret the slope of a linear function",
"Write point-slope form of a line",
"Write and interpret a linear function"],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRST_PARTY_PRACTICE_COVERING_PEDAGOGIC_MATH_SKILLS_NO_EXACT_PUBLISHER_GRAPH_OR_TEXT_CLAIM",
"OriginalNumberedSourceExercises":76,
"OriginalNumberedSourceExerciseActionLeaves":164,
"FirstPartyManuallyTeacherScoredIndependentQuestions":76,
"OriginalSourceWorkedExampleSkills":12,"FirstPartyWorkedExamples":12,
"OriginalTryIts":6,"FirstPartyTryItAlternatives":6,
"OriginalFigureAnchors":12,"FirstPartyFigureMathSkillAlternatives":12,
"FirstPartyKeyConcepts":8,"FirstPartyStructuredTables":2,
"FirstPartyIndependentCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"FirstPartyTeacherAssessmentPath":ap.relative_to(root).as_posix(),
"OriginalPublisherAndThirdPartyVisualAssetsCopied":False,
"ExactPublisherNumericalAndOriginalDiagramSemanticParityCertified":False,
"EntireBookParagraphAndPixelAtomicDenominatorCertified":False,
"SourcePdfBoundaryCorrection":"Unit2 original inventory was recorded +1 page and corrected by locally checking all section headings",
"CCBY4_SourceAttribution":"RETAIN_IN_CREDITS",
"CandidateCCSS":["CCSS:HSF-IF.A.1","CCSS:HSF-IF.C.7","CCSS:HSA-CED.A.2"],
"CCSSFormalFullAlignmentVerified":False,
"StageDStudentRuntimeAndTeacherGradebookVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextLessonOrdinal":9}
put("u02-l01-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"5_SOURCE_OBJECTIVES_12_WORKED_EXAMPLE_SKILLS_6_TRYITS_12_FIGURES_76_NUMBERED_QUESTION_SKILLS",
"StageB":"12_FIRSTPARTY_WORKED_6_FIRSTPARTY_TRYITS_8_CONCEPTS_2_TABLE_VISUALS_COOLDOWN",
"StageC":"76_FIRSTPARTY_MANUAL_TEACHER_REVIEW_SKILL_QUESTIONS_164_SOURCE_EXERCISE_ACTION_LEAVES",
"LocalPdfSectionHeadingsVerified":True,
"FullExactPublisherOriginalGraphicAndTextParity":False,
"OriginalSourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l01-abc-close-audit.json"})
put("u02-l01-abc-checkpoint.json",cp)
row=p["LessonEntries"][7]
row.update({
"ACStatus":"READY_FOR_STAGE_D",
"ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentSourceSkillMathematics":True,
"OriginalSourceLearningObjectivesCovered":5,
"OriginalSourceWorkedExampleSkillsCovered":12,
"OriginalSourceTryItSkillsCovered":6,
"OriginalSourceFigureSkillAlternativesCovered":12,
"OriginalSourceNumberedExerciseSkillQuestionsAuthored":76,
"OriginalSourceNumberedExerciseActionsAuthored":164,
"OriginalPublisherThirdPartyArtCopied":0,
"SourceFullPublisherOriginalGraphicPixelAndVerbatimParityClaim":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"StageDStudentTeacherRuntimeVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l01-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=8
p["FirstIncompleteLessonOrdinal"]=9
p["ActiveLessonOrdinal"]=9
p["NextLessonOrdinal"]=9
p["SourceVerifiedGrade12Lessons"]=8
p["ReadyForStageD"]=8
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_8_OF_73_OWNER_ACCEPTED_FIRSTPARTY_SOURCE_SKILL_MATH"
p["UpdatedUtc"]=now
assert p["LessonEntries"][8]["Ordinal"]==9 and p["LessonEntries"][8]["Unit"]==2 and p["LessonEntries"][8]["LessonNumber"]==2
assert p["LessonEntries"][8]["ReadyForStageD"] is False
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("GRADE12_U02_L01_ABC_FIRSTPARTY_SKILL_SCOPE_CLOSED_8_OF_73_NEXT_U02_L02")
