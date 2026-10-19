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

test('Public attribution names only the 2021 first edition and disclaims brand and third-party image rights',()=>{
  const fs=require('node:fs');
  const html=fs.readFileSync(path.join(__dirname,
    '../src/Edulytics.Web/wwwroot/content-sources-licenses.html'),'utf8');
  assert.match(html,/US Common Core Mathematics — Grade 1, first edition \(2021\)/);
  assert.match(html,/Based on IM® K–12 Math by Illustrative Mathematics®/);
  assert.match(html,/https:\/\/im\.kendallhunt\.com\/k5\/teachers\/grade-1/);
  assert.match(html,/https:\/\/creativecommons\.org\/licenses\/by\/4\.0\//);
  assert.match(html,/modified or newly authored/);
  assert.match(html,/does not imply endorsement, partnership/);
  assert.match(html,/Individual third-party photographs, illustrations/);
  assert.match(html,/IM v\.360 is licensed under/);
  const policy=require('../artifacts/academic-us-ccss/01-edition-specific-licence-evidence.json');
  assert.equal(policy.reviewStatus,'INDEPENDENT_RIGHTS_SIGNOFF_PENDING');
  assert.equal(policy.academicGate,'NO-GO');
});
