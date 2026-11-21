'use strict';
// Grade 9 UAE General lesson 68: authored Stage A-B-C contract checks, reserved for Stage D.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lessonCode='PED:UAE-MOE-MATH:L9:GENERAL:12:01:PARALLEL-INTERSECTING-LINES';
test('Original General textbook lesson 12-1 has an Arabic content pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l01-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===lessonCode);
 const q=rich.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(p&&q);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'المستقيمات المتوازية والمتقاطعة');
 assert.match(p.SourceLocator,/03-1\.pdf.*653/);
 assert.equal(q.CultureCode,'ar');
 assert.ok(q.KeyConcepts.some(x=>x.Title==='التوازي والتقاطع والتخالف'));
 assert.ok(q.WorkedExamples.some(x=>x.Title==='تصنيف مستقيمين في مجسم'));
 assert.ok(q.SummaryPoints.some(x=>x.includes('القاطع')));
});
test('Reuse exact planar classification only; noncoplanar and transversal remain teacher reviewed',()=>{
 const data=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=data.mappings.find(x=>x.lessonCode===lessonCode);
 assert.ok(entry);
 assert.deepEqual(entry.allowedQuestionFamilies,['usccss.g4.u07.l03.dot_grid_parallel_intersect']);
 assert.equal(entry.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 assert.equal(entry.officialOutcomeMapped,false);
 assert.ok(entry.teacherReviewRequiredFor.some(x=>x.includes('skew')));
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+lessonCode+'"]'));
 assert.ok(contracts.includes('G9_U12L01_COPLANAR_LINES_NUMERIC_PARTIAL_REUSE'));
});
test('Source-skill cooldown retains six Arabic open tasks without invented auto grading',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l01-parallel-skew-transversal-ar.html'),'utf8');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/المتبادلة داخليًا/);
 assert.match(html,/المتحالفتين داخليًا/);
 assert.match(html,/متخالفين/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/graded:false,submitted:false/);
 assert.match(html,/لم يُرسل إلى Edulytics/);
});
