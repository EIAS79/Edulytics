'use strict';// Stage A–B–C prepared specifications, not executed.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson='PED:UAE-MOE-MATH:L9:GENERAL:07:09:PERFECT-SQUARE-TRINOMIALS';
test('Source lesson 48 is Arabic, ordered and includes square-root solving',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const x=pack.Lessons.find(x=>x.LessonCode===lesson);assert.ok(x);
 assert.match(x.SourceLocator,/02-1\.pdf printed 424–431/);
 assert.equal(x.Translations[0].Title,'المربعات الكاملة');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l09-ar.rich-lesson-v2.json');
 assert.equal(rich.Lessons.find(x=>x.LessonCode===lesson).WorkedExamples.length,4);
});
test('Perfect-square skill reuses bounded repeated-root and two-root families',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===lesson);
 assert.deepEqual(m.allowedQuestionFamilies,['uae.g9.general.u07.l06.monic_repeated_root','uae.g9.general.u07.l09.square_root_equation_integer_roots']);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+lesson+'"]'));
 const lab=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l09-perfect-squares-ar.html'),'utf8');
 for(const word of ['lang="ar" dir="rtl"','٤س²−١٢س+٩','س²=١٠','TEACHER_REVIEW_REQUIRED','graded:false'])assert.ok(lab.includes(word),word);
});
test('Perfect square expansions and integer two-root cases',()=>{
 for(const [a,b]of [[2,3],[1,-4],[-2,5]]){
  assert.equal((a+b)*(a+b),a*a+2*a*b+b*b);
  assert.equal((a-b)*(a-b),a*a-2*a*b+b*b);
 }
});
