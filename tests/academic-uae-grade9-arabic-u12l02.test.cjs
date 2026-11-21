'use strict';
// Grade 9 General Stage A-C authored checks, reserved for Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:12:02:ANGLES-PARALLEL-LINES';
test('Arabic textbook parallel angle relationships exist in pack and RichV2',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l02-ar.rich-lesson-v2.json');
 const p=pack.Lessons.find(x=>x.LessonCode===code),q=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);assert.equal(p.Translations.find(x=>x.CultureCode==='ar').Title,'الزوايا والمستقيمات المتوازية');
 assert.match(p.SourceLocator,/03-1\.pdf.*660/);
 assert.ok(q.KeyConcepts.some(x=>x.Title==='التحالف الداخلي'));
 assert.ok(q.KeyConcepts.some(x=>x.Title.includes('عكس مسلمة')));
 assert.ok(q.WorkedExamples.some(x=>x.Answer==='س=٣٠'));
});
test('Parallel angle exact numeric families are reused, and proofs stay teacher reviewed',()=>{
 const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(mapping);assert.deepEqual(mapping.allowedQuestionFamilies,['geometry.angles.parallel_lines','geometry.angles.supplementary']);
 assert.equal(mapping.officialOutcomeMapped,false);
 assert.equal(mapping.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(contracts.includes('byLesson["'+code+'"]'));
 assert.ok(contracts.includes('G9_U12L02_CORRESPONDING_AND_SUPPLEMENTARY_NUMERIC'));
});
test('Six source-skills cooldown tasks stay independent and not automatically graded',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l02-parallel-line-angles-ar.html'),'utf8');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(let i=1;i<=6;i++)assert.ok(html.includes('id="q'+i+'"'));
 assert.match(html,/متبادلتان داخليًا/);
 assert.match(html,/عكس مسلمة/);
 assert.match(html,/TEACHER_REVIEW_REQUIRED/);
 assert.match(html,/graded:false/);
 assert.match(html,/لم يُرسل إلى Edulytics/);
});
