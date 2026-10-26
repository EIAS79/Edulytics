 'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
const load=p=>JSON.parse(rd(p));
const cmp=(a,b)=>{const [n,d]=a.split('/').map(Number),[m,e]=b.split('/').map(Number);return Math.sign(n*e-m*d)};
test('original source four four-fraction sets are exact, unsolved in learner RichV2',()=>{
 const v=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l16-2021.rich-lesson-v2.json').Lessons[0];
 assert.equal(v.Title,'Compare and Order Fractions');
 const rows=v.Visuals[0].Rows;
 assert.equal(rows.length,4);
 const original=[['3/12','2/4','2/3','1/8'],['8/5','5/6','11/12','11/10'],['21/20','9/10','6/5','101/100'],['5/8','2/5','3/7','3/6']];
 const sorted=[['1/8','3/12','2/4','2/3'],['5/6','11/12','11/10','8/5'],['9/10','101/100','21/20','6/5'],['2/5','3/7','3/6','5/8']];
 for(let i=0;i<4;i++){
  assert.deepEqual(rows[i].Cells,original[i]);
  const ordered=[...original[i]].sort(cmp);
  assert.deepEqual(ordered,sorted[i]);
  for(let j=1;j<4;j++) assert.ok(cmp(ordered[j-1],ordered[j])<0);
 }
 assert.equal(v.WorkedExamples.some(x=>x.Question.includes('21/20')),false);
});
test('stage A-C source limitations and exact reuse are honest; Stage D not run',()=>{
 const c=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2SixteenthLessonPracticeContracts.cs');
 const reg=rd('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 assert.match(c,/fractions\.compare\.unlike\.order_three/);
 assert.match(c,/fractions\.compare\.unlike\.select_greater/);
 assert.match(reg,/foreach\(var reviewed in UsCcssG4Unit2SixteenthLessonPracticeContracts\.Reviewed\)/);
 const cp=load('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-24-v2-stage-a-d-checkpoint.json');
 assert.equal(cp.sourceVisualParityProven,false);
 assert.equal(cp.sourceHashes.studentPdf,null);
 assert.equal(cp.stages.D,'NOT_RUN');
 assert.equal(cp.academicApproved,false);
 const pg=load('artifacts/curriculum-delivery/US-CCSS-MATH/G4/full-grade-progress.json');
 assert.equal(pg.lastClosedOrdinal,24);assert.equal(pg.currentLessonOrdinal,25);
});
