const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const c=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l02-ordered-correspondence-ssa-paper-20261031.json'),'utf8'));
test('U02:L02 source order and prior/next lesson pack integrity',()=>{
 assert.deepEqual(c.Lessons.map(x=>x.LessonCode.slice(-7)),['U02:L01','U02:L02','U02:L03']);
 assert.equal(c.Lessons[1].WorkedExamples.length,3);
 for(const x of ['Source 2.1 Math Talk','Source 2.2 Which Triangles','Source 2.3 Are These Parts','Source practice P1-P7'])
   assert.ok(c.Lessons[1].ExplanationParagraphs.some(p=>p.includes(x)),x);
});
test('SSA admits two positive different triangles from the same side-side-nonincluded angle',()=>{
 const a1=4*Math.sqrt(3)-3, a2=4*Math.sqrt(3)+3;
 for(const t of [a1,a2]){
  assert.ok(t>0);
  assert.ok(Math.abs(t*t-16*Math.cos(Math.PI/6)*t+(64-25))<1e-10);
 }
 assert.ok(Math.abs(a2-a1)>1);
 assert.equal(a.IndependentSSACounterexample.DifferentTriangles,true);
});
test('paper-only human scoring, exact seven tasks and fixtures',()=>{
 assert.equal(a.Practice.Count,7);
 assert.deepEqual(a.Practice.Items.map(x=>x.SourceProblem),[1,2,3,4,5,6,7]);
 assert.equal(a.Task.Rubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.SelectedPracticeRoute.SavedTeacherResult,'UNVERIFIED_STAGE_D');
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Incorrect','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
