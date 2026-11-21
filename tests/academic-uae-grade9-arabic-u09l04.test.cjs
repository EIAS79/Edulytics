'use strict';
// Authoring-only Stage A-B-C specification; execute in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:09:04:SETS-WORD-PROBLEMS';
const family='uae.g9.general.u09.l02.set_operation_cardinality';
test('Grade9 General 9-4 Arabic content maps contextual two- and three-set mathematics',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u09-l04-ar.rich-lesson-v2.json');
 const a=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(a&&v);assert.match(a.SourceLocator,/02-1\.pdf printed 537/);
 assert.equal(v.CultureCode,'ar');
 assert.ok(v.WorkedExamples.some(x=>x.Title==='استطلاع طلاب'));
 assert.ok(v.WorkedExamples.some(x=>x.Title==='ثلاث مجموعات دون تكرار'));
 assert.equal(v.WorkedExamples.find(x=>x.Title==='ثلاث مجموعات دون تكرار').Answer,'٣٤');
 const first=v.WorkedExamples.find(x=>x.Title==='استطلاع طلاب');
 assert.match(first.Answer,/٣٢/);assert.match(first.Answer,/٨/);
});
test('Set context lesson reuses existing bounded set cardinality engine without autograding open reasoning',()=>{
 const row=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 assert.ok(row.teacherReviewRequiredFor.some(x=>x.includes('three-set')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9SetOperationCardinality(random)'));
 assert.match(engine,/setA\|setB/);assert.match(engine,/setA&setB/);
});
test('Word problem Cool-down contains six teacher-review contextual tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u09-l04-set-word-problems-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let n=1;n<=6;n++)assert.ok(html.includes('id="q'+n+'"'));
 assert.match(html,/٤٠ طالبًا/);assert.match(html,/تقاطع الثلاث/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات ولم تُحتسب درجة/);
});
test('Lesson 58 A-B-C authoring closure does not claim tests executed',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-58-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,58);assert.equal(cp.closed,true);assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.stageD,'NOT_RUN');
});
