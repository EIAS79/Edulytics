'use strict';
const test=require('node:test'), assert=require('node:assert/strict');
const fs=require('node:fs'),cp=require('node:child_process'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const evidence=require('../artifacts/academic-us-ccss/05-clause-publisher-reference-reconciliation.json');
test('PHASE 05: all 17 source blueprint packs yield 106/15 child references reproducibly', () => {
 const r=cp.spawnSync(process.execPath,
  [path.join(root,'tools/us-ccss-publisher-clause-reconciliation.cjs'),'--check'],
  {cwd:root,encoding:'utf8'});
 assert.equal(r.status,0,r.stderr||r.stdout);
 const s=evidence.summary;
 assert.deepEqual([s.clauses,s.publisherExactReferenced,s.publisherUnresolved,
  s.previousLegacyExactReferenced,s.legacyFalseNegativeRepairs],[121,106,15,82,24]);
 assert.equal(s.independentChildReviews,0);
 assert.equal(s.certifiedChildQuestionFamilies,0);
});
test('24 legacy false negatives were HS publisher letter-case differences, not new certifications', () => {
 assert.equal(evidence.legacyNewlyResolved.length,24);
 assert.ok(evidence.legacyNewlyResolved.every(c=>c.startsWith('HS')));
 assert.equal(evidence.clauses.length,121);
 assert.equal(new Set(evidence.clauses.map(x=>x.clause)).size,121);
 for(const code of ['HSA-REI.B.4.a','HSF-LE.A.1.c','HSS-ID.B.6.c']) {
  const row=evidence.clauses.find(x=>x.clause===code);
  assert.ok(row&&row.publisherExactReferences>0&&row.legacyExactReferences===0);
  assert.equal(row.independentMathReview,'PENDING');
 }
 assert.ok(evidence.unchangedPublisherUnknown.includes('HSN-VM.B.4.a'));
});
