'use strict';
// Source-bound A-B-C acceptance specs; tests are for Stage D execution.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:06:03:ELIMINATION-ADD-SUBTRACT';
test('Grade9 General 6-3 Arabic pack and RichV2 match elimination source',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l03-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===id),q=rich.Lessons.find(x=>x.LessonCode===id);
 assert.ok(p&&q);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(q.Title,'الحذف باستخدام الجمع والطرح');
 assert.ok(q.WorkedExamples.length>=3);
 assert.ok(q.ExplanationParagraphs.some(s=>s.includes('طرح')));
 assert.ok(p.Translations.find(x=>x.CultureCode==='ar').CommonMistakes.includes('طرح'));
});
test('Teacher exercises and cooldown are independently exportable without false grading',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l03-elimination-add-sub-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l03-teacher-elimination-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="answer'+i+'"'));
 for(let i=1;i<=3;i++)assert.ok(html.includes('id="cool'+i+'"'));
 assert.match(html,/graded:false,submitted:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.equal(rubric.answers.length,8);assert.equal(rubric.coolDownAnswers.length,3);
 assert.equal(rubric.autoGrade,false);
});
test('Exact existing linear solver is only a numerical substep, not a system autograder',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('ordered-pair')));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+id+'"]'));
 assert.ok(engine.includes('string.Equals(scopeKey, "'+id+'"'));
 assert.ok(engine.includes('بعد حذف أحد المتغيرين'));
});
test('Book source elimination fixtures satisfy both original equations',()=>{
 const [x,y]=[5,2];assert.equal(4*x+6*y,32);assert.equal(3*x-6*y,3);
 const [r,t]=[4,-7];assert.equal(9*r+2*t,22);assert.equal(5*r+2*t,6);
 const [v,c]=[1.75,-1];assert.equal(8*v+3*c,11);assert.equal(8*v+7*c,7);
 const [a,b]=[2,-3];assert.equal(5*a+6*b,-8);assert.equal(2*a+3*b,-5);
 assert.equal(850*22+750*15,29950);assert.equal(850*22+1500*15,41200);
 const [f,g]=[-3,5];assert.equal(7*f+3*g,-6);assert.equal(7*f-2*g,-31);
 for(const z of [-2,0,3])assert.equal(-2*z+(2*z+1),1);
});
