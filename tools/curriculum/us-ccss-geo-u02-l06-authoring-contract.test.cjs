// Geometry U02:L06 source-order proof and assessment fixture. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l06-sas-isosceles-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l04-l06-2021.rich-lesson-v2.json');
test('source-aligned U02L06 paper route and six original practice skills',()=>{
 assert(a.LessonCode.endsWith('U02:L06'));assert.equal(a.SelectedPracticeRoute.LessonCode,a.LessonCode);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.Practice.length,6);
 assert.deepEqual(a.IndependentOracles.P4_OriginalStatementOrder,[3,1,2,4,5]);
 assert.deepEqual(a.IndependentOracles.P6_TrueChoices,['B','D','E']);
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
});
test('repair removes premature SSS from isosceles worked example',()=>{
 const l=r.Lessons.find(x=>x.LessonCode===a.LessonCode);
 assert(l && l.WorkedExamples.length===3);
 assert(l.WorkedExamples[1].Method.includes('SAS'));
 assert(!l.WorkedExamples[1].Method.includes('SSS'));
 assert(l.ExplanationParagraphs.some(x=>x.includes('NOT a premature SSS theorem')));
});
test('third-side strict bounds with 5 and 12 and right angle',()=>{
 const hyp=Math.hypot(5,12);assert.equal(hyp,13);
 assert(7<hyp && hyp<17);
});
