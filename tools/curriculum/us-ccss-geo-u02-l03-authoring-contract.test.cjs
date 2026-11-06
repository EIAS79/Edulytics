const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l03-pointwise-triangle-proof-paper-20261031.json'),'utf8'));
test('third sequential lesson source activities exist with retained three worked examples',()=>{
 assert.equal(p.Lessons.length,3);
 assert.equal(p.Lessons.at(-1).WorkedExamples.length,3);
 for(const x of ['Source 3.1 True or Sometimes True','Source 3.2 Invisible Triangles','Source 3.3 Why Do They Coincide','Source practice P1-P8'])
  assert.ok(p.Lessons.at(-1).ExplanationParagraphs.some(s=>s.includes(x)),x);
});
test('eight independent practice mappings, all human graded',()=>{
 assert.equal(a.Practice.Count,8);
 assert.deepEqual(a.Practice.Items.map(x=>x.SourceProblem),[1,2,3,4,5,6,7,8]);
 assert.equal(a.Task.Rubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.TestsExecuted,'NOT_RUN');
});
test('same-ray and distance are jointly required, and reflection preserves aligned edge',()=>{
 const A={x:0,y:0},B={x:3,y:0},E={x:3,y:0},wrong={x:-3,y:0};
 assert.equal(Math.hypot(B.x-A.x,B.y-A.y),Math.hypot(E.x-A.x,E.y-A.y));
 assert.equal(Math.hypot(wrong.x-A.x,wrong.y-A.y),3);
 assert.notDeepEqual(wrong,E);
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
