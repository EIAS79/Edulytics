const test=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l04-l06-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l04-dilating-lines-angles-paper-20261031.json'),'utf8'));
test('source 4.1 4.2 4.3 tasks and authored 4.4 alternative',()=>{
 assert.deepEqual(p.Lessons.map(x=>x.LessonCode.slice(-7)),['U03:L04','U03:L05','U03:L06']);
 for(const s of ['Source 4.1 Angle Articulation','Source 4.2 Dilating Lines','Source 4.3 Proof in Parallel','Source 4.4 Cool-down'])
 assert.ok(p.Lessons[0].ExplanationParagraphs.some(x=>x.includes(s)),s);
 assert.equal(p.Lessons[0].WorkedExamples.length,3);
});
test('source declaration preserves angle assertion versus proved line claim',()=>{
 assert.equal(a.IndependentOracles.SourceAngleCongruenceStatus,'ASSERTION_NOT_PROVED_THIS_LESSON');
 assert.equal(a.IndependentOracles.SameLineInvariantForCentreOnLine,true);
 assert.equal(a.IndependentOracles.DistinctOffCentreParallelRequiresKNotOne,true);
 assert.equal(a.IndependentOracles.P4Scale,'1/2');
 assert.equal(a.IndependentOracles.P5Perimeter,27);
 assert.equal(a.IndependentOracles.P6X,'35/2');
});
test('human-selected assessment and exact fixture classes',()=>{
 assert.equal(a.Practice.Count,7);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
