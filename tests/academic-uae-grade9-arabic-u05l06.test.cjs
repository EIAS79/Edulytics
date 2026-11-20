'use strict';
// Stage A-B-C authored tests only; do not execute before Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:05:06:GRAPHING-TWO-VARIABLE-INEQUALITIES';
test('G9 General 5-6 Arabic source half-plane lesson has worked graphs and rule',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l06-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===id),v=rich.Lessons.find(x=>x.LessonCode===id);
 assert.ok(p&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.match(p.Translations[0].Title,/ذوات متغيرين/);
 assert.equal(v.WorkedExamples.length,4);
 assert.match(v.WorkedExamples[0].Answer,/متقطع/);
});
test('Boundary conditions, student half-plane tasks and teacher rubric are present',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l06-two-variable-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l06-teacher-two-variable-inequalities-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/stroke-dasharray="9 7"/);
 assert.match(h,/fill="#cae7f4"/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="planeQ'+i+'"'),i);
 assert.match(h,/graded:false,submitted:false/);
 assert.equal(rub.answers.length,8);assert.equal(rub.autoGrade,false);
});
test('Coordinate rule assistance does not misrepresent half-plane drawing automation',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('half-plane shaded')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
});
test('Test-point and boundary inclusion fixtures distinguish graphing cases',()=>{
 const strict=(x,y)=>y<3*x-2;
 assert.equal(strict(0,0),false);assert.equal(strict(2,2),true);assert.equal(strict(1,1),false);
 const inclusive=(x,y)=>y>=-x+2;
 assert.equal(inclusive(0,0),false);assert.equal(inclusive(0,2),true);
 const vertical=(x,y)=>x<3;assert.equal(vertical(0,0),true);assert.equal(vertical(3,1),false);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-33-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
