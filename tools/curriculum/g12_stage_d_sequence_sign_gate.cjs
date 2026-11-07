'use strict';
const fs=require('node:fs');
const path=require('node:path');
const assert=require('node:assert/strict');
const root=path.resolve(__dirname,'../../');
const base='us-ccss-g12-precalculus-u11-l01';
const student=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2',base+'.rich-lesson-v2.json'),'utf8')).Lessons[0];
const teacher=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment',base+'-firstparty-teacher-led.json'),'utf8'));
const alt=teacher.IndependentStudentMathQuestions.filter(q=>q.IndependentStudentQuestion.includes('alternating'));
assert.equal(alt.length,12,'Original G12 Section 11.1 alternate-source-skills count must remain 12');
const oracle=new Map();
for(const q of alt){
 const match=q.IndependentStudentQuestion.match(/a_n=\(\u22121\)\^n\((-?\d+)\+n\)/);
 assert.ok(match,'Alternating sequence must declare its exact first-indexed formula');
 const a=Number(match[1]);
 const result='['+[1,2,3,4,5].map(n=>((-1)**n)*(a+n)).join(', ')+'].';
 assert.equal(q.TeacherMath,result,'Wrong alternating teacher answer for source exercise '+q.OriginalExercise);
 oracle.set(q.IndependentStudentQuestion,result);
}
let workedCount=0;
for(const example of student.WorkedExamples){
 const expected=oracle.get(example.Question);
 if(!expected)continue;
 assert.equal(example.Answer,expected,'Student worked example disagrees with independent oracle');
 assert.ok(example.Steps.some(step=>step.includes(expected)),'Worked solution step missing corrected result');
 workedCount++;
}
assert.equal(workedCount,2,'Both authored alternating worked examples must be synchronized');
let tryCount=0;
for(const tryIt of teacher.IndependentTryIt??[]){
 const expected=oracle.get(tryIt.Question);
 if(!expected)continue;
 assert.equal(tryIt.TeacherExpectedMath,expected,'Try It not synchronized');
 tryCount++;
}
assert.equal(tryCount,1,'Authored alternating Try It must be synchronized');
const cool=teacher.IndependentCoolDown;
if(cool?.StudentQuestions?.length){
 const expected=oracle.get(cool.StudentQuestions[0]);
 if(expected)assert.equal(cool.TeacherExpectedMath[0],expected);
}
console.log(JSON.stringify({status:'PASS',section:'11.1',sourceAlternatingSkills:alt.length,
 workedExamples:workedCount,tryIts:tryCount,mathematicalOracle:'independent n=1..5 arithmetic'}));
