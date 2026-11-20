'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lessonCode='PED:UAE-MOE-MATH:L9:GENERAL:02:08:LITERAL-EQUATIONS-DIMENSIONAL-ANALYSIS';

test('Original Grade9 General literal/dimensional lesson has Arabic pack and RichV2 with geometry and unit conversions',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u02-l08-ar.rich-lesson-v2.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.equal(pack.AcademicLanguage,'ar');
 assert.ok(lesson);
 assert.equal(lesson.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U02:L08');
 assert.match(lesson.Translations[0].Explanation,/التحليل البعدي/);
 assert.match(lesson.Translations[0].WorkedExamples,/٣٦ قدمًا\/ثانية/);
 const model=rich.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.equal(model.CultureCode,'ar');
 assert.equal(model.WorkedExamples.length,5);
 assert.ok(model.KeyConcepts.some(x=>x.Title==='عزل متغير في معادلة حرفية'));
 assert.ok(model.KeyConcepts.some(x=>x.Title==='التحليل البعدي للمعدلات'));
 assert.match(model.WorkedExamples[4].Answer,/٢٤٫٥٥/);
});

test('Numeric engine is reused only for approved supplementary skills, not falsely as complete symbolic autograding',()=>{
 const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const m=mappings.mappings.find(x=>x.lessonCode===lessonCode);
 assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c','ratio.unit_rate.direct']);
 assert.equal(m.officialOutcomeMapped,false);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('literal equation')));
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('dimensional')));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+lessonCode+'"]'));
 assert.match(contracts,/G9_U02L08_LITERAL_FORMULAS_AND_DIMENSIONAL_UNITS_NUMERIC_SUPPORT_ONLY/);
});

test('Eight independent teacher-reviewed questions cover algebra and dimensional units without claiming platform grading',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l08-literal-dimensional-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l08-teacher-cooldown-rubric.json');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/role="img"/);
 assert.match(html,/graded:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="litQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
 assert.match(rubric.answers[4].expected,/٦٫٢١٦ ميل/);
 assert.match(rubric.answers[5].expected,/٢٤٫٥٤٥ ميل\/ساعة/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics/);
});

test('Dimensional-analysis conversion arithmetic is independently recomputable',()=>{
 const miles=10*1000*1.094/1760;
 const mph=36*3600/5280;
 const metersPerSecond=3.6*1609.344/3600;
 assert.ok(Math.abs(miles-6.215909090909091)<1e-9);
 assert.ok(Math.abs(mph-24.545454545454547)<1e-9);
 assert.ok(Math.abs(metersPerSecond-1.609344)<1e-12);
 const checkpoint=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-16-stage-a-c-checkpoint.json');
 assert.equal(checkpoint.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(checkpoint.stageC.testsRun,'WRITTEN_NOT_RUN');
});
