'use strict';
// Stage A-C written tests for future Stage D, not executed.
const test=require('node:test');const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:01:GRAPHING-LINEAR-EQUATIONS';
test('Grade9 General graphing lesson uses original source-aligned Arabic and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l01-ar.rich-lesson-v2.json');
 const entry=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(entry);
 assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(entry.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U03:L01');
 assert.match(entry.Translations[0].Explanation,/المقطع السيني/);
 const v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(v);assert.equal(v.CultureCode,'ar');
 assert.equal(v.WorkedExamples.length,4);
 assert.ok(v.KeyConcepts.some(x=>x.Title==='مستقيم رأسي وأفقي'));
});
test('Only verified coordinate arithmetic families may assist teacher-reviewed plotted graphs',()=>{
 const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const item=mappings.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(item.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule','geometry.coordinate.y_intercept_from_rule']);
 assert.equal(item.officialOutcomeMapped,false);
 assert.ok(item.teacherReviewRequiredFor.some(x=>x.includes('drawing a straight line')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.match(contract,/G9_U03L01_LINEAR_GRAPHS_INTERCEPTS_NUMERIC_SUPPORT/);
});
test('Correct original SVG coordinate line passes source-aligned intercepts on correct axes',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l01-graphing-linear-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l01-teacher-graph-rubric.json');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/x1="275" y1="50" x2="475" y2="410"/);
 assert.match(html,/cx="300" cy="95"/);
 assert.match(html,/cx="400" cy="275"/);
 assert.match(html,/x="286" y="279" text-anchor="end">٠<\/text>/);
 assert.match(html,/graded:false/);
 assert.equal(rubric.answers.length,8);
 assert.match(rubric.answers[6].expected,/٤،٠/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="graphQ'+i+'"'));
});
test('Core mathematical intercept, table, horizontal and vertical cases independently recomputable',()=>{
 const y=x=>4-2*x;
 assert.equal(y(0),4);assert.equal(y(2),0);
 assert.equal(2*8+4*0,16);assert.equal(2*0+4*4,16);
 for(const x of [-3,0,3]) assert.equal((x/3)+2,[-3,0,3].indexOf(x)+1);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-18-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
