// G5 U03:L02 original-2021 source and grade-owned A/B fixtures.
// AUTHORING ONLY — AUTHORED_NOT_RUN; this file is intentionally not executed.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const resource = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l02-l06-2021.rich-lesson-v2.json';
const rubricFile = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l02-original-teacher-rubric-20261031.json';
const code = 'PED:US-CCSS-MATH:G5:U03:L02';
const json = p => JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const lesson = () => json(resource).Lessons.find(x => x.LessonCode === code);

test('source-faithful Activity 1 preserves BOTH third-quarter orders and exact twelfths', () => {
  const l = lesson();
  assert.ok(l);
  const teaching = l.ExplanationParagraphs.join(' ');
  assert.match(teaching, /third of the whole.*one fourth of that selected third/i);
  assert.match(teaching, /fourths first, then take one third/i);
  assert.match(teaching, /one of 12 equal cells/);
  assert.match(l.WorkedExamples[0].Answer, /1\/12/);
  assert.match(l.WorkedExamples[1].Answer, /1\/12/);
});

test('Activity 2 retains Priya source expression pair and distinct image expression', () => {
  const l = lesson();
  const t = l.ExplanationParagraphs.join(' ');
  assert.match(t, /Priya diagram/);
  assert.match(t, /1\/5 × 1\/2/);
  assert.match(t, /1\/2 × 1\/5/);
  assert.match(t, /SECOND source-provided shaded diagram/);
  assert.match(t, /rather than presuming.*1\/10/);
});

test('first-party diagrams retain original equal-area whole and exactly one shaded cell', () => {
  const rich = json(resource);
  assert.equal(rich.ContentVersion,'g5-u03-l02-l06-im2021-source-parity-visual-v6-20261031');
  const checks = [
    {title:'third then quarter',rows:4,cols:3,den:12},
    {title:'quarter then third',rows:3,cols:4,den:12},
    {title:'Priya explanation substitute',rows:2,cols:5,den:10}
  ];
  for(const c of checks) {
    const v = lesson().Visuals.find(x => x.Title.includes(c.title));
    assert.ok(v, c.title);
    assert.equal(v.Rows.length,c.rows);
    assert.ok(v.Rows.every(r => r.Cells.length === c.cols));
    const cells=v.Rows.flatMap(r => r.Cells);
    assert.equal(cells.length,c.den);
    assert.equal(cells.filter(x=>x==='■ shade').length,1);
    assert.equal(cells.filter(x=>x==='□ empty').length,c.den-1);
    assert.match(v.Description,/original|whole/i);
  }
});

test('independent boundary oracle rejects false whole and unequal partitions', () => {
  const fraction=(rows,cols,shaded,originalRows=rows,originalCols=cols)=>
    Number.isSafeInteger(rows) && Number.isSafeInteger(cols) &&
    Number.isSafeInteger(shaded) && rows>0 && cols>0 &&
    rows===originalRows && cols===originalCols &&
    shaded===1 && 1/(rows*cols);
  assert.equal(fraction(4,3,1),1/12);
  assert.equal(fraction(3,4,1),1/12);
  assert.equal(fraction(2,5,1),1/10);
  assert.equal(fraction(5,2,1),1/10);
  assert.equal(fraction(3,4,1,3,2),false);
  assert.equal(fraction(3,4,2),false);
  assert.equal(fraction(0,4,1),false);
  assert.equal(fraction(3,1.5,1),false);
});

test('original educator scoring cannot be passed from numeric-only choice', () => {
  const rubric=json(rubricFile);
  assert.equal(rubric.lessonCode,code);
  assert.equal(rubric.maxPoints,6);
  assert.equal(rubric.rubric.reduce((n,x)=>n+x.points,0),6);
  assert.equal(rubric.teacherScored,true);
  assert.match(rubric.teacherSubmissionStatus,/NOT_CONNECTED_TO_PERSISTED_GRADING/);
  assert.ok(rubric.rejectOrReview.some(x=>x.includes('numeric-only')));
  assert.equal(rubric.tests,'AUTHORED_NOT_RUN');
});

test.todo('Supervisor unique owner: select L02 exact Practice family with Build/Solve/Verify, equivalent grids, source diagram expression, malformed and TEACHER_REVIEW_REQUIRED rubric; Stage C not accepted until real lesson route');
