'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),base='artifacts/academic-us-ccss/batches/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,base+p),'utf8'));
const context=read('B01-v9-original-figure-115-alt-modal-context.json');
const risk=read('B01-v9-original-figure-alt-risk-triage-397.json');
const SHA=/^[a-f0-9]{64}$/;
function verify(c){
 assert.equal(c.batchId,'B01');
 assert.equal(c.academicDecision,'BATCH_NO-GO');
 assert.equal(c.releaseEligible,false);
 const s=c.scope;
 assert.equal(s.batchLessons,500);assert.equal(s.publisherSourceFigureLessons,40);assert.equal(s.figureFrames,397);
 assert.equal(s.sourceAltReviewCandidates,115);assert.equal(s.sourceLessonsWithCandidates,30);
 assert.equal(s.emptyOriginalImgAltCount,30);assert.equal(s.shortNonemptyOriginalImgAltCount,85);
 assert.equal(s.sourceModalLongDescriptionElementsPresent,115);
 assert.equal(s.sourceModalLongDescriptionElementsNonempty,1);
 assert.equal(s.originalEmptyAltWithNonemptyLongDescription,0);
 assert.equal(s.nontrivialOriginalModalCaptions,0);
 assert.equal(s.sourceModalCaptionsWithShortLabels,45);
 assert.equal(s.nontrivialVisibleFigcaption,0);
 for(const f of ['studentFacingEdulyticsActualAccessibleDescriptionsVerified','originalMathVisualSemanticsVerified','academicCertificationsGranted'])
  assert.equal(s[f],0);
 assert.equal(c.rows.length,115);
 const original=new Map(risk.risks.flatMap(l=>l.frames.map(f=>[l.lessonCode+':'+f.frameOrdinal,f])));
 let empty=0,nonempty=0,labels=0;
 const seen=new Set(),codes=new Set();
 for(const r of c.rows){
  const k=r.lessonCode+':'+r.frameOrdinal;
  assert(!seen.has(k));seen.add(k);codes.add(r.lessonCode);
  const a=original.get(k);
  assert(a);assert.equal(r.cmsRecordId,a.publisherRecordId);
  assert(SHA.test(r.sourcePageContentSHA256));
  assert.equal(r.modalLongDescriptionTagPresent,true);
  assert.equal(r.sourceAltEmpty,a.sourceHtmlAltLength===0);
  assert.equal(r.sourceAltUnder20,a.sourceHtmlAltLength<20);
  assert.equal(r.sourceHTMLProvidesIndependentCaptionDescription,false);
  assert.equal(r.actualEdulyticsStudentAccessibilityTreeNotChecked,true);
  assert.equal(r.originalMathGraphicsAccuracyNotCertified,true);
  assert.equal(r.sourceSVGTitleHasNotBeenProvenAccessible,true);
  if(r.sourceAltEmpty)empty++;
  if(r.modalLongDescriptionNonempty)nonempty++;
  if(r.modalCaptionOnlyShortLabel)labels++;
 }
 assert.equal(empty,30);assert.equal(nonempty,1);assert.equal(labels,45);
 assert.equal(codes.size,30);
 assert.equal(original.size,115);
 return true;
}
test('All 115 original source alt-context candidates are structurally inspected without inventing Edulytics L6 passes',()=>assert(verify(context)));
test('Cannot convert empty source descriptions into unsupported accessibility or academic PASS',()=>{
 for(const fn of [
  c=>c.releaseEligible=true,c=>c.academicDecision='BATCH_PASS',
  c=>c.scope.studentFacingEdulyticsActualAccessibleDescriptionsVerified=115,
  c=>c.scope.originalMathVisualSemanticsVerified=115,
  c=>c.scope.sourceModalLongDescriptionElementsNonempty=115,
  c=>c.rows[0].sourcePageContentSHA256='not-a-sha',
  c=>c.rows[0].actualEdulyticsStudentAccessibilityTreeNotChecked=false,
  c=>c.rows.pop()
 ]){
  const x=structuredClone(context);fn(x);assert.throws(()=>verify(x));
 }
});