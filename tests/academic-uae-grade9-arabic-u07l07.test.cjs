'use strict';// Stage A–B–C prepared specifications; do not run until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson='PED:UAE-MOE-MATH:L9:GENERAL:07:07:NONMONIC-QUADRATIC-FACTORING';
test('Original General lesson 46 is Arabic and source traced',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const row=pack.Lessons.find(x=>x.LessonCode===lesson);assert.ok(row);
 assert.match(row.SourceLocator,/02-1\.pdf printed 412–417/);
 assert.equal(row.Translations.find(x=>x.CultureCode==='ar').Title,'حل معادلات من الصورة أ س²+ب س+ج=٠');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l07-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons.find(x=>x.LessonCode===lesson).WorkedExamples.length,3);
});
test('Grade 9 nonmonic skill reuses extended exact quadratic engine',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===lesson);
 assert.deepEqual(mapping.allowedQuestionFamilies,['uae.g9.general.u07.l07.nonmonic_integer_roots']);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+lesson+'"]'));
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/UaeG9MonicQuadraticQuestions.cs'),'utf8');
 assert.match(engine,/NonMonic = "uae.g9.general.u07.l07.nonmonic_integer_roots"/);
 for(const method of ['Supports','Build','Solve','Verify'])assert.ok(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8').includes('UaeG9MonicQuadraticQuestions.'+method+'('));
});
test('Source-based A-C teacher cooldown includes fractional roots and no false autograding',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l07-nonmonic-factor-ar.html'),'utf8');
 for(const part of ['lang="ar" dir="rtl"','٦س²+١١س+٣=٠','٢س²−٥س+٢=٠','id="teacherReview"','id="submission"',"graded:false","TEACHER_REVIEW_REQUIRED"])assert.ok(html.includes(part),part);
});
