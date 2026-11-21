'use strict';
// Stage A-B-C authored; to execute at Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:11:02:ALGEBRAIC-PROOF';
test('Source Arabic algebraic proof has equality properties, free proof and correct solved example',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u11-l02-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&v);assert.match(p.SourceLocator,/03-1\.pdf printed 616/);
 assert.equal(v.CultureCode,'ar');assert.ok(v.KeyConcepts.some(x=>x.Title==='خواص المساواة'));
 assert.equal(v.WorkedExamples.find(x=>x.Title==='برهان معادلة متعددة الخطوات').Answer,'س=−١٨');
});
test('Shared linear equation generator is used only for numeric prerequisite; steps are teacher-reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c','algebra.linear.variables_both_sides']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('two-column')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
});
test('Algebraic proof Cool-down provides six manual proof submissions',()=>{
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u11-l02-algebraic-proof-ar.html'),'utf8');
 assert.match(s,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(s.includes('id="q'+i+'"'));
 assert.match(s,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(s,/لم تُرسل إجابات ولم تُحتسب درجة/);
});
test('Lesson65 checkpoint is A-B-C ready for Stage D but not tested',()=>{
 const x=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-65-stage-a-c-checkpoint.json');
 assert.equal(x.lessonOrdinal,65);assert.equal(x.closed,true);
 assert.equal(x.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(x.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(x.stageD,'NOT_RUN');
});
