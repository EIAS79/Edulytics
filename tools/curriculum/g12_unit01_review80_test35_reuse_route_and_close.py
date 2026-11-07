from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=json.loads((base/"full-grade-progress.json").read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==7 and p["FirstIncompleteLessonOrdinal"]==8
assert all(p["LessonEntries"][i]["ReadyForStageD"]for i in range(7))
# This is a chapter-level reuse map. Existing independent first-party
# lesson question assets provide practice on each source exercise's SKILL;
# it is not a claim of same numeric instance or publisher graph pixels.
routing={
1:[6,10,40,44,27,28,56,57,40,41,46,76,77,52,53,54,66,32,33],
2:[14,16,22,38],
3:[28,29,32,18,19,20,21,22,23,24,25,24],
4:[12,13,14,15,16,20,21,22,25,26,41],
5:[27,28,29,30,43,44,45,46,18,55,33,34,47,48,49,33,40,45],
6:[37,43,46,37,41,48,10,22,31,32],
7:[7,11,13,17,19,24,33,34]
}
assert [len(routing[i])for i in range(1,8)]==[19,4,12,11,18,10,8]
assert sum(len(x)for x in routing.values())==82
# Original review 1..80 ranges by actual chapter subheadings:
# 1..19,20..23,24..33=10,34..44=11,45..62=18,63..72=10,73..80=8
ranges=[(1,19,1),(20,23,2),(24,33,3),(34,44,4),(45,62,5),(63,72,6),(73,80,7)]
assert sum(end-start+1 for start,end,l in ranges)==80
review=[]
for start,end,lesson in ranges:
 choices=routing[lesson]
 for n in range(start,end+1):
  local_idx=n-start
  ref=choices[local_idx%len(choices)]
  page=(133 if n<=13 else 134 if n<=31 else 135 if n<=54 else 136 if n<=68 else 137)
  review.append({
   "OriginalChapterReviewExerciseNumber":n,
   "OriginalSourcePdfPage":page,
   "OriginalTopicLesson":f"U01:L{lesson:02d}",
   "FirstPartyAcademicSkillAlternativeLesson":f"U01:L{lesson:02d}",
   "FirstPartyIndependentLessonPracticeTaskNumber":ref,
   "AcceptanceBasis":"REUSED_FIRSTPARTY_EQUIVALENT_LEARNING_SKILL_NOT_EXACT_SOURCE_NUMERICS_OR_ORIGINAL_DIAGRAM",
   "OriginalSourceQuestionWordingCopied":False,
   "OriginalPublisherGraphicReused":False,
   "RuntimeTeacherGradebook":"NOT_RUN"})
assert len(review)==80 and [x["OriginalChapterReviewExerciseNumber"]for x in review]==list(range(1,81))
test_ranges=[
(1,5,1,[6,7,27,28,56]),(6,8,2,[8,38,40]),(9,9,3,[28]),
(10,12,4,[12,22,26]),(13,17,5,[27,28,47,49,50]),
(18,20,6,[43,10,31]),(21,22,7,[7,11]),
(23,26,3,[18,19,22,23]),(27,29,2,[38,39,43]),
(30,35,7,[37,38,19,21,39,8])
]
test=[]
for start,end,lesson,items in test_ranges:
 assert end-start+1==len(items)
 for n,ref in zip(range(start,end+1),items):
  test.append({
   "OriginalChapterPracticeTestExerciseNumber":n,
   "OriginalSourcePdfPage":138 if n<=22 else 139,
   "OriginalTopicLesson":f"U01:L{lesson:02d}",
   "FirstPartyAcademicSkillAlternativeLesson":f"U01:L{lesson:02d}",
   "FirstPartyIndependentLessonPracticeTaskNumber":ref,
   "AcceptanceBasis":"REUSED_FIRSTPARTY_SOURCE_PEDAGOGIC_SKILL_NOT_1_TO_1_PRINTED_GRAPH_OR_NUMERICAL_ITEM",
   "OriginalPublisherImageCopied":False,
   "SourceExactVerbatimQuestionCopied":False,
   "StageDRuntime":"NOT_RUN"})
assert len(test)==35 and [x["OriginalChapterPracticeTestExerciseNumber"]for x in test]==list(range(1,36))
student={
"SchemaVersion":1,
"Course":"US-CCSS-MATH:G12","Unit":1,
"OriginalBook":"OpenStax Precalculus (2017), Chapter 1",
"OriginalBookSourceSHA256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
"OriginalChapterReviewKeyTermsPdfPages":[128,129],
"OriginalChapterReviewKeyConceptsPdfPages":[130,132],
"OriginalChapterReviewNumberedExercisesPdfPages":[133,137],
"OriginalChapterPracticeTestPdfPages":[138,139],
"OriginalChapterReviewSourceExercises":80,
"OriginalChapterPracticeTestSourceExercises":35,
"FirstPartyExistingLessonSourceSkillPracticeReuseReferences":115,
"OriginalSourceExactMathOrGraphicsOneToOneCertified":False,
"OriginalOriginalPublisherMediaEmbedded":False,
"SourceReviewNumericInstanceParity":False,
"FirstPartyStudentChapterReviewSourceSkillRefs":review,
"FirstPartyStudentChapterPracticeTestSourceSkillRefs":test,
"FirstPartyUnitCoolDown":{
 "Prompt":"(a) Explain whether {(1,2),(2,2)} defines a function and whether it is one-to-one. (b) Find domain of √(x−4)/(x−7). (c) Compute average rate of f(x)=x² on [1,3]. (d) Form f(g(x)) for f(x)=x² and g(x)=x+2. (e) State vertex of |x−3|−2. (f) Solve |x−3|≤2. (g) Find inverse of h(x)=3x−1.",
 "TeacherOnlyExpectedMath":"(a) function, not one-to-one; (b) [4,7)∪(7,∞); (c) 4; (d) (x+2)²; (e) (3,−2); (f) [1,5]; (g) h⁻¹(x)=(x+1)/3.",
 "TeacherReview":"MANUAL_SEVEN_SKILL_SAMPLING_ONLY_NOT_REPLACEMENT_FOR_ALL_ORIGINAL_REVIEW_PROBLEMS"
 },
"ChapterA_B_C":"OWNER_ACCEPTED_FIRSTPARTY_EXISTING_LESSON_REUSE_FOR_SOURCE_PEDAGOGIC_SKILLS",
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"NOT_RUN"}
path=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-chapter-review-reuse-manifest.json"
path.parent.mkdir(parents=True,exist_ok=True)
assert not path.exists()
path.write_text(json.dumps(student,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
audit={
"SchemaVersion":1,
"Unit":1,"UnitName":"Functions",
"AcademicSource":"Local OpenStax Precalculus 2017, Chapter 1, PDF pp.18–139",
"LessonScope":[f"U01:L{i:02d}"for i in range(1,8)],
"SequentialLessonsAcademicOwnerApprovedSourceSkillA_B_C":7,
"All7LessonReadyForStageD":True,
"OriginalSourceSectionNumberedExercises":sum([92,61,47,97,81,64,47]),
"OriginalReviewExercises":80,
"OriginalPracticeTestExercises":35,
"OriginalReviewAndTestSkillAlternativeRoutes":115,
"ReviewRoutesAreExactOneToOneTextAndMathProblems":False,
"AcademicOwnerApprovedSkillOnly":True,
"StudentChapterReviewReuseResource":path.relative_to(root).as_posix(),
"PublisherOriginalGraphicsAndThirdPartyPhotosCopied":False,
"RequiredCC_BY_4_AttributionRetained":True,
"ProductionStudentRuntimeAndSQLGradebookVerified":False,
"StageD":"NOT_RUN",
"CI":"NOT_RUN",
"Unit1A_B_C_SourceSkillAuthoringClosure":"READY_FOR_STAGE_D_WITH_FIRSTPARTY_SKILL_REUSE_NOT_ORIGINAL_SOURCE_PIXEL_PARITY",
"Unit1ReassessmentNotes":"Source review and test are navigational practice reuse maps, not exact 80+35 publisher problem translations.",
"NextUnit":"U02:L01",
"VerifiedAtUtc":datetime.now(timezone.utc).isoformat()
}
(base/"u01-7lesson-academic-source-skill-unit-close-audit.json").write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_ALL_7_LESSONS_CLOSED_CHAPTER_REVIEW_80_AND_PRACTICE_TEST_35_FIRSTPARTY_SKILL_REUSE_ROUTES_AUTHORED")
