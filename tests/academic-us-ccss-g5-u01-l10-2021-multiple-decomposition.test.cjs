'use strict';
// Original first-edition 2021 G5 L10 composite source 95 and 168; WRITTEN_NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('L10 two decompositions and publisher-original exception are consistent',()=>{
 assert.equal(125-30,95);assert.equal(75+20,95);
 assert.equal(4*8*3+6*4*3,168);
 assert.equal(10*4*3+4*4*3,168);
 assert.equal(10*8*3-6*4*3,168);
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l10-abc-element-ledger-20261030.json');
 const review=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l10-abc-source-review-20261030.json');
 const cd=e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L10-CD-PUBLISHER-EXPRESSIONS');
 assert.equal(cd.Status,'SOURCE_UNAVAILABLE');assert.equal(cd.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(e.sourceCoverage100Percent,false);assert.equal(e.readyForStageD,true);assert.equal(review.readyForStageD,true);
});
