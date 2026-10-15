'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {buildGraph,standardDomain}=require('../tools/us-ccss-b01-v9-global-root-cause-graph.cjs');

function fixture(){
 const shaA='a'.repeat(64),shaB='b'.repeat(64),shaC='c'.repeat(64),
  shaD='d'.repeat(64),shaE='e'.repeat(64),shaF='f'.repeat(64);
 const repeatedSteps='Step 1: Identify what needs to be measured. Step 2: Read the exact ruler endpoint. Step 3: Count same size units accurately. Step 4: Write the measurement.';
 const repeatedMistake='Common mistake: assuming every task is about using a ruler when an illustration is present. Consider the actual learning target.';
 const lessons=Array.from({length:500},(_,i)=>{
  const code='PED:US-CCSS-MATH:G2:U01:L'+String(i).padStart(3,'0');
  return {ordinal:i+1,lessonCode:code,title:'Fixture '+i,
   sourceUrl:'https://example.invalid/teachers/lesson-'+i,
   sourceSemanticSha256:shaA,officialOutcomeCodes:[i%2?'CCSS:2.NBT.A.1':'CCSS:2.OA.A.1'],
   gradeOrCourse:i<250?'Grade 2':'Grade 3'};
 });
 const packs=[0,1,2,3].map(()=>({packCode:'US-CCSS-MATH',
  versionCode:'CCSSM-2010',lessons:[]}));
 for(let i=0;i<500;i++){
  packs[Math.floor(i/125)].lessons.push({
   lessonCode:lessons[i].lessonCode,canonicalBodySha256:shaB,
   translations:[{cultureCode:'en',explanation:'Learning goal: Compose numbers.',
    stepByStepSolutions:i<25?repeatedSteps:'Custom approach '+i,
    commonMistakes:i<15?repeatedMistake:'Unique situation '+i,
    keyConceptsAndRules:'Count accurately.',workedExamples:'A correct independent example.',
    quickSummary:'Understand counting.'}]
  });
 }
 const sourceIndex={batchId:'B01',expectedLessonCount:500,matchedToManifest:500,
  rows:lessons.map(x=>({ordinal:x.ordinal,lessonCode:x.lessonCode,
   localRelativePath:'02_Grade_2_IM_K5\\Exact_Sources\\lesson'+x.ordinal+'.html',
   localFileByteSha256:shaC,localFileBytes:500,
   state:'REFERENCE_FILE_INVENTORIED_NOT_ACADEMICALLY_CERTIFIED'}))};
 const teacherActivityIndex={batchId:'B01',sourceCoverage:500,sourceAvailable:500,
  representationCueKeys:['DOT_IMAGES','NUMBER_LINE','FRACTION_PARTS',
   'CLOCK_FACE','PLACE_VALUE','RECTANGULAR_ARRAY','MEASUREMENT_SCALE','DATA_GRAPH'],
  rows:lessons.map(x=>[x.ordinal,x.lessonCode,shaD,shaE,1,shaF,2,x.ordinal===1?2:0])};
 const sourceGoalMemory={planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  coverage:{manifestExpected:500,sourceFilesRead:500,sourceGoalSectionsUnresolved:1,alternativeLessonPurposeHeadingsVerified:1,sourcePurposeAvailableViaEitherHeading:500},
  rows:lessons.map((x,i)=>({ordinal:x.ordinal,lessonCode:x.lessonCode,
   sourceSemanticSha256:x.sourceSemanticSha256,
   sourceFileInventorySha256:sourceIndex.rows[i].localFileByteSha256,
   originalTeacherRelativePath:sourceIndex.rows[i].localRelativePath,
   originalTeacherSourceUrl:x.sourceUrl,
   teacherGoalSectionFound:i!==499,
   alternativePurposeHeadingFound:i===499,
   alternativePurposeHeading:i===499?'Lesson Purpose':undefined,
   originalTeacherGoalStillAbsent:i===499,
   teacherFacingPurposeSignals:i<20?['LENGTH']:['ADDITION']}))};
 const firstPassCoverage={
  schemaVersion:1,
  kind:'B01_FULL_500_SOURCE_PURPOSE_COMPARISON_COVERAGE_REFERENCE',
  batchId:'B01',
  scope:{manifestLessons:500,previouslySourcePurposeReviewed:1,
   newlySourcePurposeCompared315:499,sourcePurposeComparedTotal:500,
   fullL1toL6CertificationsGranted:0},
  newReviewedRows:lessons.slice(1).map((item,index)=>({
   ordinal:item.ordinal,
   lessonCode:item.lessonCode,
   sourceSemanticSha256:item.sourceSemanticSha256,
   sourceFileInventorySha256:sourceIndex.rows[index+1].localFileByteSha256,
   canonicalOriginalLearnerBodySha256:shaB,
   originalTeacherSourceUrl:item.sourceUrl,
   originalTeacherActivityUrl:item.sourceUrl.replace('/preparation.html','/lesson.html'),
   liveTeacherPreparationSha256:shaD,
   liveTeacherActivitySha256:shaE,
   officialGoalElementsRead:1,
   officialTeacherActivityHeadingsRead:2,
   academicCertified:false,effectiveRuntimeAndL1ToL6VerificationStillRequired:true
  }))
 };
 return {firstPassCoverage,manifest:{batchId:'B01',expectedLessonCount:500,lessons},
  packs,sourceIndex,teacherActivityIndex,sourceGoalMemory,reviewed:[
   {batchId:'B01',rows:[{lessonCode:lessons[0].lessonCode,sourceSemanticSha256:shaA}]},
   {batchId:'B01',reviewedObjectives:[]},
   {batchId:'B01',rows:[]},{batchId:'B01',rows:[]},
   {batchId:'B01',rows:[]},
   {batchId:'B01',rows:[]},
   {batchId:'B01',rows:[]},
   {batchId:'B01',rows:[]},
   {batchId:'B01',rows:[]}
  ]};
}
test('Full B01 500/500 cross-joins mathematical, publisher-source and shared templates in ONE graph',()=>{
 const input=fixture(),x=buildGraph(input);
 assert.deepEqual(x.coverage,{expected:500,observed:500,unique:500,missing:0});
 assert.equal(x.repeatedExactLearnerTemplates.uniqueGroups,2);
 const groups=x.repeatedExactLearnerTemplates.groups;
 const steps=groups.find(y=>y.affectedComponent==='CanonicalRichV2:stepByStepSolutions');
 const mistakes=groups.find(y=>y.affectedComponent==='CanonicalRichV2:commonMistakes');
 assert.equal(steps.candidateLessonCount,25);
 assert.equal(steps.distinctStandardsDomains,2);
 assert.equal(steps.priorityScore,50);
 assert.equal(mistakes.candidateLessonCount,15);
 assert.equal(x.nodes[0].rawExactTemplateIds.length,2);
 assert.equal(x.nodes[24].rawExactTemplateIds.length,1);
 assert.equal(x.nodes[25].rawExactTemplateIds.length,0);
 assert.equal(x.mathElementCandidatesByFamily.measurement_units,25);
 assert.equal(x.nodes[0].academicGoalSourceReview,'PRIOR_SOURCE_PURPOSE_REVIEW_NOT_L1_L6');
 assert.equal(x.nodes[1].academicGoalSourceReview,'NEW_SOURCE_GOAL_ACTIVITY_FIRST_PASS_NOT_L1_L6');
 assert.equal(x.academicSourcePurposeReviews.previousSourcePurposeReviews,1);
 assert.equal(x.academicSourcePurposeReviews.newSourceGoalActivityFirstPass,499);
 assert.equal(x.academicSourcePurposeReviews.firstPassSourceGoalActivityComparedTotal,500);
 assert.equal(x.academicSourcePurposeReviews.missingSourceGoalActivityFirstPass,0);
 assert.equal(x.academicSourcePurposeReviews.firstPassIsFullAcademicCertification,false);
 assert.equal(x.academicSourcePurposeReviews.newlyCertifiedByThisGraph,0);
 assert.equal(x.automaticSourceGoalSignalCapture.teacherGoalsFound,499);
 assert.equal(x.automaticSourceGoalSignalCapture.alternateOfficialLessonPurposeHeadings,1);
 assert.equal(x.automaticSourceGoalSignalCapture.teacherPurposesAvailableByEitherHeading,500);
 assert.equal(x.automaticSourceGoalSignalCapture.unresolvedLessonCodes.length,1);
 assert.equal(x.rootCauseSignals.SHARED_RULER_METHOD_TEMPLATE.byOriginalTeacherGoalContext.ORIGINAL_LENGTH,20);
 assert.equal(x.nodes[0].sourceTeacherPurposeSignals[0],'LENGTH');
 assert.equal(x.originalTeacherRepresentationTextCorrelation.NUMBER_LINE.rawTextGapCandidates,1);
 assert.equal(x.certificationDecision,'BATCH_NO-GO');
 assert.equal(x.repairDecision,'NO_AUTOMATIC_REPAIR_AUTHORITY');
 assert.ok(x.nodes.every(n=>n.academicDecision==='NOT_CERTIFIED_BY_THIS_GRAPH'));
 assert.ok(groups.every(g=>!g.approvedToModify&&!g.academicPass));
});
test('Repeated language does not become a mathematical defect or certification by frequency',()=>{
 const x=buildGraph(fixture());
 assert.ok(x.repeatedExactLearnerTemplates.groups.every(g=>
  g.conclusion.startsWith('SUSPECTED_ISSUE')));
 assert.equal(x.verificationDecision,
  'FULL_SOURCE_AND_EFFECTIVE_RICH_V2_COMPARISON_OUTSTANDING');
 assert.equal(x.sourceVisualTextCues,undefined);
 assert.equal(x.originalTeacherRepresentationTextCorrelation.NUMBER_LINE.actualRenderedVisualStatus,
  'UNVERIFIED');
 assert.equal(x.releaseEligible,false);
});
test('Rejects tampering with verified source, source index or the 500th identity',()=>{
 const f=fixture();
 f.reviewed[0].rows[0].sourceSemanticSha256='9'.repeat(64);
 assert.throws(()=>buildGraph(f),/Stale academic source-goal fingerprint|Source evidence fingerprint drift/);
 const s=fixture();s.sourceIndex.rows[11].lessonCode='WRONG';
 assert.throws(()=>buildGraph(s),/Source inventory provenance missing\/mismatched/);
 const m=fixture();m.manifest.lessons.pop();
 assert.throws(()=>buildGraph(m),/exactly 500|Required one 500 lesson/);
 const t=fixture();t.sourceGoalMemory.rows[0].sourceFileInventorySha256='9'.repeat(64);
 assert.throws(()=>buildGraph(t),/original teacher-goal memory/);
 const missing=fixture();missing.firstPassCoverage.newReviewedRows.pop();
 assert.throws(()=>buildGraph(missing),/Source first-pass must join/);
 const drift=fixture();drift.firstPassCoverage.newReviewedRows[0].sourceFileInventorySha256='0'.repeat(64);
 assert.throws(()=>buildGraph(drift),/Source first-pass new row mismatched/);
 const fake=fixture();fake.firstPassCoverage.newReviewedRows[0].academicCertified=true;
 assert.throws(()=>buildGraph(fake),/Source first-pass new row mismatched/);
});
test('No synthetic numeric domain guesses and no cross-field exact template conflation',()=>{
 assert.equal(standardDomain('CCSS:2.NF.A.2'),'NF');
 assert.equal(standardDomain('CCSS:3.G.A.1'),'G');
 assert.equal(standardDomain('unrecognized'),'OTHER');
 const x=buildGraph(fixture());
 const seen=new Set(x.repeatedExactLearnerTemplates.groups.map(g=>g.rootCauseId));
 assert.equal(seen.size,2);
 for(const g of x.repeatedExactLearnerTemplates.groups)
  assert.equal(g.learnerTemplateSha256.length,64);
});
