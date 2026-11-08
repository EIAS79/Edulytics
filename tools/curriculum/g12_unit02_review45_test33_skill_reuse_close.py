from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
progress=json.loads((base/"full-grade-progress.json").read_text(encoding="utf-8"))
assert progress["LastClosedLessonOrdinal"]==11 and progress["FirstIncompleteLessonOrdinal"]==12
assert all(progress["LessonEntries"][i]["ReadyForStageD"]for i in range(7,11))
review=[
(1,7,"linear classification"),(1,9,"nonlinear classification"),
(1,15,"slope sign increasing"),(1,17,"slope sign decreasing"),
(1,32,"two-point equation"),(1,36,"intercept-derived equation"),
(1,25,"graph slope"),(1,26,"graph slope"),
(1,41,"line from graph"),(1,47,"table linear test"),(1,52,"table nonlinearity"),(1,74,"applied change from initial amount"),
(2,8,"parallel comparison"),(2,10,"perpendicular slope"),(2,16,"both intercepts"),
(2,12,"intercept from rule"),(2,18,"slope pair relation"),(2,19,"slope pair relation"),
(2,26,"perpendicular line through point"),(2,40,"sketch line from intercept"),
(2,44,"draw function graph"),(2,67,"two-line intersection"),(2,74,"break-even plan choice"),
(3,7,"coordinate triangle area"),(3,11,"growth model"),(3,25,"declining count model"),
(3,31,"derive contextual graph model"),(3,32,"contextual y-intercept"),
(3,45,"multi-part population reasoning"),(3,49,"population forecast"),
(3,49,"population from year"),(3,42,"comparative house value trend"),(3,43,"linear extrapolation"),
(4,8,"scatter linearity"),(4,12,"interpolation versus extrapolation"),
(4,18,"visual best-fit"),(4,19,"visual best-fit"),
(4,22,"fit time series"),(4,23,"regression threshold"),
(4,25,"regression and correlation"),(4,26,"regression and correlation"),
(4,37,"regress year as dependent variable"),(4,38,"predict year from linear model"),
(4,25,"Pearson r of fitted regression model"),(4,38,"predict population from an inverted year model")
]
test=[
(1,7,"linear classification"),(1,17,"decreasing slope sign"),
(1,15,"increasing slope sign"),(1,32,"line from two points"),
(1,36,"intercept equation"),(1,25,"line slope from graph"),
(1,41,"equation from plotted coordinates"),(1,47,"linear table"),
(1,52,"nonlinear table"),(1,74,"constant-rate model"),
(2,11,"parallel/perpendicular equations"),(2,9,"parallel/perpendicular equations"),
(2,16,"line intercepts"),(2,18,"two-point slope relation"),
(2,27,"perpendicular through prescribed point"),(2,40,"graph from slope/intercept"),
(2,44,"sketch line"),(2,67,"intersection system"),(2,74,"two-plan cost inequality"),
(3,7,"geometric triangle model"),(3,11,"linear population forecast"),(3,25,"declining-case count trend"),
(3,31,"read contextual graph"),(3,32,"interpret contextual intercept"),
(3,45,"multi-part linear population application"),
(4,8,"scatter linearity"),(4,18,"fit a line to scatter"),
(4,22,"regression time-series"),(4,23,"regression threshold"),
(4,25,"regression and correlation"),
(4,37,"regression with year output"),(4,38,"model extrapolation"),
(4,31,"regression correlation numerical")
]
assert len(review)==45 and len(test)==33
counts={1:76,2:76,3:58,4:42}
for n,(lesson,exercise,skill)in enumerate(review+test,1):
 assert 1<=exercise<=counts[lesson]
 assert skill
assets=[]
for kind,arr,source_pages in [
("SOURCE_CHAPTER_REVIEW",review,[205,208]),
("SOURCE_CHAPTER_PRACTICE_TEST",test,[209,211])
]:
 for ordinal,(lesson,task,skill)in enumerate(arr,1):
  assets.append({
   "OriginalSourceAssessmentType":kind,
   "SourceOriginalExerciseNumber":ordinal,
   "OriginalSourcePdfPages":source_pages,
   "OriginalSourceCognitiveMathSkill":skill,
   "IndependentFirstPartyExistingLesson":f"U02:L{lesson:02d}",
   "FirstPartyExistingLessonTeacherReviewedQuestionNumber":task,
   "OriginalPublisherExerciseVerbatimNumericInstanceCopied":False,
   "OriginalPublisherGraphPixelsCopied":False,
   "SemanticMatchScope":"OWNER_APPROVED_EQUIVALENT_LEARNING_SKILL_ONLY_NOT_EXACT_ORIGINAL_PRINTED_QUESTION",
   "StudentTeacherRuntime":"NOT_RUN"})
