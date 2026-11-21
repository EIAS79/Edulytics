'use strict';
// A–B–C prepared regression specifications. Do not run before Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:08:04:SCIENTIFIC-NOTATION';

test('Grade 9 General scientific notation is authored in the existing Arabic pack and Rich V2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u08-l04-ar.rich-lesson-v2.json');
 const row=pack.Lessons.find(x=>x.LessonCode===code);
 const lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(row&&lesson);
 assert.equal(row.Translations.find(x=>x.CultureCode==='ar').Title,'الترميز العلمي');
 assert.match(row.SourceLocator,/02-1\.pdf printed 468/);
 assert.equal(lesson.CultureCode,'ar');
 assert.equal(lesson.WorkedExamples.length,4);
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='ضرب ثم تطبيع').Answer,'١٫٢×١٠^٨');
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='عدد صغير بأس سالب').Answer,'٣٫٢×١٠^(−٤)');
 assert.ok(lesson.Visuals.length>=2);
});

test('U08L04 reuses exact power-of-ten families and localizes only this lesson scope',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 const families=['supporting.standard_form.power10_exponent','supporting.powers10.multiply','supporting.powers10.divide'];
 assert.ok(row);
 assert.deepEqual(row.allowedQuestionFamilies,families);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const reused=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 for(const family of families)assert.ok(contract.includes(family)&&reused.includes(family),family);
 assert.ok(engine.includes('fingerprintNamespace.StartsWith("'+code+':"'));
 assert.match(engine,/أكمل أس العشرة ن في الترميز العلمي/);
 assert.match(engine,/أوجد الصورة العددية للقيمة/);
 assert.match(engine,/أوجد قيمة \{problem\.Parameters\["dividend"\]\}/);
});

test('Scientific notation Cool-down is Arabic RTL teacher-review only with negative-exponent and normalization tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u08-l04-scientific-notation-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=5;i++)assert.ok(html.includes('id="q'+i+'"'),'question '+i);
 assert.match(html,/id="cooldownForm"/);
 assert.match(html,/١٢×١٠\^٥/);
 assert.match(html,/١٠\^\(−٤\)/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics ولم تُحتسب درجة/);
});
