from pathlib import Path
import json,datetime,math
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l01-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==68 and a["OriginalFigureSkillAlternatives"]==6
assert len(t["FirstPartyIndependentStudentMathQuestions"])==68
routing=[]
for item in a["OriginalNumberedExerciseLedger"]:
 n=item["Number"]
 assert item["SourceMathematicsIndividuallyReviewed"] and item["FirstPartyIndependentExerciseAuthored"] and item["TeacherAnswerVerified"]
 kind="TEACHER_REVIEW_GRAPH_AND_MODEL" if n in range(18,28) or n in range(51,56) else "TEACHER_REVIEW_EXPONENTIAL_FORMULA_AND_UNITS"
 routing.append({"OriginalSourceNumber":n,"SkillFamily":item["Family"],"IndependentQuestionOrdinal":n,
 "StageCDecision":"REUSE_EXISTING_RICHV2_MANUAL_TEACHER_REVIEW_AND_TYPED_GRAPH_RENDERER",
 "AssessmentMode":kind,"AutoGradeClaimed":False,"StudentTeacherRuntimeTested":False,
 "TeacherRubricPresent":True,"PublisherOriginalTextReproduced":False})
assert [r["OriginalSourceNumber"]for r in routing]==list(range(1,69))
route=base/"u04-l01-stage-c-68-exponential-assessment-reuse-routes.json"
route.write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":1,"Section":"4.1",
"StageC":"REUSE_RICHV2_TEACHER_MANUAL_MARKING_AND_SVG_RENDERER",
"ExistingComponentsReused":["RichV2 academic content","Teacher manual assessment and rubrics","Typed graph SVG renderer"],
"NewMathEngine":False,"StudentSubmissionPersistenceVerified":False,
"RuntimeAuthenticatedStudentTeacherVerified":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"IndependentExerciseRoutes":routing},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
t["StageCDecision"]="REUSE_EXISTING_TEACHER_MANUAL_ASSESSMENT_AND_TYPED_GRAPH_RENDERER"
t["StageCAssessmentRoutePath"]=route.relative_to(root).as_posix()
t["OriginalSourceFigureSkillAlternatives"]=6
t["GraphTeacherReviewMode"]="DRAWING_REQUIRED_TEACHER_MANUAL_REVIEW"
t["OriginalPublisherFiguresCopied"]=False
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["StageC"]="68_MANUAL_TEACHER_MATH_AND_GRAPH_REVIEW_ROUTES_REUSE_EXISTING_COMPONENTS"
a["StageCRoutesEvidence"]=route.relative_to(root).as_posix()
a["StageCRoutesCount"]=68
a["BlockingReasons"]=["FINAL_TARGETED_SOURCE_AND_MATH_EVIDENCE_GATE_PENDING","STAGE_D_RUNTIME_NOT_RUN_BY_DESIGN"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_STAGE_C_68_ROUTES_AUTHORED_REUSE_NO_NEW_ENGINE_RUNTIME_NOT_RUN")
