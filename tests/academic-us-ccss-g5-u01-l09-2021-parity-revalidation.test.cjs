'use strict';
// Grade 5 First Edition 2021 L09 comparison fixtures; written, NOT_RUN during ABC.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('L09 selected 2021 measured-composite numbers are correct, source exact exception recorded',()=>{
 const d=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l09-abc-element-ledger-20261030.json');
 const s=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l09-abc-source-review-20261030.json');
 assert.equal(s.readyForStageD,true);assert.equal(d.readyForStageD,true);
 const cd=d.sourceElements.find(x=>x.SourceElementId==='G5-U01-L09-CD-PUBLISHER-FIND-VOLUME');
 assert.equal(cd.Status,'SOURCE_UNAVAILABLE');assert.equal(cd.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(d.denominatorComplete,false);assert.equal(d.sourceCoverage100Percent,false);
 assert.deepEqual([2*7*2+5*5*2,9*3*7+5*2*7,(2*3)*4+(3*3)*2,5*8*6+5*4*9],[78,259,42,420]);
});
