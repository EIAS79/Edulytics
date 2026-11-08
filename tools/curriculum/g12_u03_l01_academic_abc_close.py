from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l01-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson001.json").read_text(encoding="utf-8"))
questions=read("u03-l01-source58-independent-complex-question-contracts.json")
fig=read("u03-l01-original-three-figure-complex-plane-skill-crosswalk.json")
leaves=read("u03-l01-source-numbered-exercise-action-leaves.json")
trykeys=read("u03-l01-firstparty-seven-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l01.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l01-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==11 and p["FirstIncompleteLessonOrdinal"]==12
assert p["LessonEntries"][11]["Ordinal"]==12 and not p["LessonEntries"][11]["ReadyForStageD"]
assert (p["LessonEntries"][11]["PdfPageStart"],p["LessonEntries"][11]["PdfPageEnd"])==(213,222)
assert src["Section"]=="3.1" and src["SourceSha256"]==questions["SourceSHA256"]==fig["SourceSHA256"]
assert len(lesson["WorkedExamples"])==10 and len(lesson["KeyConcepts"])==9 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,8)))])==7
assert len(trykeys["TeacherExpectedAnswers"])==7
assert len(questions["Items"])==58 and [x["Number"]for x in questions["Items"]]==list(range(1,59))
assert len(assessment["FirstPartyStudentQuestions"])==58 and len(assessment["TeacherOnlyExpectedMath"])==58
assert len(fig["Items"])==3 and len(leaves["Items"])==118
assert len({x["SourceID"]for x in leaves["Items"]})==118
assert assessment["OriginalPublisherBitmapOrImageReproduced"] is False
assert assessment["StudentTeacherRuntime"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":1,
"OriginalSource":"OpenStax Precalculus (2017), Section 3.1 Complex Numbers, local PDF 213–222 zero-based",
"OriginalSourceSHA256":src["SourceSha256"],
"OriginalLearningObjectives":[
"Express square roots of negative numbers as multiples of i",
"Plot complex numbers on the complex plane",
"Add and subtract complex numbers",
"Multiply and divide complex numbers"],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_SOURCE_MATH_SKILL_FIRSTPARTY_AUTHORING_NOT_EXACT_PUBLISHER_SOURCE_TEXT_AND_DIAGRAM_PARITY",
"SourceWorkedExamples":10,"FirstPartyWorkedExamples":10,
"SourceTryItSkills":7,"FirstPartyIndependentTryIts":7,
"OriginalFigureAnchors":3,"FirstPartyIndependentComplexPlaneMathSkillAlternatives":3,
"SourceNumberedExercises":58,"FirstPartyManualTeacherReviewedMathQuestions":58,
"NumberedSourceCognitiveExerciseActionLeaves":118,
"FirstPartyKeyConcepts":9,"FirstPartyStructuredVisualTables":2,
"IndependentCoolDownAuthored":True,
"RichV2":rp.relative_to(root).as_posix(),
"TeacherAssessment":ap.relative_to(root).as_posix(),
"OriginalPublisherOrThirdPartyGraphicsCopied":False,
"PublisherOriginalVerbatimTextNumberAndGraphicPixelParityCertified":False,
"FullOriginalPublisherProseAndImageAtomicDenominatorCertified":False,
"CCBY4OriginalSourceAttribution":"RETAIN_IN_CREDITS",
"CCSSCandidates":["CCSS:HSN-CN.A.1","CCSS:HSN-CN.A.2","CCSS:HSN-CN.A.3"],
"FormalCompleteCCSSAlignmentVerified":False,
"RuntimeStudentTeacherGradebookVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":13}
put("u03-l01-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"4_ORIGINAL_OBJECTIVES_10_WORKED_EXAMPLES_7_TRYITS_3_FIGURES_58_NUMBERED_EXERCISE_SKILLS",
"StageB":"10_INDEPENDENT_WORKED_7_INDEPENDENT_TRYIT_9_CONCEPTS_2_TABLE_VISUALS_AND_COOLDOWN",
"StageC":"58_FIRSTPARTY_TEACHER_COMPLEX_MATH_SKILL_RUBRICS_118_COGNITIVE_ACTIONS",
"SourceAtomicPublisherImagePixelProseDenominatorVerified":False,
"PublisherExactOriginalNumericGraphicsAndVerbatimParityCertified":False,
"StageD":"NOT_RUN","Tests":"NOT_RUN","CI":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l01-abc-close-audit.json"})
put("u03-l01-abc-checkpoint.json",cp)
row=p["LessonEntries"][11]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentFirstPartySourceMathSkillAuthoring":True,
"SourceLearningObjectivesCovered":4,"SourceWorkedExampleSkillsCovered":10,
"SourceTryItSkillsCovered":7,"SourceFigureSkillsCrosswalked":3,
"SourceNumberedExerciseSkillsAuthored":58,
"SourceNumberedExerciseNamedActionLeavesAuthored":118,
"OriginalPublisherFiguresCopied":0,
"PublisherOriginalNumericGraphicVerbatimParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveRuntimeTeacherGradebookVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l01-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=12
p["FirstIncompleteLessonOrdinal"]=13
p["ActiveLessonOrdinal"]=13
p["NextLessonOrdinal"]=13
p["SourceVerifiedGrade12Lessons"]=12
p["ReadyForStageD"]=12
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_12_OF_73_OWNER_ACCEPTED_FIRSTPARTY_SOURCE_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][12]["Ordinal"]==13 and p["LessonEntries"][12]["Unit"]==3 and p["LessonEntries"][12]["LessonNumber"]==2
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L01_ABC_CLOSED_12_OF_73_NEXT_U03_L02")
