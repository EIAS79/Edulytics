const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '../..');
const code = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L22';
const content = JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l19-l22-2021.rich-lesson-v2.json'),'utf8'));
const contract = JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u01-l22-creative-construction-paper-assessment-20261031.json'),'utf8'));

test('L22 remains sole authored course addition in existing 4-lesson pack', () => {
  assert.deepEqual(content.Lessons.map(x=>x.LessonCode.slice(-7)), ['U01:L19','U01:L20','U01:L21','U01:L22']);
  const l22 = content.Lessons.at(-1);
  for (const a of ['22.1 Notice and Wonder','22.2 Duplicate a Design','22.3 Make Your Own Design','22.4 Make Their Design'])
    assert.ok(l22.ExplanationParagraphs.some(x=>x.includes(a)), a);
  assert.equal(l22.WorkedExamples.length,3);
  assert.equal(l22.Visuals.length,0);
});
test('L22 paper assessment preserves source order and ten cumulative problems', () => {
  assert.equal(contract.LessonCode,code);
  assert.equal(contract.SourceTaskCrosswalk.length,6);
  assert.equal(contract.Practice.Count,10);
  assert.equal(contract.Practice.Items.length,10);
  assert.deepEqual(contract.Practice.Items.map(x=>x.SourceProblem),[1,2,3,4,5,6,7,8,9,10]);
  assert.equal(contract.SelectedPracticeRoute.Mode,'PAPER_TEACHER_SCORED_FIRST_PARTY');
  assert.equal(contract.SelectedPracticeRoute.MachineGrade,false);
  assert.equal(contract.TestsExecuted,'NOT_RUN');
});
test('Rubric and fixtures reject image-only or scalar proxy claims', () => {
  const rubric=contract.Task.Rubric;
  assert.equal(rubric.TotalMax,8);
  assert.equal(rubric.Criteria.reduce((x,y)=>x+y.MaxPoints,0),8);
  for (const key of ['Correct','Equivalent','Incorrect','Boundary','Malformed'])
    assert.ok(contract.AuthoredFixtures[key].length>=2,key);
  assert.match(contract.AuthoredFixtures.Incorrect.join(' '),/decoration/);
  assert.match(contract.AuthoredFixtures.Boundary.join(' '),/tangent/);
  assert.equal(contract.SelectedPracticeRoute.ExactAppAssignmentAndPersistence,'UNVERIFIED_STAGE_D_ISSUE_453');
});
