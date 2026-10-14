'use strict';
const t=require('node:test'),assert=require('node:assert/strict');
const {join}=require('../tools/us-ccss-b01-delta-review.cjs');
const sha='a'.repeat(64),body='b'.repeat(64),head='c'.repeat(40);
function fixture(){
  const lessons=Array.from({length:500},(_,i)=>({
    ordinal:i+1,lessonCode:'PED:US-CCSS-MATH:G1:U01:L'+String(i+1).padStart(3,'0'),
    sourceSemanticSha256:sha,title:'Lesson '+i,sourceUrl:'https://example.org/'+i}));
  const manifest={batchId:'B01',lessons};
  const visual={expected:500,academicDecision:'NOT_CERTIFIED',
    lessonsNeedingPedagogicalVisualReview:2,rows:lessons.map(m=>({
    ordinal:m.ordinal,lessonCode:m.lessonCode,publisherTitle:'Fractions',
    sourceEffectiveContentSha256:body,richV2EffectiveContentSha256:body,
    sameEffectiveContent:true,actualRichV2VisualKinds:['EquationSet'],
    publisherCueVisualReviewTypes:[]}))};
  visual.rows[5].publisherCueVisualReviewTypes=['GRAPH_AND_SCALE'];
  visual.rows[5].actualRichV2VisualKinds=['EquationSet','PictureGraph'];
  visual.rows[7].publisherCueVisualReviewTypes=['NUMBER_LINE'];
  const activity={expected:500,academicDecision:'NOT_CERTIFIED',withReviewCues:2,
    rows:lessons.map(m=>({ordinal:m.ordinal,lessonCode:m.lessonCode,
    sourceSemanticSha256:sha,effectiveContentSha256:body,
    candidateMissingRepresentations:[]}))};
  activity.rows[5].candidateMissingRepresentations=['PICTURE_GRAPH'];
  activity.rows[8].candidateMissingRepresentations=['RULER'];
  const unified={batchId:'B01',coverage:{joined:500,unique:500},batchDecision:'BATCH_NO-GO',
    lessons:lessons.map(m=>({ordinal:m.ordinal,lessonCode:m.lessonCode,
      sourceSemanticSha256:sha,effectiveContentSha256:body,confirmedDefects:[]}))};
  const progress={batchId:'B01',expected:500,observed:500,remainingEvidence:499,state:'BATCH_NO-GO',
    lessons:lessons.map((m,i)=>({ordinal:m.ordinal,lessonCode:m.lessonCode,
      sourceSemanticSha256:sha,effectiveContentSha256:body,evidenceReady:i===0,
      confirmedDefects:[],sixGateEvidence:Object.fromEntries(
        ['L1','L2','L3','L4','L5','L6'].map(g=>[g,{status:i===0?'LEDGER_PASS':'MISSING_VERIFIED_EVIDENCE'}]))
    }))};
  return {manifest,visual,activity,unified,progress,baselineRunId:38492263814,baselineSha:head};
}
t('reuse all four immutable evidence sources and group overlapping lesson signals',()=>{
  const x=join(fixture());
  assert.equal(x.counts.publisherCues,2);
  assert.equal(x.counts.teacherCues,2);
  assert.equal(x.counts.overlap,1);
  assert.equal(x.counts.publisherOnly,1);
  assert.equal(x.counts.teacherOnly,1);
  assert.equal(x.counts.reviewUnion,3);
  assert.equal(x.counts.missingEvidenceLessons,499);
  assert.equal(x.missingByGate.L6,499);
  assert.equal(x.counts.potentialClassifierOvermatches,1);
  assert.equal(x.queue.length,3);
  assert.equal(x.rootCauseGroups.length,4);
  assert.equal(x.releaseEligible,false);
  assert.equal(x.academicDecision,'BATCH_NO-GO');
});
t('source, body, order, number and aggregate drift all fail closed',()=>{
  let x=fixture();x.visual.rows[7].richV2EffectiveContentSha256='c'.repeat(64);
  assert.throws(()=>join(x),/Effective content SHA drift/);
  x=fixture();x.activity.rows[1].lessonCode=x.activity.rows[0].lessonCode;
  assert.throws(()=>join(x),/reordered/);
  x=fixture();x.progress.lessons.pop();
  assert.throws(()=>join(x),/500 progress rows/);
  x=fixture();x.unified.lessons[7].sourceSemanticSha256='e'.repeat(64);
  assert.throws(()=>join(x),/Source semantic SHA drift/);
  x=fixture();x.visual.lessonsNeedingPedagogicalVisualReview=3;
  assert.throws(()=>join(x),/review count drift/);
});
t('confirmed defects are grouped independently of teacher and publisher review cues',()=>{
  const x=fixture(),fault={code:'MATH_ERROR',dimension:'solutions'};
  x.unified.lessons[42].confirmedDefects=[fault];
  x.progress.lessons[42].confirmedDefects=[fault];
  const r=join(x);
  assert.equal(r.counts.confirmedDefectLessons,1);
  assert.equal(r.rootCauseGroups.find(g=>g.key==='CONFIRMED_DEFECT:MATH_ERROR').count,1);
  assert.equal(r.releaseEligible,false);
  x.progress.lessons[42].confirmedDefects=[];
  assert.throws(()=>join(x),/Defect disagreement/);
});
t('likely overbroad fractional lineplot cue is never silently declared fixed',()=>{
  const x=fixture();
  x.visual.rows[7].publisherTitle='Fractional Measurements on Line Plots';
  x.visual.rows[7].publisherCueVisualReviewTypes=['FRACTION_PARTS'];
  x.visual.rows[7].actualRichV2VisualKinds=['LinePlot','EquationSet'];
  const r=join(x);
  assert.equal(r.counts.potentialClassifierOvermatches,2);
  assert.equal(r.queue.find(x=>x.ordinal===8).reviewState,'REVIEW_REQUIRED_NOT_CERTIFIED');
});
