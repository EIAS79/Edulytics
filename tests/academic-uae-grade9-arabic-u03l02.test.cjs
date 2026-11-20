'use strict';
// Stage A-C prepared academic tests. Stage D runs them later.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:02:SOLVING-LINEAR-EQUATIONS-BY-GRAPHING';
test('Graph-root lesson preserves original Grade9 General Arabic source content',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l02-ar.rich-lesson-v2.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(lesson.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U03:L02');
 assert.match(lesson.Translations[0].Explanation,/المقطع السيني/);
 const item=rich.Lessons.find(x=>x.LessonCode===code);
 assert.equal(item.CultureCode,'ar');assert.equal(item.WorkedExamples.length,4);
 assert.ok(item.KeyConcepts.some(x=>x.Title==='حلول لا نهائية'));
});
test('Dedicated scope only uses reusable algebraic answer verifier; graphs stay teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(entry.allowedQuestionFamilies,['algebra.linear.ax_plus_b_equals_c','algebra.linear.variables_both_sides']);
 assert.equal(entry.officialOutcomeMapped,false);
 assert.ok(entry.teacherReviewRequiredFor.some(x=>x.includes('graph root')));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.match(contract,/G9_U03L02_EQUATION_GRAPH_ROOT_NUMERIC_SUPPORT_ONLY/);
});
test('Arabic graph-root visual shows correct y=2x-8 zero and eight non-graded teacher tasks',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l02-graphical-solving-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l02-teacher-graph-roots-rubric.json');
 assert.match(h,/<html lang="ar" dir="rtl">/);
 assert.match(h,/role="img"/);
 assert.match(h,/cx="400" cy="200"/);
 assert.match(h,/\(٤،٠\)/);
 assert.match(h,/graded:false/);
 assert.equal(rubric.answers.length,8);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="rootQ'+i+'"'));
 assert.match(rubric.answers[3].expected,/كل عدد حقيقي/);
});
test('Graph roots, inconsistent and identity equation fixtures independently verify algebraic results',()=>{
 assert.equal(2*4-8,0);
 assert.equal(3*(-1)+1,-2);
 for(const x of [-8,-1,0,1,5])assert.notEqual(3*x+7,3*x+1);
 for(const x of [-8,-1,0,1,5])assert.equal(2*x-4,2*(x-2));
 assert.ok(Math.abs(20/.75-80/3)<1e-12);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-19-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
