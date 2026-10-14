'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),dir='artifacts/academic-us-ccss/batches/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,dir+p),'utf8'));
const dom=read('B01-v9-original-teacher-figure-dom-asset-map-40.json');
const media=read('B01-v9-original-figure-383-media-byte-provenance.json');
const risks=read('B01-v9-original-figure-alt-risk-triage-397.json');
function verify(x){
 assert.equal(x.batchId,'B01');
 assert.equal(x.kind,'SOURCE_HTML_FIGURE_ALT_TEXT_RISK_REVIEW_ONLY');
 assert.equal(x.academicDecision,'BATCH_NO-GO');
 assert.equal(x.releaseEligible,false);
 assert.equal(x.coverage.batchLessons,500);
 assert.equal(x.coverage.publisherFigureLessons,40);
 assert.equal(x.coverage.publisherFigureFrames,397);
 assert.equal(x.coverage.uniquePublisherAssetUrls,383);
 assert.equal(x.coverage.zeroLengthHtmlAltFrames,30);
 assert.equal(x.coverage.nonemptyHtmlAltUnder20Characters,85);
 assert.equal(x.coverage.originalFramesNeedingManualDescriptionContextReview,115);
 assert.equal(x.coverage.sourceFigureLessonsWithRisks,30);
 assert.equal(x.coverage.sourceFigureLessonsWithZeroAlt,10);
 for(const prop of ['originalImageRightsCertified','figureMathematicalSemanticsCertified','studentL6BrowserVerified','academicLessonsCertifiedByThisAudit'])
   assert.equal(x.coverage[prop],0);
 assert.equal(x.accessibilityDecision,'SOURCE_DESCRIPTIVE_CONTEXT_UNVERIFIED_DO_NOT_GENERATE_ALT_AUTOMATICALLY');
 assert.equal(x.risks.length,30);
 const bySource=new Map(dom.rows.map(r=>[r.lessonCode,r]));
 const byAsset=new Map(media.assets.map(a=>[a.assetUrl,a]));
 const checked=new Set();let total=0,missing=0,short=0,affectedMissing=new Set();
 for(const item of x.risks){
  const original=bySource.get(item.lessonCode);assert(original);
  assert(!checked.has(item.lessonCode));
  checked.add(item.lessonCode);
  assert.equal(item.sourceSemanticSha256,original.sourceSemanticSha256);
  assert.equal(item.sourcePreparationInventorySha256,original.preparationLocalFileInventorySha256);
  assert.equal(item.originalLessonFrameCount,original.frames.length);
  assert.equal(item.affectedFrameCount,item.frames.length);
  assert.equal(item.mathAndAccessibilityL6Certified,false);
  const byOrdinal=new Map(original.frames.map(f=>[f.frameOrdinal,f]));
  for(const f of item.frames){
   const prior=byOrdinal.get(f.frameOrdinal);
   assert(prior&&prior.publisherRecordId===f.publisherRecordId);
   assert.equal(prior.altTextLength,f.sourceHtmlAltLength);
   assert.equal(prior.altTextSha256,f.altTextSha256);
   assert.equal(prior.mediaSrc,f.mediaUrl);
   assert.equal(prior.teacherActivitySection,f.sourceActivitySection);
   assert.equal(byAsset.get(f.mediaUrl)?.mediaByteSha256,f.mediaByteSha256);
   assert(f.sourceHtmlAltLength>=0&&f.sourceHtmlAltLength<20);
   if(f.sourceHtmlAltLength===0){
    missing++;
    affectedMissing.add(item.lessonCode);
    assert.equal(f.triage,'EMPTY_SOURCE_HTML_ALT');
   } else {
    short++;
    assert.equal(f.triage,'SHORT_SOURCE_HTML_ALT_LT_20_CHARS');
   }
   assert.equal(f.separateNearbyAccessibleCaptionChecked,false);
   assert.equal(f.svgSemanticTitleAriaChecked,false);
   assert.equal(f.studentAccessibleDescriptionVerified,false);
   total++;
  }
 }
 assert.equal(missing,30);assert.equal(short,85);
 assert.equal(total,115);assert.equal(affectedMissing.size,10);
 const rawCount=dom.rows.reduce((n,r)=>n+r.frames.length,0);
 assert.equal(rawCount,397);
 assert.equal(dom.rows.flatMap(r=>r.frames).filter(f=>f.altTextLength===0).length,missing);
 assert.equal(dom.rows.flatMap(r=>r.frames).filter(f=>f.altTextLength>0&&f.altTextLength<20).length,short);
 assert.equal(checked.size,30);
 return true;
}
test('397 original figure frames preserve exactly 30 empty alt and 85 short alt review flags without inventing descriptions',()=>assert(verify(risks)));
test('Negative gates reject missing frame, changed source, invented accessibility, math or rights pass',()=>{
 for(const mutate of [
  x=>x.risks[0].frames.pop(),
  x=>x.risks[0].frames[0].sourceHtmlAltLength=90,
  x=>x.risks[0].frames[0].mediaByteSha256='0'.repeat(64),
  x=>x.risks[0].sourceSemanticSha256='0'.repeat(64),
  x=>x.risks[0].mathAndAccessibilityL6Certified=true,
  x=>x.risks[0].frames[0].studentAccessibleDescriptionVerified=true,
  x=>x.coverage.originalImageRightsCertified=383,
  x=>x.coverage.figureMathematicalSemanticsCertified=397,
  x=>x.releaseEligible=true,
  x=>x.academicDecision='BATCH_CERTIFIED'
 ]){const bad=structuredClone(risks);mutate(bad);assert.throws(()=>verify(bad));}
});