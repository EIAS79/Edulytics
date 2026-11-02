'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {audit,roster}=require('../tools/us-ccss-orphan-child-source-review.cjs');
test('15 unresolved clauses: 7 have no parent publisher Addressing, 8 only parent sources',()=>{
 const p=audit();
 assert.equal(p.publicationDecision,'NO-GO');
 assert.equal(p.officialTotal,121);
 assert.equal(p.publisherChildExact,106);
 assert.equal(p.publisherChildUnresolved,15);
 assert.equal(p.sourceParentAddressingFound,8);
 assert.equal(p.sourceParentAddressingAbsent,7);
 assert.equal(p.candidateFamilies,15);
 assert.equal(p.independentReviewerSignoffs,0);
 assert.deepEqual(p.candidates.filter(x=>x.exactParentAddressingCount===0)
  .map(x=>x.clause),[
  'HSN-VM.B.4.a','HSN-VM.B.4.b','HSN-VM.B.4.c',
  'HSN-VM.B.5.a','HSN-VM.B.5.b','HSS-MD.B.5.a','HSS-MD.B.5.b']);
});
test('No implicit promotion from a shadow question family or publisher parent link',()=>{
 for(const c of audit().candidates){
  assert.equal(c.publisherExactChildAddressing,0);
  assert.equal(c.productionRoutingMetadata,false);
  assert.equal(c.reviewStatus,'PENDING_INDEPENDENT_MATH_AND_SOURCE_REVIEW');
  assert.equal(c.certified,false);
  assert.equal(c.humanReviewerId,null);
  assert.ok(c.officialText.length>25);
  if(c.exampleParentOnlyLesson)assert.ok(c.exampleParentOnlyLesson.sourceUrl.startsWith('https://'));
 }
});
test('Forgeable review metadata and fake child alignments are rejected',()=>{
 for(const field of ['academicApproved','sourceChildAddressing']){
  const forged=JSON.parse(JSON.stringify(roster));
  forged.candidates[0][field]=true;
  assert.throws(()=>audit(forged));
 }
 const signed=JSON.parse(JSON.stringify(roster));
 signed.candidates[0].reviewerId='invented';
 assert.throws(()=>audit(signed));
});
