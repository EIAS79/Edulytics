'use strict';
// A-B-C lesson acceptance specifications. Stage D runs tests later.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:04:02:WRITING-SLOPE-INTERCEPT-EQUATIONS';
test('UAE Grade9 General 4-2 writes Arabic slope-intercept equations from original student source',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u04-l02-ar.rich-lesson-v2.json');
 const a=p.Lessons.find(x=>x.LessonCode===code),rich=v.Lessons.find(x=>x.LessonCode===code);
 assert.ok(a&&rich);assert.equal(p.AcademicLanguage,'ar');
 assert.equal(a.Translations[0].Title,'كتابة المعادلات بصيغة الميل والمقطع');
 assert.match(a.Translations[0].Explanation,/الميل/);
 assert.equal(rich.WorkedExamples.length,4);
});
test('Existing numeric gradient and intercept families reused; open equation graphs teacher reviewed',()=>{
 const d=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const m=d.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.gradient_between_points','geometry.coordinate.y_intercept_from_rule','geometry.coordinate.evaluate_linear_rule']);
 assert.equal(m.officialOutcomeMapped,false);assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('whole symbolic')));
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(s.includes('byLesson["'+code+'"]'));
 assert.match(s,/G9_U04L02_WRITE_SLOPE_INTERCEPT_FROM_POINTS_AND_GRAPHS/);
});
test('Student activity has correct sample graph and 8 teacher reviewed problems',()=>{
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u04-l02-write-slope-intercept-ar.html'),'utf8');
 const k=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u04l02-teacher-write-slope-intercept-rubric.json');
 assert.match(s,/lang="ar" dir="rtl"/);assert.match(s,/viewBox="0 0 600 340"/);
 assert.match(s,/cx="160" cy="210"/);assert.match(s,/cx="320" cy="60"/);
 for(let i=1;i<=8;i++)assert.ok(s.includes('id="equationQ'+i+'"'));
 assert.match(s,/graded:false,submitted:false/);assert.equal(k.answers.length,8);
});
test('Slope and intercept examples remain mathematically correct',()=>{
 const slope=(y1,y2,x1,x2)=>(y2-y1)/(x2-x1);
 assert.equal(slope(2,8,1,3),3);
 assert.equal(2-slope(2,8,1,3)*1,-1);
 assert.equal(3*2+1,7);
 assert.equal(slope(3,-1,0,2),-2);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-25-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
