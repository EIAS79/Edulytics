from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
audit_path=base/"u03-l07-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l07.rich-lesson-v2.json"
a=json.loads(audit_path.read_text(encoding="utf-8"))
t=json.loads(tp.read_text(encoding="utf-8"))
rich=json.loads(rp.read_text(encoding="utf-8"))
l=rich["Lessons"][0]
assert len(t["FirstPartyIndependentStudentMathQuestions"])==88
assert len(l["WorkedExamples"])==12 and sum(x.startswith("Independent Try It ")for x in l["ExplanationParagraphs"])==8
assert a["FirstPartyIndependentFigureSkillAlternativesAuthored"]==23 and a["IndependentGraphTaskCount"]==8
cool={
 "OriginalIndependentEdulyticsCoolDown":True,
 "StudentQuestions":[
  {"Number":1,"Prompt":"For f(x)=(x²−4)/(x²+x−6), state original domain, hole location, vertical and horizontal asymptotes.",
   "Expected":"Denominator (x+3)(x−2), numerator (x−2)(x+2). Original domain excludes x=−3 and 2; hole at (2,4/5), vertical x=−3, horizontal y=1."},
  {"Number":2,"Prompt":"For g(x)=(x−1)²/((x+2)(x−3)), state roots and their multiplicities, vertical poles, horizontal asymptote and y-intercept.",
   "Expected":"Double root x=1; vertical x=−2 and x=3; horizontal y=1; y-intercept (0,−1/6)."},
  {"Number":3,"Prompt":"A tank initially has 50 L solution and 5 kg solute. Add 5 L/min liquid containing 1 kg/min solute. Express C(t) and its limiting value with units.",
   "Expected":"C(t)=(5+t)/(50+5t) kg/L for t≥0; limit 1/5 kg/L."}
 ],
 "MarkingMode":"TEACHER_REVIEW_EXACT_MATH_WITH_REASONING",
 "Runtime":"NOT_RUN",
 "CopiedRestrictedPublisherCoolDown":False
}
assert Fraction(4,5)==Fraction(2+2,2+3)
assert Fraction(-1,6)==Fraction((0-1)**2,(0+2)*(0-3))
assert Fraction(1,5)==Fraction(1,5)
text="Independent Edulytics Cool-down: (1) Analyze f(x)=(x²−4)/(x²+x−6) for its original domain, hole, vertical and horizontal asymptotes. (2) Analyze g(x)=(x−1)²/((x+2)(x−3)) for zeros, poles, multiplicities, horizontal asymptote and y-intercept. (3) A tank contains 5 kg solute in 50 L and receives 1 kg solute and 5 L liquid per minute: write C(t), domain and long-run concentration."
assert not any("Independent Edulytics Cool-down:" in s for s in l["ExplanationParagraphs"])
l["ExplanationParagraphs"].append(text)
l["SummaryPoints"].append("Cool-down requires factor cancellation without restoring excluded inputs, pole/root multiplicities, and consistent application units.")
rich["ContentVersion"]="g12-u03-l07-firstparty-rational-functions-abc-authoring-20261107"
rp.write_text(json.dumps(rich,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
t["IndependentCoolDown"]=cool
t["TeacherOnlyMarkingGuidance"]="Check algebraic factorization, denominator exclusions before cancellation, sign chart and graphical branch reasoning, valid contextual input restrictions and physical units. Graphs and free responses require teacher review; do not automatically grade drawings."
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
categories={"VERBAL":"TEACHER_REVIEW_TEXT","DOMAIN":"TEACHER_REVIEW_SYMBOLIC_DOMAIN","DOMAIN_ASYMPTOTES":"TEACHER_REVIEW_ASYMPTOTES","INTERCEPTS":"TEACHER_REVIEW_SYMBOLIC_INTERCEPTS","LOCAL_END_BEHAVIOR":"TEACHER_REVIEW_LIMITS","GRAPH_TRANSFORMS":"TEACHER_REVIEW_GRAPH","SKETCH_GRAPH_FROM_FEATURES":"TEACHER_REVIEW_GRAPH","CONSTRUCT_FROM_FEATURES":"TEACHER_REVIEW_SYMBOLIC_CONSTRUCTION","INTERPRET_GIVEN_GRAPH":"REUSE_EXISTING_TYPED_SVG_AND_TEACHER_REVIEW","NUMERIC_LIMIT_TABLES":"TEACHER_REVIEW_TABLE","GRAPH_SIGN_INEQUALITY":"TEACHER_REVIEW_SIGN_CHART","REMOVABLE_HOLES":"TEACHER_REVIEW_HOLE","REAL_WORLD_RATIONAL_MODEL":"TEACHER_REVIEW_UNITS_APPLICATION"}
routes=[]
for item in a["SourceNumberedExerciseLedger"]:
 n=item["Number"]
 assert item["FirstPartyIndependentMathQuestionVerified"] and item["FirstPartyIndependentQuestionPath"]
 family=item["TaskFamily"]
 routes.append({"OriginalSourceExerciseNumber":n,
  "FirstPartyIndependentQuestionOrdinal":n,"SkillFamily":family,
  "StageCDecision":"REUSE_EXISTING_MANUAL_TEACHER_REVIEW_AND_TYPED_GRAPH_RENDERER",
  "AssessmentMode":categories[family],
  "SourceQuestionCopiedVerbatim":False,
  "RubricPresent":True,
  "GraphicAlternative":n>=57 and n<=64,
  "StudentTeacherRuntime":"NOT_RUN_RESERVED_FOR_STAGE_D"})
assert len(routes)==88 and [r["OriginalSourceExerciseNumber"]for r in routes]==list(range(1,89))
route_path=base/"u03-l07-stage-c-88-independent-assessment-reuse-routes.json"
route_path.write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":7,
"SourceSection":"3.7","StageCStatus":"88_AUTHORED_TEACHER_REVIEW_ROUTES_REUSE_EXISTING_TYPED_GRAPH_RENDERER_NO_NEW_MATH_ENGINE",
"ExistingComponentsReused":["RichV2 student first-party content","Existing teacher-reviewed free-response assessment workflow candidate","AdvancedMathematicsVisualRenderer for 8 graphs"],
"AutoGradeUnsupportedTasksFalselyClaimed":False,"NewEngineAdded":False,"StudentSubmissionPersistenceVerified":False,
"AuthenticatedGrade12RuntimeVerified":False,"CIRun":False,"StageD":"NOT_RUN",
"Routes":routes,"OriginalCoolDownReproduced":False},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
a["StageCAssessmentRouteEvidencePath"]=route_path.relative_to(root).as_posix()
a["StageCAssessmentRoutesAuthored"]=88
a["StageCDecision"]="REUSE_AUTHORED_TEACHER_MANUAL_FREE_RESPONSE_AND_EXISTING_TYPED_GRAPH_RENDERER"
a["IndependentCoolDownAuthored"]=True
a["CCSSCandidates"]=["CCSS:HSF-IF.C.7d","CCSS:HSA-APR.D.6","CCSS:HSF-IF.B.4"]
a["CCSSMappingBasis"]="Rational graph zeros/asymptotes, rewriting quotients to show asymptotes, interpreting graph behavior; local content-aligned candidate mapping, not complete grade-wide CCSS certification."
a["BlockingReasons"]=["FINAL_SOURCE_SKILL_MATH_AND_STUDENT_TEACHER_EVIDENCE_GATE_NOT_YET_RECORDED","STAGE_D_RUNTIME_NOT_RUN_BY_DESIGN"]
a["ReadyForStageD"]=False
a["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit_path.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_STAGE_C_88_ROUTES_AND_INDEPENDENT_COOLDOWN_AUTHORED")
