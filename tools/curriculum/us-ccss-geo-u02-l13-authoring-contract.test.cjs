// Geometry U02:L13 source academic acceptance fixtures AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l13-parallelogram-diagonals-paper-20261031.json'),'utf8'));
test('L13 exact permitted paper assessment and seven indexed source problems',()=>{
 assert(a.LessonCode.endsWith('U02:L13'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);assert.equal(a.Practice.length,7);assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
});
test('Parallelogram equal diagonals theorem noncircular exact givens',()=>{
 assert(a.IndependentOracles.ParallelogramIffDiagonalsBisect);
 assert(a.IndependentOracles.CongruentDiagonalsParallelogramImpliesRectangle);
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('All parallelograms rectangles')));
});
