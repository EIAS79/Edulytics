'use strict';
// Stage A-B-C 2021 Grade 5 Unit 1 optional last lesson; authored, NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('2021 optional Grade5 L12 has NO original publisher cooldown, photo rights retained',()=>{
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l12-abc-element-ledger-20261030.json');
 const review=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l12-abc-source-review-20261030.json');
 assert.equal(e.abcRevalidation20261102.originalPublisherCoolDownExists,false);
 assert.equal(e.sourceElements.find(x=>x.SourceElementId==='G5-U01-L12-PHOTO-RIGHTS').Status,'RIGHTS_RESTRICTED');
 assert.equal(e.sourceCoverage100Percent,false);assert.equal(review.readyForStageD,true);assert.equal(e.readyForStageD,true);
 assert.deepEqual([[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]].map(a=>a.reduce((b,c)=>b*c,1)),[60,60,60,60,60]);
});
test('U01 revalidation closed sequentially without claiming Stage D acceptance',()=>{
 const g=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-abc-lesson-ledger-v1.json');
 assert.equal(g.grade5FirstEdition2021Revalidation.unit01.sequentialAbcRevalidated,12);
 assert.equal(g.grade5FirstEdition2021Revalidation.grade.lessonsRevalidatedThisPass,12);
 assert.equal(g.grade5FirstEdition2021Revalidation.stageD,'NOT_RUN');
 assert.equal(g.lessons.slice(0,12).every(x=>x.READY_FOR_STAGE_D),true);
});
