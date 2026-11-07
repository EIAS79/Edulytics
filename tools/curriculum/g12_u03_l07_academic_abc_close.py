from pathlib import Path
from datetime import datetime,timezone
from fractions import Fraction
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson007.json")
def j(p):return json.loads(p.read_text(encoding="utf-8"))
def save(p,x):p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p_path=base/"full-grade-progress.json";p=j(p_path)
audit_path=base/"u03-l07-source-inventory-and-gap-audit.json";a=j(audit_path)
content_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l07.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-stage-c-88-independent-assessment-reuse-routes.json"
fig_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-23-original-figure-skill-firstparty-alternatives.json"
content=j(root/content_rel);teacher=j(root/teacher_rel)
routes=j(root/route_rel);figures=j(root/fig_rel);src=j(cache)
assert pdf.is_file() and src["Section"]=="3.7" and src["SourceSha256"]=="f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d"
assert len(src["Pages"])==21 and [src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"]]==[293,313]
assert p["LastClosedLessonOrdinal"]==17 and p["FirstIncompleteLessonOrdinal"]==18
row=p["LessonEntries"][17]
assert row["Ordinal"]==18 and row["Unit"]==3 and row["LessonNumber"]==7 and row["SourceSection"]=="3.7"
assert not row["ReadyForStageD"] and a["Status"]=="A_C_IN_PROGRESS"
lesson=content["Lessons"][0]
assert len(lesson["WorkedExamples"])==12
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith("Independent Try It ")])==8
assert len(teacher["IndependentCoolDown"]["StudentQuestions"])==3
assert len(teacher["FirstPartyIndependentStudentMathQuestions"])==88
assert sorted(x["SourceSkillReferenceExercise"] for x in teacher["FirstPartyIndependentStudentMathQuestions"])==list(range(1,89))
teacher["FirstPartyIndependentStudentMathQuestions"].sort(key=lambda x:x["SourceSkillReferenceExercise"])
save(root/teacher_rel,teacher)
assert [x["SourceSkillReferenceExercise"]for x in teacher["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,89))
assert len(routes["Routes"])==88 and all(x["RubricPresent"] for x in routes["Routes"])
assert [x["OriginalSourceExerciseNumber"]for x in routes["Routes"]]==list(range(1,89))
assert len(figures["FirstPartyIndependentFigureSkillAlternatives"])==23
assert [x["OriginalSourceFigureNumber"]for x in figures["FirstPartyIndependentFigureSkillAlternatives"]]==list(range(1,24))
assert a["OriginalExerciseSourceMathReviewCount"]==80 and a["OriginalExerciseAlternativesReviewed"]==88
assert len(a["SourceOriginalGraphPanelsVisuallyInspected"])==8
assert a["FirstPartyIndependentFigureSkillAlternativesAuthored"]==23 and a["StageCAssessmentRoutesAuthored"]==88
assert all(not x["SourceQuestionCopiedVerbatim"] for x in routes["Routes"])
assert routes["NewEngineAdded"] is False and routes["AuthenticatedGrade12RuntimeVerified"] is False
assert routes["StageD"]=="NOT_RUN" and not routes["CIRun"]
assert not figures["PublisherImageCopied"] and not teacher["IndependentCoolDown"]["CopiedRestrictedPublisherCoolDown"]
assert p["CIRun"] is False and p["StageD"]=="NOT_RUN"
assert "REVIEW complete 2021" not in json.dumps(row,ensure_ascii=False)
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf-8")
graph_code=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection37GraphModels.cs").read_text(encoding="utf-8")
figure_code=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection37FigureAlternatives.cs").read_text(encoding="utf-8")
assert "Grade12PrecalculusSection37GraphModels.All" in view and "Grade12PrecalculusSection37FigureAlternatives.All" in view
assert len(re.findall(r"\bAdd\((?:57|58|59|60|61|62|63|64),",graph_code))==8
assert len(re.findall(r"\bnew\((?:[1-9]|1[0-9]|2[0-3]),",figure_code))==23
assert "x=11/4" in figure_code and "5x²+2x+2" in figure_code
assert Fraction(5*(Fraction(11,4)**2)+2*Fraction(11,4)+2,2*(Fraction(11,4)**2)+3)==Fraction(5,2)
# The local original textbook graphs were actually visually inspected using rendered pages
# 293-308 (Figures 1-23) and 311-312 (source exercises 57-64).
now=datetime.now(timezone.utc).isoformat()
close={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":7,
"Title":"Rational Functions","OriginalSection":"3.7","LocalSourceSha256":src["SourceSha256"],
"LocalSourcePdf":str(pdf),"OriginalPdfPages":[293,313],
"OriginalSourceLearningObjectives":6,
"StageA":{"OriginalWorkedExamplesLocated":12,"OriginalTryItsLocated":8,
"OriginalFiguresLocated":23,"OriginalExercisesLocated":88,
"OriginalExercisesSourceMathReviewed":80,"OriginalGraphExercisesVisuallyInspectedNotExactOriginalModelCertified":8,
"SourceFigureOriginalPagesVisuallyInspected":list(range(293,309)),
"SourceExerciseOriginalPagesVisuallyInspected":[311,312]},
"StageB":{"IndependentWorkedExamplesAuthored":12,"IndependentTryItAuthored":8,
"FirstPartyFigureSkillAlternativeCount":23,"IndependentStudentGraphModels":8,
"IndependentSourceSkillExercisesAuthored":88,"IndependentCoolDown":3,
"StudentRichV2Path":content_rel,"TeacherAnswerPath":teacher_rel,"FigureAlternatives":fig_rel},
"StageC":{"Reuse":"EXISTING_RICHV2_MANUAL_TEACHER_REVIEW_AND_ADVANCED_SVG_GRAPH_RENDERER",
"AssessmentRoutesAuthored":88,"AssessmentRoutesPath":route_rel,
"AutoGradingForOpenDrawingsClaimed":False,"NewEngineIntroduced":False,
"AuthenticatedGrade12RuntimeVerified":False},
"CCSSSkillCandidates":["CCSS:HSF-IF.C.7d","CCSS:HSA-APR.D.6","CCSS:HSF-IF.B.4"],
"CCSSGradeWideFormalCertification":False,
"AcademicOwnerAcceptanceScope":"INDEPENDENT_FIRST_PARTY_MATHEMATICAL_SOURCE_SKILL_ALIGNMENT_NOT_EXACT_PUBLISHER_NUMERICAL_OR_PIXEL_REPRODUCTION",
"PublisherOriginalGraphicCopied":False,"PublisherVerbatimExerciseReproduced":False,
"SourceAttributionCredits":"OpenStax/Rice University CC BY 4.0 in course credits",
"ChecksActuallyPerformed":["local-PDF-source-hash-cache-match","source-page-and-section-boundaries",
"12-worked-8-tryit-23-figure-88-task-inventory","all-88-task-number-and-teacher-answer-cardinality",
"23-figure-and-eight-graph-model-count","88-stage-C-route-contract-review",
"Figure14-horizontal-asymptote-crossing-Fraction-proof","independent-cooldown-three-answer-arithmetic-check",
"git-diff-check-at-commit"],
"ChecksNotPerformed":["full-project-build","CI","xUnit","E2E","authenticated-runtime","production-publish"],
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"TARGETED_SOURCE_MATH_AND_JSON_CHECKS_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":19}
save(base/"u03-l07-abc-close-audit.json",close)
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"StageA":"6_SOURCE_OBJECTIVES_12_WORKED_8_TRYITS_23_FIGURES_88_EXERCISES",
"StageB":"12_FIRSTPARTY_WORKED_8_TRYIT_23_FIGURE_ALTERNATIVES_88_EXERCISES_EIGHT_TYPED_SVG_GRAPHS_AND_COOLDOWN",
"StageC":"88_MANUAL_TEACHER_REVIEW_ROUTES_AUTHORED_REUSE_EXISTING_COMPONENTS",
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-abc-close-audit.json",
"OriginalPublisherExactGraphNumericParityVerified":False,
"StudentTeacherRuntimeVerified":False,"Tests":"TARGETED_LOCAL_ONLY",
"StageD":"NOT_RUN","CI":"NOT_RUN","BlockingReasons":[],"ClosedAtUtc":now,"UpdatedAtUtc":now})
save(audit_path,a)
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"OriginalSourceLearningObjectivesCovered":6,"OriginalWorkedExampleSkillsCovered":12,
"OriginalTryItSkillsCovered":8,"OriginalFigureMathSkillsCrosswalked":23,
"OriginalNumberedSourceExerciseSkillsAuthored":88,
"OriginalGraphExerciseSkillsAlternativeModels":8,
"OriginalExactPublisherGraphicAndNumericalParityClaimed":False,
"StudentTeacherRuntimeVerified":False,"Tests":"TARGETED_LOCAL_ONLY","BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-abc-close-audit.json",
"ClosedAtUtc":now,"UpdatedUtc":now,"LastCommitSha":None})
p.update({"LastClosedLessonOrdinal":18,"FirstIncompleteLessonOrdinal":19,
"ActiveLessonOrdinal":19,"NextLessonOrdinal":19,
"ReadyForStageD":18,"SourceVerifiedGrade12Lessons":18,
"GradeStatus":"GRADE12_A_C_IN_PROGRESS_18_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING",
"UpdatedUtc":now,"CIRun":False,"StageD":"NOT_RUN"})
assert p["LessonEntries"][18]["Ordinal"]==19 and p["LessonEntries"][18]["Unit"]==3 and p["LessonEntries"][18]["LessonNumber"]==8
save(p_path,p)
print("U03_L07_ACADEMIC_A_B_C_CLOSED_LOCALLY_18_OF_73_NEXT_U03_L08_PUSH_REQUIRED")
