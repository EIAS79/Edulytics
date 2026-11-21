'use strict';
// Stage A-B-C checks prepared only. Run in Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>JSON.parse(fs.readFileSync(path.join(root,x),'utf8'));
const id='PED:UAE-MOE-MATH:L9:GENERAL:05:04:COMPOUND-INEQUALITIES';
test('G9 compound inequalities have Arabic source-aligned examples and graphs',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u05-l04-ar.rich-lesson-v2.json');
 const a=pack.Lessons.find(x=>x.LessonCode===id),v=rich.Lessons.find(x=>x.LessonCode===id);
 assert.ok(a&&v);assert.equal(pack.AcademicLanguage,'ar');
 assert.equal(v.WorkedExamples.length,4);assert.match(a.Translations[0].Explanation,/تقاطع/);
});
test('Compound AND/OR union and intersection have teacher tasks, not false grading',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u05-l04-compound-inequalities-ar.html'),'utf8');
 const rub=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u05l04-teacher-compound-inequalities-rubric.json');
 assert.match(html,/lang="ar" dir="rtl"/);
 assert.match(html,/٣≤س/);assert.match(html,/«و» تقاطع/);
 for(let i=1;i<=8;i++)assert.ok(html.includes('id="compoundQ'+i+'"'));
 assert.match(html,/graded:false,submitted:false/);
 assert.equal(rub.answers.length,8);
});
test('No compound score is inferred from generic single-inequality solver',()=>{
 const m=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===id);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,['algebra.linear.inequality.ax_plus_b_relation_c']);
 assert.ok(m.teacherReviewRequiredFor.some(x=>x.includes('intersection AND')));
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.ok(source.includes('byLesson["'+id+'"]'));
});
test('Mathematics fixtures distinguish empty intersection and universal union',()=>{
 const and=x=>x>=5&&x<2;const or=x=>x<5||x>=2;
 for(const x of [-100,-1,0,2,4,5,100]){assert.equal(and(x),false);assert.equal(or(x),true);}
 const triple=x=>-2<=x-3&&x-3<4;
 assert.equal(triple(1),true);assert.equal(triple(7),false);
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/lesson-31-stage-a-c-checkpoint.json');
 assert.equal(cp.closed,true);assert.equal(cp.stageC.testsRun,'WRITTEN_NOT_RUN');
});
