'use strict';
// Prepared Arabic and engine contract checks for Stage D; not executed in Stage A–B–C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:07:04:SPECIAL-PRODUCTS';
test('U07L04 Arabic book index and RichV2 preserve square and conjugate source identities',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const x=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(x);assert.equal(x.Translations[0].Title,'نواتج الضرب الخاصة');
 assert.match(x.SourceLocator,/02-1\.pdf printed388–393/);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l04-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons[0].LessonCode,code);
 assert.equal(rich.Lessons[0].WorkedExamples[0].Answer,'س²+٨س+١٦');
 assert.equal(rich.Lessons[0].WorkedExamples[2].Answer,'٢٥س²−٩');
});
test('Two special product numeric families reuse existing exact kernel and leave full models to teacher',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=map.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(row.allowedQuestionFamilies,['uae.g9.general.u07.l04.square_binomial_coefficient','uae.g9.general.u07.l04.conjugate_product_coefficient']);
 const kernel=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const start=kernel.indexOf('private static ExactProblem BuildOriginalProblem(');
 const gen=kernel.indexOf('if(family is "uae.g9.general.u07.l04.square_binomial_coefficient"',start);
 assert.ok(gen>start);
 const practice=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(practice.includes('byLesson["'+code+'"]'));
 assert.ok(row.teacherReviewRequiredFor.some(x=>x.includes('area')));
});
test('Original model and four-question teacher Cool-down provide no autograding claim',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l04-special-products-ar.html'),'utf8');
 assert.match(html,/lang="ar" dir="rtl"/);assert.match(html,/role="img"/);
 for(const id of ['cooldown','q1','q2','q3','q4','output'])assert.ok(html.includes('id="'+id+'"'));
 assert.match(html,/graded:false/);assert.match(html,/TEACHER_REVIEW_REQUIRED/);
});
