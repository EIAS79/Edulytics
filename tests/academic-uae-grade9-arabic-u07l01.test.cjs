'use strict';
// Stage A–B–C authored source fixtures. Run in Stage D, not here.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson='PED:UAE-MOE-MATH:L9:GENERAL:07:01:POLYNOMIAL-ADD-SUBTRACT';
const families=['uae.g9.general.u07.l01.add_coefficient','uae.g9.general.u07.l01.subtract_coefficient'];
test('Grade 9 General 7-1 Arabic title and source book are preserved',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const x=pack.Lessons.find(x=>x.LessonCode===lesson);
 assert.ok(x);
 assert.equal(x.Translations.find(y=>y.CultureCode==='ar').Title,'جمع كثيرات الحدود وطرحها');
 assert.match(x.SourceLocator,/02-1\.pdf printed365–372/);
 assert.match(x.Translations[0].Explanation,/الحدود المتشابهة/);
});
test('RichV2 has signed parentheses and polynomial subtraction with correct answers',()=>{
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u07-l01-ar.rich-lesson-v2.json');
 const x=rich.Lessons.find(x=>x.LessonCode===lesson);
 assert.ok(x);
 assert.match(x.WorkedExamples[1].Question,/٥س²−٣س/);
 assert.equal(x.WorkedExamples[1].Answer,'٣س²−٧س+٩');
 assert.equal(x.WorkedExamples[2].Answer,'٤س');
 assert.match(x.SummaryPoints.join(' '),/مراجعة المعلم/);
});
test('Exact kernel, mapping and practice contract agree on two coefficient families',()=>{
 const map=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=map.mappings.find(x=>x.lessonCode===lesson);
 assert.deepEqual(row.allowedQuestionFamilies,families);
 assert.equal(row.officialOutcomeMapped,false);
 const kernel=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 for(const family of families){assert.ok(kernel.includes('"'+family+'"'));assert.ok(contracts.includes('"'+family+'"'));}
 assert.ok(contracts.includes('byLesson["'+lesson+'"]'));
});
test('Original teacher reviewed tile activity and cool down do not assert platform grading',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u07-l01-polynomial-add-subtract-ar.html'),'utf8');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(const k of ['q1','q2','q3','q4','output','cooldown'])assert.ok(html.includes('id="'+k+'"'));
 assert.match(html,/graded:false/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/class="tile square negative"/);
 assert.match(html,/لم يتم إرسال البيانات/);
});
