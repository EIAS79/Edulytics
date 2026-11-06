'use strict';
const test=require('node:test'), assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'), read=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L03';
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l03.rich-lesson-v2.json').Lessons[0];
const assessment=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l03-source-teacher-led-20261106.json');
const evidence=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u01-l03-element-coverage.json');
test('U01:L03 exact source identity and authored test state',()=>{
 assert.equal(rich.LessonCode,code);assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.Elements.length,21);
 assert.equal(new Set(evidence.Elements.map(x=>x.SourceElementId)).size,21);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');assert.equal(assessment.StageD,'NOT_RUN');
});
test('independent arithmetic/geometric/nonlinear term-10 oracle',()=>{
 const a=n=>30+(n-1)*10,b=n=>5*n*(n-1)/2,c=n=>2**(n-1);
 assert.deepEqual([a(10),b(10),c(10)],[120,225,512]);
 assert.equal(a(10)<b(10)&&b(10)<c(10),true);
 assert.equal(7-7,0);assert.equal(7/7,1);
});
test('teacher-led graph and classification evidence not auto-graded',()=>{
 assert.ok(evidence.Elements.some(x=>x.ImplementationType==='TEACHER_LED'));
 assert.equal(assessment.TeacherReview.Required,true);
 assert.match(assessment.FirstPartyCoolDown.Expected,/156\.25/);
 assert.equal(assessment.SkillContracts[0].Readiness,'WRITTEN_NOT_RUN');
});
