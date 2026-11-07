from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l05-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson005.json").read_text(encoding="utf-8"))
q=read("u03-l05-original73-polynomial-division-question-contracts.json")
f=read("u03-l05-original-three-figure-source-skill-crosswalk.json")
leaves=read("u03-l05-source73-exercise-cognitive-action-leaves.json")
trykey=read("u03-l05-three-tryit-teacher-key-and-independent-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l05.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l05-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==15 and p["FirstIncompleteLessonOrdinal"]==16
assert p["LessonEntries"][15]["Ordinal"]==16 and not p["LessonEntries"][15]["ReadyForStageD"]
assert (p["LessonEntries"][15]["PdfPageStart"],p["LessonEntries"][15]["PdfPageEnd"])==(272,280)
assert src["Section"]=="3.5" and src["SourceSha256"]==q["OriginalSourceSha256"]==f["SourceSha256"]
assert len(lesson["WorkedExamples"])==6 and len(lesson["KeyConcepts"])==9 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,4)))])==3
assert len(trykey["TeacherExpectedKeys"])==3
assert len(q["Items"])==73 and [x["Number"]for x in q["Items"]]==list(range(1,74))
assert len(a["FirstPartyIndependentStudentMathTasks"])==73 and len(a["TeacherOnlyExactPolynomialDivisionAnswers"])==73
assert len(f["Items"])==3 and len(leaves["Items"])==212
assert len({x["SourceID"]for x in leaves["Items"]})==212
assert a["OriginalPublisherDiagramBitmapCopied"] is False
assert a["LiveStudentTeacherGradebookRuntime"]=="NOT_RUN"
assert trykey["IndependentCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":5,
"AuthorizedLocalSource":"OpenStax Precalculus 2017 Section 3.5 Dividing Polynomials, zero-based PDF pages272–280",
"SourceSHA256":src["SourceSha256"],
"OriginalSourceLearningObjectives":["Use polynomial long division","Use synthetic polynomial division"],
"AcademicOwnerAcceptedScope":"INDEPENDENT_FIRSTPARTY_POLYNOMIAL_DIVISION_MATH_SKILL_AUTHORING_AND_TEACHER_MANUAL_SCORING_NOT_PUBLISHER_EXACT_GRAPH_TEXT_NUMERIC_PARITY",
"OriginalSourceWorkedExamples":6,"IndependentFirstPartyWorkedExamples":6,
"OriginalSourceTryItActivities":3,"IndependentFirstPartyTryItAlternatives":3,
"OriginalSourceFigureAnchors":3,"IndependentFirstPartyFigureMathSkillAlternatives":3,
"OriginalSourceNumberedExercises":73,"FirstPartyTeacherReviewedStudentMathQuestions":73,
"OriginalSourceExerciseNamedCognitiveActionLeaves":212,
"FirstPartyKeyConcepts":9,"FirstPartyStructuredVisualTables":2,
"IndependentFirstPartyCoolDown":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"TeacherManualMathRubricResource":ap.relative_to(root).as_posix(),
"PolynomialQuotientMathEvidence":"Exact Python rational arithmetic; 73 authored teacher keys, no application test invoked",
"OriginalPublisherArtworkCopied":False,
"OriginalPublisherVerbatimQuestionNumericalGraphicPixelIdentityCertified":False,
"SourceAtomicPublisherParagraphAndPixelDenominatorCertified":False,
"CCBY4OriginalSourceAttribution":"RETAIN_IN_CREDITS",
"CCSSCandidates":["CCSS:HSA-APR.D.6","CCSS:HSA-APR.B.2"],
"FormalCCSSAllAlignmentVerified":False,
"AuthenticatedStudentTeacherRuntimeAndPersistenceVerified":False,
"StageD":"NOT_RUN","Tests":"NOT_RUN","CI":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":17}
put("u03-l05-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"2_ORIGINAL_OBJECTIVES_6_WORKED_3_TRYITS_3_FIGURES_73_SOURCE_NUMBERED_EXERCISES",
"StageB":"6_FIRSTPARTY_WORKED_3_TRYITS_9_CONCEPTS_2_TABLE_VISUALS_COOLDOWN",
"StageC":"73_FIRSTPARTY_TEACHER_MATH_QUESTION_RUBRICS_212_ORIGINAL_SOURCE_EXERCISE_NAMED_ACTIONS",
"OriginalPublisherExactNumericalGraphicTextParityCertified":False,
"EntirePublisherAtomicProseAndGraphicDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l05-abc-close-audit.json"})
put("u03-l05-abc-checkpoint.json",cp)
row=p["LessonEntries"][15]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentFirstPartyPolynomialDivisionMathSkills":True,
"OriginalSourceLearningObjectivesCovered":2,"SourceWorkedExampleSkillsCovered":6,
"SourceTryItSkillsCovered":3,"OriginalFigureMathSkillsCovered":3,
"OriginalNumberedExerciseMathSkillsAuthored":73,
"OriginalNumberedSourceExerciseCognitiveActionLeaves":212,
"OriginalPublisherGraphicsCopied":0,
"OriginalExactPublisherGraphicVerbatimNumericParityCertified":False,
"SourceAtomicDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"LiveStudentTeacherGradebookVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l05-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=16
p["FirstIncompleteLessonOrdinal"]=17
p["ActiveLessonOrdinal"]=17
p["NextLessonOrdinal"]=17
p["SourceVerifiedGrade12Lessons"]=16
p["ReadyForStageD"]=16
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_16_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][16]["Ordinal"]==17 and p["LessonEntries"][16]["Unit"]==3 and p["LessonEntries"][16]["LessonNumber"]==6
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L05_ABC_CLOSED_16_OF_73_NEXT_U03_L06")
