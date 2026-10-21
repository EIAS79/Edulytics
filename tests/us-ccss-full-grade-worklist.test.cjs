'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {buildWorklist}=require('../tools/curriculum/us-ccss-full-grade-worklist.cjs');
const ROOT=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));
const manifest=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json');
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json');
const copy=()=>structuredClone(ledger);

test('ONE whole-grade queue includes ALL 96 ordinals 51-146 with no five-lesson cap',()=>{
  const w=buildWorklist(manifest,copy());
  assert.equal(w.executionUnit,'ONE_WHOLE_GRADE');
  assert.equal(w.fixedChunkSize,null);
  assert.equal(w.stoppedEveryFiveLessons,false);
  assert.equal(w.items.length,96);
  assert.deepEqual(w.items.map(x=>x.ordinal),Array.from({length:96},(_,i)=>i+51));
  assert.equal(w.items[0].lessonCode,'PED:US-CCSS-MATH:G1:U03:L14');
  assert.equal(w.items[95].lessonCode,'PED:US-CCSS-MATH:G1:U08:L10');
  assert.equal(w.historic.count,50);
  assert.equal(w.historic.policy,'REFERENCE_ONLY_NO_RETEST');
  const complete=ledger.newLessons.every(x=>x.status==='INTERNALLY_ACCEPTED');
  assert.equal(w.status,complete?'ELIGIBLE_FOR_SEPARATE_FULL_GRADE_EVIDENCE_GATE':'NO_GO');
  assert.equal(w.nextGradeAllowed,false);
  assert.equal(ledger.release,'NO_GO');
  const start=ledger.newLessons.find(x=>x.status==='NOT_STARTED')?.ordinal??147;
  assert.equal(w.implementation.nextUnstartedOrdinal,start);
  assert.equal(w.academicVerification.firstPendingOrdinal,ledger.nextOrdinal);
  assert.equal(w.implementation.inProgressCount,
    ledger.newLessons.filter(x=>x.status==='IN_PROGRESS').length);
});
test('same one-grade queue persists beyond 5/10/30 without synthetic acceptance',()=>{
  const w=buildWorklist(manifest,copy(),{remainingOnly:true});
  const remaining=ledger.newLessons.filter(x=>x.status!=='INTERNALLY_ACCEPTED');
  assert.deepEqual(w.items.map(x=>x.ordinal),remaining.map(x=>x.ordinal));
  assert.equal(w.acceptedNew,96-remaining.length);
  assert.equal(w.firstUnfinishedOrdinal,remaining[0]?.ordinal??147);
});
test('start NEW lesson implementation after already started work, without duplicating 51–80',()=>{
  const first=ledger.newLessons.find(x=>x.status==='NOT_STARTED')?.ordinal??147;
  const queue=buildWorklist(manifest,copy(),{newOnly:true});
  assert.equal(queue.queueSelection,'NOT_STARTED_IMPLEMENTATION_ONLY');
  assert.equal(queue.implementation.nextUnstartedOrdinal,first);
  assert.equal(queue.items[0]?.ordinal??147,first);
  assert.equal(queue.items.length,ledger.newLessons.filter(x=>x.status==='NOT_STARTED').length);
  assert.ok(queue.items.every(x=>x.status==='NOT_STARTED'));
  assert.equal(queue.academicVerification.firstPendingOrdinal,ledger.nextOrdinal);
  assert.equal(queue.fixedChunkSize,null);
  assert.throws(()=>buildWorklist(manifest,copy(),{newOnly:true,remainingOnly:true}),/either academic/);
});
test('no five-chunk fallback if canonical identity or scope altered',()=>{
  const bad=copy();bad.newLessons[5].ordinal=55;
  assert.throws(()=>buildWorklist(manifest,bad),/identity changed|reordered|missing/);
  const badScope=copy();badScope.newScope.count=5;
  assert.throws(()=>buildWorklist(manifest,badScope),/Full-grade progress ledger invalid/);
  const short=copy();short.newLessons=short.newLessons.slice(0,5);
  assert.throws(()=>buildWorklist(manifest,short),/Full-grade progress ledger invalid/);
});
test('resumption never skips earlier unfinished lessons just because 30 started',()=>{
  const progress=copy();
  const pending=progress.newLessons.find(x=>x.status!=='INTERNALLY_ACCEPTED')?.ordinal??147;
  assert.equal(buildWorklist(manifest,progress).firstUnfinishedOrdinal,pending);
  progress.nextOrdinal=pending===81?51:81;
  assert.throws(()=>buildWorklist(manifest,progress),/Resume cursor disagrees/);
});
test('all-accepted label cannot itself publish the grade or impersonate source evidence',()=>{
  const progress=copy();
  for(const row of progress.newLessons)row.status='INTERNALLY_ACCEPTED';
  progress.nextOrdinal=147;
  const w=buildWorklist(manifest,progress,{remainingOnly:true});
  assert.equal(w.items.length,0);
  assert.equal(w.status,'ELIGIBLE_FOR_SEPARATE_FULL_GRADE_EVIDENCE_GATE');
  assert.equal(w.nextGradeAllowed,false);
  assert.equal(w.implementation.nextUnstartedOrdinal,147);
});

