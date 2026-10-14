'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { buildIndex } = require('../tools/us-ccss-b01-v9-math-element-index.cjs');
function sample() {
  const lessons = Array.from({ length: 500 }, (_, n) => ({
    lessonCode: 'TEST:LESSON:' + String(n).padStart(3, '0'),
    sourceSemanticSha256: 'a'.repeat(64)
  }));
  const records = lessons.map((m, i) => ({
    lessonCode: m.lessonCode, canonicalBodySha256: 'b'.repeat(64),
    translations: [{ cultureCode: 'en',
      explanation: i === 0 ? 'Solve √9 and 2^3, then explain the cube root.' :
        i === 1 ? 'Use a number line and a fraction 2/3.' : 'Count a collection.',
      workedExamples: i === 2 ? 'Compare centimeter lengths.' : '' }]
  }));
  const packs = [0, 1, 2, 3].map(i => ({
    packCode: 'US-CCSS-MATH', versionCode: 'CCSSM-2010',
    lessons: records.slice(i * 125, (i + 1) * 125)
  }));
  return { manifest: { batchId: 'B01', expectedLessonCount: 500, lessons }, packs };
}
test('Indexes all 500, without confusing candidate elements with taught skills or certification', () => {
  const result = buildIndex(sample());
  assert.deepEqual(result.coverage, { expected: 500, observed: 500, unique: 500, missing: 0 });
  assert.equal(result.academicDecision, 'NOT_CERTIFIED');
  assert.equal(result.eligibleForAutomaticRepair, false);
  assert.equal(result.rows.length, 500);
  assert.ok(result.rows.every(r => r.primaryTeachingSkill === null &&
    r.defectDecision === 'NOT_ASSESSED' && r.certificationDecision === 'NOT_CERTIFIED'));
  assert.deepEqual(result.rows[0].mathematicalElements.map(x => x.family).sort(),
    ['exponent', 'root']);
  assert.deepEqual(result.rows[1].mathematicalElements.map(x => x.family).sort(),
    ['fraction', 'visual_number_line']);
  assert.equal(result.byFamily.root.candidateLessons, 1);
  assert.equal(result.byFamily.fraction.candidateLessons, 1);
});
test('Does not infer a repair or a defect from incidental vocabulary', () => {
  const f = sample();
  f.packs[0].lessons[0].translations[0].explanation =
    'A common mistake says 2 + 2 = 5. Explain why the equation is wrong.';
  const result = buildIndex(f);
  assert.equal(result.rows[0].defectDecision, 'NOT_ASSESSED');
  assert.equal(result.eligibleForAutomaticRepair, false);
  assert.ok(result.rows[0].mathematicalElements.some(x => x.family === 'algebra_equation'));
});
test('Rejects missing or duplicate content instead of reporting fake 500 coverage', () => {
  const f = sample();
  f.packs[1].lessons.splice(0, 1);
  assert.throws(() => buildIndex(f), /Missing learner body/);
  const g = sample();
  g.packs[0].lessons[1].lessonCode = g.packs[0].lessons[0].lessonCode;
  assert.throws(() => buildIndex(g), /Duplicate learner content/);
});
test('Rejects stale or missing fingerprints and a shortened manifest', () => {
  const f = sample();
  f.packs[1].lessons[0].canonicalBodySha256 = '';
  assert.throws(() => buildIndex(f), /Missing learner body fingerprint/);
  const g = sample();
  g.manifest.lessons.pop();
  assert.throws(() => buildIndex(g), /exactly 500/);
});
