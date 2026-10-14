'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {summarize}=require('../tools/us-ccss-b01-v9-shared-root-cause-ledger.cjs');
function fixture(){
 const manifest={batchId:'B01',expectedLessonCount:500,lessons:[]};
 const packs=[0,1,2,3].map(()=>({packCode:'US-CCSS-MATH',versionCode:'CCSSM-2010',lessons:[]}));
 for(let i=0;i<500;i++){
  const code='PED:US-CCSS-MATH:G2:U01:L'+String(i).padStart(3,'0');
  manifest.lessons.push({lessonCode:code,sourceSemanticSha256:'a'.repeat(64)});
  packs[Math.floor(i/125)].lessons.push({lessonCode:code,canonicalBodySha256:'b'.repeat(64),
   translations:[{cultureCode:'en',
    stepByStepSolutions:i===0||i===1?'Step 1: Identify what needs to be measured.':'Count the groups.',
    workedExamples:i===2?'A Caption: A B Caption: B':'Compute 4 + 5 = 9',
    explanation:i===3?'This support them in adding.':'Count accurate groups.',
    keyConceptsAndRules:'Use number facts.',commonMistakes:'Do not reverse the operands.'}]});
 }
 const reviewed=[
  {batchId:'B01',rows:[{lessonCode:manifest.lessons[0].lessonCode,sourceSemanticSha256:'a'.repeat(64)}]},
  {batchId:'B01',reviewedObjectives:[]},{batchId:'B01',rows:[]},
  {batchId:'B01',rows:[]},
  {batchId:'B01',rows:[]},
  {batchId:'B01',rows:[]}
 ];
 return {manifest,packs,reviewed};
}
test('One 500-lesson pass groups shared templates without declaring academic certification',()=>{
 const x=summarize(fixture());
 assert.deepEqual(x.coverage,{expected:500,actual:500,unique:500});
 assert.equal(x.automatedFixAuthority,false);
 assert.equal(x.academicDecision,'BATCH_NO-GO');
 assert.equal(x.groups.SHARED_RULER_METHOD_TEMPLATE.rawCandidateCount,2);
 assert.equal(x.groups.SHARED_RULER_METHOD_TEMPLATE.sourceGoalReviewedCandidates,1);
 assert.equal(x.groups.SHARED_RULER_METHOD_TEMPLATE.unverifiedCandidates,1);
 assert.equal(x.groups.SOURCE_FIGURE_CAPTION_PLACEHOLDER.rawCandidateCount,1);
 assert.equal(x.rows[0].finalEffectiveBodyVerified,false);
 assert.ok(x.rows.every(r=>!r.certifiedByThisIndex));
});
test('Reject source fingerprint drift, duplicate content and missing 500th lesson',()=>{
 const x=fixture();x.reviewed[0].rows[0].sourceSemanticSha256='c'.repeat(64);
 assert.throws(()=>summarize(x),/Stale academic source-goal fingerprint/);
 const y=fixture();y.packs[0].lessons[1].lessonCode=y.packs[0].lessons[0].lessonCode;
 assert.throws(()=>summarize(y),/Duplicate pack identity/);
 const z=fixture();z.manifest.lessons.pop();
 assert.throws(()=>summarize(z),/Required one 500 lesson/);
});
test('Single signal from words or figures cannot cause blanket template repair',()=>{
 const x=summarize(fixture());
 assert.equal(x.groups.SOURCE_FIGURE_CAPTION_PLACEHOLDER.sourceGoalReviewedCandidates,0);
 assert.equal(x.rows[2].rootCauseCandidates[0].reviewState,'SOURCE_GOAL_NOT_VERIFIED');
 assert.equal(x.rows[2].independentL1L6Evidence,'NOT_IN_THIS_AUDIT');
});

test('Every ruler template cue is partitioned by explicit goal without certifying content',()=>{
 const x=fixture();
 x.packs[0].lessons[0].translations[0].explanation='Learning goal: Measure length with a ruler. ';
 x.packs[0].lessons[1].translations[0].explanation='Learning goal: Compare fractions on a number line. ';
 const audit=summarize(x), group=audit.groups.SHARED_RULER_METHOD_TEMPLATE;
 assert.deepEqual(group.byGoalContext,{
  LENGTH_CONTEXT_POSSIBLY_RELEVANT:1,NUMBER_LINE_CONTEXT_REVIEW:1,
  NON_LENGTH_GOAL_SOURCE_REVIEW:0,GOAL_UNRESOLVED:0});
 assert.equal(audit.academicDecision,'BATCH_NO-GO');
 assert.equal(audit.rows[0].rootCauseCandidates[0].goalContext,'LENGTH_CONTEXT_POSSIBLY_RELEVANT');
 assert.equal(audit.rows[1].rootCauseCandidates[0].goalContext,'NUMBER_LINE_CONTEXT_REVIEW');
});
test('Non-length goal stays review-only even if source reference is fingerprint-verified',()=>{
 const x=fixture();
 x.packs[0].lessons[0].translations[0].explanation='Learning goal: Compose hundreds as ten tens.';
 const audit=summarize(x);
 assert.equal(audit.rows[0].rootCauseCandidates[0].goalContext,'NON_LENGTH_GOAL_SOURCE_REVIEW');
 assert.equal(audit.rows[0].rootCauseCandidates[0].reviewState,'SOURCE_GOAL_REVIEWED_REQUIRES_RUNTIME_RETEST');
 assert.equal(audit.rows[0].certifiedByThisIndex,false);
});
