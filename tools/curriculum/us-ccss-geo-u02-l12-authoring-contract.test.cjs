// Geometry U02:L12 original first-party static fixture, AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l12-quadrilateral-proof-paper-20261031.json'),'utf8'));
test('L12 paper route, seven source-indexed practice prompts, 8-point rubric',()=>{
 assert(a.LessonCode.endsWith('U02:L12'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);assert.equal(a.Practice.length,7);assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
});
test('80-20-80 triangle with unspecified side 5 has 2 noncongruent classes',()=>{
 assert.equal(a.IndependentOracles.P7_NoncongruentTriangleClasses,2);
 const otherBase=5*Math.sin(20*Math.PI/180)/Math.sin(80*Math.PI/180);
 const otherLeg=5*Math.sin(80*Math.PI/180)/Math.sin(20*Math.PI/180);
 assert(otherBase<5&&otherLeg>5);
});
