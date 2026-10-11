'use strict';

// A safety test: do not ever pass a currently blocked academic release.
const {test} = require('node:test');
const assert = require('node:assert/strict');
const {spawnSync} = require('node:child_process');
const path = require('node:path');
const tool = path.resolve(__dirname, '../tools/us-ccss-certification-gate.cjs');
function run(args = []) {
  const result = spawnSync(process.execPath, [tool, ...args], {encoding:'utf8'});
  assert.equal(result.error, undefined);
  const report = JSON.parse(result.stdout);
  return {result, report};
}
test('US CCSS academic gate defaults to read-only and reports NO-GO without signoffs', () => {
  const {result, report} = run();
  assert.equal(result.status, 0);
  assert.equal(report.certificationType, 'INTERNAL_EVIDENCE_BASED');
  assert.equal(report.scope, 'US-CCSS-MATH/CCSSM-2010/Grades-1-12');
  assert.equal(report.facts.kindergartenExcluded, true);
  assert.equal(report.facts.blueprintLessons, 1560);
  assert.equal(report.facts.officialParentStandards, 363);
  assert.equal(report.facts.subordinateClauses, 121);
  assert.equal(report.decision, 'NO-GO');
  const blockers = new Set(report.blockers.map(x=>x.code));
  for (const required of [
    'OFFICIAL_TEXT_NOT_CLOSED', 'SUBCLAUSE_NOT_CERTIFIED',
    'SEMANTIC_RELEASE_BLOCKED', 'INCOMPLETE_LESSONAUDIT',
    'INCOMPLETE_PARENTTRACE', 'INCOMPLETE_CHILDTRACE',
    'INDEPENDENT_ACADEMIC_APPROVAL_MISSING',
    'RELEASE_AUTHORIZATION_MISSING', 'PRODUCTION_VALIDATION_MISSING'
  ]) assert.ok(blockers.has(required), required + ' must be a release blocker');
});
test('strict certification mode rejects incomplete evidence', () => {
  const {result, report} = run(['--strict']);
  assert.equal(result.status, 2);
  assert.equal(report.decision, 'NO-GO');
});
test('tracked authority inputs are SHA-256 fingerprinted', () => {
  const {report} = run();
  for (const name of ['structural','ledger','clauses','semantic','textDiff']) {
    assert.match(report.inputFiles[name].sha256, /^[0-9a-f]{64}$/);
    assert.ok(report.inputFiles[name].path.startsWith('docs/curriculum/'));
  }
});

test('predeployment gate does not demand impossible postdeployment verification', () => {
  const {result, report} = run(['--stage=predeploy']);
  assert.equal(result.status, 0);
  assert.equal(report.gateStage, 'predeploy');
  assert.equal(report.decision, 'NO-GO');
  assert.ok(report.blockers.some(x => x.code === 'RELEASE_AUTHORIZATION_MISSING'));
  assert.ok(!report.blockers.some(x => x.code === 'PRODUCTION_VALIDATION_MISSING'));
});
test('final gate requires production verification even with the same baseline', () => {
  const {result, report} = run(['--stage=final', '--strict']);
  assert.equal(result.status, 2);
  assert.equal(report.gateStage, 'final');
  assert.ok(report.blockers.some(x => x.code === 'PRODUCTION_VALIDATION_MISSING'));
});
test('invalid gate stages cannot accidentally permit academic certification', () => {
  const result = spawnSync(process.execPath, [tool, '--stage=ignore-production'], {encoding:'utf8'});
  assert.notEqual(result.status, 0);
  assert.match(result.stderr, /Allowed gate stages/);
});
