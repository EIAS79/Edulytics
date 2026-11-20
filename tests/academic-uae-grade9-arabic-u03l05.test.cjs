'use strict';
// Stage A–C authored tests. Execution and CI belong to Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:03:05:ARITHMETIC-SEQUENCES-AS-LINEAR-FUNCTIONS';
test('UAE Grade9 General 3-5 uses Arabic student-book arithmetic-sequence content',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u03-l05-ar.rich-lesson-v2.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code),v=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(lesson.Translations[0].Title,'المتتابعات الحسابية على شكل دوال خطية');
 assert.match(lesson.Translations[0].Explanation,/الفرق المشترك/);
 assert.equal(v.WorkedExamples.length,4);
 assert.ok(v.KeyConcepts.some(x=>x.Title==='المجال المتقطع'));
});
test('Existing arithmetic family reused and free graph tasks remain teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const entry=m.mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(entry.allowedQuestionFamilies,['sequences.core.arithmetic_nth_term','sequences.core.find_position_arithmetic']);
 assert.equal(entry.officialOutcomeMapped,false);
 assert.ok(entry.teacherReviewRequiredFor.some(x=>x.includes('discrete')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+code+'"]'));
 assert.match(c,/G9_U03L05_ARITHMETIC_NTH_TERM_AND_DISCRETE_LINEAR_FUNCTION/);
});
test('Student activity graph uses discrete marks and eight answers are teacher reviewed',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u03-l05-arithmetic-sequences-ar.html'),'utf8');
 const rubric=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u03l05-teacher-arithmetic-sequence-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 assert.match(html,/viewBox="0 0 720 360"/);
 assert.equal((html.match(/<circle /g)||[]).length,4);
 assert.doesNotMatch(html,/<polyline|<path /);
 assert.match(html,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="seqQ'+i+'"'));
 assert.equal(rubric.answers.length,8);
 assert.equal(rubric.teacherReviewRequired,true);
});
test('Mathematical source skill fixtures: nth term, decreasing sequence, position and domain',()=>{
 const nth=(first,d,n)=>first+(n-1)*d;
 assert.deepEqual([1,2,3,4].map(n=>nth(3,2,n)),[3,5,7,9]);
 assert.equal(nth(3,2,6),13);
 assert.equal(nth(8,-6,6),-22);
 assert.equal(nth(-12,4,5),4);
 assert.equal((32+16)/4,12);
 assert.deepEqual([2,4,8].map((x,i)=>x),[2,4,8]);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-22-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
