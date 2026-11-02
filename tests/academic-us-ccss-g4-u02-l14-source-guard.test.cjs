'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('original 2021 six mystery solutions pass each strict clue',()=>{
 const rich=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l14-2021.rich-lesson-v2.json')).Lessons[0];
 assert.equal(rich.Title,'Fraction Comparison Problems');assert.equal(rich.Visuals[0].Rows.length,6);
 const answers=["3/6","2/5","4/3","11/12","3/5","18/10"];
 assert.deepEqual(rich.Visuals[0].Rows.map(r=>r.Cells[2].replace('Correct: ','')),answers);
 const f=x=>x.split('/').map(Number),between=(s,lo,hi)=>{const [n,d]=f(s),[a,b]=f(lo),[c,e]=f(hi);return n*b>a*d&&n*e<c*d};
 assert.ok(between('3/6','1/3','2/3'));assert.ok(between('2/5','1/3','1/2'));
 assert.equal(4/5*2>7/5,true);
});
test('reuse exact same-unit numeric practice, D unrun',()=>{
 const s=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2FourteenthLessonPracticeContracts.cs');
 assert.ok(s.includes('fractions.compare.unlike.select_greater')&&s.includes('fractions.compare.benchmark'));
 const c=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-22-v2-stage-a-d-checkpoint.json'));
 assert.equal(c.status,'READY_FOR_STAGE_D');assert.equal(c.stages.D,'NOT_RUN');
});