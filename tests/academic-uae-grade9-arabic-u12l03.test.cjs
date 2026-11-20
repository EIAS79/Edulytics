'use strict';
// UAE Grade 9 General source lesson 12-3; prepare tests, do not run during A-B-C.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:12:03:SLOPE-OF-LINE';
test('Source-based Arabic slope lesson exists in pack and RichV2',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l03-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);assert.equal(q.CultureCode,'ar');assert.equal(q.Title,'ميل الخط المستقيم');
 assert.match(p.SourceLocator,/03-1\.pdf.*668/);
 assert.match(p.Translations[0].KeyConceptsAndRules,/غير معرف/);
 assert.ok(q.WorkedExamples.some(x=>x.Answer==='غير معرف'));
});
test('Existing exact gradient question family is reused; undefined vertical case remains teacher reviewed',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(map);assert.deepEqual(map.allowedQuestionFamilies,['geometry.coordinate.gradient_between_points']);
 assert.equal(map.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 assert.ok(map.teacherReviewRequiredFor.some(x=>x.includes('vertical undefined')));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
 assert.ok(contracts.includes('G9_U12L03_GRADIENT_NUMERIC_PARTIAL_REUSE'));
});
test('Original Arabic student cooldown covers horizontal and vertical slope without autograding',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l03-slope-ar.html'),'utf8');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/الميل غير المعرّف|ميله غير معرّف|غير معرّف/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/graded:false,submitted:false/);
});
