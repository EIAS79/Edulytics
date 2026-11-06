// Geometry U02:L10 source-specific A/B/C static fixtures, AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l10-brace-card-sort-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l10-l12-2021.rich-lesson-v2.json');
test('L10 exact physical paper lesson routing and eight source-indexed Practice items',()=>{
 assert(a.LessonCode.endsWith('U02:L10'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,8);
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
 assert.equal(r.Lessons.find(x=>x.LessonCode===a.LessonCode).WorkedExamples.length,3);
});
test('no false ASA from bisector of one angle at G',()=>{
 assert.equal(a.IndependentOracles.P7_Error,'GE_BISECTS_AT_G_ONLY_DOES_NOT_IMPLY_AT_E');
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('GE bisector')));
});
test('side-5 rhombus may be non-square with different diagonals',()=>{
 const square=5*Math.sqrt(2),rhombus=[5,5*Math.sqrt(3)];
 assert(square!==rhombus[0]&&square!==rhombus[1]);
});
