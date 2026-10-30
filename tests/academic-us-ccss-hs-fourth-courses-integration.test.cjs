'use strict';
// Static safety/ownership validation, NOT live .NET or authenticated school acceptance.
// Run: node --test tests/academic-us-ccss-hs-fourth-courses-integration.test.cjs
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.join(__dirname, '..');
const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const sha = /^[0-9a-f]{64}$/;

test('seven proposed successors remain disallowed and preserve immutable provenance', () => {
  const successors = read('docs/curriculum/us-ccss-hs-fourth-courses-v8-pending-successors.json');
  const phase = read('docs/curriculum/us-ccss-phase04-hs-content-repair-pilot.json');
  const approved = read('docs/curriculum/us-ccss-verified-body-repairs.v1.json');
  assert.equal(successors.gate, 'NO-GO');
  assert.equal(successors.successors.length, 7);
  assert.equal(phase.repairs.length, 7);
  assert.equal(phase.gate, 'NO-GO');
  assert.equal(approved.allowUnlistedContentChanges, false);
  assert.equal(approved.repairs.length, 8);
  const codes = new Set();
  for (const record of successors.successors) {
    assert.ok(!codes.has(record.lessonCode));
    codes.add(record.lessonCode);
    assert.equal(record.academicDecision, 'PENDING');
    assert.equal(record.materializationAuthorization, 'DENIED');
    assert.match(record.originalPublisherSourceSha256, sha);
    assert.match(record.previousCanonicalBodySha256, sha);
    if (record.courseCode === 'ADV-ALG-FUNC') {
      assert.match(record.proposedCanonicalBodySha256, sha);
      const historical = phase.repairs.find(x => x.lessonCode === record.lessonCode);
      assert.ok(historical);
      assert.equal(record.previousCanonicalBodySha256, historical.correctedCanonicalBodySha256);
      assert.equal(record.originalPublisherSourceSha256, historical.unchangedPublisherSourceSha256);
    } else {
      assert.equal(record.courseCode, 'ADV-TRIG-GEO');
      assert.notEqual(record.previousTranslationSha256, record.proposedTranslationSha256);
      assert.equal(record.reportedProposedCanonicalBodySha256, record.previousCanonicalBodySha256);
      assert.match(record.canonicalDigestPolicy, /UNRESOLVED/);
    }
  }
  assert.equal([...codes].filter(x => x.includes('ADV-ALG-FUNC')).length, 2);
  assert.equal([...codes].filter(x => x.includes('ADV-TRIG-GEO')).length, 5);
});

test('matrix official codes resolve to narrow target, not one-variable equations', () => {
  const map = read('src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-map.v1.json');
  const rules = read('src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-rules.v1.json');
  const targets = read('src/Edulytics.Core/Mathematics/Curriculum/supporting-practice-target-rules.v1.json');
  const actual = map.entries.filter(x => /^CCSS:HSA-REI\.C\.[89]$/.test(x.outcomeCode));
  assert.equal(actual.length, 2);
  assert.ok(actual.every(x => x.targetRuleId === 'hs-matrix-linear-systems'));
  assert.ok(actual.every(x => x.resolutionRuleId === 'ccss-hs-matrix-systems-exact'));
  const specific = rules.rules.find(x => x.id === 'ccss-hs-matrix-systems-exact');
  assert.equal(specific.targetRuleId, 'hs-matrix-linear-systems');
  assert.equal(specific.codePatterns.length, 1);
  assert.ok(new RegExp(specific.codePatterns[0]).test('CCSS:HSA-REI.C.8'));
  assert.ok(!new RegExp(specific.codePatterns[0]).test('CCSS:HSA-REI.C.6'));
  assert.ok(targets.rules.some(x => x.id === 'hs-matrix-linear-systems'));
});

test('all seven Fourth Courses contract identities are integration-pending, not student-visible', () => {
  const source = fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'), 'utf8');
  const family = read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
  const ownedIds = [
    'usccss.hs.matrix.coefficients_2x2',
    'usccss.hs.matrix.solve_2x2',
    'usccss.hs.matrix.inverse_2x2',
    'usccss.hs.matrix.classify_2x2',
    'usccss.hs.matrix.coefficients_3x3',
    'usccss.hs.matrix.solve_3x3',
    'usccss.hs.matrix.inverse_3x3',
    'usccss.hs.matrix.row_reduce_3x3',
    'usccss.hs.log.inverse_power',
    'usccss.hs.log.compare_order',
    'trig.unit_circle_reflection_exact', 'trig.principal_inverse',
    'trig.model_all_time_solutions', 'solid_geometry.cavalieri_sphere_volume',
    'coordinates.focal_distance_conics'
  ];
  const ids = family.families.filter(x => ownedIds.includes(x.id));
  assert.equal(ids.length, 15);
  // Family registration is needed for strict registry/interaction audits,
  // but no producer may promote an IntegrationPending family into a
  // READY_VERIFIED lesson or production route.
  assert.ok(ids.every(x => x.productionRouting === false &&
    x.lessonPracticeRouting === true &&
    x.status === 'IntegrationPending'));
  for (const course of ['ADV-ALG-FUNC', 'ADV-TRIG-GEO']) {
    for (let i = 1; i <= (course === 'ADV-ALG-FUNC' ? 2 : 5); i++) {
      const lesson = `PED:US-CCSS-MATH:HS:FOURTH:${course}:U01:L${String(i).padStart(2, '0')}`;
      assert.ok(source.includes(`byLesson["${lesson}"] = new LessonPracticeContract(`));
    }
  }
  assert.match(source, /"INTEGRATION_PENDING"/);
  assert.doesNotMatch(source, /"USCCSS_HS_SHARED_INTEGRATION_PENDING",\s*"READY_VERIFIED"/);
});
