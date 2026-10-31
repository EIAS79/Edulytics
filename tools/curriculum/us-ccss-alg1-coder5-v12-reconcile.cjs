#!/usr/bin/env node
'use strict';
const fs=require('fs'),path=require('path'),root=path.resolve(__dirname,'..','..'),docs=path.join(root,'docs/curriculum');
const read=name=>JSON.parse(fs.readFileSync(path.join(docs,name),'utf8'));
const progress=read('US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json');
const ledger=read('US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json');
const fetched=read('US_CCSS_ALG1_CODER5_V12_SOURCE_FETCH_AUDIT_20261029.json');
const first=read('US_CCSS_ALG1_CODER5_V12_FIRST_SEVEN_SOURCE_TASK_CORRECTIONS_20261029.json');
const second=read('US_CCSS_ALG1_CODER5_V12_SECOND_SEVEN_SOURCE_TASK_CORRECTIONS_20261029.json');
const v11=read('US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json');
const map=new Map(fetched.rows.map(x=>[x.lessonCode,x]));
const r12=new Map([...first.lessons,...second.lessons].map(x=>[x.lessonCode,x]));
const r11=new Map(v11.teacherRubricCandidates.map(x=>[x.lessonCode,x]));
const unitPlaceholders={
 1:['numeric data representation','distribution statistic and graph'],
 2:['linear equations and constraints','contextual model with units'],
 3:['categorical frequency and association','table interpretation'],
 4:['function notation and graph','domain and input-output'],
 5:['exponential growth or decay','multiplicative comparison'],
 6:['quadratic graph and expression','geometric area'],
 7:['quadratic equation solving','contextual root interpretation']
};
const svg={
'U01:L04':'u01-l04-distribution-dotplots.svg',
'U04:L01':'u04-l01-function-and-nonfunction.svg',
'U05:L01':'u05-l01-additive-vs-multiplicative.svg',
'U06:L01':'u06-l01-garden-area.svg',
'U07:L01':'u07-l01-original-frame.svg'
};
const handoff=[],teacher=[];
for(const p of progress.lessons){
 const code=p.lessonCode,s=map.get(code),c=r12.get(code),prior=r11.get(code);
 const l=ledger.lessons.find(x=>x.lessonCode===code);
 if(!l)throw Error('Ledger missing '+code);
 const sourceStatus=c?'DIRECT_SOURCE_TASK_REPAIR_NOT_ACCEPTED':
   prior?'V11_SOURCE_SPOT_CHECK_NOT_ACCEPTED':
   s?'TWO_PAGES_RETRIEVED_AUTOMATICALLY_NOT_PEDAGOGICALLY_REVIEWED':
   'SOURCE_REFERENCED_NEEDS_ACTUAL_ACTIVITY_COMPARISON';
 const family=c?.requiredPracticeFamilies||c?.requiredExactPractice||unitPlaceholders[p.unitNumber];
 const criteria=c?.proposedTeacherRubric?.criteria||c?.draftHumanRubric?.criteria||prior?.criteria||null;
 const side=JSON.parse(fs.readFileSync(path.join(root,p.authoredRichV2Sidecar),'utf8')).Lessons[0];
 const prose=side.ExplanationParagraphs.filter(x=>/rubric/i.test(x));
 if(prose.length===0)throw Error('Missing authored rubric narrative '+code);
 const suffix=code.match(/U\d\d:L\d\d$/)?.[0];
 l.v12={
  sourceBothPagesHttp200:!!s,sourceActivityHeadingCount:s?.studentActivityHeadingCandidates.length||0,
  activityReviewStatus:sourceStatus,firstPartyContentRevisedV12:!!c,
  structuredTeacherRubricDraft:!!criteria,staticSvgRelativePath:svg[suffix]||null,
  staticSvgAuthenticatedRendered:false,
  stageAAccepted:false,stageBAccepted:false,stageCVerified:false,stageDVerified:false,
  teacherGradePersisted:false
 };
 handoff.push({
  lessonCode:code,title:p.title,unitNumber:p.unitNumber,
  teacherUrl:p.officialTeacherPreparationUrl,
  studentUrl:p.officialTeacherPreparationUrl.replace('/teachers/','/students/').replace('/preparation.html','/index.html'),
  sourceActivityTitles:s?.studentActivityHeadingCandidates||[],
  sourceEvidence:sourceStatus,
  proposedAssessmentFamilies:family,
  familiesEvidenceLevel:c?'FIRST_PARTY_SOURCE_COMPARISON_DRAFT':prior?'V11_SOURCE_SPOT_CHECK':'UNIT_PLACEHOLDER_REQUIRES_LESSON_REVIEW',
  exactPracticeApproved:false,scoringVerified:false,attemptPersisted:false,
  needsSourceSpecificPracticeWork:true,requiresSupervisor2Integration:true,
  existingSafetyGate:'ALG1 FAIL CLOSED'
 });
 teacher.push({
  lessonCode:code,title:p.title,status:criteria?'STRUCTURED_DRAFT_NOT_APPROVED':'NARRATIVE_RUBRIC_NEEDS_SOURCE_REVIEW',
  sourceActivityReview:sourceStatus,originalNarrativeRubric:prose,
  structuredCriteria:criteria,possiblePoints:criteria?8:null,
  teacherAssignedAndAuthorized:false,realStudentWorkReceived:false,
  actualGradePersisted:false,feedbackPersisted:false
 });
}
if(handoff.length!==132||teacher.length!==132||r12.size!==14||r11.size!==3)
 throw Error('Unexpected counts in V12 reconciliation');
