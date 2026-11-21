'use strict';
// Stage A-B-C authoring specification, executed only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:09:01:INTRODUCTION-TO-SET-THEORY';
const family='uae.g9.general.u09.l01.set_membership_cardinality';

test('Grade 9 General sets introduction has Arabic source content and correct mathematics',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u09-l01-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&lesson);assert.match(p.SourceLocator,/02-1\.pdf printed 502/);
 assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'مقدمة في نظرية المجموعات');
 assert.equal(lesson.CultureCode,'ar');
 assert.ok(lesson.WorkedExamples.some(x=>x.Title==='الانتماء وعدد العناصر'));
 assert.equal(lesson.WorkedExamples.find(x=>x.Title==='التكرار في المجموعة').Answer,'{٢،٤،٦}، |ب|=٣');
 assert.ok(lesson.CommonMistakes.some(x=>x.Mistake.includes('∅ و{∅}')));
});
test('Set membership/cardinality extends existing exact engine with independent domain constraints',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.allowedQuestionFamilies,[family]);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const shared=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/SupportingPracticeCompletionEngine.cs'),'utf8');
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.ok(shared.includes('"'+family+'" => G9SetMembershipCardinality(random)'));
 assert.match(shared,/candidate-first\)\/step<count/);
 assert.match(shared,/candidate>first\+\(count-1\)\*step\+1/);
 assert.match(shared,/submitted|parsed==independent/);
 assert.ok(row.teacherReviewRequiredFor.includes('set equality justification'));
});
test('Sets Cool-down includes membership, cardinality, empty set and teacher-only local export',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u09-l01-set-theory-intro-ar.html'),'utf8');
 assert.match(html,/<html lang="ar" dir="rtl">/);
 for(let i=1;i<=5;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/٣∈أ/);
 assert.match(html,/∅ و\{∅\}/);
 assert.match(html,/graded:false,submitted:false,review:'TEACHER_REVIEW_REQUIRED'/);
 assert.match(html,/لم تُرسل البيانات إلى Edulytics ولم تُحتسب درجة/);
});
