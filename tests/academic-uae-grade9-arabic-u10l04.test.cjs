'use strict';
// Stage A-B-C prepared specifications. Execute in Stage D, not in authoring.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:10:04:DEDUCTIVE-REASONING';
const family='uae.g9.general.u10.l03.conditional_truth_tables';
test('Source deductive lesson has Arabic detachment and syllogism proof examples',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u10-l04-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&v);assert.match(p.SourceLocator,/02-1\.pdf printed 587/);
 assert.equal(v.Title,'التبرير الاستنتاجي');
 assert.ok(v.KeyConcepts.some(x=>x.Title==='قانون الفصل'));
 assert.ok(v.KeyConcepts.some(x=>x.Title==='قانون القياس المنطقي'));
 assert.ok(v.WorkedExamples.some(x=>x.Title==='تأكيد التالي خطأ'));
});
test('Exact engine reuses only prerequisite conditional truth calculation; proofs stay teacher reviewed',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(map);assert.deepEqual(map.allowedQuestionFamilies,[family]);
 assert.ok(map.teacherReviewRequiredFor.some(x=>x.includes('syllogism')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(engine.includes('"'+family+'" => G9ConditionalTruthTables(random)'));
});
test('Deductive Cool-down has six independent open proof tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u10-l04-deductive-reasoning-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/قانون الفصل/);assert.match(html,/قانون القياس/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات ولم تُحتسب درجة/);
});
test('Lesson 63 authoring checkpoint does not claim Stage D executed',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-63-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,63);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.stageD,'NOT_RUN');
});
