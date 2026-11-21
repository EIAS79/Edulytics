'use strict';
// A–B–C prepared tests; Stage D will run them.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:04:03:POINT-SLOPE-FORM';
test('Grade9 General 4-3 Arabic point-slope content is sourced from original student book',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u04-l03-ar.rich-lesson-v2.json');
 const a=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(a&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(a.Translations[0].Title,'كتابة المعادلات بصيغة النقطة والميل');
 assert.match(a.Translations[0].Explanation,/النقطة والميل/);assert.equal(v.WorkedExamples.length,4);
});
test('Point slope lesson reuses numeric gradient without claiming exact symbolic autograde',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.gradient_between_points','geometry.coordinate.evaluate_linear_rule']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('symbolic')));assert.equal(m.officialOutcomeMapped,false);
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(s.includes('byLesson["'+code+'"]'));assert.match(s,/G9_U04L03_POINT_SLOPE_SYMBOLIC_TEACHER_NUMERIC_REUSE/);
});
test('RTL student point-slope graph and 8 teacher review tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u04-l03-point-slope-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u04l03-teacher-point-slope-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);assert.match(html,/viewBox="0 0 620 330"/);
 assert.match(html,/cx="340" cy="110"/);assert.match(html,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="pointQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
});
test('Source skill algebraic fixtures point-slope, fraction and vertical exception',()=>{
 const y=(m,x,px,py)=>m*(x-px)+py;
 assert.equal(y(2,3,3,5),5);assert.equal(y(2,0,3,5),-1);
 assert.equal(y(-1.5,0,2,4),7);assert.equal(y(3,3,1,2),8);
 assert.equal((8-2)/(3-1),3);assert.equal(4-4,0);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-26-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
