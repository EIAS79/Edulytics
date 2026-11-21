'use strict';
// Written during Grade9 Stage A–B–C; execute in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:07:05:DISTRIBUTIVE-FACTOR';
test('Original General textbook distributive/GCF lesson is authored in Arabic',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);assert.equal(lesson.Translations[0].Title,'استخدام خاصية التوزيع');
 assert.match(lesson.SourceLocator,/02-1\.pdf printed396–402/);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l05-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons[0].WorkedExamples[0].Answer,'٦س(٢س+٣)');
 assert.equal(rich.Lessons[0].WorkedExamples[1].Answer,'٥س(٣س−٢)');
 assert.equal(rich.Lessons[0].WorkedExamples[2].Answer,'{٠، ٧}');
});
test('Existing monomial numeric engine is reused, not mistaken for complete factoring autograder',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(mapping.allowedQuestionFamilies,['uae.g8.u01.l03.monomial_multiply','uae.g8.u01.l03.monomial_divide']);
 assert.ok(mapping.teacherReviewRequiredFor.some(x=>x.includes('factorization')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
});
test('Student Arabic factorization activity has Cool-down with manual marking',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l05-distributive-factor-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(const id of ['cooldown','q1','q2','q3','q4','output'])assert.ok(h.includes('id="'+id+'"'));
 assert.match(h,/graded:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
});
