from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l02-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==54 and a["FirstPartyIndependentExercisesAuthored"]==54
assert a["OriginalFigureSkillAlternatives"]==13 and len(t["FirstPartyIndependentStudentMathQuestions"])==54
assert not t["IndependentCoolDown"]["OriginalPublisherCoolDownCopied"]
r=[]
for source in a["OriginalNumberedExerciseLedger"]:
 assert source["OriginalSourceMathematicsVerified"] and source["IndependentStudentExerciseAuthored"] and source["TeacherExpectedMathVerified"]
 n=source["Number"]
 r.append({"OriginalSourceExerciseNumber":n,"IndependentExerciseOrdinal":n,
 "SourceMathematicalSkill":source["Family"],
 "ReuseDecision":"EXISTING_RICHV2_TEACHER_MANUAL_MATH_AND_TYPED_SVG_GRAPH_RENDERER",
 "DrawingExpected":n in list(range(8,26))+list(range(38,43)),
 "TeacherRubricReady":True,"AutoGradingClaimed":False,"AuthenticatedRuntimeVerified":False,"SourceGraphicCopied":False})
assert len(r)==54
route_path=base/"u04-l02-stage-c-54-teacher-assessment-reuse-routes.json"
route_path.write_text(json.dumps({"SchemaVersion":1,"Section":"4.2","RouteCount":54,
"ExistingComponentsReused":["RichV2 explanatory content","Teacher-led mathematical rubric",
"AdvancedMathematicsVisualRenderer and CoordinatePlaneVisualSpec"],
"NewEngineIntroduced":False,"StudentRuntimeVerified":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"FirstPartyIndependentAssessmentRoutes":r},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
t["StageCAssessmentRoutes"]=route_path.relative_to(root).as_posix()
t["TeacherMarkingPolicy"]="FREE_RESPONSE_AND_GRAPHING_REQUIRE_TEACHER_MANUAL_REVIEW"
t["ExistingGraphRendererReused"]=True
t["AutoGradingClaimed"]=False
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["StageC"]="54_TEACHER_MANUAL_MATH_AND_GRAPH_ROUTES_REUSE_EXISTING_COMPONENTS"
a["StageCAssessmentRoutes"]=route_path.relative_to(root).as_posix()
a["StageCAssessmentRouteCount"]=54
a["BlockingReasons"]=["FINAL_TARGETED_ACADEMIC_SOURCE_SKILL_AND_CLOSURE_GATE_PENDING",
"STAGE_D_RUNTIME_NOT_RUN_BY_DESIGN"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_STAGE_C_REUSE_54_TEACHER_REVIEW_ROUTES_AUTHORED")
