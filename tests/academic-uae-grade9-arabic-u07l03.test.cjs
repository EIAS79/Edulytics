'use strict';
// Source-based Stage D specifications prepared in Stage A–B–C; not executed here.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:07:03:POLYNOMIAL-MULTIPLICATION';
test('7-3 Arabic content matches original General source and shows complete distributive multiplication',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);
 assert.match(lesson.SourceLocator,/02-1\.pdf printed382–387/);
 assert.equal(lesson.Translations[0].Title,'ضرب كثيرات الحدود');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l03-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons[0].LessonCode,code);
 assert.equal(rich.Lessons[0].WorkedExamples[0].Answer,'س²+٧س+١٢');
 assert.equal(rich.Lessons[0].WorkedExamples[2].Answer,'س³+٤س²+٥س+٢');
});
test('Shared exact kernel has product family with source-aligned teacher review',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(row.allowedQuestionFamilies,['uae.g9.general.u07.l03.multiply_coefficient']);
 assert.equal(row.officialOutcomeMapped,false);
 assert.ok(row.teacherReviewRequiredFor.some(x=>/whole simplified polynomial/.test(x)));
 const kernel=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const build=kernel.indexOf('private static ExactProblem BuildOriginalProblem(');
 const verify=kernel.indexOf('public static bool Verify(');
 const product=kernel.indexOf('if(family=="uae.g9.general.u07.l03.multiply_coefficient")');
 assert.ok(product>build&&build>verify,'Generator must be in build method, not verifier');
 assert.ok(kernel.includes('if (family=="uae.g9.general.u07.l03.multiply_coefficient")'));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
});
test('4-question source skill Cool-down retains manual grading for full products and drawings',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l03-polynomial-multiply-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(const id of ['cooldown','q1','q2','q3','q4','output'])assert.ok(h.includes('id="'+id+'"'));
 assert.match(h,/graded:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.match(h,/لم يتم إرسال الإجابات/);
});
