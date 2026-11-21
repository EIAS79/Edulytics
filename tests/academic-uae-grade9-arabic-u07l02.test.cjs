'use strict';
// A–B–C prepared tests; Stage D runs them.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:07:02:MONOMIAL-TIMES-POLYNOMIAL';
test('7-2 is an original Arabic General student-book lesson and not rebuilt as a different pathway',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);assert.equal(lesson.Translations[0].CultureCode,'ar');
 assert.equal(lesson.Translations[0].Title,'ضرب كثيرة حدود في حد');
 assert.match(lesson.SourceLocator,/02-1\.pdf printed374–379/);
});
test('7-2 content preserves correct distribution and negative-factor examples',()=>{
 const a=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l02-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(a.LessonCode,code);
 assert.match(a.WorkedExamples[0].Answer,/٦س³−١٢س²\+١٥س/);
 assert.match(a.WorkedExamples[1].Answer,/−٦س³\+١٠س²/);
 assert.match(a.WorkedExamples[2].Answer,/٢س²\+١٢س/);
 assert.match(a.SummaryPoints.join(' '),/المعلم/);
});
test('Numeric monomial engine is reused while full distributed polynomial remains teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const x=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(x.allowedQuestionFamilies,['uae.g8.u01.l03.monomial_multiply']);
 assert.ok(x.teacherReviewRequiredFor.some(x=>x.includes('polynomial term')||x.includes('polynomial')));
 assert.equal(x.officialOutcomeMapped,false);
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
});
test('Cooldown has four teacher-reviewed Arabic tasks and no false grading',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l02-monomial-polynomial-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(const id of ['q1','q2','q3','q4','cooldown','output'])assert.ok(h.includes('id="'+id+'"'));
 assert.match(h,/graded:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.match(h,/لم تُرسل البيانات/);
});
