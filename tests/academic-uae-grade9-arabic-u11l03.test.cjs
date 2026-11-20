'use strict';// Stage A-B-C source authoring specs; run at Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:11:03:SEGMENT-RELATIONS-PROOFS';
test('Segment relations content retains older proofs and new measuring supplement',()=>{
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u11-l03-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
assert.ok(pack&&rich);assert.match(pack.SourceLocator,/03-1\.pdf printed624/);assert.match(pack.SourceLocator,/01\.pdf printed374/);
assert.equal(rich.CultureCode,'ar');assert.ok(rich.KeyConcepts.some(x=>x.Title==='قياس القطع بدقة'));
assert.equal(rich.WorkedExamples.find(x=>x.Title==='حل قياس جبري').Answer,'س=٤');
});
test('Numeric linear solution reuses engine while diagrams and measurements remain teacher-reviewed',()=>{
const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
assert.ok(map);assert.deepEqual(map.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c']);
assert.ok(map.teacherReviewRequiredFor.some(x=>x.includes('ruler')));
const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
assert.ok(c.includes('byLesson["'+code+'"]'));
});
test('Segment Cool-down includes six Arabic non-autograded measuring and proof tasks',()=>{
const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u11-l03-segment-relations-ar.html'),'utf8');
assert.match(h,/<html lang="ar" dir="rtl">/);for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
assert.match(h,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
});
test('Lesson66 checkpoint marks Stage D unrun',()=>{
const c=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-66-stage-a-c-checkpoint.json');
assert.equal(c.lessonOrdinal,66);assert.equal(c.closed,true);assert.equal(c.closeDecision,'READY_FOR_STAGE_D');
assert.equal(c.stageC.testsRun,'WRITTEN_NOT_RUN');assert.equal(c.stageD,'NOT_RUN');
});

test('Newer General 7-2 ruler offset and estimation supplement is authored without removing segment proofs',()=>{
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u11-l03-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 const skill=rich.KeyConcepts.find(x=>x.Title==='تقدير طول قطعة بمسطرة مدرّجة');
 const worked=rich.WorkedExamples.find(x=>x.Title==='قياس قطعة لا تبدأ عند الصفر');
 assert.ok(skill&&worked);
 assert.match(skill.Rule,/قراءة النهاية/);
 assert.match(worked.Question,/١٫٢ سم/);
 assert.match(worked.Answer,/٤٫٤ سم/);
 assert.match(worked.Check,/٥٫٦/);
 assert.ok(rich.KeyConcepts.some(x=>x.Title==='مسلمة جمع القطع'));
 assert.ok(rich.WorkedExamples.some(x=>x.Title==='برهان تطابق قطعتين'));
 assert.ok(rich.Visuals.some(x=>x.Title==='قراءة مسطرة لا تبدأ عند الصفر'));
});
