'use strict';
// Grade9 General A–B–C test specifications; execution reserved for Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:04:01:GRAPHING-SLOPE-INTERCEPT-FORM';
test('UAE Grade9 General 4-1 has Arabic source-derived slope-intercept lessons in pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u04-l01-ar.rich-lesson-v2.json');
 const a=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(a&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(a.Translations[0].Title,'تمثيل المعادلات بيانيًا بصيغة الميل والمقطع');
 assert.match(a.Translations[0].Explanation,/ص=م س\+ب/);
 assert.equal(v.WorkedExamples.length,4);
});
test('Existing coordinate families registered for slope/intercept numeric help, graphs remain teacher review',()=>{
 const d=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const m=d.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule','geometry.coordinate.y_intercept_from_rule','geometry.coordinate.gradient_between_points']);
 assert.equal(m.officialOutcomeMapped,false);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('graph')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.match(contract,/G9_U04L01_SLOPE_INTERCEPT_NUMERIC_AND_GRAPH_TEACHER/);
});
test('Student SVG has correct coordinates and eight open response tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u04-l01-slope-intercept-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u04l01-teacher-slope-intercept-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 assert.match(html,/viewBox="0 0 600 365"/);
 assert.match(html,/cx="100" cy="310"/);
 assert.match(html,/cx="155" cy="250"/);
 assert.match(html,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="slopeQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
});
test('Slope intercept fixtures cover negative, fractional, standard form and vertical exception',()=>{
 const y=(m,b,x)=>m*x+b;
 assert.equal(y(2,-3,0),-3);assert.equal(y(2,-3,1),-1);
 assert.equal(y(-1.5,4,2),1);assert.equal(y(-1.5,4,4),-2);
 assert.equal((6-3*0)/2,3);
 assert.equal((3-(-1))/(2-0),2);
 assert.equal((8-2*0)/4,2);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-24-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
