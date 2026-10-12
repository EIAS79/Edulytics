'use strict';
const {test} = require('node:test');
const assert = require('node:assert/strict');
const {screenSource, scanBlueprints} = require('../tools/us-ccss-license-edition-policy.cjs');
const path = require('node:path');

const base = {
  publisher: 'Illustrative Mathematics', edition: 'IM K-12 Math First Edition (2021)',
  rootUrl: 'https://im.kendallhunt.com/HS/teachers/1',
  licenseClaim: 'CC BY 4.0'
};

test('Verified first-edition metadata stays PENDING, not licensed by a code bot', () => {
  assert.equal(screenSource(base).status, 'PENDING_INDEPENDENT_RIGHTS_REVIEW');
});

test('v.360 with a forged CC BY 4.0 claim is blocked for commercial use', () => {
  assert.equal(screenSource({...base, edition: 'IM v.360, 2024'}).status,
    'BLOCKED_V360_EDITION_LICENSE_MISMATCH');
  assert.equal(screenSource({...base, rootUrl: 'https://accessim.org/teachers/1'}).status,
    'BLOCKED_V360_EDITION_LICENSE_MISMATCH');
});

test('v.360 CC BY-NC is not commercially cleared and a missing license is blocked', () => {
  assert.equal(screenSource({...base, edition:'IM v.360',
    licenseClaim:'CC BY-NC 4.0'}).status, 'BLOCKED_COMMERCIAL_V360_LICENSE');
  assert.equal(screenSource({...base, licenseClaim:''}).status,
    'BLOCKED_UNDECLARED_LICENSE');
});

test('Noncommercial restrictions on third-party material are not silently waived', () => {
  assert.equal(screenSource({...base, publisher:'Another Publisher',
    licenseClaim:'CC BY-NC-SA 4.0'}).status, 'BLOCKED_NONCOMMERCIAL_SOURCE');
});

test('All 17 committed blueprint sources are screened without automatic rights approvals', () => {
  const dir = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');
  const result = scanBlueprints(dir);
  assert.equal(result.blueprintCount, 17);
  assert(result.sourceCount >= 17);
  assert(result.entries.every(row => row.status.startsWith('BLOCKED_') ||
    row.status === 'PENDING_INDEPENDENT_RIGHTS_REVIEW'));
  assert(result.entries.every(row => row.blueprint && row.sourceFile));
});
