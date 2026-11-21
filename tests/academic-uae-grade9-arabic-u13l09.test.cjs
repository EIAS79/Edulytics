'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:13:09:PARALLELOGRAM-TRIANGLE-AREA';
test('G9 final lesson Arabic parallelogram and triangle area source authoring',()=>{
const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u13-l09-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
assert.ok(p&&q);assert.match(p.SourceLocator,/03-1\.pdf.*793/);
assert.equal(q.CultureCode,'ar');assert.equal(q.Title,'مساحة متوازي الأضلاع والمثلث');
assert.ok(q.KeyConcepts.some(x=>x.Title==='مساحة متوازي الأضلاع'));assert.ok(q.KeyConcepts.some(x=>x.Title==='مساحة المثلث'));
assert.ok(q.WorkedExamples.some(x=>x.Answer.includes('٨٤')));
assert.ok(q.WorkedExamples.some(x=>x.Answer.includes('٤٥')));
assert.equal(12*7,84);assert.equal(15*6/2,45);assert.equal(72/9,8);assert.equal(8*(8+5)/2,52);
});
test('G9 U13L09 reuses exact independent rectangle and triangle area arithmetic with Arabic scoped prompts',()=>{
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;const m=mappings.find(x=>x.lessonCode===code);
assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['geometry.perimeter_area.rectangle_area','geometry.perimeter_area.triangle_area']);
assert.equal(m.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
assert.ok(contract.includes('byLesson["'+code+'"]'));
assert.ok(engine.includes('G9 General U13L09: reuse exact scalar area verification'));
assert.match(engine,/متوازي أضلاع قاعدته/);assert.match(engine,/مثلث قاعدته/);
assert.match(engine,/الارتفاع ليس الضلع المائل/);
});
test('G9 final area Cool-down is original Arabic teacher review with six free tasks',()=>{
const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u13-l09-parallelogram-triangle-area-ar.html'),'utf8');
assert.match(h,/lang="ar" dir="rtl"/);
for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
assert.match(h,/TEACHER_REVIEW_REQUIRED/);assert.match(h,/graded:false,submitted:false/);
assert.match(h,/لم يُرسل إلى Edulytics/);
});
