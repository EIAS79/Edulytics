'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const {ownerAcademicConsent}=require('../tools/curriculum/g5-stage-d-diagnostic.cjs');
const evidence=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/grade5-owner-academic-consent-20261101.json'),'utf8'));
const crypto=require('node:crypto');
const approvedBytes=fs.readFileSync(path.resolve(__dirname,'..',evidence.approvedContentPackPath));
const approvedGitBlob=crypto.createHash('sha1')
  .update('blob '+approvedBytes.length+'\0').update(approvedBytes).digest('hex');

test('owner authorizes all 148 already READY_FOR_STAGE_D Grade5 lessons with cited decision only',()=>{
  const c=ownerAcademicConsent();
  assert.equal(c.ownerDecision,'OWNER_ACADEMIC_CONSENT_GRANTED');
  assert.equal(c.ownerContentConsent,true);
  assert.equal(c.individualSourceParityCertified,false);
  assert.equal(evidence.scope.lessonCount,148);
  assert.equal(evidence.scope.requiredEntryState,'READY_FOR_STAGE_D');
  assert.equal(evidence.scope.grade,'G5');
  assert.equal(evidence.scope.stage,'D');
  assert.equal(evidence.authorizationClaims.ownerConsentsTo107IdentifiedTextChanges,true);
  assert.equal(evidence.approvedContentPackGitBlobSha,approvedGitBlob);
  assert.match(approvedGitBlob,/^[0-9a-f]{40}$/);
  assert.equal(evidence.publicDecisionEvidence,c.publicDecisionEvidence);
});
test('owner consent cannot fabricate individual technical checks, legal rights or release',()=>{
  const c=ownerAcademicConsent();
  assert.equal(c.ownerContentConsent,true);
  assert.equal(c.individualSourceParityCertified,false);
  assert.equal(c.thirdPartyRightsCertified,false);
  assert.equal(c.studentTeacherE2ECertified,false);
  assert.equal(c.releaseCertified,false);
  assert.equal(evidence.ownerSignedCryptographicDocument,false);
  assert.equal(evidence.certificationExclusions.grade6WithinScope,false);
  assert.ok(Object.values(evidence.certificationExclusions).every(v=>v===false));
});
