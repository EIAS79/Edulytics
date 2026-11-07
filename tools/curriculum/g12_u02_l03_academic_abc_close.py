from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,data):(base/name).write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u02-l03-abc-checkpoint.json")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson003.json").read_text(encoding="utf-8"))
q=read("u02-l03-source58-firstparty-question-contracts.json")
fig=read("u02-l03-original-eight-figure-source-skill-crosswalk.json")
leaves=read("u02-l03-original-numbered-exercise-cognitive-action-leaves.json")
trykey=read("u02-l03-firstparty-three-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u02-l03.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l03-firstparty-teacher-led.json"
assess=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==9 and p["FirstIncompleteLessonOrdinal"]==10
assert p["LessonEntries"][9]["Ordinal"]==10 and not p["LessonEntries"][9]["ReadyForStageD"]
assert (p["LessonEntries"][9]["PdfPageStart"],p["LessonEntries"][9]["PdfPageEnd"])==(177,189)
assert source["Section"]=="2.3" and source["SourceSha256"]==q["OriginalSourceSha256"]==fig["SourceSHA256"]
assert len(lesson["WorkedExamples"])==4 and len(lesson["KeyConcepts"])==8 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,4)))])==3
assert len(trykey["TeacherOnlyMathKeys"])==3
assert len(q["Items"])==58 and [x["Number"]for x in q["Items"]]==list(range(1,59))
assert len(assess["StudentFirstPartyQuestions"])==58 and len(assess["TeacherOnlyMathRubrics"])==58
assert len(fig["Items"])==8 and len(leaves["Items"])==96
assert len({x["SourceID"]for x in leaves["Items"]})==96
assert assess["OriginalPublisherGraphBitmapsCopied"] is False
assert assess["LiveStudentTeacherRuntime"]=="NOT_RUN"
assert trykey["IndependentCoolDown"]["OriginalRestrictedTeacherOnlyPublisherCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"Lesson":3,
"AuthorizedLocalSource":"OpenStax Precalculus (2017), Section 2.3, zero-based PDF pp.177–189",
"OriginalSourceSha256":source["SourceSha256"],
"OriginalLearningObjectives":[
"Identify steps for modeling and solving",
"Build linear models from verbal descriptions",
"Build systems of linear models"],
"FirstPartyPedagogicAcademicOwnerAcceptance":"INDEPENDENT_SOURCE_SKILL_MATH_AUTHORING_WITH_MANUAL_TEACHER_REVIEW_NOT_PUBLISHER_GRAPH_OR_EXACT_NUMERIC_INSTANCE_PARITY",
"OriginalSourceWorkedExamples":4,"FirstPartyIndependentWorkedExamples":4,
"OriginalTryItActivities":3,"FirstPartyTryItAlternatives":3,
"OriginalFigureAnchors":8,"FirstPartyMathFigureSkillAlternatives":8,
"OriginalNumberedExerciseAnchors":58,"FirstPartyNumberedStudentMathSkillTasks":58,
"OriginalNumberedExerciseNamedCognitiveActions":96,
"FirstPartyKeyConcepts":8,"FirstPartyStructuredTables":2,
"FirstPartyIndependentCoolDownAuthored":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"FirstPartyTeacherManualRubricPath":ap.relative_to(root).as_posix(),
"OriginalSourcePublisherAndThirdPartyImagesCopied":False,
"PublisherExactOriginalSourceVerbatimGraphicNumericalParityCertified":False,
"FullPublisherParagraphAndPixelAtomicDenominatorCertified":False,
"CCBY4AttributionRequiredInCredits":True,
"CCSSCandidates":["CCSS:HSA-CED.A.2","CCSS:HSA-REI.C.6"],
"FormalFullCCSSAlignmentVerified":False,
"StudentTeacherAuthenticatedRuntimeAndPersistenceVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":11}
put("u02-l03-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"3_SOURCE_OBJECTIVES_4_WORKED_EXAMPLE_SKILLS_3_TRYITS_8_FIGURE_SKILLS_58_NUMBERED_EXERCISES",
"StageB":"4_INDEPENDENT_WORKED_3_FIRSTPARTY_TRYITS_8_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN",
"StageC":"58_INDEPENDENT_FIRSTPARTY_TEACHER_REVIEW_SOURCE_SKILL_QUESTIONS_96_COGNITIVE_ACTIONS",
"OriginalPublisherVerbatimOrGraphPixelParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l03-abc-close-audit.json"})
put("u02-l03-abc-checkpoint.json",cp)
row=p["LessonEntries"][9]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentMathSourceSkillAuthoring":True,
"SourceOriginalLearningObjectivesCovered":3,
"SourceWorkedExampleSkillsCovered":4,"SourceTryItSkillsCovered":3,
"SourceFigureMathSkillAlternativesCovered":8,
"OriginalSourceNumberedExerciseSkillsAuthored":58,
"OriginalSourceExerciseCognitiveActionsAuthored":96,
"OriginalPublisherImagesCopied":0,
"ExactOriginalPublisherTextNumberAndGraphicParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveStudentTeacherRuntimeGradebookVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l03-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=10
p["FirstIncompleteLessonOrdinal"]=11
p["ActiveLessonOrdinal"]=11
p["NextLessonOrdinal"]=11
p["SourceVerifiedGrade12Lessons"]=10
p["ReadyForStageD"]=10
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_10_OF_73_OWNER_ACCEPTED_INDEPENDENT_MATH_SKILLS"
p["UpdatedUtc"]=now
assert p["LessonEntries"][10]["Ordinal"]==11 and p["LessonEntries"][10]["LessonNumber"]==4 and p["LessonEntries"][10]["Unit"]==2
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U02_L03_A_B_C_CLOSED_10_OF_73_NEXT_U02_L04")
