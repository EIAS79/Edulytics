'use strict';
// Grade 9 Stage A-B-C test specifications only. Stage D runs them.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:08:05:SIMPLIFYING-RADICAL-EXPRESSIONS';

test('Grade 9 Arabic radical simplification content follows the General student book',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u08-l05-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);
 assert.match(p.SourceLocator,/02-1\.pdf printed 477/);
 assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'تبسيط التعابير الجذرية');
 for(const title of ['استخراج مربع كامل','الضرب داخل الجذور','إنطاق المقام الجذري','إنطاق المقام بالمرافق'])
  assert.ok(lesson.WorkedExamples.some(e=>e.Title===title),title);
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='استخراج مربع كامل').Answer,'٦√٣');
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='إنطاق المقام بالمرافق').Answer,'(٣−√٢)/٧');
 assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes('√(س²)=|س|')));
});

test('G9 U08L05 reuses exact surd coefficient engine without auto-grading rationalization proofs',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(row);
 assert.deepEqual(row.allowedQuestionFamilies,['supporting.surds.coefficient']);
 assert.ok(row.teacherReviewRequiredFor.includes('rationalization and conjugate denominator'));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const shared=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(shared.includes('supporting.surds.coefficient'));
 assert.ok(engine.includes('fingerprintNamespace.StartsWith("'+code+':"'));
 assert.match(engine,/بسّط الجذر: √\{problem\.Parameters\["radicand"\]\}/);
});

test('G9 radical Cool-down has all five teacher-review items and no false automatic submission',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u08-l05-radical-simplification-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=5;i++)assert.ok(html.includes('id="q'+i+'"'));
 for(const skill of ['√١٠٨','√١٨','٣/√٥','٣+√٢','√(س²)=س'])
  assert.ok(html.includes(skill),'missing Cool-down topic: '+skill);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics ولم تُحتسب درجة/);
});
