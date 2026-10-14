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
 return {manifest:{batchId:'B01',expectedLessonCount:500,lessons},
  packs,sourceIndex,teacherActivityIndex,reviewed:[
   {batchId:'B01',rows:[{lessonCode:lessons[0].lessonCode,sourceSemanticSha256:shaA}]},
   {batchId:'B01',reviewedObjectives:[]},
   {batchId:'B01',rows:[]},{batchId:'B01',rows:[]}
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
 assert.equal(x.nodes[0].academicGoalSourceReview,'REVIEWED_PURPOSE_ONLY_NOT_L1_L6');
 assert.equal(x.nodes[1].academicGoalSourceReview,'NOT_SOURCE_GOAL_VERIFIED');
 assert.equal(x.academicSourcePurposeReviews.stillNotPurposeReviewed,499);
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
