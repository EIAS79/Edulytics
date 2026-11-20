'use strict';
// Prepared Grade 9 A-B-C source competency checks. Run only in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const root=path.resolve(__dirname,'..');const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const code='PED:UAE-MOE-MATH:L9:GENERAL:12:05:PROVING-PARALLEL-LINES';
test('Arabic Grade 9 converse-parallel lesson exists with textbook source',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 const q=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u12-l05-ar.rich-lesson-v2.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(p&&q);assert.equal(q.Title,'إثبات توازي المستقيمات');
 assert.match(p.SourceLocator,/03-1\.pdf.*687/);
 assert.ok(q.KeyConcepts.some(x=>x.Title.includes('عكس المتناظرتين')));
 assert.ok(q.WorkedExamples.some(x=>x.Answer==='س=١٤'));
});
test('Only true numeric supplementary prerequisite reused, while parallelism proof stays teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['geometry.angles.supplementary']);
 assert.equal(m.officialOutcomeMapped,false);assert.equal(m.practiceReadiness,'PREPARED_NOT_RUN_STAGE_D');
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('converse')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.ok(c.includes('G9_U12L05_SUPPLEMENTARY_PRECONDITION_ONLY'));
});
test('Six Arabic cooldown proofs require human review; never claim numeric auto grade',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u12-l05-prove-parallel-ar.html'),'utf8');
 assert.match(h,/lang="ar" dir="rtl"/);for(let i=1;i<=6;i++)assert.ok(h.includes('id="q'+i+'"'));
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/لم يُرسل إلى Edulytics/);
});
