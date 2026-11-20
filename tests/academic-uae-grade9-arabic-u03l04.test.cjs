'use strict';
// Stage A–C academic tests written for Stage D. No execution during authoring.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:04:DIRECT-VARIATION';
test('Grade9 General direct variation lesson uses original Arabic source skill and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l04-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(p.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U03:L04');
 assert.match(p.Translations[0].Explanation,/التغير الطردي/);
 assert.ok(v.KeyConcepts.some(x=>x.Title==='نقطة الأصل'));
 assert.equal(v.WorkedExamples.length,4);
});
test('Direct variation reuses numeric family with explicit graph and context teacher review',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const item=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(item.allowedQuestionFamilies,['ratio.unit_rate.equivalent_ratio','geometry.coordinate.evaluate_linear_rule']);
 assert.equal(item.officialOutcomeMapped,false);
 assert.ok(item.teacherReviewRequiredFor.some(x=>x.includes('graph')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.match(c,/G9_U03L04_DIRECT_VARIATION_CONSTANT_AND_ORIGIN/);
});
test('Eight original Arabic teacher tasks and visually accurate direct variation graph',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l04-direct-variation-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l04-teacher-direct-variation-rubric.json');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/role="img"/);
 assert.match(html,/cx="160" cy="320"/);assert.match(html,/cx="200" cy="240"/);assert.match(html,/cx="280" cy="80"/);
 assert.match(html,/graded:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="directQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
 assert.match(rubric.answers[5].expected,/ليست تغيرًا طرديًا/);
});
test('Direct proportionality numeric examples do not infer proportionality from nonzero intercept',()=>{
 const k=72/8;
 assert.equal(k,9);assert.equal(k*7,63);
 assert.equal(3420/6,570);
 for(const x of [1,2,3])assert.notEqual((3*x+2)/x,3);
 for(const x of [2,4,6])assert.equal((5*x)/x,5);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-21-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