assert len(assets)==78 and len({(x["OriginalSourceAssessmentType"],x["SourceOriginalExerciseNumber"])for x in assets})==78
resource=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-chapter-review-and-practice-test-firstparty-reuse.json"
resource.parent.mkdir(parents=True,exist_ok=True)
assert not resource.exists()
resource.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,
"AuthorizedSource":"OpenStax Precalculus 2017 Chapter 2 local PDF",
"SourceSha256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
"SourceChapterKeyTermsAndConceptReviewPDFPages":[202,204],
"SourceChapterReviewNumberedExercisePDFPages":[205,208],
"SourceChapterPracticeTestPDFPages":[209,211],
"SourceChapterReviewNumberedItems":45,
"SourceChapterPracticeTestNumberedItems":33,
"IndependentExistingLessonSourceMathSkillPracticeRoutes":78,
"FullExactPublisherChapterAssessmentNumericalVerbatimOrGraphImageParityClaim":False,
"OriginalPublisherThirdPartyImagesCopied":0,
"StudentUse":"REUSE_SOURCE_MATH_SKILL_FIRSTPARTY_TEACHER_MANUAL_QUESTION_CONTRACTS",
"StageDStudentTeacherRuntime":"NOT_RUN","CI":"NOT_RUN",
"ReusedSkillAssessmentItems":assets
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
audit={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":2,"UnitTitle":"Linear Functions",
"OriginalAuthorizedBook":"OpenStax Precalculus 2017, Chapter 2, local original PDF",
"OriginalSourceSha256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
"SourceBookUnit2IntroductionPDFPage":140,
"OriginalUnit2FourNumberedSectionPdfRanges":[[141,157],[158,176],[177,189],[190,201]],
"OriginalUnit2SourceIndexOffByOneError":"CORRECTED_IN_PRIOR_COMMIT_AFTER_CHECKING_LOCAL_SECTION_HEADINGS",
"UnitLessonClosureOrder":["U02:L01","U02:L02","U02:L03","U02:L04"],
"FourOfFourUnitLessonsOwnerApprovedAcademicSourceSkillA_B_C":True,
"FourUnitLessonsReadyForStageD":True,
"OriginalFourSectionNumberedExerciseSkillAnchors":76+76+58+42,
"SourceChapterReviewNumberedExercises":45,
"SourceChapterPracticeTestNumberedExercises":33,
"FirstPartyExistingLessonChapterAssessmentPedagogicSkillReuseRoutes":78,
"FirstPartyStudentTeacherManualLessonQuestionsAuthored":76+76+58+42,
"SourceChapterReviewAndPracticeTestReuseAsset":resource.relative_to(root).as_posix(),
"ReuseScopeDisclaimer":"Pedagogic skill equivalence, not original publisher's same numerical assessment or exact figure, no Runtime/Stage D tests",
"OriginalPublisherAndThirdPartyImagesCopied":0,
"OriginalPublisherGraphicPixelAndVerbatimIdentityCertified":False,
"CC_BY_4_OriginalSourceCreditsMustBeRetained":True,
"AuthenticatedStudentAndTeacherGradebookRuntime":"NOT_VERIFIED",
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN",
"AcademicUnitA_B_C_ClosureScope":"SOURCE_SKILL_FIRSTPARTY_AUTHORING_READY_FOR_D_NOT_PIXEL_MATCHED_BOOK_AND_NOT_PRODUCTION_CERTIFIED",
"NextSequentialLesson":"U03:L01",
"RecordedAtUtc":datetime.now(timezone.utc).isoformat()
}
(base/"u02-four-lesson-source-skill-academic-unit-close-audit.json").write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U02_4_OF_4_LESSONS_CLOSED_45_SOURCE_CHAPTER_REVIEW_33_TEST_SKILL_REUSE_ROUTES_FIRSTPARTY_MAPPED")
