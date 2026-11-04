'use strict';
// Grade5 L06 source-equivalent original first-party cards; authored, NOT_RUN during ABC.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.join(__dirname,'..',p),'utf8'));
test('Original Edulytiks prism cards are sound geometrical expressions',()=>{
 const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
 const l=r.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L06');
 const v=l.Visuals.find(x=>x.Title==='Edulytiks-original six prism/expression cards — first-party matching set');
 assert.ok(v);assert.equal(v.Rows.length,7);
 assert.deepEqual([[6,5,3],[6,3,4],[7,4,3],[4,5,2],[3,4,5],[2,3,5]].map(v=>v.reduce((x,y)=>x*y,1)),[90,72,84,40,60,30]);
 assert.match(v.Description,/NOT the original publisher/i);
});
test('Unverified publisher original masters remain in source denominator while approved functional substitutes close ABC',()=>{
 const e=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l06-abc-element-ledger-20261030.json');
 assert.equal(e.sourceElementsCount,21);assert.equal(e.sourceElementsAccepted,19);assert.equal(e.sourceCoverage100Percent,false);
 for(const id of ['G5-U01-L06-A1-PUBLISHER-BLACKLINE','G5-U01-L06-CD-PUBLISHER-CHO0SE-EXPRESSION']){
 const x=e.sourceElements.find(v=>v.SourceElementId===id);
 assert.equal(x.Status,'SOURCE_UNAVAILABLE');assert.equal(x.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 }
 assert.equal(e.status,'READY_FOR_STAGE_D');assert.equal(e.revalidation20261102.stageD,'NOT_RUN');
});
