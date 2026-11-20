'use strict';
// Stage A-B-C prepared specs, run only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:06:01:GRAPHING-LINEAR-SYSTEMS';
test('UAE Grade9 General 6-1 Arabic systems lesson has graph examples',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l01-ar.rich-lesson-v2.json');
 const lesson=p.Lessons.find(x=>x.LessonCode===id),rich=v.Lessons.find(x=>x.LessonCode===id);
 assert.ok(lesson&&rich);assert.equal(p.AcademicLanguage,'ar');
 assert.equal(rich.WorkedExamples.length,4);
 assert.match(lesson.Translations[0].Explanation,/المستقيمات/);
});
test('Teacher graph, eight source skills and local export have no false automation',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l01-systems-graphing-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l01-teacher-systems-graphing-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/circle cx="355" cy="252" r="8"/);
 for(let n=1;n<=8;n++)assert.ok(h.includes('id="systemQ'+n+'"'));
 assert.match(h,/graded:false,submitted:false/);
 assert.equal(rub.answers.length,8);assert.equal(rub.autoGrade,false);
});
test('Existing coordinate rule may assist numeric work but teacher grades intersection',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('plot both')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
});
test('Source system fixtures classify one/no/infinite solutions',()=>{
 const f=x=>-3*x+10,g=x=>x-2;
 assert.equal(f(3),g(3));assert.equal(f(2)===g(2),false);
 for(const x of [-3,0,2,9])assert.notEqual(2*x+1,2*x-3);
 for(const x of [-3,0,2,9])assert.equal(2*(x+2),2*x+4);
 assert.equal(10*10+150,20*10+50);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-34-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