const save=(name,obj)=>fs.writeFileSync(path.join(docs,name),JSON.stringify(obj,null,2)+'\n');
save('US_CCSS_ALG1_CODER5_V12_SUPERVISOR2_PRACTICE_CONTRACT_HANDOFF_20261029.json',{
 schemaVersion:1,courseKey:progress.courseKey,asOfDate:'2026-10-29',
 recipient:'SUPERVISOR 2',sourcePairsFetched:129,sourceComparedCandidates:17,sourceRepairedV12:14,
 sourceActivityReviewsOutstanding:115,exactAcceptedPracticeContracts:0,
 safety:'Existing ALG1 fail-closed gate remains until actual source-aligned, scored, tenant-safe integration.',
 sharedRendererNote:'Five new SVGs are route-prepared in the ALG1-only partial, not added to the shared host. Requires Supervisor 2 approval.',
 contracts:handoff
});
save('US_CCSS_ALG1_CODER5_V12_TEACHER_ASSESSMENT_MATRIX_20261029.json',{
 schemaVersion:1,courseKey:progress.courseKey,asOfDate:'2026-10-29',
 narrativeRubrics:132,structuredDrafts:17,unstructuredPending:115,approvedRubrics:0,
 actualTeacherGrades:0,feedbackPersisted:0,lessons:teacher
});
ledger.v12={date:'2026-10-29',sourcePairsFetched:129,sourcePagesHttp200:258,
 sourceActivityHeadings:fetched.qualityCorrection.totalRealStudentActivityHeadings,
 directSourceComparisonCandidates:17,sourceRepairedV12:14,fullActivityReviewsOutstanding:115,
 structuredTeacherRubricDrafts:17,unstructuredTeacherRubricReviewsPending:115,
 staticOriginalSvgNew:5,actuallyRenderedInAuthorizedStudentSession:0,
 A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,
 actualTeacherGrades:0,actualStudentSubmissions:0,authenticatedTwoSchoolDbTests:0,
 automatedAudit:'docs/curriculum/US_CCSS_ALG1_CODER5_V12_SOURCE_FETCH_AUDIT_20261029.json',
 practiceHandoff:'docs/curriculum/US_CCSS_ALG1_CODER5_V12_SUPERVISOR2_PRACTICE_CONTRACT_HANDOFF_20261029.json',
 teacherRubricMatrix:'docs/curriculum/US_CCSS_ALG1_CODER5_V12_TEACHER_ASSESSMENT_MATRIX_20261029.json',
 note:'Source page retrieval and preliminary comparisons are not independent academic approval. Static unserved SVGs are not authenticated Stage B.'
};
save('US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json',ledger);
progress.acceptanceGates={...progress.acceptanceGates,
 V12SourcePagesHttp200:258,V12ComparedLessonCandidates:17,V12NewRepairs:14,
 V12RemainingAcademicReviews:115,V12StructuredRubrics:17,V12NewStaticSvg:5,
 A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,
 teacherAssessmentVerified:0,authenticatedTwoSchoolVerified:0
};
save('US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json',progress);
console.log(JSON.stringify({lessonIds:132,teacherSourceAndStudentPairs:129,compared:17,newRepaired:14,remaining:115,rubrics:17,gates:ledger.v12.A_ACCEPTED}));
