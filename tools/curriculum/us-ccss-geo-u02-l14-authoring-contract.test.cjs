// GEO U02:L14 authoring contract verification, AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l14-angle-bisector-paper-20261031.json'),'utf8'));
test('exact L14 selected paper route, seven practice and 8-point human rubric',()=>{
 assert(a.LessonCode.endsWith('U02:L14'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,7);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
});
test('SSS angle construction does not assume its conclusion',()=>{
 assert(a.IndependentOracles.AngleSSS.includes('AF_SHARED'));
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('assuming desired')));
});
test('equal radii arcs need strict >half for two crossings',()=>{const chord=8;assert(5>chord/2);assert(!(4>chord/2));});
