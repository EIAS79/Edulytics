'use strict';// Prepare Stage A-B-C test specs; execute Stage D only.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:11:04:ANGLE-RELATIONS-PROOFS';
test('Arabic angle relations lesson has source addition, supplementary and vertical proof',()=>{
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u11-l04-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
assert.ok(a&&v);assert.match(a.SourceLocator,/03-1\.pdf printed 631/);
assert.ok(v.KeyConcepts.some(x=>x.Title==='المتقابلتان بالرأس'));
assert.equal(v.WorkedExamples.find(x=>x.Title==='زوج خطي').Answer,'١٠٧°');});
test('Existing exact angle families numeric only; human proofs remain teacher reviewed',()=>{
const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);assert.ok(m);
assert.deepEqual(m.allowedQuestionFamilies,['usccss.g7.u07.vertical_angle','usccss.g7.u07.complementary_angle']);
assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('proof')));
const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
assert.ok(c.includes('byLesson["'+code+'"]'));});
test('Angle proof Cool-down is six Arabic teacher-only questions',()=>{
const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u11-l04-angle-relations-ar.html'),'utf8');
assert.match(h,/<html lang="ar" dir="rtl">/);for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
assert.match(h,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);});
test('Lesson67 A-B-C authoring closed, Stage D still unrun',()=>{
const c=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-67-stage-a-c-checkpoint.json');assert.equal(c.lessonOrdinal,67);assert.equal(c.closed,true);
assert.equal(c.closeDecision,'READY_FOR_STAGE_D');assert.equal(c.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(c.stageD,'NOT_RUN');});
