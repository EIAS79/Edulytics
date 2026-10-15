#!/usr/bin/env node
'use strict';
// Independent visual mathematical comparison + rights + real student L6. Source linking stays in private audit metadata, never compulsory public UI credit.
// Structural evidence binding does not create academic proof or invoke approval.
const assert=require('node:assert/strict');
const sha=x=>typeof x==='string'&&/^[0-9a-f]{64}$/i.test(x);
const commit=x=>typeof x==='string'&&/^[0-9a-f]{40}$/i.test(x);
const eid=x=>typeof x==='string'&&/^[A-Za-z0-9][A-Za-z0-9:._/-]{5,200}$/.test(x);
const flow=['Open Lesson','Render','Start Practice','Answer','Grade','Feedback','Save Progress'];
function validate({domReport,activityMap,ledger,verification}) {
 assert.equal(domReport.batchId,'B01');
 assert.equal(domReport.releaseEligible,false);
 assert.equal(domReport.coverage.sourceFigureLessons,40);
 assert.equal(domReport.coverage.exactFrameToOriginalMediaDOMPointers,397);
 assert.equal(activityMap.rows.length,40);
 assert.equal(verification?.schemaVersion,1);
 assert.equal(verification.kind,'B01_V9_INDEPENDENT_FIGURE_MATH_RIGHTS_L6_EVIDENCE');
 assert.equal(verification.batchId,'B01');
 assert.ok(commit(verification.releaseCandidateSha));
 assert.equal(verification.releaseCandidateSha,ledger.releaseCandidateSha,'Wrong frozen product SHA');
 assert.equal(verification.records?.length,40,'Exactly forty complete academic visual records required');
 const canonical=new Map(ledger.records.map(x=>[x.lessonCode,x])),seen=new Set();
 let frames=0,licensed=0;
 for(let i=0;i<40;i++) {
  const source=activityMap.rows[i],proof=verification.records[i],dom=domReport.rows[i];
  assert.ok(source&&proof,'Missing original source frame or academic evidence');
  const lesson=canonical.get(source.lessonCode);
  assert.ok(lesson,'Missing canonical record');
  assert.equal(proof.lessonCode,source.lessonCode,'Reordered lesson evidence');
  assert.equal(dom.lessonCode,source.lessonCode,'DOM order mismatch');
  assert.ok(!seen.has(source.lessonCode),'Duplicate lesson evidence');
  seen.add(source.lessonCode);
  assert.equal(proof.sourceSemanticSha256,source.sourceSemanticSha256);
  assert.ok(sha(proof.effectiveContentSha256));
  assert.equal(proof.effectiveContentSha256,lesson.contentSha256,'Stale learner body');
  assert.ok(['EVIDENCE_READY','INTERNALLY_CERTIFIED'].includes(lesson.status),
    'Lesson does not meet its canonical academic evidence status');
  for(const g of ['L1','L4','L6']) {
   assert.equal(lesson.gates[g]?.status,'PASS','Missing canonical '+g);
   assert.equal(lesson.gates[g].sourceSha,ledger.releaseCandidateSha,'Stale '+g);
  }
  const linked=(g,e)=>assert.ok(eid(e)&&lesson.gates[g].evidenceIds.includes(e),
    'Missing linked evidence ID for '+g);
  assert.equal(proof.figures?.length,source.frames.length,
    'Every source figure requires an explicit comparison decision');
  for(let j=0;j<source.frames.length;j++) {
   const f=source.frames[j],p=proof.figures[j];
   assert.ok(p,'Figure evidence missing');
   assert.equal(p.publisherRecordId,f.publisherRecordId);
   assert.equal(p.sourceMediaUrl,f.mediaSrc);
   assert.equal(p.originalAltTextSha256,f.altTextSha256);
   assert.ok(sha(p.sourceMediaByteSha256),'Source image byte SHA absent');
   assert.equal(p.sourceBytesInspected,true,'Source image was not inspected');
   assert.equal(p.mathematicalEquivalence,'PASS','Mathematical comparison not proved');
   assert.equal(p.mathVerificationMethod,'SOURCE_VISUAL_AND_NUMERIC_INVARIANT');
   assert.ok(eid(p.renderedVisualId),'No actual student visual ID');
   linked('L4',p.mathEvidenceId); linked('L1',p.rightsEvidenceId);
   // Attribution belongs to the license of a copied media asset, not to
   // mathematical ideas. Neither publisher IDs nor source URLs are student UI.
   if(p.rightsBasis==='EDULYTICS_INDEPENDENT_NO_SOURCE_REUSE') {
    assert.equal(p.publisherAssetDisplayed,false,'Original source image cannot be silently reused');
    assert.equal(p.attributionRequired,false,'Independent visual has no publisher credit requirement');
   } else if(p.rightsBasis==='PUBLIC_DOMAIN_OR_UNPROTECTED_VERIFIED') {
    assert.equal(p.publisherAssetDisplayed,true);
    assert.equal(p.attributionRequired,false);
    assert.ok(sha(p.rightsDeterminationSha256),
      'No-restriction rights determination must have auditable original evidence');
    linked('L1',p.rightsDeterminationEvidenceId);
   } else if(p.rightsBasis==='LICENSED_PUBLISHER_ASSET_REUSE') {
    assert.equal(p.publisherAssetDisplayed,true);
    assert.equal(p.attributionRequired,true,
      'Copied CC BY assets require attribution; a silent copy is not licensed');
    linked('L1',p.licenseGrantEvidenceId);
    assert.ok(eid(p.attributionLocationEvidenceId),
      'A valid linked attribution/credits proof is required');
    linked('L1',p.attributionLocationEvidenceId);
    licensed++;
   } else assert.fail('Unresolved original-image rights classification');
   frames++;
  }
  const l=proof.learnerL6;
  assert.equal(l?.status,'PASS');
  assert.equal(l.candidateSha,ledger.releaseCandidateSha);
  assert.equal(l.effectiveContentSha256,lesson.contentSha256);
  assert.deepEqual(l.completedFlow,flow,'Incomplete browser Practice/grading flow');
  assert.ok(Number.isSafeInteger(l.githubRunId)&&l.githubRunId>0);
  assert.ok(sha(l.browserTraceSha256)&&sha(l.screenCaptureSha256));
  for(const e of [l.browserEvidenceId,l.accessibilityEvidenceId,l.gradingEvidenceId])
   linked('L6',e);
  assert.ok(Array.isArray(l.observedRenderedVisualIds));
  const rendered=new Set(l.observedRenderedVisualIds);
  for(const p of proof.figures)
   assert.ok(rendered.has(p.renderedVisualId),'Student L6 missing equivalent graphic');
 }
 assert.equal(frames,397);
 return {...domReport,independentlyVerifiedVisualMathematics:frames,
  copyrightReusableSourceFigureCount:licensed,
  authenticatedAccessibleProductionVisualsVerified:40,
  blockedLessonsForMathRightsAndL6:0,releaseEligible:true,
  academicDecision:'FIGURE_SCOPE_EVIDENCE_READY',
  note:'Requires independent audit of source images, math, rights and authenticated artifacts; global 500/500 collective gate remains separate.'};
}
module.exports={verifyFigureAcademicRelease:validate,exactFlow:flow};
