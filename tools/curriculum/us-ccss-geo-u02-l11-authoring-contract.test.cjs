// Geometry U02:L11 source-specific authoring fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l11-ssa-optional-paper-20261031.json'),'utf8'));
test('Optional source lesson has exact selected teacher paper seven skills',()=>{
 assert(a.LessonCode.endsWith('U02:L11'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);assert.equal(a.Practice.length,7);assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
});
test('Correct SSA ray intersection classification source examples',()=>{
 const roots=(A,c,a)=>{const b=c*Math.cos(A*Math.PI/180),disc=b*b-(c*c-a*a);if(disc<0)return [];return [b-Math.sqrt(disc),b+Math.sqrt(disc)].filter(x=>x>1e-8)};
 assert.equal(roots(30,10,6).length,2);assert.equal(roots(30,10,4).length,0);assert.equal(roots(30,10,12).length,1);
 assert.equal(roots(30,6,4).length,2);
});
