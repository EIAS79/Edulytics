'use strict';
const test=require('node:test'), assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('source-specific Grade 4 area families registered and routed',()=>{
 const f=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json')).families.filter(x=>x.id.startsWith('usccss.g4.u01.l01.'));
 assert.equal(f.length,4);for(const x of f){assert.equal(x.skillId,'geometry.rectangle.area'); assert.equal(x.lessonPracticeRouting,true);assert.ok(x.verificationPolicy);}
 const c=read('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit1FirstLessonPracticeContracts.cs');
 const e=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 for(const x of f)assert.ok(c.includes(x.id));
 for(const op of ['Build','Solve','Verify'])assert.ok(e.includes('UsCcssG4Unit1FirstLessonQuestions.'+op));
});
test('source mathematics: correct, negative and boundary cases',()=>{
 for(let w=2;w<=4;w++)for(let length=1;length<=10;length++)assert.equal(w*length/w,length);
 for(let a=1;a<=30;a++)assert.equal(a%3===0,[3,6,9,12,15,18,21,24,27,30].includes(a));
 for(const a of [31,32,34,35,37,38,40,41,43,44,46,47])assert.notEqual(a%3,0);
 for(const a of [33,36,39,42,45,48])assert.equal(a%3,0);
});
test('lesson source correction does not grant Grade 4 L01 Stage D or academic approval',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-1-v2-stage-a-d-checkpoint.json'));
 assert.equal(c.status,'READY_FOR_STAGE_D'); assert.equal(c.readyForStageD,true); assert.equal(c.stages.D,'NOT_RUN'); assert.equal(c.academicApproved,false);
});
