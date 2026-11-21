'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:13:08:TRIANGLES-AND-COORDINATE-PROOF';
test('U13L08 coordinate proof lesson is source grounded, Arabic and contains correct distance reasoning',()=>{
const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u13-l08-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
assert.ok(p&&q);assert.match(p.SourceLocator,/03-1\.pdf.*783/);assert.equal(q.CultureCode,'ar');
assert.ok(q.KeyConcepts.some(x=>x.Title.includes('المسافة')));
assert.ok(q.WorkedExamples.some(x=>x.Answer.includes('√٥٢')));
const A=[-4,0],B=[4,0],C=[0,6];
const squared=(p,q)=>(p[0]-q[0])**2+(p[1]-q[1])**2;
assert.equal(squared(A,C),52);assert.equal(squared(B,C),52);assert.deepEqual([(A[0]+B[0])/2,(A[1]+B[1])/2],[0,0]);
});
test('Teacher reviews coordinate proof; not routed to an unrelated auto grader',()=>{
const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);assert.ok(map);
assert.deepEqual(map.allowedQuestionFamilies,[]);assert.equal(map.practiceReadiness,'TEACHER_REVIEW_ONLY_STAGE_D_ENGINE_DECISION_PENDING');
const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u13-l08-coordinate-proof-ar.html'),'utf8');
assert.match(html,/lang="ar" dir="rtl"/);for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
assert.match(html,/graded:false,submitted:false/);assert.match(html,/TEACHER_REVIEW_REQUIRED/);
});
