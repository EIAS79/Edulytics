'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01';
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l01.rich-lesson-v2.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const assess=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l01-source-teacher-led-20261106.json');
const elems=read('artifacts/curriculum-delivery/US-CCSS-MATH/G11/u01-l01-element-coverage.json');
test('preserves exact U01 L01 identity and unit goals',()=>{
 assert.equal(rich.Lessons.length,1); assert.equal(rich.Lessons[0].LessonCode,code);
 assert.equal(pack.lessons.filter(l=>l.lessonCode===code).length,1);
 assert.match(rich.Lessons[0].ExplanationParagraphs.join(' '),/recurren/i);
 assert.equal(elems.LessonCode,code);
});
test('numeric oracle: recurrence minimum and inverse terms',()=>{
 const move=n=>n===1?1:2*move(n-1)+1;
 assert.deepEqual([1,2,3,4,5,6,7].map(move),[1,3,7,15,31,63,127]);
 assert.equal(move(7),2**7-1);
 const x=[2];for(let i=0;i<4;i++)x.push(-2*x.at(-1)+3);
 assert.deepEqual(x,[2,-1,5,-7,17]);
});
test('source element map and grader boundary',()=>{
 assert.equal(elems.Elements.length,24);
 assert.equal(new Set(elems.Elements.map(e=>e.SourceElementId)).size,17);
 assert.ok(elems.Elements.some(e=>e.ImplementationType==='TEACHER_LED'));
 assert.ok(elems.Elements.some(e=>e.ImplementationType==='DIGITAL'));
 assert.equal(assess.TeacherReview.Required,true);
 assert.equal(assess.StageD,'NOT_RUN');
 assert.equal(assess.TestsExecuted,'NOT_RUN');
});
test('independent cooldown and no false verified digital grading',()=>{
 assert.match(assess.IndependentCoolDown.Prompt,/six-disc/i);
 assert.match(assess.IndependentCoolDown.Expected,/63/);
 assert.equal(assess.IndependentCoolDown.Origin,'FIRST_PARTY_AUTHORED_NOT_PUBLISHER_COOLDOWN');
 assert.ok(assess.CompetencyContracts.some(c=>c.capabilityReadiness==='WRITTEN_NOT_RUN'));
 assert.ok(assess.CompetencyContracts[0].allowedQuestionFamilies.length===3);
 assert.equal(assess.TeachingPerformance,'STAGE_D_NOT_RUN');
});
