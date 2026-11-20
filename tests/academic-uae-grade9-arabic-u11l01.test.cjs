'use strict';
// Stage A-B-C prepared, not executed until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:11:01:POSTULATES-INFORMAL-PROOFS';
const family='uae.g9.general.u11.l01.incidence_postulates';
test('Both official General student editions contribute to Arabic source-aligned geometry lesson',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u11-l01-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&rich);assert.match(p.SourceLocator,/03-1\.pdf printed 608/);
 assert.match(p.SourceLocator,/01\.pdf printed 365/);
 assert.equal(p.SourceSha256,'eab188122aacdcb6a454731c72f33e62dc46b4442b690de92926b03dc8a34b10');
 assert.equal(rich.CultureCode,'ar');
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='مسلمة تحديد المستوى'));
 assert.ok(rich.WorkedExamples.some(x=>x.Title==='برهان حر على طول قطعة'));
});
test('Shared exact incidence family retains teacher review for informal geometry proofs',()=>{
 const row=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9IncidencePostulates(random)'));
 assert.ok(engine.includes('private static Problem G9IncidencePostulates(Random r)'));
 assert.match(engine,/caseIndex is <1 or >20/);
 assert.ok(row.teacherReviewRequiredFor.some(x=>x.includes('informal proofs')));
});
test('Postulates Cool-down retains six teacher-reviewed reasoning prompts and independent local export',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u11-l01-postulates-informal-proofs-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/مسلمة جمع القطع/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل إجابات ولم تُحتسب درجة/);
});
test('Lesson64 A-B-C checkpoint represents authoring closure not Stage D pass',()=>{
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-64-stage-a-c-checkpoint.json');
 assert.equal(cp.lessonOrdinal,64);assert.equal(cp.closed,true);
 assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.stageD,'NOT_RUN');
});
