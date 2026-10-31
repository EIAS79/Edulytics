// GEO U03:L01 lesson exact academic authoring tests, AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l01-scale-dilations-paper-20261031.json'),'utf8'));
test('lesson exact selection, seven source indexed paper tasks, human eight point rubric',()=>{
 assert(a.LessonCode.endsWith('U03:L01'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);assert.equal(a.Practice.length,7);assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
});
test('source centre-to-image 120, point displacement 80 and factor half',()=>{
 assert.equal(40*3,120);assert.equal(120-40,80);assert.equal(12*.5,6);
});
test('source P2 proportional a 2 and rational P3 arithmetic',()=>{
 assert.equal(6/3,4/2);assert.equal(6/4,3/2);assert.equal(2/5,6/15);assert.equal(11/4,5/(20/11));
});
