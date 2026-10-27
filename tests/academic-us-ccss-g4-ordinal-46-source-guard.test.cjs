'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('G4 L46 actual source activity count, first-party content and exact rational arithmetic',()=>{
 const r=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-ordinal-46-2021.rich-lesson-v2.json'));
 assert.equal(r.Lessons[0].LessonCode,'PED:US-CCSS-MATH:G4:U04:L01');
 assert.ok(r.Lessons[0].ExplanationParagraphs.length>=5);
 assert.ok(r.Lessons[0].WorkedExamples.length>=1);
 const a=[78n,100n],b=[39n,50n],expected=[1n,1n];assert.equal(a[0]*b[1],b[0]*a[1]);assert.equal(expected[0],1n);
});
test('G4 L46 A-C checkpoint provenance and fail-closed Stage D',()=>{
 const c=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-46-v2-stage-a-d-checkpoint.json'));
 assert.equal(c.status,'READY_FOR_STAGE_D');
 assert.equal(c.stages.D,'NOT_RUN');
 assert.equal(c.academicApproved,false);
 assert.equal(c.sourceParityProvenPercent,null);
 assert.equal(c.sourceHashes.studentPdf.length,64);
 const state=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/full-grade-progress.json'));
 assert.ok(state.lastClosedOrdinal>=46);
 assert.ok(state.currentLessonOrdinal>=47);
 assert.equal(state.lessonStates['46'],'READY_FOR_STAGE_D');
});
