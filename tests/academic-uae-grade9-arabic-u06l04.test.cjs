'use strict';
// Stage A-B-C authored source specifications. Execute only during Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const id='PED:UAE-MOE-MATH:L9:GENERAL:06:04:ELIMINATION-MULTIPLICATION';
test('Grade 9 6-4 Arabic source and RichV2 teach multiplying full equations',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l04-ar.rich-lesson-v2.json');
 const a=pack.Lessons.find(x=>x.LessonCode===id),q=rich.Lessons.find(x=>x.LessonCode===id);
 assert.ok(a&&q);assert.equal(pack.AcademicLanguage,'ar');assert.equal(q.Title,'الحذف باستخدام الضرب');
 assert.ok(q.WorkedExamples.length>=3);assert.match(a.Translations[0].Explanation,/ضرب إحدى المعادلتين أو كلتيهما/);
 assert.ok(q.WorkedExamples.some(x=>x.Answer.includes('١١٧')));
});
test('Source-specific eight teacher tasks and independent cooldown are not auto graded',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l04-elimination-multiplication-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l04-teacher-elimination-multiplication-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="answer'+i+'"'));
 for(let i=1;i<=3;i++)assert.ok(h.includes('id="cool'+i+'"'));
 assert.match(h,/graded:false,submitted:false/);assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.equal(rubric.answers.length,8);assert.equal(rubric.coolDownAnswers.length,3);assert.equal(rubric.autoGrade,false);
});
test('Existing linear engine only helps post-multiplication scalar step',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('ordered pair')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
 assert.ok(engine.includes('string.Equals(scopeKey, "'+id+'"'));
 assert.ok(engine.includes('بعد حذف أحد المتغيرين'));
});
test('Multiplication-elimination source fixtures yield verified ordered pairs',()=>{
 const [x,y]=[2,-3];assert.equal(5*x+6*y,-8);assert.equal(2*x+3*y,-5);
 const [a,z]=[1,2];assert.equal(4*a+2*z,8);assert.equal(3*a+3*z,9);
 const [speed,wind]=[117,13];assert.equal(4*speed+4*wind,520);assert.equal(5*speed-5*wind,520);
 const [u,v]=[-3,1];assert.equal(2*u+7*v,1);assert.equal(u+5*v,2);
 const [p,q]=[3,2];assert.equal(3*p+4*q,17);assert.equal(2*p-3*q,0);
 for(const t of [-4,-1,0,3])assert.equal(2*(t+2),2*t+4);
 for(const t of [-4,-1,0,3])assert.notEqual(2*t+4,2*(t+2)+2);
});
