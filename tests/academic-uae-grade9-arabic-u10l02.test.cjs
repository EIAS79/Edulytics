'use strict';
// Stage A–B–C specs; do not run until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:10:02:LOGIC';
const family='uae.g9.general.u10.l02.logic_truth_tables';
test('Source Arabic logic pack includes true/false, negation, conjunction and inclusive disjunction',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u10-l02-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&rich);assert.match(p.SourceLocator,/02-1\.pdf printed 569/);
 assert.equal(rich.CultureCode,'ar');
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='نفي العبارة'));
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='الوصل والفصل'));
 assert.match(rich.WorkedExamples.find(x=>x.Title==='جدول صدق كامل').Answer,/الوصل ص،خ،خ،خ/);
});
test('Shared exact engine uses independent truth-table verifier and bounded parameters',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(mapping);assert.deepEqual(mapping.allowedQuestionFamilies,[family]);
 assert.ok(mapping.teacherReviewRequiredFor.some(x=>x.includes('Venn')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9LogicTruthTables(random)'));
 assert.ok(engine.includes('private static Problem G9LogicTruthTables(Random r)'));
 assert.match(engine,/mode is <0 or >2/);
 assert.match(engine,/provided==expected/);
 for(let p=0;p<=1;p++)for(let q=0;q<=1;q++){
  assert.equal(p&q,(p===1&&q===1)?1:0);
  assert.equal(p|q,(p===1||q===1)?1:0);
 }
});
test('Logic Cool-down shows full table and six teacher-reviewed answers',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u10-l02-logic-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/p∧q/);
 assert.match(html,/p∨q/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات ولم تُحتسب درجة/);
});
test('Lesson 61 Stage D tests remain written but unrun at closure',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-61-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,61);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.stageD,'NOT_RUN');
});
