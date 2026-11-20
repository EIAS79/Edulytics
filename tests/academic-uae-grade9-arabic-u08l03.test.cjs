'use strict';
// A-B-C authoring specifications. Stage D runs the tests; do not execute in A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:08:03:RATIONAL-EXPONENTS';
test('UAE Grade 9 U08L03 Arabic book-based lesson and exact rational exponent authoring',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u08-l03-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);assert.match(p.SourceLocator,/02-1\.pdf printed 460/);
 assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'الأسس النسبية');
 assert.equal(lesson.CultureCode,'ar');
 for(const title of ['جذر تكعيبي','أس كسري','أس كسري سالب','الجذر الفردي للعدد السالب','الجذر الزوجي للعدد السالب'])
  assert.ok(lesson.WorkedExamples.some(x=>x.Title===title),title);
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='أس كسري سالب').Answer,'١/٨');
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='الجذر الفردي للعدد السالب').Answer,'−٢');
 assert.ok(lesson.WorkedExamples.find(x=>x.Title==='الجذر الزوجي للعدد السالب').Answer.includes('غير معرف'));
});
test('Rational exponent numeric families are connected to both mapping and existing verifier',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const v=map.mappings.find(x=>x.lessonCode===code);
 const families=['uae.g9.general.u08.l03.rational_positive_exact','uae.g9.general.u08.l03.rational_negative_exact'];
 assert.ok(v);
 assert.deepEqual(v.allowedQuestionFamilies,families);
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const impl=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/UaeG9RationalExponentQuestions.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 for(const f of families)assert.ok(c.includes(f)&&impl.includes(f));
 for(const op of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UaeG9RationalExponentQuestions.'+op+'('));
 assert.match(impl,/denominator is >= 2 and <= 4/);
 assert.match(impl,/1m\/expected/);
});
test('U08L03 Cool-down has five Arabic teacher-reviewed items and no false submission',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u08-l03-rational-exponents-ar.html'),'utf8');
 assert.match(h,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=5;i++)assert.ok(h.includes('id="a'+i+'"'));
 assert.match(h,/id="cooldownForm"/);
 assert.match(h,/id="record" readonly/);
 assert.match(h,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(h,/لم تُرسل البيانات إلى Edulytics ولم تُحتسب درجة/);
});
