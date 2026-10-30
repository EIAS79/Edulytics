'use strict';
// Authored fixture; not a Stage D or live Practice test.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const file = p => fs.readFileSync(path.join(root, p), 'utf8');
const cd = JSON.parse(file('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l01-edulytiks-cooldown.json'));
const cursor = JSON.parse(file('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l01-abc-cursor.json'));
test('Grade 7 L01 independent original Cool-down math', () => {
  assert.equal(cd.lessonCode,'PED:US-CCSS-MATH:G7:U01:L01');
  assert.deepEqual([6/3,8/4,10/5],[2,2,2]);
  assert.notEqual(9/5,6/3);
  assert.deepEqual([3*.5,4*.5,5*.5],[1.5,2,2.5]);
  assert.equal(cd.publisherCooldown,'NOT_VERIFIED_GATED');
  assert.match(cd.status,/NOT_IN_PRODUCT/);
});
test('Grade 7 L01 acceptance must remain blocked', () => {
  assert.equal(cursor.READY_FOR_STAGE_D,false);
  assert.match(cursor.C_STATUS,/BLOCKED/);
  assert.equal(cursor.testExecutionStatus,'NOT_RUN');
});
test('Grade 7 optional visual has unique switch case labels', () => {
  const text=file('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
  for(const section of ['var altText =','var constructionPrompt =']) {
    const start=text.indexOf(section), end=text.indexOf('};',start);
    assert.ok(start>=0 && end>start);
    const cases=[...text.slice(start,end).matchAll(/"PED:US-CCSS-MATH:G7:U01:L\d\d"\s*=>/g)].map(x=>x[0]);
    assert.equal(cases.length,new Set(cases).size,section+' duplicate switch case');
  }
});
