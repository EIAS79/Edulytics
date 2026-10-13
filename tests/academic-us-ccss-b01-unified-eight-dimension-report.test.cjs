#!/usr/bin/env node
'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { summarize, DIMENSIONS, CORRECT, REWORK, BLOCKED } =
  require('../tools/us-ccss-b01-unified-eight-dimension-report.cjs');
function fixture() {
  const manifest = {batchId:'B01', lessons:[]};
  const ledger={records:[]},bulk={evidenceMatrix:[]},
    runtime={lessonRows:[]},publisher={rows:[]},
    practice={rows:[]},rich={rows:[]},visual={rows:[]};
  for(let i=1;i<=500;i++) {
    const code=i===1?'PED:US-CCSS-MATH:G1:U01:L01':
      i===2?'PED:US-CCSS-MATH:G1:U01:L02':
      'PED:US-CCSS-MATH:G2:U01:L'+i;
    const h='a'.repeat(64);
    manifest.lessons.push({ordinal:i,lessonCode:code,title:'Lesson '+i,
      gradeOrCourse:'Grade 1',sourceUrl:'https://example.org/'+i,
      sourceSemanticSha256:h,officialOutcomeCodes:['CCSS:1.OA.C.5']});
    ledger.records.push({lessonCode:code,
      status:i===1?'INTERNALLY_CERTIFIED':'PENDING_ACADEMIC_REVIEW',
      academicDecision:i===1?'INTERNALLY_CERTIFIED':'NOT_CERTIFIED',
      contentSha256:h});
    bulk.evidenceMatrix.push({ordinal:i,lessonCode:code,screeningIssues:[]});
    runtime.lessonRows.push({ordinal:i,lessonCode:code,
      effectiveContentSha256:h,engineIssues:[],generatedCases:3});
    publisher.rows.push({ordinal:i,lessonCode:code,
      effectiveContentSha256:h,issues:[],verdict:'SOURCE_SCREENED_NOT_CERTIFIED'});
    practice.rows.push({ordinal:i,lessonCode:code,conflicts:[]});
    rich.rows.push({ordinal:i,lessonCode:code,status:'RICH_V2_COMPOSED',
      visualCount:1,workedExampleCount:2,explanationParagraphs:2,commonMistakes:1});
    visual.rows.push({ordinal:i,lessonCode:code,
      sameEffectiveContent:true,publisherCueVisualReviewTypes:[]});
  }
  return [manifest,ledger,bulk,runtime,publisher,practice,rich,visual];
}
test('joins all 500 and never treats screening as academic proof',()=>{
  const a=fixture(), report=summarize(...a);
  assert.deepEqual(report.coverage,{expected:500,joined:500,unique:500});
  assert.equal(report.counts[CORRECT],1);
  assert.equal(report.counts[REWORK],1);
  assert.equal(report.counts[BLOCKED],498);
  assert.equal(Object.keys(report.lessons[0].dimensions).length,DIMENSIONS.length);
  assert.ok(report.lessons[1].confirmedDefects.some(x=>x.code.includes('PREVIOUS_SOURCE')));
  assert.equal(report.batchDecision,'BATCH_NO-GO');
});
test('confirmed runtime generator defects must be NEEDS_REPAIR',()=>{
  const a=fixture();
  a[3].lessonRows[2].engineIssues=['NO_READY_VERIFIED_PRACTICE_CONTRACT'];
  const r=summarize(...a);
  assert.equal(r.lessons[2].verdict,REWORK);
  assert.equal(r.counts[REWORK],2);
});
test('historical pilot is not accepted if effective body SHA drifts',()=>{
  const a=fixture();
  a[3].lessonRows[0].effectiveContentSha256='b'.repeat(64);
  a[4].rows[0].effectiveContentSha256='b'.repeat(64);
  const r=summarize(...a);
  assert.equal(r.lessons[0].verdict,BLOCKED);
  assert.equal(r.lessons[0].historicCertification,'PR_488_SHA_RECHECK_REQUIRED');
});
test('publisher keyword heuristic and visual cue are not treated as proven errors',()=>{
  const a=fixture();
  a[4].rows[3].issues=['SOURCE_GOAL_INSTRUCTIONAL_ALIGNMENT_REVIEW'];
  a[7].rows[3].publisherCueVisualReviewTypes=['CLOCK_DIAGRAM'];
  const r=summarize(...a);
  assert.equal(r.lessons[3].verdict,BLOCKED);
  assert.equal(r.lessons[3].confirmedDefects.length,0);
  assert.ok(r.lessons[3].missingEvidenceOrReviewCues.length===2);
});
test('source join refuses missing evidence, duplicates and changed order',()=>{
  const a=fixture();
  a[6].rows.pop();
  assert.throws(()=>summarize(...a), /exactly 500/);
  const b=fixture();
  b[5].rows[2].ordinal=99;
  assert.throws(()=>summarize(...b), /ordinal mismatch/);
});
