#!/usr/bin/env node
'use strict';
// V11 evidence is intentionally below academic A/B/C/D approval thresholds.
// A static SVG or draft rubric is never an authenticated student / teacher result.
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const ledgerPath=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V10_ACCEPTANCE_LEDGER_20261029.json');
const progressPath=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json');
const rubricsPath=path.join(root,'docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json');
const ledger=JSON.parse(fs.readFileSync(ledgerPath,'utf8'));
const progress=JSON.parse(fs.readFileSync(progressPath,'utf8'));
const rubricDoc=JSON.parse(fs.readFileSync(rubricsPath,'utf8'));
const graphByCode=new Map([
 ['U06:L11','/images/curriculum/alg1-v11/u06-l11-factored-graph.svg'],
 ['U06:L16','/images/curriculum/alg1-v11/u06-l16-vertex-graph.svg'],
 ['U07:L24','/images/curriculum/alg1-v11/u07-l24-quadratic-line.svg']
]);
const reviewerEvidence=new Map(rubricDoc.teacherRubricCandidates.map(x=>[x.lessonCode,x]));
if(ledger.lessons.length!==132||rubricDoc.teacherRubricCandidates.length!==3)
 throw Error('Ledger/rubric count drift');
const counts={sourceTaskComparisons:0,staticGraphAssets:0,teacherRubricDrafts:0};
for(const lesson of ledger.lessons){
 const suffix=lesson.lessonCode.match(/U\d\d:L\d\d$/)?.[0];
 const graph=graphByCode.get(suffix);
 const rubric=reviewerEvidence.get(lesson.lessonCode);
 if(Boolean(graph)!==Boolean(rubric))throw Error('Graph / rubric mismatch: '+lesson.lessonCode);
 if(graph){
   const assetPath=path.join(root,'src/Edulytics.Web/wwwroot',graph.replace(/^\//,''));
   if(!fs.existsSync(assetPath))throw Error('Missing graph asset '+graph);
   const json=JSON.parse(fs.readFileSync(path.join(root,lesson.candidatePath),'utf8'));
   if(!json.ContentVersion.includes('v11-source-task'))throw Error('Missing source task revision '+suffix);
   counts.sourceTaskComparisons++;
   counts.staticGraphAssets++;
   counts.teacherRubricDrafts++;
 }
 lesson.v11={
   stageASourceActivityComparison:graph
     ?'SOURCE_ACTIVITIES_AND_TEACHER_GOALS_SPOT_CHECKED_AWAITING_INDEPENDENT_ACCEPTANCE'
     :'PENDING_INDEPENDENT_SOURCE_TASK_COMPARISON',
   stageAReviewerSignoff:false,
   stageBFirstPartyGraphAsset:graph||null,
   stageBGraphSourceModel:graph?'INDEPENDENTLY_AUTHORED_SOURCE_EQUIVALENT_MODEL':null,
   stageBAuthenticatedGraphRendered:false,
   stageBEffectiveVersionResolved:false,
   stageCTeacherRubricDraft:rubric?rubricDoc.courseKey+'/'+suffix:null,
   stageCTeacherGradeFeedbackPersistenceVerified:false,
   stageCSourceSpecificPracticeGradingVerified:false,
   stageDTwoSchoolHttpsDbVerified:false,
   remainingBlockers:graph
     ?['AUTHOR_EDUCATOR_REVIEW_REQUIRED','AUTHENTICATED_ACTUAL_GRAPH_RENDER_UNVERIFIED',
        'TEACHER_SUBMISSION_FEEDBACK_PERSISTENCE_UNVERIFIED','EXACT_PRACTICE_AND_TENANT_UNVERIFIED']
     :['SOURCE_TASK_COMPARISON_REQUIRED','AUTHENTICATED_RENDER_UNVERIFIED',
        'EXACT_PRACTICE_AND_TEACHER_SCORING_UNVERIFIED','TWO_SCHOOL_DB_UNVERIFIED']
 };
 if(lesson.A.accepted||lesson.B.accepted||lesson.C.verified||lesson.D.verified)
   throw Error('Unexpected academic gate promotion: '+lesson.lessonCode);
}
ledger.v11={
 ...(ledger.v11||{}),
 date:'2026-10-29',
 status:'REAL_COURSE_LOCAL_CODE_ASSETS_AND_SOURCE_TASK_EVIDENCE__NO_STAGE_ACCEPTANCE',
 ...counts,
 graphGalleryView:'src/Edulytics.Web/Views/Shared/_UsCcssAlgebra1GraphGallery.cshtml',
 minimalRichV2HostHook:'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml',
 rubricCandidateManifest:'docs/curriculum/US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json',
 reviewNotes:[
  'Three first-party diagrams use original mathematical functions, not publisher image reproductions.',
  'The existing 129 unreviewed lesson source-activity comparisons are not promoted.',
  'Shared host-view single-line course-gated hook requires Supervisor 2 code-owner review; no Practice engine modifications.',
  'No teacher submission/grade/feedback persistence exists as verified V11 evidence.',
  'No authenticated two-school TLS / DB tests were performed for ALG1; all A-D counters stay zero.'
 ],
 gates:{A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,TEACHER_GRADED:0,AUTH_TWO_SCHOOL:0}
};
progress.acceptanceGates={
 ...(progress.acceptanceGates||{}),
 V11SourceActivityComparisons:counts.sourceTaskComparisons,
 V11StaticOriginalGraphs:counts.staticGraphAssets,
 V11TeacherRubricDrafts:counts.teacherRubricDrafts,
 A_ACCEPTED:0,B_ACCEPTED:0,C_VERIFIED:0,D_VERIFIED:0,
 teacherAssessmentVerified:0,authenticatedTwoSchoolVerified:0,
 note:'V11 created 3 source-reviewed candidate tasks, 3 accessible static SVG graph assets, and 3 draft human rubrics. These are not actual academic approval, authenticated rendering, grading or persistence.'
};
fs.writeFileSync(ledgerPath,JSON.stringify(ledger,null,2)+'\n');
fs.writeFileSync(progressPath,JSON.stringify(progress,null,2)+'\n');
console.log(JSON.stringify({lessons:132,unacceptedA:132,unacceptedB:132,
 counters:ledger.v11.gates,completedSourceSpotChecks:counts.sourceTaskComparisons,staticGraphs:counts.staticGraphAssets,rubricDrafts:counts.teacherRubricDrafts}));
