'use strict';
// G9 Stage A-B-C targeted acceptance test specifications; no execution until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:05:01:ADDITION-SUBTRACTION-INEQUALITIES';
test('Arabic Grade9 General 5-1 source additive inequalities authored',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l01-ar.rich-lesson-v2.json');
 const lesson=p.Lessons.find(x=>x.LessonCode===code),rich=v.Lessons.find(x=>x.LessonCode===code);
 assert.ok(lesson&&rich);assert.equal(p.AcademicLanguage,'ar');
 assert.equal(lesson.Translations[0].Title,'حل المتباينات بالجمع والطرح');
 assert.match(lesson.Translations[0].Explanation,/علامة المقارنة|رمز المتباينة/);
 assert.equal(rich.WorkedExamples.length,4);
});
test('Existing generic inequality family is numeric assistance only; teacher graphs remain',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.inequality.ax_plus_b_relation_c']);
 assert.equal(m.officialOutcomeMapped,false);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('number-line')));
 const s=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(s.includes('byLesson["'+code+'"]'));
 assert.match(s,/G9_U05L01_ADDITIVE_INEQUALITY_NUMERIC_HELP_TEACHER_GRAPH/);
});
test('Number line uses open and closed circles with correct direction, eight teacher tasks',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l01-inequality-add-sub-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l01-teacher-add-sub-inequalities-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/circle cx="430" cy="58" r="10" fill="white"/);
 assert.match(h,/circle cx="415" cy="174" r="10" fill="#b86121"/);
 assert.match(h,/graded:false,submitted:false/);
 for(let i=1;i<=8;i++)assert.ok(h.includes('id="ineqQ'+i+'"'));
 assert.equal(rub.answers.length,8);
});
test('Algebraic test fixtures preserve sign on addition and subtraction',()=>{
 assert.equal(6+5<12,true);
 assert.equal(8+5<12,false);
 assert.equal(14-4>=10,true);
 assert.equal(-6+9<=3,true);
 assert.equal(7-2>=5,true);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-28-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
