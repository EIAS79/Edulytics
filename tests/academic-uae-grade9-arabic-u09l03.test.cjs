'use strict';
// Stage A-B-C prepared specifications. Execute in Stage D, not authoring.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:09:03:VENN-DIAGRAMS-SET-OPERATIONS';
const family='uae.g9.general.u09.l02.set_operation_cardinality';
test('Grade 9 General 9-3 is Arabic, source referenced and preserves Venn three-set modelling',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u09-l03-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);assert.match(p.SourceLocator,/02-1\.pdf printed 526/);
 assert.equal(lesson.CultureCode,'ar');
 assert.equal(lesson.Title,'استخدام مخططات فن لدراسة العمليات على المجموعات');
 assert.match(lesson.ExplanationParagraphs.join(' '),/ثلاث مجموعات/);
 assert.ok(lesson.WorkedExamples.some(x=>x.Title==='عمليات مركبة على ثلاث مجموعات'));
 assert.match(lesson.WorkedExamples.find(x=>x.Title==='عمليات مركبة على ثلاث مجموعات').Answer,/٢،٤/);
 assert.ok(lesson.Visuals.some(x=>x.Kind==='VennDiagram'));
});
test('Venn cardinality reuses existing union/intersection/difference generator without automatic diagram grading',()=>{
 const row=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 assert.ok(row.teacherReviewRequiredFor.some(x=>x.includes('Venn shading')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9SetOperationCardinality(random)'));
 assert.match(engine,/setA\|setB/);
 assert.match(engine,/setA&setB/);
 assert.match(engine,/setA&~setB/);
});
test('Venn Cool-down contains accessible original diagram and six manual-review questions',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u09-l03-venn-diagrams-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/id="vennTitle"/);assert.match(html,/id="vennDesc"/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/أ∩ب/);assert.match(html,/أᶜ/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم يُرسل إلى النظام ولم تُحتسب درجة/);
});
test('Lesson 57 checkpoint separates closed A-B-C authorship from unrun Stage D',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-57-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,57);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(cp.stageD,'NOT_RUN');
});
