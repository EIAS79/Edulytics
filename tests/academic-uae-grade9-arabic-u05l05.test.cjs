'use strict';
// Prepared in A-B-C; execution belongs to Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:05:05:ABSOLUTE-VALUE-INEQUALITIES';
test('Absolute inequality Arabic content covers inside/outside and negative threshold',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l05-ar.rich-lesson-v2.json');
 const lesson=pack.Lessons.find(x=>x.LessonCode===id),rich=v.Lessons.find(x=>x.LessonCode===id);
 assert.ok(lesson&&rich);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(rich.WorkedExamples.length,4);
 assert.match(lesson.Translations[0].Explanation,/الشعاعين/);
 assert.match(rich.WorkedExamples[3].Answer,/∅/);
});
test('Arabic original number lines, 8 open questions and teacher-only export',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l05-absolute-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l05-teacher-absolute-inequalities-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 for(let n=1;n<=8;n++)assert.ok(html.includes('id="absIneqQ'+n+'"'),n);
 assert.match(html,/circle cx="330" cy="65" r="10" fill="white"/);
 assert.match(html,/circle cx="530" cy="186" r="10" fill="white"/);
 assert.match(html,/graded:false,submitted:false/);
 assert.equal(rub.answers.length,8);assert.equal(rub.autoGrade,false);
});
test('Only existing linear substep family is auto-eligible and absolute source requires teacher',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.equal(m.officialOutcomeMapped,false);
 assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.inequality.ax_plus_b_relation_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('full absolute-value')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
});
test('Source-inspired absolute inequalities fixtures distinguish strict and inclusive boundaries',()=>{
 const inside=x=>Math.abs(x+2)<11;assert.equal(inside(-13),false);assert.equal(inside(0),true);assert.equal(inside(9),false);
 const outside=x=>Math.abs(x-6)>3;assert.equal(outside(3),false);assert.equal(outside(2),true);assert.equal(outside(10),true);
 const inclusive=x=>Math.abs(3*x+6)>=12;assert.equal(inclusive(-6),true);assert.equal(inclusive(2),true);assert.equal(inclusive(0),false);
 for(const x of [-100,0,100]){assert.equal(Math.abs(x-1)<-2,false);assert.equal(Math.abs(x+2)>=-1,true);}
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-32-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
