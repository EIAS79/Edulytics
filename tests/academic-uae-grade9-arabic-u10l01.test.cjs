'use strict';
// Stage A-B-C prepared source tests; run only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:10:01:INDUCTIVE-REASONING-CONJECTURES';
test('G9 General U10L01 Arabic content preserves induction, differences and counterexamples',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u10-l01-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&v);assert.match(p.SourceLocator,/02-1\.pdf printed 561/);
 assert.equal(v.CultureCode,'ar');
 assert.ok(v.KeyConcepts.some(x=>x.Title==='المثال المضاد'));
 assert.equal(v.WorkedExamples.find(x=>x.Title==='نمط بفروق ثانية ثابتة').Answer,'٥٤');
 assert.ok(v.WorkedExamples.find(x=>x.Title==='نقض تخمين عام').Answer.includes('٣+٥=٨'));
});
test('Only source-supported sequence numeric subskills reuse existing exact engine',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(entry);assert.deepEqual(entry.allowedQuestionFamilies,['sequences.core.arithmetic_nth_term','sequences.core.geometric_nth_term']);
 assert.ok(entry.teacherReviewRequiredFor.some(x=>x.includes('figure-based')));
 assert.ok(entry.teacherReviewRequiredFor.some(x=>x.includes('counterexamples')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"sequences.core.arithmetic_nth_term"'));
 assert.ok(engine.includes('"sequences.core.geometric_nth_term"'));
});
test('Inductive Cool-down has six Arabic source cases and teacher-only local record',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u10-l01-inductive-reasoning-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/٤،١٠،١٨،٢٨،٤٠/);
 assert.match(html,/مثال مضاد/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل إجابات ولم تُحتسب درجة/);
});
test('Lesson 60 reports A-B-C closure with Stage D unrun',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-60-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,60);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.stageD,'NOT_RUN');
});
