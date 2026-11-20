'use strict';
// Stage A-B-C authored targeted specs. Execute in Stage D, not here.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const id='PED:UAE-MOE-MATH:L9:GENERAL:06:05:LINEAR-SYSTEMS-APPLICATIONS';
test('UAE G9 General 6-5 Arabic pack and RichV2 model real-world systems',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l05-ar.rich-lesson-v2.json');
 const a=p.Lessons.find(x=>x.LessonCode===id),b=v.Lessons.find(x=>x.LessonCode===id);
 assert.ok(a&&b);assert.equal(p.AcademicLanguage,'ar');assert.equal(b.Title,'تطبيق أنظمة المعادلات الخطية');
 assert.ok(b.WorkedExamples.some(x=>x.Answer.includes('١٢٠')));
 assert.ok(b.KeyConcepts.some(x=>x.Title.includes('اختيار')));
 assert.match(a.Translations[0].Explanation,/المتغيرين/);
});
test('Eight independent teacher tasks and cooldown require human interpretation',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l05-systems-applications-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l05-teacher-systems-applications-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="answer'+i+'"'));
 for(let i=1;i<=3;i++)assert.ok(h.includes('id="cool'+i+'"'));
 assert.match(h,/graded:false,submitted:false/);assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.equal(rubric.answers.length,8);assert.equal(rubric.coolDownAnswers.length,3);assert.equal(rubric.autoGrade,false);
});
test('Only existing one-variable family assists system solving; methods are teacher assessed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('model two independent')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const e=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));assert.ok(e.includes('string.Equals(scopeKey, "'+id+'"'));
});
test('Source penguin and method-selection fixtures satisfy both original equations',()=>{
 const [p,g]=[120,49];assert.equal(p+g,169);assert.equal(p,22+2*g);
 const [x,y]=[-3,-5];assert.equal(4*x-4*y,8);assert.equal(-8*x+y,19);
 const [a,b]=[3,2];assert.equal(2*a+3*b,12);assert.equal(4*a-3*b,6);
 const [c,d]=[-2,-3];assert.equal(d,2*c+1);assert.equal(3*c+d,-9);
 assert.equal(55+25,80);assert.equal(55,5+2*25);
});
