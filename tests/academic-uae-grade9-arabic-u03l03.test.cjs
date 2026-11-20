'use strict';
// Stage A-C authored acceptance specs: Stage D executes.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:03:RATE-OF-CHANGE-AND-SLOPE';
test('Grade9 General source slope lesson has Arabic curriculum pack and RichV2 examples',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l03-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),ar=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p);assert.ok(ar);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(p.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U03:L03');
 assert.match(p.Translations[0].Explanation,/الميل/);
 assert.equal(ar.WorkedExamples.length,5);
 assert.ok(ar.KeyConcepts.some(x=>x.Title==='ميل أفقي ورأسي'));
});
test('Existing independent gradient solver reused, graph interpretation under teacher review',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const item=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(item.allowedQuestionFamilies,['geometry.coordinate.gradient_between_points']);
 assert.equal(item.officialOutcomeMapped,false);
 assert.ok(item.teacherReviewRequiredFor.some(x=>x.includes('unit rate')));
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(s.includes('byLesson["'+code+'"]'));
});
test('Original slope triangle SVG and eight source-skill tasks keep teacher grading',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l03-slope-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l03-teacher-slope-rubric.json');
 assert.match(h,/<html lang="ar" dir="rtl">/);assert.match(h,/role="img"/);
 assert.match(h,/cx="160" cy="290"/);assert.match(h,/cx="340" cy="110"/);
 assert.match(h,/graded:false/);assert.equal(rubric.answers.length,8);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="slopeQ'+i+'"'));
});
test('Slope arithmetic distinguishes rise, descent, horizontal and undefined vertical',()=>{
 const slope=(x1,y1,x2,y2)=>x1===x2?null:(y2-y1)/(x2-x1);
 assert.equal(slope(1,3,4,9),2);
 assert.equal(slope(-2,4,2,-4),-2);
 assert.equal(slope(-3,2,4,2),0);
 assert.equal(slope(3,-1,3,6),null);
 assert.equal(slope(-2,-3,-4,-1),-1);
 assert.equal((234-78)/(6-2),39);
 const c=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-20-stage-a-c-checkpoint.json');
 assert.equal(c.closed,true);assert.equal(c.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(c.stageC.testsRun,'WRITTEN_NOT_RUN');
});
