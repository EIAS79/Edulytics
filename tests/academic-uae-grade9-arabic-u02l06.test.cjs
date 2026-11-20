'use strict';
// Stage A-C authored specifications; do NOT execute until Stage D.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = file => JSON.parse(fs.readFileSync(path.join(root, file),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:02:06:RATIOS-AND-PROPORTIONS';

test('Grade 9 General student book ratios and proportions appears in existing Arabic pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u02-l06-ar.rich-lesson-v2.json');
 assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(pack.Lessons.filter(x=>x.LessonCode===code).length,1);
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.equal(lesson.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U02:L06');
 assert.ok(lesson.SourceLocator.includes('01.pdf'));
 assert.equal(lesson.Translations[0].CultureCode,'ar');
 assert.match(lesson.Translations[0].WorkedExamples,/س\/١٠=٣\/٥/);
 const ar=rich.Lessons.find(x=>x.LessonCode===code);
 assert.equal(ar.CultureCode,'ar');
 assert.equal(ar.WorkedExamples.length,4);
 assert.ok(ar.KeyConcepts.some(x=>x.Title==='خاصية الضرب التبادلي'));
 assert.ok(ar.WorkedExamples.some(x=>x.Answer==='س=٦'));
 assert.ok(ar.Visuals.some(x=>x.Kind==='CalculationChain'));
});

test('Only existing reusable exact ratio families are mapped to Grade 9, no foreign language prompt leakage',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const lesson=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(lesson.allowedQuestionFamilies,[
 'ratio.unit_rate.equivalent_ratio','ratio.unit_rate.direct','ratio.unit_rate.divide_total']);
 assert.equal(lesson.officialOutcomeMapped,false);
 assert.equal(lesson.practiceReadiness,'PREPARED_STAGE_D_NOT_RUN');
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('string.Equals(scopeKey,'));
 assert.ok(engine.includes('ratio["baseFirst"]'));
 assert.ok(engine.includes('حل التناسب'));
 assert.ok(engine.includes('يوزَّع مجموع'));
});

test('Original ratio bar and six cool-down tasks remain teacher reviewed and local-only',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l06-ratio-proportion-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/ratioBarDescription/);
 assert.match(html,/M63 110v40/);
 assert.match(html,/3 : 4 = 6 : 8/);
 for(let n=1;n<=6;n++)assert.ok(html.includes('id="ratioQ'+n+'"'));
 assert.match(html,/جدول أصلي لبيانات إنتاج تناسبية/);
 assert.match(html,/id="ratioQ6"/);
 assert.match(html,/id="ratioCooldown"/);
 assert.match(html,/graded:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics/);
});

test('Grade 9 ratio independent mathematics fixtures satisfy cross multiplication',()=>{
 const equal=(a,b,c,d)=>b!==0&&d!==0&&a*d===b*c;
 assert.equal(equal(2,5,8,20),true);
 assert.equal(equal(6,10,3,5),true);
 assert.equal(equal(9,12,15,20),true);
 assert.equal(equal(3,7,6,14),true);
 assert.equal(equal(4,7,24,42),true);
 assert.equal(equal(2,5,8,0),false);
 assert.equal(equal(2,5,8,21),false);
});
