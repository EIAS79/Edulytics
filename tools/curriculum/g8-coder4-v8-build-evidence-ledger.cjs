'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const source=read('docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json');
const prior=read('docs/curriculum/us-ccss-g8-coder4-v7-lesson-gates-20261029.json');
const actual=read('docs/curriculum/us-ccss-g8-coder4-v8-publisher-activity-structure-20261029.json');
const rubrics=read('docs/curriculum/us-ccss-g8-coder4-teacher-rubric-candidates-20261029.json');
const cross=read('docs/curriculum/us-ccss-g8-coder4-practice-crosswalk-20261029.json');
const byPrior=new Map(prior.rows.map(x=>[x.lessonCode,x]));
const bySource=new Map(source.rows.map(x=>[x.lessonCode,x]));
const byActivities=new Map(actual.rows.map(x=>[x.lessonCode,x]));
const byPractice=new Map(cross.rows.map(x=>[x.lessonCode,x]));
const byTeacher=new Map(rubrics.lessons.map(x=>[x.lessonCode,x]));
const rows=prior.rows.map(old=>{
 const id=old.lessonCode,a=byActivities.get(id),p=byPractice.get(id),s=bySource.get(id),rubric=byTeacher.get(id);
 if(!a||!p||!s||!rubric)throw Error('Evidence lost '+id);
 const reasons=[
 'A_SOURCE_ACTIVITY_SEMANTICS_NOT_INDEPENDENTLY_REVIEWED',
 'B_AUTHENTICATED_LEARNER_RENDERING_NOT_OBSERVED',
 'C_LESSON_EXACT_GENERATOR_AND_GRADE_NOT_VERIFIED',
 'C_ATTEMPT_PERSISTENCE_NEGATIVE_CASES_NOT_OBSERVED',
 'D_AUTHENTICATED_STUDENT_TEACHER_TWO_SCHOOL_NOT_VERIFIED',
 'D_ACCESSIBLE_REPRESENTATION_NOT_REVIEWED',
 'TEACHER_RUBRIC_DRAFT_NOT_RUNTIME_CONNECTED',
 'EXTERNAL_OWNER_UAT_AND_PROTECTED_SHA_CI_NOT_COMPLETE'
 ];
 if(a.publisherFigureInspectionNeeded)reasons.push('A_PUBLISHER_STUDENT_PAGE_CONTAINS_IMAGE_TAGS_BUT_RICH_V2_VISUALS_EMPTY');
 if(s.blueprintExtraRoles.length||s.blueprintMissingRoles.length)reasons.push('A_STANDARDS_ROLE_PROVENANCE_DIFF_REQUIRES_EDITION_JUDGMENT');
 if(old.evidence.mathematicalPracticeOnlyOutcomes)reasons.push('C_MP_ONLY_OUTCOME_WITHOUT_LESSON_SPECIFIC_HUMAN_ASSESSMENT_MAPPING');
 if(old.evidence.multipleMappedPracticeFamilies)reasons.push('C_MULTI_FAMILY_EXACT_QUESTION_SELECTION_NOT_VERIFIED');
 if(old.evidence.semanticPracticeMismatchCandidate)reasons.push('C_KNOWN_TWO_VARIABLE_VS_ONE_VARIABLE_MAPPING_MISMATCH');
 return {lessonCode:id,sourceLessonCode:s.sourceLessonCode,sourceLessonTitle:s.title,unit:s.unit,
 sourceEvidence:{teacherPreparationUrl:s.url,teacherLessonUrl:a.teacherUrl,studentLessonUrl:a.studentUrl,
  teacherLessonHttp200:a.teacher.httpStatus===200,studentLessonHttp200:a.student.httpStatus===200,
  teacherHtmlSha256:a.teacher.sha256,studentHtmlSha256:a.student.sha256,
  studentActivityTitles:a.student.activities,studentActivityCount:a.student.activities.length,
  studentPageImageTagCount:a.student.sourceImageTags,visualSourceInspectionPriority:a.publisherFigureInspectionNeeded,
  standardsRoleExtraInBlueprint:s.blueprintExtraRoles,standardsRoleMissingInBlueprint:s.blueprintMissingRoles,
  fullSourceStudentActivityReviewedByHuman:false},
 candidate:{richV2Asset:old.evidence.richV2Candidate,richV2WorkedExampleCount:a.richV2WorkedExamples,
  richV2VisualCount:a.richV2VisualCount,draftTeacherRubric:true},
 practice:{outcomeTargets:p.officialOutcomeTargets,explicitSkill:p.explicitLessonSkill,
  mathematicalPracticeOnlyOutcomes:old.evidence.mathematicalPracticeOnlyOutcomes,
  multiMappedFamilies:old.evidence.multipleMappedPracticeFamilies,
  knownSemanticMappingMismatch:old.evidence.semanticPracticeMismatchCandidate,
  generatedLessonSpecificPracticeVerified:false,scoringOracleVerifiedForLesson:false,
  wrongAnswerAndMalformedVerifiedForLesson:false,attemptPersistenceVerified:false,
  wrongLessonRequestRejectedVerified:false},
 limitedEngineSmoke:a.lessonCode==='PED:US-CCSS-MATH:G8:U04:L05'?
 'Eight generated ax+b=c questions: Verify accepted exact answers and rejected malformed/obviously wrong answers in .NET test. Does NOT certify all lesson targets or authenticated flow.':null,
 verdict:'BLOCKED_WITH_CAUSE',
 stageA:'NOT_ACCEPTED',stageB:'NOT_ACCEPTED',stageC:'NOT_VERIFIED',stageD:'NOT_VERIFIED',
 nextChecks:reasons};
});
const counts={total:rows.length,aAccepted:0,bAccepted:0,cVerified:0,dVerified:0,
 sourceTeacherFetched:rows.filter(x=>x.sourceEvidence.teacherLessonHttp200).length,
 sourceStudentFetched:rows.filter(x=>x.sourceEvidence.studentLessonHttp200).length,
 sourceActivityHeadings:rows.reduce((sum,r)=>sum+r.sourceEvidence.studentActivityCount,0),
 originalFiguresNeedingReview:rows.filter(x=>x.sourceEvidence.visualSourceInspectionPriority).length,
 standardsRoleDifferences:rows.filter(x=>x.sourceEvidence.standardsRoleExtraInBlueprint.length||x.sourceEvidence.standardsRoleMissingInBlueprint.length).length,
 explicitPracticeSemanticMismatch:rows.filter(x=>x.practice.knownSemanticMappingMismatch).length,
 lessonScopedPracticeFullyVerified:0,teacherAssessmentRuntimeVerified:0};
if(counts.total!==131||counts.sourceStudentFetched!==131||counts.sourceTeacherFetched!==131||new Set(rows.map(x=>x.lessonCode)).size!==131)throw Error('V8 ledger failed integrity check');
const report={schemaVersion:1,course:'US-CCSS-MATH:G8:OUR-IM-2017',purpose:'Reproducible per-lesson Stage A-D source and runtime evidence inventory. BLOCKED_WITH_CAUSE means release evidence missing, not necessarily that previously authored math is wrong.',
 builtAgainst:'Original Grade 8 branch with Supervisor 2 RichV2 catalogue patch',counts,
 limitations:'Fetching public teacher/student activity pages and parsing headings/figure tags is not a human inspection of activity fidelity or original figures. RichV2 exact registry tests do not prove student sees the material. No live tenant, persisted grading, teacher scoring, or academic acceptance is inferred. External production remains untouched.',
 rows};
const out='docs/curriculum/us-ccss-g8-coder4-v8-acceptance-ledger-20261029.json';
fs.writeFileSync(path.join(root,out),JSON.stringify(report,null,2)+'\n');
console.log('V8_LEDGER '+JSON.stringify({path:out,...counts}));
