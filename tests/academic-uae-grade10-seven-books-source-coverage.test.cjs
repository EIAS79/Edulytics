'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const coverage=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-toc-and-body-coverage.json'),'utf8'));
test('Main uploaded source family TOC units are counted once, without Applied double counting',()=>{
  const g=coverage.mainBookFamilies.find(x=>x.pathway==='General');
  const a=coverage.mainBookFamilies.find(x=>x.pathway==='Advanced');
  const apps=coverage.mainBookFamilies.filter(x=>x.pathway==='Applied');
  assert.equal(g.sourceTocLessonCount,48);
  assert.equal(a.sourceTocLessonCount,86);
  assert.equal(apps.length,2);
  assert.ok(apps.every(x=>x.sourceTocLessonCount===59));
  assert.equal(coverage.mainBookHeadingTotalBeforeDedup,193);
  assert.equal(coverage.actualCanonicalLessonCount,null,'do not certify preliminary 156');
});
test('Missing lesson-body volumes cannot silently be counted as academically audited',()=>{
  assert.ok(coverage.sourceGaps.some(x=>x.case==='ADVANCED_NEW_VOLUME2_MISSING'));
  assert.ok(coverage.sourceGaps.some(x=>x.case==='APPLIED_VOLUME1_MISSING'));
  assert.equal(coverage.sourceBodyAcademicCoveragePercent,null);
  assert.equal(coverage.deduplicationStatus,'GLOBAL_SOURCE_LOCK_PENDING');
  assert.ok(coverage.mainBookFamilies.every(x=>x.unitBodiesFullAcademicAudit===false));
});
