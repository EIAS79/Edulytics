from pathlib import Path
from datetime import datetime,timezone
from fractions import Fraction
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson008.json")
def rd(p):return json.loads(p.read_text(encoding="utf8"))
def wr(p,o):p.write_text(json.dumps(o,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
src=rd(cache)
p_path=base/"full-grade-progress.json";p=rd(p_path)
ap=base/"u03-l08-source-inventory-and-gap-audit.json";a=rd(ap)
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l08.rich-lesson-v2.json"
teach_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l08-firstparty-teacher-led.json"
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-stage-c-66-independent-assessment-reuse-routes.json"
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-nine-figure-firstparty-skill-alternatives.json"
rich=rd(root/rich_rel);teacher=rd(root/teach_rel);routes=rd(root/route_rel);fig=rd(root/figure_rel)
assert pdf.is_file() and src["Section"]=="3.8" and src["SourceSha256"]==p["SourceSnapshotSha256"]
assert [src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"]]==[314,324]
assert p["LastClosedLessonOrdinal"]==18 and p["FirstIncompleteLessonOrdinal"]==19
row=p["LessonEntries"][18]
assert row["Ordinal"]==19 and row["SourceSection"]=="3.8" and not row["ReadyForStageD"]
lesson=rich["Lessons"][0]
assert len(lesson["WorkedExamples"])==8
assert len([x for x in lesson["ExplanationParagraphs"]if x.startswith("Independent Try It ")])==5
assert len(teacher["FirstPartyIndependentTryItTeacherGuidance"])==5
assert [x["SourceSkillReferenceExercise"]for x in teacher["FirstPartyIndependentStudentMathQuestions"]]==list(range(1,67))
assert len(routes["Routes"])==66 and all(x["TeacherAnswerPresent"] for x in routes["Routes"])
assert len(fig["IndependentFigureConcepts"])==9
assert len(teacher["IndependentCoolDown"]["StudentQuestions"])==3
assert a["OriginalSourceMathExercisesIndividuallyVerified"]==66 and a["FirstPartyIndependentSourceExerciseMathAuthored"]==66
assert a["FirstPartyIndependentFigureSkillAlternativesAuthored"]==9 and a["FirstPartyInverseGraphModelsAuthored"]==4
assert routes["NewMathEngine"] is False and routes["AutoGradingClaims"] is False
assert routes["StageD"]=="NOT_RUN" and routes["CIRun"] is False and p["CIRun"] is False
assert not fig["OriginalPublisherGraphicBitmapsCopied"]
view=(root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml").read_text(encoding="utf8")
graphs=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection38InverseGraphs.cs").read_text(encoding="utf8")
figcode=(root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection38FigureAlternatives.cs").read_text(encoding="utf8")
assert "Grade12PrecalculusSection38InverseGraphs.All" in view and "Grade12PrecalculusSection38FigureAlternatives.All" in view
assert len(re.findall(r"\bAdd\((?:35|36|32|40),",graphs))==4
assert len(re.findall(r"\bnew\([1-9],",figcode))==9
assert Fraction(-3-1,-3+2)==4
assert Fraction(1-1,1+2)==0
assert "REVIEW complete 2021" not in json.dumps(row,ensure_ascii=False)
now=datetime.now(timezone.utc).isoformat()
close={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":8,"Section":"3.8",
"Title":"Inverses and Radical Functions","SourceSha256":src["SourceSha256"],
"LocalSourcePdf":str(pdf),"OriginalPdfPages":[314,324],
"OriginalSourceObjectives":2,"OriginalWorkedExamples":8,"OriginalTryItActivities":5,
"OriginalFigureSkills":9,"OriginalNumberedExercises":66,
"StageA":"TWO_ORIGINAL_OBJECTIVES_EIGHT_EXAMPLES_FIVE_TRYITS_NINE_FIGURES_66_ORIGINAL_SOURCE_EXERCISE_MATH_INTERPRETED",
"StageB":{"FirstPartyWorkedExamples":8,"FirstPartyTryItActivities":5,"FirstPartyFigureAlternatives":9,
"IndependentStudentExercisesWithTeacherKeys":66,"IndependentInverseGraphModels":4,"IndependentCoolDownQuestions":3,
"RichV2":rich_rel,"TeacherAssessment":teach_rel,"FigureSkillAsset":figure_rel},
"StageC":{"Decision":"REUSE_EXISTING_RICHV2_TYPED_GRAPH_SVG_AND_MANUAL_TEACHER_REVIEW",
"IndependentAssessmentRoutes":66,"RouteEvidence":route_rel,
"NewMathEngineCreated":False,"RuntimeVerified":False},
"DisclosedOriginalBookAmbiguities":teacher["SourceOriginalAmbiguitiesDisclosed"],
"AcademicOwnerAcceptanceScope":"INDEPENDENT_MATH_SKILL_ALIGNED_FIRSTPARTY_AUTHORING_NOT_PUBLISHER_FIGURE_PIXEL_PARITY",
"PublisherImagesCopied":False,"CCBY4Attribution":"RETAIN_SOURCE_IN_COURSE_CREDITS",
"CCSSCandidates":a["CCSSCandidates"],"CCSSGradeWideCertification":False,
"ChecksPerformed":["local-source-section-and-page-boundaries","source-hash-matches-approved-pdf-cache",
"8_5_9_66_cardinality","66_ordered_standalone_teacher_math_keys","four_independent_graph_model_source_presence",
"nine_figure_skill_student_view_wiring","66_stage_c_manual_review_route_coverage",
"independent_cooldown_three_math_answers","no_original_source_graph_copy","git_diff_cached_check"],
"ChecksNotPerformed":["full_build","CI","xUnit","E2E","authenticated_student_runtime","publication"],
"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_MATH_JSON_AND_TEXT_STRUCTURAL_CHECKS_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now,"NextSequentialLessonOrdinal":20}
wr(base/"u03-l08-abc-close-audit.json",close)
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"StageA":"2_OBJECTIVES_8_EXAMPLES_5_TRYITS_9_FIGURES_66_EXERCISE_SOURCE_MATH",
"StageB":"8_FIRSTPARTY_EXAMPLES_5_TRYITS_9_FIGURE_SKILL_ALTERNATIVES_66_EXERCISES_4_GRAPHS_COOLDOWN",
"StageC":"66_MANUAL_TEACHER_REVIEW_ASSESSMENT_ROUTES_REUSE_NO_NEW_ENGINE",
"BlockingReasons":[],"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-abc-close-audit.json",
"StudentTeacherRuntimeVerified":False,"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ClosedAtUtc":now,"UpdatedUtc":now})
wr(ap,a)
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"OriginalSourceLearningObjectivesCovered":2,"OriginalWorkedExampleSkillsCovered":8,
"OriginalTryItSkillsCovered":5,"OriginalFigureSkillsCrosswalked":9,
"OriginalNumberedSourceExerciseMathReviewed":66,
"OriginalNumberedSourceExerciseFirstPartyQuestions":66,"FirstPartyIndependentInverseGraphModels":4,
"OriginalPublisherExactGraphicNumericParityClaimed":False,"StudentTeacherRuntimeVerified":False,
"Tests":"TARGETED_LOCAL_ONLY","BlockingReasons":[],
"ClosureEvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-abc-close-audit.json",
"ClosedAtUtc":now,"UpdatedUtc":now,"LastCommitSha":None})
p.update({"LastClosedLessonOrdinal":19,"FirstIncompleteLessonOrdinal":20,
"ActiveLessonOrdinal":20,"NextLessonOrdinal":20,"ReadyForStageD":19,
"SourceVerifiedGrade12Lessons":19,
"GradeStatus":"GRADE12_A_C_IN_PROGRESS_19_OF_73_OWNER_ACCEPTED_INDEPENDENT_SOURCE_MATH_SKILL_AUTHORING",
"UpdatedUtc":now,"CIRun":False,"StageD":"NOT_RUN"})
assert p["LessonEntries"][19]["Unit"]==3 and p["LessonEntries"][19]["LessonNumber"]==9
wr(p_path,p)
print("U03_L08_ACADEMIC_ABC_CLOSED_LOCALLY_19_OF_73_NEXT_U03_L09_PUSH_REQUIRED")
