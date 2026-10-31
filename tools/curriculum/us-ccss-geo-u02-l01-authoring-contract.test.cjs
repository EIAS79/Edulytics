const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const content=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l01-corresponding-parts-paper-assessment-20261031.json'),'utf8'));
test('only U02 L01 content augmented, source sequence and three old worked examples preserved',()=>{
  assert.equal(content.Lessons.length,3);
  assert.deepEqual(content.Lessons.map(x=>x.LessonCode.slice(-7)),['U02:L01','U02:L02','U02:L03']);
  for(const s of ['Source 1.1 Notice and Wonder','Source 1.2 If We Know','Source 1.3 Making Quadrilaterals','Source Practice P1-P6']){
    assert.ok(content.Lessons[0].ExplanationParagraphs.some(x=>x.includes(s)),s);
  }
  assert.equal(content.Lessons[0].WorkedExamples.length,3);
});
test('first-party teacher assessment documents six source practice tasks and no auto grading',()=>{
  assert.equal(a.Practice.Count,6);
  assert.deepEqual(a.Practice.Items.map(x=>x.SourceProblem),[1,2,3,4,5,6]);
  assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
  assert.equal(a.SelectedPracticeRoute.SavedTeacherScore,'UNVERIFIED_STAGE_D');
  assert.equal(a.Task.Rubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
  assert.equal(a.TestsExecuted,'NOT_RUN');
});
test('mathematical boundary, counterexample and malformed fixtures are authored',()=>{
  for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
  assert.match(a.AuthoredFixtures.Correct.join(' '),/parallelogram/);
  assert.match(a.AuthoredFixtures.Boundary.join(' '),/noncollinear/);
  assert.equal(a.PublisherCooldown,'ORIGINAL_COOLDOWN_UNVERIFIED');
});
