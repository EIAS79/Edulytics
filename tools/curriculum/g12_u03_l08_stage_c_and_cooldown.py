from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l08-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l08-firstparty-teacher-led.json"
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l08.rich-lesson-v2.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"));rich=json.loads(rp.read_text(encoding="utf8"))
assert len(t["FirstPartyIndependentStudentMathQuestions"])==66
assert len(rich["Lessons"][0]["WorkedExamples"])==8 and len(t["FirstPartyIndependentTryItTeacherGuidance"])==5
assert a["SourceFigureConceptsCrosswalked"]==9 and a["FirstPartyInverseGraphModelsAuthored"]==4
cool=[
("Invert f(x)=(x+2)² on the original domain x≥−2.","f⁻¹(x)=−2+√x for x≥0."),
("Invert f(x)=√(x−4)+3 and give the inverse input domain.","f⁻¹(x)=(x−3)²+4 for x≥3."),
("Find the real domain of h(x)=√((x−1)/(x+2)).","(−∞,−2) ∪ [1,∞); x=−2 excluded and x=1 included.")
]
assert Fraction(1-1,1+2)==0
assert Fraction(-3-1,-3+2)==4
cooldown={"IndependentFirstPartyCoolDown":True,"StudentQuestions":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(cool,1)],
"Method":"TEACHER_REVIEW_COMPOSITION_DOMAIN_AND_SIGN_CHART","OriginalPublisherRestrictedCoolDownCopied":False,"Runtime":"NOT_RUN"}
t["IndependentCoolDown"]=cooldown
t["TeacherReviewGuidance"]="Check both inverse compositions where well-defined, exchange of domain and range, restriction of non-one-to-one polynomial branches, radical sign interval endpoints, dimensional physical units and exceptions for degenerate symbolic parameters. Graph drawings and explanations require teacher review; no automatic grading asserted."
t["StageD"]="NOT_RUN"
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
lesson=rich["Lessons"][0]
lesson["ExplanationParagraphs"]=[p.replace("Independent Cool-down draft:","Independent Edulytics Cool-down:") for p in lesson["ExplanationParagraphs"]]
lesson["SummaryPoints"].append("Cool-down reviews a restricted quadratic inverse, a radical inverse, and real-radicand sign restrictions.")
rich["ContentVersion"]="g12-u03-l08-firstparty-inverse-radical-abc-authoring-20261107"
rp.write_text(json.dumps(rich,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
families={"VERBAL":"TEACHER_REVIEW_EXPLANATION","RESTRICTED_QUADRATIC_INVERSE":"TEACHER_REVIEW_RESTRICTED_INVERSE",
"CUBIC_INVERSE":"TEACHER_REVIEW_EXACT_INVERSE","RADICAL_INVERSE":"TEACHER_REVIEW_RADICAL_DOMAIN",
"RATIONAL_INVERSE":"TEACHER_REVIEW_RATIONAL_EXCEPTION",
"COMPLETING_SQUARE_INVERSE":"TEACHER_REVIEW_RESTRICTION",
"GRAPH_INVERSE_REFLECTION":"REUSE_EXISTING_TYPED_SVG_AND_TEACHER_REVIEW",
"RADICAL_RATIONAL_DOMAIN":"TEACHER_REVIEW_SIGN_CHART",
"NUMERIC_INVERSE_POINTS":"TEACHER_REVIEW_INVERSE_ORDERED_PAIRS",
"PARAMETRIC_INVERSE":"TEACHER_REVIEW_PARAMETER_DEGENERACY",
"APPLIED_INVERSE_MODELS":"TEACHER_REVIEW_PHYSICAL_UNITS"}
routes=[]
for item in a["SourceNumberedExerciseLedger"]:
 n=item["Number"]
 assert item["OriginalMathematicalAnswerIndividuallyVerified"] and item["FirstPartyIndependentMathQuestionVerified"] and item["TeacherRubricVerified"]
 assert n==len(routes)+1
 routes.append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestionOrdinal":n,
 "TaskFamily":item["TaskFamily"],"AssessmentMode":families[item["TaskFamily"]],
 "StageCDecision":"REUSE_RICHV2_TEACHER_REVIEW_AND_EXISTING_SVG_GRAPH_RENDERER",
 "TeacherAnswerPresent":True,"OriginalPublisherExerciseVerbatimCopied":False,
 "OriginalPublisherGraphCopied":False,"AuthenticatedRuntime":"NOT_RUN"})
route={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":8,
"SourceSection":"3.8","StageC":"66_INDEPENDENT_TEACHER_REVIEW_TASKS_AUTHORED_REUSE_EXISTING_COMPONENTS",
"Routes":routes,"OriginalTeacherOnlyContentCopied":False,"NewMathEngine":False,
"RuntimeManualGradingPersistenceVerified":False,"AutoGradingClaims":False,
"CIRun":False,"StageD":"NOT_RUN"}
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-stage-c-66-independent-assessment-reuse-routes.json"
(base/"u03-l08-stage-c-66-independent-assessment-reuse-routes.json").write_text(json.dumps(route,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
assert len(routes)==66
a.update({"StageC":"66_INDEPENDENT_QUESTION_TEACHER_REVIEW_ROUTES_AUTHORED_REUSE",
"StageCAssessmentRouteEvidencePath":route_rel,"StageCAssessmentRoutesAuthored":66,
"IndependentCoolDownAuthored":True,
"CCSSCandidates":["CCSS:HSF-BF.B.4a","CCSS:HSF-BF.B.4b","CCSS:HSF-BF.B.4d","CCSS:HSF-IF.B.5"],
"CCSSMappingBasis":"Find algebraic inverses, composition domain constraints, inverse symmetry about y=x and applied inverse-model domains.",
"BlockingReasons":["FINAL_SOURCE_MATH_AND_A_B_C_EVIDENCE_GATE_PENDING"],
"Status":"A_C_IN_PROGRESS","ReadyForStageD":False,
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03_L08_STAGE_C_66_ROUTES_AND_INDEPENDENT_COOLDOWN_AUTHORED_NOT_CLOSED")