test('whole-grade resumption preserves existing checkpoints rather than restarting 90 lessons',()=>{
  const dir=path.join(ROOT,'artifacts/curriculum-delivery/US-CCSS-MATH/G1');
  const checkpointOrdinals=fs.readdirSync(dir)
    .filter(name=>/^lesson-\d+-v2-stage-a-d-checkpoint\.json$/.test(name))
    .map(name=>Number(name.match(/^lesson-(\d+)-/)[1]));
  const w=buildWorklist(manifest,copy(),{checkpointOrdinals});
  const pending=w.items.filter(x=>x.needsNewAcademicEvidence);
  const accepted=w.items.filter(x=>!x.needsNewAcademicEvidence);
  assert.equal(w.items.length,96);
  assert.equal(w.academicVerification.pendingCount,pending.length);
  assert.equal(w.sourceReviewResumption.checkpointFiles,checkpointOrdinals.length);
  assert.equal(w.sourceReviewResumption.pendingWithCheckpoint,
    pending.filter(x=>checkpointOrdinals.includes(x.ordinal)).length);
  assert.equal(w.sourceReviewResumption.pendingWithoutCheckpoint,
    pending.filter(x=>!checkpointOrdinals.includes(x.ordinal)).length);
  assert.deepEqual(w.sourceReviewResumption.pendingWithoutCheckpointOrdinals,
    pending.filter(x=>!checkpointOrdinals.includes(x.ordinal)).map(x=>x.ordinal));
  assert.equal(w.sourceReviewResumption.acceptedWithCheckpoint,
    accepted.filter(x=>checkpointOrdinals.includes(x.ordinal)).length);
  assert.equal(w.sourceReviewResumption.pendingWithCheckpoint+
    w.sourceReviewResumption.pendingWithoutCheckpoint,pending.length);
  assert.ok(w.items.every(x=>x.hasV2Checkpoint!==null));
  assert.ok(w.items.filter(x=>x.hasV2Checkpoint&&x.needsNewAcademicEvidence).every(x=>
    x.resumptionClass==='CONTINUE_EXISTING_A_D_CHECKPOINT'));
  assert.ok(w.items.filter(x=>!x.hasV2Checkpoint&&x.needsNewAcademicEvidence).every(x=>
    x.resumptionClass==='REUSE_INDEXED_SOURCE_AND_EXISTING_CANDIDATE_THEN_COMPLETE_A_D'));
  assert.match(w.sourceReviewResumption.note,/NOT an academic acceptance/);
  assert.equal(w.status,w.academicVerification.pendingCount===0?
    'ELIGIBLE_FOR_SEPARATE_FULL_GRADE_EVIDENCE_GATE':'NO_GO');
  assert.equal(w.nextGradeAllowed,false);
});

test('checkpoint presence cannot manufacture acceptance, suppress missing work or silently add historical lessons',()=>{
  const baseline=buildWorklist(manifest,copy());
  assert.equal(baseline.sourceReviewResumption,null);
  assert.ok(baseline.items.every(x=>x.hasV2Checkpoint===null));
  for(const invalid of [[50],[147],[51,51],['51'],[51.5]]) {
    assert.throws(()=>buildWorklist(manifest,copy(),{checkpointOrdinals:invalid}),
      /Checkpoint snapshot/);
  }
  const progress=copy();
  const checkpoints=[57,80,106];
  const w=buildWorklist(manifest,progress,{remainingOnly:true,checkpointOrdinals:checkpoints});
  const remaining=progress.newLessons.filter(x=>x.status!=='INTERNALLY_ACCEPTED');
  const pendingWithCheckpoint=remaining.filter(x=>checkpoints.includes(x.ordinal)).length;
  assert.equal(w.academicVerification.pendingCount,remaining.length);
  assert.equal(w.sourceReviewResumption.pendingWithCheckpoint,pendingWithCheckpoint);
  assert.equal(w.sourceReviewResumption.pendingWithoutCheckpoint,
    w.academicVerification.pendingCount-pendingWithCheckpoint);
  assert.equal(w.nextGradeAllowed,false);
});
