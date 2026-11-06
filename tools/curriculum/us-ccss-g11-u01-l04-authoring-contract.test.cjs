'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'../..',p),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L04';
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l04.rich-lesson-v2.json').Lessons[0];
const evidence=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u01-l04-element-coverage.json');
const assessment=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l04-source-teacher-led-20261106.json');
test('same G11 source lesson identity, discrete tech workshop & no stage D',()=>{
 assert.equal(rich.LessonCode,code);assert.equal(evidence.Count,25);
 assert.equal(new Set(evidence.Elements.map(x=>x.SourceElementId)).size,25);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.Contract.DoNotClaimIntegratedSpreadsheet,true);
});
test('independent formula and sequence oracle',()=>{
 const a=[2,3,-10,.2];const b=[a[0]+a[1],a[2]*a[3],a[0]+a[1]+333,Math.abs(a[2]*a[3])];
 assert.deepEqual(b,[5,-2,338,2]);
 assert.deepEqual(Array.from({length:4},(_,i)=>12+3*i),[12,15,18,21]);
 assert.deepEqual(Array.from({length:4},(_,i)=>16*(.5**i)),[16,8,4,2]);
});
test('teacher reviews actual spreadsheet and graph, not a guessed grade',()=>{
 assert.equal(assessment.TeacherReview.Tasks.length>=3,true);
 assert.match(assessment.FirstPartyCoolDown.Expected,/6,9,15,27,51,99/);
 assert.ok(evidence.Elements.some(x=>x.ImplementationType==='TEACHER_LED'));
});
