'use strict';
// Source aligned test specifications: Stage A-B-C prepares; Stage D executes.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:12:04:EQUATIONS-OF-LINES';
test('Grade 9 General line equations are authored in Arabic pack and RichV2',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l04-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);assert.equal(q.Title,'معادلات المستقيم');assert.match(p.SourceLocator,/03-1\.pdf.*678/);
 assert.ok(q.KeyConcepts.some(x=>x.Title==='الأفقي والرأسي'));
 assert.ok(q.WorkedExamples.some(x=>x.Answer==='ص=٢س+١'));
 assert.ok(q.WorkedExamples.some(x=>x.Answer==='س=٢'));
});
test('Only numerically supported linear subskills reuse existing verifier; equation writing remains teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule','geometry.coordinate.y_intercept_from_rule','geometry.coordinate.gradient_between_points']);
 assert.equal(m.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 assert.ok(m.teacherReviewRequiredFor.includes('vertical line equations'));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.ok(c.includes('G9_U12L04_LINEAR_RULE_NUMERIC_PARTIAL_REUSE'));
});
test('Original cooldown preserves six Arabic free equations and no artificial auto grade',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l04-line-equations-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);
 for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/لم يُرسل إلى Edulytics/);
});
