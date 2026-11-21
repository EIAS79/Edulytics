'use strict';
// Stage A-B-C prepared tests; execute in Stage D only.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:09:05:INFINITE-SETS';
const family='uae.g9.general.u09.l05.finite_infinite_classification';
test('G9 General 9-5 source-derived Arabic finite/countable/uncountable concepts',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u09-l05-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&rich);assert.match(p.SourceLocator,/02-1\.pdf printed 547/);
 assert.equal(rich.Title,'المجموعات غير المنتهية');
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='مراسلة واحد لواحد'));
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='غير قابلة للعد'));
 assert.ok(rich.WorkedExamples.some(x=>x.Answer==='تقابل ن↦٢ن'));
});
test('G9 9-5 shared exact engine classifies finite/infinite examples independently',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(map);assert.deepEqual(map.allowedQuestionFamilies,[family]);
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9FiniteInfiniteClassification(random)'));
 assert.ok(engine.includes('private static Problem G9FiniteInfiniteClassification(Random r)'));
 assert.match(engine,/mode is 0 or 3 \? "0" : "1"/);
 assert.match(engine,/mode is <0 or >5/);
 assert.ok(engine.includes('normalized==(mode is 0 or 3 ? "0" : "1")'));
 for(const mode of [0,1,2,3,4,5])assert.equal([0,3].includes(mode)?0:1,Number([0,3].includes(mode)?'0':'1'));
 assert.ok(map.teacherReviewRequiredFor.some(x=>x.includes('uncountability') || x.includes('uncountable')));
});
test('Infinite sets Cool-down has teacher review and six independent written answers',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u09-l05-infinite-sets-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/ن ↦ ٢ن/);assert.match(html,/الأعداد الصحيحة/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل إجابات إلى Edulytics ولم تُحتسب درجة/);
});
test('Lesson 59 is A-B-C closed while stage D is unrun',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-59-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,59);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(cp.stageD,'NOT_RUN');
});
