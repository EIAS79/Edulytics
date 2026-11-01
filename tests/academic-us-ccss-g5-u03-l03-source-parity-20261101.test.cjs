// G5 U03:L03 IM 2021 teacher lesson/preparation source A–B authoring fixture.
// AUTHORED_NOT_RUN: no Node execution, dotnet execution, CI or Stage D.
'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname,'..');
const file = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l02-l06-2021.rich-lesson-v2.json';
const rubricFile = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l03-original-teacher-rubric-20261101.json';
const source = p => JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const target = () => source(file).Lessons.find(x => x.LessonCode === 'PED:US-CCSS-MATH:G5:U03:L03');

test('original grade 5 lesson 3 title and full source timeline', () => {
  const l = target();
  assert.equal(l.Title,'Multiply Unit Fractions');
  const s = l.ExplanationParagraphs.join(' ');
  assert.match(s,/Estimation Exploration/);
  assert.match(s,/1\/2.*1\/6.*1\/12/);
  assert.match(s,/Notice Patterns in Expressions/);
  assert.match(s,/FOUR equal columns/);
  assert.match(s,/2, 3, 4, 5/);
  assert.match(s,/Write a Multiplication Equation/);
  assert.match(s,/1\/5.*1\/3.*1\/15/);
  assert.match(s,/1\/2.*1\/6.*1\/12/);
  assert.match(s,/1\/4.*1\/6.*1\/24/);
  assert.match(s,/Stronger and Clearer Each Time/);
  assert.match(s,/TEACHER_REVIEW_REQUIRED/);
});

test('first-party A–D diagrams preserve four equal columns, exactly one shaded cell', () => {
  const l = target();
  for (const [label,rows,answer] of [['A',2,'1/8'],['B',3,'1/12'],['C',4,'1/16'],['D',5,'1/20']]) {
    const grid = l.Visuals.find(v => v.Title.endsWith('source ' + label + ')'));
    assert.ok(grid,label);
    assert.equal(grid.Rows.length,rows);
    assert.ok(grid.Rows.every(r => r.Cells.length === 4));
    const cells = grid.Rows.flatMap(r => r.Cells);
    assert.equal(cells.filter(c => c === '■ shade').length,1);
    assert.equal(cells.length,rows*4);
    assert.ok(grid.Description.includes(answer));
    assert.match(grid.Description,/NOT publisher artwork/);
  }
});

test('source Activity 2 math and seven-row extension are separate', () => {
  const l = target();
  const expected = ['1/15','1/12','1/24'];
  assert.ok(expected.every(value => l.WorkedExamples.some(ex => ex.Answer === value || ex.Answer === value + '.')));
  const extension = l.WorkedExamples.find(ex => ex.Title.startsWith('Optional extension'));
  assert.ok(extension);
  assert.match(extension.Question,/EXTENSION ONLY/);
  assert.match(l.ExplanationParagraphs[3],/FIRST source-provided shaded image/);
  assert.match(l.ExplanationParagraphs[3],/SECOND source diagram/);
  for (const [rows,cols,answer] of [[3,5,'1/15'],[2,6,'1/12'],[4,6,'1/24']]) {
    const grid = l.Visuals.find(x => x.Description.includes('1/' + (rows*cols)) &&
      x.Rows.length === rows && x.Rows.every(r => r.Cells.length === cols));
    assert.ok(grid,'sourced Activity 2 '+answer);
  }
});

test('teacher rubric is not falsely machine-completed', () => {
  const r = source(rubricFile);
  assert.equal(r.teacherScored,true);
  assert.equal(r.maxPoints,7);
  assert.equal(r.rubric.reduce((n,x)=>n+x.points,0),7);
  assert.match(r.teacherSubmissionStatus,/NOT_CONNECTED_TO_PERSISTED_GRADING/);
  assert.equal(r.stageD,'NOT_RUN');
  assert.equal(r.tests,'AUTHORED_NOT_RUN');
});

test.todo('Shared Practice route and exact issuer family for L03 require separate exclusive single-writer authorization; do not infer readiness from the lesson-local candidate');
