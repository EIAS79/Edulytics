from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=read("full-grade-progress.json");cp=read("u03-l04-abc-checkpoint.json")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson004.json").read_text(encoding="utf-8"))
q=read("u03-l04-original79-polynomial-graph-exercise-contracts.json")
fig=read("u03-l04-original24-polynomial-graph-figure-source-skills.json")
leaves=read("u03-l04-original79-exercise-cognitive-action-leaves.json")
trykeys=read("u03-l04-firstparty-six-tryit-teacher-key-and-cooldown.json")
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l04.rich-lesson-v2.json"
lesson=json.loads(rp.read_text(encoding="utf-8"))["Lessons"][0]
ap=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l04-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==14 and p["FirstIncompleteLessonOrdinal"]==15
assert p["LessonEntries"][14]["Ordinal"]==15 and p["LessonEntries"][14]["ReadyForStageD"] is False
assert (p["LessonEntries"][14]["PdfPageStart"],p["LessonEntries"][14]["PdfPageEnd"])==(254,271)
assert src["Section"]=="3.4" and src["SourceSha256"]==q["OriginalSourceSHA256"]==fig["OriginalSourceSHA256"]
assert len(lesson["WorkedExamples"])==11 and len(lesson["KeyConcepts"])==10 and len(lesson["Visuals"])==2
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith(tuple(f"Independent Try It {i} —"for i in range(1,7)))])==6
assert len(trykeys["TeacherExpectedMathematics"])==6
assert len(q["Items"])==79 and [x["Number"]for x in q["Items"]]==list(range(1,80))
assert len(a["FirstPartyIndependentStudentQuestions"])==79 and len(a["TeacherOnlyMathScoring"])==79
assert len(fig["Items"])==24 and len(leaves["Items"])==232
assert len({x["SourceID"]for x in leaves["Items"]})==232
assert a["PublisherOriginalGraphImagesCopied"] is False
assert a["StageDStudentTeacherRuntime"]=="NOT_RUN"
assert trykeys["IndependentCoolDown"]["OriginalPublisherRestrictedCoolDownCopied"] is False
now=datetime.now(timezone.utc).isoformat()
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":4,
"OriginalLocalPdf":"OpenStax Precalculus 2017 Section 3.4 Graphs of Polynomial Functions, zero-based PDF pages 254–271",
"OriginalBookSHA256":src["SourceSha256"],
"OriginalSevenLearningObjectives":[
"Recognize characteristics of graphs of polynomial functions",
"Factor to find zeros of polynomials","Identify zeros and multiplicities",
"Determine polynomial end behavior","Relate degree to turning points",
"Graph polynomial functions","Apply Intermediate Value Theorem"],
"AcademicOwnerAcceptedScope":"INDEPENDENT_FIRSTPARTY_SOURCE_MATH_SKILL_EQUIVALENCE_NO_PUBLISHER_ORIGINAL_NUMERIC_GRAPHIC_TEXT_IDENTITY_CLAIM",
"OriginalWorkedExamples":11,"FirstPartyIndependentWorkedExamples":11,
"OriginalTryItActivities":6,"FirstPartyIndependentTryIts":6,
"OriginalFigureAnchors":24,"FirstPartyGraphAcademicMathSkillAlternatives":24,
"OriginalNumberedExerciseParents":79,"IndependentTeacherReviewStudentMathQuestions":79,
"SourceExerciseNamedCognitiveActions":232,
"FirstPartyKeyConcepts":10,"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"StudentFirstPartyRichV2":rp.relative_to(root).as_posix(),
"FirstPartyTeacherRubricAsset":ap.relative_to(root).as_posix(),
"OriginalPublisherThirdPartyImagesCopied":False,
"ExactPublisherOriginalNumericalQuestionTextGraphPixelParityCertified":False,
"FullSourcePublisherProseAndGraphAtomicDenominatorCertified":False,
"CCBY4SourceAttribution":"RETAIN_IN_CREDITS",
"CCSSCandidates":["CCSS:HSF-IF.C.7c","CCSS:HSF-IF.B.4","CCSS:HSA-APR.B.3"],
"FormalAllCCSSAlignmentVerified":False,
"AuthenticatedLiveStudentTeacherPersistenceVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":16}
put("u03-l04-abc-close-audit.json",audit)
cp.update({
"Status":"READY_FOR_STAGE_D",
"StageA":"7_SOURCE_OBJECTIVES_11_WORKED_6_TRYITS_24_FIGURES_79_NUMBERED_QUESTION_SKILLS",
"StageB":"11_FIRSTPARTY_WORKED_6_FIRSTPARTY_TRYITS_10_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN",
"StageC":"79_FIRSTPARTY_TEACHER_MATH_REVIEW_QUESTIONS_232_SOURCE_EXERCISE_COGNITIVE_ACTIONS",
"OriginalPublisherVerbatimOriginalNumericalGraphicPixelParityCertified":False,
"SourceWholeBookAtomicElementDenominatorVerified":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now,
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l04-abc-close-audit.json"})
put("u03-l04-abc-checkpoint.json",cp)
row=p["LessonEntries"][14]
row.update({
"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"AcademicOwnerAcceptedIndependentFirstPartyPolynomialGraphMathSkills":True,
"OriginalSourceLearningObjectivesCovered":7,
"OriginalWorkedExampleSkillsCovered":11,"OriginalTryItSkillsCovered":6,
"OriginalFigureMathSkillsCovered":24,
"OriginalNumberedExerciseMathSkillsAuthored":79,
"OriginalSourceNumberedExerciseCognitiveActionLeaves":232,
"PublisherOriginalGraphicImageCopied":False,
"PublisherExactVerbatimNumberGraphPixelParityCertified":False,
"OriginalSourceAtomicElementDenominatorVerified":False,
"SemanticEquivalenceVerified":False,"VisualsVerified":False,"PracticeVerified":False,
"AuthenticatedStudentTeacherGradebookRuntimeVerified":False,
"Tests":"NOT_RUN","ClosedAtUtc":now,"UpdatedUtc":now,
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l04-abc-close-audit.json",
"LastCommitSha":None})
p["LastClosedLessonOrdinal"]=15
p["FirstIncompleteLessonOrdinal"]=16
p["ActiveLessonOrdinal"]=16
p["NextLessonOrdinal"]=16
p["SourceVerifiedGrade12Lessons"]=15
p["ReadyForStageD"]=15
p["GradeStatus"]="GRADE12_A_C_IN_PROGRESS_15_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING"
p["UpdatedUtc"]=now
assert p["LessonEntries"][15]["Ordinal"]==16 and p["LessonEntries"][15]["Unit"]==3 and p["LessonEntries"][15]["LessonNumber"]==5
p["StageD"]="NOT_RUN";p["CIRun"]=False
put("full-grade-progress.json",p)
print("G12_U03_L04_A_B_C_CLOSED_15_OF_73_NEXT_U03_L05")
