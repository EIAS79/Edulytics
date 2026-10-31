// Geometry U02:L08 source-specific static fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'), assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l08-perpendicular-bisector-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l07-l09-2021.rich-lesson-v2.json');
test('U02:L08 exact selected teacher paper route, seven practice, rubric, no machine grade',()=>{
 assert(a.LessonCode.endsWith('U02:L08'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,7);
 assert.equal(a.HumanRubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
});
test('L08 removes premature SSS from converse proof and keeps three worked examples',()=>{
 const l=r.Lessons.find(x=>x.LessonCode===a.LessonCode);
 assert(l && l.WorkedExamples.length===3);
 assert(l.WorkedExamples[1].Method.includes('SAS'));assert(!l.WorkedExamples[1].Method.includes('SSS'));
 assert(l.ExplanationParagraphs.some(x=>x.includes('without premature SSS')));
});
test('five selected true converses include only B C D',()=>{
 assert.deepEqual(a.IndependentOracles.P1_TrueConverses,['B','C','D']);
});
test('coordinate bisector locus for A=(-3,0), B=(3,0)',()=>{
 for(const y of [-4,0,7]) assert.equal(Math.hypot(3,y),Math.hypot(-3,y));
 assert(Math.hypot(4,2)!==Math.hypot(2,2));
});