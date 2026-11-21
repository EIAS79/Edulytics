'use strict';
// Grade9 Stage A-B-C authored test cases; do not execute until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:08:06:OPERATIONS-ON-RADICAL-EXPRESSIONS';
const family='uae.g9.general.u08.l06.like_radical_coefficient';

test('Grade9 General U08L06 Arabic lessons preserve source radical operations',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u08-l06-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);
 assert.match(p.SourceLocator,/02-1\.pdf printed 484/);
 assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'العمليات على التعابير الجذرية');
 assert.equal(lesson.CultureCode,'ar');
 for(const expected of ['الجمع بعد التبسيط','الطرح من جذور متشابهة','الضرب بالتوزيع','حاصل ضرب المرافقين'])
  assert.ok(lesson.WorkedExamples.some(x=>x.Title===expected));
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='الجمع بعد التبسيط').Answer,'٥√٣');
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='حاصل ضرب المرافقين').Answer,'−١');
});
test('Like-radical engine extension is mapped, independently verified and cannot claim full symbolic autograding',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 assert.ok(row.teacherReviewRequiredFor.includes('conjugate products'));
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(engine.includes('"'+family+'" => G9LikeRadicalCoefficient(random)'));
 assert.match(engine,/p\["mode"\]==0 \? p\["a"\]\+p\["b"\] : p\["a"\]-p\["b"\]/);
 assert.match(engine,/submitted==\(mode==0\?a\+bb:a-bb\)/);
 assert.match(engine,/radicand is not \(2 or 3 or 5 or 6 or 7\)/);
});
test('Arabic five-item Cool-down for radical operations is teacher-only and local',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u08-l06-radical-operations-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=5;i++)assert.ok(html.includes('id="q'+i+'"'));
 for(const expression of ['√١٢+√٢٧','٧√٣−٢√٣','√٥(٢+√٥)','(√٣+٢)(√٣−٢)','(√٢+√٣)²'])
  assert.ok(html.includes(expression),expression);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics ولم تُحتسب درجة/);
});
