'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:13:07:CONGRUENCE-TRANSFORMATIONS';
test('Grade 9 General U13L07 has original Arabic source-based transformations in both content layers',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u13-l07-ar.rich-lesson-v2.json');
 const q=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);assert.equal(pack.AcademicLanguage,'ar');assert.equal(q.CultureCode,'ar');
 assert.match(p.SourceLocator,/03-1\.pdf.*776/);
 for(const name of ['الانسحاب','الانعكاس','الدوران'])assert.ok(q.KeyConcepts.some(x=>x.Title===name),name);
 assert.ok(q.WorkedExamples.length>=3);
});
test('U13L07 does not falsely route rigid transformations to a different exact math family',()=>{
 const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const m=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,[]);
 assert.equal(m.practiceReadiness,'TEACHER_REVIEW_ONLY_STAGE_D_ENGINE_DECISION_PENDING');
 assert.ok(m.teacherReviewRequiredFor.includes('rotation coordinates'));
});
test('Original six-case rigid-motion cooldown is Arabic, local and teacher reviewed',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u13-l07-congruence-transformations-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
 assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.match(h,/لم تُرسل البيانات إلى Edulytics/);
});
