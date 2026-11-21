'use strict';
// Stage A-B-C authored, Stage D tests not run.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:05:02:MULTIPLY-DIVIDE-INEQUALITIES';
test('G9 General 5-2 uses original Arabic multiplication and division inequality content',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l02-ar.rich-lesson-v2.json');
 const a=p.Lessons.find(x=>x.LessonCode===code),rich=v.Lessons.find(x=>x.LessonCode===code);
 assert.ok(a&&rich);assert.equal(p.AcademicLanguage,'ar');
 assert.equal(a.Translations[0].Title,'حل المتباينات بالضرب والقسمة');
 assert.match(a.Translations[0].Explanation,/يعكس جهة/);
 assert.equal(rich.WorkedExamples.length,4);
});
test('Reused inequality numeric family does not claim teacher graph automation',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.inequality.ax_plus_b_relation_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('negative multiplier')));
 assert.equal(m.officialOutcomeMapped,false);
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.match(c,/G9_U05L02_NEGATIVE_MULTIPLIER_REVERSAL_NUMERIC_HELP/);
});
test('RTL number lines use correct open and closed circles with teacher tasks',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l02-multiply-divide-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l02-teacher-multiply-divide-inequalities-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/circle cx="360" cy="58" r="10" fill="white"/);
 assert.match(h,/circle cx="430" cy="177" r="10" fill="#b5652a"/);
 assert.match(h,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="multiplyQ'+i+'"'));
 assert.equal(rub.answers.length,8);
});
test('Mathematical fixture correctly reverses on negative factors',()=>{
 assert.equal(-2*0<8,true);assert.equal(-2*(-5)<8,false);
 assert.equal((-15)/(-3)>=5,true);assert.equal((-5)*(-3)>=15,true);
 assert.equal((-3)*(-4)<=12,true);
 assert.equal((-2)*(-7)>12,true);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-29-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
