'use strict';
// Grade 9 Stage A-C fixtures; Stage D executes. Do not run now.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=file=>JSON.parse(fs.readFileSync(path.join(root,file),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:02:07:PERCENT-CHANGE';

test('Source-book percent-change lesson uses Grade9 General Arabic pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u02-l07-ar.rich-lesson-v2.json');
 assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(pack.Lessons.filter(x=>x.LessonCode===code).length,1);
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);
 assert.equal(lesson.OfficialReferenceCode,'UAE:REF:TEXTBOOK:G9:GENERAL:T1:U02:L07');
 assert.match(lesson.Translations.find(x=>x.CultureCode==='ar').Explanation,/القيمة الأصلية/);
 const ar=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(ar);
 assert.equal(ar.CultureCode,'ar');
 assert.equal(ar.WorkedExamples.length,4);
 assert.ok(ar.KeyConcepts.some(x=>x.Title==='نسبة الزيادة'));
 assert.match(ar.WorkedExamples[0].Answer,/١٥٪ زيادة/);
});

test('Existing percent verifier families are scoped to the new Grade9 Arabic lesson',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(entry.allowedQuestionFamilies,['percentages.core.increase_decrease','percentages.core.of_quantity']);
 assert.equal(entry.officialOutcomeMapped,false);
 assert.equal(entry.practiceReadiness,'PREPARED_STAGE_D_NOT_RUN');
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('string.Equals(scopeKey,'));
 assert.ok(engine.includes('زادت قيمة أصلية'));
 assert.ok(engine.includes('انخفضت قيمة أصلية'));
 assert.ok(engine.includes('يُمنح خصم'));
});

test('Original Grade9 percent change diagram and nine independent teacher-only cooldown tasks',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u02-l07-percent-change-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l07-teacher-cooldown-rubric.json');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 assert.match(html,/pctDesc/);
 assert.match(html,/role="img"/);
 assert.match(html,/القيمة الأصلية/);
 for(let n=1;n<=9;n++)assert.ok(html.includes('id="pctQ'+n+'"'));
 assert.equal(rubric.answers.length,9);
 assert.match(rubric.answers[8].expected,/مئيني=١٨\/٢٠/);
 assert.match(rubric.answers[4].expected,/١٦⅔٪/);
 assert.match(rubric.answers[7].expected,/غير معرّفة/);
 assert.match(html,/graded:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/لم تُرسل الإجابات إلى Edulytics/);
});

test('Independent percent change mathematics checks bases, direction and zero-denominator boundary',()=>{
 const percent=(a,b)=>a===0?null:Math.abs(b-a)/Math.abs(a)*100;
 assert.equal(percent(20,23),15);
 assert.equal(percent(100,80),20);
 assert.equal(percent(100,120),20);
 assert.ok(Math.abs(percent(120,100)-100/6)<1e-10);
 assert.equal(percent(0,10),null);
 assert.ok(Math.abs(100*.8*.9-72)<1e-10);
});


test('Grade 9 percent-change core 40 source exercises are classified without misrepresenting spiral exercises',()=>{
 const ledger=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u02l07-source-element-ledger.json');
 const checkpoint=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-15-stage-a-c-checkpoint.json');
 assert.equal(ledger.sourceElements.length,73);
 const verified=ledger.sourceElements.filter(x=>x.sourceMathIndependentlyVerified);
 assert.equal(verified.length,40);
 assert.ok(verified.every(x=>x.sourceMathematicsSpotAudit?.expectedMathAnswer));
 const others=ledger.sourceElements.filter(x=>!x.sourceMathIndependentlyVerified);
 assert.equal(others.length,33);
 assert.ok(others.every(x=>x.teacherReviewRequired));
 assert.equal(checkpoint.closed,true);
 assert.equal(checkpoint.closeDecision,'READY_FOR_STAGE_D');
 assert.equal(checkpoint.stageC.testsRun,'WRITTEN_NOT_RUN');
});
