'use strict';
// Stage A-B-C focused specifications: authored now, execution deferred to Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:05:03:MULTI-STEP-INEQUALITIES';
test('Grade9 Arabic multi-step lesson contains source-grounded worked cases',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l03-ar.rich-lesson-v2.json');
 const a=p.Lessons.find(x=>x.LessonCode===id),rich=v.Lessons.find(x=>x.LessonCode===id);
 assert.ok(a&&rich);
 assert.equal(p.AcademicLanguage,'ar');
 assert.equal(a.Translations[0].Title,'حل المتباينات متعددة الخطوات');
 assert.equal(rich.WorkedExamples.length,4);
 assert.match(a.Translations[0].WorkedExamples,/−١١س−١٣>٤٢/);
 assert.match(rich.WorkedExamples[2].Answer,/∅/);
});
test('Grade9 original teacher tasks include inequality distribution, negative sign, and open/closed endpoints',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l03-multi-step-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l03-teacher-multistep-inequalities-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="multiQ'+i+'"'),i);
 assert.match(h,/circle cx="425" cy="65" r="10" fill="#166d9d"/);
 assert.match(h,/circle cx="425" cy="185" r="10" fill="white"/);
 assert.match(h,/graded:false,submitted:false/);
 assert.equal(rub.answers.length,8);
 assert.equal(rub.autoGrade,false);
});
test('Existing inequality numeric contract is reused without false multi-step auto-scoring claims',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);
 assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.inequality.ax_plus_b_relation_c']);
 assert.equal(m.officialOutcomeMapped,false);
 assert.ok(m.teacherReviewRequiredFor.includes('distribute and combine like terms on both sides'));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
});
test('Multi-step original math fixtures preserve solution direction',()=>{
 const evaluate=x=>4*(3*x-5)+7>=8*x+3;
 assert.equal(evaluate(4),true);assert.equal(evaluate(3),false);
 const noSolution=x=>9*x-5*(x-5)<=4*(x-3);
 for(const x of [-100,-2,0,10,100])assert.equal(noSolution(x),false);
 const allSolutions=x=>2*(x+3)>=2*x+5;
 for(const x of [-100,0,100])assert.equal(allSolutions(x),true);
 const negative=x=>-11*x-13>42;
 assert.equal(negative(-6),true);assert.equal(negative(-5),false);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-30-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);
 assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
