from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l02-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson002.json").read_text(encoding="utf-8"))
q=read("u03-l02-original94-firstparty-quadratic-exercise-contracts.json")
f=read("u03-l02-original16-figure-quadratic-skills-crosswalk.json")
leaves=read("u03-l02-original94-exercise-cognitive-action-leaves.json")
trykeys=read("u03-l02-five-tryit-teacher-key-and-independent-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l02.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l02-firstparty-teacher-led.json"
assess=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==12 and p["FirstIncompleteLessonOrdinal"]==13
assert p["LessonEntries"][12]["Ordinal"]==13 and p["LessonEntries"][12]["ReadyForStageD"] is False
assert (p["LessonEntries"][12]["PdfPageStart"],p["LessonEntries"][12]["PdfPageEnd"])==(223,238)
assert src["Section"]=="3.2" and src["SourceSha256"]==q["SourceBookSHA256"]==f["OriginalBookSha256"]
assert len(lesson["WorkedExamples"])==10 and len(lesson["KeyConcepts"])==10 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,6)))])==5
assert len(trykeys["TeacherOnlyExpectedMath"])==5
assert len(q["Items"])==94 and [x["Number"]for x in q["Items"]]==list(range(1,95))
assert len(assess["FirstPartyIndependentStudentQuestions"])==94 and len(assess["TeacherOnlyMathematicalExpectedResults"])==94
assert len(f["Items"])==16 and len(leaves["Items"])==261
assert len({x["SourceID"]for x in leaves["Items"]})==261
assert assess["SourcePublisherDiagramImageCopied"] is False
assert assess["LiveTeacherGradebookStudentRuntime"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalRestrictedPublisherTeacherCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":2,
"OriginalSource":"OpenStax Precalculus (2017) Section 3.2 Quadratic Functions, local PDF zero-based pp223–238",
"SourceSHA256":src["SourceSha256"],
"OriginalLearningSkills":["Interpret quadratic graph vertex, symmetry and intercepts","Complete square and find vertex","Find quadratic domain, range, maximum and minimum","Solve real and complex quadratic equations","Optimize a quadratic in realistic contexts"],
"AcademicOwnerAcceptedScope":"FIRSTPARTY_EQUIVALENT_MATH_LEARNING_SKILLS_WITH_INDEPENDENT_EXERCISES_NOT_SOURCE_VERBATIM_EXACT_GRAPHIC_PARITY",
"SourceWorkedExamples":10,"FirstPartyWorkedExamples":10,
"OriginalSourceTryIts":5,"FirstPartyIndependentTryIts":5,
"OriginalSourceFigures":16,"FirstPartyQuadraticFigureMathSkillAlternatives":16,
"OriginalSourceNumberedExercises":94,"FirstPartyTeacherReviewedMathQuestions":94,
"OriginalNumberedSourceExerciseNamedCognitiveActions":261,
"FirstPartyKeyConcepts":10,"FirstPartyVisualTables":2,
"FirstPartyIndependentCoolDown":True,
"RichV2Path":rp.relative_to(root).as_posix(),
"TeacherQuestionRubrics":ap.relative_to(root).as_posix(),
"OriginalPublisherAndThirdPartyImagesCopied":False,
"ExactPublisherSourceVerbatimNumericAndGraphPixelParityCertified":False,
"FullBookParagraphAndPixelAtomicDenominatorVerified":False,
"CC_BY_4_SourceCreditsRequired":True,
"CCSSCandidates":["CCSS:HSF-IF.C.7a","CCSS:HSF-IF.B.4","CCSS:HSA-REI.B.4"],
"FormalCCSSMappingVerified":False,
"ProductionStudentTeacherRuntimeAndGradebookVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":14}
put("u03-l02-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"QUADRATIC_10_WORKED_SKILLS_5_TRYIT_16_SOURCE_FIGURES_94_NUMBERED_EXERCISE_SKILLS",
"StageB":"10_FIRSTPARTY_WORKED_5_FIRSTPARTY_TRYIT_10_CONCEPTS_2_VISUAL_TABLES_COOLDOWN",
"StageC":"94_FIRSTPARTY_MANUAL_TEACHER_REVIEW_QUESTIONS_261_SOURCE_COGNITIVE_ACTIONS",
"OriginalPublisherExactOriginalGraphicAndTextParityVerified":False,
"EntireSourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l02-abc-close-audit.json"})
put("u03-l02-abc-checkpoint.json",cp)
row=p["LessonEntries"][12]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentQuadraticSourceSkillAuthoring":True,
"OriginalSourceWorkedExampleSkillsCovered":10,
"OriginalSourceTryItMathSkillsCovered":5,
"OriginalFigurePedagogicSkillsMapped":16,
"OriginalNumberedExercisesSourceSkillsAuthored":94,
"OriginalNumberedExerciseCognitiveActionLeaves":261,
"PublisherGraphicOriginalsCopied":0,
"OriginalPublisherVerbatimNumberAndImagePixelParityClaim":False,
"OriginalSourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"StudentTeacherRuntimeVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l02-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=13
p["FirstIncompleteLessonOrdinal"]=14
p["ActiveLessonOrdinal"]=14
p["NextLessonOrdinal"]=14
p["SourceVerifiedGrade12Lessons"]=13
p["ReadyForStageD"]=13
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_13_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][13]["Ordinal"]==14 and p["LessonEntries"][13]["Unit"]==3 and p["LessonEntries"][13]["LessonNumber"]==3
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L02_ABC_CLOSED_13_OF_73_NEXT_U03_L03")
