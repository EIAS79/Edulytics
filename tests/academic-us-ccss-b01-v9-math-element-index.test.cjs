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
  const sourceIndex = {
    batchId: 'B01', expectedLessonCount: 500, matchedToManifest: 500,
    rows: lessons.map((m, i) => ({
      ordinal: i + 1, lessonCode: m.lessonCode,
      localRelativePath: '01_Grade_1_IM_K5\\Exact_Sources\\lesson-' + (i + 1) + '.html',
      localFileByteSha256: 'c'.repeat(64), localFileBytes: 1024,
      state: 'REFERENCE_FILE_INVENTORIED_NOT_ACADEMICALLY_CERTIFIED'
    }))
  };
  const teacherActivityIndex = {
    batchId: 'B01', sourceCoverage: 500, sourceAvailable: 500,
    representationCueKeys: ['DOT_IMAGES', 'NUMBER_LINE', 'FRACTION_PARTS',
      'CLOCK_FACE', 'PLACE_VALUE', 'RECTANGULAR_ARRAY', 'MEASUREMENT_SCALE', 'DATA_GRAPH'],
    rows: lessons.map((m, i) => [i + 1, m.lessonCode, 'd'.repeat(64),
      'e'.repeat(64), 2, 'f'.repeat(64), 7, i === 1 ? 6 : 0])
  };
  return { manifest: { batchId: 'B01', expectedLessonCount: 500, lessons },
    sourceIndex, teacherActivityIndex, packs };
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
  assert.equal(result.rows[0].originalTeacherSource.rawFileSha256, 'c'.repeat(64));
  assert.equal(result.rows[0].originalTeacherSource.state,
    'HASH_INVENTORIED_NOT_ACADEMICALLY_VERIFIED');
  assert.deepEqual(result.rows[1].originalTeacherActivitySnapshot.representationCueCandidates,
    ['NUMBER_LINE', 'FRACTION_PARTS']);
  assert.equal(result.byTeacherCue.NUMBER_LINE.candidateLessons, 1);
  assert.equal(result.rows[1].originalTeacherActivitySnapshot.state,
    'SOURCE_ACTIVITY_HEURISTIC_NOT_L4_OR_DEFECT_PROOF');
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

test('Rejects source inventory drift, not simply missing learner content', () => {
  const f = sample();
  f.sourceIndex.rows[10].lessonCode = 'WRONG:SOURCE:LESSON';
  assert.throws(() => buildIndex(f), /Source inventory provenance missing\/mismatched/);
  const g = sample();
  g.sourceIndex.rows[0].localFileByteSha256 = '';
  assert.throws(() => buildIndex(g), /Source inventory provenance missing\/mismatched/);
});

test('Teacher source cue identity or signature drift fails closed', () => {
  const f = sample();
  f.teacherActivityIndex.rows[0][1] = 'SOME:OTHER:LESSON';
  assert.throws(() => buildIndex(f), /Teacher activity identity\/hash\/flag mismatch/);
  const g = sample();
  g.teacherActivityIndex.rows[0][7] = 256;
  assert.throws(() => buildIndex(g), /Teacher activity identity\/hash\/flag mismatch/);
});
