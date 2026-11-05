'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {grade5EditionManifests,original2021StudentFacingLocator}=
  require('../tools/curriculum/g5-stage-d-diagnostic.cjs');

test('all 148 first-edition Kendall Hunt teacher lessons expose original 2021 Student Facing source blocks',()=>{
  const source=original2021StudentFacingLocator();
  const editions=grade5EditionManifests();
  assert.equal(source.size,148);
  assert.equal(editions.size,148);
  let sections=0;
  let assetOccurrences=0;
  for(const [code,row] of source){
    assert.ok(Number.isInteger(row.sections)&&row.sections>0,code);
    assert.ok(Number.isInteger(row.cmsAssetReferences)&&row.cmsAssetReferences>=0);
    assert.match(row.path,/im2021-student-facing-local-source-u0[1-8]\.json$/);
    const entries=editions.get(code);
    assert.ok(entries&&entries.length===2,code);
    assert.equal(entries.find(x=>x.role==='Teacher').editionMatch,
      'IM2021_TEACHER_CANDIDATE_SOURCE');
    assert.equal(entries.find(x=>x.role==='Student').editionMatch,
      'WRONG_EDITION_V360_STUDENT_MIRROR');
    sections+=row.sections;
    assetOccurrences+=row.cmsAssetReferences;
  }
  assert.equal(sections,483);
  assert.equal(assetOccurrences,1150);
  // Source-heading inventory is NOT equivalent to item-by-item exercise mapping.
  console.log('IM2021_STUDENT_FACING_LOCATED 148 lessons / 483 2021 teacher source blocks; 1150 referenced CMS asset occurrences; item parity and source rights PENDING');
});
