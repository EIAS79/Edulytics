'use strict';
// UAE G9 General: Stage A-C tests are authored but reserved for Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:12:06:PERPENDICULAR-DISTANCE-MIDPOINTS';
test('Both UAE General textbook sources are represented in single Arabic lesson 73',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l06-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);
 assert.match(p.SourceLocator,/03-1\.pdf.*695/);
 assert.match(p.SourceLocator,/01\.pdf.*385/);
 assert.equal(q.Title,'التعامدات والمسافة');
 assert.ok(q.KeyConcepts.some(x=>x.Title==='المسافة بين نقطتين'));
 assert.ok(q.KeyConcepts.some(x=>x.Title==='نقطة المنتصف'));
 assert.ok(q.KeyConcepts.some(x=>x.Title==='البعد العمودي'));
});
test('Reused numeric families are prerequisites only; exact distances and midpoint must be teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.allowedQuestionFamilies,['usccss.g4.u07.l10.perpendicular_right_angle_judgment','geometry.coordinate.gradient_between_points']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('two-point Euclidean')));
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('segment midpoint')));
 assert.equal(m.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.ok(c.includes('G9_U12L06_PERPENDICULAR_AND_GRADIENT_PREREQUISITES'));
});
test('Textbook index preserves separate 7-3 midpoint skills in source supplement',()=>{
 const d=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-82-target-index.json');
 const x=d.records.find(x=>x.ordinal===73);
 assert.equal(x.LessonArabicTitle,'التعامدات والمسافة');
 assert.ok(x.SourceRefs.includes('03-1.pdf:12-6'));
 assert.ok(x.SourceRefs.includes('01.pdf:7-3'));
 assert.equal(d.SOURCE_INDEX_COMPLETE,false);
});
test('Original cooldown covers eight teacher tasks and never falsely grades',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l06-perpendicular-distance-midpoints-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);for(let i=1;i<=8;i++)assert.ok(h.includes('id="q'+i+'"'));
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/لم يُرسل إلى Edulytics/);
});
