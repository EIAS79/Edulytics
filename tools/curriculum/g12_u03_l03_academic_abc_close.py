from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l03-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson003.json").read_text(encoding="utf-8"))
q=read("u03-l03-original70-power-polynomial-question-contracts.json")
f=read("u03-l03-original14-polynomial-figure-source-skill-crosswalk.json")
leaves=read("u03-l03-original70-exercise-action-leaves.json")
trykey=read("u03-l03-original-nine-tryit-firstparty-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l03.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l03-firstparty-teacher-led.json"
assessment=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==13 and p["FirstIncompleteLessonOrdinal"]==14
assert p["LessonEntries"][13]["Ordinal"]==14 and not p["LessonEntries"][13]["ReadyForStageD"]
assert (p["LessonEntries"][13]["PdfPageStart"],p["LessonEntries"][13]["PdfPageEnd"])==(239,253)
assert src["Section"]=="3.3" and src["SourceSha256"]==q["SourceBookSHA256"]==f["OriginalBookSha256"]
assert len(lesson["WorkedExamples"])==12 and len(lesson["KeyConcepts"])==10 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,10)))])==9
assert len(trykey["TeacherExpectedMath"])==9
assert len(q["Items"])==70 and [x["Number"]for x in q["Items"]]==list(range(1,71))
assert len(assessment["FirstPartyIndependentStudentMathQuestions"])==70 and len(assessment["TeacherOnlyMathematicsRubrics"])==70
assert len(f["Items"])==14 and len(leaves["Items"])==193
assert len({x["SourceID"]for x in leaves["Items"]})==193
assert assessment["OriginalPublisherGraphBitmapsCopied"] is False
assert assessment["StageDStudentTeacherAppRuntime"]=="NOT_RUN"
assert trykey["FirstPartyCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":3,
"OriginalLocalPdf":"OpenStax Precalculus 2017 Section 3.3 Power Functions and Polynomial Functions, zero-based pages 239–253",
"SourceSHA256":src["SourceSha256"],
"OriginalFourLearningObjectives":["Identify power functions","Identify end behavior of power functions","Identify polynomial functions","Identify degree and leading coefficient of polynomials"],
"AcademicOwnerAcceptance":"INDEPENDENT_FIRSTPARTY_MATH_SOURCE_SKILL_AUTHORING_NO_PUBLISHER_VERBATIM_NUMERIC_GRAPHIC_IDENTITY_CLAIM",
"OriginalWorkedExampleSkills":12,"FirstPartyIndependentWorkedExamples":12,
"OriginalSourceTryIt":9,"FirstPartyIndependentTryIt":9,
"OriginalSourceFigureAnchors":14,"FirstPartyFigureMathSkillAlternatives":14,
"OriginalSourceNumberedExercises":70,"FirstPartyTeacherReviewStudentMathQuestions":70,
"OriginalNumberedSourceExerciseNamedActions":193,
"FirstPartyKeyConcepts":10,"FirstPartyStructuredVisualTables":2,
"FirstPartyCoolDown":True,
"FirstPartyRichV2":rp.relative_to(root).as_posix(),
"TeacherManualMathRubrics":ap.relative_to(root).as_posix(),
"PublisherSourceImagesCopied":False,
"PublisherOriginalVerbatimTextNumericalAndGraphicPixelParityCertified":False,
"SourceWholeBookParagraphAndPixelAtomicDenominatorCertified":False,
"CCBY4SourceCredits":"RETAIN_IN_CREDITS",
"CCSSCandidates":["CCSS:HSF-IF.C.7c","CCSS:HSF-IF.B.4","CCSS:HSA-APR.B.3"],
"FormalCCSSCompleteAlignmentVerified":False,
"LiveStudentTeacherRuntimeAndPersistenceVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":15}
put("u03-l03-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"4_SOURCE_OBJECTIVES_12_WORKED_9_TRYITS_14_FIGURES_70_NUMBERED_EXERCISE_SKILLS",
"StageB":"12_FIRSTPARTY_WORKED_9_FIRSTPARTY_TRYITS_10_CONCEPTS_2_TABLES_COOLDOWN",
"StageC":"70_FIRSTPARTY_TEACHER_MATH_QUESTIONS_193_COGNITIVE_ACTIONS",
"OriginalPublisherExactSourceGraphNumericVerbatimParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l03-abc-close-audit.json"})
put("u03-l03-abc-checkpoint.json",cp)
row=p["LessonEntries"][13]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentPolynomialMathSourceSkillAuthoring":True,
"OriginalSourceLearningObjectivesCovered":4,
"OriginalSourceWorkedExampleSkillsCovered":12,"OriginalSourceTryItSkillsCovered":9,
"OriginalFigureMathSkillAlternativesCovered":14,
"OriginalSourceNumberedExerciseSkillsAuthored":70,
"OriginalSourceNumberedExerciseCognitiveActionsAuthored":193,
"OriginalPublisherFiguresCopied":0,
"ExactOriginalPublisherVerbatimGraphNumericParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveRuntimeTeacherGradebookVerified":False,"Tests":"NOT_RUN",
"ClosedAtUtc":now,"UpdatedUtc":now,"BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l03-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=14
p["FirstIncompleteLessonOrdinal"]=15
p["ActiveLessonOrdinal"]=15
p["NextLessonOrdinal"]=15
p["SourceVerifiedGrade12Lessons"]=14
p["ReadyForStageD"]=14
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_14_OF_73_OWNER_ACCEPTED_FIRSTPARTY_SOURCE_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][14]["Ordinal"]==15 and p["LessonEntries"][14]["Unit"]==3 and p["LessonEntries"][14]["LessonNumber"]==4
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L03_A_B_C_CLOSED_14_OF_73_NEXT_U03_L04")
