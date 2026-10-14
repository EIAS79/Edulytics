'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {reconcile,expectedSubject}=require('../tools/us-ccss-b01-v9-effective-root-cause-reconciliation.cjs');
const shaA='a'.repeat(64),shaB='b'.repeat(64),shaC='c'.repeat(64);
const lesson=i=>'PED:US-CCSS-MATH:G2:U01:L'+String(i).padStart(3,'0');
function fixture(){
 const nodes=Array.from({length:500},(_,i)=>({
  ordinal:i+1,lessonCode:lesson(i),sourceSemanticSha256:shaA,
  rawLearnerBodySha256:shaB,
  originalTeacherGoalSectionCaptured:true,
  sourceTeacherPurposeSignals:i===0?['LENGTH']:['ADDITION'],
  rawExactTemplateIds:i<2?['SHARED_TEMPLATE']:[],
  academicDecision:'NOT_CERTIFIED_BY_THIS_GRAPH'
 }));
 const effective=Array.from({length:500},(_,i)=>({
  ordinal:i+1,lessonCode:lesson(i),effectiveContentVersion:'fixture-v1',
  effectiveBodyFingerprint:shaC,
  exactEffectiveFieldSha256:{
   stepByStepSolutions:i===0?shaA:shaB,commonMistakes:shaC
  },
  effectiveRawPatternSignals:{
   rulerTemplate:i===0,figureCaptionPlaceholder:i===1,
   learnerTeacherPronoun:false
  },
  richV2:{source:i===1?'EXPLICIT_SIDECAR':'RUNTIME_COMPOSER',
   composed:true,visualCount:i===0?0:1,visualKinds:i===0?[]:['NumberLine']},
  academicDisposition:'EFFECTIVE_RUNTIME_EVIDENCE_ONLY_NOT_CERTIFIED'
 }));
 return {
  graph:{planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
   coverage:{expected:500,observed:500,unique:500,missing:0},
   repairDecision:'NO_AUTOMATIC_REPAIR_AUTHORITY',
   nodes,repeatedExactLearnerTemplates:{groups:[{
    rootCauseId:'SHARED_TEMPLATE',affectedComponent:'CanonicalRichV2:stepByStepSolutions',
    candidateLessonCount:2,learnerTemplateSha256:shaA,
    excerpt:'Step 1: Identify what needs to be measured.',
    affectedLessonCodes:[lesson(0),lesson(1)],distinctStandardsDomains:2
   }]},
   rootCauseSignals:{
    SHARED_RULER_METHOD_TEMPLATE:{affectedLessonCodes:[lesson(0),lesson(1)],candidateLessonCount:2},
    SOURCE_FIGURE_CAPTION_PLACEHOLDER:{affectedLessonCodes:[lesson(1)],candidateLessonCount:1},
    LEARNER_TEACHER_PRONOUN_DRIFT:{affectedLessonCodes:[],candidateLessonCount:0}
   }},
  effective:{planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
   coverage:{expected:500,materialized:500,unique:500},academicDecision:'BATCH_NO-GO',
   rows:effective}
 };
}
test('One joined graph reports actual materializer transforms while remaining fail-closed',()=>{
 const {graph,effective}=fixture(),x=reconcile(graph,effective);
 assert.deepEqual(x.coverage,{expected:500,joined:500,unique:500,missing:0});
 assert.equal(x.groupCount,1);
 assert.equal(x.repeatedTemplateGroups[0].exactMaterializedTemplateStillUsed,1);
 assert.equal(x.repeatedTemplateGroups[0].exactMaterializedTemplateReplaced,1);
 assert.deepEqual(x.repeatedTemplateGroups[0].lexicalSourcePurposeMismatchCandidates,[lesson(1)]);
 assert.equal(x.repeatedTemplateGroups[0].priorityScore,2);
 assert.equal(x.sharedPatternRoots.SHARED_RULER_METHOD_TEMPLATE.effectiveMarkerStillPresent,1);
 assert.equal(x.sharedPatternRoots.SHARED_RULER_METHOD_TEMPLATE.effectiveMarkerAbsent,1);
 assert.equal(x.sharedPatternRoots.SOURCE_FIGURE_CAPTION_PLACEHOLDER.withComposedVisual,1);
 assert.equal(x.richV2WithoutStructuralVisuals.count,1);
 assert.equal(x.confirmedSemanticDefectsByThisTool,0);
 assert.equal(x.automaticFixAuthorization,false);
 assert.equal(x.academicDecision,'BATCH_NO-GO');
 assert.ok(x.rows.every(r=>r.certificationState==='NOT_CERTIFIED_BY_THIS_JOIN'));
});
test('Template subject inference is bounded and never a defect verdict',()=>{
 assert.deepEqual(expectedSubject('Identify the whole and the part being removed'),['SUBTRACTION']);
 assert.deepEqual(expectedSubject('Step 1: Identify what needs to be measured.'),['LENGTH','WEIGHT_MASS']);
 assert.deepEqual(expectedSubject('Step 1: Identify what the task asks you to find or show.'),[]);
 assert.deepEqual(expectedSubject('A novel uncommon string'),[]);
});
test('Reject stale identity, missing 500th runtime lesson and false Academic PASS',()=>{
 const a=fixture();a.effective.rows[4].lessonCode='WRONG';
 assert.throws(()=>reconcile(a.graph,a.effective),/Expected values to be strictly equal/);
 const b=fixture();b.effective.rows.pop();
 assert.throws(()=>reconcile(b.graph,b.effective),/Expected values to be strictly equal/);
 const c=fixture();c.effective.academicDecision='BATCH_CERTIFIED';
 assert.throws(()=>reconcile(c.graph,c.effective),/Expected values to be strictly equal/);
});
test('Reject forged fingerprint and fabricated Rich V2 compositions',()=>{
 const a=fixture();a.effective.rows[0].effectiveBodyFingerprint='not-a-sha';
 assert.throws(()=>reconcile(a.graph,a.effective),/regular expression/);
 const b=fixture();b.effective.rows[1].richV2.composed=false;
 assert.throws(()=>reconcile(b.graph,b.effective),/Unusable Rich V2/);
});
