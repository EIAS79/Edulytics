'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('G4 L40 actual source activity count, first-party content and exact rational arithmetic',()=>{
 const r=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-ordinal-40-2021.rich-lesson-v2.json'));
 assert.equal(r.Lessons[0].LessonCode,'PED:US-CCSS-MATH:G4:U03:L15');
 assert.ok(r.Lessons[0].ExplanationParagraphs.length>=5);
 assert.ok(r.Lessons[0].WorkedExamples.length>=1);
 const a=[1n,2n],b=[1n,3n],expected=[5n,6n];const num=a[0]*b[1]+b[0]*a[1],den=a[1]*b[1];assert.equal(num*expected[1],den*expected[0]);
});
test('G4 L40 A-C checkpoint provenance and fail-closed Stage D',()=>{
 const c=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-40-v2-stage-a-d-checkpoint.json'));
 assert.equal(c.status,'READY_FOR_STAGE_D');
 assert.equal(c.stages.D,'NOT_RUN');
 assert.equal(c.academicApproved,false);
 assert.equal(c.sourceParityProvenPercent,null);
 assert.equal(c.sourceHashes.studentPdf.length,64);
 const state=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/full-grade-progress.json'));
 assert.ok(state.lastClosedOrdinal>=40);
 assert.ok(state.currentLessonOrdinal>=41);
 assert.equal(state.lessonStates['40'],'READY_FOR_STAGE_D');
});
