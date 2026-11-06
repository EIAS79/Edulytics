// Geometry U02:L09 lesson-specific source A/B/C fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l09-sss-parallelogram-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l07-l09-2021.rich-lesson-v2.json');
test('L09 exact teacher selected SSS reflection task and seven paper Practice items',()=>{
 assert(a.LessonCode.endsWith('U02:L09'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,7);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.Lessons.find(x=>x.LessonCode===a.LessonCode).WorkedExamples.length,3);
});
test('SSS radius reflection and correct source-indexed true converse set',()=>{
 assert(a.IndependentOracles.SSS_BASE.includes('ST_TO_GH'));
 assert.equal(a.IndependentOracles.ReflectionAxis,'GH_PERP_BISECTOR_OF_U_PRIME_J');
 assert.deepEqual(a.IndependentOracles.P4_TrueConverses,['B','C','E']);
});
test('degenerate triple excluded and 4,4,t open interval',()=>{
 const valid=(a,b,c)=>a>0&&b>0&&c>0&&a+b>c&&a+c>b&&b+c>a;
 assert(valid(1.5,2.4,2));assert(!valid(2,3,6));
 assert(!valid(4,4,8));assert(valid(4,4,7.9));
});
