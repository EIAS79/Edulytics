'use strict';
// Source A-B-C authored acceptance specifications; run during Stage D only.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));const id='PED:UAE-MOE-MATH:L9:GENERAL:06:06:SYSTEMS-OF-INEQUALITIES';
test('Grade9 General 6-6 source and RichV2 explain shared halfplane intersection in Arabic',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const v=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u06-l06-ar.rich-lesson-v2.json');
 const lesson=p.Lessons.find(x=>x.LessonCode===id),rich=v.Lessons.find(x=>x.LessonCode===id);
 assert.ok(lesson&&rich);assert.equal(p.AcademicLanguage,'ar');assert.equal(rich.Title,'أنظمة المتباينات');
 assert.match(lesson.Translations[0].Explanation,/تقاطع/);
 assert.ok(rich.WorkedExamples.some(x=>x.Answer.includes('∅')));
});
test('Arabic graph has real shared region, solid and dashed boundaries, teacher-reviewed written work',()=>{
 const h=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u06-l06-systems-of-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u06l06-teacher-inequality-systems-rubric.json');
 assert.match(h,/lang="ar" dir="rtl"/);
 assert.match(h,/aria-labelledby="svgTitle svgDesc"/);
 assert.match(h,/stroke-dasharray="10 7"/);
 assert.match(h,/M170 300 L170 96 L380 198 Z/);
 for(let n=1;n<=8;n++)assert.ok(h.includes('id="answer'+n+'"'));
 for(let n=1;n<=3;n++)assert.ok(h.includes('id="cool'+n+'"'));
 assert.match(h,/graded:false,submitted:false/);
 assert.match(h,/TEACHER_REVIEW_REQUIRED/);
 assert.equal(rub.autoGrade,false);assert.equal(rub.answers.length,8);assert.equal(rub.coolDownAnswers.length,3);
});
test('Existing coordinate family is only a boundary-value aid; full system remains teacher reviewed',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['geometry.coordinate.evaluate_linear_rule']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('shade INTERSECTION')));
 const c=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const e=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.ok(c.includes('byLesson["'+id+'"]'));
 assert.ok(e.includes('string.Equals(scopeKey, "'+id+'"'));
 assert.ok(e.includes('مراجعة تظليل نظام المتباينات'));
});
test('Strict and nonstrict source points and contextual costs are mathematically consistent',()=>{
 const feasible=(x,y)=>y>=x+1&&y<-x+5;
 assert.equal(feasible(1,3),true);assert.equal(feasible(2,3),false);
 assert.equal(feasible(3,1),false);
 for(const x of [-4,0,3])assert.equal((x+4)>(x+1),true);
 const impossible=(x,y)=>y>x+3&&y<=x+1;
 for(const x of [-4,0,3])for(const y of [-3,2,8])assert.equal(impossible(x,y),false);
 assert.ok(20*6+60*2<=480);assert.ok(6+2>=8);
 assert.ok(4>=1+2&&4< -1+6);
 assert.equal(4< -2+6,false);
});
