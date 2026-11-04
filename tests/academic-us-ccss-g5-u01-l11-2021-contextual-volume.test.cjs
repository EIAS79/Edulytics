'use strict';
// First-edition 2021 G5 L11 teacher-source-context checks; authored NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('L11 source contexts and original publisher cooldown exception',()=>{
 assert.deepEqual([10*6*5,4*9*5,30*9,10*3*4+8*3*4],[300,180,270,216]);
 assert.equal((10+8)*3*4,216);
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l11-abc-element-ledger-20261030.json');
 const rev=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l11-abc-source-review-20261030.json');
 const cd=e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L11-CD-PUBLISHER-SANDBOX');
 assert.equal(cd.Status,'SOURCE_UNAVAILABLE');assert.equal(cd.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(e.sourceCoverage100Percent,false);assert.equal(rev.readyForStageD,true);assert.equal(e.readyForStageD,true);
});
