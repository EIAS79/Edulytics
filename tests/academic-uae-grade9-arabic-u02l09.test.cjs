'use strict';
// Prepared Stage A-C academic fixtures; do not run until Stage D.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:02:09:WEIGHTED-AVERAGE';
test('Weighted average source lesson has authored Arabic content and unit-correct worked examples',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u02-l09-ar.rich-lesson-v2.json');
 assert.equal(pack.AcademicLanguage,'ar');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);
 assert.equal(lesson.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U02:L09');
 const translated=lesson.Translations.find(x=>x.CultureCode==='ar');
 assert.match(translated.Explanation,/المتوسط المرجح/);
 assert.match(translated.Explanation,/المسافة الكلية\/الزمن الكلي/);
 const ar=rich.Lessons.find(x=>x.LessonCode===code);
 assert.equal(ar.CultureCode,'ar');
 assert.equal(ar.WorkedExamples.length,5);
 assert.ok(ar.KeyConcepts.some(x=>x.Title==='تركيز المحلول المختلط'));
 assert.ok(ar.WorkedExamples.some(x=>/٦٠ كم\/ساعة/.test(x.Answer)));
});
test('Weighted mean existing question engine is a numeric prerequisite, never misrepresented as grading mixtures',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=mapping.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(entry.allowedQuestionFamilies,['statistics.center_spread.mean']);
 assert.equal(entry.officialOutcomeMapped,false);
 assert.ok(entry.teacherReviewRequiredFor.includes('algebraic mixture concentration equations'));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
 assert.match(contracts,/G9_U02L09_WEIGHTED_CORE_WITH_TEACHER_REVIEW/);
});
test('Eight teacher-reviewed original cooldown cases distinguish weighted averages from simple averages',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l09-weighted-average-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l09-teacher-cooldown-rubric.json');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/role="img"/);
 assert.match(html,/graded:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="weightedQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
 assert.match(rubric.answers[7].expected,/٥٣⅓/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics/);
});
test('Weighted mean, mixture and average speed have independent arithmetic fixtures; Stage D remains unrun',()=>{
 const mean=(a,b,wa,wb)=>(a*wa+b*wb)/(wa+wb);
 assert.equal(mean(80,90,.3,.7),87);
 assert.ok(Math.abs(mean(.10,.30,2,3)-.22)<1e-12);
 assert.equal(mean(10,20,1,3),17.5);
 assert.ok(Math.abs(80/(40/40+40/80)-160/3)<1e-12);
 assert.equal(mean(60,85,.2,.8),80);
 const c=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-17-stage-a-c-checkpoint.json');
 assert.equal(c.closed,true);
 assert.equal(c.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(c.stageC.testsRun,'WRITTEN_NOT_RUN');
});
