// Geometry U02:L05 source-aligned authoring-only checks. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l05-point-segment-zigzag-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l04-l06-2021.rich-lesson-v2.json');
test('exact paper lesson route with seven source-indexed teacher tasks and no machine proof grading',()=>{
 assert(a.LessonCode.endsWith('U02:L05'));
 assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.deepEqual(a.Practice.map(x=>x.Id),Array.from({length:7},(_,i)=>'P'+(i+1)));
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.Lessons.find(x=>x.LessonCode===a.LessonCode).WorkedExamples.length,3);
});
test('rigid translations preserve point identity and circle radius',()=>{
 const p=[-2,3],v=[5,-1],q=p.map((x,i)=>x+v[i]);
 assert.deepEqual(q,[3,2]);
 assert.equal(Math.hypot(...[3,4]),5);
});
test('ordered zigzag proof and negative equivalence requirements',()=>{
 assert.equal(a.IndependentOracles.ZigzagStepOrder.length,5);
 assert.deepEqual(a.IndependentOracles.P6_TrueChoices,['B','D']);
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('all segments')));
});
