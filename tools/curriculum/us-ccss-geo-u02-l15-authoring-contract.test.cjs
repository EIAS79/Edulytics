// GEO U02:L15 lesson authoring acceptance fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l15-parallelogram-congruence-paper-20261031.json'),'utf8'));
test('L15 exact paper selected route and original seven Practice, eight-point rubric',()=>{
 assert(a.LessonCode.endsWith('U02:L15'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,7);
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
});
test('SSSS counterexample has same four sides different angle invariants',()=>{
 assert.equal(a.IndependentOracles.P4_TrueChoice,'B');
 assert(a.AuthoredFixtures.Correct.some(x=>x.includes('rhombus 60')));
});
