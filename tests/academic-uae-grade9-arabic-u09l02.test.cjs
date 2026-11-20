'use strict';
// Stage A–B–C source-bound acceptance specifications; executed only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:09:02:SUBSETS-AND-SET-OPERATIONS';
const family='uae.g9.general.u09.l02.set_operation_cardinality';

test('Grade 9 General U09L02 has Arabic student-book source concepts and correct set examples',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u09-l02-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);assert.match(p.SourceLocator,/02-1\.pdf printed 515/);
 assert.equal(lesson.CultureCode,'ar');
 assert.equal(lesson.Title,'المجموعات الجزئية والعمليات على المجموعات');
 for(const name of ['الجزئية والجزئية الحقيقية','الاتحاد والتقاطع','الفرق والمتممة','عدد عناصر الاتحاد'])
   assert.ok(lesson.KeyConcepts.some(x=>x.Title===name));
 const union=lesson.WorkedExamples.find(x=>x.Title==='الاتحاد والتقاطع');
 assert.ok(union);assert.match(union.Answer,/أ∩ب=\{٢،٤\}/);
 assert.ok(lesson.WorkedExamples.some(x=>x.Title==='متممة مجموعة'));
});

test('Grade 9 U09L02 reuses finite set-operation engine and independently verifies counts',()=>{
 const row=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9SetOperationCardinality(random)'));
 assert.ok(engine.includes('setA|setB'));
 assert.ok(engine.includes('setA&setB'));
 assert.ok(engine.includes('setA&~setB'));
 assert.match(engine,/parsed==independent/);
 assert.ok(row.teacherReviewRequiredFor.some(s=>s.includes('Venn')));
});

test('Student Cool-down captures six original tasks only for teacher review',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u09-l02-subsets-set-operations-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'),'missing question '+i);
 assert.match(html,/أ⊂ب/);
 assert.match(html,/أ\\ب/);
 assert.match(html,/أᶜ/);
 assert.ok(html.includes('|أ∪ب|'))
 assert.match(html,/مخطط فن/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات أو تُحتسب درجة/);
});

test('A-B-C records authoring closure but does not claim Stage D runtime PASS',()=>{
 const checkpoint=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-56-stage-a-c-checkpoint.json');
 assert.equal(checkpoint.lessonOrdinal,56);
 assert.equal(checkpoint.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(checkpoint.closed,true);
 assert.equal(checkpoint.stageC.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(checkpoint.stageD,'NOT_RUN');
});
