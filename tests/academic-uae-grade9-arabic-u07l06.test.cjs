'use strict';
// Prepared at Stage A–B–C: execution belongs to Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson='PED:UAE-MOE-MATH:L9:GENERAL:07:06:MONIC-QUADRATIC-FACTORING';
test('Grade 9 General lesson 45 uses actual historical General book source and Arabic content',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const x=p.Lessons.find(x=>x.LessonCode===lesson);
 assert.ok(x);assert.equal(p.AcademicLanguage,'ar');
 assert.match(x.SourceLocator,/02-1\.pdf printed 405–411/);
 assert.equal(x.Translations.find(y=>y.CultureCode==='ar').Title,'حل معادلات من الصورة س²+ب س+ج=٠');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l06-ar.rich-lesson-v2.json');
 const v=rich.Lessons.find(x=>x.LessonCode===lesson);
 assert.equal(v.CultureCode,'ar');
 assert.equal(v.WorkedExamples.length,3);
 assert.match(v.KeyConcepts.map(x=>x.Rule).join(' '),/خاصية|س=أ أو س=ب/);
});
test('Grade 9 General lesson 45 only uses the quadratic-specific exact numeric contract',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const x=m.mappings.find(x=>x.lessonCode===lesson);
 assert.ok(x);
 assert.deepEqual(x.allowedQuestionFamilies,['uae.g9.general.u07.l06.monic_distinct_roots','uae.g9.general.u07.l06.monic_repeated_root']);
 assert.equal(x.officialOutcomeMapped,false);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+lesson+'"]'));
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 for(const name of ['Supports','Build','Solve','Verify'])
  assert.ok(engine.includes('UaeG9MonicQuadraticQuestions.'+name+'('));
});
test('Original Arabic cooldown covers roots, repeat roots, sign reasoning and teacher review',()=>{
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l06-monic-factor-ar.html'),'utf8');
 for(const text of ['lang="ar" dir="rtl"','س²+٥س+٦=٠','س²−٦س+٩=٠','(س−٢)(س+٣)','id="teacherReview"','id="submission"','TEACHER_REVIEW_REQUIRED','graded:false'])
  assert.ok(s.includes(text),text);
 assert.match(s,/لم تُرسل البيانات إلى Edulytics/);
});
test('Both expected source factor fixtures satisfy the original polynomial identity',()=>{
 const roots=[[-3,-4,7,12],[4,-3,-1,-12],[4,4,-8,16],[2,3,-5,6]];
 for(const [m,n,b,c]of roots){
  assert.equal(-(m+n),b);
  assert.equal(m*n,c);
 }
});
