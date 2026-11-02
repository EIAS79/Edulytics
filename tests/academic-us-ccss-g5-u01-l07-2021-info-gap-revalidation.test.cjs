'use strict';
// 2021 Grade 5 L07 A-B-C source and selected practice guard; authored, NOT RUN in ABC.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('L07 information gap and independent cooldown stay teacher-led without copied publisher prompt',()=>{
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l07-abc-element-ledger-20261030.json');
 assert.equal(e.sourceElementsCount,17);assert.equal(e.sourceElementsAccepted,16);
 assert.equal(e.sourceCoverage100Percent,false);assert.equal(e.status,'READY_FOR_STAGE_D');
 const card=e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L07-A02-INFOGAP-CARDS');
 assert.equal(card.Status,'MATCHED_TEACHER_LED');
 const cd=e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L07-CD-PUBLISHER-FIND-VOLUME');
 assert.equal(cd.Status,'SOURCE_UNAVAILABLE');
 assert.equal(cd.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 const rub=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l07-practice-rubric-v15.json');
 assert.match(rub.selectedRuntimePractice,/RUNTIME_STAGE_D_NOT_RUN/);
 assert.equal(4*2*3,24);
});
