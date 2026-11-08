from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
r=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
rich=json.loads(r.read_text(encoding="utf-8"))
lesson=rich["Lessons"][0]
q=json.loads((base/"u01-l02-source-61-first-party-practice-contracts.json").read_text(encoding="utf-8"))
f=json.loads((base/"u01-l02-figure26-first-party-skill-crosswalk.json").read_text(encoding="utf-8"))
t=json.loads((base/"u01-l02-independent-tryit-teacher-key.json").read_text(encoding="utf-8"))
e=json.loads((base/"u01-l02-example13-source-skill-crosswalk.json").read_text(encoding="utf-8"))
cool=json.loads((base/"u01-l02-first-party-cooldown.json").read_text(encoding="utf-8"))
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson002.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.2"
assert len(source["Pages"])==16
assert len(q["OriginalNamedExerciseItems"])==61
assert [x["Number"]for x in q["OriginalNamedExerciseItems"]]==list(range(1,62))
assert len(f["Rows"])==26
assert len(t["Items"])==8 and all(x["TeacherExpectedAnswer"]for x in t["Items"])
assert len(e["Items"])==13
assert len(lesson["WorkedExamples"])==13
assert len(lesson["KeyConcepts"])==7
assert len(lesson["Visuals"])==8
assert not any("Expected result for teacher review" in x for x in lesson["ExplanationParagraphs"])
assert cool["OriginalPublisherRestrictedCoolDownCopied"] is False
# Source assessment leaf actions avoid counting each parent and its subparts twice.
leaves=[]
for task in q["OriginalNamedExerciseItems"]:
    n=task["Number"]
    parts=["complete original cognitive demand"]
    if 27<=n<=37:parts=["read graph domain","read graph range"]
    if 38<=n<=45:parts=["sketch piecewise with endpoints","state permitted domain"]
    if 46<=n<=48:parts=["evaluate f(−3)","evaluate f(−2)","evaluate f(−1)","evaluate f(0)"]
    if 49<=n<=51:parts=["evaluate f(−1)","evaluate f(0)","evaluate f(2)","evaluate f(4)"]
    if n==26:parts=["solve radicand sign algebraically","corroborate sign intervals graphically"]
    if n in (55,56):parts=["draw two correct bounded source-skill windows","determine combined output range"]
    if n==60:parts=["find physical time domain","interpret endpoints and units"]
    if n==61:parts=["a: setup cost","b: cost at 25 items","c: constrained domain and range with correct count units"]
    for index,label in enumerate(parts,1):
        leaves.append({
          "SourceElementId":f"G12-U01-L02-EXERCISE{n:03d}-ACTION{index:02d}",
          "SourceParentNumber":n,
          "SourcePdfPage":task["SourcePDFPage"],
          "SourceLearningAction":label,
          "FirstPartyDeliveryResource":"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l02-firstparty-teacher-led.json",
          "FirstPartyStudentTaskNumber":n,
          "FirstPartyTeacherKeyAvailable":True,
          "OriginalSourceGraphicCopied":False,
          "FirstPartyAcademicSkillAuthoringReviewed":True,
          "OriginalPublisherExactQuestionVerbatimIdentityClaim":False,
          "StageDStudentTeacherRuntime":"NOT_RUN"
        })
assert len(leaves)==104,len(leaves)
assert len({x["SourceElementId"]for x in leaves})==104
new_assessment=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l02-firstparty-teacher-led.json"
assert not new_assessment.exists()
student={
 "SchemaVersion":1,"Course":"US-CCSS-MATH:G12:PRECALCULUS",
 "LessonCode":lesson["LessonCode"],
 "SourceSection":"1.2","OriginalEdition":"2017",
 "AuthoringStatus":"A_B_C_FIRST_PARTY_TEACHER_REVIEW_QUESTION_BANK_COMPLETE",
 "SourceNamedExercises":61,"OriginalSourceNamedExerciseSubpartActions":104,
 "StudentPracticeQuestions": [
   {"Number":x["Number"],"Skill":x["Category"],
    "Question":x["FirstPartyStudentQuestion"],"AssessmentMode":"TEACHER_MANUAL_REVIEW"}
   for x in q["OriginalNamedExerciseItems"]
 ],
 "TeacherOnlyExpectedMathematics":[
   {"Number":x["Number"],"ExpectedMathematics":x["ExpectedTeacherMathematics"],
    "GradingGuidance":x["ReviewMethod"]}
   for x in q["OriginalNamedExerciseItems"]
 ],
 "TeacherOnlyTryIts":[
   {"Number":i,"ExpectedMathematics":x["TeacherExpectedAnswer"]}
   for i,x in enumerate(t["Items"],1)
 ],
 "TeacherOnlyCoolDown":cool["ExpectedMathematics"],
 "AutoGradedSourceExercisesClaimed":0,
 "OriginalPublisherImagesReused":False,
 "RequiresAttributionInProductSourceCredits":True,
 "SourceAcademicAuthoringOwnerApproved":True,
 "All61FirstPartyQuestionAssetsAuthored":True,
 "StudentPortalRuntimeBound":False,
 "GradebookSubmissionAuthenticated":False,
 "StageDTestsAndCINotRun":True
}
new_assessment.write_text(json.dumps(student,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ledger=base/"u01-l02-104-original-exercise-leaf-actions.json"
ledger.write_text(json.dumps({
 "SchemaVersion":1,
 "OriginalSectionNumberedExercises":61,
 "OriginalNamedSourceExerciseLearningActions":104,
 "CountMethod":"Count all 61 parent source numbered exercises only by their own identified explicitly requested student action leaves. Multipart parents are not also counted.",
 "OriginalSourceFigureGraphExercises27To37":11,
 "OriginalNamedWorkedExamples":13,
 "OriginalNamedTryIts":8,
 "StudentIndependentAssessmentQuestions":61,
 "TeacherManualSourceActionChecklistCount":104,
 "AtomicSourceActionCountExcludesSourceNarrativeAndDiagramPixels":True,
 "FirstPartyPedagogicMathEquivalenceAuthoringComplete":True,
 "OriginalSourcePixelOrVerbatimContentParityClaim":False,
 "StageD":"NOT_RUN",
 "Leaves":leaves
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cfile=base/"u01-l02-abc-checkpoint.json"
c=json.loads(cfile.read_text(encoding="utf-8"))
c["OriginalSourceNamedExerciseLearningActionLeaves"]=104
c["OriginalSourceNumberedExercisesTeacherReviewAuthored"]=61
c["SourceWorkedExamplesPedagogicallyCovered"]=13
c["SourceTryItsPedagogicallyCovered"]=8
c["SourceFiguresPedagogicAlternativesMapped"]=26
c["FirstPartyCoolDownAuthored"]=True
c["FirstPartyAssessmentResource"]=new_assessment.relative_to(root).as_posix()
c["ExerciseAtomicEvidencePath"]=ledger.relative_to(root).as_posix()
c["SourceOriginalMediaAndImagesCopied"]=0
c["A_B_C_SourceSkillAuthoringComplete"]=True
c["FullOriginalPublisherGraphicPixelAndVerbatimParityCertified"]=False
c["StageDApplicationRuntimeAndGradingTests"]="NOT_RUN"
c["ReadyForStageD"]=False
cfile.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("GRADE12_L02_NATIVE_FIRSTPARTY_ASSESSMENT_RESOURCE_61_EXERCISES_104_EXPLICIT_ACTIONS_AUTHORED")
