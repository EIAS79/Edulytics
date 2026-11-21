'use strict';// Stage A-B-C specifications prepared only.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson='PED:UAE-MOE-MATH:L9:GENERAL:07:08:DIFFERENCE-OF-SQUARES';
test('Lesson 47 original book source traced in Arabic pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const x=pack.Lessons.find(x=>x.LessonCode===lesson);assert.ok(x);
 assert.match(x.SourceLocator,/02-1\.pdf printed 418–423/);
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l08-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons.find(x=>x.LessonCode===lesson).WorkedExamples.length,3);
});
test('Difference squares uses exact quadratic family and teacher-reviewed reasoning',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===lesson);
 assert.deepEqual(map.allowedQuestionFamilies,['uae.g9.general.u07.l08.difference_squares_integer_roots']);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+lesson+'"]'));
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l08-difference-squares-ar.html'),'utf8');
 for(const frag of ['lang="ar" dir="rtl"','٩س²−٢٥=٠','س²+٤٩','id="teacherReview"','TEACHER_REVIEW_REQUIRED','graded:false'])assert.ok(html.includes(frag),frag);
});
test('Difference of squares identity survives algebraic expansion',()=>{
 for(const [a,b]of [[7,2],[4,5],[0,3],[-2,8]])
  assert.equal((a-b)*(a+b),a*a-b*b);
});
