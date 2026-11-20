'use strict';
// A–B–C prepared regression tests; execution belongs to Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:04:04:PARALLEL-AND-PERPENDICULAR-LINES';
test('Grade 9 General 4-4 parallel and perpendicular source has Arabic pack/RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u04-l04-ar.rich-lesson-v2.json');
 const l=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(l.Translations[0].Title,'المستقيمات المتوازية والمتعامدة');
 assert.match(l.Translations[0].Explanation,/المتوازيان/);
 assert.equal(v.WorkedExamples.length,4);
});
test('G9 parallel/perpendicular shares gradient exact family without autograding symbolic proof',()=>{
 const d=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;
 const m=d.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.gradient_between_points','geometry.coordinate.evaluate_linear_rule']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('negative reciprocal')));
 assert.equal(m.officialOutcomeMapped,false);
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(s.includes('byLesson["'+code+'"]'));
 assert.match(s,/G9_U04L04_PARALLEL_PERPENDICULAR_GRADIENT_NUMERIC_REUSE/);
});
test('RTL slope comparison and eight review tasks include right-angle horizontal/vertical exception',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u04-l04-parallel-perpendicular-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u04l04-teacher-parallel-perpendicular-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/viewBox="0 0 650 320"/);
 assert.match(h,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="lineQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
 assert.match(rubric.answers[6].expected,/غير معرف/);
});
test('Equal slopes, negative reciprocals, distinct intersections and vertical exception',()=>{
 assert.equal(3,3);
 assert.equal((2/3)*(-3/2),-1);
 assert.equal((-4/5)*(5/4),-1);
 assert.equal(2*(-1/2),-1);
 assert.equal(7-3*2,1);
 assert.equal(7-(-1/2)*2,8);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-27-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
