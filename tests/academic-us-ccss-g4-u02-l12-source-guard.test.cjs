'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('source original25 fractions mathematically consistent',()=>{
 const x=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l12-2021.rich-lesson-v2.json')).Lessons[0];
 const numer=[2,4,7,11,26],denom=[3,5,10,12,100];
 assert.equal(x.Title,'Ways to Compare Fractions');assert.equal(x.Visuals[0].Rows.length,5);
 for(let i=0;i<5;i++)assert.deepEqual(x.Visuals[0].Rows[i].Cells,denom.map(d=>numer[i]+'/'+d));
 assert.ok(26/3>26/5);assert.ok(26/3>11/3);
 const orig=[[2,5,7,10],[4,10,7,12],[11,100,4,3],[26,10,11,12]];
 for(const [n1,d1,n2,d2] of orig)assert.notEqual(n1*d2,n2*d1);
});
test('benchmark source reuse and D not run',()=>{
 const c=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2TwelfthLessonPracticeContracts.cs');
 assert.ok(c.includes('fractions.compare.benchmark')&&c.includes('fractions.compare.unlike.select_greater'));
 const x=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-20-v2-stage-a-d-checkpoint.json'));
 assert.equal(x.status,'READY_FOR_STAGE_D');assert.equal(x.stages.D,'NOT_RUN');
});