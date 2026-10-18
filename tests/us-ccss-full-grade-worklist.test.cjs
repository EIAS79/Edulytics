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
  assert.equal(w.status,'NO_GO');
});
test('same one-grade queue persists beyond 5/10/30 without synthetic acceptance',()=>{
  const w=buildWorklist(manifest,copy(),{remainingOnly:true});
  assert.equal(w.items.length,96);
  assert.equal(w.items[5].ordinal,56);
  assert.equal(w.items[30].ordinal,81);
  assert.equal(w.items[95].ordinal,146);
  assert.equal(w.acceptedNew,0);
  assert.equal(w.firstUnfinishedOrdinal,51);
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
  assert.equal(buildWorklist(manifest,progress).firstUnfinishedOrdinal,51);
  progress.nextOrdinal=81;
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
});
