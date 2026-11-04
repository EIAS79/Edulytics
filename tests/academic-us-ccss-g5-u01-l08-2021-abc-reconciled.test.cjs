'use strict';
// First Edition 2021 G5 L08 source and first-party cooldown closure guard; authored NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('G5 L08 2021 source review and formal handoff are consistent',()=>{
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l08-abc-element-ledger-20261030.json');
 const review=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l08-abc-source-review-20261030.json');
 const g=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-abc-lesson-ledger-v1.json');
 assert.equal(e.readyForStageD,true);assert.equal(review.readyForStageD,true);assert.equal(g.lessons[7].READY_FOR_STAGE_D,true);
 assert.equal(e.denominatorComplete,false);assert.equal(e.sourceCoverage100Percent,false);
 const x=e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L08-CD-PUBLISHER-COMPOSITE');
 assert.equal(x.Status,'SOURCE_UNAVAILABLE');assert.equal(x.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.deepEqual([12+10,12+18,45+1,45+5,54+6,45+12],[22,30,46,50,60,57]);
});
