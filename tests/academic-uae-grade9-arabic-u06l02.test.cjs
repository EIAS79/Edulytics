'use strict';
// A-B-C prepared targeted specifications; execute in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:06:02:SUBSTITUTION-SYSTEMS';
test('Existing Grade9 Arabic pack and new RichV2 agree with original substitution objectives',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l02-ar.rich-lesson-v2.json');
 const lesson=p.Lessons.find(x=>x.LessonCode===id);
 assert.ok(lesson && lesson.Translations.some(x=>x.CultureCode==='ar'));
 assert.equal(p.AcademicLanguage,'ar');
 const a=rich.Lessons.find(x=>x.LessonCode===id);
 assert.ok(a);assert.equal(a.Title,'التعويض');assert.ok(a.WorkedExamples.length>=4);
 assert.ok(a.WorkedExamples.some(x=>x.Answer.includes('−٢،−٣')));
 assert.ok(a.WorkedExamples.some(x=>x.Answer.includes('−١')));
 assert.ok(a.WorkedExamples.some(x=>x.Answer.includes('٥٣')));
 assert.match(lesson.Translations[0].Explanation,/المعادلتين الأصليتين/);
});
test('Teacher activity covers unique, contradictory, and dependent source systems',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l02-substitution-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l02-teacher-substitution-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="answer'+i+'"'),'missing source task '+i);
 for(let i=1;i<=3;i++)assert.ok(h.includes('id="cool'+i+'"'),'missing cooldown '+i);
 assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.match(h,/لم تُرسل الإجابات/);
 assert.equal(rubric.answers.length,8);assert.equal(rubric.coolDownAnswers.length,3);
 assert.equal(rubric.autoGrade,false);
});
test('Reuse existing single-variable exact solver only, with Arabic prompt scoped to Grade9 lesson',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.variables_both_sides']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('verify both')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
 assert.ok(engine.includes('string.Equals(scopeKey, "'+id+'"'));
 assert.ok(engine.includes('بعد التعويض في نظام معادلتين'));
 assert.match(engine,/problem = problem with/);
});
test('Independent source substitution arithmetic is consistent',()=>{
 assert.equal(3*(-2)+(-3),-9);
 assert.equal(2*(-2)+1,-3);
 assert.equal(8+2*(-1),6);
 assert.equal(3*8-4*(-1),28);
 assert.equal(-13-(-21),8);
 assert.equal(3*(-13)-2*(-21),3);
 assert.equal(10495*53+1895*72,692675); // use integer fils to avoid floating-point rounding
 for(const x of [-3,-1,0,4])assert.equal(-6*x+3*(2*x-4),-12);
 for(const x of [-3,-1,0,4])assert.notEqual(2*(2*x+1),4*x+6);
});
