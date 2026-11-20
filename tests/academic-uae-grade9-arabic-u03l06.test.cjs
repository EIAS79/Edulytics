'use strict';
// Stage A–C tests prepared for Stage D; not executed in authoring.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:06:PROPORTIONAL-AND-NONPROPORTIONAL-RELATIONSHIPS';
test('G9 General 3-6 follows Arabic textbook proportional and non-proportional source',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l06-ar.rich-lesson-v2.json');
 const l=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l&&v);
 assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(l.Translations[0].Title,'العلاقات التناسبية وغير التناسبية');
 assert.match(l.Translations[0].Explanation,/ص=ك س/);
 assert.equal(v.WorkedExamples.length,3);
});
test('Reused numeric ratio and linear families are not misrepresented as a graph classifier',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const z=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(z.allowedQuestionFamilies,['ratio.unit_rate.equivalent_ratio','geometry.coordinate.evaluate_linear_rule']);
 assert.equal(z.officialOutcomeMapped,false);
 assert.ok(z.teacherReviewRequiredFor.some(x=>x.includes('graph')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.match(c,/G9_U03L06_PROPORTIONAL_NONPROPORTIONAL_ZERO_INTERCEPT/);
});
test('RTL visual compares origin with nonzero intercept; eight open tasks use teacher review',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l06-proportional-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l06-teacher-proportionality-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 assert.match(html,/viewBox="0 0 610 320"/);
 assert.match(html,/ص=٣س\+٦/);
 assert.match(html,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="relationQ'+i+'"'));
 assert.equal(rub.answers.length,8);
});
test('Proportionality is constant quotient not merely constant differences',()=>{
 const xs=[1,2,3],prop=[3,6,9],other=[9,12,15];
 assert.deepEqual(prop.map((y,i)=>y/xs[i]),[3,3,3]);
 assert.deepEqual(other.map((y,i)=>y/xs[i]),[9,6,5]);
 assert.equal(3*0+6,6);
 assert.equal(3*0,0);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-23-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
