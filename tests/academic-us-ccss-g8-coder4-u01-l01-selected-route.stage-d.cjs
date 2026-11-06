'use strict';
// Authored Stage C/D fixture; NOT_RUN during the sequential A-B-C pass.
// This remains a red gate until the single shared writer selects an actual lesson
// Practice contract or provides an equally auditable teacher-scored assessment route.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const text = file => fs.readFileSync(path.join(root, file), 'utf8');
const contract = JSON.parse(text('docs/curriculum/us-ccss-g8-coder4-u01-l01-academic-assessment-contract-20261030.json'));
const registry = text('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
const lesson = 'PED:US-CCSS-MATH:G8:U01:L01';
const candidate = contract.coordinateQuestionCandidate;

// Independent small coordinate oracle. It is not the production generator
// or permission to auto-grade the original partner-dance tasks.
function point(value) {
  if (typeof value !== 'string') return null;
  let s = value.trim();
  const hasOpen = s.startsWith('(');
  const hasClose = s.endsWith(')');
  if (hasOpen !== hasClose) return null;
  if (hasOpen) s = s.slice(1, -1);
  const m = s.match(/^\s*([+-]?\d+(?:\.\d+)?)\s*,\s*([+-]?\d+(?:\.\d+)?)\s*$/);
  if (!m) return null;
  const coordinates = [Number(m[1]), Number(m[2])];
  if (!coordinates.every(Number.isFinite) || coordinates.some(v => Math.abs(v) > Number.MAX_SAFE_INTEGER)) return null;
  return coordinates.map(v => Object.is(v, -0) ? 0 : v);
}

test('selected Stage C route exists for the exact lesson, not a fallback standard mapping', () => {
  assert.ok(registry.includes('"' + lesson + '"'),
    'Missing lesson-selected Practice contract; Supervisor 2 must provide an approved human assessment route or exact generator');
});

test('all accepted, rejected and malformed coordinate fixtures use an independent oracle', () => {
  for (const s of candidate.correct) assert.deepEqual(point(s), candidate.key, 'accepted ' + s);
  for (const s of candidate.wrong) assert.notDeepEqual(point(s), candidate.key, 'wrong ' + s);
  for (const s of candidate.malformed) assert.equal(point(s), null, 'malformed ' + s);
  assert.notDeepEqual(point('(-0,1)'), candidate.key);
  assert.notDeepEqual(point('(4,-0)'), candidate.key);
  assert.notDeepEqual(point('(4,1.0000001)'), candidate.key);
  assert.equal(point('1e309,1'), null);
});

test('source movement, explanation and cooldown remain teacher-reviewed, not auto-scored', () => {
  assert.deepEqual(contract.studentModes.map(x => x.scoring), [
    'TEACHER_ONLY', 'TEACHER_ONLY', 'TEACHER_ONLY', 'TEACHER_ONLY_EIGHT_POINT_RUBRIC'
  ]);
  assert.equal(contract.studentModes[3].rubric.maxPoints, 8);
  assert.equal(contract.studentModes[3].rubric.criteria.length, 4);
  assert.equal(candidate.runtimeEnabled, false);
});
